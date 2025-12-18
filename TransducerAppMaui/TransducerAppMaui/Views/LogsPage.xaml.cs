using System.Collections.ObjectModel;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Services.Logging;

namespace TransducerAppMaui.Views;

public partial class LogsPage : ContentPage
{
    private readonly DbHelper _db;
    private readonly IAppLog _appLog;

    private readonly ObservableCollection<string> _items = new();
    private bool _subscribed;

    public LogsPage(DbHelper db, IAppLog appLog)
    {
        InitializeComponent();
        _db = db;
        _appLog = appLog;

        LogsCollection.ItemsSource = _items;

        RefreshButton.Clicked += async (_, __) => await LoadFromDbAsync();
        ClearButton.Clicked += async (_, __) => await ClearDbAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_subscribed)
        {
            _appLog.OnLogAppended += OnLogAppended;
            _subscribed = true;
        }

        await LoadFromDbAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (_subscribed)
        {
            _appLog.OnLogAppended -= OnLogAppended;
            _subscribed = false;
        }
    }

    private void OnLogAppended(AppLogRecord rec)
    {
        // Atualização em tempo real na tela de logs
        MainThread.BeginInvokeOnMainThread(() =>
        {
            _items.Insert(0, rec.ToString());
            if (_items.Count > 3000)
                _items.RemoveAt(_items.Count - 1);
        });
    }

    private async Task LoadFromDbAsync()
    {
        try
        {
            _appLog.Info("UI", "LogsPage: loading logs from DB...");

            var list = await Task.Run(() => _db.GetRecentLogs(1000));

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _items.Clear();
                foreach (var l in list)
                    _items.Add($"{l.TimestampUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");
            });

            _appLog.Info("UI", $"LogsPage: loaded {list.Count} logs from DB.");
        }
        catch (Exception ex)
        {
            _appLog.Exception(ex, "UI", "LogsPage: LoadFromDbAsync failed");
            await DisplayAlert("Erro", "Falha ao carregar logs: " + ex.Message, "OK");
        }
    }

    private async Task ClearDbAsync()
    {
        try
        {
            _appLog.Warn("DB", "LogsPage: clearing logs table...");

            await Task.Run(() => _db.ClearAllLogs());

            _items.Clear();

            _appLog.Info("DB", "LogsPage: logs cleared.");
        }
        catch (Exception ex)
        {
            _appLog.Exception(ex, "DB", "LogsPage: ClearDbAsync failed");
            await DisplayAlert("Erro", "Falha ao limpar logs: " + ex.Message, "OK");
        }
    }
}