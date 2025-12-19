using Microsoft.Maui.Storage;

namespace TransducerAppMaui.Logs;

/// <summary>
/// Preferências simples para ligar/desligar coleta/persistência de logs.
/// Centraliza a chave e permite que o logger consulte sem acoplamento à UI.
/// </summary>
public static class LoggingSettings
{
    private const string KEY_ENABLED = "logging_enabled";

    /// <summary>
    /// Se false: logger não enfileira e não grava no DB (retorno imediato).
    /// Default: false (mais seguro para performance).
    /// </summary>
    public static bool Enabled
    {
        get => Preferences.Get(KEY_ENABLED, false);
        set => Preferences.Set(KEY_ENABLED, value);
    }
}