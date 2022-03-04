using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace EloPedidos.Models
{
	[Table("DADAPAR")]
	public class Aparelho
	{
		[PrimaryKey, AutoIncrement, Column("Id")]
		public long Id { get; set; }
		public string ID_APARELHO { get; set; }
		public string DSCAPAR { get; set; }
		public string NOMOPER { get; set; }
		public string IDTPESS { get; set; }
		public string NROVERS { get; set; }
		public string TIPSAPAR { get; set; }
		public string INDINAT { get; set; }
		public bool INDSYNC { get; set; }
		public DateTime DTHULTAT { get; set; }
		public string USRULTAT { get; set; }
	}
}