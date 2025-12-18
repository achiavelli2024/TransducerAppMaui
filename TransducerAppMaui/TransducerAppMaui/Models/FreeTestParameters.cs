using System.Globalization;
using TransducerAppMaui.Drivers;

namespace TransducerAppMaui.Models;

/// <summary>
/// Parâmetros do teste "Free Test" (equivalente aos campos da tela no Xamarin).
/// Mantemos tipagem forte para evitar string solta.
/// </summary>
public sealed class FreeTestParameters
{
    public const decimal DEFAULT_THRESHOLD_INI = 3M;
    public const int DEFAULT_TIMEOUT_END = 500;
    public const decimal DEFAULT_MIN_TORQUE = 8M;
    public const decimal DEFAULT_NOM_TORQUE = 10M;
    public const decimal DEFAULT_MAX_TORQUE = 12M;
    public const int DEFAULT_FREQ = 2000;

    public string Ip { get; set; } = "192.168.4.1";
    public decimal ThresholdIni { get; set; } = DEFAULT_THRESHOLD_INI;
    public int TimeoutEndMs { get; set; } = DEFAULT_TIMEOUT_END;
    public decimal MinTorque { get; set; } = DEFAULT_MIN_TORQUE;
    public decimal NomTorque { get; set; } = DEFAULT_NOM_TORQUE;
    public decimal MaxTorque { get; set; } = DEFAULT_MAX_TORQUE;
    public int Frequency { get; set; } = DEFAULT_FREQ;

    public ToolType ToolType { get; set; } = ToolType.ToolType1;

    public decimal ThresholdEnd => ThresholdIni / 2M;

    /// <summary>
    /// Sanitiza valores para evitar parâmetros inválidos.
    /// Segue a lógica do Xamarin (com ranges básicos).
    /// </summary>
    public void Sanitize()
    {
        if (ThresholdIni < 0) ThresholdIni = DEFAULT_THRESHOLD_INI;

        if (TimeoutEndMs < 10 || TimeoutEndMs > 10000)
            TimeoutEndMs = DEFAULT_TIMEOUT_END;

        if (MinTorque <= 0) MinTorque = DEFAULT_MIN_TORQUE;
        if (NomTorque <= 0) NomTorque = DEFAULT_NOM_TORQUE;
        if (MaxTorque <= 0) MaxTorque = DEFAULT_MAX_TORQUE;

        if (Frequency <= 0) Frequency = DEFAULT_FREQ;

        if (string.IsNullOrWhiteSpace(Ip))
            Ip = "192.168.4.1";
    }

    public static decimal ParseDecimalOrDefault(string? text, decimal fallback)
    {
        var s = (text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(s)) return fallback;

        if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var d))
            return d;

        if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out d))
            return d;

        return fallback;
    }

    public static int ParseIntOrDefault(string? text, int fallback)
    {
        var s = (text ?? "").Trim();
        if (string.IsNullOrWhiteSpace(s)) return fallback;

        return int.TryParse(s, out var i) ? i : fallback;
    }
}