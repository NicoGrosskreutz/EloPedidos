using System;
using System.IO;
using Android;
using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using EloPedidos.Controllers;
using PCLExt.FileStorage.Folders;

namespace EloPedidos.Utils
{
	public class SisLog
	{
		public static string folder = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "LOG" + Java.IO.File.Separator + "SisLog.txt";
		public static void GetLog(string title, string message)
		{
			try
			{
				StreamWriter w = new StreamWriter(folder, true);
				w.WriteLine($"LOG {DateTime.Now.ToString("dd / MM / yyyy HH: mm:ss")}: {title}\n");
				w.WriteLine($" {message} ");
				w.WriteLine("\n");
				w.Flush();
				w.Close();
			}
			catch (Exception ex)
			{
				Log.Error("Elo_LOG", ex.ToString());
			}
		}

		public string ReadLog()
		{
			try
			{
				string log = File.ReadAllText(folder);
				return log;
			}
			catch (Exception ex)
			{
				Log.Error("Elo_LOG", ex.ToString());
				return string.Empty;
			}
		}

		public static void Logger(string message, string title = "")
		{
			if (title == "")
				title = Utils.Ext.LOG_APP;

			GetLog(title, message);

			Log.Error(title, message);
		}

		public static void CreateFolder(Activity activity)
		{
			try
			{
				if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
					ActivityCompat.RequestPermissions(activity, new string[] { Manifest.Permission.ReadExternalStorage }, 11);
				if (ContextCompat.CheckSelfPermission(Application.Context, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
					ActivityCompat.RequestPermissions(activity, new string[] { Manifest.Permission.WriteExternalStorage }, 12);

				Java.IO.File file = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "LOG");

				if (!file.Exists())
					file.Mkdir();
				else
					Log.Debug("Elo_LOG", "A Pasta Já Existe !");
			}
			catch (Exception e)
			{
				Log.Debug("Elo_LOG", e.Message);
			}
		}
	}
}