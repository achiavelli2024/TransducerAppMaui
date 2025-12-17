using TransducerAppMaui.Services;

namespace TransducerAppMaui.Views;

public partial class LicensePage : ContentPage
{
    private readonly LicenseService _licenseService;
    private readonly string _keyCode;

    public LicensePage(LicenseService licenseService)
    {
        InitializeComponent();

        _licenseService = licenseService;

        _keyCode = _licenseService.GetKeyCode();
        KeyCodeLabel.Text = _keyCode;
    }

    private async void OnValidateClicked(object sender, EventArgs e)
    {
        try
        {
            ErrorLabel.IsVisible = false;

            var entered = (LicenseEntry.Text ?? string.Empty).Trim().ToUpperInvariant();

            if (entered.Length != 5 || !_licenseService.ValidateLicense(_keyCode, entered))
            {
                ErrorLabel.Text = "Licença inválida. Tente novamente.";
                ErrorLabel.IsVisible = true;

                // MAUI: foco é síncrono
                LicenseEntry.Focus();

                return;
            }

            _licenseService.SaveLicense(entered);

            // Fecha modal e volta pro app
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = "Erro ao validar licença: " + ex.Message;
            ErrorLabel.IsVisible = true;
        }
    }

    private void OnExitClicked(object sender, EventArgs e)
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#else
        Application.Current?.Quit();
#endif
    }
}