using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Storage;

namespace TransducerAppMaui.Services;

/// <summary>
/// Implementa o mesmo algoritmo de licença do Xamarin:
/// - KeyCode: MD5(deviceId) -> primeiros 5 chars
/// - License: transformação do KeyCode (reverse + shift)
/// - Persistência: Preferences (MAUI)
/// </summary>
public sealed class LicenseService
{
    private const string PrefsKeyLicense = "LicenseCode";
    private const int LicenseLength = 5;

    private readonly IDeviceIdProvider _deviceIdProvider;

    public LicenseService(IDeviceIdProvider deviceIdProvider)
    {
        _deviceIdProvider = deviceIdProvider;
    }

    public string GetKeyCode()
    {
        try
        {
            var source = _deviceIdProvider.GetDeviceId() ?? "UNKNOWN_DEVICE";

            using var md5 = MD5.Create();
            var data = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

            var sb = new StringBuilder();
            foreach (var b in data)
                sb.Append(b.ToString("X2"));

            var fullHash = sb.ToString();
            if (fullHash.Length >= LicenseLength)
                return fullHash.Substring(0, LicenseLength).ToUpperInvariant();

            return fullHash.ToUpperInvariant().PadRight(LicenseLength, 'X');
        }
        catch
        {
            return "ABCDE";
        }
    }

    public bool IsLicenseValid()
    {
        var stored = Preferences.Get(PrefsKeyLicense, string.Empty);
        if (string.IsNullOrWhiteSpace(stored))
            return false;

        var keyCode = GetKeyCode();
        return ValidateLicense(keyCode, stored);
    }

    public bool ValidateLicense(string keyCode, string license)
    {
        if (string.IsNullOrWhiteSpace(license))
            return false;

        var expected = GenerateLicenseFromKey(keyCode);
        return string.Equals(expected, license.Trim().ToUpperInvariant(), StringComparison.Ordinal);
    }

    public void SaveLicense(string license)
    {
        if (string.IsNullOrWhiteSpace(license))
            return;

        Preferences.Set(PrefsKeyLicense, license.Trim().ToUpperInvariant());
    }

    public string GenerateLicenseFromKey(string keyCode)
    {
        if (string.IsNullOrEmpty(keyCode))
            keyCode = "ABCDE";

        keyCode = keyCode.Trim().ToUpperInvariant();
        if (keyCode.Length > LicenseLength)
            keyCode = keyCode.Substring(0, LicenseLength);

        var chars = keyCode.ToCharArray();
        Array.Reverse(chars);

        for (int i = 0; i < chars.Length; i++)
        {
            var ch = chars[i];

            if (ch >= '0' && ch <= '8')
                ch = (char)(ch + 1);
            else if (ch == '9')
                ch = 'A';
            else if (ch >= 'A' && ch <= 'Y')
                ch = (char)(ch + 1);
            else if (ch == 'Z')
                ch = '0';
            else
                ch = '0';

            chars[i] = ch;
        }

        var license = new string(chars).ToUpperInvariant();
        if (license.Length > LicenseLength)
            license = license.Substring(0, LicenseLength);

        return license;
    }

    /// <summary>
    /// Garante a licença: se inválida, pede ao usuário e (se recusar) fecha o app.
    /// Esta parte envolve UI, então recebe um Page para mostrar dialogs.
    /// </summary>
    public async Task EnsureLicenseAsync(Page page)
    {
        // Se já válida, não faz nada
        if (IsLicenseValid())
            return;

        var keyCode = GetKeyCode();

        while (true)
        {
            // DisplayPromptAsync substitui o AlertDialog com EditText
            var entered = await page.DisplayPromptAsync(
                "Licença necessária",
                $"Este dispositivo ainda não possui licença.\n\nKeyCode deste aparelho: {keyCode}\n\nInforme a licença (5 caracteres):",
                accept: "VALIDAR",
                cancel: "SAIR",
                placeholder: "ABCDE",
                maxLength: LicenseLength,
                keyboard: Keyboard.Text);

            // Cancelou: sair do app (equivalente ao FinishAffinity)
            if (entered is null)
            {
#if ANDROID
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#else
                // Em outras plataformas, encerramento pode variar.
                // Mantemos o comportamento: fechar janela atual.
                Application.Current?.Quit();
#endif
                return;
            }

            entered = entered.Trim().ToUpperInvariant();

            if (entered.Length != LicenseLength || !ValidateLicense(keyCode, entered))
            {
                await page.DisplayAlert("Erro", "Licença inválida. Tente novamente.", "OK");
                continue;
            }

            SaveLicense(entered);
            await page.DisplayAlert("OK", "Licença aceita.", "OK");
            return;
        }
    }


#if DEBUG
    /// <summary>
    /// DEBUG ONLY: limpa a licença armazenada para forçar o prompt aparecer novamente.
    /// </summary>
    public void ResetLicenseForDebug()
    {
        try
        {
            Preferences.Remove(PrefsKeyLicense);
        }
        catch { }
    }
#endif






}