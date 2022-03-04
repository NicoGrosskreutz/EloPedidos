﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using AndroidX.Work;
using EloPedidos.Utils;

namespace EloPedidos.Services
{
	/// <summary>
	///   Muito bom, porém só possível a utilização no android -8 (OREO)
	/// </summary>
	[Service(Name = "Services.GeolocatorService")]
	public class GeolocatorService : Service
	{
		private HandlerThread hThread;
		private static Handler handler;


		public override IBinder OnBind(Intent intent)
		{
			return null;
		}
		[return: GeneratedEnum]
		public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
		{
			hThread = new HandlerThread("gservice");
			hThread.Start();
			handler = new Handler(hThread.Looper);
			handler.Post(LocalizationSerder());

			Log.Debug("gservice", "INICIANDO O SERVICO ===> OK");
			return StartCommandResult.NotSticky;
		}

		private static Java.Lang.Runnable LocalizationSerder() => new Java.Lang.Runnable(() =>
		{
			//System.Timers.Timer timer = null;
			try
			{

				//Task.Run(async () => await new Localization().GetLocalizationAsync());
				//handler.PostDelayed(LocalizationSerder(), 300000);
				auxLocalizationSerder();
				//Log.Debug("gservice", "DADOS ENVIADOS ===> OK");

				//timer = new System.Timers.Timer(TimeSpan.FromMilliseconds(300000).TotalMilliseconds);
				//timer.Elapsed += (s, a) => auxLocalizationSerder();
				//timer.Enabled = true;
				//timer.AutoReset = true;

				//timer.Start();
				//Log.Debug("gservice", "TIMER INICIADO ===> OK");
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
			}
		});

		public static void auxLocalizationSerder()
		{
			//Task.Run(() =>{
			//System.Timers.Timer timer = null;
			//timer = new System.Timers.Timer(TimeSpan.FromMilliseconds(300000).TotalMilliseconds);
			//timer.Elapsed += (s, a) =>
			//{
			//	auxLocalizationSerder();
			//	timer.Stop();
			//};
			//timer.Enabled = true;
			//timer.AutoReset = false;

			//timer.Start();
			//});

			//new Handler().PostDelayed(async () => await new Localization().GetLocalizationAsync(), 100000);


			Task.Run(async () =>
			{
				await new Localization().GetLocalizationAsync();
				Thread.Sleep(300000);
				auxLocalizationSerder();
			});
		}
	}


	/// <summary>
	///  Inicia o servico ao ligar o celular ou receber o broadcast2 34de5t78u9i0o-p´]	/// </summary>
	[BroadcastReceiver(Enabled = true)]
	[IntentFilter(new[] { Intent.ActionBootCompleted })]
	public class GeolocatorBroadCast : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
			//Log.Debug("gwork", "BROADCAST CHAMADO ===> OK!");

			//if (Build.VERSION.SdkInt < BuildVersionCodes.O)
			//{
			//Log.Debug("gwork", "SERVICOS INICIADOS VIA startService() E NÃO VIA WORK MANAGER!");

			//context.StartService(new Intent(context, typeof(GeolocatorService)));

			//GeolocatorWork.WorkRequest();


			//}
			//else
			//{
			//	try
			//	{
			//		////Task.Run(() => AlarmForGeolocator.Alarm());

			//GeolocatorWork.Init();

			//		Log.Debug("gwork", "MÉTODO INICIADO ===> OK!");

			//		SisLog.Logger("Serviço em background iniciado.\n");
			//	}
			//	catch (Exception ex)
			//	{
			//		Log.Error("gwork", "ERRO AO EXECUTAR CHAMADA DO MÉTODO! \n" + ex.ToString());

			//		SisLog.Logger("Erro ao iniciar serviços.\n" + ex.ToString());
			//	}
			//}
		}

	}


	/// <summary>
	///  Inicia serviços em background para dispositivos android compatível com versões 7 e 7+ ;
	/// 
	///  https://devblogs.microsoft.com/xamarin/getting-started-workmanager/
	///  
	/// </summary>
	public class GeolocatorWork : Worker
	{
		public GeolocatorWork(Context context, WorkerParameters workerParams) : base(context, workerParams)
		{

		}
		public override Result DoWork()
		{
			System.Timers.Timer timer = null;

			try
			{
				if (timer == null)
				{
					//timer = new System.Timers.Timer(TimeSpan.FromMilliseconds(300000).TotalMilliseconds);
					//timer.Elapsed += async (s, a) => await new Localization().GetLocalizationAsync();
					//timer.Enabled = true;
					//timer.AutoReset = true;

					//timer.Start();


					return Result.InvokeSuccess();
				}
				else
				{
					return Result.InvokeSuccess();
				}

			}
			catch (Exception ex)
			{
				Log.Error("gwork", "ERRO AO ENVIAR DADOS ===> \n" + ex.ToString());
				return Result.InvokeRetry();
			}
		}

		public static void Init()
		{
			//Método mais apropriado
			PeriodicWorkRequest request = PeriodicWorkRequest.Builder.From<GeolocatorWork>(TimeSpan.FromMinutes(20)).Build();

			/* Método que envia o trabalho para ser executado */
			WorkManager.Instance.EnqueueUniquePeriodicWork("GWork", ExistingPeriodicWorkPolicy.Keep, request);
		}

		public static void WorkRequest()
		{
			OneTimeWorkRequest request = new OneTimeWorkRequest.Builder(typeof(GeolocatorWork)).Build();

			WorkManager.Instance.Enqueue(request);
		}

		public override void OnStopped()
		{
			base.OnStopped();
			//Init();
		}
	}

	/// <summary>
	///  Classe que pode utilizar o serviço de alarme para enviar uma notificação ao 
	///  BroadcastReceiver responsável por iniciar os serviços em Background.
	/// </summary>
	//public class AlarmForGeolocator
	//{
	//	/// <summary>
	//	///  Lançar este método em uma Task
	//	/// </summary>
	//	public static void Alarm()
	//	{
	//		AlarmManager manager = (AlarmManager)Application.Context.GetSystemService(Context.AlarmService);

	//		Intent intent = new Intent(Application.Context, typeof(GeolocatorBroadCast));
	//		PendingIntent pending = PendingIntent.GetBroadcast(Application.Context, 0, intent, 0);

	//		Thread.Sleep(TimeSpan.FromMinutes(2));
	//		manager.Set(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + 1, pending);

	//		Log.Debug("gwork", "VERIFICAÇÃO REALIZADA COM ALARMMANAGER!");
	//	}
	//}

}