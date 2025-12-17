using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransducerAppMaui.Helpers;


namespace TransducerAppMaui.Logs
{
    /// <summary>
    /// TransducerLogAndroid
    /// - Logger central para o app: coleta logs de envio/recebimento de telegramas,
    ///   parâmetros, erros, estados, direciona para UI via evento e grava em SQLite via DbHelper.
    /// - Projetado para ser inicializado a partir do MainActivity (Initialize).
    /// - Use TransducerLogAndroid.LogSend(...), LogReceive(...), LogInfo(...), LogError(...),
    ///   LogParameterChange(...), LogException(...), etc.
    /// - Subscritores: MainActivity deve subscrever OnLogAppended para encaminhar para a UI.
    /// </summary>
    public static class TransducerLogAndroid
    {
        public enum LogLevel { Debug, Info, Warn, Error, Send, Receive, Param }

        // Registro estruturado emitido para UI
        public class LogRecord
        {
            public DateTime TimestampUtc { get; set; }
            public LogLevel Level { get; set; }
            public string Category { get; set; }   // ex: "COMM", "PROTO", "UI", "PARAM"
            public string Message { get; set; }    // mensagem legível
            public string RawHex { get; set; }     // opcional: payload hex
            public override string ToString()
            {
                var t = TimestampUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                var lvl = Level.ToString().ToUpper();
                var cat = string.IsNullOrEmpty(Category) ? "" : $"[{Category}] ";
                var raw = string.IsNullOrEmpty(RawHex) ? "" : $" | HEX: {RawHex}";
                return $"{t} - {lvl} - {cat}{Message}{raw}";
            }
        }

        // Evento público que a UI (MainActivity) subscreve para receber logs em tempo real.
        // Emissão ocorre em thread do produtor; assinante deve usar RunOnUiThread.
        public static event Action<LogRecord> OnLogAppended;

        // Optional callback invoked when batch persist finished (for diagnostics)
        public static event Action<int> OnBatchPersisted;

        // Internal queue to buffer logs before persisting to SQLite
        static readonly ConcurrentQueue<LogRecord> _queue = new ConcurrentQueue<LogRecord>();

        // DB helper reference (inicializado por MainActivity)
        static DbHelper _db = null;

        // flush task control
        static CancellationTokenSource _cts;
        static Task _flusherTask;
        static readonly object _initLock = new object();

        // flush tuning - defaults (can be changed at Initialize)
        static int _flushIntervalMs = 400;   // flush timer
        static int _flushBatchSize = 100;    // max itens por batch

        // initialized flag
        static bool _initialized = false;

        /// <summary>
        /// Inicializa o logger. Deve ser chamado uma vez no OnCreate do MainActivity.
        /// Passar a instância de DbHelper do app para persistência em SQLite.
        /// </summary>
        public static void Initialize(DbHelper dbHelper, int flushIntervalMs = 400, int flushBatchSize = 100)
        {
            if (dbHelper == null) throw new ArgumentNullException(nameof(dbHelper));

            lock (_initLock)
            {
                if (_initialized) return;
                _db = dbHelper;
                _flushIntervalMs = Math.Max(50, flushIntervalMs);
                _flushBatchSize = Math.Max(10, flushBatchSize);

                _cts = new CancellationTokenSource();
                _flusherTask = Task.Run(() => FlusherLoopAsync(_cts.Token), _cts.Token);

                _initialized = true;

                // Emit a startup info log
                LogInfo("TransducerLogAndroid initialized (flushIntervalMs={0} batchSize={1})", _flushIntervalMs, _flushBatchSize);
            }
        }

        /// <summary>Stops background flusher. Call from Activity.OnDestroy if necessary.</summary>
        public static void Shutdown()
        {
            lock (_initLock)
            {
                if (!_initialized) return;
                try
                {
                    _cts.Cancel();
                    _flusherTask?.Wait(1000);
                }
                catch { }
                _cts?.Dispose();
                _cts = null;
                _flusherTask = null;
                _initialized = false;
            }
        }

        // ---------- Public logging API (high level helpers) ----------

        public static void LogInfo(string fmt, params object[] args) => Enqueue(LogLevel.Info, "APP", string.Format(fmt, args));
        public static void LogDebug(string fmt, params object[] args) => Enqueue(LogLevel.Debug, "APP", string.Format(fmt, args));
        public static void LogWarn(string fmt, params object[] args) => Enqueue(LogLevel.Warn, "APP", string.Format(fmt, args));
        public static void LogError(string fmt, params object[] args) => Enqueue(LogLevel.Error, "APP", string.Format(fmt, args));
        public static void LogException(Exception ex, string context = null)
        {
            try
            {
                var msg = context != null ? $"{context} - {ex.Message}" : ex.Message;
                var sb = new StringBuilder();
                sb.AppendLine(msg);
                sb.AppendLine(ex.ToString());
                Enqueue(LogLevel.Error, "EX", sb.ToString());
            }
            catch { }
        }

        public static void LogParameterChange(string paramName, string oldValue, string newValue)
        {
            Enqueue(LogLevel.Param, "PARAM", $"Parameter changed: {paramName}  from='{oldValue}' to='{newValue}'");
        }

        // When sending a telegram/command to device
        public static void LogSend(string directionDescription, byte[] raw, string humanText = null)
        {
            var hex = raw != null ? ByteArrayToHex(raw) : null;
            var message = !string.IsNullOrEmpty(humanText) ? humanText : "Telegram sent";
            Enqueue(LogLevel.Send, "COMM", message, hex);
        }

        // When receiving a telegram from device
        public static void LogReceive(string directionDescription, byte[] raw, string humanText = null)
        {
            var hex = raw != null ? ByteArrayToHex(raw) : null;
            var message = !string.IsNullOrEmpty(humanText) ? humanText : "Telegram received";
            Enqueue(LogLevel.Receive, "COMM", message, hex);
        }

