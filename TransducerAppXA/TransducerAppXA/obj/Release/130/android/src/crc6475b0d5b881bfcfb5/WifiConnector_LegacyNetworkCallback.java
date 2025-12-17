package crc6475b0d5b881bfcfb5;


public class WifiConnector_LegacyNetworkCallback
	extends android.net.ConnectivityManager.NetworkCallback
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAvailable:(Landroid/net/Network;)V:GetOnAvailable_Landroid_net_Network_Handler\n" +
			"n_onLost:(Landroid/net/Network;)V:GetOnLost_Landroid_net_Network_Handler\n" +
			"n_onUnavailable:()V:GetOnUnavailableHandler\n" +
			"";
		mono.android.Runtime.register ("TransducerAppXA.Helpers.WifiConnector+LegacyNetworkCallback, TransducerAppXA", WifiConnector_LegacyNetworkCallback.class, __md_methods);
	}


	public WifiConnector_LegacyNetworkCallback ()
	{
		super ();
		if (getClass () == WifiConnector_LegacyNetworkCallback.class) {
			mono.android.TypeManager.Activate ("TransducerAppXA.Helpers.WifiConnector+LegacyNetworkCallback, TransducerAppXA", "", this, new java.lang.Object[] {  });
		}
	}


	public void onAvailable (android.net.Network p0)
	{
		n_onAvailable (p0);
	}

	private native void n_onAvailable (android.net.Network p0);


	public void onLost (android.net.Network p0)
	{
		n_onLost (p0);
	}

	private native void n_onLost (android.net.Network p0);


	public void onUnavailable ()
	{
		n_onUnavailable ();
	}

	private native void n_onUnavailable ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
