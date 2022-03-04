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
    public class DevolucaoItensAdapter : BaseAdapter<DevolucaoItensAdapterCls>
    {
        private Activity context;
        private List<DevolucaoItensAdapterCls> list;

        public DevolucaoItensAdapter(Activity _context, List<DevolucaoItensAdapterCls> _list)
        {
            context = _context;
            list = _list;
        }

        public override DevolucaoItensAdapterCls this[int position] => list[position];

        public override int Count => list == null ? 0 : list.Count;

        public override long GetItemId(int position)
        {
            return list[position].FT_PEDIDO_ITEM_ID;
        }
        
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.adapter_devolucao, parent, false);

            var id = view.FindViewById<TextView>(Resource.Id.label1);
            var dscprod = view.FindViewById<TextView>(Resource.Id.label3);
            var qntprod = view.FindViewById<TextView>(Resource.Id.label4);
            var saldo = view.FindViewById<TextView>(Resource.Id.label5);
            var qnt = view.FindViewById<TextView>(Resource.Id.label6);

            dscprod.Text = list[position].DSCPROD;
            qntprod.Text = list[position].QTDPROD;
            saldo.Text = (list[position].QTDPROD.ToDouble() - list[position].QTDDEVOL.ToDouble() - list[position].QTDDEVOLNOW.ToDouble()).ToString();
            qnt.Text = list[position].QTDDEVOLNOW;

            return view;
        }
    }
}