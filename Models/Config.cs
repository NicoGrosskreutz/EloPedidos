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
    [Table("CONFIG")]
    public class Config
    {
        [PrimaryKey, AutoIncrement, Column("CONFIG_ID")]
        public long? CONFIG_ID { get; set; } = null;
        public string DNSINT { get; set; }
        public string DNSEXT { get; set; }
        public bool INDSINC { get; set; }
        /// <summary>
        ///  True - DNS Externo / False - DNS Interno
        /// </summary>
        public bool INDDNS { get; set; }
        public bool INDEMAIL { get; set; }
        public string DSCEMAIL { get; set; }
        public string DTHSINC { get; set; }
        public long VERSAODB { get; set; }
        public bool ENVLOC { get; set; }
        public bool GRVLOC { get; set; }
        public bool INDDESAT { get; set; }
        public int MINESP { get; set; }
        public bool GRVLOG { get; set; }
        public int CODMUNPQ { get; set; }
        public string NOMMUNPQ { get; set; }
        public decimal VLR2COM { get; set; }
        public decimal PER2COM { get; set; }
		public string NOMIMPRE { get; set; }

        /// <summary>
        /// INDICA SE A EMPRESA IRÁ USAR CÓDIGO DE BARRA
        /// </summary>
        public bool CODEAN { get; set; } = true;

        /// <summary>
        /// VERIFICA A DATA DA ULTIMA VERIFICAÇÃO DE PERMISSÃO DE USO
        /// </summary>
        public DateTime DTHULTVER { get; set; }
        public bool isAuthorized { get; set; }
        public string DSCRERRO { get; set; }
        public bool? BLOQROMAN { get; set; } = null;
    }
}