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

namespace EloPedidos.Models
{
	public class EmissaoAdapterCls
	{
		public long FT_EMISSAO_ID { get; set; }
		public long NROPED { get; set; }
		public string NOMCLIE { get; set; }
		public string VLRPED { get; set; }
		public string IDTSIT { get; set; }
		public string DATAEMISS { get; set; }

		public EmissaoAdapterCls()
		{
		}

		public EmissaoAdapterCls(long nROPED, string nOMCLIE, string vLRPED, string iDTSIT, string dATAEMISS)
		{
			NROPED = nROPED;
			NOMCLIE = nOMCLIE;
			VLRPED = vLRPED;
			IDTSIT = iDTSIT;
			DATAEMISS = dATAEMISS;
		}
	}

}