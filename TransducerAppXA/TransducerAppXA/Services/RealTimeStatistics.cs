using System;

namespace TransducerAppXA.Services
{
    /// <summary>
    /// Mantém amostras (janela deslizante ou acumulativa) e calcula:
    ///  - média
    ///  - desvio padrão (populacional ou amostral)
    ///  - Cm e Cmk (quando USL e LSL são fornecidos)
    /// Dispara evento OnStatisticsUpdated sempre que há atualização.
    /// Versão compatível com compiladores C# mais antigos (sem init-only setters).
    /// </summary>
    public class RealTimeStatistics
    {
        private readonly object _lock = new object();
        private readonly double[] _buffer;
        private readonly bool _useBuffer;
        private int _index = 0;
        private int _count = 0;
        private double _sum = 0.0;
        private double _sumSquares = 0.0;

        public enum StdDevType
        {
            Population, // divisor N
            Sample      // divisor N-1
        }

        /// <summary>
        /// Tamanho da janela. Se null, usa acúmulo infinito (todas as amostras desde o reset).
        /// </summary>
        public int? WindowSize { get; }

        /// <summary>
        /// Tipo de cálculo do desvio padrão.
        /// </summary>
        public StdDevType SigmaType { get; set; }

        /// <summary>
        /// Número mínimo de amostras necessárias para emitir estatísticas.
        /// </summary>
        public int MinSamplesForStats { get; set; }

        /// <summary>
        /// Valores de especificação padrão (opcionais). Podem também ser passados por AddSample.
        /// </summary>
        public double? DefaultUpperSpecLimit { get; set; }
        public double? DefaultLowerSpecLimit { get; set; }

        public event Action<StatsResult> OnStatisticsUpdated;

        public RealTimeStatistics(int? windowSize = 200, double? defaultUSL = null, double? defaultLSL = null)
        {
            if (windowSize.HasValue && windowSize <= 0) throw new ArgumentException("windowSize must be > 0 or null");
            WindowSize = windowSize;
            if (WindowSize.HasValue)
            {
                _useBuffer = true;
                _buffer = new double[WindowSize.Value];
            }
            else
            {
                _useBuffer = false;
                _buffer = new double[0]; // buffer não usado
            }
            DefaultUpperSpecLimit = defaultUSL;
            DefaultLowerSpecLimit = defaultLSL;

            SigmaType = StdDevType.Population;
            MinSamplesForStats = 4;
        }

        public struct StatsResult
        {
            public int Count { get; set; }
            public double Mean { get; set; }
            public double StdDev { get; set; }
            public double? Cm { get; set; }
            public double? Cmk { get; set; }
            public double? USL { get; set; }
            public double? LSL { get; set; }
            public DateTime TimestampUtc { get; set; }
        }

        /// <summary>
        /// Adiciona uma amostra e atualiza as estatísticas.
        /// Opcionalmente passe USL e LSL; caso não passe, a instância usará os DefaultUpperSpecLimit/DefaultLowerSpecLimit.
        /// </summary>
        public void AddSample(double value, double? upperSpecLimit = null, double? lowerSpecLimit = null)
        {
            StatsResult? result = null;

            lock (_lock)
            {
                if (!_useBuffer)
                {
                    // acumulativo
                    _sum += value;
                    _sumSquares += value * value;
                    _count++;
                }
                else
                {
                    // janela deslizante circular
                    if (_count < WindowSize.Value)
                    {
                        _buffer[_index] = value;
                        _sum += value;
                        _sumSquares += value * value;
                        _index = (_index + 1) % WindowSize.Value;
                        _count++;
                    }
                    else
                    {
                        var old = _buffer[_index];
                        _sum -= old;
                        _sumSquares -= old * old;

                        _buffer[_index] = value;
                        _sum += value;
                        _sumSquares += value * value;
                        _index = (_index + 1) % WindowSize.Value;
                    }
                }

                if (_count >= MinSamplesForStats)
                {
                    double mean = _sum / _count;

                    // Variância: usando soma de quadrados (correção numérica simples)
                    double variance;
                    if (SigmaType == StdDevType.Population)
                    {
                        variance = (_sumSquares / _count) - (mean * mean);
                    }
                    else
                    {
                        // amostral (N-1)
                        if (_count <= 1) variance = 0.0;
                        else
                        {
                            // var = (sumSquares - N * mean^2) / (N-1)
                            variance = (_sumSquares - _count * mean * mean) / (_count - 1);
                        }
                    }

                    if (variance < 0 && variance > -1e-12) variance = 0;
                    if (variance < 0) variance = 0;
                    double stdDev = Math.Sqrt(variance);

                    double? cm = null;
                    double? cmk = null;

                    double? usl = upperSpecLimit ?? DefaultUpperSpecLimit;
                    double? lsl = lowerSpecLimit ?? DefaultLowerSpecLimit;

                    if (stdDev > 0 && usl.HasValue && lsl.HasValue)
                    {
                        cm = (usl.Value - lsl.Value) / (6.0 * stdDev);

                        double v1 = (usl.Value - mean) / (3.0 * stdDev);
                        double v2 = (mean - lsl.Value) / (3.0 * stdDev);
                        cmk = Math.Min(v1, v2);
                    }

                    result = new StatsResult
                    {
                        Count = _count,
                        Mean = mean,
                        StdDev = stdDev,
                        Cm = cm,
                        Cmk = cmk,
                        USL = usl,
                        LSL = lsl,
                        TimestampUtc = DateTime.UtcNow
                    };
                }
            }

            if (result.HasValue)
            {
                var r = result.Value;
                var handler = OnStatisticsUpdated;
                if (handler != null) handler(r);
            }
        }

        /// <summary>
        /// Retorna as estatísticas atuais (ou null se não houver amostras suficientes).
        /// </summary>
        public StatsResult? GetCurrentStats(double? upperSpecLimit = null, double? lowerSpecLimit = null)
        {
            lock (_lock)
            {
                if (_count < MinSamplesForStats) return null;

                double mean = _sum / _count;
                double variance;
                if (SigmaType == StdDevType.Population)
                {
                    variance = (_sumSquares / _count) - (mean * mean);
                }
                else
                {
                    if (_count <= 1) variance = 0.0;
                    else variance = (_sumSquares - _count * mean * mean) / (_count - 1);
                }

                if (variance < 0 && variance > -1e-12) variance = 0;
                if (variance < 0) variance = 0;
                double stdDev = Math.Sqrt(variance);

                double? cm = null;
                double? cmk = null;

                double? usl = upperSpecLimit ?? DefaultUpperSpecLimit;
                double? lsl = lowerSpecLimit ?? DefaultLowerSpecLimit;

                if (stdDev > 0 && usl.HasValue && lsl.HasValue)
                {
                    cm = (usl.Value - lsl.Value) / (6.0 * stdDev);
                    double v1 = (usl.Value - mean) / (3.0 * stdDev);
                    double v2 = (mean - lsl.Value) / (3.0 * stdDev);
                    cmk = Math.Min(v1, v2);
                }

                return new StatsResult
                {
                    Count = _count,
                    Mean = mean,
                    StdDev = stdDev,
                    Cm = cm,
                    Cmk = cmk,
                    USL = usl,
                    LSL = lsl,
                    TimestampUtc = DateTime.UtcNow
                };
            }
        }

        /// <summary>
        /// Reseta todas as estatísticas (limpa buffer).
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                _index = 0;
                _count = 0;
                _sum = 0;
                _sumSquares = 0;
                if (_useBuffer) Array.Clear(_buffer, 0, _buffer.Length);
            }
        }
    }
}