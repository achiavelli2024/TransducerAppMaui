using System.Globalization;
using TransducerAppMaui.Resources.Strings;

namespace TransducerAppMaui.Helpers;

/// <summary>
/// Aplica a cultura no app com base em LanguageSettings.
/// </summary>
public static class LocalizationService
{
    public static void ApplyCultureFromSettings()
    {
        try
        {
            // ✅ garante que temos a cultura real do sistema capturada
            SystemCulture.CaptureOnce();

            var selected = (LanguageSettings.Selected ?? LanguageSettings.Auto).Trim();

            CultureInfo culture;

            if (string.Equals(selected, LanguageSettings.PortugueseBrazil, StringComparison.OrdinalIgnoreCase))
            {
                culture = new CultureInfo("pt-BR");
            }
            else if (string.Equals(selected, LanguageSettings.English, StringComparison.OrdinalIgnoreCase))
            {
                culture = new CultureInfo("en");
            }
            else
            {
                // ✅ AUTO: usar cultura real do sistema capturada no startup
                var sys = SystemCulture.CapturedSystemUICulture ?? new CultureInfo("en");
                var name = (sys.Name ?? "en").ToLowerInvariant();

                culture = name.StartsWith("pt")
                    ? new CultureInfo("pt-BR")
                    : new CultureInfo("en");
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // ✅ MUITO IMPORTANTE: força o ResourceManager do .resx a usar a cultura aplicada
            AppResources.Culture = culture;
        }
        catch
        {
            var culture = new CultureInfo("en");

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            AppResources.Culture = culture;
        }
    }
}