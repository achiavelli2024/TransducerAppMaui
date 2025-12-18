using System;
using System.Text;

namespace TransducerAppMaui.Services.Logging;

/// <summary>
/// Logger central do APP (camada MAUI).
/// - Emite evento para UI (tempo real)
/// - Escreve em Debug/Console
/// - NÃO persiste ainda (isso entra na próxima etapa com SQLite)
///
/// Observação importante:
/// Como existe um método chamado "Debug(...)" nesta classe,
/// sempre usamos "System.Diagnostics.Debug.WriteLine" (fully qualified)
/// para evitar conflito de nomes.
/// </summary>
public sealed class AppLog : IAppLog
{
    public event Action<AppLogRecord>? OnLogAppended;

    public void Debug(string category, string message) => Append("DEBUG", category, message);
    public void Info(string category, string message) => Append("INFO", category, message);
    public void Warn(string category, string message) => Append("WARN", category, message);
    public void Error(string category, string message) => Append("ERROR", category, message);

    public void Exception(Exception ex, string category, string contextMessage)
    {
        if (ex == null) return;
        Append("ERROR", category, $"{contextMessage} | EX: {ex.GetType().Name}: {ex.Message}\n{ex}");
    }

    public void Raw(string category, string message, byte[]? rawBytes = null)
        => Append("RAW", category, message, rawBytes);

    private void Append(string level, string? category, string? message, byte[]? raw = null)
    {
        try
        {
            var rec = new AppLogRecord
            {
                TimestampUtc = DateTime.UtcNow,
                Level = level,
                Category = string.IsNullOrWhiteSpace(category) ? "APP" : category.Trim(),
                Message = message ?? "",
                RawHex = raw is { Length: > 0 } ? ToHex(raw) : null
            };

            var line = rec.ToString();

            // Debug output (Visual Studio / Logcat)
            System.Diagnostics.Debug.WriteLine(line);
            Console.WriteLine(line);

            // UI subscribers (não pode derrubar o app)
            try { OnLogAppended?.Invoke(rec); } catch { }
        }
        catch
        {
            // Nunca deixar logging derrubar o app
        }
    }

    private static string ToHex(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 3);
        foreach (var b in bytes)
            sb.AppendFormat("{0:X2} ", b);

        return sb.ToString().Trim();
    }
}