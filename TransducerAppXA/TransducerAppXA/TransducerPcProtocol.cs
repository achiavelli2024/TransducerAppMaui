// (arquivo já enviado antes: versão final do driver cliente — copie/cole este arquivo no projeto Android)
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TransducerAppXA;

namespace TransducerProtocol
{
    public class TransducerPcProtocol : IDisposable
    {
        TcpClient _client;
        Stream _netStream;
        StreamReader _reader;
        StreamWriter _writer;
        CancellationTokenSource _cts;
        readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);

        readonly ConcurrentDictionary<string, TaskCompletionSource<AckMessage>> _pendingAcks =
            new ConcurrentDictionary<string, TaskCompletionSource<AckMessage>>(StringComparer.OrdinalIgnoreCase);

        public event Action<string> OnLog;
        public event Action<BaseMessage> OnMessageReceived;
        public event Action<AckMessage> OnAckReceived;
        public event Action<Exception> OnDisconnected;

        public static event Action<string, TestPushMessage> OnTestPushReceived;



        public bool UseTls { get; set; } = false;
        public string ServerThumbprint { get; set; } = null;
        public string ApiToken { get; set; } = null;

        Timer _heartbeatTimer;
        public int HeartbeatIntervalMs { get; set; } = 30000;

        public TransducerPcProtocol(string apiToken = null) { ApiToken = apiToken; }

