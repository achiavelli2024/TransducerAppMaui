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

            await lic.EnsureLicenseAsync(this);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao validar licença: " + ex.Message, "OK");
        }
    }


}