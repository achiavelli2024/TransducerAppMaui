using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;

namespace TransducerAppMaui.Views;

public partial class LogsPage : ContentPage
{
    private readonly DbHelper _db;

    // LIVE
    private readonly ObservableCollection<string> _liveItems = new();
    private readonly ConcurrentQueue<string> _pendingLogs = new();

    private bool _flushScheduled;
    private const int LOG_FLUSH_MS = 350;
    private const int FLUSH_BATCH = 50;
    private const int MAX_LIVE_ITEMS = 1500;

    private volatile bool _logsPaused = false;
    private bool _subscribed;

    private bool _showingDbSnapshot = true;

    // Xamarin-like: carregar por páginas
    private const int PAGE_SIZE = 200;
    private int _take = PAGE_SIZE;

    public LogsPage(DbHelper db)
    {
        InitializeComponent();
        _db = db;

        LogsList.ItemsSource = _liveItems;

        RefreshButton.Clicked += async (_, __) =>
        {
            _take = PAGE_SIZE;
            await LoadDbSnapshotAsync();
        };

        // Reaproveita o botão RESUME como "LIVE" e PAUSE como "snapshot"
        PauseButton.Clicked += async (_, __) =>
        {
            _logsPaused = true;
            _showingDbSnapshot = true;

            // volta a mostrar snapshot do DB
            await LoadDbSnapshotAsync();
        };

        ResumeButton.Clicked += (_, __) =>
        {
            _logsPaused = false;

            if (_showingDbSnapshot)
            {
                _showingDbSnapshot = false;
                LogsList.ItemsSource = _liveItems;
                _liveItems.Clear();
            }

            InfoLabel.Text = $"Running | Live {_liveItems.Count}";
        };

        ClearButton.Clicked += async (_, __) => await ClearDbAsync();
        ExportButton.Clicked += async (_, __) => await ExportCsvAndShareAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _logsPaused = true;
        _showingDbSnapshot = true;

        // não bloqueia abertura: carrega depois
        _ = LoadDbSnapshotAsync();

        if (!_subscribed)
        {
            TransducerLogAndroid.OnLogAppended += TransducerLog_OnLogAppended;
            _subscribed = true;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (_subscribed)
        {
            TransducerLogAndroid.OnLogAppended -= TransducerLog_OnLogAppended;
            _subscribed = false;
        }
    }

    private static string UiCompact(string line)
    {
        if (string.IsNullOrEmpty(line)) return "";
        const int maxLen = 180;
        if (line.Length <= maxLen) return line;
        return line.Substring(0, maxLen) + " ...";
    }

    private void TransducerLog_OnLogAppended(TransducerLogAndroid.LogRecord rec)
    {
        try
        {
            if (_logsPaused) return;
            if (_showingDbSnapshot) return;

            var s = rec?.ToString() ?? "";
            _pendingLogs.Enqueue(UiCompact(s));

            if (_pendingLogs.Count > 20000)
                _pendingLogs.TryDequeue(out _);

            ScheduleLogFlush();
        }
        catch { }
    }

    private void ScheduleLogFlush()
    {
        if (_flushScheduled) return;
        _flushScheduled = true;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(LOG_FLUSH_MS), () =>
        {
            try { FlushPendingLogs(); }
            finally { _flushScheduled = false; }

            if (!_pendingLogs.IsEmpty && !_logsPaused)
                ScheduleLogFlush();

            return false;
        });
    }

    private void FlushPendingLogs()
    {
        if (_logsPaused) return;

        int processed = 0;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            try
            {
                while (processed < FLUSH_BATCH && _pendingLogs.TryDequeue(out var line))
                {
                    _liveItems.Insert(0, line);
                    processed++;
                }

                while (_liveItems.Count > MAX_LIVE_ITEMS)
                    _liveItems.RemoveAt(_liveItems.Count - 1);

                if (processed > 0)
                    InfoLabel.Text = $"Running | +{processed} | Live {_liveItems.Count}";
            }
            catch { }
        });
    }

    private async Task LoadDbSnapshotAsync()
    {
        try
        {
            InfoLabel.Text = "Loading DB...";

            var snapshot = await Task.Run(() =>
            {
                // conta total para você entender se realmente tem 600 no DB
                int total = 0;
                try { total = _db.CountLogs(); } catch { }

                var list = _db.GetRecentLogs(_take);

                var outList = new List<string>(list.Count);
                foreach (var l in list)
                    outList.Add($"{l.TimestampUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");

                return (total, outList);
            });

            MainThread.BeginInvokeOnMainThread(() =>
            {
                // aplica em lote: rolável e mais leve
                LogsList.ItemsSource = snapshot.outList;
                InfoLabel.Text = $"DB: showing {snapshot.outList.Count} / total {snapshot.total} (take={_take})";
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LoadDbSnapshotAsync");
            await DisplayAlert("Erro", "Falha ao carregar logs do DB: " + ex.Message, "OK");
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
                _liveItems.Clear();
                LogsList.ItemsSource = _liveItems;
                _showingDbSnapshot = false;
                InfoLabel.Text = "Logs cleared";
            });

            TransducerLogAndroid.LogInfo("All logs cleared from DB by user.");
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "ClearDbAsync");
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

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Export Logs (CSV)",
                File = new ShareFile(fullPath)
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "ExportCsvAndShareAsync");
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