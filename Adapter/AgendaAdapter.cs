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
	public class AgendaAdapter : BaseAdapter<Agenda>
	{
        private Activity context;
        private List<Agenda> list;

        public AgendaAdapter(Activity _context, List<Agenda> _list)
        {
            context = _context;
            list = _list;
        }

        public override Agenda this[int position] => list[position];

        public override int Count => list == null ? 0 : list.Count;

        public override long GetItemId(int position)
        {
            return list[position].FT_PEDIDO_ID;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_agenda, parent, false);

            var pedido = view.FindViewById<TextView>(Resource.Id.label1);
            var cliente = view.FindViewById<TextView>(Resource.Id.label2);
            var receber = view.FindViewById<TextView>(Resource.Id.label3);
            var data = view.FindViewById<TextView>(Resource.Id.label4);

            var nome = new PessoaController().FindById(list[position].ID_PESSOA.Value).NOMFANTA;
            if (nome.Length > 16)
                nome = nome.Substring(0, 16);

            pedido.Text = list[position].NROPEDID.ToString();
            cliente.Text = nome;
            receber.Text = list[position].VLRAREC.ToString("C");
            data.Text = list[position].DATERET;

            return view;
        }
    }
}