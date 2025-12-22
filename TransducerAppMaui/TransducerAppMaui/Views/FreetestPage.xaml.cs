using System.Collections.ObjectModel;
using System.Globalization;
using TransducerAppMaui.Drivers;
using TransducerAppMaui.Models;
using TransducerAppMaui.Resources.Strings;
using TransducerAppMaui.Services;
using TransducerAppMaui.Services.Logging;


namespace TransducerAppMaui.Views;

public partial class FreeTestPage : ContentPage
{
    private readonly ITransducerService _transducerService;

    // Evita duplicar subscribe/unsubscribe quando navega pra frente/volta
    private bool _subscribed;

    //private readonly IAppLog _appLog;
    //private readonly ObservableCollection<string> _logLines = new();
    //private const int MAX_LOG_LINES = 2000;





    public FreeTestPage()
    {
        InitializeComponent();

        // Picker (labels amigáveis)
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

        // Pega serviço via DI
        _transducerService = Application.Current!.Handler!.MauiContext!.Services.GetService<ITransducerService>()
                           ?? throw new InvalidOperationException("ITransducerService não registrado no DI.");


        //_appLog = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IAppLog>();

        // setar ItemsSource 1x
        //LogsCollection.ItemsSource = _logLines;



        // Defaults visuais (igual Xamarin)
        ApplyDefaultsToUi();

        // Eventos de UI (botões)
        ConnectButton.Clicked += OnConnectClicked;
        DisconnectButton.Clicked += OnDisconnectClicked;

        // ESTES DOIS ESTAVAM FALTANDO (ou dependiam de estar no XAML)
        InitReadButton.Clicked += OnInitReadClicked;
        StopReadButton.Clicked += OnStopReadClicked;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Subscribe robusto (faz ao aparecer na tela)
        if (!_subscribed)
        {
            _transducerService.ConnectionChanged += OnConnectionChanged;

            ReconnectCountLabel.Text = string.Format(AppResources.FreeTest_Label_Reconnection, 0);
            UptimeLabel.Text = string.Format(AppResources.FreeTest_Label_Uptime, "00:00:00");

            _transducerService.LiveDataReceived += OnLiveDataReceived;
            _transducerService.ErrorRaised += OnErrorRaised;
            _subscribed = true;

            //_appLog.OnLogAppended += OnAppLogAppended;
        }

        



        // Reaplica estado atual do serviço na UI (importante ao voltar para a tela)
        OnConnectionChanged(_transducerService.IsConnected);

        // Ajusta botões Init/Stop conforme estado do teste
        UpdateAcquisitionUiState();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (_subscribed)
        {
            _transducerService.ConnectionChanged -= OnConnectionChanged;
            _transducerService.LiveDataReceived -= OnLiveDataReceived;
            _transducerService.ErrorRaised -= OnErrorRaised;
            _subscribed = false;

            //_appLog.OnLogAppended -= OnAppLogAppended;

        }
    }

