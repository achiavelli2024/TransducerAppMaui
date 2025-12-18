using TransducerAppMaui.Views;

namespace TransducerAppMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(FreetestPage), typeof(FreetestPage));


        }
    }
}
