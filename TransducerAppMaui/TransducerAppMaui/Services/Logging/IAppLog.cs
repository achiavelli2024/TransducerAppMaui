using System;

namespace TransducerAppMaui.Services.Logging;

/// <summary>
/// Logger central do APP (camada MAUI).
/// - Parecido com TransducerLogAndroid do Xamarin, mas desacoplado.
/// - UI pode subscrever OnLogAppended para mostrar logs em tempo real.
/// - Na próxima etapa (SQLite), este logger vai persistir em BD.
/// </summary>
public interface IAppLog
{
    event Action<AppLogRecord>? OnLogAppended;

    void Debug(string category, string message);
    void Info(string category, string message);
    void Warn(string category, string message);
    void Error(string category, string message);
    void Exception(Exception ex, string category, string contextMessage);

    void Raw(string category, string message, byte[]? rawBytes = null);
}

public sealed class AppLogRecord
{
    public DateTime TimestampUtc { get; init; } = DateTime.UtcNow;
    public string Level { get; init; } = "INFO";
    public string Category { get; init; } = "APP";
    public string Message { get; init; } = "";
    public string? RawHex { get; init; }

    public override string ToString()
    {
        var t = TimestampUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
        var raw = string.IsNullOrWhiteSpace(RawHex) ? "" : $" | HEX: {RawHex}";
        return $"{t} - {Level} - [{Category}] {Message}{raw}";
    }
}