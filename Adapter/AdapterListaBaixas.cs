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
using EloPedidos.Controllers;

namespace EloPedidos.Adapter
{
    public class AdapterListaBaixas : BaseAdapter<BaixasPedido>
    {
        private Activity context;
        private List<BaixasPedido> list;

        public AdapterListaBaixas(Activity context, List<BaixasPedido> list)
        {
            this.context = context;
            this.list = list;
        }

        public override BaixasPedido this[int position] => list[position];

        public override int Count => list == null ? 0 : list.Count;

        public override long GetItemId(int position)
        {
            if (list[position].FT_PEDIDO_BAIXA_ID.HasValue)
                return list[position].FT_PEDIDO_BAIXA_ID.Value;
            else
                return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_pedidos, parent, false);

            var data = view.FindViewById<TextView>(Resource.Id.txDATEMISS);
            var nro = view.FindViewById<TextView>(Resource.Id.listNROPEDID);
            var total = view.FindViewById<TextView>(Resource.Id.listTOTLPEDID);
            var receber = view.FindViewById<TextView>(Resource.Id.listVLRRECBR);

            BaixasPedido baixa = list[position];
            Pedido pedido = new PedidoController().FindById(baixa.FT_PEDIDO_ID.Value);

            data.Text = pedido.DATEMISS.ToString("dd/MM/yyyy");
            nro.Text = pedido.NROPEDID.ToString();
            total.Text = baixa.TOTLPEDID.ToString("C2");
            receber.Text = baixa.VLRRECBR.ToString("C2");

            if (new PedidoController().FindByNROPEDID(pedido.NROPEDID).SITPEDID == (short)Pedido.SitPedido.Aberto)
                view.SetBackgroundColor(Android.Graphics.Color.Cyan);
            return view;
        }
    }
}