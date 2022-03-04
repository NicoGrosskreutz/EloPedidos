package crc645624b9e5377aa8fe;


public class GeolocatorWork
	extends androidx.work.Worker
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doWork:()Landroidx/work/ListenableWorker$Result;:GetDoWorkHandler\n" +
			"n_onStopped:()V:GetOnStoppedHandler\n" +
			"";
		mono.android.Runtime.register ("EloPedidos.Services.GeolocatorWork, EloPedidos", GeolocatorWork.class, __md_methods);
	}


	public GeolocatorWork (android.content.Context p0, androidx.work.WorkerParameters p1)
	{
		super (p0, p1);
		if (getClass () == GeolocatorWork.class)
			mono.android.TypeManager.Activate ("EloPedidos.Services.GeolocatorWork, EloPedidos", "Android.Content.Context, Mono.Android:AndroidX.Work.WorkerParameters, Xamarin.Android.Arch.Work.Runtime", this, new java.lang.Object[] { p0, p1 });
	}


	public androidx.work.ListenableWorker.Result doWork ()
	{
		return n_doWork ();
	}

	private native androidx.work.ListenableWorker.Result n_doWork ();


	public void onStopped ()
	{
		n_onStopped ();
	}

	private native void n_onStopped ();

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
