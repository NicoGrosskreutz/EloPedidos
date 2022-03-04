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
    [Table("CG_VENDEDOR")]
    public class Vendedor
    {
        [PrimaryKey, Column("CG_VENDEDOR_ID")]
        public long? CG_VENDEDOR_ID { get; set; } = null;
        public string CODEMPRE { get; set; }
        public long? CODVEND { get; set; } = null;
        public string NOMVEND { get; set; }
        public string NROTLFN { get; set; }
        public string USROPER { get; set; }
        public long ES_ESTOQUE_LOCAL_ID { get; set; }
        public double PERCOMIS { get; set; } = 30; /* 30 inicializado para teste remover depois */
        public DateTime DTHULTAT { get; set; }
        public string USRULTAT { get; set; }

        /// <summary>
        ///  Referente a USROPER
        /// </summary>
        [Ignore]
        public Operador Operador { get; set; }
    }
}