    private void OnAppLogAppended(AppLogRecord rec)
    {
        if (ShowLogsSwitch?.IsToggled == false) return;
        if (PauseLogsSwitch?.IsToggled == true) return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            //_logLines.Insert(0, rec.ToString());
            //if (_logLines.Count > MAX_LOG_LINES)
                //_logLines.RemoveAt(_logLines.Count - 1);
        });

    }



    private void ApplyDefaultsToUi()
    {
        // Só aplica se estiver vazio
        IpEntry.Text ??= "192.168.4.1";

        ThresholdEntry.Text ??= FreeTestParameters.DEFAULT_THRESHOLD_INI.ToString(CultureInfo.InvariantCulture);
        TimeoutEntry.Text ??= FreeTestParameters.DEFAULT_TIMEOUT_END.ToString(CultureInfo.InvariantCulture);
        MinTorqueEntry.Text ??= FreeTestParameters.DEFAULT_MIN_TORQUE.ToString(CultureInfo.InvariantCulture);
        NomTorqueEntry.Text ??= FreeTestParameters.DEFAULT_NOM_TORQUE.ToString(CultureInfo.InvariantCulture);
        MaxTorqueEntry.Text ??= FreeTestParameters.DEFAULT_MAX_TORQUE.ToString(CultureInfo.InvariantCulture);
        FrequencyEntry.Text ??= FreeTestParameters.DEFAULT_FREQ.ToString(CultureInfo.InvariantCulture);
    }

    private FreeTestParameters ReadParametersFromUi()
    {
        // Equivalente ao TryLoadParamsFromUi do Xamarin
        var p = new FreeTestParameters
        {
            Ip = (IpEntry.Text ?? "").Trim(),
            ThresholdIni = FreeTestParameters.ParseDecimalOrDefault(ThresholdEntry.Text, FreeTestParameters.DEFAULT_THRESHOLD_INI),
            TimeoutEndMs = FreeTestParameters.ParseIntOrDefault(TimeoutEntry.Text, FreeTestParameters.DEFAULT_TIMEOUT_END),
            MinTorque = FreeTestParameters.ParseDecimalOrDefault(MinTorqueEntry.Text, FreeTestParameters.DEFAULT_MIN_TORQUE),
            NomTorque = FreeTestParameters.ParseDecimalOrDefault(NomTorqueEntry.Text, FreeTestParameters.DEFAULT_NOM_TORQUE),
            MaxTorque = FreeTestParameters.ParseDecimalOrDefault(MaxTorqueEntry.Text, FreeTestParameters.DEFAULT_MAX_TORQUE),
            Frequency = FreeTestParameters.ParseIntOrDefault(FrequencyEntry.Text, FreeTestParameters.DEFAULT_FREQ),
            ToolType = ToolTypeFromPicker()
        };

        p.Sanitize();
        return p;
    }

    private ToolType ToolTypeFromPicker()
    {
        // posição do picker -> ToolType1..ToolType9
        var idx = ToolTypePicker.SelectedIndex;
        if (idx < 0) idx = 0;

        var toolTypeNumber = idx + 1; // ToolType1 é 1
        var enumName = $"ToolType{toolTypeNumber}";

        return Enum.TryParse(enumName, out ToolType tt) ? tt : ToolType.ToolType1;
    }

    private static string BuildConfirmationText(FreeTestParameters p)
    {
        return
            "Confirme os parâmetros do teste:\n\n" +
            $"Torque mínimo:  {p.MinTorque.ToString("F3", CultureInfo.InvariantCulture)} Nm\n" +
            $"Torque nominal: {p.NomTorque.ToString("F3", CultureInfo.InvariantCulture)} Nm\n" +
            $"Torque máximo:  {p.MaxTorque.ToString("F3", CultureInfo.InvariantCulture)} Nm\n\n" +
            $"Threshold inicial: {p.ThresholdIni.ToString("F3", CultureInfo.InvariantCulture)} Nm\n" +
            $"Threshold final:   {p.ThresholdEnd.ToString("F3", CultureInfo.InvariantCulture)} Nm\n" +
            $"Timeout fim (ms):  {p.TimeoutEndMs}\n\n" +
            $"Frequência (hz):  {p.Frequency}\n\n" +
            $"Ferramenta: {p.ToolType}";
    }

    private async void OnInitReadClicked(object? sender, EventArgs e)
    {
        try
        {
            if (!_transducerService.IsConnected)
            {
                await DisplayAlert(AppResources.FreeTest_Alert_WarningTitle, AppResources.FreeTest_Warn_ConnectBefore, AppResources.Dialog_Ok);
                return;
            }

            // 1) Ler UI -> parâmetros e salvar no serviço (cache)
            var p = ReadParametersFromUi();
            _transducerService.SetParameters(p);

            // 2) Confirmação (igual Xamarin)
            var ok = await DisplayAlert(
                AppResources.FreeTest_Confirm_Params_Title,
                BuildConfirmationText(p),
                AppResources.FreeTest_Confirm_Params_Proceed,
                AppResources.FreeTest_Confirm_Params_Cancel);

            if (!ok) return;

            // Travar UI básica para evitar mudanças no meio
            SetParameterInputsEnabled(false);

            //StatusLabel.Text = "Status: Starting acquisition...";
            StatusLabel.Text = AppResources.FreeTest_Status_StartingAcq;

            await _transducerService.StartAcquisitionAsync(firstStart: true);

            //StatusLabel.Text = "Status: Acquisition started";
            StatusLabel.Text = AppResources.FreeTest_Status_AcqStarted;

            UpdateAcquisitionUiState();
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.FreeTest_Alert_ErrorTitle, string.Format(AppResources.FreeTest_Error_InitRead, ex.Message), AppResources.Dialog_Ok);
            //StatusLabel.Text = "Status: Error";
            StatusLabel.Text = AppResources.FreeTest_Status_ErrorGeneric;


            // Em caso de erro, libera inputs
            SetParameterInputsEnabled(true);
            UpdateAcquisitionUiState();
        }
    }

    private async void OnStopReadClicked(object? sender, EventArgs e)
    {
        try
        {
            await _transducerService.StopAcquisitionAsync();
            //StatusLabel.Text = "Status: Stopped";
            StatusLabel.Text = AppResources.FreeTest_Status_Stopped;


            // Libera inputs ao parar
            SetParameterInputsEnabled(true);
            UpdateAcquisitionUiState();
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.FreeTest_Alert_ErrorTitle, string.Format(AppResources.FreeTest_Error_Stop, ex.Message), AppResources.Dialog_Ok);
        }
    }

    private async void OnConnectClicked(object? sender, EventArgs e)
    {
        try
        {
            //StatusLabel.Text = "Status: Connecting";
           //ConnectionStatusLabel.Text = "Connecting";

            StatusLabel.Text = AppResources.FreeTest_Status_Connecting;
            ConnectionStatusLabel.Text = AppResources.FreeTest_Conn_Connecting;



            ConnectionIndicator.TextColor = Color.FromArgb("#FF9800"); // laranja

            ConnectButton.IsEnabled = false;

            var ip = (IpEntry.Text ?? "").Trim();
            const int port = 23;

            await _transducerService.ConnectAsync(ip, port);
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.FreeTest_Alert_ErrorTitle, string.Format(AppResources.FreeTest_Error_Connect, ex.Message), AppResources.Dialog_Ok);
            ConnectButton.IsEnabled = true;

            //ConnectionStatusLabel.Text = "Disconnected";
            //ConnectionIndicator.TextColor = Color.FromArgb("#F44336");
            //StatusLabel.Text = "Status: Disconnected";
            ConnectionStatusLabel.Text = AppResources.FreeTest_Conn_Disconnected;
            StatusLabel.Text = AppResources.FreeTest_Status_Disconnected;
            ConnectionIndicator.TextColor = Color.FromArgb("#F44336");


        }
    }

    private async void OnDisconnectClicked(object? sender, EventArgs e)
    {
        try
        {
            await _transducerService.DisconnectAsync();

            // Ao desconectar, libera inputs e reseta UI
            SetParameterInputsEnabled(true);
            UpdateAcquisitionUiState();
        }
        catch (Exception ex)
        {
            await DisplayAlert(AppResources.FreeTest_Alert_ErrorTitle, string.Format(AppResources.FreeTest_Error_Disconnect, ex.Message), AppResources.Dialog_Ok);
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
                //ConnectionStatusLabel.Text = "Connected";
                //StatusLabel.Text = "Status: Connected";

                ConnectionStatusLabel.Text = AppResources.FreeTest_Conn_Connected;
                StatusLabel.Text = AppResources.FreeTest_Status_Connected;



                ConnectButton.IsVisible = false;
                DisconnectButton.IsVisible = true;
            }
            else
            {
                ConnectionIndicator.TextColor = Color.FromArgb("#F44336");

                //ConnectionStatusLabel.Text = "Disconnected";
                //StatusLabel.Text = "Status: Disconnected";
                ConnectionStatusLabel.Text = AppResources.FreeTest_Conn_Disconnected;
                StatusLabel.Text = AppResources.FreeTest_Status_Disconnected;


                ConnectButton.IsVisible = true;
                DisconnectButton.IsVisible = false;
                ConnectButton.IsEnabled = true;
            }

            // Quando muda conexão, também ajusta botões de aquisição
            UpdateAcquisitionUiState();
        });
    }

    private void OnLiveDataReceived(DataResult data)
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
            //StatusLabel.Text = $"Status: Error ER{err:00}";
            StatusLabel.Text = string.Format(AppResources.FreeTest_Status_ErrorCode, err);

        });
    }

    private void UpdateAcquisitionUiState()
    {
        // Ajuste mínimo de UX:
        // - só pode iniciar se estiver conectado
        // - só pode parar se estiver rodando
        var connected = _transducerService.IsConnected;

        InitReadButton.IsEnabled = connected;
        StopReadButton.IsEnabled = connected && _transducerService.IsTestRunning;
    }

    private void SetParameterInputsEnabled(bool enabled)
    {
        // Equivalente ao SetParameterInputsEnabled do Xamarin (versão MAUI)
        IpEntry.IsEnabled = enabled;

        ThresholdEntry.IsEnabled = enabled;
        TimeoutEntry.IsEnabled = enabled;

        MinTorqueEntry.IsEnabled = enabled;
        NomTorqueEntry.IsEnabled = enabled;
        MaxTorqueEntry.IsEnabled = enabled;

        FrequencyEntry.IsEnabled = enabled;
        ToolTypePicker.IsEnabled = enabled;
    }
}