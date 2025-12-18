using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;
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
            if (_licenseCheckedOnce) return;
            _licenseCheckedOnce = true;

            try
            {
                await Task.Delay(250);

                var services = Current?.Handler?.MauiContext?.Services;

                // ✅ Xamarin-like logger init
                try
                {
                    var db = services?.GetService<DbHelper>();
                    if (db != null)
                    {
                        TransducerLogAndroid.Initialize(db);
                        TransducerLogAndroid.LogInfo("Logger initialized (Xamarin-like).");
                    }
                }
                catch (Exception ex)
                {
                    // fallback mínimo (não pode travar app)
                    try { System.Diagnostics.Debug.WriteLine("TransducerLogAndroid init error: " + ex); } catch { }
                }

                var lic = services?.GetService<LicenseService>();
                if (lic is null) return;

                if (!lic.IsLicenseValid())
                {
                    var page = new LicensePage(lic);
                    await Shell.Current.Navigation.PushModalAsync(page);
                }
            }
            catch
            {
                // swallow
            }
        };

        return window;
    }
}