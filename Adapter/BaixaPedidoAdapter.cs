using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;

namespace EloPedidos.Adapter
{
    public class BaixaPedidoAdapter : BaseAdapter<BaixasPedidoAdapterCls>
    {
        private Activity context;
        private List<BaixasPedidoAdapterCls> list;

        public BaixaPedidoAdapter(Activity context, List<BaixasPedidoAdapterCls> list)
        {
            this.context = context;
            this.list = list;
        }

        public override BaixasPedidoAdapterCls this[int position] => list[position];

        public override int Count => list == null ? 0 : list.Count;

        public override long GetItemId(int position)
        {
            return list[position].NROPEDID;   
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_pedidos, parent, false);

            var nro = view.FindViewById<TextView>(Resource.Id.listNROPEDID);
            var total = view.FindViewById<TextView>(Resource.Id.listTOTLPEDID);
            var receber = view.FindViewById<TextView>(Resource.Id.listVLRRECBR);

            nro.Text = list[position].NROPEDID.ToString();
            total.Text = list[position].TOTLPEDID;
            receber.Text = list[position].VLRRECBR;

            if (new PedidoController().FindByNROPEDID(list[position].NROPEDID).SITPEDID == (short)Pedido.SitPedido.Aberto)
                view.SetBackgroundColor(Android.Graphics.Color.Cyan);
            return view;
        }
    }
}