using TransducerAppMaui.Views;

namespace TransducerAppMaui.Views;

public partial class FreetestPage : ContentPage
{
    public FreetestPage()
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