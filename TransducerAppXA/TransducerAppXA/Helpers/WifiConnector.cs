using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Net.Wifi.P2p; // correção: namespace P2p
using Android.OS;
using Android.Util;

namespace TransducerAppXA.Helpers
{
    /// <summary>
    /// Helper para conectar a um SSID e bindar o processo ao Network obtido.
    /// - Android Q+ (API 29+) -> WifiNetworkSpecifier (ephemeral connection)
    /// - Android < 29 -> WifiManager.AddNetwork + RequestNetwork + BindProcessToNetwork
    ///
    /// Uso:
    ///   var ok = await WifiConnector.ConnectToNetworkAsync(this, "MY_SSID", "mypassword", isWpa2: true);
    ///   if (ok) { /* seu socket TCP pode abrir usando o IP do transdutor */ }
    ///   WifiConnector.Disconnect(this); // para limpar
    /// </summary>
    public static class WifiConnector
    {
        const string TAG = "WifiConnector";
        static ConnectivityManager _cm;
        static WifiManager _wm;
        static ConnectivityManager.NetworkCallback _netCallback;
        static Network _boundNetwork;
        static int _addedNetworkId = -1;

        static void Init(Context ctx)
        {
            if (_cm == null) _cm = (ConnectivityManager)ctx.GetSystemService(Context.ConnectivityService);
            if (_wm == null) _wm = (WifiManager)ctx.ApplicationContext.GetSystemService(Context.WifiService);
        }

        /// <summary>
        /// Tenta conectar e bindar o processo ao Network retornado. Retorna true se ok.
        /// </summary>
        public static async Task<bool> ConnectToNetworkAsync(Context ctx, string ssid, string password, bool isWpa2 = true, int timeoutMs = 20000)
        {
            try
            {
                Init(ctx);

                if (Build.VERSION.SdkInt >= BuildVersionCodes.Q) // Android 10+
                {
                    Log.Info(TAG, $"Using WifiNetworkSpecifier for SSID={ssid}");
                    return await ConnectUsingNetworkSpecifierAsync(ctx, ssid, password, timeoutMs).ConfigureAwait(false);
                }
                else
                {
                    Log.Info(TAG, $"Using WifiManager for SSID={ssid}");
                    return await ConnectUsingWifiManagerAsync(ctx, ssid, password, isWpa2, timeoutMs).ConfigureAwait(false);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, "ConnectToNetworkAsync error: " + ex);
                return false;
            }
        }

        static TaskCompletionSource<bool> _tcs;

        // Android Q+ (API 29) -> WifiNetworkSpecifier
        static Task<bool> ConnectUsingNetworkSpecifierAsync(Context ctx, string ssid, string password, int timeoutMs)
        {
            _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            try
            {
                var specBuilder = new Android.Net.Wifi.WifiNetworkSpecifier.Builder()
                    .SetSsid(ssid);

                if (!string.IsNullOrEmpty(password))
                {
                    specBuilder.SetWpa2Passphrase(password);
                }

                var spec = specBuilder.Build();

                var reqBuilder = new NetworkRequest.Builder()
                    .AddTransportType(TransportType.Wifi)
                    .SetNetworkSpecifier(spec);

                var request = reqBuilder.Build();

                // remove callback antigo
                if (_netCallback != null)
                {
                    try { _cm.UnregisterNetworkCallback(_netCallback); } catch { }
                    _netCallback = null;
                    _boundNetwork = null;
                }

                _netCallback = new SpecifierNetworkCallback(_tcs);

                // request network on main looper
                _cm.RequestNetwork(request, _netCallback, new Handler(Looper.MainLooper));

                // timeout
                var cts = new CancellationTokenSource(timeoutMs);
                cts.Token.Register(() =>
                {
                    if (!_tcs.Task.IsCompleted) _tcs.TrySetResult(false);
                });

                return _tcs.Task;
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, "NetworkSpecifier request error: " + ex);
                return Task.FromResult(false);
            }
        }

