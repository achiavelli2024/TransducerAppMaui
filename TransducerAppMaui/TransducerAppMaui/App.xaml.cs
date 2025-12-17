using TransducerAppMaui.Services;
using TransducerAppMaui.Views;

namespace TransducerAppMaui;

public partial class App : Application
{
    private bool _licenseCheckedOnce;

    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Created += async (_, __) =>
        {
            if (_licenseCheckedOnce)
                return;

            _licenseCheckedOnce = true;

            try
            {
                // Pequeno delay garante que a UI e o Shell já estão prontos
                await Task.Delay(250);

                var services = Current?.Handler?.MauiContext?.Services;
                var lic = services?.GetService<LicenseService>();
                if (lic is null)
                    return;

#if DEBUG
                // Para forçar teste, você pode descomentar:
                // lic.ResetLicenseForDebug();
#endif

                if (!lic.IsLicenseValid())
                {
                    // Abre a LicensePage modal (robusto, controlado, igual Xamarin)
                    var page = new LicensePage(lic);
                    await Shell.Current.Navigation.PushModalAsync(page);
                }
            }
            catch
            {
                // Se quiser logar depois, adicionamos logger
            }
        };

        return window;
    }
}