        // Generic low-level log (used by PhoenixTransducer if you call it)
        public static void LogRaw(string category, string text, byte[] raw = null)
        {
            var hex = raw != null ? ByteArrayToHex(raw) : null;
            Enqueue(LogLevel.Debug, category ?? "RAW", text, hex);
        }

        // ---------- Internal helpers ----------

        static void Enqueue(LogLevel level, string category, string message, string rawHex = null)
        {
            try
            {
                var rec = new LogRecord
                {
                    TimestampUtc = DateTime.UtcNow,
                    Level = level,
                    Category = category,
                    Message = message,
                    RawHex = rawHex
                };
                _queue.Enqueue(rec);

                // notify UI immediately (non-blocking). MainActivity must handle UI threading.
                try { OnLogAppended?.Invoke(rec); } catch { }

                // If queue grows too large, signal immediate short flush by kicking flusher thread via Task
                if (_queue.Count > 2000)
                {
                    Task.Run(() => FlushOneBatchAsync().ConfigureAwait(false));
                }
            }
            catch { /* never throw from logger */ }
        }

        // Flusher loop: persiste periodicamente em batches na DB
        static async Task FlusherLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(_flushIntervalMs, token).ConfigureAwait(false);
                    await FlushOneBatchAsync().ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException) { }
            catch (Exception)
            {
                // swallow to keep logger running
            }
        }

        // Persiste um batch para o DB (assíncrono)
        static async Task FlushOneBatchAsync()
        {
            try
            {
                if (_db == null) return;
                var batch = new List<LogRecord>();
                while (batch.Count < _flushBatchSize && _queue.TryDequeue(out var rec))
                {
                    batch.Add(rec);
                }
                if (batch.Count == 0) return;

                // Persist each item (DbHelper expected to have InsertLog(LogEntry) as used in the app)
                int persisted = 0;
                foreach (var r in batch)
                {
                    try
                    {
                        // create/translate to LogEntry if exists
                        try
                        {
                            var entry = new LogEntry { TimestampUtc = r.TimestampUtc, Message = r.ToString() };
                            // try db.InsertLog(entry) - DbHelper from repo uses this earlier
                            _db.InsertLog(entry);
                            persisted++;
                        }
                        catch
                        {
                            // fallback: try reflection InsertLog method name variations
                            try
                            {
                                var method = _db.GetType().GetMethod("InsertLog");
                                if (method != null) method.Invoke(_db, new object[] { new LogEntry { TimestampUtc = r.TimestampUtc, Message = r.ToString() } });
                                persisted++;
                            }
                            catch { /* ignore per-item */ }
                        }
                    }
                    catch { /* ignore per-item */ }
                }

                // notify subscribers that a batch was persisted
                try { OnBatchPersisted?.Invoke(persisted); } catch { }
            }
            catch { /* never throw */ }
        }

        // Utility: fetch recent logs from DB (uses DbHelper.GetRecentLogs if available)
        public static List<LogRecord> GetRecentFromDb(int max = 500)
        {
            var outList = new List<LogRecord>();
            try
            {
                if (_db == null) return outList;
                try
                {
                    var recent = _db.GetRecentLogs(max);
                    if (recent != null)
                    {
                        foreach (var r in recent)
                        {
                            // recent items likely have TimestampUtc and Message (string)
                            try
                            {
                                var lr = new LogRecord
                                {
                                    TimestampUtc = r.TimestampUtc,
                                    Level = LogLevel.Info,
                                    Category = "DB",
                                    Message = r.Message
                                };
                                outList.Add(lr);
                            }
                            catch { }
                        }
                    }
                }
                catch
                {
                    // reflection fallback if method name different
                    try
                    {
                        var method = _db.GetType().GetMethod("GetRecentLogs");
                        if (method != null)
                        {
                            var recent = method.Invoke(_db, new object[] { max }) as System.Collections.IEnumerable;
                            if (recent != null)
                            {
                                foreach (var r in recent)
                                {
                                    try
                                    {
                                        var tsProp = r.GetType().GetProperty("TimestampUtc");
                                        var msgProp = r.GetType().GetProperty("Message");
                                        DateTime ts = tsProp != null ? (DateTime)tsProp.GetValue(r) : DateTime.UtcNow;
                                        string msg = msgProp != null ? (string)msgProp.GetValue(r) : r.ToString();
                                        outList.Add(new LogRecord { TimestampUtc = ts, Level = LogLevel.Info, Category = "DB", Message = msg });
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            return outList;
        }

        // Request immediate dump of all queued logs to DB and UI (blocking)
        public static void FlushAndDumpNow()
        {
            try
            {
                // flush queue synchronously by calling FlushOneBatchAsync repeatedly until empty
                Task.Run(async () =>
                {
                    while (!_queue.IsEmpty)
                    {
                        await FlushOneBatchAsync().ConfigureAwait(false);
                    }
                }).Wait(1000);
            }
            catch { }
        }

        // Clear logs from DB (if DbHelper exposes ClearAllLogs or DeleteAllLogs)
        public static void ClearAllPersistedLogs()
        {
            try
            {
                if (_db == null) return;
                try { _db.GetType().GetMethod("ClearAllLogs")?.Invoke(_db, null); } catch { }
                try { _db.GetType().GetMethod("ClearLogs")?.Invoke(_db, null); } catch { }
            }
            catch { }
        }

        static string ByteArrayToHex(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            var sb = new StringBuilder(bytes.Length * 3);
            foreach (var b in bytes) sb.AppendFormat("{0:X2} ", b);
            return sb.ToString().Trim();
        }
    }
}