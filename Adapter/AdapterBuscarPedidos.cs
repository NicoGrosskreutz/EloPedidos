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
	public class AdapterBuscarPedidos : BaseAdapter<Pedido>
	{
		private Activity context;
		private List<Pedido> list;

		public AdapterBuscarPedidos(Activity _context, List<Pedido> _list)
		{
			context = _context;
			list = _list;
		}

		public override Pedido this[int position] => list[position];

		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position) => list[position].FT_PEDIDO_ID.Value;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_buscarPedidos, parent, false);

			var lblNROPED = view.FindViewById<TextView>(Resource.Id.lblNROPED);
			var lblNOMPESS = view.FindViewById<TextView>(Resource.Id.lblNOMPESS);
			var lblDATA = view.FindViewById<TextView>(Resource.Id.lblDATA);
			var lblVLRTOT = view.FindViewById<TextView>(Resource.Id.lblVLRTOT);


			Pessoa cliente = new PessoaController().FindById(list[position].ID_PESSOA.Value);

			lblNROPED.Text = list[position].NROPEDID.ToString();

			if(cliente != null)
				lblNOMPESS.Text = cliente.NOMFANTA;

			lblDATA.Text = "Emissão: " + list[position].DATEMISS.ToString("dd/MM/yyyy");
			lblVLRTOT.Text = new PedidoController().GetTotalValue(list[position].NROPEDID).ToString("C2");

			return view;
		}
	}
}