using TransducerAppMaui.Drivers;
using TransducerAppMaui.Models;

namespace TransducerAppMaui.Services;

public sealed class TransducerService : ITransducerService
{
    private PhoenixTransducer? _trans;
    private readonly SemaphoreSlim _lock = new(1, 1);

    // Cache de parâmetros (equivalente ao Xamarin)
    private bool _paramsLoaded;
    private FreeTestParameters _currentParameters = new();

    // === Rearm (equivalente ao Xamarin) ===
    private readonly SemaphoreSlim _rearmLock = new(1, 1);
    private readonly int _rearmCooldownMs = 800;

    private volatile bool _isTestRunning;
    public bool IsTestRunning => _isTestRunning;

    public FreeTestParameters CurrentParameters => _currentParameters;

    public void SetParameters(FreeTestParameters parameters)
    {
        if (parameters is null) throw new ArgumentNullException(nameof(parameters));
        parameters.Sanitize();

        _currentParameters = parameters;
        _paramsLoaded = true;
    }

    public bool IsConnected => _trans?.IsConnected ?? false;

    public event Action<bool>? ConnectionChanged;
    public event Action<DataResult>? LiveDataReceived;
    public event Action<int>? ErrorRaised;

    public async Task ConnectAsync(string ip, int port, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(ip))
            throw new ArgumentException("IP inválido.", nameof(ip));

        await _lock.WaitAsync(ct);
        try
        {
            await DisconnectInternalAsync().ConfigureAwait(false);

            _trans = new PhoenixTransducer
            {
                Eth_IP = ip.Trim(),
                Eth_Port = port
            };

            _trans.DataResult += OnDataResult;
            _trans.RaiseError += OnRaiseError;

            await Task.Run(() =>
            {
                _trans.StartService();
                Thread.Sleep(50);
                _trans.StartCommunication();
                _trans.RequestInformation();
            }, ct).ConfigureAwait(false);

            var ok = await WaitUntilConnectedAsync(ct).ConfigureAwait(false);
            ConnectionChanged?.Invoke(ok);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task DisconnectAsync()
    {
        await _lock.WaitAsync();
        try
        {
            await StopAcquisitionInternalAsync().ConfigureAwait(false);

            await DisconnectInternalAsync().ConfigureAwait(false);
            ConnectionChanged?.Invoke(false);

            // igual ResetCachedParams
            _paramsLoaded = false;
        }
        finally
        {
            _lock.Release();
        }
    }

    // ======================
    // ====== INIT READ ======
    // ======================
    public async Task StartAcquisitionAsync(bool firstStart, CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        try
        {
            if (_trans is null || !_trans.IsConnected)
                throw new InvalidOperationException("Transdutor não conectado.");

            if (!_paramsLoaded)
                throw new InvalidOperationException("Parâmetros não carregados. Chame SetParameters antes.");

            // Copia local para evitar race (se a UI atualizar enquanto estamos executando)
            var p = _currentParameters;
            p.Sanitize();

            // Diagnóstico: frames que seriam montados (igual Xamarin logava)
            // (não quebra se falhar)
            try
            {
                var frames = _trans.GetInitReadFrames();
                // futuramente: jogar isso num logger/console
                _ = frames?.Count;
            }
            catch { }

            // Execução em background (igual seu Task.Run no Xamarin)
            await Task.Run(() =>
            {
                try
                {
                    // A diferença do Xamarin:
                    // - no firstStart fazemos o "zero torque/angle"
                    if (firstStart)
                    {
                        _trans.SetZeroTorque();
                        Thread.Sleep(10);
                        _trans.SetZeroAngle();
                        Thread.Sleep(10);
                    }

                    // Click wrench config: mesmo valor do Xamarin
                    _trans.SetTestParameter_ClickWrench(85, 95, 20);
                    Thread.Sleep(10);

                    var thresholdEnd = p.ThresholdIni / 2M;
                    if (thresholdEnd < 0) thresholdEnd = 0;

                    const int timeStepMs = 5;

                    _trans.SetTestParameter(
                        new DataInformation(),
                        TesteType.TorqueOnly,
                        p.ToolType,
                        p.NomTorque,
                        p.ThresholdIni,
                        thresholdEnd,
                        p.TimeoutEndMs,
                        timeStepMs,
                        p.Frequency,
                        eDirection.CW);

                    Thread.Sleep(100);

                    _trans.StartReadData();

                    _isTestRunning = true;
                }
                catch
                {
                    _isTestRunning = false;
                    throw;
                }
            }, ct).ConfigureAwait(false);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task StopAcquisitionAsync()
    {
        await _lock.WaitAsync();
        try
        {
            await StopAcquisitionInternalAsync().ConfigureAwait(false);

            // Igual Xamarin: ao parar, marca params como “não carregados” para forçar reload
            // (se você quiser manter params para o próximo start, a gente muda isso depois)
            _paramsLoaded = false;
        }
        finally
        {
            _lock.Release();
        }
    }

    private Task StopAcquisitionInternalAsync()
    {
        try
        {
            if (_trans != null)
            {
                try { _trans.StopReadData(); } catch { }
            }
        }
        finally
        {
            _isTestRunning = false;
        }

        return Task.CompletedTask;
    }

    public async Task TryRearmAsync(CancellationToken ct = default)
    {
        // Igual ao Xamarin: tenta entrar no lock sem esperar
        var entered = await _rearmLock.WaitAsync(0, ct);
        if (!entered) return;

        try
        {
            await Task.Delay(_rearmCooldownMs, ct).ConfigureAwait(false);

            // second pass: se desconectou no meio, não faz nada
            if (_trans is null || !_trans.IsConnected)
                return;

            // Aqui é exatamente seu InitReadAsyncLoop
            await StartAcquisitionAsync(firstStart: false, ct).ConfigureAwait(false);
        }
        finally
        {
            try { _rearmLock.Release(); } catch { }
        }
    }

    // ======================
    // ===== Helpers ========
    // ======================
    private async Task DisconnectInternalAsync()
    {
        var t = _trans;
        if (t is null)
            return;

        try { t.DataResult -= OnDataResult; } catch { }
        try { t.RaiseError -= OnRaiseError; } catch { }

        _trans = null;

        await Task.Run(() =>
        {
            try { t.StopReadData(); } catch { }
            try { t.StopService(); } catch { }
        }).ConfigureAwait(false);
    }

    private async Task<bool> WaitUntilConnectedAsync(CancellationToken ct)
    {
        const int maxTries = 50;
        for (var i = 0; i < maxTries; i++)
        {
            ct.ThrowIfCancellationRequested();
            await Task.Delay(200, ct).ConfigureAwait(false);

            try
            {
                if (_trans?.IsConnected == true)
                    return true;
            }
            catch { }
        }
        return false;
    }

    private void OnDataResult(DataResult data)
    {
        try { LiveDataReceived?.Invoke(data); } catch { }
    }

    private void OnRaiseError(int err)
    {
        try { ErrorRaised?.Invoke(err); } catch { }
    }

    public void Dispose()
    {
        try { _lock.Wait(); } catch { }
        try { DisconnectInternalAsync().GetAwaiter().GetResult(); } catch { }
        try { _lock.Release(); } catch { }
        _lock.Dispose();
        _rearmLock.Dispose();
    }
}