using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;
using TransducerProtocol;
using SQLite;

namespace TransducerAppXA
{
    [Activity(Label = "Sync to PC")]
    public class SyncToPcActivity : Activity
    {
        // Se true, o campo IP ficará preenchido e não editável.
        const bool LOCK_IP = false;
        const string DEFAULT_IP = "192.168.4.2";
        const int DEFAULT_PORT = 5000;

        EditText edtIp;
        EditText edtPort;
        EditText edtToken;
        Button btnConnect;
        Button btnDisconnect;
        Button btnSendAll;
        Button btnSendUnsent;
        Button btnSendId;
        EditText edtSendId;
        TextView tvStatus;
        ProgressBar progressBar;

        // Log UI
        ListView lvLog;
        ArrayAdapter<string> logAdapter;

        // Tests UI
        ListView lvTests;
        ArrayAdapter<string> testsAdapter;
        List<TestDefinitionEntry> testsList = new List<TestDefinitionEntry>();

        // Usamos o driver compartilhado TransducerPcProtocol (colocado em TransducerProtocol namespace)
        TransducerPcProtocol driver;
        CancellationTokenSource sendCts;

        // Caminho DB local (mesmo que o seu TcpResultSender usava)
        string dbPath;
        TestsRepository testsRepo;

        // Keep a reference to the event handler so we can unsubscribe on destroy
        Action<string, TestPushMessage> onTestPushHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_sync_to_pc);

            // find views
            edtIp = FindViewById<EditText>(Resource.Id.edtIp);
            edtPort = FindViewById<EditText>(Resource.Id.edtPort);
            edtToken = FindViewById<EditText>(Resource.Id.edtToken);
            btnConnect = FindViewById<Button>(Resource.Id.btnConnect);
            btnDisconnect = FindViewById<Button>(Resource.Id.btnDisconnect);
            btnSendAll = FindViewById<Button>(Resource.Id.btnSendAll);
            btnSendUnsent = FindViewById<Button>(Resource.Id.btnSendUnsent);
            btnSendId = FindViewById<Button>(Resource.Id.btnSendId);
            edtSendId = FindViewById<EditText>(Resource.Id.edtSendId);
            tvStatus = FindViewById<TextView>(Resource.Id.tvStatus);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            lvLog = FindViewById<ListView>(Resource.Id.lvLog);
            lvTests = FindViewById<ListView>(Resource.Id.lvTests);

            // adapters
            logAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            lvLog.Adapter = logAdapter;

            testsAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            lvTests.Adapter = testsAdapter;

