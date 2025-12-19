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

    private const int PAGE_SIZE = 100;

    // Cursor: menor Id já carregado (para pegar mais antigos)
    private int? _beforeId = null;

    public LogsPage(DbHelper db)
    {
        InitializeComponent();
        _db = db;

        LogsList.ItemsSource = _items;

        // LOAD (do zero)
        RefreshButton.Clicked += async (_, __) =>
        {
            _beforeId = null;
            _items.Clear();
            await LoadNextPageAsync();
        };

        // LOAD +
        LoadMoreButton.Clicked += async (_, __) => await LoadNextPageAsync();

        // CLEAR UI
        ClearScreenButton.Clicked += (_, __) =>
        {
            _items.Clear();
            _beforeId = null;
            InfoLabel.Text = "UI cleared (DB unchanged)";
        };

        // CLEAR DB
        ClearButton.Clicked += async (_, __) => await ClearDbAsync();

        // EXPORT CSV (tudo do DB)
        ExportButton.Clicked += async (_, __) => await ExportCsvAndShareAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // não auto-carrega. Você pediu botão para carregar.
        InfoLabel.Text = "Ready (press LOAD)";
    }

    private async Task LoadNextPageAsync()
    {
        try
        {
            InfoLabel.Text = "Loading...";

            var page = await Task.Run(() =>
            {
                var list = _db.GetLogsBeforeId(_beforeId, PAGE_SIZE);

                // atualiza cursor
                int? nextBefore = _beforeId;
                if (list.Count > 0)
                    nextBefore = list.Min(x => x.Id);

                var lines = new List<string>(list.Count);
                foreach (var l in list)
                {
                    lines.Add($"{l.TimestampUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");
                }

                int total = 0;
                try { total = _db.CountLogs(); } catch { }

                return (lines, nextBefore, total);
            });

            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var line in page.lines)
                    _items.Add(line);

                _beforeId = page.nextBefore;

                InfoLabel.Text = $"Loaded +{page.lines.Count} | Showing {_items.Count} | Total {page.total}";
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage LoadNextPageAsync");
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

            await Task.Run(() => _db.ClearAllLogs());

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _items.Clear();
                _beforeId = null;
                InfoLabel.Text = "DB cleared";
            });

            TransducerLogAndroid.LogInfo("Logs DB cleared by user.");
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage ClearDbAsync");
            await DisplayAlert("Erro", "Falha ao limpar logs: " + ex.Message, "OK");
        }
    }

    private async Task ExportCsvAndShareAsync()
    {
        try
        {
            TransducerLogAndroid.LogInfo("Exporting logs to CSV...");

            var logs = await Task.Run(() => _db.GetRecentLogs(100000)); // se quiser exportar TUDO MESMO, podemos criar GetAllLogs()
            var csv = BuildCsv(logs);

            var folder = Path.Combine(FileSystem.AppDataDirectory, "Exports");
            Directory.CreateDirectory(folder);

            var fileName = $"TransducerLogs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var fullPath = Path.Combine(folder, fileName);

            await File.WriteAllTextAsync(fullPath, csv, Encoding.UTF8);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Export Logs (CSV)",
                File = new ShareFile(fullPath)
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage ExportCsvAndShareAsync");
            await DisplayAlert("Erro", "Falha ao exportar logs: " + ex.Message, "OK");
        }
    }

    private static string BuildCsv(List<LogEntry> logs)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Id;TimestampUtc;Message");

        foreach (var l in logs)
        {
            var ts = l.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var msg = (l.Message ?? "").Replace(";", ",").Replace("\n", " ").Replace("\r", " ");
            sb.Append(l.Id).Append(';').Append(ts).Append(';').Append(msg).AppendLine();
        }

        return sb.ToString();
    }

    private async void LogsList_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {
            if (e.Item is not string line) return;
            LogsList.SelectedItem = null;

            var action = await DisplayActionSheet("Log", "Cancelar", null, "Copiar", "Ver completo");
            if (action == "Copiar")
                await Clipboard.Default.SetTextAsync(line);
            else if (action == "Ver completo")
                await DisplayAlert("Log completo", line, "OK");
        }
        catch { }
    }
}