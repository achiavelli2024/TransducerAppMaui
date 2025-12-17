using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TransducerProtocol;
using SQLite;
using Transducers; // PhoenixTransducer and related types (adjust if your project namespace differs)
using System.Globalization;
using Android.Net;

namespace TransducerAppXA
{
    [Activity(Label = "Test Runner")]
    public class TestRunnerActivity : Activity
    {
        // Defaults for transducer (TCP on port 23 and default local IP)
        const string DEFAULT_IP = "192.168.4.1";
        const int DEFAULT_PORT = 23;

        // UI
        ListView lvTests;
        ArrayAdapter<string> testsAdapter;
        List<TestDefinitionEntry> testsList = new List<TestDefinitionEntry>();

        EditText edtToolType;
        EditText edtTorqueMin;
        EditText edtTorqueNom;
        EditText edtTorqueMax;
        EditText edtFrequencia;
        EditText edtThreshold;
        EditText edtTimeout;

        Button btnRefresh;
        Button btnStart;
        Button btnStop;
        TextView tvStatus;
        TextView tvTorque;
        TextView tvAngle;

        // DB & repo
        string dbPath;
        TestsRepository testsRepo;

        // Phoenix transducer driver instance (native transducer connection)
        PhoenixTransducer Trans;

        // Cached params like in MainActivity (use same types)
        bool _paramsLoaded = false;
        decimal _currentThresholdIni = 0m;
        int _currentTimeoutEnd = 0;
        decimal _currentMinT = 0m;
        decimal _currentNomT = 0m;
        decimal _currentMaxT = 0m;
        ToolType selectedToolType = ToolType.ToolType1; // same enum used in MainActivity

        // --- FIX: explicit selected index so GetSelectedEntry never depends on SelectedItemPosition behavior ---
        int selectedTestIndex = -1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_test_runner);

            // find views (must match ids in activity_test_runner.xml)
            lvTests = FindViewById<ListView>(Resource.Id.lvTestsRunner);
            edtToolType = FindViewById<EditText>(Resource.Id.edtToolType);
            edtTorqueMin = FindViewById<EditText>(Resource.Id.edtTorqueMin);
            edtTorqueNom = FindViewById<EditText>(Resource.Id.edtTorqueNom);
            edtTorqueMax = FindViewById<EditText>(Resource.Id.edtTorqueMax);
            edtFrequencia = FindViewById<EditText>(Resource.Id.edtFrequencia);
            edtThreshold = FindViewById<EditText>(Resource.Id.edtThreshold);
            edtTimeout = FindViewById<EditText>(Resource.Id.edtTimeout);

            btnRefresh = FindViewById<Button>(Resource.Id.btnRefreshTests);
            btnStart = FindViewById<Button>(Resource.Id.btnStartTest);
            btnStop = FindViewById<Button>(Resource.Id.btnStopTest);
            tvStatus = FindViewById<TextView>(Resource.Id.tvRunnerStatus);

