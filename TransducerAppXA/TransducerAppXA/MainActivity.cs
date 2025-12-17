using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Transducers;
using System.IO;
using System.Diagnostics;
using AndroidSwitch = Android.Widget.Switch;
using TransducerAppXA.Helpers;
using Android.Util;
using Android.Content.PM;
using Android.Net;
using Android.Provider;
using System.Security.Cryptography;
using System.Globalization;

//using Android.App;
//using Android.Widget;
using Android;
//using Android.Content.PM;
using AndroidX.Core.App;
using AndroidX.Core.Content;
//using System.IO;
//using TransducerAppXA.Services; // <-- RealTimeStatistics

using TransducerAppXA.Services; // <- usado StatisticsService
using TransducerAppXA.Activities; // <- StatsActivity






namespace TransducerAppXA
{
    [Activity(
        Label = "TransducerAppXA",
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        const string APP_VERSION_V1 = "V2";

        PhoenixTransducer Trans;

        // === LICENÇA ===
        const string PrefsName = "TransducerAppLicensePrefs";
        const string PrefsKeyLicense = "LicenseCode";
        const int LicenseLength = 5;

        // === DEFAULTS ÚNICOS (O QUE VOCÊ PEDIU) ===
        const decimal DEFAULT_THRESHOLD_INI = 3M;
        const int DEFAULT_TIMEOUT_END = 500;
        const decimal DEFAULT_MIN_TORQUE = 8M;
        const decimal DEFAULT_NOM_TORQUE = 10M;
        const decimal DEFAULT_MAX_TORQUE = 12M;

        const int DEFAULT_FREQ = 2000;


        // Estado atual de parâmetros de teste Variaveis da tela
        bool _paramsLoaded = false;
        decimal _currentThresholdIni;
        int _currentTimeoutEnd;
        decimal _currentMinT;
        decimal _currentNomT;
        decimal _currentMaxT;

        int _currentFreq;



        // UI controls
        EditText txtIP;
        EditText txtPort;
        EditText txtIndex;
        Button btnConnectIP;
        Button btnDisconnect;
        Button btnInitRead;
        Button btnStartRead;
        Button btnStopRead;
        Button btnCopyLog;
        Button btnClearLog;
        Button btnClearResults;
        Button btnZeroTqAn;
        Button btnExportResults;

        Button btnExportLogs;

        Button btnSyncToPc;

        Button btnExportDB;

        Button btnTestRunner;

        Button btnTestAndroidLayout;



        EditText txtThresholdIniFree;
        EditText txtThresholdEndFree;
        EditText txtTimeoutFree;
        EditText txtNominalTorque;
        EditText txtMinimumTorque;
        EditText txtMaximoTorque;

        EditText txtFrequencia;




        EditText txtMinimumTorque2;
        EditText txtNominalTorque2;
        EditText txtMaximoTorque2;

        TextView tvTorque;
        TextView tvAngle;
        TextView tvResultsCounter;
        TextView tvUntighteningsCounter;
        TextView tvStatus;


        TextView tvMainStatsMean;
        TextView tvMainStatsCm;
        TextView tvMainStatsCmk;


        TextView tvReconnectCount;
        TextView tvConnUptime;
        System.Timers.Timer _uiTimerForUptime;





        TextView toolTypeName;

        Spinner sprToolType;

        TextView connectionIndicator;

        TextView testIndicator;



        TextView tvConnectionStatus;

        AndroidSwitch switchShowLogs;
        AndroidSwitch switchPauseLogs;
        AndroidSwitch switchDebugger;
        AndroidSwitch switchShowTrace;
        LinearLayout debugPanel;
        LinearLayout logContainer;

        // NOVO: switch programático para abrir a telinha de estatísticas
        AndroidSwitch switchShowStats;

        // REQ para StartActivityForResult
        const int REQ_STATS = 1005;





        ListView lvLog;
        List<string> logItems = new List<string>();
        CustomStringAdapter logAdapter;

        ListView lvResults;
        List<string> resultItems = new List<string>();
        CustomStringAdapter resultAdapter;

        int ResultsCounter = 0;
        int UntighteningsCounter = 0;

        List<DataResult> lastTrace = new List<DataResult>();

        const int FIXED_PORT = 23;

        DbHelper db;

        volatile bool logsEnabled = true;
        volatile bool logsPaused = false;
        volatile bool showTrace = true;

        ToolType selectedToolType = ToolType.ToolType1;
        readonly List<ToolType> toolTypeValues = new List<ToolType>();

        // Dedup
        DateTime lastAddedResultTime = DateTime.MinValue;
        decimal lastAddedTorque = decimal.MinValue;
        decimal lastAddedAngle = decimal.MinValue;

        readonly TimeSpan dedupeTimeWindow = TimeSpan.FromSeconds(2);
        readonly decimal dedupeTorqueThreshold = 0.05M;
        readonly decimal dedupeAngleThreshold = 0.5M;

        readonly int rearmCooldownMs = 800;
        readonly SemaphoreSlim rearmLock = new SemaphoreSlim(1, 1);

        readonly ConcurrentQueue<string> pendingLogs = new ConcurrentQueue<string>();
        volatile bool logFlushScheduled = false;
        readonly int LOG_FLUSH_MS = 250;
        Handler uiHandler;

        DateTime lastToastTime = DateTime.MinValue;
        readonly TimeSpan toastThrottle = TimeSpan.FromSeconds(1);

        const int MAX_LOG_ITEMS = 4000;
        const int MAX_RESULT_ITEMS = 2000;

        Delegate protocolHandlerDelegate = null;
        Type protocolLoggerType = null;

        AlertDialog currentTraceDialog = null;
        int nextTraceDialogId = 0;
        int lastTraceDialogId = 0;

        int er01Count = 0;
        int er02Count = 0;
        int er03Count = 0;
        int er04Count = 0;
        int er04Retries = 0;
        const int ER04_MAX_RETRIES = 5;

        const int REQ_LOCATION = 1001;

        // CONNECTION MONITORING (ADDED)
        DateTime lastComm = DateTime.MinValue; // timestamp do último pacote recebido

        volatile bool lastKnownTestRunning = false; // Para o indicador de ciclo

        private volatile bool isTestRunning = false;

        private volatile bool _connectOperationInProgress = false;





        // handler nomeado para permitir unsubscribe corretamente
        private Action _connectionCounterHandler;
        private bool _connectionCounterSubscribed = false;






        CancellationTokenSource connectionMonitorCts = null;
        
        
        volatile bool lastKnownConnected = false; // estado conhecido pelo monitor


        // ========= NOVO: Estatísticas em tempo real =========
        RealTimeStatistics _rtStats = null; // instanciado em OnCreate para evitar alterações de lógica existentes

        // ========= NOVO: Dialog/Views de Stats =========
        AlertDialog statsDialog = null;
        TextView tvStatsMean = null;
        TextView tvStatsStd = null;
        TextView tvStatsCm = null;
        TextView tvStatsCmk = null;
        Action<RealTimeStatistics.StatsResult> statsHandler = null;











        // ============== TIPOS PARA JULGAMENTO ÚNICO ==============
        enum JudgmentStatus { OK, NOK }

        struct JudgmentLimits
        {
            public decimal ThresholdIni;
            public int TimeoutEnd;
            public decimal MinT;
            public decimal NomT;
            public decimal MaxT;
            public decimal FreqT;
        }

        string GetFriendlyToolTypeName(ToolType toolType)
        {
            var friendlyLabels = new string[]
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



            //var friendlyLabels = new string[]
            //{
                //"Apertadeira eletrônica com cabo",
                //"Apertadeira de impulso",
                //"Torquimetro de estalo",
                //"Torquimetro Digital/Analógico",
                //"Apertadeira pneumática",
                //"Apertadeira à bateria",
                //"Apertadeira à bateria transdutorizada",
                //"Apertadeira embreagem com shutoff",
                //"Vazio Ainda"
            //};



            int index = ((int)toolType) - 1;
            if (index >= 0 && index < friendlyLabels.Length)
                return friendlyLabels[index];

            return toolType.ToString();
        }


        // ==== Leitura única dos limites (com defaults) ====
        // ==== Leitura única dos limites (com defaults) ====
        JudgmentLimits GetCurrentLimitsFromUi()
        {
            // começa SEMPRE dos defaults
            decimal thr = DEFAULT_THRESHOLD_INI;
            int tmo = DEFAULT_TIMEOUT_END;
            decimal min = DEFAULT_MIN_TORQUE;
            decimal nom = DEFAULT_NOM_TORQUE;
            decimal max = DEFAULT_MAX_TORQUE;
            decimal f = DEFAULT_FREQ;



            string thrText = "", tmoText = "", minText = "", nomText = "", maxText = "", fText = "";

            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        thrText = txtThresholdIniFree?.Text?.Trim() ?? "";
                        tmoText = txtTimeoutFree?.Text?.Trim() ?? "";
                        minText = txtMinimumTorque?.Text?.Trim() ?? "";
                        nomText = txtNominalTorque?.Text?.Trim() ?? "";
                        maxText = txtMaximoTorque?.Text?.Trim() ?? "";
                        fText = txtFrequencia?.Text?.Trim() ?? "";

                    }
                    catch { }
                });

                // LOGA o que veio da tela
                QueueLog($"LIMIT TEXTS UI: thr='{thrText}' tmo='{tmoText}' min='{minText}' nom='{nomText}' max='{maxText}'");
                TransducerLogAndroid.LogInfo($"LIMIT TEXTS UI: thr='{thrText}' tmo='{tmoText}' min='{minText}' nom='{nomText}' max='{maxText}'");

                // ThresholdIni
                if (!string.IsNullOrWhiteSpace(thrText))
                {
                    if (!decimal.TryParse(thrText, NumberStyles.Any, CultureInfo.InvariantCulture, out thr))
                        decimal.TryParse(thrText, NumberStyles.Any, CultureInfo.CurrentCulture, out thr);
                }

                // Timeout
                if (!string.IsNullOrWhiteSpace(tmoText))
                    int.TryParse(tmoText, out tmo);

                // Min
                if (!string.IsNullOrWhiteSpace(minText))
                {
                    decimal parsed;
                    if (decimal.TryParse(minText, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) ||
                        decimal.TryParse(minText, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
                    {
                        min = parsed;
                    }
                }

                // Nom
                if (!string.IsNullOrWhiteSpace(nomText))
                {
                    decimal parsed;
                    if (decimal.TryParse(nomText, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) ||
                        decimal.TryParse(nomText, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
                    {
                        nom = parsed;
                    }
                }

                // Max
                if (!string.IsNullOrWhiteSpace(maxText))
                {
                    decimal parsed;
                    if (decimal.TryParse(maxText, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) ||
                        decimal.TryParse(maxText, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
                    {
                        max = parsed;
                    }
                }



                if (!string.IsNullOrWhiteSpace(fText))
                {
                    decimal parsed;
                    if (decimal.TryParse(fText, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed) ||
                        decimal.TryParse(fText, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed))
                    {
                        f = parsed;
                    }
                }







            }
            catch (Exception ex)
            {
                QueueLog("GetCurrentLimitsFromUi error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "GetCurrentLimitsFromUi");
            }

            // Sanitiza
            if (thr < 0) thr = DEFAULT_THRESHOLD_INI;
            if (tmo < 10 || tmo > 10000) tmo = DEFAULT_TIMEOUT_END;

            if (min <= 0) min = DEFAULT_MIN_TORQUE;
            if (nom <= 0) nom = DEFAULT_NOM_TORQUE;
            if (max <= 0) max = DEFAULT_MAX_TORQUE;

            if (min > max)
                QueueLog($"WARN: Limites inconsistentes -> Min({min}) > Max({max})");

            // LOGA o que vai ser usado no julgamento
            QueueLog($"LIMIT VALUES USED: thr={thr} tmo={tmo} min={min} nom={nom} max={max}");
            TransducerLogAndroid.LogInfo($"LIMIT VALUES USED: thr={thr} tmo={tmo} min={min} nom={nom} max={max}");

            return new JudgmentLimits
            {
                ThresholdIni = thr,
                TimeoutEnd = tmo,
                MinT = min,
                NomT = nom,
                MaxT = max,
                FreqT = f
            };
        }



        JudgmentStatus JudgeTorque(decimal torque, JudgmentLimits limits)
        {
            return (torque >= limits.MinT && torque <= limits.MaxT)
                ? JudgmentStatus.OK
                : JudgmentStatus.NOK;
        }



        // ============== OnCreate ====================
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            uiHandler = new Handler(Looper.MainLooper);

            // Licença
            await CheckLicenseAsync();

            // DB + logs
            db = new DbHelper();
            try
            {
                TransducerLogAndroid.Initialize(db, flushIntervalMs: 400, flushBatchSize: 100);
                TransducerLogAndroid.OnLogAppended += TransducerLog_OnLogAppended;
                TransducerLogAndroid.OnBatchPersisted += (n) => { };
            }
            catch (Exception ex)
            {
                QueueLog("TransducerLogAndroid initialize error: " + ex.Message);
                Log.Error("TransducerAppXA/MainActivity", "TransducerLogAndroid initialize error: " + ex);
            }

            EnsureLocationPermission();

            // Bind UI
            txtIP = FindViewById<EditText>(Resource.Id.txtIP);
            txtPort = FindViewById<EditText>(Resource.Id.txtPort);
            btnConnectIP = FindViewById<Button>(Resource.Id.btnConnectIP);
            btnDisconnect = FindViewById<Button>(Resource.Id.btnDisconnect);
            btnInitRead = FindViewById<Button>(Resource.Id.btnInitRead);
            btnStopRead = FindViewById<Button>(Resource.Id.btnStopRead);
            btnCopyLog = FindViewById<Button>(Resource.Id.btnCopyLog);
            btnClearLog = FindViewById<Button>(Resource.Id.btnClearLog);
            btnClearResults = FindViewById<Button>(Resource.Id.btnClearResults);
            btnExportResults = FindViewById<Button>(Resource.Id.btnExportResults);

            btnExportLogs = FindViewById<Button>(Resource.Id.btnExportLogs);

            btnSyncToPc = FindViewById<Button>(Resource.Id.btnSyncToPc);

            btnExportDB = FindViewById<Button>(Resource.Id.btnExportDB);

            btnTestRunner = FindViewById<Button>(Resource.Id.btnOpenRunner);


            btnTestAndroidLayout = FindViewById<Button>(Resource.Id.btnOpenLayoutAndroid);



            tvMainStatsMean = FindViewById<TextView>(Resource.Id.tvMainStatsMean);
            tvMainStatsCm = FindViewById<TextView>(Resource.Id.tvMainStatsCm);
            tvMainStatsCmk = FindViewById<TextView>(Resource.Id.tvMainStatsCmk);





            btnZeroTqAn = FindViewById<Button>(Resource.Id.btnZeroTqAn);

            txtThresholdIniFree = FindViewById<EditText>(Resource.Id.txtThresholdIniFree);
            txtThresholdEndFree = FindViewById<EditText>(Resource.Id.txtThresholdEndFree);
            txtTimeoutFree = FindViewById<EditText>(Resource.Id.txtTimeoutFree);
            txtNominalTorque = FindViewById<EditText>(Resource.Id.txtNominalTorque);
            txtMinimumTorque = FindViewById<EditText>(Resource.Id.txtMinimumTorque);
            txtMaximoTorque = FindViewById<EditText>(Resource.Id.txtMaximoTorque);

            txtFrequencia = FindViewById<EditText>(Resource.Id.txtFrequencia);



            sprToolType = FindViewById<Spinner>(Resource.Id.spinnerToolType);

            tvTorque = FindViewById<TextView>(Resource.Id.tvTorque);
            tvAngle = FindViewById<TextView>(Resource.Id.tvAngle);
            tvResultsCounter = FindViewById<TextView>(Resource.Id.tvResultsCounter);
            //tvUntighteningsCounter = FindViewById<TextView>(Resource.Id.tvUntighteningsCounter);
            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);

            connectionIndicator = FindViewById<TextView>(Resource.Id.connectionIndicator);

            testIndicator = FindViewById<TextView>(Resource.Id.testIndicator);


            tvReconnectCount = FindViewById<TextView>(Resource.Id.tvReconnectCount);
            tvConnUptime = FindViewById<TextView>(Resource.Id.tvConnUptime);




            ///////////////
            ///
            // registra um observer simples (opcional) para atualizar a UI quando o contador mudar
            // cria um handler nomeado para atualizar a UI quando o contador mudar
            _connectionCounterHandler = () =>
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (tvReconnectCount != null)
                            tvReconnectCount.Text = $"Reconnection: {ConnectionCounter.Instance.ReconnectCount}";

                        if (tvConnUptime != null)
                        {
                            var up = ConnectionCounter.Instance.GetUptime();
                            tvConnUptime.Text = $"Uptime: {up:hh\\:mm\\:ss}";
                        }
                    }
                    catch (Exception ex)
                    {
                        QueueLog("ConnectionCounter OnChanged handler error: " + ex.Message);
                    }
                });
            };

