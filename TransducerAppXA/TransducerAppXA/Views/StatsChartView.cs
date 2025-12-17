using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace TransducerAppXA.Views
{
    public class StatsChartView : View
    {
        List<double> _samples = new List<double>();
        double? _usl = null;
        double? _lsl = null;
        double? _nominal = null;

        Paint axisPaint;
        Paint linePaint;
        Paint pointPaint;
        Paint labelPaint;

        // layout internals (ajuste visual aqui)
        readonly int pointSpacing = 120; // pixels entre amostras - se mudar aqui, GetScrollXForLastPage também usa
        readonly int pointRadius = 6;
        readonly int leftPadding = 24;
        readonly int rightPadding = 24;
        readonly int topPadding = 28;
        readonly int bottomPadding = 28;
        readonly int endExtraMargin = 140; // margem extra ao final

        // guarda a largura calculada do conteúdo (de acordo com os samples)
        int _contentWidth = 0;
        public int ContentWidth => _contentWidth;

        public StatsChartView(Context context) : base(context)
        {
            Init();
        }
        public StatsChartView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }
        void Init()
        {
            axisPaint = new Paint { Color = Color.ParseColor("#1976D2"), StrokeWidth = 2f, AntiAlias = true };
            linePaint = new Paint { Color = Color.ParseColor("#1565C0"), StrokeWidth = 2f, AntiAlias = true };
            pointPaint = new Paint { Color = Color.ParseColor("#0D47A1"), StrokeWidth = 3f, AntiAlias = true };
            pointPaint.SetStyle(Paint.Style.Fill);
            labelPaint = new Paint { Color = Color.ParseColor("#212121"), TextSize = 28f, AntiAlias = true };
        }

        /// <summary>
        /// Atualiza dados e recalcula largura necessária (para scroll).
        /// </summary>
        public void SetData(List<double> samples, double? usl, double? nominal, double? lsl)
        {
            _samples = (samples != null) ? new List<double>(samples) : new List<double>();
            _usl = usl;
            _lsl = lsl;
            _nominal = nominal;

            int count = Math.Max(1, _samples.Count);

            // Para 1 amostra não queremos aplicar espaçamento grande; o ponto fica à esquerda.
            // Quando houver várias amostras, aplicamos pointSpacing entre elas.
            int desiredWidth;
            if (count <= 1)
            {
                // largura mínima para 1 amostra: padding + pequeno espaço + margem final
                desiredWidth = leftPadding + rightPadding + endExtraMargin + 80;
            }
            else
            {
                // largura baseada na quantidade de pontos
                desiredWidth = leftPadding + rightPadding + (count - 1) * pointSpacing + endExtraMargin + 80;
            }

            _contentWidth = desiredWidth;

            try
            {
                var lp = LayoutParameters;
                if (lp == null) lp = new ViewGroup.LayoutParams(desiredWidth, ViewGroup.LayoutParams.MatchParent);
                lp.Width = desiredWidth;
                LayoutParameters = lp;
            }
            catch { }

            RequestLayout();
            Invalidate();
        }





        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int desiredHeight = 260 + topPadding + bottomPadding;

            int widthSize = MeasureSpec.GetSize(widthMeasureSpec);
            var heightMode = MeasureSpec.GetMode(heightMeasureSpec);
            int heightSize = MeasureSpec.GetSize(heightMeasureSpec);

            // Se já calculamos _contentWidth, use-a como largura exata do view
            int measuredWidth;
            if (_contentWidth > 0)
            {
                measuredWidth = _contentWidth;
            }
            else
            {
                measuredWidth = widthSize;
            }

            int height = desiredHeight;
            if (heightMode == MeasureSpecMode.Exactly) height = heightSize;
            else if (heightMode == MeasureSpecMode.AtMost) height = Math.Min(desiredHeight, heightSize);

            SetMeasuredDimension(measuredWidth, height);
        }


        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (canvas == null) return;

            float w = Width;
            float h = Height;
            if (h <= topPadding + bottomPadding) return;

            float x0 = leftPadding;
            float xEnd = w - rightPadding;
            float yTop = topPadding;
            float yBottom = h - bottomPadding;

            // calcula min/max incluindo LSL/USL e amostras (para permitir plot abaixo/above)
            double lsl = _lsl ?? double.NaN;
            double usl = _usl ?? double.NaN;
            double nominal = _nominal ?? double.NaN;

            double samplesMin = double.NaN, samplesMax = double.NaN;
            if (_samples != null && _samples.Count > 0)
            {
                samplesMin = double.MaxValue; samplesMax = double.MinValue;
                foreach (var v in _samples)
                {
                    if (v < samplesMin) samplesMin = v;
                    if (v > samplesMax) samplesMax = v;
                }
            }

            double minVal, maxVal;
            if (!double.IsNaN(usl) && !double.IsNaN(lsl))
            {
                minVal = double.IsNaN(samplesMin) ? lsl : Math.Min(lsl, samplesMin);
                maxVal = double.IsNaN(samplesMax) ? usl : Math.Max(usl, samplesMax);
                if (Math.Abs(maxVal - minVal) < 1e-6) { minVal -= 1.0; maxVal += 1.0; }
            }
            else if (!double.IsNaN(samplesMin))
            {
                minVal = samplesMin; maxVal = samplesMax;
                if (Math.Abs(maxVal - minVal) < 1e-6) { minVal -= 1.0; maxVal += 1.0; }
            }
            else if (!double.IsNaN(nominal))
            {
                minVal = nominal - 2.0; maxVal = nominal + 2.0;
            }
            else { minVal = 0.0; maxVal = 10.0; }

            Func<double, float> MapY = (val) =>
            {
                double t = (val - minVal) / (maxVal - minVal);
                return (float)(yBottom - t * (yBottom - yTop));
            };

            // Font metrics (para posicionar texto corretamente)
            var fm = labelPaint.GetFontMetrics();
            // fm.Ascent é negativo; fm.Descent é positivo.
            float textTopToBaseline = -fm.Ascent; // distância do topo da caixa até a baseline
            float textBottomToBaseline = fm.Descent; // distância da baseline até a parte inferior do texto

            // função auxiliar para calcular baseline razoável acima/na margem da linha
            float ComputeBaselineForLabel(float yLine, float aboveMargin = 4f)
            {
                // baseline tentativa para posicionar texto acima da linha, com margin
                float baselineCandidate = yLine - textBottomToBaseline - aboveMargin;

                // calculamos limites para que o texto não saia da área visível
                float minBaseline = yTop + textTopToBaseline + 2f; // garantir topo do texto >= yTop + 2
                float maxBaseline = yBottom - textBottomToBaseline - 2f; // garantir parte inferior do texto <= yBottom - 2

                if (baselineCandidate < minBaseline) baselineCandidate = minBaseline;
                if (baselineCandidate > maxBaseline) baselineCandidate = maxBaseline;
                return baselineCandidate;
            }

            // desenha linhas de spec (linha + rótulo) usando FontMetrics e clamp do baseline
            if (!double.IsNaN(usl))
            {
                float yUs = MapY(usl);
                axisPaint.Color = Color.ParseColor("#1976D2");
                canvas.DrawLine(x0, yUs, xEnd, yUs, axisPaint);

                float baseline = ComputeBaselineForLabel(yUs);
                canvas.DrawText($"USL ({usl:F2})", x0 + 4, baseline, labelPaint);
            }
            if (!double.IsNaN(nominal))
            {
                float yNom = MapY(nominal);
                axisPaint.Color = Color.ParseColor("#388E3C");
                canvas.DrawLine(x0, yNom, xEnd, yNom, axisPaint);

                float baseline = ComputeBaselineForLabel(yNom);
                canvas.DrawText($"Nominal ({nominal:F2})", x0 + 4, baseline, labelPaint);
            }
            if (!double.IsNaN(lsl))
            {
                float yLs = MapY(lsl);
                axisPaint.Color = Color.ParseColor("#1976D2");
                canvas.DrawLine(x0, yLs, xEnd, yLs, axisPaint);

                float baseline = ComputeBaselineForLabel(yLs);
                canvas.DrawText($"LSL ({lsl:F2})", x0 + 4, baseline, labelPaint);
            }

            if (_samples == null || _samples.Count == 0) return;

            var pts = new List<PointF>();
            for (int i = 0; i < _samples.Count; i++)
            {
                float x = x0 + i * pointSpacing;
                float y = MapY(_samples[i]);
                pts.Add(new PointF(x, y));
            }

            for (int i = 1; i < pts.Count; i++)
                canvas.DrawLine(pts[i - 1].X, pts[i - 1].Y, pts[i].X, pts[i].Y, linePaint);

            foreach (var p in pts)
                canvas.DrawCircle(p.X, p.Y, pointRadius, pointPaint);
        }






        /// <summary>
        /// Retorna a coordenada Y (em pixels, relativa ao topo do view) correspondente ao valor informado.
        /// Se a altura ainda não estiver disponível, retorna topPadding como fallback.
        /// </summary>
        public float GetYForValue(double value)
        {
            // se não há área válida, devolve topPadding
            if (Height <= topPadding + bottomPadding) return topPadding;

            // recalcula min/max tal como no OnDraw (para ficar consistente)
            double lsl = _lsl ?? double.NaN;
            double usl = _usl ?? double.NaN;
            double nominal = _nominal ?? double.NaN;

            double samplesMin = double.NaN, samplesMax = double.NaN;
            if (_samples != null && _samples.Count > 0)
            {
                samplesMin = double.MaxValue; samplesMax = double.MinValue;
                foreach (var v in _samples)
                {
                    if (v < samplesMin) samplesMin = v;
                    if (v > samplesMax) samplesMax = v;
                }
            }

            double minVal, maxVal;
            if (!double.IsNaN(usl) && !double.IsNaN(lsl))
            {
                minVal = double.IsNaN(samplesMin) ? lsl : Math.Min(lsl, samplesMin);
                maxVal = double.IsNaN(samplesMax) ? usl : Math.Max(usl, samplesMax);
                if (Math.Abs(maxVal - minVal) < 1e-6) { minVal -= 1.0; maxVal += 1.0; }
            }
            else if (!double.IsNaN(samplesMin))
            {
                minVal = samplesMin; maxVal = samplesMax;
                if (Math.Abs(maxVal - minVal) < 1e-6) { minVal -= 1.0; maxVal += 1.0; }
            }
            else if (!double.IsNaN(nominal))
            {
                minVal = nominal - 2.0; maxVal = nominal + 2.0;
            }
            else { minVal = 0.0; maxVal = 10.0; }

            // mapeamento Y (igual ao OnDraw)
            float yTop = topPadding;
            float yBottom = Height - bottomPadding;
            double t = (value - minVal) / (maxVal - minVal);
            if (double.IsInfinity(t) || double.IsNaN(t)) t = 0.0;
            float y = (float)(yBottom - t * (yBottom - yTop));
            return y;
        }

        /// <summary>
        /// Calcula o scroll X necessário para mostrar a "última página" de pontos
        /// (os últimos que cabem na largura hsvWidth). Retorna 0 se não precisa scroll.
        /// </summary>
        public int GetScrollXForLastPage(int hsvWidth)
        {
            try
            {
                // prefira usar o Width efetivamente medido do view quando disponível
                int contentW = _contentWidth;
                if (contentW <= 0) contentW = Width;

                if (hsvWidth <= 0) return 0;

                // se o view já foi medido, use sua largura (mais confiável)
                int effectiveContentW = (Width > 0) ? Width : contentW;

                // threshold para evitar rolagem por pequenas diferenças (padding/margem)
                int threshold = Math.Max(1, pointSpacing / 2);

                if (effectiveContentW <= hsvWidth + threshold) return 0;

                int usable = Math.Max(1, hsvWidth - leftPadding - rightPadding);
                int fit = Math.Max(1, usable / Math.Max(1, pointSpacing));
                int count = Math.Max(1, (_samples?.Count) ?? 1);

                int firstIndexToShow = Math.Max(0, count - fit);
                int scrollX = leftPadding + firstIndexToShow * pointSpacing;

                int maxScroll = Math.Max(0, effectiveContentW - hsvWidth);
                if (scrollX > maxScroll) scrollX = maxScroll;
                if (scrollX < 0) scrollX = 0;
                return scrollX;
            }
            catch
            {
                return 0;
            }
        }

    }
}