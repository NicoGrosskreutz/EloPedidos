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
using SQLite;

namespace EloPedidos.Models
{
	public class Estoque
	{
		[PrimaryKey, AutoIncrement, Column("ITEM_ESTOQUE_ID")]
		public long ITEM_ESTOQUE_ID { get; set; }
		public long CG_PRODUTO_ID { get; set; }
		public string NOMPROD { get; set; }
		public double QTDPROD { get; set; }
		public double VLRPROD { get; set; }
		public string DSCLOTE { get; set; }
		public long CG_CLASSE_PRODUTO_ID { get; set; }
		public long BARCODE { get; set; }
		public DateTime DSCVCMTO { get; set; }
		public long NCM_PROD { get; set; }
		public long CEST_PROD { get; set; }
		public string USULTAT { get; set; }
		public DateTime DTHULTAT { get; set; }

	}
}