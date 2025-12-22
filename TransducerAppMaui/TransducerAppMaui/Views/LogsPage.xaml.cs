using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using TransducerAppMaui.Helpers;
using TransducerAppMaui.Logs;
using TransducerAppMaui.Resources.Strings;

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

        RefreshButton.Clicked += async (_, __) =>
        {
            _beforeId = null;
            _items.Clear();
            await LoadNextPageAsync();
        };

        LoadMoreButton.Clicked += async (_, __) => await LoadNextPageAsync();

        ClearScreenButton.Clicked += (_, __) =>
        {
            _items.Clear();
            _beforeId = null;
            InfoLabel.Text = AppResources.Logs_Info_UiCleared;
        };

        ClearButton.Clicked += async (_, __) => await ClearDbAsync();
        ExportButton.Clicked += async (_, __) => await ExportCsvAndShareAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        InfoLabel.Text = AppResources.Logs_Info_Ready;
    }

    private async Task LoadNextPageAsync()
    {
        try
        {
            InfoLabel.Text = AppResources.Logs_Info_Loading;

            var page = await Task.Run(() =>
            {
                var list = _db.GetLogsBeforeId(_beforeId, PAGE_SIZE);

                int? nextBefore = _beforeId;
                if (list.Count > 0)
                    nextBefore = list.Min(x => x.Id);

                var lines = new List<string>(list.Count);
                foreach (var l in list)
                    lines.Add($"{l.TimestampUtc.ToLocalTime():yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");

                int total = 0;
                try { total = _db.CountLogs(); } catch { }

                return (lines, nextBefore, total);
            });

            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var line in page.lines)
                    _items.Add(line);

                _beforeId = page.nextBefore;

                InfoLabel.Text = string.Format(
                    AppResources.Logs_Info_LoadedFormat,
                    page.lines.Count,
                    _items.Count,
                    page.total);
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage LoadNextPageAsync");
            await DisplayAlert(AppResources.Dialog_Error, string.Format(AppResources.Logs_Error_Load, ex.Message), AppResources.Dialog_Ok);
            InfoLabel.Text = AppResources.Logs_Info_Error;
        }
    }

    private async Task ClearDbAsync()
    {
        try
        {
            var ok = await DisplayAlert(
                AppResources.Logs_Confirm_Title,
                AppResources.Logs_Confirm_ClearDb,
                AppResources.Common_Yes,
                AppResources.Common_No);

            if (!ok) return;

            await Task.Run(() => _db.ClearAllLogs());

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _items.Clear();
                _beforeId = null;
                InfoLabel.Text = AppResources.Logs_Info_DbCleared;
            });

            TransducerLogAndroid.LogInfo("Logs DB cleared by user.");
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage ClearDbAsync");
            await DisplayAlert(AppResources.Dialog_Error, string.Format(AppResources.Logs_Error_Clear, ex.Message), AppResources.Dialog_Ok);
        }
    }

    private async Task ExportCsvAndShareAsync()
    {
        try
        {
            TransducerLogAndroid.LogInfo("Exporting logs to CSV...");

            var logs = await Task.Run(() => _db.GetRecentLogs(100000));
            var csv = BuildCsv(logs);

            var folder = Path.Combine(FileSystem.AppDataDirectory, "Exports");
            Directory.CreateDirectory(folder);

            var fileName = $"TransducerLogs_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
            var fullPath = Path.Combine(folder, fileName);

            await File.WriteAllTextAsync(fullPath, csv, Encoding.UTF8);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = AppResources.Logs_Share_Title,
                File = new ShareFile(fullPath)
            });
        }
        catch (Exception ex)
        {
            TransducerLogAndroid.LogException(ex, "LogsPage ExportCsvAndShareAsync");
            await DisplayAlert(AppResources.Dialog_Error, string.Format(AppResources.Logs_Error_Export, ex.Message), AppResources.Dialog_Ok);
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

            var action = await DisplayActionSheet(
                AppResources.Logs_Item_ActionSheetTitle,
                AppResources.Common_Cancel,
                null,
                AppResources.Common_Copy,
                AppResources.Logs_Item_ViewFull);

            if (action == AppResources.Common_Copy)
                await Clipboard.Default.SetTextAsync(line);
            else if (action == AppResources.Logs_Item_ViewFull)
                await DisplayAlert(AppResources.Logs_Item_FullTitle, line, AppResources.Dialog_Ok);
        }
        catch { }
    }
}