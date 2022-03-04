using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Utils;
using EloPedidos.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Android.App.ActivityManager;

namespace EloPedidos.Services
{
    [Service(Name = "Services.LocationService")]
    public class LocationService : Service
    {
        NotificationCompat.Builder notification = null;
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        [return: GeneratedEnum]
        [Obsolete]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            notification = new NotificationCompat.Builder(this, "PedidosNotification");
            var notificationIntent = new Intent(Application.Context, typeof(MainActivity));
            var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);

            var cancelNotificationIntent = new Intent("closeNotification");
            var cancelPendingIntent = PendingIntent.GetBroadcast(Application.Context, 0, cancelNotificationIntent, 0);

            var hora = DateTime.Now.Hour;
            string text = string.Empty;
            if (hora < 17)
            {
                text = "Sincronizando os pedidos...";
            }
            else
            {
                List<BaixasPedido> baixas = new BaixasPedidoController().FindByDatRet(DateTime.Now);
                if (baixas.Count > 0)
                {
                    if (baixas.Count == 1)
                        text = $"Existe 1 pedido para remarcar o vencimento";
                    else
                        text = $"Existem {baixas.Count} pedidos para remarcar o vencimento";
                }
                else
                    text = "Sincronizando os pedidos...";
            }

            //var builder = new NotificationCompat.Builder(this, "PedidosNotification")
            //.SetContentTitle("Elo Pedidos")
            //.SetContentText(text)
            //.SetContentIntent(pendingIntent)
            //.SetSmallIcon(Resource.Mipmap.logo)
            //.SetAutoCancel(false)
            //.AddAction(Resource.Drawable.abc_ic_arrow_drop_right_black_24dp, "Cancelar", cancelPendingIntent)
            //.SetChannelId("PedidosNotification")
            //.SetPriority(4)
            //.Build();

            notification.SetContentTitle("Elo Pedidos");
            notification.SetContentText(text);
            notification.SetContentIntent(pendingIntent);
            notification.SetSmallIcon(Resource.Mipmap.logo);
            notification.SetAutoCancel(false);
            notification.AddAction(Resource.Drawable.abc_ic_arrow_drop_right_black_24dp, "Cancelar", cancelPendingIntent);
            notification.SetChannelId("PedidosNotification");
            notification.SetPriority(4);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                createNotificationChannel("PedidosNotification", "Canal");

            StartForeground(1, notification.Build());
            startSincronizacao();

            //StartNotification();

            return base.OnStartCommand(intent, flags, startId);
        }

        private void startLocationUpdates()
        {
            Task.Run(async () =>
            {
                await new Localization().GetLocalizationAsync();
                Thread.Sleep(300000);
                startLocationUpdates();
            });

            Log.Debug("EloPEDIDOS", "LOCATION SERVICE INICIADO");
        }

        private void startSincronizacao()
        {
            Task.Run(() =>
            {
                if (new ConfigController().TestServerConnection())
                    new Sincronizador().SincronizarAllNotSync();

                Thread.Sleep(30000);
                startSincronizacao();
            });

            Log.Debug("EloPEDIDOS", "LOCATION SERVICE INICIADO");
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void createNotificationChannel(string channelId, string channelName)
        {
            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.None);

            channel.LightColor = Color.Blue;
            channel.LockscreenVisibility = NotificationVisibility.Private;

            NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        private void StartNotification()
        {
            if (notification != null)
            {
                NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);
                notificationManager.Notify(90000, notification.Build());
            }
        }

        private void stopNotificationChannel()
        {
            NotificationManagerCompat manager = NotificationManagerCompat.From(Application.Context);
            manager.Cancel(1);
        }

        public bool isServiceRunning()
        {
            string service = "Services.LocationService";
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
    }
}