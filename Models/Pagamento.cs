using SQLite;
using System;

namespace EloPedidos.Models
{
	[Table("FT_PAGAMENTO")]
	public class Pagamento
	{
		[PrimaryKey, AutoIncrement, Column("FT_PAGAMENTO_ID")]
		public long? FT_PAGAMENTO_ID { get; set; } = null;
		public long? FT_PEDIDO_BAIXA_ID { get; set; } = null;
		public long? FT_PEDIDO_ID { get; set; }
		public long? CG_PESSOA_ID { get; set; }
		public double VLRPGMT { get; set; }
		public bool INDSYNC { get; set; }
		public DateTime DTHPGMTO { get; set; }
		public DateTime DTHRTRN { get; set; }
		public DateTime DTHULTAT { get; set; }
		public string USRULTAT { get; set; }
	}
}