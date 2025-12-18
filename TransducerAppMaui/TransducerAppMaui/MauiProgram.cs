using Microsoft.Extensions.Logging;
using TransducerAppMaui.Services;
using TransducerAppMaui.Services.Logging;
using TransducerAppMaui.Helpers;


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

        // NOVO: logger central do APP (camada MAUI)
        builder.Services.AddSingleton<IAppLog, AppLog>();

        // NOVO: serviço do transdutor (única instância)
        builder.Services.AddSingleton<ITransducerService, TransducerService>();


        // DB (SQLite) - instância única
        builder.Services.AddSingleton<DbHelper>();

        // Logger central do APP
        builder.Services.AddSingleton<IAppLog, AppLog>();

        // NOVO: persiste logs no SQLite
        builder.Services.AddSingleton<ILogPersistenceService, LogPersistenceService>();





#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}