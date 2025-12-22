using System.Globalization;

namespace TransducerAppMaui.Helpers;

/// <summary>
/// Guarda a cultura real do sistema capturada no início do app.
/// Isso é essencial para o modo AUTO, porque depois que você força um idioma,
/// CultureInfo.CurrentUICulture passa a refletir o override e não o sistema.
/// </summary>
public static class SystemCulture
{
    /// <summary>
    /// Cultura do sistema capturada no startup (antes de qualquer override).
    /// </summary>
    public static CultureInfo? CapturedSystemUICulture { get; private set; }

    public static void CaptureOnce()
    {
        if (CapturedSystemUICulture != null) return;

        try
        {
            CapturedSystemUICulture = CultureInfo.CurrentUICulture;
        }
        catch
        {
            CapturedSystemUICulture = new CultureInfo("en");
        }
    }
}