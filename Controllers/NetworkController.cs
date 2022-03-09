using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EloPedidos.Controllers
{
    public class NetworkController
    {
        private Context context;

        public NetworkController()
        {
            context = Application.Context;
        }

        /// <summary>
        ///  Retorna verdadeiro se tiver conexão e falso se não.
        /// </summary>
        /// <returns></returns>
        public bool TestConnection()
        {
            var conn = (ConnectivityManager) context.GetSystemService(Context.ConnectivityService);
            var info = conn.ActiveNetworkInfo;
            if (info != null && info.IsConnected)
                return true;
            else
                return false;
        }
    }
}