using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentFTP;
using Android.Util;
using EloPedidos.Models;
using PCLExt;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using System.IO;
using Xamarin.Essentials;
using Android.Content.PM;

namespace EloPedidos.Controllers
{
	public class FTPController
	{
		public bool updateApp()
		{
			bool result = false;
			ConfigController cController = new ConfigController();
			FtpClient client = new FtpClient();
			DNS dns = cController.GetDNS();
			FileInfo fileInfo;

			try
			{
				if (cController.TestServerConnection("elosoftware.dyndns.org", 8560))
				{
					client.Host = "elosoftware.dyndns.org";
					client.Credentials = new System.Net.NetworkCredential("usuario", "penasoft");
					client.Connect();

					string localPath = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware/EloPedidos.EloPedidos.apk";
					string remotePath = "/AtServidor/EloPedidos.EloPedidos.apk";
					client.DownloadFile(localPath, remotePath);

					result = true;
				}
			}
			catch (Exception e)
			{
				Log.Error("FTP_ERROR", e.ToString());
			}
			finally
			{
				client.Disconnect();
			}
			return result;
		}

		public bool BackupDataBase()
		{
			bool result = false;
			ConfigController cController = new ConfigController();
			FtpClient client = new FtpClient();
			Empresa empresa = new EmpresaController().Empresa;
			Vendedor vendedor = new VendedorController().Vendedor;
			var nomeVendedor = vendedor.NOMVEND;
			if (nomeVendedor.Contains("/"))
				nomeVendedor = nomeVendedor.Split("/")[0];

			try
			{
				if (cController.TestServerConnection("elosoftware.dyndns.org", 8560))
				{
					client.Host = "elosoftware.dyndns.org";
					client.Credentials = new System.Net.NetworkCredential("usuario", "penasoft");
					client.Connect();

					string localPathLogs = string.Empty;

					string localPath = System.IO.Path.Combine(
						System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Dados.db3");

					localPathLogs = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "LOG" + Java.IO.File.Separator + "SisLog.txt";

					Java.IO.File folder = new Java.IO.File(localPathLogs);

					string remotePath = $"/AtServidor/BackupApp/Backup_{empresa.CODEMPRE}_{nomeVendedor}_{DateTime.Now.ToString("ddMMyyyy")}";

					if (!client.FileExists(remotePath))
						client.CreateDirectory(remotePath);

					client.UploadFile(localPath, remotePath + "/ Dados.db3", FtpRemoteExists.Append, true, FtpVerify.None);

					if (folder.Exists())
						client.UploadFile(localPathLogs, remotePath + "/ SisLog.txt", FtpRemoteExists.Append, true, FtpVerify.None);

					result = true;
				}
			}
			catch (Exception e)
			{
				Log.Error("FTP_ERROR", e.ToString());
				result = false;
			}
			finally
			{
				client.Disconnect();
			}
			return result;
		}

		public string getCurrentVersion()
		{
			ConfigController cController = new ConfigController();
			FtpClient client = new FtpClient();
			DNS dns = cController.GetDNS();
			string data = "";
			try
			{
				if (cController.TestServerConnection("elosoftware.dyndns.org", 8560))
				{
					client.Host = "elosoftware.dyndns.org";
					client.Credentials = new System.Net.NetworkCredential("usuario", "penasoft");
					client.Connect();

					data = client.GetModifiedTime("/AtServidor/EloPedidos.EloPedidos.apk").ToString("dd/MM/yyyy HH:mm:ss");
				}

			}
			catch (Exception e)
			{
				Log.Error("FTP_ERROR", e.ToString());
			}
			finally
			{
				client.Disconnect();
			}
			return data;
		}

		public string getApkVersion()
		{
			FileInfo fileInfo;
			string data = "";
			string path = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware/EloPedidos.EloPedidos.apk";
			try
			{
				fileInfo = new FileInfo(path);
				data = fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");
			}
			catch (Exception e)
			{
				Log.Error("FILE_ERROR", e.ToString());
			}
			return data;
		}

		public static DateTime getAppVersion(out string version)
		{
			try
			{
				VersionTracking.Track();

				PackageInfo info = Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0);
				long unixDate = info.LastUpdateTime;
				DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

				version = VersionTracking.CurrentVersion;
				return date;
			}
			catch
			{
				version = string.Empty;
				return new DateTime();
			}
		}
	}
}