        // Android < 10 -> WifiConfiguration + RequestNetwork
        static Task<bool> ConnectUsingWifiManagerAsync(Context ctx, string ssid, string password, bool isWpa2, int timeoutMs)
        {
            _tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            try
            {
                // OBS: WifiConfiguration API está deprecated em versões recentes, mas ainda necessário para < Q
                var config = new WifiConfiguration
                {
                    Ssid = $"\"{ssid}\""
                };

                if (string.IsNullOrEmpty(password))
                {
                    config.AllowedKeyManagement.Set((int)KeyManagementType.None);
                }
                else
                {
                    config.PreSharedKey = $"\"{password}\"";
                    config.AllowedKeyManagement.Set((int)KeyManagementType.WpaPsk);
                }

                // Add or find network
                _addedNetworkId = _wm.AddNetwork(config);
                if (_addedNetworkId == -1)
                {
                    Log.Warn(TAG, "AddNetwork returned -1; attempting to find existing network id");
                    var list = _wm.ConfiguredNetworks;
                    if (list != null)
                    {
                        foreach (var n in list)
                        {
                            try
                            {
                                if (n.Ssid != null && n.Ssid.Trim('"') == ssid)
                                {
                                    _addedNetworkId = n.NetworkId;
                                    break;
                                }
                            }
                            catch { }
                        }
                    }
                }

                if (_addedNetworkId == -1)
                {
                    Log.Error(TAG, "Unable to add/find WiFi network config for SSID=" + ssid);
                    _tcs.TrySetResult(false);
                    return _tcs.Task;
                }

                bool enabled = _wm.EnableNetwork(_addedNetworkId, true);
                Log.Info(TAG, "EnableNetwork result: " + enabled);

                // Request network and bind
                var builder = new NetworkRequest.Builder()
                    .AddTransportType(TransportType.Wifi);
                var request = builder.Build();

                if (_netCallback != null)
                {
                    try { _cm.UnregisterNetworkCallback(_netCallback); } catch { }
                    _netCallback = null;
                    _boundNetwork = null;
                }

                _netCallback = new LegacyNetworkCallback(_tcs, _addedNetworkId, _wm);
                _cm.RequestNetwork(request, _netCallback);

                var cts = new CancellationTokenSource(timeoutMs);
                cts.Token.Register(() =>
                {
                    if (!_tcs.Task.IsCompleted) _tcs.TrySetResult(false);
                });

                return _tcs.Task;
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, "ConnectUsingWifiManagerAsync error: " + ex);
                _tcs.TrySetResult(false);
                return _tcs.Task;
            }
        }

        // Disconnect / Unbind and cleanup
        public static void Disconnect(Context ctx)
        {
            try
            {
                Init(ctx);

                // Unbind process from network
                try
                {
                    if (_boundNetwork != null)
                    {
                        ConnectivityManager.SetProcessDefaultNetwork(null);
                        _boundNetwork = null;
                    }
                }
                catch { }

                // unregister callback
                if (_netCallback != null)
                {
                    try { _cm.UnregisterNetworkCallback(_netCallback); } catch { }
                    _netCallback = null;
                }

                // remove added network config if present (for legacy)
                try
                {
                    if (_addedNetworkId != -1 && _wm != null)
                    {
                        _wm.RemoveNetwork(_addedNetworkId);
                        _wm.SaveConfiguration();
                        _addedNetworkId = -1;
                    }
                }
                catch { }

                Log.Info(TAG, "Disconnect cleanup done");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, "Disconnect error: " + ex);
            }
        }

        // API29+ NetworkCallback
        class SpecifierNetworkCallback : ConnectivityManager.NetworkCallback
        {
            TaskCompletionSource<bool> _tcs;
            public SpecifierNetworkCallback(TaskCompletionSource<bool> tcs) { _tcs = tcs; }

            public override void OnAvailable(Network network)
            {
                base.OnAvailable(network);
                try
                {
                    Log.Info(TAG, "Specifier OnAvailable");
                    bool ok = ConnectivityManager.SetProcessDefaultNetwork(network);
                    Log.Info(TAG, "Bind process to network ok=" + ok);
                    _tcs.TrySetResult(ok);
                }
                catch (System.Exception ex)
                {
                    Log.Error(TAG, "Specifier OnAvailable error: " + ex);
                    _tcs.TrySetResult(false);
                }
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();
                Log.Warn(TAG, "Specifier OnUnavailable");
                _tcs.TrySetResult(false);
            }
        }

        // Legacy NetworkCallback (Android < Q)
        class LegacyNetworkCallback : ConnectivityManager.NetworkCallback
        {
            TaskCompletionSource<bool> _tcs;
            int _expectedNetworkId;
            WifiManager _wmLocal;
            public LegacyNetworkCallback(TaskCompletionSource<bool> tcs, int expectedNetworkId, WifiManager wm)
            {
                _tcs = tcs; _expectedNetworkId = expectedNetworkId; _wmLocal = wm;
            }

            public override void OnAvailable(Network network)
            {
                base.OnAvailable(network);
                try
                {
                    Log.Info(TAG, "Legacy OnAvailable - binding to network");
                    bool ok = ConnectivityManager.SetProcessDefaultNetwork(network);
                    Log.Info(TAG, "Bind process to network ok=" + ok);
                    _tcs.TrySetResult(ok);
                }
                catch (System.Exception ex)
                {
                    Log.Error(TAG, "Legacy OnAvailable error: " + ex);
                    _tcs.TrySetResult(false);
                }
            }

            public override void OnLost(Network network)
            {
                base.OnLost(network);
                Log.Warn(TAG, "Legacy OnLost");
            }

            public override void OnUnavailable()
            {
                base.OnUnavailable();
                Log.Warn(TAG, "Legacy OnUnavailable");
                _tcs.TrySetResult(false);
            }
        }
    }
}