        void Log(string s) { try { OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {s}"); } catch { } }

        public bool IsConnected => _client?.Connected ?? false;

        public async Task<bool> ConnectAsync(string host, int port, bool useTls = false, string token = null, int timeoutMs = 5000)
        {
            Disconnect();
            UseTls = useTls;
            if (!string.IsNullOrEmpty(token)) ApiToken = token;
            _cts = new CancellationTokenSource();

            try
            {
                _client = new TcpClient();
                var connectTask = _client.ConnectAsync(host, port);
                var delay = Task.Delay(timeoutMs);
                var completed = await Task.WhenAny(connectTask, delay).ConfigureAwait(false);
                if (completed != connectTask) { Log($"Connect timeout to {host}:{port}"); _client.Close(); _client = null; return false; }
                if (!_client.Connected) { Log($"Unable to connect to {host}:{port}"); return false; }

                var baseStream = _client.GetStream();
                if (UseTls)
                {
                    var ssl = new SslStream(baseStream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
                    try { await ssl.AuthenticateAsClientAsync(host).ConfigureAwait(false); _netStream = ssl; Log("TLS negotiation succeeded."); }
                    catch (Exception ex) { Log("TLS auth error: " + ex.Message); try { ssl.Dispose(); } catch { } _client.Close(); _client = null; return false; }
                }
                else _netStream = baseStream;

                _reader = new StreamReader(_netStream, Encoding.UTF8);
                _writer = new StreamWriter(_netStream, Encoding.UTF8) { AutoFlush = true };

                // Handshake (send and wait ack) using same reader/writer
                var handshake = new HelloMessage { Client = "TransducerClient", ClientVersion = "1.0", Device = Environment.MachineName, Token = ApiToken ?? "" };
                handshake.MessageId = Guid.NewGuid().ToString();
                var helloJson = ProtocolJson.Serialize(handshake);
                await SendRawAsync(helloJson).ConfigureAwait(false);
                Log("Sent transducer handshake.");

                string ackLine = await ReadLineWithTimeoutAsync(5000).ConfigureAwait(false);
                if (ackLine != null)
                {
                    try
                    {
                        var j = JObject.Parse(ackLine);
                        var tp = (string)j["type"];
                        if (tp == "transducer_ack" || tp == "hello_ack")
                        {
                            Log("Received handshake ack from server: " + tp);
                            try { var helloAck = ProtocolJson.Deserialize<HelloAck>(ackLine); OnMessageReceived?.Invoke(helloAck); } catch { }
                        }
                        else Log("Handshake response unexpected (receive loop will handle): " + ackLine);
                    }
                    catch (Exception ex) { Log("Handshake ack parse error: " + ex.Message); }
                }
                else Log("No handshake ack received (server may not send).");

                // Now safe to start receive loop
                _ = Task.Run(() => ReceiveLoopAsync(_cts.Token));
                StartHeartbeat();
                return true;
            }
            catch (Exception ex) { Log("Connect error: " + ex.Message); Disconnect(); return false; }
        }

        bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            try
            {
                if (string.IsNullOrEmpty(ServerThumbprint)) return sslPolicyErrors == SslPolicyErrors.None;
                if (certificate == null) return false;
                var cert2 = new X509Certificate2(certificate);
                var thumb = cert2.Thumbprint?.Replace(" ", "").ToUpperInvariant();
                var expected = ServerThumbprint.Replace(" ", "").ToUpperInvariant();
                return thumb == expected;
            }
            catch { return false; }
        }

        public void StartHeartbeat()
        {
            try { StopHeartbeat(); _heartbeatTimer = new Timer(async _ => { try { if (!IsConnected) return; var ping = new BaseMessage { Type = "ping", MessageId = Guid.NewGuid().ToString() }; await SendMessageAsync(ping).ConfigureAwait(false); } catch { } }, null, HeartbeatIntervalMs, HeartbeatIntervalMs); } catch { }
        }
        public void StopHeartbeat() { try { _heartbeatTimer?.Dispose(); _heartbeatTimer = null; } catch { } }

        public void Disconnect()
        {
            try { StopHeartbeat(); } catch { }
            try { _cts?.Cancel(); } catch { }
            try { _writer?.Close(); } catch { }
            try { _reader?.Close(); } catch { }
            try { _netStream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _writer = null; _reader = null; _netStream = null; _client = null;
            Log("Disconnected.");
        }

        public async Task SendMessageAsync(BaseMessage msg)
        {
            if (msg == null) throw new ArgumentNullException(nameof(msg));
            var json = ProtocolJson.Serialize(msg);
            await SendRawAsync(json).ConfigureAwait(false);
        }

        public async Task SendRawAsync(string json)
        {
            if (_writer == null) throw new InvalidOperationException("Not connected or writer not available.");
            await _sendLock.WaitAsync().ConfigureAwait(false);
            try { await _writer.WriteLineAsync(json).ConfigureAwait(false); Log($"Sent JSON [{json.Length} bytes]"); } finally { _sendLock.Release(); }
        }

        public async Task<AckMessage> SendResultAsync(ResultMessage rm, int timeoutMs = 10000, int maxAttempts = 3, CancellationToken cancellation = default)
        {
            if (rm == null) throw new ArgumentNullException(nameof(rm));
            if (string.IsNullOrEmpty(rm.MessageId)) rm.MessageId = Guid.NewGuid().ToString();

            int attempt = 0;
            while (attempt < maxAttempts && !cancellation.IsCancellationRequested)
            {
                attempt++;
                var tcs = new TaskCompletionSource<AckMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
                _pendingAcks[rm.MessageId] = tcs;

                try
                {
                    await SendMessageAsync(rm).ConfigureAwait(false);
                    Log($"SendResult attempt {attempt} message_id={rm.MessageId}");

                    var completed = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs, cancellation)).ConfigureAwait(false);
                    if (completed == tcs.Task)
                    {
                        var ack = await tcs.Task.ConfigureAwait(false);
                        OnAckReceived?.Invoke(ack);
                        _pendingAcks.TryRemove(rm.MessageId, out _);
                        return ack;
                    }
                    else
                    {
                        Log($"No ACK for message_id={rm.MessageId} attempt {attempt}");
                        _pendingAcks.TryRemove(rm.MessageId, out _);
                    }
                }
                catch (Exception ex)
                {
                    Log("SendResult error: " + ex.Message);
                    _pendingAcks.TryRemove(rm.MessageId, out _);
                }

                await Task.Delay(500 * attempt, cancellation).ConfigureAwait(false);
            }

            return null;
        }

        public async Task SendResultBatchAsync(ResultBatchMessage batch)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));
            if (string.IsNullOrEmpty(batch.MessageId)) batch.MessageId = Guid.NewGuid().ToString();
            await SendMessageAsync(batch).ConfigureAwait(false);
        }

        async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    string line = null;
                    try { line = await _reader.ReadLineAsync().ConfigureAwait(false); } catch (Exception ex) { Log("Receive read error: " + ex.Message); break; }
                    if (line == null) break;
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var j = JObject.Parse(line);
                        var type = (string)j["type"];
                        if (string.IsNullOrEmpty(type)) { Log("Incoming message missing type."); continue; }

                        switch (type)
                        {
                            case "ack":
                                {
                                    var ack = ProtocolJson.Deserialize<AckMessage>(line);
                                    if (ack != null)
                                    {
                                        if (!string.IsNullOrEmpty(ack.AckMessageId) && _pendingAcks.TryRemove(ack.AckMessageId, out var tcs))
                                        {
                                            tcs.TrySetResult(ack);
                                        }
                                        OnAckReceived?.Invoke(ack);
                                        OnMessageReceived?.Invoke(ack);
                                    }
                                }
                                break;

                            case "test_push":
                                {
                                    try
                                    {
                                        // mantenha o raw JSON para inspeção e desseralize para DTO
                                        var raw = line;
                                        TestPushMessage push = null;
                                        try
                                        {
                                            push = ProtocolJson.Deserialize<TestPushMessage>(line);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Se desserialização falhar, logue (o logger do projeto)
                                            TransducerLogAndroid.LogError("Protocol", "Failed to deserialize test_push: " + ex.Message);
                                        }

                                        // notifica ouvintes (raw + DTO possivelmente null se desserialização falhar)
                                        try { OnTestPushReceived?.Invoke(raw, push); } catch { }

                                        // --- CORREÇÃO: garanta que OnMessageReceived receba um BaseMessage ---
                                        try
                                        {
                                            // push pode ser null; cast safe com 'as' retorna null se incompatível.
                                            BaseMessage baseMsg = push as BaseMessage ?? ProtocolJson.Deserialize<BaseMessage>(line);
                                            OnMessageReceived?.Invoke(baseMsg);
                                        }
                                        catch { }
                                    }
                                    catch (Exception ex)
                                    {
                                        TransducerLogAndroid.LogError("Protocol", "Error handling test_push: " + ex.Message);
                                    }
                                }
                                break;



                            case "ack_batch": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<AckBatchMessage>(line)); break;
                            case "result": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<ResultMessage>(line)); break;
                            case "result_batch": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<ResultBatchMessage>(line)); break;
                            case "test_command": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<TestCommandMessage>(line)); break;
                            case "test_command_ack": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<TestCommandAck>(line)); break;
                            case "test_progress": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<TestProgressMessage>(line)); break;
                            case "error": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<ErrorMessage>(line)); break;
                            case "ping": await SendMessageAsync(new BaseMessage { Type = "pong", MessageId = Guid.NewGuid().ToString() }).ConfigureAwait(false); break;
                            case "pong": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<BaseMessage>(line)); break;
                            case "transducer_ack":
                            case "hello_ack": OnMessageReceived?.Invoke(ProtocolJson.Deserialize<HelloAck>(line)); break;
                            default:
                                try { OnMessageReceived?.Invoke(ProtocolJson.Deserialize<BaseMessage>(line)); }
                                catch { Log("Unknown message type received: " + type); }
                                break;
                        }
                    }
                    catch (JsonReaderException jex) { Log("JSON parse error: " + jex.Message); }
                    catch (Exception ex) { Log("Receive dispatch error: " + ex.Message); }
                }
            }
            catch (Exception ex) { Log("ReceiveLoop error: " + ex.Message); }
            finally { Log("Receive loop ended, connection closed."); try { OnDisconnected?.Invoke(null); } catch { } Disconnect(); }
        }

        async Task<string> ReadLineWithTimeoutAsync(int timeoutMs)
        {
            if (_reader == null) return null;
            var readTask = _reader.ReadLineAsync();
            var delay = Task.Delay(timeoutMs);
            var completed = await Task.WhenAny(readTask, delay).ConfigureAwait(false);
            if (completed == readTask) { try { return await readTask.ConfigureAwait(false); } catch { return null; } }
            return null;
        }

        public void Dispose() { try { Disconnect(); } catch { } try { _sendLock?.Dispose(); } catch { } _pendingAcks.Clear(); }
    }
}