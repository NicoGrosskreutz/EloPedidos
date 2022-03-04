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
    [Table("FT_PEDIDO_ITEM")]
    public class ItemPedido
    {
        [PrimaryKey, AutoIncrement, Column("FT_PEDIDO_ITEM_ID")]
        public long? FT_PEDIDO_ITEM_ID { get; set; } = null;
        public long? FT_PEDIDO_ID { get; set; }
        public long? CG_PRODUTO_ID { get; set; }
        public string CPLPROD { get; set; }
        public string CODPROD { get; set; }
        public string NOMPROD { get; set; }
        public string IDTUNID { get; set; }
        public double QTDUNID { get; set; }
        public double QTDPROD { get; set; }
        public double VLRUNIT { get; set; }
        public double VLRDSCTO { get; set; }
        public bool INDBRIND { get; set; }
        public long SITPEDID { get; set; }
        /// <summary>
        ///  Quantidade atual do produto
        /// </summary>
        public double QTDATPRO { get; set; } = 0;
        public double QTDDEVOL { get; set; } = 0;
        public string USRULTAT { get; set; }
        public DateTime DTHULTAT { get; set; }

        /// <summary>
        ///  Referente a FT_PEDIDO_ID
        /// </summary>
        [Ignore]
        public Pedido Pedido { get; set; }

        /// <summary>
        ///  Referente a CG_PRODUTO_ID
        /// </summary>
        [Ignore]
        public Produto Produto { get; set; }
    }
}