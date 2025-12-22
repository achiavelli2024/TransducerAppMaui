using System.Globalization;

namespace TransducerAppMaui.Helpers;

/// <summary>
/// Aplica a cultura no app com base em LanguageSettings.
/// Nota: em MAUI, mudar cultura em runtime exige atualizar UI (recriar Shell) para refletir em todas as páginas.
/// </summary>
public static class LocalizationService
{
    public static void ApplyCultureFromSettings()
    {
        try
        {
            var selected = LanguageSettings.Selected;

            CultureInfo culture;

            if (string.Equals(selected, LanguageSettings.PortugueseBrazil, StringComparison.OrdinalIgnoreCase))
                culture = new CultureInfo("pt-BR");
            else if (string.Equals(selected, LanguageSettings.English, StringComparison.OrdinalIgnoreCase))
                culture = new CultureInfo("en");
            else
            {
                // Auto: tenta usar a cultura do sistema, mas com fallback para inglês
                culture = CultureInfo.CurrentUICulture ?? new CultureInfo("en");

                // Garantir que só aceitamos en* ou pt-BR por enquanto
                var name = culture.Name?.ToLowerInvariant() ?? "en";
                if (name.StartsWith("pt"))
                    culture = new CultureInfo("pt-BR");
                else
                    culture = new CultureInfo("en");
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            // Também ajusta CultureInfo.CurrentCulture/CurrentUICulture do thread atual
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
        catch
        {
            // fallback seguro
            var culture = new CultureInfo("en");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}