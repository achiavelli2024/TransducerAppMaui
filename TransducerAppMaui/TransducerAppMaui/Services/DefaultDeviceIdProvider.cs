using Microsoft.Maui.Storage;

namespace TransducerAppMaui.Services;

/// <summary>
/// Fallback cross-platform: cria um GUID e persiste em Preferences.
/// Assim o "device id" permanece estável naquele PC/aparelho.
/// </summary>
public sealed class DefaultDeviceIdProvider : IDeviceIdProvider
{
    private const string DeviceIdKey = "TransducerAppMaui.DeviceId";

    public string GetDeviceId()
    {
        try
        {
            var existing = Preferences.Get(DeviceIdKey, string.Empty);
            if (!string.IsNullOrWhiteSpace(existing))
                return existing;

            var created = Guid.NewGuid().ToString("N");
            Preferences.Set(DeviceIdKey, created);
            return created;
        }
        catch
        {
            return "UNKNOWN_DEVICE";
        }
    }
}