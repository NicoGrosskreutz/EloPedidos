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

namespace EloPedidos.Models
{
	public class DevolucoesAdapterCls
	{
		public long FT_DEVOL_ID { get; set; }
		public string NROPED { get; set; }
		public string NOMPESS { get; set; }
		public string CODPROD { get; set; }
		public string DSCRPRO { get; set; }
		public string QTDDEVOL { get; set; }

		public DevolucoesAdapterCls()
		{

		}

		public DevolucoesAdapterCls(string nROPED, string cODPROD, string nOMPESS, string dSCRPRO, string qTDDEVOL)
		{
			NROPED = nROPED;
			CODPROD = cODPROD;
			NOMPESS = nOMPESS;
			DSCRPRO = dSCRPRO;
			QTDDEVOL = qTDDEVOL;
		}
	}
}