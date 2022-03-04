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
    [Table("CG_VENDEDOR_SEQ_PEDIDO")]
    public class Sequencia
    {
        [PrimaryKey, Column("CG_VENDEDOR_SEQ_PEDIDO_ID")]
        public long? CG_VENDEDOR_SEQ_PEDIDO_ID { get; set; } = null;
        public long CG_VENDEDOR_ID { get; set; }
        public long NROPEDIN { get; set; }
        public long NROPEDFI { get; set; }
        public long NROPEDAT { get; set; }
        public DateTime DTHULTAT { get; set; }
        public string USRULTAT { get; set; }

        /// <summary>
        ///  Referente a CG_VENDEDOR_ID
        /// </summary>
        [Ignore]
        public Vendedor Vendedor { get; set; }
    }
}