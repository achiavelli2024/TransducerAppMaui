using Microsoft.Extensions.Logging;
using TransducerAppMaui.Services;

namespace TransducerAppMaui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // =========================
        // Dependency Injection (DI)
        // =========================

#if ANDROID
        builder.Services.AddSingleton<IDeviceIdProvider, AndroidDeviceIdProvider>();
#else
        builder.Services.AddSingleton<IDeviceIdProvider, DefaultDeviceIdProvider>();
#endif

        builder.Services.AddSingleton<LicenseService>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}