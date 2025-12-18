using System.Text;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;

namespace TransducerAppMaui.Services.Logging;

/// <summary>
/// Persiste logs do AppLog + ProtocolFileLogger no SQLite (DbHelper).
/// - Não pode quebrar o app se DB falhar.
/// - Foco: rastrear RX/TX e logs críticos.
/// </summary>
public sealed class LogPersistenceService : ILogPersistenceService
{
    private readonly IAppLog _appLog;
    private readonly DbHelper _db;

    private bool _started;

    public LogPersistenceService(IAppLog appLog, DbHelper db)
    {
        _appLog = appLog;
        _db = db;
    }

    public void Start()
    {
        if (_started) return;

        _appLog.OnLogAppended += OnAppLogAppended;

        // Captura RX/TX do protocolo (pacotes)
        ProtocolFileLogger.OnLogWritten += OnProtocolLogWritten;

        _started = true;

        _appLog.Info("DB", "LogPersistenceService started (AppLog + ProtocolFileLogger subscribed)");
    }

    public void Stop()
    {
        if (!_started) return;

        _appLog.OnLogAppended -= OnAppLogAppended;
        ProtocolFileLogger.OnLogWritten -= OnProtocolLogWritten;

        _started = false;

        _appLog.Info("DB", "LogPersistenceService stopped (unsubscribed)");
    }

    private void OnAppLogAppended(AppLogRecord rec)
    {
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
            // Nunca derrubar o app por falha de DB
        }
    }

    private void OnProtocolLogWritten(string direction, string text, byte[] raw)
    {
        try
        {
            // Monta uma linha bem parecida com seu padrão do Xamarin:
            // PROTO: [RX] texto | HEX: ...
            var sb = new StringBuilder();
            sb.Append("PROTO: ");
            sb.Append('[').Append(direction ?? "LOG").Append("] ");
            sb.Append(text ?? "");

            if (raw != null && raw.Length > 0)
            {
                sb.Append(" | HEX: ");
                sb.Append(ToHex(raw));
            }

            var line = sb.ToString();

            // 1) Também manda pro AppLog (pra UI ver ao vivo, se quiser)
            try { _appLog.Raw("PROTO", line, raw); } catch { }

            // 2) Persiste no DB
            _db.InsertLog(new LogEntry
            {
                TimestampUtc = DateTime.UtcNow,
                Message = line
            });
        }
        catch
        {
            // Nunca derrubar o app por falha de DB/log
        }
    }

    private static string ToHex(byte[] bytes)
    {
        if (bytes == null) return "";
        var sb = new StringBuilder(bytes.Length * 3);
        foreach (var b in bytes) sb.AppendFormat("{0:X2} ", b);
        return sb.ToString().Trim();
    }
}