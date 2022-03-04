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
    [Table("CG_PESSOA_DIAS_VCTO")]
    public class Vencimento
    {
        [PrimaryKey, AutoIncrement, Column("CG_PESSOA_DIAS_VCTO_ID")]
        public long? CG_PESSOA_DIAS_VCTO_ID { get; set; } = null;
        public long CG_PESSOA_ID { get; set; }

        /// <summary>
        ///  Quantidade de dias de vencimento
        /// </summary>
        public int QTDDVCTO { get; set; }

        public DateTime DTHULTAT {get;set;}
        public string USRULTAT { get; set; }

        /// <summary>
        ///  Referente a CG_PESSOA_ID
        /// </summary>
        [Ignore]
        public Pessoa Pessoa { get; set; }

    }
}