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
	public class EmissaoRelatorioAdapter : BaseAdapter<EmissaoAdapterCls>
	{
		private Activity context;
		private List<EmissaoAdapterCls> list;

		public EmissaoRelatorioAdapter(Activity _context, List<EmissaoAdapterCls> _list)
		{
			context = _context;
			list = _list;
		}

		public override EmissaoAdapterCls this[int position] => list[position];

		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position)
		{
			return list[position].FT_EMISSAO_ID;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_emissoes, parent, false);

			var nroped = view.FindViewById<TextView>(Resource.Id.label2);
			var nomclie = view.FindViewById<TextView>(Resource.Id.label3);
			var valor = view.FindViewById<TextView>(Resource.Id.label4);
			var situacao = view.FindViewById<TextView>(Resource.Id.label5);


			nroped.Text = list[position].NROPED.ToString();
			nomclie.Text = list[position].NOMCLIE;
			valor.Text = list[position].VLRPED;
			situacao.Text = list[position].IDTSIT;

			return view;
		}
	}
}