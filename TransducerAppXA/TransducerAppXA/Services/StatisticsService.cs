using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;

namespace TransducerAppXA.Services
{
    /// <summary>
    /// Serviço singleton que disponibiliza a instância compartilhada de RealTimeStatistics
    /// e guarda a lista de amostras persistidas (para plot).
    /// Adicionado logs para depuração.
    /// </summary>
    public class StatisticsService
    {
        const string TAG = "StatisticsService";

        private static readonly StatisticsService _instance = new StatisticsService();
        public static StatisticsService Instance => _instance;

        public RealTimeStatistics Stats { get; private set; }

        readonly List<double> _samples = new List<double>();
        public IReadOnlyList<double> Samples => _samples.AsReadOnly();

        public string LastResultText { get; private set; }

        public event Action<RealTimeStatistics.StatsResult> OnStatisticsUpdated;
        public event Action<string> OnLastResultUpdated;
        public event Action<List<double>> OnSamplesChanged;

        public double? CurrentUSL { get; private set; }
        public double? CurrentLSL { get; private set; }
        public double? CurrentNominal { get; private set; }

        private StatisticsService() { }

        public void Initialize(int? windowSize = 200, double? defaultUSL = null, double? defaultLSL = null, RealTimeStatistics.StdDevType sigmaType = RealTimeStatistics.StdDevType.Population, int minSamples = 4)
        {
            if (Stats != null) return;

            Stats = new RealTimeStatistics(windowSize: windowSize, defaultUSL: defaultUSL, defaultLSL: defaultLSL);
            Stats.MinSamplesForStats = minSamples;
            Stats.SigmaType = sigmaType;

            Stats.OnStatisticsUpdated += (r) =>
            {
                try
                {
                    OnStatisticsUpdated?.Invoke(r);
                }
                catch { }
            };

            CurrentUSL = defaultUSL;
            CurrentLSL = defaultLSL;
            CurrentNominal = (defaultUSL.HasValue && defaultLSL.HasValue) ? ((defaultUSL.Value + defaultLSL.Value) / 2.0) : (double?)null;

            Log.Info(TAG, $"Initialized: windowSize={windowSize} minSamples={minSamples} defaultUSL={defaultUSL} defaultLSL={defaultLSL}");
            TransducerLogger.Log(message: $"StatisticsService initialized: windowSize={windowSize} minSamples={minSamples} defaultUSL={defaultUSL} defaultLSL={defaultLSL}");
        }

        public void SeedFromValues(IEnumerable<double> values, double? upperSpecLimit = null, double? lowerSpecLimit = null, double? nominal = null, string lastResultText = null)
        {
            try
            {
                Log.Info(TAG, $"SeedFromValues start: valuesCount={(values == null ? 0 : values.Count())} usl={upperSpecLimit} lsl={lowerSpecLimit} nominal={nominal}");
                TransducerLogger.Log(message: $"StatisticsService SeedFromValues start: valuesCount={(values == null ? 0 : values.Count())} usl={upperSpecLimit} lsl={lowerSpecLimit} nominal={nominal}");


                if (Stats == null) return;

                if (upperSpecLimit.HasValue) CurrentUSL = upperSpecLimit;
                if (lowerSpecLimit.HasValue) CurrentLSL = lowerSpecLimit;
                if (nominal.HasValue) CurrentNominal = nominal;

                Stats.Reset();
                lock (_samples) { _samples.Clear(); }

                foreach (var v in values ?? Enumerable.Empty<double>())
                {
                    try
                    {
                        Stats.AddSample(v, upperSpecLimit: CurrentUSL, lowerSpecLimit: CurrentLSL);
                        lock (_samples) { _samples.Add(v); }
                    }
                    catch (Exception ex)
                    {
                        Log.Warn(TAG, "SeedFromValues add sample error: " + ex.Message);
                    }
                }

                if (!string.IsNullOrEmpty(lastResultText))
                {
                    LastResultText = lastResultText;
                    try { OnLastResultUpdated?.Invoke(LastResultText); } catch { }
                }

                Log.Info(TAG, $"SeedFromValues finished: samplesCount={_samples.Count} LastResultText={(LastResultText ?? "null")}");
                TransducerLogger.Log(message: $"StatisticsService SeedFromValues: samplesCount={_samples.Count} LastResultText={(LastResultText ?? "null")}");

                try { OnSamplesChanged?.Invoke(new List<double>(_samples)); } catch { }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "SeedFromValues outer error: " + ex.Message);
            }
        }

