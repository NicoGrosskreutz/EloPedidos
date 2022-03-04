using Android.App;
using Android.OS;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Utils;
using System;
using System.Linq;
using static EloPedidos.Models.Pedido;

namespace EloPedidos.Views
{
    [Activity(Label = "BuscarPedidoDevolucao")]
    public class BuscarPedidoDevolucao : BuscarPedidoView
    {
        private CheckBox ckCancelado;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ckCancelado = FindViewById<CheckBox>(Resource.Id.ckCancelados);
            lbCheckBox = FindViewById<TextView>(Resource.Id.lbCheckBox);

            ckCancelado.Visibility = Android.Views.ViewStates.Invisible;
            lbCheckBox.Visibility = Android.Views.ViewStates.Invisible;
        }

        protected override void LoadList(string name, bool cancelado)
        {
            try
            {
                var pedidos = new PedidoController().FindAll()
                    .Where(p => !p.INDCANC &&
                    (p.SITPEDID != (int)SitPedido.Atendido && p.SITPEDID != (int)SitPedido.Cancelado) &&
                     !p.DSCPRZPG.Equals("0"))
                    .OrderBy(p => p.NROPEDID).ToList();

                if (!name.IsEmpty())
                    pedidos = pedidos.Where(p => p.Pessoa.NOMFANTA.ToLower().Contains(name.ToLower())).ToList();

                FormatList(pedidos);
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }
    }
}