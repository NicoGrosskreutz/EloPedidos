using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EloPedidos.Adapter
{
	public class EstoqueAdapter : BaseAdapter<RomaneioItem>
	{
		private Activity context;
		private List<RomaneioItem> list;
		public EstoqueAdapter(Activity _context, List<RomaneioItem> _list)
		{
			context = _context;
			list = _list;
		}
		public override RomaneioItem this[int position] => list[position];

		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position)
		{
			return list[position].ES_ESTOQUE_ROMANEIO_ITEM_ID;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_estoque, parent, false);

			var nome = view.FindViewById<TextView>(Resource.Id.lbDSCPROD);
			var qtd = view.FindViewById<TextView>(Resource.Id.txQTDPROD);
			var data = view.FindViewById<TextView>(Resource.Id.txtDTULAT); ;


			if (list[position].DSCRPROD.Length > 38)
				nome.Text = list[position].DSCRPROD.Substring(0, 38);
			else
				nome.Text = list[position].DSCRPROD;
			qtd.Text = (list[position].QTDPROD - list[position].QTDVENDA - list[position].QTDBRINDE + list[position].QTDDEVCL).ToString();
			data.Text = list[position].DTHULTAT.ToString();

			return view;
		}
	}
}