            // inscreve somente se ainda não inscrito (evita duplicação)
            if (!_connectionCounterSubscribed)
            {
                ConnectionCounter.Instance.OnChanged += _connectionCounterHandler;
                _connectionCounterSubscribed = true;
            }

            // timer para atualizar uptime a cada 1s enquanto o app aberto
            _uiTimerForUptime = new System.Timers.Timer(1000) { AutoReset = true };
            _uiTimerForUptime.Elapsed += (s, e) =>
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (tvConnUptime != null)
                        {
                            var up = ConnectionCounter.Instance.GetUptime();
                            tvConnUptime.Text = $"Uptime: {up:hh\\:mm\\:ss}";
                        }

                        if (tvReconnectCount != null)
                        {
                            tvReconnectCount.Text = $"Reconnection: {ConnectionCounter.Instance.ReconnectCount}";
                        }
                    }
                    catch (Exception ex)
                    {
                        QueueLog("Uptime timer update error: " + ex.Message);
                    }
                });
            };
            _uiTimerForUptime.Start();

            QueueLog("Uptime UI timer started");
            Android.Util.Log.Info("MainActivity", "Uptime UI timer started");

            ////////////
            ///














            tvConnectionStatus = FindViewById<TextView>(Resource.Id.tvConnectionStatus);

            switchShowLogs = FindViewById<AndroidSwitch>(Resource.Id.switchShowLogs);
            switchPauseLogs = FindViewById<AndroidSwitch>(Resource.Id.switchPauseLogs);
            switchDebugger = FindViewById<AndroidSwitch>(Resource.Id.switchDebugger);
            switchShowTrace = FindViewById<AndroidSwitch>(Resource.Id.switchShowTrace);

            debugPanel = FindViewById<LinearLayout>(Resource.Id.debugPanel);
            logContainer = FindViewById<LinearLayout>(Resource.Id.logContainer);

            SetConnectionIndicator(false);

            SetTestIndicator(false);



            lvLog = FindViewById<ListView>(Resource.Id.lvLog);
            logAdapter = new CustomStringAdapter(this, logItems, Color.ParseColor("#212121"), 13f);
            lvLog.Adapter = logAdapter;

            lvResults = FindViewById<ListView>(Resource.Id.lvResults);
            resultAdapter = new CustomStringAdapter(this, resultItems, Color.ParseColor("#212121"), 14f, bold: true);
            lvResults.Adapter = resultAdapter;

            //////////////////
            ///
            try
            {
                switchShowStats = FindViewById<AndroidSwitch>(Resource.Id.switchShowStats);
                if (switchShowStats != null)
                {
                    // inicial state false
                    switchShowStats.Checked = false;
                    switchShowStats.CheckedChange += (s, e) =>
                    {
                        try
                        {
                            if (e.IsChecked)
                            {
                                // Abre a StatsActivity (usando StartActivityForResult para saber quando fechar)
                                try
                                {
                                    var intent = new Intent(this, typeof(StatsActivity));
                                    StartActivityForResult(intent, REQ_STATS);
                                }
                                catch (Exception ex)
                                {
                                    QueueLog("Start StatsActivity error: " + ex.Message);
                                }
                            }
                            else
                            {
                                // Se o usuário desmarcou a switch enquanto a Activity pode estar aberta,
                                // solicita que a StatsActivity feche (se existir)
                                try
                                {
                                    StatsActivity.RequestClose?.Invoke();
                                }
                                catch { }
                            }
                        }
                        catch { }
                    };
                }
            }
            catch (Exception ex)
            {
                QueueLog("Find switchShowStats error: " + ex.Message);
            }

            ////////////////////////
            ///

            // Inicializa o StatisticsService (idempotente)
            try
            {
                double defaultUSL = (double)DEFAULT_MAX_TORQUE;
                double defaultLSL = (double)DEFAULT_MIN_TORQUE;
                // alterar minSamples para 3
                StatisticsService.Instance.Initialize(windowSize: 200, defaultUSL: defaultUSL, defaultLSL: defaultLSL, sigmaType: RealTimeStatistics.StdDevType.Population, minSamples: 3);

                // Log de callback (opcional)
                StatisticsService.Instance.OnStatisticsUpdated += (r) =>
                {
                    try
                    {
                        string msg = $"RT STATS -> Count={r.Count} Mean={r.Mean:F3} Std={r.StdDev:F4} Cm={(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")} Cmk={(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                        QueueLog(msg);
                        TransducerLogAndroid.LogInfo(msg);
                    }
                    catch { }
                };
            }
            catch (Exception ex)
            {
                QueueLog("RTStats init error: " + ex.Message);
            }






            try
            {
                if (StatisticsService.Instance != null)
                {
                    // já existe evento OnStatisticsUpdated no seu projeto (StatsActivity usa isso).
                    // Se o nome do evento for diferente, ajuste conforme seu StatisticsService.
                    StatisticsService.Instance.OnStatisticsUpdated += Main_StatsUpdatedHandler;
                }
            }
            catch { /* não falhar se algo der errado */ }








            // Popule valores iniciais quando abrir a MainActivity (coloque no OnCreate após a inscrição):
            try
            {
                if (StatisticsService.Instance != null && StatisticsService.Instance.Stats != null)
                {
                    var cur = StatisticsService.Instance.Stats.GetCurrentStats();
                    if (cur.HasValue)
                    {
                        var r = cur.Value;
                        // atualiza de imediato
                        tvMainStatsMean.Text = $"Mean: {r.Mean:F3}";
                        tvMainStatsCm.Text = $"Cm: {(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")}";
                        tvMainStatsCmk.Text = $"Cmk: {(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                    }
                    else
                    {
                        tvMainStatsMean.Text = "Mean: n/a";
                        tvMainStatsCm.Text = "Cm: n/a";
                        tvMainStatsCmk.Text = "Cmk: n/a";
                    }
                }
            }
            catch { /* ignore */ }















            // Defaults visuais = defaults únicos
            if (string.IsNullOrWhiteSpace(txtIP?.Text)) txtIP.Text = "192.168.4.1";
            if (txtThresholdIniFree != null) txtThresholdIniFree.Text = DEFAULT_THRESHOLD_INI.ToString(CultureInfo.InvariantCulture);
            if (txtThresholdEndFree != null) txtThresholdEndFree.Text = (DEFAULT_THRESHOLD_INI / 2M).ToString(CultureInfo.InvariantCulture);
            if (txtTimeoutFree != null) txtTimeoutFree.Text = DEFAULT_TIMEOUT_END.ToString(CultureInfo.InvariantCulture);
            if (txtNominalTorque != null) txtNominalTorque.Text = DEFAULT_NOM_TORQUE.ToString(CultureInfo.InvariantCulture);
            if (txtMinimumTorque != null) txtMinimumTorque.Text = DEFAULT_MIN_TORQUE.ToString(CultureInfo.InvariantCulture);
            if (txtMaximoTorque != null) txtMaximoTorque.Text = DEFAULT_MAX_TORQUE.ToString(CultureInfo.InvariantCulture);

            if (txtFrequencia != null) txtFrequencia.Text = DEFAULT_FREQ.ToString(CultureInfo.InvariantCulture);




            // Eventos

            //btnConnectIP.Click += BtnConnectIP_Click;

            btnConnectIP.Click += BtnConnectToggle_Click;


            btnDisconnect.Click += BtnDisconnect_Click;
            btnInitRead.Click += BtnInitRead_Click;
            btnStopRead.Click += BtnStopRead_Click;
            btnCopyLog.Click += BtnCopyLog_Click;
            btnClearLog.Click += BtnClearLog_Click;
            btnClearResults.Click += BtnClearResults_Click;
            btnZeroTqAn.Click += BtnZeroTqAn_Click;
            btnExportResults.Click += BtnExportResults_Click;
            btnExportLogs.Click += BtnExportLogs_Click;
            btnSyncToPc.Click += BtnSyncToPc_Click;
            btnExportDB.Click += BtnExportDB_Click;

            btnTestRunner.Click += BtnTestRunner_Click;


            //btnTestRunner.Click += BtnTestRunner_Click;

            btnTestAndroidLayout.Click += BtnTestAndroidLayout_Click;



            // Spinner ToolType
            try
            {
                var friendlyLabels = new string[]
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

                var toolOptions = new List<string>();
                toolTypeValues.Clear();

                for (int i = 1; i <= 9; i++)
                {
                    string enumName = $"ToolType{i}";
                    try
                    {
                        var enumValue = (ToolType)Enum.Parse(typeof(ToolType), enumName);
                        string label = (i - 1) < friendlyLabels.Length ? friendlyLabels[i - 1] : enumName;
                        toolTypeValues.Add(enumValue);
                        toolOptions.Add(label);
                    }
                    catch { }
                }

                if (toolOptions.Count == 0)
                {
                    toolOptions.Add(friendlyLabels[0]);
                    toolTypeValues.Add(ToolType.ToolType1);
                }

                var adapterTT = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, toolOptions.ToArray());
                adapterTT.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                sprToolType.Adapter = adapterTT;

                int defaultIndex = 0;
                for (int i = 0; i < toolTypeValues.Count; i++)
                    if (toolTypeValues[i] == ToolType.ToolType1) { defaultIndex = i; break; }
                sprToolType.SetSelection(defaultIndex);

                sprToolType.ItemSelected += (s, e) =>
                {
                    if (e.Position >= 0 && e.Position < toolTypeValues.Count)
                    {
                        selectedToolType = toolTypeValues[e.Position];
                        TransducerLogAndroid.LogParameterChange("ToolType", "", selectedToolType.ToString());
                    }
                };
            }
            catch (Exception ex)
            {
                TransducerLogAndroid.LogException(ex, "MainActivity.OnCreate");
                TransducerLogAndroid.LogError("Spinner population error" + ex.Message);
            }

            // Switch logs
            if (switchShowLogs != null)
            {
                switchShowLogs.Checked = true;
                SetLoggingEnabled(true);
                switchShowLogs.CheckedChange += (s, e) =>
                {
                    if (switchShowLogs.Checked)
                    {
                        logContainer.Visibility = ViewStates.Visible;
                        SetLoggingEnabled(true);
                    }
                    else
                    {
                        logContainer.Visibility = ViewStates.Gone;
                        SetLoggingEnabled(false);
                    }
                };
            }

            if (switchPauseLogs != null)
            {
                switchPauseLogs.Checked = false;
                switchPauseLogs.CheckedChange += (s, e) =>
                {
                    logsPaused = switchPauseLogs.Checked;
                    RunOnUiThread(() =>
                    {
                        try
                        {
                            if (logsPaused)
                            {
                                Toast.MakeText(this, "Logging paused (analysis mode)", ToastLength.Short).Show();
                                TransducerLogAndroid.LogInfo("User PAUSED logging (pause switch ON)");
                            }
                            else
                            {
                                Toast.MakeText(this, "Logging resumed", ToastLength.Short).Show();
                                TransducerLogAndroid.LogInfo("User RESUMED logging (pause switch OFF)");
                            }
                        }
                        catch { }
                    });
                };
            }

            if (switchShowTrace != null)
            {
                switchShowTrace.Checked = false;
                showTrace = false;
                switchShowTrace.CheckedChange += (s, e) => { showTrace = e.IsChecked; };
            }

            if (switchDebugger != null && debugPanel != null)
            {
                try { switchDebugger.Checked = (debugPanel.Visibility == ViewStates.Visible); } catch { }

                switchDebugger.CheckedChange += (s, e) =>
                {
                    RunOnUiThread(() =>
                    {
                        try { debugPanel.Visibility = e.IsChecked ? ViewStates.Visible : ViewStates.Gone; }
                        catch { }
                    });

                    try
                    {
                        var prefs = GetSharedPreferences("TransducerAppXA.Prefs", FileCreationMode.Private);
                        prefs.Edit().PutBoolean("debugger_visible", e.IsChecked).Commit();
                    }
                    catch { }
                };






                try
                {
                    var prefs = GetSharedPreferences("TransducerAppXA.Prefs", FileCreationMode.Private);
                    bool visible = prefs.GetBoolean("debugger_visible", false);
                    debugPanel.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
                    switchDebugger.Checked = visible;
                }
                catch { }

            }


            // ---------- NOVO: Criar switch programaticamente para abrir a telinha de estatísticas ----------
            try
            {
                // Criar o switch apenas se ainda não existir
                if (switchShowStats == null)
                {
                    switchShowStats = new AndroidSwitch(this) { Text = "Show Stats" };
                    switchShowStats.Checked = false;
                    switchShowStats.CheckedChange += (s, e) =>
                    {
                        try
                        {
                            if (switchShowStats.Checked)
                                ShowStatsDialog();
                            else
                                HideStatsDialog();
                        }
                        catch { }
                    };

                    // Adiciona ao debugPanel (se existir), senão no logContainer, senão adiciona no root content
                    if (debugPanel != null)
                    {
                        debugPanel.AddView(switchShowStats);
                    }
                    else if (logContainer != null)
                    {
                        logContainer.AddView(switchShowStats);
                    }
                    else
                    {
                        try
                        {
                            var root = FindViewById(Android.Resource.Id.Content) as ViewGroup;
                            root?.AddView(switchShowStats);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                QueueLog("Create ShowStats switch error: " + ex.Message);
            }

            // ---------- FIM: Criar switch programaticamente para abrir a telinha de estatísticas ----------





            TrySubscribeProtocolLogger();




            // Inicializa StatisticsService o mais cedo possível (idempotente)
            try
            {
                double defaultUSL = (double)DEFAULT_MAX_TORQUE;
                double defaultLSL = (double)DEFAULT_MIN_TORQUE;
                StatisticsService.Instance.Initialize(windowSize: 200, defaultUSL: defaultUSL, defaultLSL: defaultLSL, sigmaType: RealTimeStatistics.StdDevType.Population, minSamples: 6);

                // Log de callback (opcional)
                StatisticsService.Instance.OnStatisticsUpdated += (r) =>
                {
                    try
                    {
                        string msg = $"RT STATS -> Count={r.Count} Mean={r.Mean:F3} Std={r.StdDev:F4} Cm={(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")} Cmk={(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                        QueueLog(msg);
                        TransducerLogAndroid.LogInfo(msg);
                    }
                    catch { }
                };
            }
            catch (Exception ex)
            {
                QueueLog("RTStats init error: " + ex.Message);
            }














            _ = LoadDataFromDbAsync();

            ScheduleLogFlush();
            QueueLog("App initialized (diagnostic entries added).");
            TransducerLogAndroid.LogInfo("App initialized (diagnostic entries added).");



            ///////////////////////
            ///
            // Inicializa RealTimeStatistics (uso simples: janela 200, limites default dos constants)
            try
            {
                try
                {
                    double defaultUSL = (double)DEFAULT_MAX_TORQUE;
                    double defaultLSL = (double)DEFAULT_MIN_TORQUE;

                    _rtStats = new RealTimeStatistics(windowSize: 200, defaultUSL: defaultUSL, defaultLSL: defaultLSL);


                    _rtStats.MinSamplesForStats = 6;
                    _rtStats.SigmaType = RealTimeStatistics.StdDevType.Population;
                    _rtStats.OnStatisticsUpdated += (r) =>
                    {
                        try
                        {
                            string msg = $"RT STATS -> Count={r.Count} Mean={r.Mean:F3} Std={r.StdDev:F4} Cm={(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")} Cmk={(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                            QueueLog(msg);
                            TransducerLogAndroid.LogInfo(msg);
                        }
                        catch { }
                    };
                }
                catch (Exception ex)
                {
                    QueueLog("RTStats init error: " + ex.Message);
                }
            }
            catch { }

            // Start the connection monitor (checks every 2 seconds)
            StartConnectionMonitor();
        }

        /// <summary>
        /// Abre um diálogo simples com as estatísticas em tempo real e faz subscribe ao evento do _rtStats.
        /// </summary>
        void ShowStatsDialog()
        {
            try
            {
                // usa o singleton (em vez de _rtStats local)
                if (StatisticsService.Instance == null || StatisticsService.Instance.Stats == null)
                {
                    QueueLog("ShowStatsDialog: StatisticsService not initialized");
                    return;
                }

                // Se já estiver exibido, apenas retorna
                if (statsDialog != null && statsDialog.IsShowing) return;

                var builder = new AlertDialog.Builder(this);
                builder.SetTitle("Real-time Statistics");

                var scroll = new ScrollView(this);
                var container = new LinearLayout(this) { Orientation = Orientation.Vertical };
                int pad = (int)(8 * Resources.DisplayMetrics.Density);
                container.SetPadding(pad, pad, pad, pad);
                scroll.AddView(container);

                var lbl = new TextView(this);
                lbl.Text = "Real-time statistics (window preview)";
                lbl.SetTypeface(lbl.Typeface, TypefaceStyle.Bold);
                lbl.SetTextColor(Color.ParseColor("#212121"));
                lbl.SetTextSize(ComplexUnitType.Sp, 16f);
                lbl.Gravity = GravityFlags.CenterHorizontal;
                container.AddView(lbl);

                // Espaço
                var spacer = new View(this);
                spacer.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, (int)(6 * Resources.DisplayMetrics.Density));
                container.AddView(spacer);

                tvStatsMean = new TextView(this) { Text = "Mean: n/a" };
                tvStatsStd = new TextView(this) { Text = "StdDev: n/a" };
                tvStatsCm = new TextView(this) { Text = "Cm: n/a" };
                tvStatsCmk = new TextView(this) { Text = "Cmk: n/a" };

                tvStatsMean.SetTextSize(ComplexUnitType.Sp, 14f);
                tvStatsStd.SetTextSize(ComplexUnitType.Sp, 14f);
                tvStatsCm.SetTextSize(ComplexUnitType.Sp, 14f);
                tvStatsCmk.SetTextSize(ComplexUnitType.Sp, 14f);

                container.AddView(tvStatsMean);
                container.AddView(tvStatsStd);
                container.AddView(tvStatsCm);
                container.AddView(tvStatsCmk);

                builder.SetView(scroll);
                builder.SetPositiveButton("Close", (s, e) =>
                {
                    try { if (switchShowStats != null) switchShowStats.Checked = false; } catch { }
                });

                statsDialog = builder.Create();
                statsDialog.SetCanceledOnTouchOutside(true);

                // Handler que atualiza as TextViews (guardamos para remover a inscrição)
                statsHandler = (r) =>
                {
                    try
                    {
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                if (tvStatsMean != null) tvStatsMean.Text = $"Mean: {r.Mean:F3}";
                                if (tvStatsStd != null) tvStatsStd.Text = $"StdDev: {r.StdDev:F4}";
                                if (tvStatsCm != null) tvStatsCm.Text = $"Cm: {(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")}";
                                if (tvStatsCmk != null) tvStatsCmk.Text = $"Cmk: {(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                            }
                            catch { }
                        });
                    }
                    catch { }
                };

                // Inscreve no evento do StatisticsService (não no _rtStats local)
                try
                {
                    StatisticsService.Instance.OnStatisticsUpdated += statsHandler;
                }
                catch (Exception ex)
                {
                    QueueLog("ShowStatsDialog: subscribe error: " + ex.Message);
                }

                // Preenche os valores iniciais imediatamente (se houver estatísticas já calculadas)
                try
                {
                    var current = StatisticsService.Instance.Stats.GetCurrentStats();
                    if (current.HasValue)
                    {
                        var r = current.Value;
                        // atualiza UI imediatamente
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                if (tvStatsMean != null) tvStatsMean.Text = $"Mean: {r.Mean:F3}";
                                if (tvStatsStd != null) tvStatsStd.Text = $"StdDev: {r.StdDev:F4}";
                                if (tvStatsCm != null) tvStatsCm.Text = $"Cm: {(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")}";
                                if (tvStatsCmk != null) tvStatsCmk.Text = $"Cmk: {(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";
                            }
                            catch { }
                        });
                    }
                }
                catch (Exception ex)
                {
                    QueueLog("ShowStatsDialog: GetCurrentStats error: " + ex.Message);
                }

                statsDialog.DismissEvent += (s, e) =>
                {
                    // Ao dismiss, limpa assinatura e referencia; garante que switch seja desmarcado
                    HideStatsDialog();
                    RunOnUiThread(() => { try { if (switchShowStats != null) switchShowStats.Checked = false; } catch { } });
                };

                statsDialog.Show();
            }
            catch (Exception ex)
            {
                QueueLog("ShowStatsDialog error: " + ex.Message);
            }
        }



        /// <summary>
        /// Fecha o diálogo (se aberto) e remove a inscrição no evento.
        /// </summary>
        void HideStatsDialog()
        {
            try
            {
                if (statsDialog != null)
                {
                    try { if (statsDialog.IsShowing) statsDialog.Dismiss(); } catch { }
                    statsDialog = null;
                }

                if (statsHandler != null)
                {
                    try
                    {
                        // Remove a inscrição do singleton
                        StatisticsService.Instance.OnStatisticsUpdated -= statsHandler;
                    }
                    catch { }
                    statsHandler = null;
                }

                tvStatsMean = null;
                tvStatsStd = null;
                tvStatsCm = null;
                tvStatsCmk = null;
            }
            catch (Exception ex)
            {
                QueueLog("HideStatsDialog error: " + ex.Message);
            }
        }





        void Main_StatsUpdatedHandler(RealTimeStatistics.StatsResult r)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        // Atualiza textos
                        tvMainStatsMean.Text = $"Mean: {r.Mean:F3}";
                        tvMainStatsCm.Text = $"Cm: {(r.Cm.HasValue ? r.Cm.Value.ToString("F3") : "n/a")}";
                        tvMainStatsCmk.Text = $"Cmk: {(r.Cmk.HasValue ? r.Cmk.Value.ToString("F3") : "n/a")}";

                        // Opcional: colorir quando houver valores válidos (ex.: cima/baixo)
                        // (comentado — active se quiser)
                        // if (r.Cmk.HasValue && r.Cmk.Value >= 1.0) tvMainStatsCmk.SetTextColor(Color.ParseColor("#2E7D32"));
                        // else tvMainStatsCmk.SetTextColor(Color.ParseColor("#D32F2F"));
                    }
                    catch { }
                });
            }
            catch { }
        }










        // Start the connection monitor (checks every 2 seconds)
        //StartConnectionMonitor();
        //}


        // Quando a StatsActivity for fechada, OnActivityResult é chamado (StartActivityForResult)
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            try
            {
                if (requestCode == REQ_STATS)
                {
                    // Garantir que a switch reflita que a Activity foi fechada
                    try { if (switchShowStats != null) switchShowStats.Checked = false; } catch { }
                }
            }
            catch { }
        }












        #region LICENÇA

        private async Task CheckLicenseAsync()
        {
            try
            {
                ISharedPreferences prefs = GetSharedPreferences(PrefsName, FileCreationMode.Private);
                string storedLicense = prefs.GetString(PrefsKeyLicense, string.Empty);

                string keyCode = GenerateKeyCode();
                bool licenseOk = false;

                if (!string.IsNullOrWhiteSpace(storedLicense))
                {
                    licenseOk = ValidateLicense(keyCode, storedLicense);
                }

                if (!licenseOk)
                {
                    await ShowLicenseDialogAsync(keyCode);
                }
            }
            catch (Exception ex)
            {
                Log.Error("TransducerAppXA/Lic", "License check error: " + ex);
            }
        }

        private string GenerateKeyCode()
        {
            try
            {
                string source;

                string androidId = Settings.Secure.GetString(ContentResolver, Settings.Secure.AndroidId);
                if (!string.IsNullOrEmpty(androidId))
                    source = androidId;
                else
                    
                    
                    //source = Build.Serial ?? "UNKNOWN_DEVICE";

                    source = androidId;



                using (var md5 = MD5.Create())
                {
                    byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(source));
                    var sb = new StringBuilder();
                    foreach (byte b in data)
                        sb.Append(b.ToString("X2"));

                    string fullHash = sb.ToString();
                    if (fullHash.Length >= LicenseLength)
                        return fullHash.Substring(0, LicenseLength).ToUpperInvariant();
                    else
                        return fullHash.ToUpperInvariant().PadRight(LicenseLength, 'X');
                }
            }
            catch
            {
                return "ABCDE";
            }
        }

        private string GenerateLicenseFromKey(string keyCode)
        {
            if (string.IsNullOrEmpty(keyCode))
                keyCode = "ABCDE";

            keyCode = keyCode.Trim().ToUpperInvariant();
            if (keyCode.Length > LicenseLength)
                keyCode = keyCode.Substring(0, LicenseLength);

            char[] chars = keyCode.ToCharArray();
            Array.Reverse(chars);

            for (int i = 0; i < chars.Length; i++)
            {
                char ch = chars[i];

                if (ch >= '0' && ch <= '8')
                    ch = (char)(ch + 1);
                else if (ch == '9')
                    ch = 'A';
                else if (ch >= 'A' && ch <= 'Y')
                    ch = (char)(ch + 1);
                else if (ch == 'Z')
                    ch = '0';
                else
                    ch = '0';

                chars[i] = ch;
            }

            string license = new string(chars).ToUpperInvariant();
            if (license.Length > LicenseLength)
                license = license.Substring(0, LicenseLength);

            return license;
        }

        private bool ValidateLicense(string keyCode, string license)
        {
            if (string.IsNullOrWhiteSpace(license))
                return false;

            string expected = GenerateLicenseFromKey(keyCode);
            return string.Equals(expected, license.Trim().ToUpperInvariant(), StringComparison.Ordinal);
        }

        private Task ShowLicenseDialogAsync(string keyCode)
        {
            var tcs = new TaskCompletionSource<bool>();

            RunOnUiThread(() =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Licença necessária");

                LinearLayout layout = new LinearLayout(this)
                {
                    Orientation = Orientation.Vertical
                };
                int pad = (int)(16 * Resources.DisplayMetrics.Density);
                layout.SetPadding(pad, pad, pad, pad);

                TextView tvMsg = new TextView(this);
                tvMsg.Text = "Este dispositivo ainda não possui licença.\n\n" +
                             "Informe a licença fornecida pelo suporte.\n\n" +
                             "KeyCode deste aparelho:";
                layout.AddView(tvMsg);

                TextView tvKeyCode = new TextView(this);
                tvKeyCode.Text = keyCode;
                tvKeyCode.TextSize = 20;
                tvKeyCode.SetTypeface(tvKeyCode.Typeface, TypefaceStyle.Bold);
                tvKeyCode.SetPadding(0, pad / 2, 0, pad);
                layout.AddView(tvKeyCode);

                EditText txtLicense = new EditText(this);
                txtLicense.Hint = "Licença (5 caracteres)";
                txtLicense.InputType = Android.Text.InputTypes.TextVariationVisiblePassword;
                txtLicense.SetMaxLines(1);
                layout.AddView(txtLicense);

                builder.SetView(layout);
                builder.SetCancelable(false);

                builder.SetPositiveButton("VALIDAR", (s, e) =>
                {
                    string entered = txtLicense.Text?.Trim().ToUpperInvariant() ?? "";
                    if (entered.Length != LicenseLength || !ValidateLicense(keyCode, entered))
                    {
                        Toast.MakeText(this, "Licença inválida. Tente novamente.", ToastLength.Long).Show();
                        _ = ShowLicenseDialogAsync(keyCode);
                    }
                    else
                    {
                        ISharedPreferences prefs = GetSharedPreferences(PrefsName, FileCreationMode.Private);
                        using (var editor = prefs.Edit())
                        {
                            editor.PutString(PrefsKeyLicense, entered);
                            editor.Commit();
                        }

                        Toast.MakeText(this, "Licença aceita.", ToastLength.Short).Show();
                        tcs.TrySetResult(true);
                    }
                });

                builder.SetNegativeButton("SAIR", (s, e) =>
                {
                    Toast.MakeText(this, "Aplicativo fechado. Licença não informada.", ToastLength.Short).Show();
                    tcs.TrySetResult(false);
                    FinishAffinity();
                });

                AlertDialog dialog = builder.Create();
                dialog.Show();
            });

            return tcs.Task;
        }

        #endregion

        // ============== PERMISSÃO LOCALIZAÇÃO ==============
        void EnsureLocationPermission()
        {
            try
            {
                if ((int)Build.VERSION.SdkInt < 23) return;
                if (CheckSelfPermission(Android.Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(new string[] { Android.Manifest.Permission.AccessFineLocation }, REQ_LOCATION);
                }
                else
                {
                    QueueLog("Location permission already granted (required for Wi-Fi APIs).");
                }
            }
            catch (Exception ex) { QueueLog("EnsureLocationPermission error: " + ex.Message); }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            try
            {
                if (requestCode == REQ_LOCATION)
                {
                    if (grantResults != null && grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                    {
                        QueueLog("User granted location permission (Wi‑Fi operations allowed).");
                        TransducerLogAndroid.LogInfo("User granted location permission (Wi‑Fi operations allowed).");
                    }
                    else
                    {
                        QueueLog("User denied location permission — some Wi‑Fi features may not work.");
                        TransducerLogAndroid.LogInfo("User denied location permission — some Wi‑Fi features may not work.");
                    }
                }
            }
            catch { }
        }











        bool TryLoadParamsFromUi()
        {
            try
            {
                string thrText = "", tmoText = "", minText = "", nomText = "", maxText = "", freq ="";

                RunOnUiThread(() =>
                {
                    try
                    {
                        thrText = txtThresholdIniFree?.Text?.Trim() ?? "";
                        tmoText = txtTimeoutFree?.Text?.Trim() ?? "";
                        minText = txtMinimumTorque?.Text?.Trim() ?? "";
                        nomText = txtNominalTorque?.Text?.Trim() ?? "";
                        maxText = txtMaximoTorque?.Text?.Trim() ?? "";
                        freq = txtFrequencia?.Text?.Trim() ?? "";
                    }
                    catch { }
                });

                // Defaults
                _currentThresholdIni = DEFAULT_THRESHOLD_INI;
                _currentTimeoutEnd = DEFAULT_TIMEOUT_END;
                _currentMinT = DEFAULT_MIN_TORQUE;
                _currentNomT = DEFAULT_NOM_TORQUE;
                _currentMaxT = DEFAULT_MAX_TORQUE;
                _currentFreq =  DEFAULT_FREQ;

                // Parse simples; se funcionar, sobrescreve defaults
                decimal tmpDec;
                int tmpInt;

                if (!string.IsNullOrWhiteSpace(thrText) &&
                    (decimal.TryParse(thrText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(thrText, NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentThresholdIni = tmpDec;

                if (!string.IsNullOrWhiteSpace(tmoText) &&
                    int.TryParse(tmoText, out tmpInt))
                    _currentTimeoutEnd = tmpInt;

                if (!string.IsNullOrWhiteSpace(minText) &&
                    (decimal.TryParse(minText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(minText, NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentMinT = tmpDec;

                if (!string.IsNullOrWhiteSpace(nomText) &&
                    (decimal.TryParse(nomText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(nomText, NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentNomT = tmpDec;

                if (!string.IsNullOrWhiteSpace(maxText) &&
                    (decimal.TryParse(maxText, NumberStyles.Any, CultureInfo.InvariantCulture, out tmpDec) ||
                     decimal.TryParse(maxText, NumberStyles.Any, CultureInfo.CurrentCulture, out tmpDec)))
                    _currentMaxT = tmpDec;


                if (!string.IsNullOrWhiteSpace(freq) &&
                    (int.TryParse(freq, out tmpInt) ||
                     int.TryParse(freq, out tmpInt)))
                    _currentFreq = tmpInt;



                // Sanitiza mínimos básicos
                if (_currentThresholdIni < 0) _currentThresholdIni = DEFAULT_THRESHOLD_INI;
                if (_currentTimeoutEnd < 10 || _currentTimeoutEnd > 10000) _currentTimeoutEnd = DEFAULT_TIMEOUT_END;

                if (_currentMinT <= 0) _currentMinT = DEFAULT_MIN_TORQUE;
                if (_currentNomT <= 0) _currentNomT = DEFAULT_NOM_TORQUE;
                if (_currentMaxT <= 0) _currentMaxT = DEFAULT_MAX_TORQUE;

                QueueLog($"PARAMS LOADED: thr={_currentThresholdIni} tmo={_currentTimeoutEnd} min={_currentMinT} nom={_currentNomT} max={_currentMaxT}");
                _paramsLoaded = true;
                return true;
            }
            catch (Exception ex)
            {
                QueueLog("TryLoadParamsFromUi error: " + ex.Message);
                _paramsLoaded = false;
                return false;
            }
        }



        void ResetCachedParams()
        {
            _paramsLoaded = false;
        }







        // ============== LOG / DB ==============
        void QueueLog(string s)
        {
            try
            {
                if (!logsEnabled) return;
                if (logsPaused) return;

                var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {s}";
                pendingLogs.Enqueue(line);
                if (pendingLogs.Count > 20000) pendingLogs.TryDequeue(out _);
                ScheduleLogFlush();
            }
            catch { }
        }

        void ScheduleLogFlush()
        {
            if (logFlushScheduled) return;
            logFlushScheduled = true;
            uiHandler.PostDelayed(new Java.Lang.Runnable(() =>
            {
                try { FlushPendingLogs(); } finally { logFlushScheduled = false; }
            }), LOG_FLUSH_MS);
        }

        void FlushPendingLogs()
        {
            int batch = 200;
            int processed = 0;
            while (processed < batch && pendingLogs.TryDequeue(out var line))
            {
                if (logsPaused) continue;

                logItems.Insert(0, line);
                processed++;

                if (logsEnabled)
                {
                    var logToSave = new LogEntry { TimestampUtc = DateTime.UtcNow, Message = line };
                    Task.Run(() => { try { db.InsertLog(logToSave); } catch { } });
                }
            }

            if (logItems.Count > MAX_LOG_ITEMS) logItems.RemoveRange(MAX_LOG_ITEMS, logItems.Count - MAX_LOG_ITEMS);

            RunOnUiThread(() =>
            {
                try
                {
                    logAdapter.NotifyDataSetChanged();
                    lvLog.InvalidateViews();
                    lvLog.RequestLayout();
                }
                catch { }
            });

            if (!pendingLogs.IsEmpty) ScheduleLogFlush();
        }

        void QueueResultItem(string s)
        {
            try
            {
                // atualiza UI imediatamente
                RunOnUiThread(() =>
                {
                    resultItems.Insert(0, s);
                    if (resultItems.Count > MAX_RESULT_ITEMS) resultItems.RemoveRange(MAX_RESULT_ITEMS, resultItems.Count - MAX_RESULT_ITEMS);
                    resultAdapter.NotifyDataSetChanged();
                    lvResults.InvalidateViews();
                    try { lvResults.SmoothScrollToPosition(0); } catch { }
                });

                // Não gravar no DB aqui. A gravação de Result + Trace será
                // feita explicitamente no fluxo que conhece os pontos (ex: TesteResultReceiver),
                // evitando duplicação e inconsistência (arredondamento vs precisão).
            }
            catch { }
        }













        // Substitua o método LoadDataFromDbAsync pelo bloco abaixo no MainActivity.cs
        async Task LoadDataFromDbAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var recentResults = db.GetRecentResults(500);
                        var recentLogs = db.GetRecentLogs(1000);

                        RunOnUiThread(() =>
                        {
                            resultItems.Clear();
                            foreach (var r in recentResults)
                            {
                                resultItems.Add(r.Text ?? $"{r.TimestampUtc:yyyy-MM-dd HH:mm:ss}  {r.Torque:F3} Nm  {r.Angle:F2} º");
                            }
                            resultAdapter.NotifyDataSetChanged();
                            try { lvResults.SmoothScrollToPosition(0); } catch { }

                            logItems.Clear();
                            foreach (var l in recentLogs)
                            {
                                logItems.Add($"{l.TimestampUtc:yyyy-MM-dd HH:mm:ss.fff} - {l.Message}");
                            }
                            logAdapter.NotifyDataSetChanged();
                        });

                        // --- SEMENTE: alimentar StatisticsService apenas com os valores salvos no DB ---
                        try
                        {
                            if (StatisticsService.Instance != null && StatisticsService.Instance.Stats != null && recentResults != null)
                            {
                                double usl = (double)DEFAULT_MAX_TORQUE;
                                double lsl = (double)DEFAULT_MIN_TORQUE;
                                double nominal = (double)DEFAULT_NOM_TORQUE;

                                // Reset antes de semear para evitar duplicação caso LoadDataFromDbAsync seja chamado mais de uma vez
                                StatisticsService.Instance.Reset();

                                // monta lista de torques (em ordem cronológica)
                                var torqueList = new List<double>();
                                string lastText = null;
                                foreach (var r in recentResults)
                                {
                                    try
                                    {
                                        double torqueVal = Convert.ToDouble(r.Torque);
                                        torqueList.Add(torqueVal);
                                        lastText = r.Text ?? lastText;
                                    }
                                    catch { }
                                }

                                // Semente e também informa texto do último resultado (para tvLastResult)
                                StatisticsService.Instance.SeedFromValues(torqueList, upperSpecLimit: usl, lowerSpecLimit: lsl, nominal: nominal, lastResultText: lastText);

                                QueueLog($"Stats seeded with {(recentResults?.Count ?? 0)} results from DB.");
                            }
                        }
                        catch (Exception ex)
                        {
                            QueueLog("Error seeding stats from DB: " + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        QueueLog("LoadDataFromDbAsync error: " + ex.Message);
                    }
                }).ConfigureAwait(false);
            }
            catch (Exception ex) { QueueLog("LoadDataFromDbAsync outer error: " + ex.Message); }
        }


















        void SetLoggingEnabled(bool enabled)
        {
            logsEnabled = enabled;
            RunOnUiThread(() =>
            {
                try
                {
                    if (enabled) Toast.MakeText(this, "Logging enabled", ToastLength.Short).Show();
                    else Toast.MakeText(this, "Logging disabled (new logs will be ignored)", ToastLength.Short).Show();
                }
                catch { }
            });

            if (!enabled)
            {
                try { while (pendingLogs.TryDequeue(out _)) { } } catch { }
            }
        }

        // ============== CONEXÃO / TCP ==============
        async Task TryBindToActiveWifiAsync0()
        {
            try
            {
                var cm = (ConnectivityManager)GetSystemService(ConnectivityService);
                if (cm == null)
                {
                    QueueLog("TryBind: ConnectivityManager null");
                    return;
                }

                var active = cm.ActiveNetwork;
                if (active == null)
                {
                    QueueLog("TryBind: no active network");
                    return;
                }

                var caps = cm.GetNetworkCapabilities(active);
                if (caps == null)
                {
                    QueueLog("TryBind: network capabilities null");
                    return;
                }

                if (caps.HasTransport(TransportType.Wifi))
                {
                    try
                    {
                        bool ok = ConnectivityManager.SetProcessDefaultNetwork(active);
                        QueueLog("TryBind: bound process to active Wi‑Fi network: " + ok);
                        TransducerLogAndroid.LogInfo("TryBind: bound process to active Wi‑Fi network: {0}", ok);
                    }
                    catch (Exception ex)
                    {
                        QueueLog("TryBind error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "TryBindToActiveWifiAsync");
                    }
                }
                else
                {
                    QueueLog("TryBind: active network transport is not Wi‑Fi");
                }
            }
            catch (Exception ex) { QueueLog("TryBindToActiveWifiAsync error: " + ex.Message); }
        }


        // Substitua o método antigo por este
        Task TryBindToActiveWifiAsync()
        {
            try
            {
                var cm = (ConnectivityManager)GetSystemService(ConnectivityService);
                if (cm == null)
                {
                    QueueLog("TryBind: ConnectivityManager null");
                    return Task.CompletedTask;
                }

                var active = cm.ActiveNetwork;
                if (active == null)
                {
                    QueueLog("TryBind: no active network");
                    return Task.CompletedTask;
                }

                var caps = cm.GetNetworkCapabilities(active);
                if (caps == null)
                {
                    QueueLog("TryBind: network capabilities null");
                    return Task.CompletedTask;
                }

                if (caps.HasTransport(TransportType.Wifi))
                {
                    try
                    {
                        bool ok = false;

                        // Use BindProcessToNetwork (instância) em API >= 23 (M)
                        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                        {
                            try
                            {
                                ok = cm.BindProcessToNetwork(active);
                            }
                            catch (Exception exBind)
                            {
                                // log e tentar fallback
                                QueueLog("TryBind: BindProcessToNetwork failed: " + exBind.Message);
                                try
                                {
                                    // Fallback para API antiga (obsoleta) — suprimimos o warning localmente
#pragma warning disable CS0618
                                    ok = ConnectivityManager.SetProcessDefaultNetwork(active);
#pragma warning restore CS0618
                                }
                                catch (Exception ex2)
                                {
                                    QueueLog("TryBind fallback SetProcessDefaultNetwork failed: " + ex2.Message);
                                    ok = false;
                                }
                            }
                        }
                        else
                        {
                            // versões antigas: usa a API estática (obsoleta) com supressão de warning
                            try
                            {
#pragma warning disable CS0618
                                ok = ConnectivityManager.SetProcessDefaultNetwork(active);
#pragma warning restore CS0618
                            }
                            catch (Exception ex)
                            {
                                QueueLog("TryBind: SetProcessDefaultNetwork failed: " + ex.Message);
                                ok = false;
                            }
                        }

                        QueueLog("TryBind: bound process to active Wi‑Fi network: " + ok);
                        TransducerLogAndroid.LogInfo("TryBind: bound process to active Wi‑Fi network: {0}", ok);
                    }
                    catch (Exception ex)
                    {
                        QueueLog("TryBind error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "TryBindToActiveWifiAsync");
                    }
                }
                else
                {
                    QueueLog("TryBind: active network transport is not Wi‑Fi");
                }
            }
            catch (Exception ex)
            {
                QueueLog("TryBindToActiveWifiAsync error: " + ex.Message);
            }

            return Task.CompletedTask;
        }






        void SetConnectionIndicator(bool connected)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    if (connectionIndicator != null)
                    {
                        connectionIndicator.Text = "●";
                        if (connected)
                        {
                            connectionIndicator.SetTextColor(Color.ParseColor("#4CAF50"));
                            if (tvConnectionStatus != null) tvConnectionStatus.Text = "Connected";
                        }
                        else
                        {
                            connectionIndicator.SetTextColor(Color.ParseColor("#F44336"));
                            if (tvConnectionStatus != null) tvConnectionStatus.Text = "Disconnected";
                        }
                    }
                });
            }
            catch { }
        }



        void SetTestIndicator(bool running)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (testIndicator == null) return;

                        // sempre exibe o mesmo símbolo (bolinha)
                        testIndicator.Text = "●";

                        if (running)
                        {
                            // verde = teste em execução
                            testIndicator.SetTextColor(Color.ParseColor("#4CAF50")); // verde
                                                                                     // opcional: accessibility / tooltip
                            if (tvStatus != null) tvStatus.Text = "Status: Test running";
                        }
                        else
                        {
                            // vermelho = teste parado
                            testIndicator.SetTextColor(Color.ParseColor("#F44336")); // vermelho
                            if (tvStatus != null) tvStatus.Text = "Status: Test stopped";
                        }
                    }
                    catch { /* não deixar quebrar UI */ }
                });
            }
            catch { }
        }



        void SetTestIndicator2(bool running) //checar
        {
            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (testIndicator == null) return;
                        testIndicator.Text = "●";
                        if (running)
                        {
                            testIndicator.SetTextColor(Color.ParseColor("#4CAF50")); // verde
                        }
                        else
                        {
                            testIndicator.SetTextColor(Color.ParseColor("#F44336")); // vermelho
                        }
                    }
                    catch { }
                });
            }
            catch { }
        }








        void SetConnectionConnecting()
        {
            try
            {
                RunOnUiThread(() =>
                {
                    if (connectionIndicator != null)
                    {
                        connectionIndicator.Text = "●";
                        connectionIndicator.SetTextColor(Color.ParseColor("#FF9800"));
                        if (tvConnectionStatus != null) tvConnectionStatus.Text = "Connecting";
                    }
                });
            }
            catch { }
        }

        void TrySubscribeProtocolLogger()
        {
            try
            {
                protocolLoggerType = Type.GetType("ProtocolFileLogger")
                    ?? Type.GetType("Transducer_Estudo.ProtocolFileLogger, Transducer_Estudo")
                    ?? Type.GetType("ProtocolFileLogger, Transducer_Estudo");

                if (protocolLoggerType != null)
                {
                    var ev = protocolLoggerType.GetEvent("OnLogWritten", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (ev != null)
                    {
                        Action<string, string, byte[]> handler = ProtocolFileLogger_OnLogWritten;
                        protocolHandlerDelegate = handler;
                        ev.AddEventHandler(null, handler);
                        QueueLog("DIAG: Subscribed to ProtocolFileLogger.OnLogWritten (reflection).");
                        return;
                    }
                    else QueueLog("DIAG: ProtocolFileLogger found but OnWritten event not present.");
                }
                else QueueLog("DIAG: ProtocolFileLogger type not found (driver may not expose it).");
            }
            catch (Exception ex) { QueueLog("DIAG: TrySubscribeProtocolLogger error: " + ex.Message); }
        }

        void ProtocolFileLogger_OnLogWritten(string direction, string text, byte[] raw)
        {
            try
            {
                // update last communication timestamp
                lastComm = DateTime.Now;

                //SetTestIndicator(true); //aqui indicador de ciclo ativo



                var sb = new StringBuilder();
                sb.AppendFormat("[{0}] {1}", direction ?? "LOG", text ?? "");
                if (raw != null && raw.Length > 0)
                {
                    sb.Append(" | HEX: ");
                    sb.Append(ByteArrayToHex(raw));
                }

                QueueLog("PROTO: " + sb.ToString());
                try { TransducerLogAndroid.LogRaw("PROTO", sb.ToString(), raw); } catch { }
            }
            catch (Exception ex) { QueueLog("PROTO HANDLER ERROR: " + ex.Message); }
        }

        static string ByteArrayToHex(byte[] bytes)
        {
            if (bytes == null) return null;
            var sb = new StringBuilder(bytes.Length * 3);
            foreach (var b in bytes) sb.AppendFormat("{0:X2} ", b);
            return sb.ToString().Trim();
        }

        // ============== BOTÕES ==============
        private void BtnConnectIP_Click(object sender, EventArgs e)
        {
            try
            {
                string ip = txtIP.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(ip))
                {
                    QueueLog("Connect: IP empty");
                    if (DateTime.Now - lastToastTime > toastThrottle)
                    {
                        lastToastTime = DateTime.Now;
                        Toast.MakeText(this, "Informe IP válido", ToastLength.Short).Show();
                    }
                    return;
                }

                QueueLog($"Connect -> {ip}:{FIXED_PORT}");

                Task.Run(async () =>
                {
                    try
                    {
                        await TryBindToActiveWifiAsync().ConfigureAwait(false);
                    }
                    catch { }
                });

                if (Trans != null)
                {
                    try { Trans.StopReadData(); Trans.StopService(); } catch { }
                    Trans = null;
                }

                Trans = new PhoenixTransducer();

                Trans.bPrintCommToFile = true;

                Trans.DataResult += ResultReceiver;
                Trans.TesteResult += TesteResultReceiver;
                Trans.DataInformation += DataInformationReceiver;
                Trans.DebugInformation += DebugInformationReceiver;
                try { Trans.RaiseError += TransducerErrorReceiver; } catch { QueueLog("DIAG: RaiseError subscribe failed"); }

                Trans.SetPerformance(ePCSpeed.Medium, eCharPoints.VeryFew);
                Trans.Eth_IP = ip;
                Trans.Eth_Port = FIXED_PORT;

                Task.Run(() =>
                {
                    try
                    {
                        Trans.StartService();
                        Thread.Sleep(50);
                        Trans.StartCommunication();
                        Trans.RequestInformation();
                        QueueLog("StartCommunication & RequestInformation invoked");
                        TransducerLogAndroid.LogInfo("StartCommunication & RequestInformation invoked (IP={0})", ip);
                    }
                    catch (Exception ex)
                    {
                        QueueLog("StartService error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "StartService");
                    }
                });

                RunOnUiThread(() =>
                {
                    tvStatus.Text = "Status: Connecting";
                    SetConnectionConnecting();
                });

                _ = Task.Run(async () =>
                {
                    try
                    {
                        int tries = 0;
                        int maxTries = 50;
                        while (tries++ < maxTries)
                        {
                            await Task.Delay(200).ConfigureAwait(false);
                            if (Trans == null) break;
                            try
                            {
                                if (Trans.IsConnected)
                                {
                                    // update lastComm and known state
                                    lastComm = DateTime.Now;

                                    // IMPORTANT: notify the known-state and the counter immediately
                                    // set the UI indicator
                                    lastKnownConnected = true;
                                    SetConnectionIndicator(true);

                                    // Ensure the ConnectionCounter records this connection (start uptime)
                                    try
                                    {
                                        ConnectionCounter.Instance.Connected();
                                        QueueLog("ConnectionCounter: Connected() called from BtnConnectIP_Click.");
                                    }
                                    catch (Exception ex)
                                    {
                                        QueueLog("ConnectionCounter.Connected error: " + ex.Message);
                                    }

                                    RunOnUiThread(() => tvStatus.Text = "Status: Connected");
                                    QueueLog("Connection established (Trans.IsConnected==true)");
                                    TransducerLogAndroid.LogInfo("Connection established (IP={0})", ip);
                                    return;
                                }
                            }
                            catch (Exception ex) { QueueLog("Error reading Trans.IsConnected: " + ex.Message); }
                        }
                        lastKnownConnected = false;
                        SetConnectionIndicator(false);

                        RunOnUiThread(() => tvStatus.Text = "Status: Disconnected");
                        QueueLog("Connection timed out (IsConnected false after polling)");
                        TransducerLogAndroid.LogWarn("Connection timed out (IP={0})", ip);
                    }
                    catch (Exception ex) { QueueLog("Connection poll task error: " + ex.Message); }
                });
            }
            catch (Exception ex)
            {
                QueueLog("BtnConnectIP_Click error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnConnectIP_Click");
            }
        }


        private void BtnExportResults_Click(object sender, EventArgs e)
        {
            try
            {
                QueueLog("ExportResults: export requested by user.");

                Task.Run(() =>
                {
                    string path = null;
                    string error = null;

                    try
                    {
                        path = DbExportHelper.ExportResultsToCsv(this, db);
                        QueueLog("ExportResults: file created at " + path);
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        QueueLog("ExportResults error: " + ex.Message);
                    }

                    RunOnUiThread(() =>
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(path))
                            {
                                Toast.MakeText(this, "Error exporting results: " + error, ToastLength.Long).Show();
                                return;
                            }

                            ShowExportDialog(path);
                        }
                        catch { }
                    });
                });
            }
            catch (Exception ex)
            {
                QueueLog("BtnExportResults_Click error: " + ex.Message);
            }
        }





        private void BtnSyncToPc_Click(object sender, EventArgs e)
        {
            try
            {
                // abre a Activity SyncToPcActivity
                var intent = new Intent(this, typeof(SyncToPcActivity));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                QueueLog("Erro abrindo SyncToPcActivity: " + ex.Message);
                Toast.MakeText(this, "Erro ao abrir Sync: " + ex.Message, ToastLength.Short).Show();
            }


        }






        private void BtnTestRunner_Click(object sender, EventArgs e)
        {
            try
            {
                // abre a Activity SyncToPcActivity
                var intent = new Intent(this, typeof(TestRunnerActivity));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                QueueLog("Erro abrindo SyncToPcActivity: " + ex.Message);
                Toast.MakeText(this, "Erro ao abrir Sync: " + ex.Message, ToastLength.Short).Show();
            }


        }



        private void BtnTestAndroidLayout_Click(object sender, EventArgs e)
        {
            try
            {
                // abre a Activity SyncToPcActivity
                var intent = new Intent(this, typeof(TestAndroidLayout));
                StartActivity(intent);
            }
            catch (Exception ex)
            {
                QueueLog("Erro abrindo TestAndroidLayout: " + ex.Message);
                Toast.MakeText(this, "Erro ao abrir Sync: " + ex.Message, ToastLength.Short).Show();
            }


        }








        private void BtnExportDB_Click(object sender, EventArgs e)
        {
            try
            {
                ExportHelper.ExportDatabaseToDownloads(this);

            }
            catch (Exception ex)
            {
                QueueLog("Erro Export DB: " + ex.Message);
                Toast.MakeText(this, "Erro ao Exportar DB: " + ex.Message, ToastLength.Short).Show();
            }


        }












        // ========== EXPORT LOGS (NOVO) ==========
        private void BtnExportLogs_Click(object sender, EventArgs e)
        {
            try
            {
                QueueLog("ExportLogs: export requested by user.");

                Task.Run(() =>
                {
                    string path = null;
                    string error = null;

                    try
                    {
                        path = DbExportHelper.ExportLogsToCsv(this, db);
                        QueueLog("ExportLogs: file created at " + path);
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        QueueLog("ExportLogs error: " + ex.Message);
                    }

                    RunOnUiThread(() =>
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(error) || string.IsNullOrEmpty(path))
                            {
                                Toast.MakeText(this, "Error exporting logs: " + error, ToastLength.Long).Show();
                                return;
                            }

                            ShowExportDialog(path);
                        }
                        catch { }
                    });
                });
            }
            catch (Exception ex)
            {
                QueueLog("BtnExportLogs_Click error: " + ex.Message);
            }
        }






        private void ShowExportDialog(string fullPath)
        {
            try
            {
                var builder = new Android.App.AlertDialog.Builder(this);
                builder.SetTitle("Results exported");

                string msg = "File saved to:\n\n" + fullPath +
                             "\n\n(Open: use share or open. If 'Open' fails, use your Files app and go to Documents → TransducerAppXA)";

                builder.SetMessage(msg);

                builder.SetPositiveButton("OK", (s, e) => { });

                builder.SetNeutralButton("ABRIR", (s, e) =>
                {
                    try
                    {
                        // use FileProvider-based open
                        DbExportHelper.ShareOrOpenCsvFile(this, fullPath, share: false);
                    }
                    catch (Exception ex)
                    {
                        QueueLog("ShowExportDialog OPEN error: " + ex.Message);
                        Toast.MakeText(this, "Unable to open file.", ToastLength.Short).Show();
                    }
                });

                builder.SetNegativeButton("SHARE", (s, e) =>
                {
                    try
                    {
                        DbExportHelper.ShareOrOpenCsvFile(this, fullPath, share: true);
                    }
                    catch (Exception ex)
                    {
                        QueueLog("ShowExportDialog SHARE error: " + ex.Message);
                        Toast.MakeText(this, "Unable to share file.", ToastLength.Short).Show();
                    }
                });

                builder.Show();
            }
            catch (Exception ex)
            {
                QueueLog("ShowExportDialog error: " + ex.Message);
                Toast.MakeText(this, "Exported to:\n" + fullPath, ToastLength.Long).Show();
            }
        }
















        private void BtnZeroTqAn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Trans == null)
                {
                    QueueLog("ZeroTQAN: Trans not connected");
                    if (DateTime.Now - lastToastTime > toastThrottle)
                    {
                        lastToastTime = DateTime.Now;
                        Toast.MakeText(this, "Transdutor não conectado", ToastLength.Short).Show();
                    }
                    return;
                }


                if (!isTestRunning)
                {
                    QueueLog("ZeroTQAN: no test running - abort");
                    if (DateTime.Now - lastToastTime > toastThrottle)
                    {
                        lastToastTime = DateTime.Now;
                        Toast.MakeText(this, "Nenhum teste em andamento", ToastLength.Short).Show();
                    }
                    return;
                }



                QueueLog("ZeroTQAN: SetZeroTorque + SetZeroAngle");
                TransducerLogAndroid.LogInfo("ZeroTQAN: SetZeroTorque + SetZeroAngle requested by user");

                Task.Run(() =>
                {
                    try
                    {
                        //if (Trans.DataResult = null)
                        //{

                        //}

                        Trans.SetZeroTorque();
                        Thread.Sleep(10);
                        Trans.SetZeroAngle();
                        Thread.Sleep(10);
                        QueueLog("ZeroTQAN: offsets zeroed");
                        TransducerLogAndroid.LogInfo("ZeroTQAN: offsets zeroed");

                        Thread.Sleep(20);


                        //Trans.StopReadData();
                        //QueueLog("StopReadData invoked");
                        //TransducerLogAndroid.LogInfo("StopReadData invoked");


                    }
                    catch (Exception ex)
                    {
                        QueueLog("ZeroTQAN error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "ZeroTQAN");
                    }
                });
            }
            catch (Exception ex)
            {
                QueueLog("BtnZeroTqAn_Click error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnZeroTqAn_Click");
            }
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                QueueLog("Disconnect requested");
                TransducerLogAndroid.LogInfo("Disconnect requested by user");
                
                // Notifica contador imediatamente que o usuário solicitou desconexão
                try { ConnectionCounter.Instance.Disconnected(); } catch { }

                if (Trans != null)
                {
                    try { Trans.RaiseError -= TransducerErrorReceiver; } catch { }
                    var t = Trans;
                    Task.Run(() =>
                    {
                        try { t.StopReadData(); t.StopService(); }
                        catch (Exception ex)
                        {
                            QueueLog("Stop error: " + ex.Message);
                            TransducerLogAndroid.LogException(ex, "Disconnect.Stop");
                        }
                    });

                    try
                    {
                        Trans.DataResult -= ResultReceiver;
                        Trans.TesteResult -= TesteResultReceiver;
                        Trans.DataInformation -= DataInformationReceiver;
                        Trans.DebugInformation -= DebugInformationReceiver;
                    }
                    catch { }
                    Trans = null;
                }
                RunOnUiThread(() =>
                {
                    tvStatus.Text = "Status: Disconnected";
                    SetConnectionIndicator(false);
                    SetTestIndicator(false); // <- garante que a bolinha volte a vermelho
                    SetParameterInputsEnabled(true);
                    ResetCachedParams();


                });
                // update monitor-known state
                lastKnownConnected = false;
            }
            catch (Exception ex)
            {
                QueueLog("BtnDisconnect_Click error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnDisconnect_Click");
            }
        }

        private void BtnInitRead_Click(object sender, EventArgs e)
        {
            ShowInitParamsConfirmation();
        }

        void ShowInitParamsConfirmation()
        {
            try
            {
                if (Trans == null)
                {
                    Toast.MakeText(this, "Transdutor não conectado", ToastLength.Short).Show();
                    return;
                }

                var limits = GetCurrentLimitsFromUi();

                var sb = new StringBuilder();
                sb.AppendLine("Confirme os parâmetros do teste:");
                sb.AppendLine();
                sb.AppendLine($"Torque mínimo:  {limits.MinT:F3} Nm");
                sb.AppendLine($"Torque nominal: {limits.NomT:F3} Nm");
                sb.AppendLine($"Torque máximo:  {limits.MaxT:F3} Nm");
                sb.AppendLine();
                sb.AppendLine($"Threshold inicial: {limits.ThresholdIni:F3} Nm");
                sb.AppendLine($"Threshold final:   {(limits.ThresholdIni / 2M):F3} Nm");
                sb.AppendLine($"Timeout fim (ms):  {limits.TimeoutEnd}");
                sb.AppendLine();
                sb.AppendLine($"Frequência (hz):  {limits.FreqT}");
                sb.AppendLine();
                sb.AppendLine($"Ferramenta: {GetFriendlyToolTypeName(selectedToolType)}");

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Confirmar parâmetros");
                builder.SetMessage(sb.ToString());
                builder.SetPositiveButton("Prosseguir", (s, e) => { _ = InitReadAsync(); });
                builder.SetNegativeButton("Cancelar", (s, e) => { });
                builder.Show();
            }
            catch (Exception ex)
            {
                QueueLog("ShowInitParamsConfirmation error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "ShowInitParamsConfirmation");
            }
        }

        void SetParameterInputsEnabled(bool enabled)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (txtThresholdIniFree != null) txtThresholdIniFree.Enabled = enabled;
                        if (txtThresholdEndFree != null) txtThresholdEndFree.Enabled = enabled;
                        if (txtTimeoutFree != null) txtTimeoutFree.Enabled = enabled;

                        if (txtNominalTorque != null) txtNominalTorque.Enabled = enabled;
                        if (txtMinimumTorque != null) txtMinimumTorque.Enabled = enabled;
                        if (txtMaximoTorque != null) txtMaximoTorque.Enabled = enabled;

                        if (txtFrequencia != null) txtFrequencia.Enabled = enabled;



                        if (txtIP != null) txtIP.Enabled = enabled;
                        if (sprToolType != null) sprToolType.Enabled = enabled;
                    }
                    catch { }
                });
            }
            catch { }
        }

        private void BtnStopRead_Click(object sender, EventArgs e)
        {

            if (!isTestRunning)
            {
                QueueLog("btnStop");
                if (DateTime.Now - lastToastTime > toastThrottle)
                {
                    lastToastTime = DateTime.Now;
                    Toast.MakeText(this, "Nenhum teste em andamento", ToastLength.Short).Show();
                }
                return;
            }





            try
            {
                if (Trans == null) return;
                QueueLog("StopRead: StopReadData called");
                TransducerLogAndroid.LogInfo("StopRead requested by user");
                Task.Run(() =>
                {
                    try
                    {
                        Trans.StopReadData();
                        QueueLog("StopReadData invoked");
                        TransducerLogAndroid.LogInfo("StopReadData invoked");
                        SetTestIndicator(false); // desliga indicador de teste

                    }
                    catch (Exception ex)
                    {
                        QueueLog("StopReadData error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "StopRead");
                    }
                });

                // atualiza indicador de teste para "parado"
                //SetTestIndicator(false); // desliga indicador de teste

                RunOnUiThread(() => tvStatus.Text = "Status: Stopped");
                SetParameterInputsEnabled(true);
                ResetCachedParams();

                isTestRunning = false;
                QueueLog("Test stopped (isTestRunning=false)");



            }
            catch (Exception ex)
            {
                QueueLog("BtnStopRead_Click error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnStopRead_Click");
            }
        }

        private void BtnCopyLog_Click(object sender, EventArgs e)
        {
            try
            {
                var sb = new StringBuilder();
                for (int i = logItems.Count - 1; i >= 0; i--) sb.AppendLine(logItems[i]);
                var cm = (ClipboardManager)GetSystemService(ClipboardService);
                var clip = ClipData.NewPlainText("TransducerLog", sb.ToString());
                cm.PrimaryClip = clip;
                if (DateTime.Now - lastToastTime > toastThrottle)
                {
                    lastToastTime = DateTime.Now;
                    Toast.MakeText(this, "Log copied to clipboard", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                QueueLog("Copy log error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnCopyLog_Click");
            }
        }

        private void BtnClearLog_Click(object sender, EventArgs e)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Limpar Logs");
                        builder.SetMessage("Deseja limpar o histórico de Logs? Isso também apagará os Logs salvos no banco de dados.");

                        builder.SetPositiveButton("Sim", (s, ev) =>
                        {
                            RunOnUiThread(() =>
                            {
                                try
                                {
                                    logItems.Clear();
                                    logAdapter.NotifyDataSetChanged();
                                    lvLog.InvalidateViews();
                                }
                                catch { }
                            });

                            Task.Run(() =>
                            {
                                try
                                {
                                    TransducerLogAndroid.ClearAllPersistedLogs();
                                    QueueLog("All logs cleared from database.");
                                    TransducerLogAndroid.LogInfo("All logs cleared from database by user.");
                                }
                                catch (Exception ex)
                                {
                                    QueueLog("Error clearing logs from database: " + ex.Message);
                                    TransducerLogAndroid.LogException(ex, "BtnClearLog_Click/DB");
                                }
                            });
                        });

                        builder.SetNegativeButton("Não", (s, ev) => { });
                        builder.Show();
                    }
                    catch (Exception ex)
                    {
                        QueueLog("BtnClearLog_Click dialog error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "BtnClearLog_Click/Dialog");
                    }
                });
            }
            catch (Exception ex)
            {
                QueueLog("BtnClearLog_Click outer error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnClearLog_Click");
            }
        }

        private void BtnClearResults_Click(object sender, EventArgs e)
        {
            try
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Limpar resultados");
                builder.SetMessage("Deseja limpar o histórico de resultados? Isso também apagará os resultados salvos no banco de dados.");
                builder.SetPositiveButton("Sim", (s, ev) =>
                {
                    // Atualiza a UI imediatamente (na UI thread)
                    RunOnUiThread(() =>
                    {
                        ResultsCounter = 0;
                        tvResultsCounter.Text = "0";
                        resultItems.Clear();
                        resultAdapter.NotifyDataSetChanged();
                        try { lvResults.InvalidateViews(); } catch { }
                    });

                    // 1) Reseta o StatisticsService (em foreground — é rápido)
                    try
                    {
                        // Se não quiser bloquear, este call é rápido: limpa listas internas e dispara eventos
                        StatisticsService.Instance?.Reset();
                        QueueLog("StatisticsService reset after user cleared results.");
                    }
                    catch (Exception ex)
                    {
                        QueueLog("Error resetting StatisticsService after clearing DB: " + ex.Message);
                    }

                    // 2) Limpa a DB em background (operações I/O)
                    Task.Run(() =>
                    {
                        try
                        {
                            db.ClearAllResults();
                            QueueLog("All results cleared from database.");
                            TransducerLogAndroid.LogInfo("All results cleared from database by user.");
                        }
                        catch (Exception ex)
                        {
                            QueueLog("Error clearing results from database: " + ex.Message);
                            TransducerLogAndroid.LogException(ex, "BtnClearResults_Click/DB");
                        }
                    });
                });

                builder.SetNegativeButton("Não", (s, ev) => { });
                builder.Show();
            }
            catch (Exception ex)
            {
                QueueLog("BtnClearResults_Click error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "BtnClearResults_Click");
            }
        }








        // ================== InitReadAsync / Loop usando variáveis ==================
        private async Task InitReadAsync()
        {
            if (Trans == null)
            {
                QueueLog("InitRead: Trans not connected");
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

            QueueLog("InitRead: using cached params");
            QueueLog($"InitRead PARAMS: thrIni={_currentThresholdIni} timeout={_currentTimeoutEnd} min={_currentMinT} nom={_currentNomT} max={_currentMaxT}");



            // -------------------
            // Ajustes para StatisticsService:
            // 1) Reset das estatísticas para começar o ciclo atual limpo
            // 2) Definir limites/spec (USL, LSL, Nominal) para o gráfico e cálculos (Cm/Cmk)
            try
            {
                // garante inicialização do singleton (idempotente)
                double defaultUSL = (double)DEFAULT_MAX_TORQUE;
                double defaultLSL = (double)DEFAULT_MIN_TORQUE;
                // já deve ter sido inicializado em OnCreate, mas fazemos safe-guard
                StatisticsService.Instance.Initialize(windowSize: 200, defaultUSL: defaultUSL, defaultLSL: defaultLSL, sigmaType: RealTimeStatistics.StdDevType.Population, minSamples: 3);

                // reset para iniciar novo ciclo
                try
                {
                    StatisticsService.Instance.Reset();
                    QueueLog("StatisticsService reset at test start.");
                }
                catch (Exception exReset)
                {
                    QueueLog("StatisticsService reset error: " + exReset.Message);
                }

                // atualiza limites com os valores que estão no UI (usa o mesmo método que você já tem)
                try
                {
                    var limitsForSpecs = GetCurrentLimitsFromUi(); // lê os valores atuais da UI
                    StatisticsService.Instance.SetSpecLimits((double)limitsForSpecs.MaxT, (double)limitsForSpecs.MinT, (double)limitsForSpecs.NomT);
                    QueueLog($"StatisticsService spec limits set: USL={limitsForSpecs.MaxT} LSL={limitsForSpecs.MinT} Nom={limitsForSpecs.NomT}");
                }
                catch (Exception exSpecs)
                {
                    QueueLog("StatisticsService SetSpecLimits error: " + exSpecs.Message);
                }
            }
            catch (Exception ex)
            {
                QueueLog("StatisticsService init/seed error (ignored): " + ex.Message);
            }
            // -------------------








            await Task.Run(() =>
            {
                try
                {
                    try
                    {
                        var frames = Trans.GetInitReadFrames();
                        if (frames != null)
                        {
                            foreach (var f in frames) QueueLog($"InitRead pre-CRC: {f.Item1}");
                        }
                    }
                    catch (Exception ex)
                    {
                        QueueLog("InitRead: failed to get init frames: " + ex.Message);
                    }

                    Trans.SetZeroTorque();
                    Thread.Sleep(10);
                    Trans.SetZeroAngle();
                    Thread.Sleep(10);

                    Trans.SetTestParameter_ClickWrench(85, 95, 20);
                    Thread.Sleep(10);

                    decimal thresholdEnd = _currentThresholdIni / 2M;
                    if (thresholdEnd < 0) thresholdEnd = 0;

                    int timeStepMs = 5;
                    int filterFreq = 2000;

                    Trans.SetTestParameter(
                        new DataInformation(),
                        TesteType.TorqueOnly,
                        selectedToolType,
                        _currentNomT,
                        _currentThresholdIni,
                        thresholdEnd,
                        _currentTimeoutEnd,
                        timeStepMs,
                        //filterFreq,
                        _currentFreq,
                        eDirection.CW);

                    Thread.Sleep(100);
                    Trans.StartReadData();

                    // atualiza indicador de teste para "executando"
                    //SetTestIndicator(true);

                    SetParameterInputsEnabled(false);

                    isTestRunning = true;
                    QueueLog("Test started (isTestRunning=true)");








                }
                catch (Exception ex)
                {
                    QueueLog("InitRead (background) error: " + ex.Message);
                }
            }).ConfigureAwait(false);

            RunOnUiThread(() => tvStatus.Text = "Status: Acquisition started");
        }

        private async Task InitReadAsyncLoop()
        {
            if (Trans == null || !_paramsLoaded)
            {
                QueueLog("InitReadLoop: params not loaded or Trans null");
                return;
            }

            QueueLog("InitReadLoop: using cached params");
            QueueLog($"InitReadLoop PARAMS: thrIni={_currentThresholdIni} timeout={_currentTimeoutEnd} min={_currentMinT} nom={_currentNomT} max={_currentMaxT}");

            await Task.Run(() =>
            {
                try
                {
                    try
                    {
                        var frames = Trans.GetInitReadFrames();
                        if (frames != null)
                        {
                            foreach (var f in frames) QueueLog($"InitReadLoop pre-CRC: {f.Item1}");
                        }
                    }
                    catch (Exception ex)
                    {
                        QueueLog("InitReadLoop: failed to get init frames: " + ex.Message);
                    }

                    Trans.SetTestParameter_ClickWrench(85, 95, 20);
                    Thread.Sleep(10);

                    decimal thresholdEnd = _currentThresholdIni / 2M;
                    if (thresholdEnd < 0) thresholdEnd = 0;

                    int timeStepMs = 5;
                    int filterFreq = 2000;

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
                    SetParameterInputsEnabled(false);
                }
                catch (Exception ex)
                {
                    QueueLog("InitReadLoop (background) error: " + ex.Message);
                }
            }).ConfigureAwait(false);

            RunOnUiThread(() => tvStatus.Text = "Status: Acquisition started");
        }


        // ============== RESULTADOS + JULGAMENTO ==============
        private void ResultReceiver(DataResult Data)
        {
            try
            {
                // update last communication timestamp
                lastComm = DateTime.Now;

                SetTestIndicator(true); //aqui indicador de ciclo




                // ----- ADICIONADO: alimentar RealTimeStatistics via StatisticsService -----
                //try
                //{
                    //if (StatisticsService.Instance != null && StatisticsService.Instance.Stats != null && Data != null)
                    //{
                        //double usl = (double)((_currentMaxT > 0) ? _currentMaxT : DEFAULT_MAX_TORQUE);
                        //double lsl = (double)((_currentMinT > 0) ? _currentMinT : DEFAULT_MIN_TORQUE);
                        //try
                        //{
                            //StatisticsService.Instance.Stats.AddSample((double)Data.Torque, upperSpecLimit: usl, lowerSpecLimit: lsl);
                        //}
                        //catch { /* não deixar quebrar fluxo principal */ }
                   // }
                //}
                //catch { }











            }
            catch { }

            TransducerLogAndroid.LogInfo($"ResultReceiver - Torque={Data?.Torque} Angle={Data?.Angle}");
            RunOnUiThread(() =>
            {
                try
                {
                    tvTorque.Text = $"{Data.Torque:F3} Nm";
                    tvAngle.Text = $"{Data.Angle:F2} º";
                }
                catch (Exception ex)
                {
                    QueueLog("ResultReceiver UI update error: " + ex.Message);
                    TransducerLogAndroid.LogException(ex, "ResultReceiver");
                }
            });
        }

        // ================== JULGAMENTO ==================
        private async void TesteResultReceiver(List<DataResult> Result)
        {
            try
            {
                // update last communication timestamp
                lastComm = DateTime.Now;

                //SetTestIndicator(true); //aqui indicador de ciclo


                if (Result == null || Result.Count == 0)
                {
                    QueueLog("TesteResultReceiver: empty list");
                    return;
                }

                var fr = Result.Find(x => x.Type == "FR");
                if (fr == null)
                {
                    QueueLog("TesteResultReceiver: FR not found -> untighten");
                    UntighteningsCounter++;
                    RunOnUiThread(() => tvUntighteningsCounter.Text = UntighteningsCounter.ToString());
                    await TryRearmAsync();
                    return;
                }

                var torqueVal = fr.Torque;
                var angleVal = fr.Angle;
                var now = DateTime.Now;

                // Dedup (igual ao seu)
                bool isDuplicate = false;
                if (lastAddedResultTime != DateTime.MinValue)
                {
                    var dt = now - lastAddedResultTime;
                    var dTorque = Math.Abs(torqueVal - lastAddedTorque);
                    var dAngle = Math.Abs(angleVal - lastAddedAngle);
                    if (dt <= dedupeTimeWindow && dTorque <= dedupeTorqueThreshold && dAngle <= dedupeAngleThreshold)
                        isDuplicate = true;
                }

                if (isDuplicate)
                {
                    QueueLog("Ignored duplicate FR");
                    await TryRearmAsync();
                    return;
                }

                ResultsCounter++;
                lastAddedResultTime = now;
                lastAddedTorque = torqueVal;
                lastAddedAngle = angleVal;

                if (!_paramsLoaded)
                {
                    QueueLog("TesteResultReceiver: params not loaded, using defaults in memory");
                    _currentMinT = DEFAULT_MIN_TORQUE;
                    _currentNomT = DEFAULT_NOM_TORQUE;
                    _currentMaxT = DEFAULT_MAX_TORQUE;
                }

                string statusText = (torqueVal >= _currentMinT && torqueVal <= _currentMaxT) ? "OK" : "NOK";
                QueueLog($"JUDGE: torque={torqueVal:F3} | min={_currentMinT} nom={_currentNomT} max={_currentMaxT} -> {statusText}");

                RunOnUiThread(() =>
                {
                    tvResultsCounter.Text = ResultsCounter.ToString();
                    tvTorque.Text = $"{torqueVal:F3} Nm";
                    tvAngle.Text = $"{angleVal:F2} º";
                    tvStatus.Text = $"Test completed ({statusText})";
                });

                var line = $"{ResultsCounter}  {now:yyyy-MM-dd HH:mm:ss}  {torqueVal:F3} Nm  {angleVal:F2} º  [{statusText}]";
                QueueResultItem(line);


                //try
                //{

                    Task.Run(() =>
                {
                    try
                    {
                        var entry = new ResultEntry
                        {
                            TimestampUtc = now.ToUniversalTime(),
                            Torque = torqueVal,
                            Angle = angleVal,
                            Text = line
                        };
                        db.InsertResult(entry);

                        // entry.Id será preenchido pelo sqlite-net após Insert (AutoIncrement)
                        int resultId = entry.Id;

                        // grava os pontos da curva (lastTrace foi populado logo acima)
                        try
                        {
                            if (lastTrace != null && lastTrace.Count > 0)
                            {
                                var pts = new List<TracePointEntry>();
                                int idx = 0;
                                foreach (var p in lastTrace)
                                {
                                    if (p == null) continue;
                                    // normalmente apenas os pontos TV (Test Verification) fazem sentido como trace
                                    // mas vamos salvar todos os pontos que tiverem SampleTime definido
                                    pts.Add(new TracePointEntry
                                    {
                                        ResultId = resultId,
                                        PointIndex = idx++,
                                        TimeMs = p.SampleTime, // assumimos SampleTime já em ms; ajuste se for diferente
                                        Torque = (double)p.Torque,
                                        Angle = (double)p.Angle
                                    });
                                }

                                if (pts.Count > 0)
                                {
                                    db.InsertTracePoints(resultId, pts);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            QueueLog("Error saving trace points to DB: " + ex.Message);
                        }



                        // --- ADICIONADO: após inserir no DB, alimentar as estatísticas somente com valores persistidos ---
                        try
                        {
                            if (StatisticsService.Instance != null && StatisticsService.Instance.Stats != null)
                            {
                                double usl = (double)((_currentMaxT > 0) ? _currentMaxT : DEFAULT_MAX_TORQUE);
                                double lsl = (double)((_currentMinT > 0) ? _currentMinT : DEFAULT_MIN_TORQUE);
                                try
                                {
                                    double torqueAsDouble = Convert.ToDouble(entry.Torque);
                                    Android.Util.Log.Info("TesteResultReceiver", $"About to add persisted sample: torque={torqueAsDouble} usl={usl} lsl={lsl} samplesBefore={StatisticsService.Instance.GetSamplesSnapshot().Count}");
                                    StatisticsService.Instance.AddPersistedSample(torqueAsDouble, entry.Text);
                                    Android.Util.Log.Info("TesteResultReceiver", $"Added persisted sample: torque={torqueAsDouble} samplesAfter={StatisticsService.Instance.GetSamplesSnapshot().Count}");
                                }
                                catch (Exception ex)
                                {
                                    Android.Util.Log.Warn("TesteResultReceiver", "Error adding persisted sample to StatisticsService: " + ex.Message);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Android.Util.Log.Warn("TesteResultReceiver", "Stats AddSample after DB insert outer error: " + ex.Message);
                        }

                        // --- ADICIONADO: após inserir no DB, alimentar as estatísticas somente com valores persistidos ---



                    }
                    catch (Exception ex)
                    {
                        QueueLog("Error inserting result: " + ex.Message);
                    }
                });















                if (DateTime.Now - lastToastTime > toastThrottle)
                {
                    lastToastTime = DateTime.Now;
                    try
                    {
                        RunOnUiThread(() =>
                            Toast.MakeText(this, $"Result #{ResultsCounter}: {torqueVal:F3} Nm [{statusText}]", ToastLength.Short).Show());
                    }
                    catch { }
                }

                lastTrace.Clear();
                lastTrace.AddRange(Result);
                if (showTrace)
                {
                    int dialogId = Interlocked.Increment(ref nextTraceDialogId);
                    lastTraceDialogId = dialogId;
                    RunOnUiThread(() => ShowTraceDialog(lastTrace, dialogId));
                }

                await TryRearmAsync();
                Thread.Sleep(10);
            }
            catch (Exception ex)
            {
                QueueLog("TesteResultReceiver error: " + ex.Message);
            }
        }










        // ============== ERROS / REARME ==============
        private void TransducerErrorReceiver(int err)
        {
            QueueLog($"EVENT: RaiseError invoked - err={err}");
            TransducerLogAndroid.LogError($"EVENT: RaiseError invoked - err={err}");
            Task.Run(() => HandleTransducerError(err));
        }

        private void HandleTransducerError(int err)
        {
            try
            {
                switch (err)
                {
                    case 1:
                        er01Count++;
                        QueueLog($"ERROR ER01: CRC invalid ({er01Count})");
                        RunOnUiThread(() => tvStatus.Text = "Error: ER01 CRC");
                        TransducerLogAndroid.LogError($"ERROR ER01: CRC invalid ({er01Count})");
                        break;
                    case 2:
                        er02Count++;
                        QueueLog($"ERROR ER02: Syntax invalid ({er02Count})");
                        RunOnUiThread(() => tvStatus.Text = "Error: ER02 Syntax");
                        TransducerLogAndroid.LogError($"ERROR ER02: Syntax invalid ({er02Count})");
                        break;
                    case 3:
                        er03Count++;
                        QueueLog($"ERROR ER03: Invalid command ({er03Count})");
                        RunOnUiThread(() => tvStatus.Text = "Error: ER03");
                        TransducerLogAndroid.LogError($"ERROR ER03: Invalid command ({er03Count})");
                        break;
                    case 4:
                        er04Count++;
                        QueueLog($"ERROR ER04: Device not ready ({er04Count})");
                        RunOnUiThread(() => tvStatus.Text = "Error: ER04");
                        TransducerLogAndroid.LogError($"ERROR ER04: Device not ready ({er04Count})");
                        break;
                    default:
                        QueueLog($"ERROR Unknown: code={err}");
                        RunOnUiThread(() => tvStatus.Text = $"Error: code={err}");
                        TransducerLogAndroid.LogError($"ERROR Unknown: code={err}");
                        break;
                }
            }
            catch (Exception ex)
            {
                QueueLog("HandleTransducerError error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "HandleTransducerError");
            }
        }










        private async Task TryRearmAsync()
        {
            bool entered = await rearmLock.WaitAsync(0);
            if (!entered) return;
            try
            {
                await Task.Delay(rearmCooldownMs).ConfigureAwait(false);
                if (Trans == null) return;
                await InitReadAsyncLoop().ConfigureAwait(false);
            }
            finally { try { rearmLock.Release(); } catch { } }
        }

        // ============== DATA / DEBUG / DESTROY ==============
        private void DataInformationReceiver(DataInformation info)
        {
            try
            {
                // update last communication timestamp
                lastComm = DateTime.Now;

                //SetTestIndicator(true); //aqui indicador de ciclo ativo


            }
            catch { }

            TransducerLogAndroid.LogInfo($"DataInformation - HardID={info?.HardID} FullScale={info?.FullScale} TorqueLimit={info?.TorqueLimit}");
        }

        private void DebugInformationReceiver(DebugInformation dbg)
        {
            try
            {
                // update last communication timestamp
                lastComm = DateTime.Now;

                //SetTestIndicator(true); // aqui teste ativo


            }
            catch { }

            TransducerLogAndroid.LogDebug($"DebugInformation - State={dbg?.State} Error={dbg?.Error}");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                // stop monitor
                StopConnectionMonitor();

                try
                {
                    // Fecha e limpa dialog de stats caso esteja aberto
                    HideStatsDialog();

                    // stop monitor
                    StopConnectionMonitor();





                    if (protocolLoggerType != null && protocolHandlerDelegate != null)
                    {
                        var ev = protocolLoggerType.GetEvent("OnLogWritten", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        if (ev != null) ev.RemoveEventHandler(null, protocolHandlerDelegate);
                    }
                }
                catch { }

                try { TransducerLogAndroid.OnLogAppended -= TransducerLog_OnLogAppended; } catch { }
                try { TransducerLogAndroid.Shutdown(); } catch { }

                if (Trans != null)
                {
                    try
                    {
                        Trans.RaiseError -= TransducerErrorReceiver;
                        Trans.StopReadData();
                        Trans.StopService();
                        //SetTestIndicator(false);
                    }
                    catch { }
                    Trans = null;
                }


                //try { testIndicator = testIndicator; /* no-op para evitar warnings */ } catch { } // substituido por SetTestIndicator(false);

                //SetTestIndicator(false);

                
                //SetTestIndicator(false);


                SetConnectionIndicator(false);


                try { db?.Dispose(); } catch { }


                base.OnDestroy();
                try
                {
                    if (StatisticsService.Instance != null)
                        StatisticsService.Instance.OnStatisticsUpdated -= Main_StatsUpdatedHandler;
                }
                catch { }





                try
                {
                    // remover inscrição do handler corretamente
                    if (_connectionCounterSubscribed && _connectionCounterHandler != null)
                    {
                        try
                        {
                            ConnectionCounter.Instance.OnChanged -= _connectionCounterHandler;
                        }
                        catch { }
                        _connectionCounterSubscribed = false;
                        _connectionCounterHandler = null;
                    }

                    // parar e desalocar timer
                    try
                    {
                        _uiTimerForUptime?.Stop();
                        _uiTimerForUptime?.Dispose();
                        _uiTimerForUptime = null;
                    }
                    catch { }

                    // ... resto do seu OnDestroy atual ...
                }
                catch { }
                finally
                {
                    base.OnDestroy();
                }











            }
            catch { }
        }

        void TransducerLog_OnLogAppended(TransducerLogAndroid.LogRecord rec)
        {
            try
            {
                if (logsPaused) return;

                RunOnUiThread(() =>
                {
                    try
                    {
                        var line = rec.ToString();
                        logItems.Insert(0, line);
                        if (logItems.Count > MAX_LOG_ITEMS) logItems.RemoveRange(MAX_LOG_ITEMS, logItems.Count - MAX_LOG_ITEMS);
                        logAdapter.NotifyDataSetChanged();
                    }
                    catch { }
                });
            }
            catch { }
        }

        // ============== TRACE DIALOG + ADAPTERS ==============
        // Substitua seu método ShowTraceDialog por este (cópia/cola no MainActivity.cs)
        void ShowTraceDialog(IList<DataResult> trace, int dialogId)
        {
            try
            {
                if (trace == null || trace.Count == 0)
                {
                    Toast.MakeText(this, "No trace data", ToastLength.Short).Show();
                    return;
                }

                if (dialogId != lastTraceDialogId)
                    return;

                try
                {
                    if (currentTraceDialog != null && currentTraceDialog.IsShowing)
                        currentTraceDialog.Dismiss();
                }
                catch { }

                // Encontra FR
                DataResult fr = null;
                foreach (var p in trace)
                {
                    if (p != null && p.Type == "FR")
                    {
                        fr = p;
                        break;
                    }
                }

                decimal torqueVal = 0m;
                decimal angleVal = 0m;
                bool isOk = false;
                string statusText = "NOK";

                try
                {
                    if (fr != null)
                    {
                        torqueVal = fr.Torque;
                        angleVal = fr.Angle;

                        // Usa os limites atuais (mesma lógica que você tinha)
                        var limits = GetCurrentLimitsFromUi();
                        var status = JudgeTorque(torqueVal, limits);
                        statusText = status == JudgmentStatus.OK ? "OK" : "NOK";
                        isOk = status == JudgmentStatus.OK;
                    }
                }
                catch
                {
                    // mantém defaults em caso de erro
                    try
                    {
                        if (fr != null)
                        {
                            torqueVal = fr.Torque;
                            angleVal = fr.Angle;
                        }
                    }
                    catch { }
                    isOk = false;
                    statusText = "NOK";
                }

                // Monta diálogo
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Trace");

                // Root container (ScrollView para telas pequenas)
                var scroll = new ScrollView(this);
                var container = new LinearLayout(this) { Orientation = Orientation.Vertical };
                int pad = (int)(8 * Resources.DisplayMetrics.Density);
                container.SetPadding(pad, pad, pad, pad);
                scroll.AddView(container);

                // ----- PAINEL SUPERIOR: Results (torque/angle grandes) -----
                var topPanel = new LinearLayout(this)
                {
                    Orientation = Orientation.Vertical,
                    LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                };
                topPanel.SetPadding(pad, pad / 2, pad, pad / 2);
                // centraliza horizontalmente o conteúdo do LinearLayout
                topPanel.SetGravity(GravityFlags.CenterHorizontal);

                // Label "Results"
                var lblResults = new TextView(this);
                lblResults.Text = "Results";
                lblResults.Gravity = GravityFlags.Center;
                lblResults.SetTextSize(Android.Util.ComplexUnitType.Sp, 18f);
                lblResults.Typeface = Typeface.DefaultBold;
                lblResults.SetTextColor(Color.ParseColor("#212121"));
                topPanel.AddView(lblResults);

                // Torque value (grande)
                var tvTorqueLarge = new TextView(this);
                tvTorqueLarge.Text = $"{torqueVal:F3} Nm";
                tvTorqueLarge.Gravity = GravityFlags.Center;
                tvTorqueLarge.SetTextSize(Android.Util.ComplexUnitType.Sp, 36f);
                tvTorqueLarge.Typeface = Typeface.DefaultBold;
                tvTorqueLarge.SetTextColor(isOk ? Color.ParseColor("#2E7D32") : Color.ParseColor("#C62828"));
                topPanel.AddView(tvTorqueLarge);

                // Angle value (grande)
                var tvAngleLarge = new TextView(this);
                tvAngleLarge.Text = $"{angleVal:F2}°";
                tvAngleLarge.Gravity = GravityFlags.Center;
                tvAngleLarge.SetTextSize(Android.Util.ComplexUnitType.Sp, 32f);
                tvAngleLarge.Typeface = Typeface.Default;
                tvAngleLarge.SetTextColor(isOk ? Color.ParseColor("#2E7D32") : Color.ParseColor("#C62828"));
                topPanel.AddView(tvAngleLarge);

                // espaço abaixo do painel
                var spacer = new View(this);
                spacer.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, (int)(6 * Resources.DisplayMetrics.Density));
                topPanel.AddView(spacer);

                // adiciona painel superior ao container (mantém visível acima do trace)
                container.AddView(topPanel);

                // ----- Seção original do trace (radio buttons + gráfico) -----
                var radioGroup = new RadioGroup(this) { Orientation = Orientation.Horizontal };
                var rbTime = new RadioButton(this) { Text = "Torque x Tempo" };
                var rbAngle = new RadioButton(this) { Text = "Torque x Ângulo" };
                rbTime.Id = View.GenerateViewId();
                rbAngle.Id = View.GenerateViewId();
                radioGroup.AddView(rbTime);
                radioGroup.AddView(rbAngle);
                rbTime.Checked = true;

                // cria o TraceView exatamente como você já fazia
                var traceView = new TraceView(this, trace, TraceMode.TorqueVsTime);
                var tvParams = new LinearLayout.LayoutParams(
                    ViewGroup.LayoutParams.MatchParent,
                    (int)(220 * Resources.DisplayMetrics.Density));
                tvParams.TopMargin = pad;

                // texto de resultado menor (mantém o comportamento original)
                string resultText = "FR: n/a";
                try
                {
                    if (fr != null)
                    {
                        resultText = $"FR: {torqueVal:F3} Nm  {angleVal:F2} º  [{statusText}]";
                    }
                }
                catch { }

                var tvResult = new TextView(this)
                {
                    Text = resultText
                };
                tvResult.SetTextColor(isOk ? Color.ParseColor("#2E7D32") : Color.ParseColor("#C62828"));
                tvResult.TextSize = 12f;
                tvResult.SetPadding(0, pad, 0, 0);

                container.AddView(radioGroup);
                container.AddView(traceView, tvParams);
                container.AddView(tvResult);

                // evento do radio para alternar o modo do gráfico (como antes)
                radioGroup.CheckedChange += (s, e) =>
                {
                    try
                    {
                        if (e.CheckedId == rbTime.Id)
                            traceView.Mode = TraceMode.TorqueVsTime;
                        else if (e.CheckedId == rbAngle.Id)
                            traceView.Mode = TraceMode.TorqueVsAngle;

                        traceView.Invalidate();
                    }
                    catch { }
                };

                // botão Close preservado
                builder.SetView(scroll);
                builder.SetPositiveButton("Close", (s, e) => { });

                var dialog = builder.Create();

                dialog.DismissEvent += (s, e) =>
                {
                    if (ReferenceEquals(currentTraceDialog, dialog))
                        currentTraceDialog = null;
                };

                currentTraceDialog = dialog;

                if (dialogId == lastTraceDialogId)
                    dialog.Show();
                else
                    dialog.Dismiss();
            }
            catch (Exception ex)
            {
                // loga o erro para debugar
                try { QueueLog("ShowTraceDialog error: " + ex.Message); } catch { }
            }
        }



        class CustomStringAdapter : BaseAdapter<string>
        {
            readonly Activity context;
            readonly IList<string> items;
            readonly Color textColor;
            readonly float textSizeSp;
            readonly bool bold;

            public CustomStringAdapter(Activity ctx, IList<string> items, Color textColor, float textSizeSp = 13f, bool bold = false)
            {
                this.context = ctx;
                this.items = items ?? new List<string>();
                this.textColor = textColor;
                this.textSizeSp = textSizeSp;
                this.bold = bold;
            }

            public override string this[int position] => items[position];
            public override int Count => items.Count;
            public override long GetItemId(int position) => position;

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                TextView tv = convertView as TextView;
                if (tv == null)
                {
                    tv = new TextView(context);
                    int pad = (int)(8 * context.Resources.DisplayMetrics.Density);
                    tv.SetPadding(pad, pad / 2, pad, pad / 2);
                    tv.SetTextSize(ComplexUnitType.Sp, textSizeSp);
                    if (bold) tv.SetTypeface(tv.Typeface, TypefaceStyle.Bold);
                    tv.SetSingleLine(false);
                }

                string text = items[position];
                tv.Text = text;

                if (text.Contains("[NOK]"))
                {
                    tv.SetTextColor(Color.ParseColor("#F44336")); // aqui pinta a cor na lista de resultados (Vermelho)
                }
                else
                {
                    //tv.SetTextColor(textColor);
                    tv.SetTextColor(Color.ParseColor("#4CAF50")); // aqui pinta a cor na lista de resultados (Verde)


                }

                return tv;
            }
        }

        enum TraceMode
        {
            TorqueVsTime,
            TorqueVsAngle
        }

        class TraceView : View
        {
            readonly IList<DataResult> points;
            readonly Paint axisPaint;
            readonly Paint tracePaint;
            readonly Paint textPaint;
            readonly Paint frPointPaint;

            public TraceMode Mode { get; set; }

            public TraceView(Context context, IList<DataResult> points, TraceMode mode)
                : base(context)
            {
                this.points = points ?? new List<DataResult>();
                Mode = mode;

                axisPaint = new Paint
                {
                    Color = Color.Gray,
                    StrokeWidth = 2f,
                    AntiAlias = true
                };

                tracePaint = new Paint
                {
                    Color = Color.Rgb(33, 150, 243),
                    StrokeWidth = 3f,
                    AntiAlias = true
                };
                tracePaint.SetStyle(Paint.Style.Stroke);

                textPaint = new Paint
                {
                    Color = Color.Black,
                    TextSize = 24f,
                    AntiAlias = true
                };

                frPointPaint = new Paint
                {
                    Color = Color.Rgb(13, 71, 161),
                    StrokeWidth = 4f,
                    AntiAlias = true
                };
                frPointPaint.SetStyle(Paint.Style.Fill);
            }

            protected override void OnDraw(Canvas canvas)
            {
                base.OnDraw(canvas);
                if (canvas == null) return;

                float w = Width;
                float h = Height;
                if (w <= 0 || h <= 0) return;

                float marginLeft = 50f;
                float marginBottom = 30f;
                float marginRight = 10f;
                float marginTop = 10f;

                float x0 = marginLeft;
                float y0 = h - marginBottom;

                canvas.DrawLine(x0, y0, w - marginRight, y0, axisPaint);
                canvas.DrawLine(x0, y0, x0, marginTop, axisPaint);

                if (points == null || points.Count == 0)
                {
                    canvas.DrawText("No trace data", marginLeft, h / 2f, textPaint);
                    return;
                }

                var tvPoints = new List<DataResult>();
                DataResult frPoint = null;

                foreach (var p in points)
                {
                    if (p == null) continue;
                    if (p.Type == "TV")
                        tvPoints.Add(p);
                    else if (p.Type == "FR")
                        frPoint = p;
                }

                if (tvPoints.Count == 0)
                {
                    canvas.DrawText("No trace data (TV)", marginLeft, h / 2f, textPaint);
                    return;
                }

                decimal minX = decimal.MaxValue, maxX = decimal.MinValue;
                decimal minTq = decimal.MaxValue, maxTq = decimal.MinValue;

                foreach (var p in tvPoints)
                {
                    decimal xVal = (Mode == TraceMode.TorqueVsTime)
                        ? (decimal)p.SampleTime
                        : p.Angle;

                    if (xVal < minX) minX = xVal;
                    if (xVal > maxX) maxX = xVal;

                    if (p.Torque < minTq) minTq = p.Torque;
                    if (p.Torque > maxTq) maxTq = p.Torque;
                }

                if (minX == maxX) { minX -= 1; maxX += 1; }
                if (minTq == maxTq) { minTq -= 1; maxTq += 1; }

                float plotWidth = (w - marginLeft - marginRight);
                float plotHeight = (y0 - marginTop);

                var path = new Android.Graphics.Path();
                bool first = true;

                foreach (var p in tvPoints)
                {
                    decimal xVal = (Mode == TraceMode.TorqueVsTime)
                        ? (decimal)p.SampleTime
                        : p.Angle;

                    float xNorm = (float)((xVal - minX) / (maxX - minX));
                    float yNorm = (float)((p.Torque - minTq) / (maxTq - minTq));

                    float x = x0 + xNorm * plotWidth;
                    float y = y0 - yNorm * plotHeight;

                    if (first)
                    {
                        path.MoveTo(x, y);
                        first = false;
                    }
                    else
                    {
                        path.LineTo(x, y);
                    }
                }

                canvas.DrawPath(path, tracePaint);

                if (frPoint != null && tvPoints.Count > 0)
                {
                    decimal xValFr = (Mode == TraceMode.TorqueVsTime)
                        ? (decimal)frPoint.SampleTime
                        : frPoint.Angle;

                    DataResult nearest = tvPoints[0];
                    decimal bestDx = Math.Abs(
                        (Mode == TraceMode.TorqueVsTime ? (decimal)nearest.SampleTime : nearest.Angle) - xValFr);

                    for (int i = 1; i < tvPoints.Count; i++)
                    {
                        var p = tvPoints[i];
                        decimal xVal = (Mode == TraceMode.TorqueVsTime)
                            ? (decimal)p.SampleTime
                            : p.Angle;

                        decimal dx = Math.Abs(xVal - xValFr);
                        if (dx < bestDx)
                        {
                            bestDx = dx;
                            nearest = p;
                        }
                    }

                    decimal xPlot = xValFr;
                    decimal yPlot = nearest.Torque;

                    float xNormFr = (float)((xPlot - minX) / (maxX - minX));
                    float yNormFr = (float)((yPlot - minTq) / (maxTq - minTq));

                    float xFr = x0 + xNormFr * plotWidth;
                    float yFr = y0 - yNormFr * plotHeight;

                    canvas.DrawCircle(xFr, yFr, 8f, frPointPaint);
                }

                string xLabel = (Mode == TraceMode.TorqueVsTime) ? "Tempo (ms)" : "Ângulo (deg)";
                canvas.DrawText(xLabel, w / 2f - 60, h - 5, textPaint);

                canvas.Save();
                canvas.Rotate(-90, 15, h / 2f);
                canvas.DrawText("Torque (Nm)", 15, h / 2f, textPaint);
                canvas.Restore();
            }
        }

        // ============ CONNECTION MONITOR =============
        void StartConnectionMonitor()
        {
            try
            {
                StopConnectionMonitor(); // ensure single instance
                connectionMonitorCts = new CancellationTokenSource();
                var ct = connectionMonitorCts.Token;

                Task.Run(async () =>
                {
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            bool connected = false;
                            try
                            {
                                if (Trans != null)
                                {
                                    try
                                    {
                                        connected = Trans.IsConnected;
                                    }
                                    catch
                                    {
                                        connected = false;
                                    }
                                }
                                else
                                {
                                    connected = false;
                                }

                                // If Trans reports connected but there's no recent traffic,
                                // attempt a lightweight probe (RequestInformation) and wait shortly.
                                if (connected)
                                {
                                    // If no packets received recently (>3s), probe
                                    if (lastComm == DateTime.MinValue || (DateTime.Now - lastComm).TotalSeconds > 3)
                                    {
                                        try
                                        {
                                            Trans.RequestInformation();
                                            QueueLog("Connection monitor: sent RequestInformation probe");
                                        }
                                        catch (Exception ex)
                                        {
                                            QueueLog("Connection monitor probe send error: " + ex.Message);
                                        }

                                        // wait a bit to allow handler to update lastComm
                                        await Task.Delay(500).ConfigureAwait(false);

                                        // if still stale, mark as disconnected
                                        if (lastComm == DateTime.MinValue || (DateTime.Now - lastComm).TotalSeconds > 3)
                                        {
                                            connected = false;
                                        }
                                    }
                                }

                                // If state changed, update UI & log
                                if (connected != lastKnownConnected)
                                {
                                    lastKnownConnected = connected;
                                    RunOnUiThread(() =>
                                    {
                                        try
                                        {
                                            if (connected)
                                            {
                                                SetConnectionIndicator(true);
                                                tvStatus.Text = "Status: Connected";
                                                try { ConnectionCounter.Instance.Connected(); } catch { }

                                            }
                                            else
                                            {
                                                SetConnectionIndicator(false);
                                                //SetTestIndicator(false);
                                                tvStatus.Text = "Status: Disconnected";
                                                try { ConnectionCounter.Instance.Disconnected(); } catch { }

                                            }
                                        }
                                        catch { }
                                    });

                                    QueueLog($"Connection monitor: state changed -> {(connected ? "Connected" : "Disconnected")}");
                                }

                                /////////////////////////////////////////////////////////////////////////////
                                // dentro do loop do StartConnectionMonitor, depois da lógica que atualiza 'connected':
                                try
                                {
                                    // define threshold em segundos para considerar "rodando"
                                    double thresholdSeconds = 1.5; // ajuste entre 1.0 e 3.0 conforme desejar

                                    bool testRunning = (lastComm != DateTime.MinValue && (DateTime.Now - lastComm).TotalSeconds <= thresholdSeconds);

                                    if (testRunning != lastKnownTestRunning)
                                    {
                                        lastKnownTestRunning = testRunning;
                                        //SetTestIndicator(testRunning);
                                        QueueLog($"Test indicator changed -> {(testRunning ? "Running (green)" : "Stopped (red)")}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    QueueLog("Connection monitor test-indicator error: " + ex.Message);
                                }
                                ////////////////////////////////////////////////////////////////////////////////















                            }
                            catch (Exception ex)
                            {
                                QueueLog("Connection monitor loop error: " + ex.Message);
                            }
                        }
                        catch (TaskCanceledException) { break; }
                        catch (Exception) { /* swallow */ }

                        // wait 2 seconds between checks
                        try { await Task.Delay(2000, ct).ConfigureAwait(false); } catch { break; }
                    }
                }, ct);
            }
            catch (Exception ex)
            {
                QueueLog("StartConnectionMonitor error: " + ex.Message);
            }
        }

        void StopConnectionMonitor()
        {
            try
            {
                try
                {
                    if (connectionMonitorCts != null)
                    {
                        try { connectionMonitorCts.Cancel(); } catch { }
                        try { connectionMonitorCts.Dispose(); } catch { }
                    }
                }
                catch { }
                connectionMonitorCts = null;
            }
            catch { }
        }



        /// <summary>
        /// /aqui nova logica de botão conectar/desconectar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConnectToggle_Click(object sender, EventArgs e)
        {
            // evita reentrância por múltiplos cliques rápidos
            if (_connectOperationInProgress) return;

            // Decide ação pelo estado atual (se Trans existe e reporta conectado)
            bool currentlyConnected = false;
            try
            {
                if (Trans != null)
                {
                    try { currentlyConnected = Trans.IsConnected; } catch { currentlyConnected = false; }
                }
            }
            catch { currentlyConnected = false; }

            if (currentlyConnected)
            {
                // desconectar
                _ = Task.Run(async () =>
                {
                    _connectOperationInProgress = true;
                    try
                    {
                        await DisconnectAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        _connectOperationInProgress = false;
                    }
                });
            }
            else
            {
                // conectar
                _ = Task.Run(async () =>
                {
                    _connectOperationInProgress = true;
                    try
                    {
                        await ConnectAsync().ConfigureAwait(false);
                    }
                    finally
                    {
                        _connectOperationInProgress = false;
                    }
                });
            }
        }

        // Extracted connection logic (async)
        private async Task ConnectAsync()
        {
            try
            {
                // pega IP no UI thread
                string ip = null;
                RunOnUiThread(() =>
                {
                    ip = txtIP.Text?.Trim() ?? "";
                });
                
                ip = txtIP.Text?.Trim() ?? "";


                if (string.IsNullOrEmpty(ip))
                {
                    QueueLog("Connect: IP empty");
                    RunOnUiThread(() =>
                    {
                        if (DateTime.Now - lastToastTime > toastThrottle)
                        {
                            lastToastTime = DateTime.Now;
                            Toast.MakeText(this, "Informe IP válido", ToastLength.Short).Show();
                        }
                    });
                    return;
                }


                






                QueueLog($"Connect -> {ip}:{FIXED_PORT}");

                try
                {
                    await TryBindToActiveWifiAsync().ConfigureAwait(false);
                }
                catch (Exception) { /* não falhar se bind falhar */ }

                // Cleanup previous transducer instance if present
                if (Trans != null)
                {
                    try { Trans.RaiseError -= TransducerErrorReceiver; } catch { }
                    try { Trans.StopReadData(); Trans.StopService(); } catch { }
                    try
                    {
                        Trans.DataResult -= ResultReceiver;
                        Trans.TesteResult -= TesteResultReceiver;
                        Trans.DataInformation -= DataInformationReceiver;
                        Trans.DebugInformation -= DebugInformationReceiver;
                    }
                    catch { }
                    Trans = null;
                }

                // instantiate
                Trans = new PhoenixTransducer();
                Trans.bPrintCommToFile = true;

                // subscribe events
                Trans.DataResult += ResultReceiver;
                Trans.TesteResult += TesteResultReceiver;
                Trans.DataInformation += DataInformationReceiver;
                Trans.DebugInformation += DebugInformationReceiver;
                try { Trans.RaiseError += TransducerErrorReceiver; } catch { QueueLog("DIAG: RaiseError subscribe failed"); }

                Trans.SetPerformance(ePCSpeed.Medium, eCharPoints.VeryFew);
                Trans.Eth_IP = ip;
                Trans.Eth_Port = FIXED_PORT;

                // start service & communication in background
                Task.Run(() =>
                {
                    try
                    {
                        Trans.StartService();
                        Thread.Sleep(50);
                        Trans.StartCommunication();
                        Trans.RequestInformation();
                        QueueLog("StartCommunication & RequestInformation invoked");
                        TransducerLogAndroid.LogInfo("StartCommunication & RequestInformation invoked (IP={0})", ip);
                    }
                    catch (Exception ex)
                    {
                        QueueLog("StartService error: " + ex.Message);
                        TransducerLogAndroid.LogException(ex, "StartService");
                    }
                });

                // update UI: connecting
                RunOnUiThread(() =>
                {
                    tvStatus.Text = "Status: Connecting";
                    SetConnectionConnecting();
                    // update button label to show actioning (optional)
                    try { btnConnectIP.Text = "Connecting..."; } catch { }
                });

                // Polling loop to detect actual connected state (keeps your existing behavior)
                int tries = 0;
                int maxTries = 50;
                while (tries++ < maxTries)
                {
                    await Task.Delay(200).ConfigureAwait(false);
                    if (Trans == null) break;
                    try
                    {
                        if (Trans.IsConnected)
                        {
                            // update lastComm and known state
                            lastComm = DateTime.Now;
                            lastKnownConnected = true;
                            SetConnectionIndicator(true);

                            // Start uptime counter
                            try
                            {
                                ConnectionCounter.Instance.Connected();
                                QueueLog("ConnectionCounter: Connected() called from ConnectAsync.");
                            }
                            catch (Exception ex)
                            {
                                QueueLog("ConnectionCounter.Connected error: " + ex.Message);
                            }

                            // update UI: connected
                            RunOnUiThread(() =>
                            {
                                tvStatus.Text = "Status: Connected";
                                try { btnConnectIP.Text = "Disconnect"; } catch { }
                            });

                            QueueLog("Connection established (Trans.IsConnected==true)");
                            TransducerLogAndroid.LogInfo("Connection established (IP={0})", ip);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        QueueLog("Error reading Trans.IsConnected: " + ex.Message);
                    }
                }

                // timeout: not connected
                lastKnownConnected = false;
                SetConnectionIndicator(false);
                RunOnUiThread(() =>
                {
                    tvStatus.Text = "Status: Disconnected";
                    try { btnConnectIP.Text = "Connect"; } catch { }
                });
                QueueLog("Connection timed out (IsConnected false after polling)");
                TransducerLogAndroid.LogWarn("Connection timed out (IP={0})", ip);
            }
            catch (Exception ex)
            {
                QueueLog("ConnectAsync error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "ConnectAsync");
                RunOnUiThread(() =>
                {
                    try { btnConnectIP.Text = "Connect"; } catch { }
                    tvStatus.Text = "Status: Disconnected";
                    SetConnectionIndicator(false);
                });
            }
        }

        // Disconnect helper (async)
        private async Task DisconnectAsync()
        {
            try
            {
                QueueLog("Disconnect requested (toggle)");
                TransducerLogAndroid.LogInfo("Disconnect requested by user (toggle)");

                // notify counter immediately
                try { ConnectionCounter.Instance.Disconnected(); } catch { }

                if (Trans != null)
                {
                    try { Trans.RaiseError -= TransducerErrorReceiver; } catch { }
                    var t = Trans;
                    await Task.Run(() =>
                    {
                        try { t.StopReadData(); t.StopService(); }
                        catch (Exception ex)
                        {
                            QueueLog("Stop error: " + ex.Message);
                            TransducerLogAndroid.LogException(ex, "Disconnect.Stop");
                        }
                    }).ConfigureAwait(false);

                    try
                    {
                        Trans.DataResult -= ResultReceiver;
                        Trans.TesteResult -= TesteResultReceiver;
                        Trans.DataInformation -= DataInformationReceiver;
                        Trans.DebugInformation -= DebugInformationReceiver;
                    }
                    catch { }

                    Trans = null;
                }

                RunOnUiThread(() =>
                {
                    tvStatus.Text = "Status: Disconnected";
                    SetConnectionIndicator(false);
                    SetTestIndicator(false);
                    SetParameterInputsEnabled(true);
                    ResetCachedParams();
                    try { btnConnectIP.Text = "Connect"; } catch { }
                });

                lastKnownConnected = false;
            }
            catch (Exception ex)
            {
                QueueLog("DisconnectAsync error: " + ex.Message);
                TransducerLogAndroid.LogException(ex, "DisconnectAsync");
                RunOnUiThread(() =>
                {
                    try { btnConnectIP.Text = "Connect"; } catch { }
                    tvStatus.Text = "Status: Disconnected";
                    SetConnectionIndicator(false);
                });
            }
        }






























    }
}