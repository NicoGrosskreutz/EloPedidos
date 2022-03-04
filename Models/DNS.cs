using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EloPedidos.Models
{
    /// <summary>
    ///  Melhor definição para DNS
    /// </summary>
    public class DNS
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        /// <summary>
        ///  Indica se o dns é interno ou externo
        /// </summary>
        public IndDNS DNSInfo { get; set; }

        public enum IndDNS
        {
            DNSInterno = 1,
            DNSExterno = 2
        };
    }
}