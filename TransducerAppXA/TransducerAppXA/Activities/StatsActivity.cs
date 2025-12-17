using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using Android.Util;
using Android.Content.PM;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using TransducerAppXA.Services;
using TransducerAppXA.Views;
using static Android.Widget.TextView;

namespace TransducerAppXA.Activities
{
    [Activity(Label = "Statistics", Theme = "@style/AppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class StatsActivity : Activity
    {
        const string TAG = "StatsActivity";

        public static Action RequestClose;

        TextView tvMean;
        TextView tvStd;
        TextView tvCm;
        TextView tvCmk;
        TextView tvLastResult;
        Button btnClose;
        StatsChartView chartView;
        HorizontalScrollView hsvStats;

        const int SCROLL_RETRIES = 10;
        const int SCROLL_DELAY_MS = 120;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_stats);

            tvMean = FindViewById<TextView>(Resource.Id.tvStatsMean);
            tvStd = FindViewById<TextView>(Resource.Id.tvStatsStd);
            tvCm = FindViewById<TextView>(Resource.Id.tvStatsCm);
            tvCmk = FindViewById<TextView>(Resource.Id.tvStatsCmk);
            tvLastResult = FindViewById<TextView>(Resource.Id.tvLastResult);
            btnClose = FindViewById<Button>(Resource.Id.btnCloseStats);
            chartView = FindViewById<StatsChartView>(Resource.Id.statsChartView);
            hsvStats = FindViewById<HorizontalScrollView>(Resource.Id.hsvStats);

            if (chartView != null) chartView.LayoutChange += ChartView_LayoutChange;

            try
            {
                if (StatisticsService.Instance != null && StatisticsService.Instance.Stats != null)
                {
                    var cur = StatisticsService.Instance.Stats.GetCurrentStats();
                    if (cur.HasValue)
                    {
                        var r = cur.Value;
                        tvMean.Text = $"Mean: {r.Mean:F3}";
                        tvStd.Text = $"StdDev: {r.StdDev:F4}";
                        tvCm.Text = $"Cm: {(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")}";
                        tvCmk.Text = $"Cmk: {(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                    }
                    else
                    {
                        tvMean.Text = "Mean: n/a (need ≥ 3 saved results)";
                        tvStd.Text = "StdDev: n/a";
                        tvCm.Text = "Cm: n/a";
                        tvCmk.Text = "Cmk: n/a";
                    }

                    // usa o helper para aplicar cor dependendo do status
                    UpdateLastResultText(StatisticsService.Instance.LastResultText);

                    try
                    {
                        var samples = StatisticsService.Instance.GetSamplesSnapshot();
                        chartView?.SetData(samples, StatisticsService.Instance.CurrentUSL, StatisticsService.Instance.CurrentNominal, StatisticsService.Instance.CurrentLSL);
                        Log.Info(TAG, $"OnCreate: chart set with samples={samples.Count} contentWidth={chartView?.ContentWidth}");
                        TryScrollToEndSoon();
                        PositionLimitLabels();

                        // reforço: após layout
                        chartView?.Post(() =>
                        {
                            TryScrollToEndSoon();
                            PositionLimitLabels();
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(TAG, "Erro ao popular gráfico: " + ex.Message);
                    }

                    StatisticsService.Instance.OnStatisticsUpdated += StatsUpdatedHandler;
                    StatisticsService.Instance.OnLastResultUpdated += LastResultHandler;
                    StatisticsService.Instance.OnSamplesChanged += SamplesChangedHandler;
                }
                else
                {
                    tvMean.Text = "Mean: n/a (stats not initialized)";
                    tvStd.Text = "StdDev: n/a";
                    tvCm.Text = "Cm: n/a";
                    tvCmk.Text = "Cmk: n/a";
                    UpdateLastResultText(null);
                }
            }
            catch (Exception ex)
            {
                tvMean.Text = "Mean: error";
                Log.Info(TAG, "OnCreate error: " + ex.Message);
            }

            if (btnClose != null) btnClose.Click += (s, e) => { Finish(); };

            RequestClose = () =>
            {
                RunOnUiThread(() =>
                {
                    try { Finish(); } catch { }
                });
            };
        }

        void ChartView_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            Log.Info(TAG, $"LayoutChange: left={e.Left} right={e.Right} width={e.Right - e.Left} chart.ContentWidth={chartView?.ContentWidth}");
            TryScrollToEndSoon();

            // sempre reposicionar rótulos ao alterar layout do chart
            PositionLimitLabels();
        }

        // Atualiza o TextView do último resultado e colore o status OK/NOK
        void UpdateLastResultText(string lastResultText)
        {
            RunOnUiThread(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(lastResultText))
                    {
                        tvLastResult.SetTextColor(Color.ParseColor("#2E7D32")); // verde padrão para n/a (ou escolha outra)
                        tvLastResult.Text = "Último resultado: n/a";
                        return;
                    }

                    // Detecta token de status - procura [OK] ou [NOK]
                    bool containsOK = lastResultText.IndexOf("[OK]", StringComparison.OrdinalIgnoreCase) >= 0;
                    bool containsNOK = lastResultText.IndexOf("[NOK]", StringComparison.OrdinalIgnoreCase) >= 0;

                    // Cores (ajuste hex se preferir)
                    var green = Color.ParseColor("#2E7D32"); // tom verde similar usado no layout
                    var red = Color.ParseColor("#D32F2F");

                    // Se quiser colorir o texto inteiro:
                    if (containsOK)
                    {
                        // colorir o token e manter o texto legível
                        var ssb = new SpannableStringBuilder(lastResultText);
                        int start = lastResultText.IndexOf("[", StringComparison.Ordinal);
                        int end = lastResultText.IndexOf("]", start >= 0 ? start : 0);
                        if (start >= 0 && end > start)
                        {
                            ssb.SetSpan(new ForegroundColorSpan(green), start, end + 1, SpanTypes.ExclusiveExclusive);
                        }
                        // opcional: definir cor base do TextView também
                        tvLastResult.SetTextColor(green);
                        tvLastResult.SetText(ssb, BufferType.Spannable);
                    }
                    else if (containsNOK)
                    {
                        var ssb = new SpannableStringBuilder(lastResultText);
                        int start = lastResultText.IndexOf("[", StringComparison.Ordinal);
                        int end = lastResultText.IndexOf("]", start >= 0 ? start : 0);
                        if (start >= 0 && end > start)
                        {
                            ssb.SetSpan(new ForegroundColorSpan(red), start, end + 1, SpanTypes.ExclusiveExclusive);
                        }
                        tvLastResult.SetTextColor(red);
                        tvLastResult.SetText(ssb, BufferType.Spannable);
                    }
                    else
                    {
                        // sem token reconhecível: mantém cor padrão (verde neutro) e o texto completo
                        tvLastResult.SetTextColor(green);
                        tvLastResult.Text = lastResultText;
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn(TAG, "UpdateLastResultText error: " + ex.Message);
                    // fallback simples
                    try { tvLastResult.Text = !string.IsNullOrEmpty(lastResultText) ? lastResultText : "Último resultado: n/a"; } catch { }
                }
            });
        }

        // NOVO: método robusto para posicionar labels estáticos (USL/NOM/LSL) - mantido como antes
        void PositionLimitLabels()
        {
            try
            {
                var lblUsl = FindViewById<TextView>(Resource.Id.txtUSLLabel);
                var lblNom = FindViewById<TextView>(Resource.Id.txtNominalLabel);
                var lblLsl = FindViewById<TextView>(Resource.Id.txtLSLLabel);
                var labelsCol = FindViewById<FrameLayout>(Resource.Id.labelsColumn); // FrameLayout no XML

                if (chartView == null || labelsCol == null) return;

                chartView.Post(() =>
                {
                    try
                    {
                        int chartH = chartView.Height;
                        if (chartH <= 0)
                        {
                            labelsCol.PostDelayed(() => PositionLimitLabels(), 80);
                            return;
                        }

                        var parentLp = labelsCol.LayoutParameters;
                        if (parentLp != null && parentLp.Height != chartH)
                        {
                            parentLp.Height = chartH;
                            labelsCol.LayoutParameters = parentLp;
                        }

                        int EnsureMeasuredHeight(View v)
                        {
                            if (v == null) return 0;
                            if (v.Height > 0) return v.Height;
                            v.Measure(View.MeasureSpec.MakeMeasureSpec(labelsCol.Width, MeasureSpecMode.AtMost),
                                      View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                            return v.MeasuredHeight;
                        }

                        void PlaceLabel(TextView label, double? value)
                        {
                            if (label == null) return;

                            if (!value.HasValue || double.IsNaN(value.Value))
                            {
                                label.Visibility = ViewStates.Gone;
                                return;
                            }

                            label.Visibility = ViewStates.Visible;

                            float yChart = chartView.GetYForValue(value.Value);
                            int labelH = EnsureMeasuredHeight(label);
                            int top = (int)Math.Round(yChart - (labelH / 2.0f));
                            top = Math.Max(0, top);
                            top = Math.Min(chartH - labelH, top);

                            var flp = label.LayoutParameters as FrameLayout.LayoutParams;
                            if (flp == null) flp = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                            flp.TopMargin = top;
                            flp.LeftMargin = 0;
                            flp.Gravity = GravityFlags.Left | GravityFlags.Top;
                            label.LayoutParameters = flp;
                        }

                        double? uslVal = StatisticsService.Instance?.CurrentUSL;
                        double? nomVal = StatisticsService.Instance?.CurrentNominal;
                        double? lslVal = StatisticsService.Instance?.CurrentLSL;

                        PlaceLabel(lblUsl, uslVal);
                        PlaceLabel(lblNom, nomVal);
                        PlaceLabel(lblLsl, lslVal);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(TAG, "PositionLimitLabels inner error: " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "PositionLimitLabels error: " + ex.Message);
            }
        }

        void StatsUpdatedHandler(RealTimeStatistics.StatsResult r)
        {
            try
            {
                RunOnUiThread(async () =>
                {
                    try
                    {
                        tvMean.Text = $"Mean: {r.Mean:F3}";
                        tvStd.Text = $"StdDev: {r.StdDev:F4}";
                        tvCm.Text = $"Cm: {(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")}";
                        tvCmk.Text = $"Cmk: {(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";

                        var samples = StatisticsService.Instance.GetSamplesSnapshot();
                        chartView?.SetData(samples, StatisticsService.Instance.CurrentUSL, StatisticsService.Instance.CurrentNominal, StatisticsService.Instance.CurrentLSL);

                        Log.Info(TAG, $"StatsUpdatedHandler: samples={samples.Count} contentWidth={chartView?.ContentWidth}");

                        await TryScrollToEndWithRetriesAsync();

                        // reposiciona labels porque GetYForValue pode devolver diferente se escala do chart mudou
                        PositionLimitLabels();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(TAG, "StatsUpdatedHandler inner error: " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "StatsUpdatedHandler error: " + ex.Message);
            }
        }

        void SamplesChangedHandler(List<double> samples)
        {
            try
            {
                RunOnUiThread(async () =>
                {
                    try
                    {
                        chartView?.SetData(samples, StatisticsService.Instance.CurrentUSL, StatisticsService.Instance.CurrentNominal, StatisticsService.Instance.CurrentLSL);
                        Log.Info(TAG, $"SamplesChangedHandler: samples={samples.Count} contentWidth={chartView?.ContentWidth}");

                        await TryScrollToEndWithRetriesAsync();

                        // reposiciona labels após o chart crescer
                        PositionLimitLabels();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(TAG, "SamplesChangedHandler inner error: " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "SamplesChangedHandler error: " + ex.Message);
            }
        }

        void LastResultHandler(string text)
        {
            try
            {
                Log.Info(TAG, "LastResultHandler: " + (text ?? "null"));
                UpdateLastResultText(text);
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "LastResultHandler error: " + ex.Message);
            }
        }

        void TryScrollToEndSoon()
        {
            hsvStats?.PostDelayed(new Java.Lang.Runnable(() =>
            {
                try { ScrollToEndIfPossible(); }
                catch (Exception ex) { Log.Warn(TAG, "TryScrollToEndSoon error: " + ex.Message); }
            }), 120);
        }

        void ScrollToEndIfPossible()
        {
            try
            {
                if (chartView == null || hsvStats == null) return;

                int hsvW = hsvStats.Width;
                if (hsvW <= 0)
                {
                    Log.Info(TAG, "ScrollToEndIfPossible: hsvW<=0");
                    return;
                }

                int childWidth = chartView.Width;
                int contentWPlanned = chartView.ContentWidth;
                int effectiveContentW = childWidth > 0 ? childWidth : contentWPlanned;
                const int tolerance = 12;

                if (effectiveContentW <= hsvW + tolerance)
                {
                    Log.Info(TAG, $"Scroll ignorado: effectiveContentW={effectiveContentW} <= hsvW({hsvW}) + tol({tolerance})");
                    return;
                }

                int scrollX = chartView.GetScrollXForLastPage(hsvW);
                int childRight = chartView.Right;
                if (childRight > 0)
                {
                    int fallback = Math.Max(0, childRight - hsvW);
                    if (scrollX > fallback) scrollX = fallback;
                }
                else
                {
                    int maxScroll = Math.Max(0, contentWPlanned - hsvW);
                    if (scrollX > maxScroll) scrollX = maxScroll;
                }

                Log.Info(TAG, $"ScrollToEndIfPossible: hsvW={hsvW} effectiveContentW={effectiveContentW} computedScrollX={scrollX} childRight={childRight} contentWPlanned={contentWPlanned}");

                if (scrollX > 0)
                    hsvStats.SmoothScrollTo(scrollX, 0);
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "ScrollToEndIfPossible error: " + ex.Message);
            }
        }

        async Task TryScrollToEndWithRetriesAsync()
        {
            try
            {
                for (int i = 0; i < SCROLL_RETRIES; i++)
                {
                    await Task.Delay(SCROLL_DELAY_MS).ConfigureAwait(false);
                    RunOnUiThread(() =>
                    {
                        try { ScrollToEndIfPossible(); }
                        catch (Exception ex) { Log.Warn(TAG, "Retry scroll error: " + ex.Message); }
                    });
                }

                RunOnUiThread(() =>
                {
                    try
                    {
                        if (chartView != null && hsvStats != null)
                        {
                            int childRight = chartView.Right;
                            int hsvW = hsvStats.Width;
                            int scrollX = Math.Max(0, childRight - hsvW);
                            Log.Info(TAG, $"TryScrollToEndWithRetriesAsync final fallback: childRight={childRight} hsvW={hsvW} fallbackScrollX={scrollX}");

                            if (childRight > hsvW && hsvW > 0) hsvStats.ScrollTo(scrollX, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(TAG, "Final fallback scroll error: " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "TryScrollToEndWithRetriesAsync error: " + ex.Message);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                if (chartView != null) chartView.LayoutChange -= ChartView_LayoutChange;

                if (StatisticsService.Instance != null)
                {
                    StatisticsService.Instance.OnStatisticsUpdated -= StatsUpdatedHandler;
                    StatisticsService.Instance.OnLastResultUpdated -= LastResultHandler;
                    StatisticsService.Instance.OnSamplesChanged -= SamplesChangedHandler;
                }
            }
            catch (Exception ex)
            {
                Log.Warn(TAG, "OnDestroy error: " + ex.Message);
            }

            RequestClose = null;
            try { SetResult(Result.Ok); } catch { }
        }
    }
}