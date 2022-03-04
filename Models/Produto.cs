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
    [Table("CG_PRODUTO")]
    public class Produto
    {
        [PrimaryKey, Column("CG_PRODUTO_ID")]
        public long? CG_PRODUTO_ID { get; set; } = null;
        public string CODEMPRE { get; set; }
        public long? CODPROD { get; set; }
        public string DSCPROD { get; set; }
        public string IDTUNID { get; set; }
        public string CODEAN { get; set; }
        public long? CODNCM { get; set; }
		public long CG_CLASSE_PRODUTO_ID { get; set; }

		/// <summary>
		///  Quantidade produto unidade
		/// </summary>
		public double QTDUNID { get; set; }

        /// <summary>
        ///  Preço de venda
        /// </summary>
        public double PRCVENDA { get; set; }
        
        /// <summary>
        ///  Preço custo
        /// </summary>
        public double PRCCUSTO { get; set; }

        /// <summary>
        ///  Percentual de desconto especial
        /// </summary>
        public double PERDSESP { get; set; }

        /// <summary>
        ///  Percentual Preço a Vista
        /// </summary>
        public double PERVISTA { get; set; }

        /// <summary>
        ///  Indica se esta inativo
        /// </summary>
        public bool INDINAT { get; set; }

        public DateTime DTHULTAT { get; set; }

        public string USRULTAT { get; set; }
    }
}