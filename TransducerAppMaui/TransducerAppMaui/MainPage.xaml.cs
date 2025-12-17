namespace TransducerAppMaui;

public partial class MainPage : ContentPage
{
    private bool _licenseChecked;

    public MainPage()
    {
        InitializeComponent();

        // Somente layout nesta etapa.
        // Aqui você pode preencher o Picker com valores mockados só para visualizar:
        ToolTypePicker.ItemsSource = new[]
        {
            "Corded electronic tool",
            "Impulse tool",
            "Click wrench",
            "Digital/analog wrench",
            "Pneumatic tool",
            "Cordless tool",
            "Cordless transducerized tool",
            "Shut-off clutch tool",
            "Empty"
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_licenseChecked)
            return;

        _licenseChecked = true;

        try
        {
            var lic = Application.Current?.Handler?.MauiContext?.Services.GetService<Services.LicenseService>();
            if (lic is null)
            {
                await DisplayAlert("Erro", "LicenseService não está registrado no DI.", "OK");
                return;
            }

#if DEBUG
            // Diagnóstico: ajuda a entender por que não está aparecendo o prompt.
            // Se IsLicenseValid() for true, o prompt NÃO aparece.
            var keyCode = lic.GetKeyCode();
            var isValid = lic.IsLicenseValid();

            // Exibe 1 vez apenas para debug
            await DisplayAlert("DEBUG LICENÇA",
                $"KeyCode atual: {keyCode}\nIsLicenseValid: {isValid}\n\n" +
                "Se IsLicenseValid=true, o app não pedirá licença.\n" +
                "Para forçar, limpe os dados do app (Android) ou implemente Reset (DEBUG).",
                "OK");
#endif

            await lic.EnsureLicenseAsync(this);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao validar licença: " + ex.Message, "OK");
        }
    }
}