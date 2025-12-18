using TransducerAppMaui.Drivers;

namespace TransducerAppMaui.Services;

/// <summary>
/// Implementação concreta do serviço de transdutor.
/// Nesta etapa: foco em Connect/Disconnect e leitura de live data.
/// </summary>
public sealed class TransducerService : ITransducerService
{
    private PhoenixTransducer? _trans;
    private readonly SemaphoreSlim _lock = new(1, 1);

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
            // Se já existe instância, garante cleanup
            await DisconnectInternalAsync().ConfigureAwait(false);

            _trans = new PhoenixTransducer
            {
                Eth_IP = ip.Trim(),
                Eth_Port = port
            };

            // Assina eventos essenciais
            _trans.DataResult += OnDataResult;
            _trans.RaiseError += OnRaiseError;

            // Start do driver (equivalente ao Xamarin)
            await Task.Run(() =>
            {
                _trans.StartService();
                Thread.Sleep(50);
                _trans.StartCommunication();
                _trans.RequestInformation();
            }, ct).ConfigureAwait(false);

            // Poll simples para confirmar conexão (igual Xamarin fazia)
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
            await DisconnectInternalAsync().ConfigureAwait(false);
            ConnectionChanged?.Invoke(false);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task DisconnectInternalAsync()
    {
        var t = _trans;
        if (t is null)
            return;

        // desassina antes de parar (evita callback depois de dispose)
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
            catch
            {
                // ignora e continua tentando
            }
        }
        return false;
    }

    private void OnDataResult(DataResult data)
    {
        try
        {
            LiveDataReceived?.Invoke(data);
        }
        catch { }
    }

    private void OnRaiseError(int err)
    {
        try
        {
            ErrorRaised?.Invoke(err);
        }
        catch { }
    }

    public void Dispose()
    {
        try { _lock.Wait(); } catch { }
        try { DisconnectInternalAsync().GetAwaiter().GetResult(); } catch { }
        try { _lock.Release(); } catch { }
        _lock.Dispose();
    }
}