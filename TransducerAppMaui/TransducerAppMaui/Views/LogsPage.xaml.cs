using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;

namespace TransducerAppMaui.Views;

public partial class LogsPage : ContentPage
{
    private readonly DbHelper _db;

    // UI list (equivalente ao logItems do Xamarin)
    private readonly ObservableCollection<string> _logItems = new();

    // Queue (equivalente ao pendingLogs do Xamarin)
    private readonly ConcurrentQueue<string> _pendingLogs = new();

    // Controle de flush (equivalente ao LOG_FLUSH_MS + Handler)
    private bool _flushScheduled;
    private const int LOG_FLUSH_MS = 350;
    private const int FLUSH_BATCH = 50;

    // Limites (UI) - DB continua com tudo
    private const int MAX_LOG_ITEMS = 1500;

    // Pause/resume (igual Xamarin)
    private volatile bool _logsPaused = false;

    // Subscribe guard
    private bool _subscribed;

    public LogsPage(DbHelper db)
    {
        InitializeComponent();

        _db = db;

        LogsList.ItemsSource = _logItems;

        RefreshButton.Clicked += async (_, __) => await ReloadFromDbAsync();
        ClearButton.Clicked += async (_, __) => await ClearDbAsync();
        ExportButton.Clicked += async (_, __) => await ExportCsvAndShareAsync();

        PauseButton.Clicked += (_, __) =>
        {
            _logsPaused = true;
            InfoLabel.Text = $"Paused | Showing {_logItems.Count}";
        };

        ResumeButton.Clicked += (_, __) =>
        {
            _logsPaused = false;
            InfoLabel.Text = $"Running | Showing {_logItems.Count}";
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // IMPORTANTE: ao entrar, começa PAUSADO para não competir com UI enquanto carrega e renderiza
        _logsPaused = true;
        InfoLabel.Text = "Loading from DB... (paused)";

        await ReloadFromDbAsync();

        // Subscribe para live logs (igual Xamarin)
        if (!_subscribed)
        {
            TransducerLogAndroid.OnLogAppended += TransducerLog_OnLogAppended;
            _subscribed = true;
        }

        InfoLabel.Text = $"Paused | Showing {_logItems.Count}";
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
        // reduz custo de renderização sem perder rastreabilidade (DB/CSV continuam completos)
        if (string.IsNullOrEmpty(line)) return "";

        // se tiver HEX gigantesco, corta para preview
        const int maxLen = 180;
        if (line.Length <= maxLen) return line;

        return line.Substring(0, maxLen) + " ...";
    }

    private void TransducerLog_OnLogAppended(TransducerLogAndroid.LogRecord rec)
    {
        try
        {
            if (_logsPaused) return;

            // ✅ CRÍTICO: NÃO DUPLICAR. Calcula ToString() uma vez só.
            var s = rec?.ToString() ?? "";
            _pendingLogs.Enqueue(UiCompact(s));

            // evita explosão
            if (_pendingLogs.Count > 20000)
                _pendingLogs.TryDequeue(out _);

            ScheduleLogFlush();
        }
        catch
        {
            // nunca quebrar
        }
    }

    private void ScheduleLogFlush()
    {
        if (_flushScheduled) return;
        _flushScheduled = true;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(LOG_FLUSH_MS), () =>
        {
            try
            {
                FlushPendingLogs();
            }
            finally
            {
                _flushScheduled = false;
            }

            // Se ainda tem pendente, agenda outro ciclo (igual Xamarin)
            if (!_pendingLogs.IsEmpty && !_logsPaused)
            {
                ScheduleLogFlush();
            }

            // false = roda uma vez (timer one-shot)
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
                    _logItems.Insert(0, line);
                    processed++;
                }

                // Limita UI (igual Xamarin)
                while (_logItems.Count > MAX_LOG_ITEMS)
                    _logItems.RemoveAt(_logItems.Count - 1);

                if (processed > 0)
                    InfoLabel.Text = $"Running | +{processed} | Showing {_logItems.Count}";
            }
            catch
            {
                // swallow
            }
        });
    }

    private async Task ReloadFromDbAsync()
    {
        var sw = Stopwatch.StartNew();
        try
        {
            // semelhante ao Xamarin: recentLogs = db.GetRecentLogs(1000);
            var list = await Task.Run(() => _db.GetRecentLogs(1000));

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _logItems.Clear();
                foreach (var l in list)
                {
                    // Note: l.Message já pode estar formatado. Mantemos como você está.
                    _logItems.Add($"{l.TimestampUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "ReloadFromDbAsync");
            await DisplayAlert("Erro", "Falha ao carregar logs do DB: " + ex.Message, "OK");
        }
        finally
        {
            sw.Stop();
            InfoLabel.Text = $"Loaded {_logItems.Count} from DB in {sw.ElapsedMilliseconds} ms | Paused";
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
                _logItems.Clear();
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
        catch
        {
            // ignore
        }
    }
}