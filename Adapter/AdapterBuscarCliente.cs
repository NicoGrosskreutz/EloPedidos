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
	public class AdapterBuscarCliente : BaseAdapter<Pessoa>
	{
		private Activity context;
		private List<Pessoa> list;

		public AdapterBuscarCliente(Activity _context, List<Pessoa> _list)
		{
			context = _context;
			list = _list;
		}

		public override Pessoa this[int position] => list[position];

		public override int Count => list == null ? 0 : list.Count;

		public override long GetItemId(int position)
		{
			return list[position].ID.Value;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.Adapter_clientes, parent, false);

			var NOMPESS = view.FindViewById<TextView>(Resource.Id.lblNOMPESS);
			var NRODOC = view.FindViewById<TextView>(Resource.Id.lblNRODOC);
			//var MUNIC = view.FindViewById<TextView>(Resource.Id.lblMUNICIPIO);
			var RUA = view.FindViewById<TextView>(Resource.Id.lblRUA);
			var BAIRRO = view.FindViewById<TextView>(Resource.Id.lblBAIRRO);


			//MUNIC.Text = municipio.NOMMUNIC;

			NOMPESS.Text = list[position].NOMFANTA.ToUpper();
			NRODOC.Text = list[position].IDTPESS;
			RUA.Text = list[position].DSCENDER + " " + list[position].NROENDER;
			BAIRRO.Text = list[position].NOMBAIRR;

			Municipio municipio = new MunicipioController().FindById(list[position].CODMUNIC);
			if (municipio != null)
			{
				if (!string.IsNullOrEmpty(list[position].NOMBAIRR))
					BAIRRO.Text = list[position].NOMBAIRR + " - " + municipio.NOMMUNIC;
				else
					BAIRRO.Text = municipio.NOMMUNIC;
			}
			return view;
		}
	}
}