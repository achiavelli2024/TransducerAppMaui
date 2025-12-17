#if ANDROID
using Android.Provider;
using Application = Android.App.Application;

namespace TransducerAppMaui.Services;

/// <summary>
/// AndroidId: Settings.Secure.AndroidId.
/// Mantém a mesma fonte usada no Xamarin.
/// </summary>
public sealed class AndroidDeviceIdProvider : IDeviceIdProvider
{
    public string GetDeviceId()
    {
        try
        {
            var ctx = Application.Context;
            var androidId = Settings.Secure.GetString(ctx.ContentResolver, Settings.Secure.AndroidId);
            return string.IsNullOrWhiteSpace(androidId) ? "UNKNOWN_ANDROID_DEVICE" : androidId;
        }
        catch
        {
            return "UNKNOWN_ANDROID_DEVICE";
        }
    }
}
#endif