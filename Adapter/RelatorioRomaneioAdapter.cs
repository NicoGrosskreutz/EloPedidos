using Android.App;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using System.Collections.Generic;

namespace EloPedidos.Adapter
{
	public class RelatorioRomaneioAdapter : BaseAdapter<EstoqueAdapterCls>
    {
        private Activity context;
        private List<EstoqueAdapterCls> list;

        public RelatorioRomaneioAdapter(Activity _context, List<EstoqueAdapterCls> _list)
        {
            context = _context;
            list = _list;
        }

        public override EstoqueAdapterCls this[int position] => list[position];

        public override int Count => list == null ? 0 : list.Count;

        public override long GetItemId(int position)
        {
            return list[position].FT_ESTOQUE_ITEM_ID;
        }

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_romaneio, parent, false);

			var codprod = view.FindViewById<TextView>(Resource.Id.label2);
			var dscprod = view.FindViewById<TextView>(Resource.Id.label3);
			var saldo = view.FindViewById<TextView>(Resource.Id.label4);


			codprod.Text = list[position].CODPROD.ToString();
			dscprod.Text = list[position].DSCPROD;
			saldo.Text = list[position].QTDSALDO;

			return view;
		}
	}
}