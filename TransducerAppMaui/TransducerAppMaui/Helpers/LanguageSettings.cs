using Microsoft.Maui.Storage;

namespace TransducerAppMaui.Helpers;

/// <summary>
/// Guarda preferência de idioma:
/// - "auto" => seguir idioma do sistema
/// - "en"   => forçar inglês
/// - "pt-BR"=> forçar português (Brasil)
/// </summary>
public static class LanguageSettings
{
    private const string KEY_LANGUAGE = "ui_language";
    public const string Auto = "auto";
    public const string English = "en";
    public const string PortugueseBrazil = "pt-BR";

    public static string Selected
    {
        get => Preferences.Get(KEY_LANGUAGE, Auto);
        set => Preferences.Set(KEY_LANGUAGE, value ?? Auto);
    }
}