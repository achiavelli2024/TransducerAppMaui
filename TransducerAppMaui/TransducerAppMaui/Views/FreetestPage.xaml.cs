using TransducerAppMaui.Views;
using TransducerAppMaui.Services;


namespace TransducerAppMaui.Views;

public partial class FreeTestPage : ContentPage
{
    private readonly ITransducerService _transducerService;
    public FreeTestPage()
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

        // Pega serviço via DI (sem mudar navegação atual)
        _transducerService = Application.Current!.Handler!.MauiContext!.Services.GetService<ITransducerService>()
                           ?? throw new InvalidOperationException("ITransducerService não registrado no DI.");

        // Default visual
        IpEntry.Text ??= "192.168.4.1";

        // Eventos UI
        ConnectButton.Clicked += OnConnectClicked;
        DisconnectButton.Clicked += OnDisconnectClicked;

        // Eventos do serviço
        _transducerService.ConnectionChanged += OnConnectionChanged;
        _transducerService.LiveDataReceived += OnLiveDataReceived;
        _transducerService.ErrorRaised += OnErrorRaised;
    }

    private async void OnConnectClicked(object? sender, EventArgs e)
    {
        try
        {
            StatusLabel.Text = "Status: Connecting";
            ConnectionStatusLabel.Text = "Connecting";
            ConnectionIndicator.TextColor = Color.FromArgb("#FF9800"); // laranja

            ConnectButton.IsEnabled = false;

            var ip = (IpEntry.Text ?? "").Trim();
            const int port = 23; // mesmo do Xamarin

            await _transducerService.ConnectAsync(ip, port);

            // a UI final é atualizada no evento ConnectionChanged
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao conectar: " + ex.Message, "OK");
            ConnectButton.IsEnabled = true;

            ConnectionStatusLabel.Text = "Disconnected";
            ConnectionIndicator.TextColor = Color.FromArgb("#F44336");
            StatusLabel.Text = "Status: Disconnected";
        }
    }

    private async void OnDisconnectClicked(object? sender, EventArgs e)
    {
        try
        {
            await _transducerService.DisconnectAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", "Falha ao desconectar: " + ex.Message, "OK");
        }
    }

    private void OnConnectionChanged(bool connected)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ConnectionIndicator.Text = "●";
            if (connected)
            {
                ConnectionIndicator.TextColor = Color.FromArgb("#4CAF50");
                ConnectionStatusLabel.Text = "Connected";
                StatusLabel.Text = "Status: Connected";

                ConnectButton.IsVisible = false;
                DisconnectButton.IsVisible = true;
            }
            else
            {
                ConnectionIndicator.TextColor = Color.FromArgb("#F44336");
                ConnectionStatusLabel.Text = "Disconnected";
                StatusLabel.Text = "Status: Disconnected";

                ConnectButton.IsVisible = true;
                DisconnectButton.IsVisible = false;
                ConnectButton.IsEnabled = true;
            }
        });
    }

    private void OnLiveDataReceived(TransducerAppMaui.Drivers.DataResult data)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            TorqueLabel.Text = $"{data.Torque:F3} Nm";
            AngleLabel.Text = $"{data.Angle:F2} º";
        });
    }

    private void OnErrorRaised(int err)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            // Nesta etapa: só mostra no Status (simples).
            StatusLabel.Text = $"Status: Error ER{err:00}";
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // evita duplicar handlers se a página for recriada
        _transducerService.ConnectionChanged -= OnConnectionChanged;
        _transducerService.LiveDataReceived -= OnLiveDataReceived;
        _transducerService.ErrorRaised -= OnErrorRaised;
    }











}

