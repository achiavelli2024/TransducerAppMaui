using TransducerAppMaui.Helpers;

namespace TransducerAppMaui.Services.Logging;

/// <summary>
/// Serviço responsável por persistir logs do AppLog no SQLite (DbHelper).
/// - NÃO mexe na UI
/// - Não deixa exceção de DB derrubar o app
/// </summary>
public sealed class LogPersistenceService : ILogPersistenceService
{
    private readonly IAppLog _log;
    private readonly DbHelper _db;

    private bool _started;

    public LogPersistenceService(IAppLog log, DbHelper db)
    {
        _log = log;
        _db = db;
    }

    public void Start()
    {
        if (_started) return;

        _log.OnLogAppended += OnLogAppended;
        _started = true;

        _log.Info("DB", "LogPersistenceService started (subscribed to AppLog)");
    }

    public void Stop()
    {
        if (!_started) return;

        _log.OnLogAppended -= OnLogAppended;
        _started = false;

        _log.Info("DB", "LogPersistenceService stopped (unsubscribed from AppLog)");
    }

    private void OnLogAppended(AppLogRecord rec)
    {
        // Persistência deve ser rápida e resiliente
        try
        {
            _db.InsertLog(new LogEntry
            {
                TimestampUtc = rec.TimestampUtc,
                Message = rec.ToString()
            });
        }
        catch
        {
            // Nunca deixar falha de DB derrubar o app.
            // (Se quiser, na próxima etapa podemos colocar fila + flush em batch)
        }
    }
}