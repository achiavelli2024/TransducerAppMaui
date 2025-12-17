namespace TransducerAppMaui.Services;

/// <summary>
/// Abstrai a obtenção de um identificador estável do dispositivo.
/// Android: AndroidId. Outros: GUID persistido.
/// </summary>
public interface IDeviceIdProvider
{
    string GetDeviceId();
}