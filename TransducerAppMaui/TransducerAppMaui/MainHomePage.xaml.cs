namespace TransducerAppMaui;
using TransducerAppMaui.Views;


public partial class MainHomePage : ContentPage
{
    

    public MainHomePage()
    {
        InitializeComponent();
    }

    

    private async void OnFreetestClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FreeTestPage));
    }



}