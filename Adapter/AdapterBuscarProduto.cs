using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EloPedidos.Adapter
{
	public class AdapterBuscarProduto : BaseAdapter<Produto>
	{
		private Activity context;
		private List<Produto> list;

		public AdapterBuscarProduto(Activity _context, List<Produto> _list)
		{
			context = _context;
			list = _list;
		}
		public override Produto this[int position] => list[position];
		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position)
		{
			return list[position].CG_PRODUTO_ID.Value;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Adapter_BuscarProd, parent, false);

			TextView codprod = view.FindViewById<TextView>(Resource.Id.lblCODPROD);
			TextView nomprod = view.FindViewById<TextView>(Resource.Id.lblNOMPROD);
			TextView saldo = view.FindViewById<TextView>(Resource.Id.lblQTDPROD);

			codprod.Text = list[position].CODPROD.ToString();

			if (list[position].DSCPROD.Length <= 40)
				nomprod.Text = list[position].DSCPROD;
			else
				nomprod.Text = list[position].DSCPROD.Substring(0, 40);

			RomaneioItem romaneioItem = new RomaneioController().FindByIdItem(list[position].CG_PRODUTO_ID.Value);

			if (romaneioItem != null)
				saldo.Text = (romaneioItem.QTDPROD + romaneioItem.QTDDEVCL - romaneioItem.QTDVENDA - romaneioItem.QTDBRINDE).ToString();
			else
				saldo.Text = "0";


			return view;
		}
	}
}