        public void AddPersistedSample(double value, string lastResultText = null)
        {
            try
            {
                if (Stats == null) return;

                Log.Info(TAG, $"AddPersistedSample: value={value} lastText={(lastResultText ?? "null")}");
                TransducerLogger.Log(message: $"StatisticsService AddPersistedSample: value={value} lastText={(lastResultText ?? "null")}");

                Stats.AddSample(value, upperSpecLimit: CurrentUSL, lowerSpecLimit: CurrentLSL);

                lock (_samples) { _samples.Add(value); }

                Log.Info(TAG, $"AddPersistedSample -> samplesCount={_samples.Count}");
                TransducerLogger.Log(message: $"StatisticsService AddPersistedSample -> samplesCount={_samples.Count}");

                try { OnSamplesChanged?.Invoke(new List<double>(_samples)); } catch { }

                if (!string.IsNullOrEmpty(lastResultText))
                {
                    LastResultText = lastResultText;
                    try { OnLastResultUpdated?.Invoke(LastResultText); } catch { }
                    Log.Info(TAG, $"LastResultText updated: {LastResultText}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "AddPersistedSample error: " + ex.Message);
            }
        }

        public void Reset()
        {
            try
            {
                Log.Info(TAG, "Reset statistics and samples");
                TransducerLogger.Log(message: "StatisticsService Reset statistics and samples");

                // reseta estatísticas internas (se houver implementação própria)
                Stats?.Reset();

                // limpa samples thread-safe
                lock (_samples)
                {
                    _samples.Clear();
                }

                // limpa texto do último resultado
                LastResultText = null;
                try { OnLastResultUpdated?.Invoke(null); } catch { }

                // dispara SamplesChanged com lista vazia
                try { OnSamplesChanged?.Invoke(new List<double>(_samples)); } catch { }

                // --- NOVO: dispara OnStatisticsUpdated com estatísticas zeradas ---
                try
                {
                    var zeroStats = new RealTimeStatistics.StatsResult
                    {
                        Count = 0,
                        Mean = 0.0,
                        StdDev = 0.0,
                        Cm = 0.0,
                        Cmk = 0.0
                    };

                    OnStatisticsUpdated?.Invoke(zeroStats);
                    Log.Info(TAG, "Reset: OnStatisticsUpdated invoked with zeroStats");
                    TransducerLogger.Log(message: "StatisticsService Reset: OnStatisticsUpdated invoked with zeroStats");
                }
                catch (Exception ex)
                {
                    Log.Warn(TAG, "Reset: erro ao disparar OnStatisticsUpdated: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "Reset error: " + ex.Message);
            }
        }

        public List<double> GetSamplesSnapshot()
        {
            lock (_samples)
            {
                var copy = new List<double>(_samples);
                Log.Info(TAG, $"GetSamplesSnapshot -> count={copy.Count}");
                return copy;
            }
        }

        public void SetSpecLimits(double? usl, double? lsl, double? nominal = null)
        {
            try
            {
                CurrentUSL = usl;
                CurrentLSL = lsl;
                CurrentNominal = nominal ?? (usl.HasValue && lsl.HasValue ? ((usl.Value + lsl.Value) / 2.0) : (double?)null);
                Log.Info(TAG, $"SetSpecLimits -> USL={CurrentUSL} LSL={CurrentLSL} Nominal={CurrentNominal}");
                TransducerLogger.Log(message: $"StatisticsService SetSpecLimits -> USL={CurrentUSL} LSL={CurrentLSL} Nominal={CurrentNominal}");
            }
            catch (Exception ex)
            {
                Log.Error(TAG, "SetSpecLimits error: " + ex.Message);
            }
        }
    }
}