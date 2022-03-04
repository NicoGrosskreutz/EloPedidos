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
	public class Agenda
	{
		public long FT_PEDIDO_ID { get; set;}
		public long? ID_PESSOA { get; set; } = null;
		public long NROPEDID { get; set; }
		public string NOMFANT { get; set; }
		public DateTime DATEMISS { get; set; }
		public string DATERET { get; set; }
		public long? CG_PESSOA_ID { get; set; }
		public double VLRAREC { get; set; }
	}
}