            // optional small live values on this screen (may be absent in layout)
            tvTorque = FindViewById<TextView>(Resource.Id.tvTorque);
            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);

            // adapters
            testsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItemSingleChoice);
            if (lvTests != null)
            {
                lvTests.Adapter = testsAdapter;
                // Make ListView single-choice so SelectedItemPosition/SetItemChecked works visually
                lvTests.ChoiceMode = ChoiceMode.Single;

                // ItemClick: set selected index explicitly and mark item checked (this fixes the "selecionar teste" issue)
                lvTests.ItemClick += (s, e) =>
                {
                    selectedTestIndex = e.Position;
                    try
                    {
                        lvTests.SetItemChecked(e.Position, true);
                    }
                    catch { }
                    var entry = testsList.ElementAtOrDefault(e.Position);
                    if (entry != null) PopulateFieldsFromEntry(entry);
                };
            }

            // dbPath and repo — QUALIFIQUEI System.IO.Path aqui para evitar ambiguidade
            dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "transducer.db3");
            testsRepo = new TestsRepository(dbPath);
            testsRepo.Initialize();

            // load list
            LoadTestsList();

            if (btnRefresh != null) btnRefresh.Click += (s, e) => LoadTestsList();

            // Start: connect to transducer (if needed) and show confirmation dialog (uses DB params)
            if (btnStart != null)
            {
                btnStart.Click += async (s, e) =>
                {
                    // Use the explicit selectedTestIndex instead of SelectedItemPosition
                    var entry = GetSelectedEntry();
                    if (entry == null)
                    {
                        // Helpful debug: if no test selected, show available count
                        int cnt = testsList?.Count ?? 0;
                        Toast.MakeText(this, $"Selecione um teste. (Há {cnt} testes no DB)", ToastLength.Short).Show();
                        return;
                    }

                    SetRunnerStatus("Preparing to start...");

                    // Connect to the transducer using PhoenixTransducer (port 23)
                    bool connected = await ConnectToTransducerAsync(entry);
                    if (!connected)
                    {
                        SetRunnerStatus("Connection failed");
                        Toast.MakeText(this, "Falha ao conectar ao transdutor.", ToastLength.Short).Show();
                        return;
                    }

                    // Now show confirmation dialog with the parameters from the selected test (like MainActivity.ShowInitParamsConfirmation)
                    ShowInitParamsConfirmation(entry);
                };
            }

            // Stop: stop acquisition / stop service
            if (btnStop != null)
            {
                btnStop.Click += (s, e) =>
                {
                    try
                    {
                        if (Trans != null)
                        {
                            try { Trans.StopReadData(); } catch { }
                            try { Trans.StopService(); } catch { }
                        }
                        SetRunnerStatus("Stopped");
                        Toast.MakeText(this, "Acquisition stopped", ToastLength.Short).Show();
                    }
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, "Stop error: " + ex.Message, ToastLength.Long).Show();
                    }
                };
            }
            
            void SetFieldsReadOnly(bool ro)
            {
                if (edtToolType != null) edtToolType.Enabled = !ro;
                if (edtTorqueMin != null) edtTorqueMin.Enabled = !ro;
                if (edtTorqueNom != null) edtTorqueNom.Enabled = !ro;
                if (edtTorqueMax != null) edtTorqueMax.Enabled = !ro;
                if (edtFrequencia != null) edtFrequencia.Enabled = !ro;
                if (edtThreshold != null) edtThreshold.Enabled = !ro;
                if (edtTimeout != null) edtTimeout.Enabled = !ro;
            }


            // set fields readonly (we auto-fill from DB, user doesn't edit here)
            SetFieldsReadOnly(true);

            SetRunnerStatus("Idle");
        }

        // Populate UI fields from DB TestDefinitionEntry (and set selectedToolType enum)
        void PopulateFieldsFromEntry(TestDefinitionEntry e)
        {
            try
            {
                string toolText = e.Notes != null ? ParseToolFromNotes(e.Notes) : "";
                if (edtToolType != null) edtToolType.Text = toolText;
                // try to convert to ToolType enum (notes may contain "ToolType1" or friendly name)
                selectedToolType = ParseToolEnumFromNotes(e.Notes) ?? ToolType.ToolType1;

                // numeric fields
                if (edtTorqueMin != null) edtTorqueMin.Text = e.MinTorque.ToString(CultureInfo.InvariantCulture);
                if (edtTorqueNom != null) edtTorqueNom.Text = e.NominalTorque.ToString(CultureInfo.InvariantCulture);
                if (edtTorqueMax != null) edtTorqueMax.Text = e.MaxTorque.ToString(CultureInfo.InvariantCulture);

                if (edtFrequencia != null) edtFrequencia.Text = ParseFreqFromNotes(e.Notes);
                if (edtThreshold != null) edtThreshold.Text = e.Notes != null ? ParseThresholdFromNotes(e.Notes) : "";
                if (edtTimeout != null) edtTimeout.Text = e.TimestampUtc ?? ParseTimeoutFromNotes(e.Notes) ?? "";
            }
            catch { /* ignore UI fill errors */ }
        }

        // Show confirmation dialog using parameters extracted from the selected DB entry (like MainActivity)
        void ShowInitParamsConfirmation(TestDefinitionEntry entry)
        {
            try
            {
                if (Trans == null)
                {
                    Toast.MakeText(this, "Transdutor não conectado", ToastLength.Short).Show();
                    return;
                }

                var limits = GetLimitsFromTestEntry(entry);

                var sb = new StringBuilder();
                sb.AppendLine("Confirme os parâmetros do teste:");
                sb.AppendLine();
                sb.AppendLine($"Torque mínimo:  {limits.MinT:F3} Nm");
                sb.AppendLine($"Torque nominal: {limits.NomT:F3} Nm");
                sb.AppendLine($"Torque máximo:  {limits.MaxT:F3} Nm");
                sb.AppendLine();
                sb.AppendLine($"Threshold inicial: {limits.ThresholdIni:F3} Nm");
                sb.AppendLine($"Threshold final:   {(limits.ThresholdIni / 4M):F3} Nm");
                sb.AppendLine($"Timeout fim (ms):  {limits.TimeoutEnd}");
                sb.AppendLine();
                sb.AppendLine($"Ferramenta: {GetFriendlyToolTypeName(limits.Tool)}");

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Confirmar parâmetros");
                builder.SetMessage(sb.ToString());
                builder.SetPositiveButton("Prosseguir", (s, e) =>
                {
                    // set cached params from the selected entry limits so InitReadAsync uses them
                    _currentMinT = limits.MinT;
                    _currentNomT = limits.NomT;
                    _currentMaxT = limits.MaxT;
                    _currentThresholdIni = limits.ThresholdIni;
                    _currentTimeoutEnd = limits.TimeoutEnd;
                    selectedToolType = limits.Tool;
                    _paramsLoaded = true;

                    _ = InitReadAsync();
                });
                builder.SetNegativeButton("Cancelar", (s, e) => { });
                builder.Show();
            }
            catch (Exception ex)
            {
                try { TransducerLogAndroid.LogException(ex, "ShowInitParamsConfirmation"); } catch { }
            }
        }

        // Convert TestDefinitionEntry into the limits structure used by InitRead
        struct TestLimits
        {
            public decimal ThresholdIni;
            public int TimeoutEnd;
            public decimal MinT;
            public decimal NomT;
            public decimal MaxT;
            public ToolType Tool;
        }

        TestLimits GetLimitsFromTestEntry(TestDefinitionEntry entry)
        {
            // start with defaults similar to MainActivity
            decimal thr = 3m;
            int tmo = 500;
            decimal min = 8m;
            decimal nom = 10m;
            decimal max = 12m;
            ToolType tool = ToolType.ToolType1;

            try
            {
                if (entry != null)
                {
                    // numeric values exist on entry
                    min = (decimal)entry.MinTorque;
                    nom = (decimal)entry.NominalTorque;
                    max = (decimal)entry.MaxTorque;

                    // parse threshold/timeout from notes if present
                    var thrText = ParseThresholdFromNotes(entry.Notes);
                    if (!string.IsNullOrWhiteSpace(thrText))
                    {
                        if (!decimal.TryParse(thrText, NumberStyles.Any, CultureInfo.InvariantCulture, out thr))
                            decimal.TryParse(thrText, NumberStyles.Any, CultureInfo.CurrentCulture, out thr);
                    }

                    var tmoText = ParseTimeoutFromNotes(entry.Notes);
                    if (!string.IsNullOrWhiteSpace(tmoText))
                        int.TryParse(tmoText, out tmo);

                    // tool: try parse enum from notes, fallback to default
                    var parsedTool = ParseToolEnumFromNotes(entry.Notes);
                    if (parsedTool.HasValue) tool = parsedTool.Value;
                }
            }
            catch (Exception ex)
            {
                try { TransducerLogAndroid.LogException(ex, "GetLimitsFromTestEntry"); } catch { }
            }

            return new TestLimits
            {
                ThresholdIni = thr,
                TimeoutEnd = tmo,
                MinT = min,
                NomT = nom,
                MaxT = max,
                Tool = tool
            };
        }

        string GetFriendlyToolTypeName(ToolType toolType)
        {
            var friendlyLabels = new string[]
            {
                "Apertadeira eletrônica com cabo",
                "Apertadeira de impulso",
                "Torquimetro de estalo",
                "Torquimetro Digital/Analógico",
                "Apertadeira pneumática",
                "Apertadeira à bateria",
                "Apertadeira à bateria transdutorizada",
                "Apertadeira embreagem com shutoff",
                "Vazio Ainda"
            };

            int index = ((int)toolType) - 1;
            if (index >= 0 && index < friendlyLabels.Length)
                return friendlyLabels[index];

            return toolType.ToString();
        }

        // Try parse ToolType enum from notes (returns null if can't)
        ToolType? ParseToolEnumFromNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes)) return null;
            try
            {
                var parts = notes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var kv = p.Split(new[] { '=' }, 2);
                    if (kv.Length != 2) continue;
                    if (kv[0].Trim().ToLowerInvariant() == "tool")
                    {
                        var val = kv[1].Trim();
                        // try parse as enum name
                        if (Enum.TryParse(typeof(ToolType), val, ignoreCase: true, out var ev))
                            return (ToolType)ev;
                        // try pattern like "ToolType1"
                        if (val.StartsWith("ToolType", StringComparison.OrdinalIgnoreCase))
                        {
                            if (Enum.TryParse(typeof(ToolType), val, ignoreCase: true, out var ev2))
                                return (ToolType)ev2;
                        }
                        // maybe it's a friendly label - try map by index if numeric
                        if (int.TryParse(val, out int idx))
                        {
                            string enumName = $"ToolType{idx}";
                            if (Enum.TryParse(typeof(ToolType), enumName, out var ev3))
                                return (ToolType)ev3;
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        // Load tests from DB into list view
        void LoadTestsList()
        {
            try
            {
                var list = testsRepo.GetAll();
                testsList = list ?? new List<TestDefinitionEntry>();
                testsAdapter.Clear();
                foreach (var t in testsList) testsAdapter.Add(t.ToString());
                testsAdapter.NotifyDataSetChanged();

                // auto-select first test if any (convenience to avoid "Selecione um teste." in many cases)
                if (testsList.Count > 0)
                {
                    selectedTestIndex = 0;
                    try
                    {
                        lvTests.SetItemChecked(0, true);
                        PopulateFieldsFromEntry(testsList[0]);
                    }
                    catch { }
                }
                else
                {
                    selectedTestIndex = -1;
                }

                SetRunnerStatus($"Loaded {testsList.Count} tests");
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Load tests error: " + ex.Message, ToastLength.Long).Show();
            }
        }

        // Get selected entry using explicit index
        TestDefinitionEntry GetSelectedEntry()
        {
            if (selectedTestIndex >= 0 && selectedTestIndex < testsList.Count)
                return testsList[selectedTestIndex];
            // fallback: try SelectedItemPosition (rare)
            try
            {
                int pos = lvTests?.SelectedItemPosition ?? -1;
                if (pos >= 0 && pos < testsList.Count) return testsList[pos];
            }
            catch { }
            return null;
        }

        // Connect to transducer using PhoenixTransducer (mirrors MainActivity logic)
        async Task<bool> ConnectToTransducerAsync(TestDefinitionEntry entry)
        {
            try
            {
                // Use default IP unless you extend UI to provide IP per test
                string ip = DEFAULT_IP;
                int port = DEFAULT_PORT;

                // If already have Trans and connected, reuse
                if (Trans != null)
                {
                    try
                    {
                        if (Trans.IsConnected)
                        {
                            SetRunnerStatus("Already connected");
                            return true;
                        }
                        try { Trans.StopReadData(); Trans.StopService(); } catch { }
                        Trans = null;
                    }
                    catch { Trans = null; }
                }

                // Bind process to WiFi network (like MainActivity)
                await TryBindToActiveWifiAsync().ConfigureAwait(false);

                // instantiate and wire events
                Trans = new PhoenixTransducer();
                Trans.bPrintCommToFile = true;

                // subscribe handlers (similar to MainActivity) - minimal handlers to update UI
                Trans.DataResult += (r) =>
                {
                    // update live torque/angle if present
                    RunOnUiThread(() =>
                    {
                        try
                        {
                            if (tvTorque != null) tvTorque.Text = $"{r.Torque:F3} Nm";
                            if (tvAngle != null) tvAngle.Text = $"{r.Angle:F2} º";
                        }
                        catch { }
                    });
                };

                Trans.TesteResult += (results) =>
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "Teste finished (TesteResult)", ToastLength.Short).Show();
                        SetRunnerStatus("Test finished (TesteResult)");
                    });
                };

                Trans.DataInformation += (di) => { /* no-op */ };
                Trans.DebugInformation += (s) => { /* no-op */ };
                try { Trans.RaiseError += (err) => RunOnUiThread(() => Toast.MakeText(this, "Transducer error: " + err, ToastLength.Short).Show()); } catch { }

                Trans.SetPerformance(ePCSpeed.Slow, eCharPoints.VeryFew);
                Trans.Eth_IP = ip;
                Trans.Eth_Port = port;

                // start service & communication on background thread
                Task.Run(() =>
                {
                    try
                    {
                        Trans.StartService();
                        Thread.Sleep(50);
                        Trans.StartCommunication();
                        Trans.RequestInformation();
                    }
                    catch (Exception ex)
                    {
                        RunOnUiThread(() => Toast.MakeText(this, "StartService error: " + ex.Message, ToastLength.Long).Show());
                    }
                });

                // poll for connection (same loop as MainActivity)
                int tries = 0;
                int maxTries = 50;
                while (tries++ < maxTries)
                {
                    await Task.Delay(200).ConfigureAwait(false);
                    try
                    {
                        if (Trans != null && Trans.IsConnected)
                        {
                            RunOnUiThread(() =>
                            {
                                SetRunnerStatus("Connected to transducer");
                                Toast.MakeText(this, $"Connected {ip}:{port}", ToastLength.Short).Show();
                            });
                            return true;
                        }
                    }
                    catch { /* continue polling */ }
                }

                // timed out
                RunOnUiThread(() =>
                {
                    SetRunnerStatus("Connection timed out");
                    Toast.MakeText(this, "Connection timed out (transducer)", ToastLength.Short).Show();
                });
                return false;
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => Toast.MakeText(this, "Connect error: " + ex.Message, ToastLength.Long).Show());
                return false;
            }
        }

        // Read UI fields and populate cached parameters. Returns true if ok.
        bool TryLoadParamsFromUi()
        {
            try
            {
                decimal tmpDec;
                int tmpInt;

                if (!string.IsNullOrWhiteSpace(edtTorqueMin?.Text) &&
                    (decimal.TryParse(edtTorqueMin.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(edtTorqueMin.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentMinT = tmpDec;

                if (!string.IsNullOrWhiteSpace(edtTorqueNom?.Text) &&
                    (decimal.TryParse(edtTorqueNom.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(edtTorqueNom.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentNomT = tmpDec;

                if (!string.IsNullOrWhiteSpace(edtTorqueMax?.Text) &&
                    (decimal.TryParse(edtTorqueMax.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(edtTorqueMax.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentMaxT = tmpDec;

                if (!string.IsNullOrWhiteSpace(edtThreshold?.Text) &&
                    (decimal.TryParse(edtThreshold.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(edtThreshold.Text.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentThresholdIni = tmpDec;
                else _currentThresholdIni = 3m;

                if (!string.IsNullOrWhiteSpace(edtTimeout?.Text) && int.TryParse(edtTimeout.Text.Trim(), out tmpInt))
                    _currentTimeoutEnd = tmpInt;
                else _currentTimeoutEnd = 500;

                // selectedToolType already set in PopulateFieldsFromEntry (fallback to default)
                _paramsLoaded = true;
                return true;
            }
            catch (Exception)
            {
                _paramsLoaded = false;
                return false;
            }
        }

        // Initialization and start of read sequence (same as MainActivity.InitReadAsync)
        async Task InitReadAsync()
        {
            if (Trans == null)
            {
                try { TransducerLogAndroid.LogInfo("InitRead: Trans not connected"); } catch { }
                RunOnUiThread(() => Toast.MakeText(this, "InitRead: Trans not connected", ToastLength.Short).Show());
                return;
            }

            // Carrega parâmetros uma vez
            if (!_paramsLoaded)
            {
                if (!TryLoadParamsFromUi())
                {
                    RunOnUiThread(() =>
                        Toast.MakeText(this, "Erro ao ler parâmetros da tela.", ToastLength.Long).Show());
                    return;
                }
            }

            try { TransducerLogAndroid.LogInfo($"InitRead PARAMS: thrIni={_currentThresholdIni} timeout={_currentTimeoutEnd} min={_currentMinT} nom={_currentNomT} max={_currentMaxT}"); } catch { }
            RunOnUiThread(() => SetRunnerStatus("InitRead: using cached params"));

            await Task.Run(() =>
            {
                try
                {
                    try
                    {
                        var frames = Trans.GetInitReadFrames();
                        if (frames != null)
                        {
                            foreach (var f in frames) { /* optionally log frames */ }
                        }
                    }
                    catch (Exception ex)
                    {
                        try { TransducerLogAndroid.LogError("InitRead", "failed to get init frames: " + ex.Message); } catch { }
                    }

                    Trans.SetZeroTorque();
                    Thread.Sleep(10);
                    Trans.SetZeroAngle();
                    Thread.Sleep(10);

                    Trans.SetTestParameter_ClickWrench(85, 95, 20);
                    Thread.Sleep(10);

                    decimal thresholdEnd = _currentThresholdIni / 4M;
                    if (thresholdEnd < 0) thresholdEnd = 0;

                    int timeStepMs = 5;
                    int filterFreq = 2000;

                    // Use same parameter types as MainActivity
                    Trans.SetTestParameter(
                        new DataInformation(),
                        TesteType.TorqueOnly,
                        selectedToolType,
                        _currentNomT,
                        _currentThresholdIni,
                        thresholdEnd,
                        _currentTimeoutEnd,
                        timeStepMs,
                        filterFreq,
                        eDirection.CW);

                    Thread.Sleep(100);
                    Trans.StartReadData();
                    RunOnUiThread(() => SetRunnerStatus("Acquisition started"));
                }
                catch (Exception ex)
                {
                    try { TransducerLogAndroid.LogError("InitRead", "background error: " + ex.Message); } catch { }
                    RunOnUiThread(() => SetRunnerStatus("InitRead error: " + ex.Message));
                }
            }).ConfigureAwait(false);

            RunOnUiThread(() => tvStatus.Text = "Status: Acquisition started");
        }

        // Try binding process to active Wi-Fi network (copied from MainActivity)
        async Task TryBindToActiveWifiAsync()
        {
            try
            {
                var cm = (ConnectivityManager)GetSystemService(ConnectivityService);
                if (cm == null)
                {
                    RunOnUiThread(() => SetRunnerStatus("TryBind: ConnectivityManager null"));
                    return;
                }

                var active = cm.ActiveNetwork;
                if (active == null)
                {
                    RunOnUiThread(() => SetRunnerStatus("TryBind: no active network"));
                    return;
                }

                var caps = cm.GetNetworkCapabilities(active);
                if (caps == null)
                {
                    RunOnUiThread(() => SetRunnerStatus("TryBind: network capabilities null"));
                    return;
                }

                if (caps.HasTransport(TransportType.Wifi))
                {
                    try
                    {
                        bool ok = ConnectivityManager.SetProcessDefaultNetwork(active);
                        RunOnUiThread(() => SetRunnerStatus("Bound process to Wi‑Fi: " + ok));
                    }
                    catch (Exception ex)
                    {
                        RunOnUiThread(() => SetRunnerStatus("TryBind error: " + ex.Message));
                    }
                }
                else
                {
                    RunOnUiThread(() => SetRunnerStatus("TryBind: active transport not Wi‑Fi"));
                }
            }
            catch (Exception ex) { RunOnUiThread(() => SetRunnerStatus("TryBindToActiveWifiAsync error: " + ex.Message)); }
        }

        // Parsing helpers for notes fields
        string ParseToolFromNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes)) return "";
            try
            {
                var parts = notes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var kv = p.Split(new[] { '=' }, 2);
                    if (kv.Length == 2 && kv[0].Trim().ToLowerInvariant() == "tool") return kv[1].Trim();
                }
            }
            catch { }
            return "";
        }

        string ParseFreqFromNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes)) return "";
            try
            {
                var parts = notes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var kv = p.Split(new[] { '=' }, 2);
                    if (kv.Length == 2 && kv[0].Trim().ToLowerInvariant() == "freq") return kv[1].Trim();
                }
            }
            catch { }
            return "";
        }

        string ParseThresholdFromNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes)) return "";
            try
            {
                var parts = notes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var kv = p.Split(new[] { '=' }, 2);
                    if (kv.Length == 2 && kv[0].Trim().ToLowerInvariant() == "threshold") return kv[1].Trim();
                }
            }
            catch { }
            return "";
        }

        string ParseTimeoutFromNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes)) return "";
            try
            {
                var parts = notes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var kv = p.Split(new[] { '=' }, 2);
                    if (kv.Length == 2 && kv[0].Trim().ToLowerInvariant() == "timeout") return kv[1].Trim();
                }
            }
            catch { }
            return "";
        }

        void SetRunnerStatus(string s)
        {
            RunOnUiThread(() => { try { tvStatus.Text = s; } catch { } });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                if (Trans != null)
                {
                    try { Trans.StopReadData(); } catch { }
                    try { Trans.StopService(); } catch { }
                    try { Trans.Dispose(); } catch { }
                    Trans = null;
                }
            }
            catch { }
        }
    }
}