            // dbPath: mesma pasta que seu SQLite usa (Personal)
            dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "transducer.db3");

            // create repository
            testsRepo = new TestsRepository(dbPath);
            testsRepo.Initialize();

            // load saved tests
            LoadTestsList();

            // prefill token with the token you've chosen (you can change in UI)
            edtToken.Text = "2025_DEZ_TRANSDUCER_@A@C";

            // instantiate driver (we'll create it on connect with token)
            driver = new TransducerPcProtocol(edtToken.Text.Trim());

            // basic logging handlers (these will be reattached when driver recreated on connect)
            driver.OnLog += (s) => RunOnUiThread(() =>
            {
                logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - {s}", 0);
            });
            driver.OnMessageReceived += (msg) =>
            {
                // if needed, show message types in log
                RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Received {msg.Type}", 0));
            };
            driver.OnAckReceived += (ack) =>
            {
                RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - ACK id={ack.Id} status={ack.Status}", 0));
            };
            driver.OnDisconnected += (ex) =>
            {
                RunOnUiThread(() =>
                {
                    logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Disconnected.", 0);
                    SetUiConnected(false);
                });
            };

            // subscribe to test_push events (Iteração 1 & 2)
            onTestPushHandler = (rawJson, dto) => OnTestPushReceivedHandler(rawJson, dto);
            TransducerPcProtocol.OnTestPushReceived += onTestPushHandler;

            // Preencher IP e porta por padrão
            edtIp.Text = DEFAULT_IP;
            edtPort.Text = DEFAULT_PORT.ToString();

            if (LOCK_IP)
            {
                // torna o campo não editável (aparência normal)
                edtIp.Focusable = false;
                edtIp.FocusableInTouchMode = false;
                edtIp.Clickable = false;
            }

            btnConnect.Click += async (s, e) =>
            {
                string ip = edtIp.Text.Trim();
                if (!int.TryParse(edtPort.Text.Trim(), out int port)) port = DEFAULT_PORT;
                string token = edtToken.Text.Trim();

                SetUiConnected(false);
                tvStatus.Text = "Connecting...";
                // create a new driver instance with token (or reuse)
                try { driver?.Dispose(); } catch { }
                driver = new TransducerPcProtocol(token);

                // reattach instance level handlers for logging/ack/disconnect
                driver.OnLog += (msg) => RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - {msg}", 0));
                driver.OnAckReceived += (ack) => RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - ACK id={ack.Id} status={ack.Status}", 0));
                driver.OnMessageReceived += (m) => RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Msg {m.Type}", 0));
                driver.OnDisconnected += (ex) => RunOnUiThread(() => { SetUiConnected(false); logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Disconnected", 0); });

                // IMPORTANT: OnTestPushReceived is static and we subscribed above; no need to subscribe here.

                bool ok = await driver.ConnectAsync(ip, port, useTls: false, token: token);
                SetUiConnected(ok);
                if (!ok) Toast.MakeText(this, "Connect failed", ToastLength.Short).Show();
            };

            btnDisconnect.Click += (s, e) =>
            {
                try { driver?.Disconnect(); } catch { }
                SetUiConnected(false);
            };

            btnSendAll.Click += async (s, e) =>
            {
                if (!driver.IsConnected)
                {
                    Toast.MakeText(this, "Not connected", ToastLength.Short).Show();
                    return;
                }

                // desabilita botões enquanto envia
                SetSendingUi(true);
                sendCts?.Cancel();
                sendCts = new CancellationTokenSource();
                progressBar.Progress = 0;

                // envia tudo (inclusive já enviados) -> pass sendAll = true
                await Task.Run(() => SendAllInternalAsync(sendAll: true, perItemTimeoutMs: 20000, cancellation: sendCts.Token));

                SetSendingUi(false);
                Toast.MakeText(this, "SendAll finished", ToastLength.Short).Show();
            };

            btnSendUnsent.Click += async (s, e) =>
            {
                if (!driver.IsConnected)
                {
                    Toast.MakeText(this, "Not connected", ToastLength.Short).Show();
                    return;
                }

                SetSendingUi(true);
                sendCts?.Cancel();
                sendCts = new CancellationTokenSource();
                progressBar.Progress = 0;

                // envia só não enviados (recomendado) -> sendAll = false
                await Task.Run(() => SendAllInternalAsync(sendAll: false, perItemTimeoutMs: 20000, cancellation: sendCts.Token));

                SetSendingUi(false);
                Toast.MakeText(this, "SendUnsent finished", ToastLength.Short).Show();
            };

            btnSendId.Click += async (s, e) =>
            {
                if (!driver.IsConnected)
                {
                    Toast.MakeText(this, "Not connected", ToastLength.Short).Show();
                    return;
                }

                if (!int.TryParse(edtSendId.Text.Trim(), out int resultId))
                {
                    Toast.MakeText(this, "Id inválido", ToastLength.Short).Show();
                    return;
                }

                SetSendingUi(true);
                sendCts?.Cancel();
                sendCts = new CancellationTokenSource();

                bool ok = await SendSingleResultByIdAsync(resultId, 20000, sendCts.Token);
                SetSendingUi(false);

                Toast.MakeText(this, ok ? "SendResult ok" : "SendResult failed", ToastLength.Short).Show();
            };

            // When user taps an item in tests list, show detail and option to "Start" later
            lvTests.ItemClick += (s, e) =>
            {
                var entry = testsList.ElementAtOrDefault(e.Position);
                if (entry == null) return;
                // show detail dialog with full JSON
                var builder = new AlertDialog.Builder(this);
                builder.SetTitle(entry.Name ?? entry.TestId);
                var message = $"TestId: {entry.TestId}\nName: {entry.Name}\nNominal: {entry.NominalTorque}\nMin: {entry.MinTorque}\nMax: {entry.MaxTorque}\nNotes: {entry.Notes}\n\nRaw JSON:\n{entry.RawJson}";
                builder.SetMessage(message);
                builder.SetPositiveButton("OK", (sender, args) => { });
                // future: add "Iniciar Teste" button
                builder.Show();
            };

            SetUiConnected(false);
        }

        void SetUiConnected(bool connected)
        {
            RunOnUiThread(() =>
            {
                btnConnect.Enabled = !connected;
                btnDisconnect.Enabled = connected;
                btnSendAll.Enabled = connected;
                btnSendUnsent.Enabled = connected;
                btnSendId.Enabled = connected;
                tvStatus.Text = connected ? "Connected" : "Disconnected";
            });
        }

        void SetSendingUi(bool sending)
        {
            RunOnUiThread(() =>
            {
                btnSendAll.Enabled = !sending;
                btnSendUnsent.Enabled = !sending;
                btnConnect.Enabled = !sending;
                btnDisconnect.Enabled = !sending;
                btnSendId.Enabled = !sending;
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try { driver?.Dispose(); } catch { }
            sendCts?.Cancel();

            // unsubscribe the static event to avoid leaks
            try
            {
                if (onTestPushHandler != null)
                    TransducerPcProtocol.OnTestPushReceived -= onTestPushHandler;
            }
            catch { }

            try { testsRepo?.Dispose(); } catch { }
        }

        // Handler for incoming test_push messages (Iteração 2: persist + UI update)
        void OnTestPushReceivedHandler(string rawJson, TestPushMessage dto)
        {
            try
            {
                // Log and show dialog as before
                RunOnUiThread(() =>
                {
                    try
                    {
                        logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Msg test_push", 0);

                        var preview = rawJson ?? "<empty>";
                        if (preview.Length > 2000) preview = preview.Substring(0, 2000) + " … (truncated)";

                        var dlgBuilder = new Android.App.AlertDialog.Builder(this);
                        dlgBuilder.SetTitle("test_push recebido");
                        dlgBuilder.SetMessage(preview);
                        dlgBuilder.SetPositiveButton("OK", (sender, args) => { /* ok */ });
                        dlgBuilder.Show();

                        if (dto != null)
                        {
                            var tid = dto.TestId ?? "(no id)";
                            var name = string.IsNullOrEmpty(dto.Name) ? "(no name)" : dto.Name;
                            Toast.MakeText(this, $"Teste recebido: {tid} - {name}", ToastLength.Short).Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        TransducerLogAndroid.LogError("SyncToPcActivity", "OnTestPushReceived UI error: " + ex.Message);
                    }
                });

                // Validate minimal fields and persist in background
                Task.Run(() =>
                {
                    try
                    {
                        // If dto is null, try to parse to TestPushMessage
                        TestPushMessage push = dto;
                        if (push == null)
                        {
                            try { push = ProtocolJson.Deserialize<TestPushMessage>(rawJson); }
                            catch { push = null; }
                        }

                        if (push == null || string.IsNullOrEmpty(push.TestId))
                        {
                            // invalid message — log and return
                            RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Ignored invalid test_push (no test_id)", 0));
                            return;
                        }

                        var entry = new TestDefinitionEntry
                        {
                            TestId = push.TestId,
                            Name = push.Name,
                            NominalTorque = push.NominalTorque,
                            MinTorque = push.MinTorque,
                            MaxTorque = push.MaxTorque,
                            Repetitions = push.Repetitions,
                            Notes = push.Notes,
                            CreatedBy = push.CreatedBy,
                            TimestampUtc = push.TimestampUtc,
                            RawJson = rawJson,
                            ReceivedAtUtc = DateTime.UtcNow
                        };

                        // insert or replace
                        try
                        {
                            testsRepo.InsertOrReplace(entry);
                        }
                        catch (Exception ex)
                        {
                            RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - DB insert error: {ex.Message}", 0));
                            return;
                        }

                        // reload list on UI thread
                        RunOnUiThread(() =>
                        {
                            LoadTestsList();
                            logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Test saved: {entry.TestId}", 0);
                        });
                    }
                    catch (Exception ex)
                    {
                        RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - OnTestPush process error: {ex.Message}", 0));
                    }
                });
            }
            catch { }
        }

        void LoadTestsList()
        {
            try
            {
                var list = testsRepo.GetAll();
                testsList = list ?? new List<TestDefinitionEntry>();
                testsAdapter.Clear();
                foreach (var t in testsList) testsAdapter.Add(t.ToString());
                testsAdapter.NotifyDataSetChanged();
            }
            catch (Exception ex)
            {
                logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - LoadTestsList error: {ex.Message}", 0);
            }
        }

        // ------------------------------
        // DB / Send helpers (kept as in your original)
        // ------------------------------

        async Task SendAllInternalAsync(bool sendAll, int perItemTimeoutMs, CancellationToken cancellation)
        {
            // open local DB (same path used elsewhere)
            SQLiteConnection conn = null;
            try
            {
                conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - DB open error: {ex.Message}", 0));
                return;
            }

            List<ResultEntry> list = null;
            try
            {
                list = conn.Table<ResultEntry>().OrderBy(r => r.TimestampUtc).ToList();
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - DB query error: {ex.Message}", 0));
                conn.Close();
                return;
            }

            int total = list.Count;
            int sentCount = 0;
            RunOnUiThread(() => { progressBar.Max = Math.Max(total, 1); progressBar.Progress = 0; tvStatus.Text = $"Sent 0/{total}"; });

            foreach (var r in list)
            {
                if (cancellation.IsCancellationRequested) break;

                bool alreadySent = IsResultMarkedSent(r.Id);
                if (!sendAll && alreadySent)
                {
                    sentCount++;
                    RunOnUiThread(() => { progressBar.Progress = sentCount; tvStatus.Text = $"Sent {sentCount}/{total}"; });
                    continue;
                }

                bool ok = await SendSingleResultInternalAsync(r, perItemTimeoutMs, cancellation);

                if (ok) MarkResultSent(r.Id);

                sentCount++;
                RunOnUiThread(() => { progressBar.Progress = sentCount; tvStatus.Text = $"Sent {sentCount}/{total}"; });
            }

            conn.Close();
        }

        async Task<bool> SendSingleResultInternalAsync(ResultEntry rEntry, int perItemTimeoutMs, CancellationToken cancellation)
        {
            try
            {
                // build ResultMessage (ProtocolMessages DTO)
                var rm = new ResultMessage
                {
                    Id = rEntry.Id.ToString(),
                    TimestampUtc = rEntry.TimestampUtc.ToString("o"),
                    Torque = Convert.ToDouble(rEntry.Torque),
                    Angle = Convert.ToDouble(rEntry.Angle),
                    Status = InferStatusFromText(rEntry.Text),
                    Text = rEntry.Text ?? "",
                    Trace = new List<TracePointDto>()
                };

                // load trace points from local DB, compute FR point and exclude from trace array
                try
                {
                    var tmpConn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
                    var pts = tmpConn.Table<TracePointEntry>().Where(tp => tp.ResultId == rEntry.Id).OrderBy(tp => tp.PointIndex).ToList();
                    tmpConn.Close();

                    int frIdxInList = -1;
                    double bestDiff = double.MaxValue;
                    for (int i = 0; i < pts.Count; i++)
                    {
                        double pTorque = Convert.ToDouble(pts[i].Torque);
                        double diff = Math.Abs(pTorque - Convert.ToDouble(rEntry.Torque));
                        if (diff < bestDiff - 1e-12) { bestDiff = diff; frIdxInList = i; }
                        else if (Math.Abs(diff - bestDiff) <= 1e-12)
                        {
                            if (frIdxInList >= 0 && pts[i].PointIndex > pts[frIdxInList].PointIndex) frIdxInList = i;
                        }
                    }

                    if (frIdxInList >= 0)
                    {
                        var frp = pts[frIdxInList];
                        rm.FrIndex = frp.PointIndex;
                        rm.FrTime = Convert.ToDouble(frp.TimeMs);
                        rm.FrAngle = Convert.ToDouble(frp.Angle);
                    }

                    // add all pts except fr
                    foreach (var p in pts)
                    {
                        if (frIdxInList >= 0 && p.PointIndex == pts[frIdxInList].PointIndex) continue;
                        rm.Trace.Add(new TracePointDto
                        {
                            PointIndex = p.PointIndex,
                            TimeMs = Convert.ToDouble(p.TimeMs),
                            Torque = Convert.ToDouble(p.Torque),
                            Angle = Convert.ToDouble(p.Angle)
                        });
                    }
                }
                catch (Exception)
                {
                    rm.Trace = null;
                }

                // send via driver and await ack
                var ack = await driver.SendResultAsync(rm, timeoutMs: perItemTimeoutMs, maxAttempts: 4, cancellation: cancellation);
                if (ack != null && (ack.Status == "stored" || ack.Status == "received"))
                {
                    RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - ACK received for id={rm.Id} status={ack.Status}", 0));
                    return true;
                }
                else
                {
                    RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - No ACK / failed for id={rm.Id}", 0));
                    return false;
                }
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - SendSingle error: {ex.Message}", 0));
                return false;
            }
        }

        async Task<bool> SendSingleResultByIdAsync(int resultId, int perItemTimeoutMs, CancellationToken cancellation)
        {
            try
            {
                var tmpConn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
                var r = tmpConn.Table<ResultEntry>().FirstOrDefault(x => x.Id == resultId);
                tmpConn.Close();
                if (r == null) { RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - Result not found: {resultId}", 0)); return false; }

                return await SendSingleResultInternalAsync(r, perItemTimeoutMs, cancellation);
            }
            catch (Exception ex)
            {
                RunOnUiThread(() => logAdapter.Insert($"{DateTime.Now:HH:mm:ss} - SendById DB read error: {ex.Message}", 0));
                return false;
            }
        }

        // MARK RESULT AS SENT: uses SentResultEntry table (same schema as previous TcpResultSender)
        void MarkResultSent(int resultId)
        {
            try
            {
                var conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
                conn.CreateTable<SentResultEntry>();
                conn.Insert(new SentResultEntry { ResultId = resultId, SentAtUtc = DateTime.UtcNow });
                conn.Close();
            }
            catch { }
        }

        bool IsResultMarkedSent(int resultId)
        {
            try
            {
                var conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
                var e = conn.Table<SentResultEntry>().FirstOrDefault(x => x.ResultId == resultId);
                conn.Close();
                return e != null;
            }
            catch { return false; }
        }

        string InferStatusFromText(string text)
        {
            try
            {
                var txt = text ?? "";
                if (txt.Contains("[OK")) return "OK";
                if (txt.Contains("[NOK")) return "NOK";
            }
            catch { }
            return "N/A";
        }
    }
}