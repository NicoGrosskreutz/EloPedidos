using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Content;
using Android.Views;
using Plugin.Permissions;
using Android.Runtime;
using EloPedidos.Services;
using EloPedidos.Controllers;
using Android.Widget;
using static Android.App.ActivityManager;
using System.Collections.Generic;
using AlertDialog = Android.App.AlertDialog;
using EloPedidos.Persistence;
using System;
using EloPedidos.Models;
using Plugin.DeviceInfo;
using Android.Locations;
using Xamarin.Essentials;
using Java.IO;
using Android.Util;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;
using Plugin.CurrentActivity;
using System.Threading;
using System.Threading.Tasks;

namespace EloPedidos
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@mipmap/logo", WindowSoftInputMode = SoftInput.AdjustResize)]
	public class MainActivity : AppCompatActivity
	{
		private System.Timers.Timer timer = null;

		private Button btnlogout;
		private TextView lblVERSION;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);


			SetContentView(Resource.Layout.activity_main);

			btnlogout = FindViewById<Button>(Resource.Id.btnlogout);
			lblVERSION = FindViewById<TextView>(Resource.Id.lblVERSION);

			btnlogout.Visibility = ViewStates.Invisible;

			getVersion();

			//if (!isServiceRunning("Services.GeolocatorService"))
			//	StartService(new Intent(this, typeof(GeolocatorService)));

			if (!isServiceRunning("Services.LocationService"))
				StartService(new Intent(this, typeof(LocationService)));

			// Filtros para o BroadCastReceiver
			//IntentFilter filters = new IntentFilter();
			//filters.AddAction("android.intent.action.BOOT_COMPLETED");

			// Registrando o BroadCastReceiver com o servico	
			//RegisterReceiver(new GeolocatorBroadCast(), filters);

			//SendBroadcast(new Intent(this, typeof(GeolocatorBroadCast)));



			btnlogout.Click += (sender, args) =>
			{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("AVISO");
				builder.SetMessage("GOSTARIA DE REINICIAR O SISTEMA ? \n(TODOS OS DADOS SERÃO PERDIDOS !)");
				builder.SetPositiveButton("SIM", (s, a) =>
				{
					Database.ResetDatabase();

					Intent i = new Intent(this, typeof(Views.LoginView));
					StartActivity(i);
					Finish();
				});
				builder.SetNegativeButton("CANCELAR", (s, a) => { return; });
				AlertDialog dialog = builder.Create();
				dialog.Show();

			};

			timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
			timer.Elapsed += (s, a) =>
			{
				endSplashView();
				//timer.Stop();
			};
			timer.Enabled = true;
			timer.AutoReset = false;
			timer.Start();
		}

		/// <summary>
		///  Cria o menu para o app
		/// </summary>
		/// <param name="menu"></param>
		/// <returns></returns>
		//public override bool OnCreateOptionsMenu(IMenu menu)
		//{
		//	return new Views.PedidoView().OnCreateOptionsMenu(menu);
		//}


		///// <summary>
		/////  Controla a ação do botão ao ser clicado (Menu)
		///// </summary>
		///// <param name="item"></param>
		///// <returns></returns>
		//public override bool OnOptionsItemSelected(IMenuItem item)
		//{
		//	return new Views.PedidoView().OnOptionsItemSelected(item);
		//}

		private void endSplashView()
		{
			if (new ConfigController().Config != null)
			{
				Intent i = new Intent(Application.Context, typeof(Views.PedidoView));
				StartActivity(i);
				Finish();
			}
			else
			{
				Intent i = new Intent(Application.Context, typeof(Views.LoginView));
				StartActivity(i);
				Finish();
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			//SendBroadcast(new Intent(this, typeof(GeolocatorBroadCast)));
			//StartService(new Intent(this, typeof(GeolocatorService)));
		}

		private void Msg(string message)
				=> RunOnUiThread(() => Toast.MakeText(ApplicationContext, message, ToastLength.Long).Show());

		public bool isServiceRunning(string service)
		{
			ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);
			foreach (RunningServiceInfo serviceinfo in manager.GetRunningServices(int.MaxValue))
			{
				if (service.Equals(serviceinfo.Service.ClassName))
				{
					return true;
				}
			}
			return false;
		}

		private void getVersion()
		{
			try
			{
				//VersionTracking.Track();
				//var version = VersionTracking.CurrentVersion;

				//var appInfo = Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0);
				//var dataVersion = new FTPController().getAppVersion();

				//PackageInfo info = this.PackageManager.GetPackageInfo(this.PackageName, 0);
				//long unixDate = info.LastUpdateTime;
				//DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
				//DateTime date = start.AddMilliseconds(unixDate).ToLocalTime();

				DateTime versionDate = FTPController.getAppVersion(out string versao);

				lblVERSION.Text = $"Versão: {versao} ({versionDate})";
			}
			catch (Exception e)
			{
				Log.Error("Elo_Log", e.ToString());
			}
		}
	}
}