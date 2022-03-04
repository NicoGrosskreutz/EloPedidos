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
    [Table("CG_PESSOA")]
    public class Pessoa
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public long? ID { get; set; } = null;
        public long? CG_PESSOA_ID { get; set; } = null;
        public string CODEMPRE { get; set; }
        public long? CODPESS { get; set; } = null;
        public string NOMPESS { get; set; }
        public string NOMFANTA { get; set; }
        public short TIPPESS { get; set; }
        public string DSCEMAIL { get; set; }
        
        /// <summary>
        /// Indicador documento da pessoa
        /// </summary>
        public short IDTDCPES { get; set; }
        
        /// <summary>
        /// documento cpf ou cnpj
        /// </summary>
        public string IDTPESS { get; set; }

        public string NROINEST { get; set; }
        public string DSCENDER { get; set; }
        public string NROENDER { get; set; }
        public string CPLENDER { get; set; }
        public string NOMBAIRR { get; set; }
        public long CODMUNIC { get; set; }
        public string NOMMUNIC { get; set; }
        public string NROFONER { get; set; }
        public string NROFONEC { get; set; }
        public string NROCELUL { get; set; }
        public double VLRLIMIT { get; set; }
        public double VLRLIMPD { get; set; }
        public long? NROCEP { get; set; } = null;
        public short IDTSEXO { get; set; }
        public string NOMPESCT { get; set; }
        public long CG_FORMA_PAGAMENTO_ID { get; set; }
        public bool INDINAT { get; set; }
        public string DSCEMNFE { get; set; }
        public DateTime DTHULTAT { get; set; }
        public string USRULTAT { get; set; }
        public bool INDSINC { get; set; }

        public enum TipoDocumento
        {
            CPF = 1,
            CNPJ = 0
        };


    }
}