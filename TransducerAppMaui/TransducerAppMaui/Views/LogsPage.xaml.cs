using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;

namespace TransducerAppMaui.Views;

public partial class LogsPage : ContentPage
{
    private readonly DbHelper _db;
    private readonly ObservableCollection<string> _items = new();

    private const int PAGE_SIZE = 200;
    private int _take = PAGE_SIZE;

    public LogsPage(DbHelper db)
    {
        InitializeComponent();

        _db = db;
        LogsCollection.ItemsSource = _items;

        RefreshButton.Clicked += async (_, __) =>
        {
            _take = PAGE_SIZE;
            await LoadFromDbAsync();
        };

        LoadMoreButton.Clicked += async (_, __) =>
        {
            _take += PAGE_SIZE;
            await LoadFromDbAsync();
        };

        ClearButton.Clicked += async (_, __) => await ClearDbAsync();
        ExportButton.Clicked += async (_, __) => await ExportCsvAndShareAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFromDbAsync();
    }

    private async Task LoadFromDbAsync()
    {
        try
        {
            InfoLabel.Text = $"Loading... (take={_take})";

            // Xamarin-like: pega do DB sem travar UI
            var list = await Task.Run(() => _db.GetRecentLogs(_take));

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _items.Clear();
                foreach (var l in list)
                    _items.Add($"{l.TimestampUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");

                InfoLabel.Text = $"Showing {_items.Count} logs";
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage LoadFromDbAsync failed");
            await DisplayAlert("Erro", "Falha ao carregar logs: " + ex.Message, "OK");
            InfoLabel.Text = "Error";
        }
    }

    private async Task ClearDbAsync()
    {
        try
        {
            var ok = await DisplayAlert("Confirmar", "Apagar todos os logs do banco?", "SIM", "NÃO");
            if (!ok) return;

            TransducerLogAndroid.LogWarn("Clearing logs from DB...");
            await Task.Run(() => _db.ClearAllLogs());

            _take = PAGE_SIZE;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _items.Clear();
                InfoLabel.Text = "Logs cleared";
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage ClearDbAsync failed");
            await DisplayAlert("Erro", "Falha ao limpar logs: " + ex.Message, "OK");
        }
    }

    private async Task ExportCsvAndShareAsync()
    {
        try
        {
            TransducerLogAndroid.LogInfo("Exporting logs to CSV...");

            var list = await Task.Run(() => _db.GetRecentLogs(100000));
            var csv = BuildCsv(list);

            var folder = Path.Combine(FileSystem.AppDataDirectory, "Exports");
            Directory.CreateDirectory(folder);

            var fileName = $"TransducerLogs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var fullPath = Path.Combine(folder, fileName);

            await File.WriteAllTextAsync(fullPath, csv, Encoding.UTF8);

            TransducerLogAndroid.LogInfo("Logs exported: {0}", fullPath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Export Logs (CSV)",
                File = new ShareFile(fullPath)
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "ExportCsvAndShareAsync failed");
            await DisplayAlert("Erro", "Falha ao exportar logs: " + ex.Message, "OK");
        }
    }

    private static string BuildCsv(List<LogEntry> logs)
    {
        var sb = new StringBuilder();
        sb.AppendLine("TimestampUtc;Message");

        foreach (var l in logs)
        {
            var ts = l.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var msg = (l.Message ?? "").Replace(";", ",").Replace("\n", " ").Replace("\r", " ");
            sb.Append(ts).Append(';').Append(msg).AppendLine();
        }

        return sb.ToString();
    }
}