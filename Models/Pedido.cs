using Android.App;
using Android.Util;
using SQLite;
using System;
using System.Collections.Generic;

namespace EloPedidos.Models
{
    [Table("FT_PEDIDO")]
    public class Pedido
    {
        [PrimaryKey, Column("FT_PEDIDO_ID")]
        public long? FT_PEDIDO_ID { get; set; } = null;
        public string CODEMPRE { get; set; }
        public long NROPEDID { get; set; }
        public DateTime DATEMISS { get; set; }
        public DateTime DATERET { get; set; }

        public long? CG_PESSOA_ID { get; set; }
        public long? ID_PESSOA { get; set; }
        public string NOMPESS { get; set; }
        public string CODMUNIC { get; set; }

        public long? CG_VENDEDOR_ID { get; set; }
		public long? LOCALIZACAO_ID { get; set; } = null;

        /// <summary>
        ///  Prazo pagamento
        /// </summary>
        public string DSCPRZPG { get; set; }
        
        /// <summary>
        ///  Indica forma de pagamento
        /// </summary>
        public string IDTFRMPG { get; set; }

        /// <summary>
        ///  Observação
        /// </summary>
        public string DSCOBSER { get; set; }

        /// <summary>
        ///  Situação do pedido
        ///  <para>Aberto = 1</para>
        ///  <para>Confirmado = 2</para>
        ///  <para>Atendido = 3</para>
        ///  <para>ParcialTotal = 4</para>
        ///  <para>Cancelado = 5</para>
        /// </summary>
        public short SITPEDID { get; set; } = (short)SitPedido.Aberto;

        /// <summary>
        ///  Indica se cancelado
        /// </summary>
        public bool INDCANC { get; set; }

        /// <summary>
        ///  Indica se cancelado
        /// </summary>
        public bool SYNCCANC { get; set; } = true;

        /// <summary>
        ///  Quantidade de dias de entrega
        /// </summary>
        public int QTDDIENT { get; set; }

        /// <summary>
        ///  Motivo de cancelamento
        /// </summary>
        public string DSCMOTCA { get; set; }

        /// <summary>
        ///  Data de confirmação
        /// </summary>
        public string DTHCONFP { get; set; }

        /// <summary>
        ///  Id do estoque de romaneio
        /// </summary>
        public long ES_ESTOQUE_ROMANEIO_ID { get; set; }

        /// <summary>
        ///  Valor total da comissão do vendedor para este pedido
        /// </summary>
        public double TOTLCOMIS { get; set; }

        public string DSCLOCDG { get; set; }

        /// <summary>
        ///  Indica se o pedido foi transmitido para o servidor ou não
        /// </summary>
        public bool INDSINC { get; set; }

        public string USRULTAT { get; set; }

        public DateTime DTHULTAT { get; set; }
		public double PERCOMIS { get; set; }

		/* Criado dia 12/11/2019 */
		/// <summary>
		///  Auxíliar da classe para armazenar retorno do socket
		/// </summary>
		public string MSGPEDID { get; set; }

        /// <summary>
        ///  Referente a CG_PESSOA_ID
        /// </summary>
        [Ignore]
        public Pessoa Pessoa { get ; set; }

        /// <summary>
        ///  Referente a CG_VENDEDOR_ID
        /// </summary>
        [Ignore]
        public Vendedor Vendedor { get; set; }

        /// <summary>
        ///  Itens vinculados a este pedido por <see cref="ItemPedido.FT_PEDIDO_ID"/>
        /// </summary>
        [Ignore]
        public List<ItemPedido> Itens { get; set; }

        /// <summary>
        ///  Enum que indica a situação do pedido
        /// </summary>
        public enum SitPedido
        {
            Aberto = 1,
            Confirmado = 2,
            ParcialTotal = 3,
            Atendido = 4,
            Cancelado = 5
        };

        /// <summary>
        ///  Carrega dependencias da classe se possível
        /// </summary>
        public void Load()
        {
            try
            {
                Vendedor = new Controllers.VendedorController().Vendedor;

                if (CG_PESSOA_ID.HasValue)
                    Pessoa = new Controllers.PessoaController().FindById(ID_PESSOA.Value);

                if (FT_PEDIDO_ID.HasValue)
                    Itens = new Controllers.ItemPedidoController().FindAllOrderItems(FT_PEDIDO_ID.Value);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
            }
        }
    }
}