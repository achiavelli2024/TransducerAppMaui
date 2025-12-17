using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using TransducerProtocol;

namespace TransducerAppXA
{
    public class SentResultEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ResultId { get; set; }
        public DateTime SentAtUtc { get; set; }
    }

    public class TcpResultSender : IDisposable
    {
        readonly Context ctx;
        readonly string dbPath;
        readonly SQLiteConnection conn;
        TcpClient client;
        Stream netStream;
        StreamReader reader;
        StreamWriter writer;
        CancellationTokenSource connectCts;
        readonly SemaphoreSlim sendLock = new SemaphoreSlim(1, 1);

        public bool UseTls { get; set; } = false;
        public string ServerIp { get; set; }
        public int ServerPort { get; set; } = 5000;
        public string ServerCertThumbprint { get; set; } = null;
        public bool AcceptAllCertificatesForTesting { get; set; } = false;
        public string ApiToken { get; set; } = "";

        public event Action<string> OnLog;
        public event Action<int, int> OnProgress;

        public TcpResultSender(Context context, string dbFileName = "transducer.db3")
        {
            ctx = context ?? throw new ArgumentNullException(nameof(context));
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbFileName);

            conn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
            try { conn.CreateTable<SentResultEntry>(); } catch { }
            try { conn.CreateTable<ResultEntry>(); } catch { }
            try { conn.CreateTable<TracePointEntry>(); } catch { }
        }

        void Log(string s) { try { OnLog?.Invoke(s); } catch { } }

        public bool IsConnected => client?.Connected ?? false;

        public async Task<bool> ConnectAsync(string ip, int port, string token = "", int timeoutMs = 5000)
        {
            if (string.IsNullOrWhiteSpace(ip)) throw new ArgumentNullException(nameof(ip));
            ServerIp = ip; ServerPort = port; ApiToken = token ?? "";

            Disconnect();

            connectCts = new CancellationTokenSource();

            try
            {
                client = new TcpClient();
                var connectTask = client.ConnectAsync(ip, port);
                var delay = Task.Delay(timeoutMs);
                var completed = await Task.WhenAny(connectTask, delay);
                if (completed != connectTask)
                {
                    Log($"Connect timeout to {ip}:{port}");
                    client.Close(); client = null;
                    return false;
                }

                if (!client.Connected) { Log($"Unable to connect to {ip}:{port}"); return false; }

                var baseStream = client.GetStream();

                if (UseTls)
                {
                    var ssl = new SslStream(baseStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                    try
                    {
                        await ssl.AuthenticateAsClientAsync(ip);
                        netStream = ssl;
                        Log("TLS negotiation succeeded.");
                    }
                    catch (Exception ex)
                    {
                        Log("TLS auth error: " + ex.Message);
                        try { ssl.Dispose(); } catch { }
                        client.Close(); client = null;
                        return false;
                    }
                }
                else
                {
                    netStream = baseStream;
                }

                reader = new StreamReader(netStream, Encoding.UTF8);
                writer = new StreamWriter(netStream, Encoding.UTF8) { AutoFlush = true };

                var hello = new HelloMessage
                {
                    Client = "TransducerAppXA",
                    ClientVersion = "1.0",
                    Device = Android.OS.Build.Model,
                    Token = ApiToken ?? ""
                };

                var helloJson = ProtocolJson.Serialize(hello);

                await sendLock.WaitAsync();
                try
                {
                    await writer.WriteLineAsync(helloJson);
                    Log("Sent hello");
                    string resp = await ReadLineWithTimeoutAsync(5000);
                    if (resp == null) { Log("No hello_ack"); }
                    else
                    {
                        try
                        {
                            var j = JObject.Parse(resp);
                            var tp = (string)j["type"];
                            if (tp == "hello_ack") Log("Received hello_ack from server.");
                            else Log("Hello response unexpected: " + resp);
                        }
                        catch (Exception ex) { Log("Hello ack parse error: " + ex.Message); }
                    }
                }
                finally { sendLock.Release(); }

                return true;
            }
            catch (Exception ex)
            {
                Log("Connect error: " + ex.Message);
                Disconnect();
                return false;
            }
        }

        bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            try
            {
                if (AcceptAllCertificatesForTesting) return true;
                if (certificate == null) return false;
                var cert2 = new X509Certificate2(certificate);
                var thumb = cert2.Thumbprint?.Replace(" ", "").ToUpperInvariant();
                if (!string.IsNullOrEmpty(ServerCertThumbprint))
                {
                    var expected = ServerCertThumbprint.Replace(" ", "").ToUpperInvariant();
                    return thumb == expected;
                }
                return sslPolicyErrors == SslPolicyErrors.None;
            }
            catch { return false; }
        }

        async Task<string> ReadLineWithTimeoutAsync(int timeoutMs)
        {
            if (reader == null) return null;
            var readTask = reader.ReadLineAsync();
            if (timeoutMs == Timeout.Infinite) return await readTask;
            var delay = Task.Delay(timeoutMs);
            var completed = await Task.WhenAny(readTask, delay);
            if (completed == readTask) { try { return await readTask; } catch { return null; } }
            return null;
        }

        public void Disconnect()
        {
            try { connectCts?.Cancel(); } catch { }
            try { writer?.Close(); } catch { }
            try { reader?.Close(); } catch { }
            try { netStream?.Close(); } catch { }
            try { client?.Close(); } catch { }
            writer = null; reader = null; netStream = null; client = null; connectCts = null;
            Log("Disconnected.");
        }

        public async Task SendAllAsync(bool sendAll = false, int perItemTimeoutMs = 10000, CancellationToken cancellation = default)
        {
            if (!IsConnected) { Log("Not connected."); return; }

            SQLiteConnection localConn = null;
            try { localConn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex); }
            catch (Exception ex) { Log("DB open error: " + ex.Message); return; }

            List<ResultEntry> list;
            try { list = localConn.Table<ResultEntry>().OrderBy(r => r.TimestampUtc).ToList(); }
            catch (Exception ex) { Log("DB query error: " + ex.Message); localConn.Close(); return; }

            int total = list.Count;
            int sentCount = 0;
            OnProgress?.Invoke(sentCount, total);

            foreach (var r in list)
            {
                if (cancellation.IsCancellationRequested) break;
                if (!sendAll && IsResultAlreadySent(r.Id)) { OnProgress?.Invoke(++sentCount, total); continue; }
                bool ok = await SendSingleResultInternalAsync(r, perItemTimeoutMs, cancellation);
                if (ok) MarkResultSent(r.Id);
                OnProgress?.Invoke(++sentCount, total);
            }

            localConn.Close();
        }

        async Task<bool> SendSingleResultInternalAsync(ResultEntry rEntry, int perItemTimeoutMs, CancellationToken cancellation)
        {
            try
            {
                string statusVal = "N/A";
                try { var txt = rEntry.Text ?? ""; if (txt.Contains("[OK")) statusVal = "OK"; else if (txt.Contains("[NOK")) statusVal = "NOK"; } catch { }

                var rm = new ResultMessage
                {
                    Id = rEntry.Id.ToString(),
                    TimestampUtc = rEntry.TimestampUtc.ToString("o"),
                    Torque = Convert.ToDouble(rEntry.Torque),
                    Angle = Convert.ToDouble(rEntry.Angle),
                    Status = statusVal,
                    Text = rEntry.Text ?? "",
                    Trace = new List<TracePointDto>(),
                    CommandId = null
                };

                // load trace points from local DB and add to rm.Trace (skip FR if you want)
                try
                {
                    var tmp = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
                    var pts = tmp.Table<TracePointEntry>().Where(tp => tp.ResultId == rEntry.Id).OrderBy(tp => tp.PointIndex).ToList();
                    tmp.Close();
                    int nearestIndex = -1;
                    double bestDiff = double.MaxValue;
                    for (int i = 0; i < pts.Count; i++)
                    {
                        var p = pts[i];
                        // convert p.Torque (may be decimal) to double for comparison
                        double pTorque = Convert.ToDouble(p.Torque);
                        double rTorque = Convert.ToDouble(rEntry.Torque);
                        double diff = Math.Abs(pTorque - rTorque);
                        if (diff < bestDiff - 1e-12) { bestDiff = diff; nearestIndex = i; }
                        else if (Math.Abs(diff - bestDiff) <= 1e-12)
                        {
                            if (nearestIndex >= 0 && p.PointIndex > pts[nearestIndex].PointIndex) nearestIndex = i;
                        }
                    }
                    if (nearestIndex >= 0)
                    {
                        var frp = pts[nearestIndex];
                        rm.FrIndex = frp.PointIndex;
                        rm.FrTime = Convert.ToDouble(frp.TimeMs);
                        rm.FrAngle = Convert.ToDouble(frp.Angle);
                    }
                    // add all points EXCEPT FR
                    foreach (var p in pts)
                    {
                        if (nearestIndex >= 0 && p.PointIndex == pts[nearestIndex].PointIndex) continue;
                        rm.Trace.Add(new TracePointDto
                        {
                            PointIndex = p.PointIndex,
                            TimeMs = Convert.ToDouble(p.TimeMs),
                            Torque = Convert.ToDouble(p.Torque),
                            Angle = Convert.ToDouble(p.Angle)
                        });
                    }
                }
                catch { rm.Trace = null; }

                string json = ProtocolJson.Serialize(rm);
                Log($"Sending id={rm.Id}");

                int attempts = 0;
                int maxAttempts = 4;
                int backoffMs = 500;
                bool ackOk = false;

                while (attempts < maxAttempts && !ackOk && !cancellation.IsCancellationRequested)
                {
                    attempts++;
                    await sendLock.WaitAsync(cancellation);
                    try
                    {
                        if (writer == null) { Log("Writer not available"); break; }
                        await writer.WriteLineAsync(json);
                        string resp = await ReadLineWithTimeoutAsync(perItemTimeoutMs);
                        if (resp != null)
                        {
                            try
                            {
                                var j = JObject.Parse(resp);
                                var tp = (string)j["type"];
                                if (tp == "ack")
                                {
                                    var ackMsg = ProtocolJson.Deserialize<AckMessage>(resp);
                                    if (ackMsg != null && ackMsg.Id == rm.Id && ackMsg.AckMessageId == rm.MessageId)
                                    {
                                        if (ackMsg.Status == "stored" || ackMsg.Status == "received")
                                        {
                                            ackOk = true;
                                            Log($"ACK received for id={rm.Id} status={ackMsg.Status}");
                                            return true;
                                        }
                                        else
                                        {
                                            Log($"ACK status {ackMsg.Status} for id={rm.Id} info={ackMsg.Info}");
                                        }
                                    }
                                    else
                                    {
                                        Log("ACK mismatch or missing ack_message_id.");
                                    }
                                }
                                else
                                {
                                    Log("Non-ack response: " + resp);
                                }
                            }
                            catch (Exception ex) { Log("Ack parse error: " + ex.Message); }
                        }
                        else
                        {
                            Log($"No ACK for id={rm.Id} attempt {attempts}");
                        }
                    }
                    catch (Exception ex) { Log("Send error: " + ex.Message); if (!IsConnected) break; }
                    finally { try { sendLock.Release(); } catch { } }

                    if (!ackOk) await Task.Delay(backoffMs, cancellation).ContinueWith(_ => { });
                    backoffMs *= 2;
                }

                return false;
            }
            catch (Exception ex) { Log("SendSingleResultInternalAsync error: " + ex.Message); return false; }
        }

        public async Task<bool> SendResultByIdAsync(int resultId, int perItemTimeoutMs = 10000, CancellationToken cancellation = default)
        {
            if (!IsConnected) { Log("Not connected."); return false; }

            ResultEntry r = null;
            try
            {
                var tmpConn = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadOnly | SQLiteOpenFlags.FullMutex);
                r = tmpConn.Table<ResultEntry>().FirstOrDefault(x => x.Id == resultId);
                tmpConn.Close();
            }
            catch (Exception ex) { Log("DB read error: " + ex.Message); return false; }

            if (r == null) { Log("Result not found: " + resultId); return false; }

            return await SendSingleResultInternalAsync(r, perItemTimeoutMs, cancellation);
        }

        bool IsResultAlreadySent(int resultId)
        {
            try { return conn.Table<SentResultEntry>().FirstOrDefault(x => x.ResultId == resultId) != null; }
            catch { return false; }
        }

        void MarkResultSent(int resultId)
        {
            try { conn.Insert(new SentResultEntry { ResultId = resultId, SentAtUtc = DateTime.UtcNow }); } catch { }
        }

        public void Dispose()
        {
            try { Disconnect(); } catch { }
            try { conn?.Close(); } catch { }
            try { conn?.Dispose(); } catch { }
            try { sendLock?.Dispose(); } catch { }
        }
    }
}