using System;
using Android.Util;

namespace TransducerAppXA.Services
{
    // Simples contador de reconexões em memória (para uso enquanto o app estiver aberto)
    public class ConnectionCounter
    {
        const string TAG = "ConnectionCounter";
        static readonly object _lock = new object();
        static ConnectionCounter _instance;

        public static ConnectionCounter Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = new ConnectionCounter();
                    return _instance;
                }
            }
        }

        // Estado
        public bool IsConnected { get; private set; } = false;
        public int ReconnectCount { get; private set; } = 0;
        public DateTime? ConnectedSince { get; private set; } = null;

        // Interno: marca se já teve pelo menos uma conexão desde que o app abriu
        bool _hasEverConnected = false;

        // Evento para UI (opcional): dispara quando estado muda
        public event Action OnChanged;

        ConnectionCounter() { }

        // Chamar quando a conexão for (re)estabelecida
        public void Connected()
        {
            lock (_lock)
            {
                try
                {
                    // se já estava conectado, ignora
                    if (IsConnected)
                    {
                        Log.Info(TAG, "Connected() called but already connected - ignored");
                        return;
                    }

                    // se já houve ao menos uma conexão antes, então isto é uma REconexão
                    if (_hasEverConnected)
                    {
                        ReconnectCount++;
                        Log.Info(TAG, $"Connected(): reconnection detected. ReconnectCount={ReconnectCount}");
                    }
                    else
                    {
                        // primeira conexão desde o app foi aberto
                        _hasEverConnected = true;
                        Log.Info(TAG, "Connected(): first connection since app start");
                    }

                    IsConnected = true;
                    ConnectedSince = DateTime.UtcNow;

                    // notifica UI
                    try { OnChanged?.Invoke(); } catch (Exception ex) { Log.Warn(TAG, "Connected: OnChanged handler threw: " + ex.Message); }
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, "Connected error: " + ex.Message);
                }
            }
        }

        // Chamar quando detectar desconexão
        public void Disconnected()
        {
            lock (_lock)
            {
                try
                {
                    if (!IsConnected)
                    {
                        Log.Info(TAG, "Disconnected() called but already disconnected - ignored");
                        return;
                    }

                    IsConnected = false;
                    Log.Info(TAG, $"Disconnected(): was connected since {ConnectedSince?.ToString("o") ?? "n/a"}; uptime={GetUptime()}");

                    // notifica UI
                    try { OnChanged?.Invoke(); } catch (Exception ex) { Log.Warn(TAG, "Disconnected: OnChanged handler threw: " + ex.Message); }
                }
                catch (Exception ex)
                {
                    Log.Error(TAG, "Disconnected error: " + ex.Message);
                }
            }
        }

        // Retorna uptime atual (em TimeSpan). Se desconectado => TimeSpan.Zero
        public TimeSpan GetUptime()
        {
            lock (_lock)
            {
                if (!IsConnected || !ConnectedSince.HasValue)
                {
                    // Log ocasional (não poluir demais)
                    // Log.Debug(TAG, "GetUptime: not connected -> returning Zero");
                    return TimeSpan.Zero;
                }
                var ts = DateTime.UtcNow - ConnectedSince.Value;
                // Log.Debug(TAG, $"GetUptime: {ts}");
                return ts;
            }
        }

        // Reset apenas em memória (se algum botão "limpar" desejar)
        public void Reset()
        {
            lock (_lock)
            {
                IsConnected = false;
                ReconnectCount = 0;
                ConnectedSince = null;
                _hasEverConnected = false;
                Log.Info(TAG, "Reset: cleared in-memory counters");
                try { OnChanged?.Invoke(); } catch (Exception ex) { Log.Warn(TAG, "Reset: OnChanged handler threw: " + ex.Message); }
            }
        }
    }
}