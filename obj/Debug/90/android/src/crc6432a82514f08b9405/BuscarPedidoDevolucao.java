package crc6432a82514f08b9405;


public class BuscarPedidoDevolucao
	extends crc6432a82514f08b9405.BuscarPedidoView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("EloPedidos.Views.BuscarPedidoDevolucao, EloPedidos", BuscarPedidoDevolucao.class, __md_methods);
	}


	public BuscarPedidoDevolucao ()
	{
		super ();
		if (getClass () == BuscarPedidoDevolucao.class)
			mono.android.TypeManager.Activate ("EloPedidos.Views.BuscarPedidoDevolucao, EloPedidos", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
