using TransducerAppMaui.Drivers;
using TransducerAppMaui.Models;

namespace TransducerAppMaui.Services;

public interface ITransducerService : IDisposable
{
    bool IsConnected { get; }

    event Action<bool>? ConnectionChanged;
    event Action<DataResult>? LiveDataReceived;
    event Action<int>? ErrorRaised;

    FreeTestParameters CurrentParameters { get; }
    void SetParameters(FreeTestParameters parameters);

    // === NOVO: aquisição (InitRead / Stop / Loop) ===
    bool IsTestRunning { get; }

    /// <summary>
    /// firstStart=true  => InitReadAsync do Xamarin (zera torque/ângulo e inicia)
    /// firstStart=false => InitReadAsyncLoop do Xamarin (rearma com os mesmos parâmetros)
    /// </summary>
    Task StartAcquisitionAsync(bool firstStart, CancellationToken ct = default);

    Task StopAcquisitionAsync();

    /// <summary>
    /// Tenta rearmar a aquisição com debounce e lock (igual Xamarin).
    /// </summary>
    Task TryRearmAsync(CancellationToken ct = default);

    Task ConnectAsync(string ip, int port, CancellationToken ct = default);
    Task DisconnectAsync();
}