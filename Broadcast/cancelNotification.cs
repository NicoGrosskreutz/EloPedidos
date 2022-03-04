using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using EloPedidos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EloPedidos.Broadcast
{
	[BroadcastReceiver(Name = "Broadcast.cancelNotification")]
	public class cancelNotification : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			context.StopService(new Intent(context, typeof(LocationService)));
		}
	}
}