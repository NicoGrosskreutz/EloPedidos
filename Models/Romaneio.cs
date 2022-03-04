using SQLite;
using System;

namespace EloPedidos.Models
{
	[Table("ES_ESTOQUE")]
	public class Romaneio
	{
		[PrimaryKey, Column("ES_ESTOQUE_ROMANEIO_ID")]
		public long ES_ESTOQUE_ROMANEIO_ID { get; set; }
		public long NROROMAN { get; set; }
		public DateTime DATEMISS { get; set; }
		public short SITROMAN { get; set; }
		public double NROKMINI { get; set; }
		public double NROKMFIN { get; set; }
		public string IDTPLACA { get; set; }
		public string DSCOBSER { get; set; }
		public DateTime DTHULTAT { get; set; }
		public string USRULTAT { get; set; }
		


		public enum SitRoman
		{
			Aberto = 1,
			Fechado = 2
		}
	}
}