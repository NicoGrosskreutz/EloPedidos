using Android.Views;
using EloPedidos.Controllers;
using System;
using System.Collections.Generic;

namespace EloPedidos.Models
{
    public class BaixasPedidoAdapterCls
    {
        public long NROPEDID
        {
            get { return this._nropedid; }
            set
            {
                this._nropedid = value;
                this.TOTLPEDID = baixaController.ValorTotal(this.NROPEDID).ToString("C");
                //this.TOTLPEDID = baixaController.FindByNROPEDID(this.NROPEDID).TOTLPEDID.ToString("C");
                var p = new PedidoController().FindByNROPEDID(this.NROPEDID);
                //this.VLRRECBR = baixaController.OrderBalance(p.FT_PEDIDO_ID.Value).ToString("C");
                this.VLRRECBR = baixaController.toReceive(p.FT_PEDIDO_ID.Value).ToString("C");
                //this.VLRRECBR = baixaController.FindByNROPEDID(this.NROPEDID).VLRRECBR.ToString("C");
            }
        }

        /// <summary>
        /// Prévia do contas a receber após as devoluções
        /// </summary>
        public long PREVIA_NROPEDID
        {
            get { return this._nropedid; }
            set
            {
                this._nropedid = value;
                this.TOTLPEDID = baixaController.ValorTotal(this.PREVIA_NROPEDID).ToString("C");
                //this.TOTLPEDID = baixaController.FindByNROPEDID(this.NROPEDID).TOTLPEDID.ToString("C");
                var p = new PedidoController().FindByNROPEDID(this.PREVIA_NROPEDID);
                //this.VLRRECBR = baixaController.OrderBalance(p.FT_PEDIDO_ID.Value).ToString("C");
                this.VLRRECBR = baixaController.PrevisaoReceber(itens).ToString("C");
                //this.VLRRECBR = baixaController.FindByNROPEDID(this.NROPEDID).VLRRECBR.ToString("C");
            }
        }

        public string TOTLPEDID { get; set; }
        public string VLRRECBR { get; set; }

        public BaixasPedidoAdapterCls()
        {
            baixaController = new BaixasPedidoController();
        }

        public BaixasPedidoAdapterCls(long _NROPEDID)
        {
            baixaController = new BaixasPedidoController();
            this.NROPEDID = _NROPEDID;
        }

        public BaixasPedidoAdapterCls(List<ItemPedido> itens)
        {
            baixaController = new BaixasPedidoController();
            this.itens = itens;
            this.PREVIA_NROPEDID = new PedidoController().FindById(itens[0].FT_PEDIDO_ID.Value).NROPEDID;
        }

        private BaixasPedidoController baixaController;
        private long _nropedid;
        private List<ItemPedido> itens;


        public static implicit operator View(BaixasPedidoAdapterCls v)
		{
			throw new NotImplementedException();
		}
	}
}