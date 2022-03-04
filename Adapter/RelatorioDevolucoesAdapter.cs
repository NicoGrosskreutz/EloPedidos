using Android.App;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using System.Collections.Generic;


namespace EloPedidos.Adapter
{
	public class RelatorioDevolucoesAdapter : BaseAdapter<DevolucoesAdapterCls>
	{
		private Activity context;
		private List<DevolucoesAdapterCls> list;

		public RelatorioDevolucoesAdapter(Activity _context, List<DevolucoesAdapterCls> _list)
		{
			context = _context;
			list = _list;
		}

		public override DevolucoesAdapterCls this[int position] => list[position];

		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position)
		{
			return list[position].FT_DEVOL_ID;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_relatoriodevol, parent, false);

			var nroped = view.FindViewById<TextView>(Resource.Id.label2);
			var codprod = view.FindViewById<TextView>(Resource.Id.label4);
			var nompess = view.FindViewById<TextView>(Resource.Id.label3);
			var dscrprod = view.FindViewById<TextView>(Resource.Id.label5);
			var qtddevol = view.FindViewById<TextView>(Resource.Id.label6);


			nroped.Text = list[position].NROPED;
			codprod.Text = list[position].CODPROD;
			nompess.Text = list[position].NOMPESS;
			dscrprod.Text = list[position].DSCRPRO;
			qtddevol.Text = list[position].QTDDEVOL;

			return view;
		}
	}
}