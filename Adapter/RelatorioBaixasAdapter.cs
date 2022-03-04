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
	class RelatorioBaixasAdapter : BaseAdapter<Pagamento>
	{

		private Activity _context;
		private List<Pagamento> _list;

		public RelatorioBaixasAdapter(Activity context,  List<Pagamento> lista)
		{
			_context = context;
			_list = lista;
		}

		public override Pagamento this[int position] => _list[position];
		public override int Count => _list == null ? 0 : _list.Count;
		public override long GetItemId(int position)
		{
			return _list[position].FT_PAGAMENTO_ID.Value;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.adapter_relatoriobaixa, parent, false);

			var data = view.FindViewById<TextView>(Resource.Id.lblDATA);
			var cliente = view.FindViewById<TextView>(Resource.Id.lblNOMPESS);
			var pago = view.FindViewById<TextView>(Resource.Id.lbTotal);

			var nome = new PessoaController().FindByCG_PESSOA_ID(_list[position].CG_PESSOA_ID.Value).NOMFANTA;
			if (nome.Length > 16)
				nome = nome.Substring(0, 16);

			data.Text = _list[position].DTHULTAT.ToString("dd/MM/yyyy");
			cliente.Text = nome;
			pago.Text = _list[position].VLRPGMT.ToString("C");

			return view;
		}


	}

}