using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using EloPedidos.Utils;

namespace EloPedidos.Adapter
{
    public class AdapterItensDevolucao : BaseAdapter<ItemPedido>
    {
        private Activity context;
        private List<ItemPedido> list;
        private List<DevolucaoItem> devolucaoItems;

        public AdapterItensDevolucao(Activity _context, List<ItemPedido> _list, List<DevolucaoItem> _devolucaoItems = null)
        {
            context = _context;
            list = _list;
            devolucaoItems = _devolucaoItems;
        }

        public override ItemPedido this[int position] => list[position];

        public override int Count => list == null ? 0 : list.Count;

        public override long GetItemId(int position)
        {
            return list[position].FT_PEDIDO_ITEM_ID.Value;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_devolucao, parent, false);

            var id = view.FindViewById<TextView>(Resource.Id.label1);
            var dscprod = view.FindViewById<TextView>(Resource.Id.label3);
            var qntprod = view.FindViewById<TextView>(Resource.Id.label4);
            var saldo = view.FindViewById<TextView>(Resource.Id.label5);
            var qnt = view.FindViewById<TextView>(Resource.Id.label6);

            var item = list[position];


            dscprod.Text = item.NOMPROD;
            qntprod.Text = item.QTDPROD.ToString();
            saldo.Text = item.QTDATPRO.ToString();
            qnt.Text = item.QTDDEVOL.ToString();
            //qnt.Text = "0";

            //if (devolucaoItems != null && devolucaoItems.Count > 0)
            //{
            //    foreach(var i in devolucaoItems)
            //    {
            //        if (i.FT_PEDIDO_ITEM_ID == item.FT_PEDIDO_ITEM_ID.Value)
            //            qnt.Text = i.QTDDEVOL.ToString();
            //    }
            //}

            return view;
        }
    }
}