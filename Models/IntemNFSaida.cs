using SQLite;


namespace EloPedidos.Models
{
    public class ItemNFSaida
    {
        [PrimaryKey, Column("FT_ITEM_NFSAIDA_ID")]
        public long? FT_ITEM_NFSAIDA_ID { get; set; } = null;
        public long? FT_NFSAIDA_ID { get; set; } = null;
        public long? CG_PRODUTO_ID { get; set; } = null;
        public string CPLPROD { get; set; }
        public double QTDUNIT { get; set; }
        public double QTDPROD { get; set; }
        public double VLRUNIT { get; set; }
        public double VLRDSCTO { get; set; }
        public double QTDATPRO { get; set; }
        public int SITPEDID { get; set; }
        public string DTHULTAT { get; set; }
        public string USRULTAT { get; set; }

        /// <summary>
        ///  Referente a FT_NFSAIDA_ID
        /// </summary>
        [Ignore]
        public NFSaida NFSaida { get; set; }

        /// <summary>
        ///  Referente a CG_PRODUTO_ID
        /// </summary>
        [Ignore]
        public Produto Produto { get; set; }
    }
}