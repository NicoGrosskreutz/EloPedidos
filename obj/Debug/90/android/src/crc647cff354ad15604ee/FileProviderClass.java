package crc647cff354ad15604ee;


public class FileProviderClass
	extends android.support.v4.content.FileProvider
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("EloPedidos.Provider.FileProviderClass, EloPedidos", FileProviderClass.class, __md_methods);
	}


	public FileProviderClass ()
	{
		super ();
		if (getClass () == FileProviderClass.class)
			mono.android.TypeManager.Activate ("EloPedidos.Provider.FileProviderClass, EloPedidos", "", this, new java.lang.Object[] {  });
	}

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
