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
	public class EstoqueAdapterCls
	{
        public long FT_ESTOQUE_ITEM_ID { get; set; }
        public long ES_ROMANEIO_ID { get; set; }
        public long CODPROD { get; set; }
        public string DSCPROD { get; set; }
        public string QTDSALDO { get; set; }

		public EstoqueAdapterCls()
		{
		}

		public EstoqueAdapterCls(long cODPROD, string dSCPROD, string qTDSALDO)
		{
			CODPROD = cODPROD;
			DSCPROD = dSCPROD;
			QTDSALDO = qTDSALDO;
		}
	}

}