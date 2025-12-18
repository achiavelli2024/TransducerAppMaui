namespace TransducerAppMaui.Services.Logging;

/// <summary>
/// Serviço responsável por persistir logs no SQLite.
/// Separa "captura de log" (App/Protocolo) de "UI".
/// </summary>
public interface ILogPersistenceService
{
    void Start();
    void Stop();
}