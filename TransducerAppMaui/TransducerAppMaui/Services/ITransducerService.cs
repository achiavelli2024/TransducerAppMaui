using TransducerAppMaui.Drivers;

namespace TransducerAppMaui.Services;

/// <summary>
/// Serviço responsável por gerenciar conexão e eventos do transdutor.
/// Encapsula o driver PhoenixTransducer e expõe um contrato simples para a UI.
/// </summary>
public interface ITransducerService : IDisposable
{
    bool IsConnected { get; }

    event Action<bool>? ConnectionChanged;
    event Action<DataResult>? LiveDataReceived;
    event Action<int>? ErrorRaised;

    Task ConnectAsync(string ip, int port, CancellationToken ct = default);
    Task DisconnectAsync();
}