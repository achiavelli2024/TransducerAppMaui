namespace TransducerAppMaui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

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
}