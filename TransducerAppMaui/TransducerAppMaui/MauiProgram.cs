using Microsoft.Extensions.Logging;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Services;
using TransducerAppMaui.Services.Logging;

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

#if ANDROID
        builder.Services.AddSingleton<IDeviceIdProvider, AndroidDeviceIdProvider>();
#else
        builder.Services.AddSingleton<IDeviceIdProvider, DefaultDeviceIdProvider>();
#endif

        builder.Services.AddSingleton<LicenseService>();

        // SQLite
        builder.Services.AddSingleton<DbHelper>();

        // Serviço transdutor
        builder.Services.AddSingleton<ITransducerService, TransducerService>();

        // Mantemos AppLog por enquanto (não vou quebrar chamadas existentes),
        // mas o “logger oficial” para RX/TX e DB será TransducerLogAndroid (Xamarin-like).
        
        
        //builder.Services.AddSingleton<IAppLog, AppLog>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}