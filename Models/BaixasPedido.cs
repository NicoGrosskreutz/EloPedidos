using SQLite;
using System;
using System.ComponentModel;

namespace EloPedidos.Models
{
    [Table("FT_PEDIDO_BAIXA")]
    public class BaixasPedido
    {
        /// <summary>
        /// Id da classe
        /// </summary>
        [PrimaryKey, Unique]
        public long? FT_PEDIDO_BAIXA_ID { get; set; } = null;
        /// <summary>
        /// Id do pedido realcionado
        /// </summary>
        public long? FT_PEDIDO_ID
        {
            get { return this._ft_pedido_id; }
            set
            {
                this._ft_pedido_id = value;

                if (_ft_pedido_id.HasValue)
                    this.Pedido = new Persistence.PedidoDAO().FindById(_ft_pedido_id.Value);
            }
        }
        /// <summary>
        /// Id do cliente relacionado
        /// </summary>
        public long? CG_PESSOA_ID
        {
            get { return this._cg_pessoa_id; }
            set
            {
                this._cg_pessoa_id = value;

                if (_cg_pessoa_id.HasValue)
                    this.Pessoa = new Persistence.PessoaDAO().FindById(_cg_pessoa_id);
            }
        }
        /// <summary>
        /// Id do vendedor
        /// </summary>
        public long? CG_VENDEDOR_ID
        {
            get { return this._cg_vendedor_id; }
            set
            {
                this._cg_vendedor_id = value;

                if (_cg_vendedor_id.HasValue)
                    this.Vendedor = new Persistence.VendedorDAO().FindById(_cg_vendedor_id.Value);
            }
        }
        /// <summary>
        /// Localização do local da baixa
        /// </summary>
        public long? LOCALIZACAO_ID { get; set; }
        /// <summary>
        /// Id da empresa
        /// </summary>
        public string CODEMPRE { get; set; }
        /// <summary>
        /// Data do pagamento
        /// </summary>
        public DateTime DATPGMT { get; set; }
        /// <summary>
        /// Data de vencimento
        /// </summary>
        public DateTime DATVCTO { get; set; }
        /// <summary>
        /// Total pedido
        /// </summary>
        public double TOTLPEDID { get; set; }
        /// <summary>
        /// Valor total pago
        /// </summary>
        public double VLRPGMT { get; set; }

        /// <summary>
        /// Valor do recebimento
        /// </summary>
        public double ULTPGMTO { get; set; }
        /// <summary>
        /// Valor a receber
        /// </summary>
        public double VLRRECBR { get; set; }
        /// <summary>
        /// Valor de devolução do pedido
        /// </summary>
        public double VLRDEVOL { get; set; }
        /// <summary>
        ///  Indicação do motivo
        /// </summary>
        public int INDMOTVO { get; set; }
        /// <summary>
        ///  Indica se a baixa foi sincronizada com o servidor
        /// </summary>
        public bool INDSINC { get; set; }
        /// <summary>
        ///  Indica se foi pago parcialmente
        /// </summary>
        public string INDPAGO { get; set; }
        /// <summary>
        /// Observação
        /// </summary>
        public string DSCOBSER { get; set; }
        /// <summary>
        /// <para>1 - Aberta</para> 
        /// <para>2 - Atendida</para>
        /// </summary>
        public int SITBAIXA { get; set; }
        /// <summary>
        /// Mensagem de retorno do servidor
        /// </summary>
        public string ENVMSG { get; set; }
        /// <summary>
        /// Data e hora da última atualização
        /// </summary>
        public DateTime DTHULTAT { get; set; }
        /// <summary>
        /// Usuário última atualização
        /// </summary>
        public string USRULTAT { get; set; }
        /// <summary>
        ///  Referente a CG_VENDEDOR_ID
        /// </summary>
        [Ignore]
        public Vendedor Vendedor { get; set; }
        /// <summary>
        ///  Referente a FT_PEDIDO_ID
        /// </summary>
        [Ignore]
        public Pedido Pedido { get; set; }
        /// <summary>
        ///  Referente a CG_PESSOA
        /// </summary>
        [Ignore]
        public Pessoa Pessoa { get; set; }

        /// <summary>
        /// Enum que descreve algum motivo na baixa
        /// </summary>
        public enum Motivo
        {
            [Description("Vencimento Remarcado")]
            RemarcouVencimento = 1,

            [Description("Cliente não encontrado")]
            NaoEncontrou = 2,

            [Description("Somente devolução")]
            SomenteDevolveu = 3,

            [Description("Parcialmente pago")]
            PagamentoParcial = 4,

            [Description("Devolução e pagamento parcial")]
            DevPagoParcial = 5
        }

        public enum SitBaixa
        {
            [Description("Aberta")]
            Aberto = 1,

            [Description("Atendida")]
            Atendido = 2
        }

        private long? _ft_pedido_id = null;
        private long? _cg_pessoa_id = null;
        private long? _cg_vendedor_id = null;
    }
}