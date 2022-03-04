using System;
using SQLite;

namespace EloPedidos.Models
{
	public class RomaneioItem
	{
		[PrimaryKey, Column("ES_ESTOQUE_ROMANEIO_ITEM_ID")]
		public long ES_ESTOQUE_ROMANEIO_ITEM_ID { get; set; }
		public long ES_ESTOQUE_ROMANEIO_ID { get; set; }
		public long CG_PRODUTO_ID { get; set; }
		public string DSCRPROD { get; set; }
		public string BARCODE { get; set; }
		public double QTDPROD { get; set; }
		public double VLRCUSTO { get; set; }
		public double PRCVENDA { get; set; }
		public double QTDDEVCL { get; set; }
		public double QTDBRINDE { get; set; }
		public double QTDVENDA { get; set; }
		public double QTDCONT { get; set; }
		public DateTime DTHULTAT { get; set; }
		public string USRULTAT { get; set; }
	}
}