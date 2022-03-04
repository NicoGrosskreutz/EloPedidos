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
using EloPedidos.Models;

namespace EloPedidos.Adapter
{
	public class AdapterItensPedido : BaseAdapter<ItemPedido>
	{
		private Activity context;
		private List<ItemPedido> list;

		public AdapterItensPedido(Activity _context, List<ItemPedido> _list)
		{
			context = _context;
			list = _list;
		}

		public override ItemPedido this[int position] => list[position];

		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position)
		{
			return list[position].CG_PRODUTO_ID.Value;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Adapter_itensPedido, parent, false);

			var NOMPROD = view.FindViewById<TextView>(Resource.Id.lblNOMPROD);
			var QTDPROD = view.FindViewById<TextView>(Resource.Id.lblQTDPROD);
			//var INDUNID = view.FindViewById<TextView>(Resource.Id.lblINDUNID);
			var VLRUNID = view.FindViewById<TextView>(Resource.Id.lblPRCUNI);
			var VLRTOT = view.FindViewById<TextView>(Resource.Id.lblVLRTOT);


			var auxNOME = list[position].NOMPROD;
			if (auxNOME.Length > 14)
				auxNOME = auxNOME.Substring(0, 14);

			if (list[position].INDBRIND)
				auxNOME = "* " + auxNOME;

			NOMPROD.Text = auxNOME;
			QTDPROD.Text = list[position].QTDPROD.ToString() + " " + list[position].IDTUNID;
			//INDUNID.Text = list[position].IDTUNID;
			VLRUNID.Text = list[position].VLRUNIT.ToString("C");
			VLRTOT.Text = (list[position].QTDPROD * list[position].VLRUNIT).ToString("C");

			return view;
		}
	}
}