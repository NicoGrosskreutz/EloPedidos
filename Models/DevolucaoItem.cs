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
using SQLite;

namespace EloPedidos.Models
{
    [Table("FT_PEDIDO_ITEM_DEVOLUCAO")]
    public class DevolucaoItem
    {
        [PrimaryKey, AutoIncrement, Column("FT_PEDIDO_ITEM_DEVOLUCAO_ID")]
        public long? FT_PEDIDO_ITEM_DEVOLUCAO_ID { get; set; } = null;
        public string CODEMPRE { get; set; }
        public long? CG_VENDEDOR_ID { get; set; }
        public long? FT_PEDIDO_ITEM_ID { get; set; } 
        public long? FT_PEDIDO_ID { get; set; }
        public long? CG_PRODUTO_ID { get; set; }
        public long CODPROD { get; set; }
        public long? NROPEDIDO { get; set; } = null;
        public string? NOMPESS { get; set; } = null;
        public string? NOMPROD { get; set; } = null;
        public string IDTUNID { get; set; }
        /// <summary>
        /// Localização do local da baixa
        /// </summary>
        public long? LOCALIZACAO_ID { get; set; }
        public DateTime DATDEVOL { get; set; }
        public double QTDDEVOL { get; set; }
        public string DSCOBSER { get; set; }
        public DateTime DTHULTAT { get; set; }
        public string USRULTAT { get; set; }
        public bool INDSINC { get; set; }

        public long PositionIten { get; set; }
        /// <summary>
        /// Referente a CG_VENDEDOR_ID
        /// </summary>
        [Ignore]
        public Vendedor Vendedor { get; set; }

        /// <summary>
        /// Referente a FT_PEDIDO_ITEM_ID
        /// </summary>
        [Ignore]
        public ItemPedido ItemPedido { get; set; }

        /// <summary>
        /// Referente a FT_PEDIDO_ID
        /// </summary>
        [Ignore]
        public Pedido Pedido { get; set; }

        /// <summary>
        /// Referente a CG_PRODUTO_ID
        /// </summary>
        [Ignore]
        public Produto Produto { get; set; }

        /// <summary>
        ///  Carrega as dependências da classe se possível
        /// </summary>
        public void Load()
        {
            try
            {
                Vendedor = new Controllers.VendedorController().Vendedor;
                this.CG_VENDEDOR_ID = Vendedor.CG_VENDEDOR_ID;

                if (this.FT_PEDIDO_ITEM_ID.HasValue)
                    ItemPedido = new Controllers.ItemPedidoController().FindById(this.FT_PEDIDO_ITEM_ID.Value);

                if (this.FT_PEDIDO_ID.HasValue)
                    Pedido = new Controllers.PedidoController().FindById(this.FT_PEDIDO_ID.Value);

                if (this.CG_PRODUTO_ID.HasValue)
                    Produto = new Controllers.ProdutoController().FindById(this.CG_PRODUTO_ID.Value);
            }
            catch
            {
                throw;
            }
        }
	}
}