using SQLite;

namespace EloPedidos.Models
{
    [Table("FT_NFESAIDA")]
    public class NFSaida
    {
        [PrimaryKey, Column("FT_NFSAIDA_ID")]
        public long? FT_NFSAIDA_ID { get; set; } = null;
        public string CODEMPRE { get; set; }
        public long? CG_PESSOA_ID { get; set; } = null;
        public int NRONF { get; set; }
        public string DATEMISS { get; set; }
        public long? CG_VENDEDOR_ID { get; set; } = null;
        /// <summary>
        /// Descrição de prazo de pagamento
        /// </summary>
        public string DSCPRZPG { get; set; }
        public long? ES_ESTOQUE_ROMANEIO_ID { get; set; } = null;
        /// <summary>
        ///  Forma de pagamento
        /// </summary>
        public int IDTFRMPG { get; set; }
        public string DSCOBSER { get; set; }
        public int SITNF { get; set; }
        public bool INDCANC { get; set; }
        public int QTDDIENT { get; set; }
        /// <summary>
        ///  Descrição do motivo de cancelamento
        /// </summary>
        public string DSCMOTCA { get; set; }
        /// <summary>
        /// Data de confirmação
        /// </summary>
        public string DTHCONFP { get; set; }
        public bool INDSINC { get; set; }
        public string USRULTAT { get; set; }
        public string DTHULTAT { get; set; }

        /// <summary>
        ///  Referente a CG_PESSOA_ID
        /// </summary>
        [Ignore]
        public Pessoa Pessoa { get; set; }

        /// <summary>
        ///  Referente a CG_VENDEDOR_ID
        /// </summary>
        [Ignore]
        public Vendedor Vendedor { get; set; }

        public enum SituacaoDANFE
        {
            Imprimir = 0,
            AguardadoImpressao = 1,
            Impressoa = 2
        };

        public enum FormaPagamento
        {

        };
    }
}