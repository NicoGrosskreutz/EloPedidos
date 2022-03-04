using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EloPedidos.Controllers
{
	public class BuscarPedidoController
	{
		public bool FindOrderToReport(out List<Pedido> pedidos, out List<BaixasPedido> baixas)
		{
			bool result = true;
			List<Pedido> listaPedidos = new List<Pedido>();
			List<BaixasPedido> listaBaixas = new List<BaixasPedido>();
			BaixasPedidoController bController = new BaixasPedidoController();
			PedidoController pController = new PedidoController();

			var data = DateTime.Now.ToString("dd/MM/yyyy");

			listaPedidos = pController.FindAll().Where(p => p.DATEMISS.ToString("dd/MM/yyyy") == data).ToList();
			listaBaixas = bController.FindAll().Where(b => b.DTHULTAT.ToString("dd/MM/yyyy") == data).ToList();

			pedidos = listaPedidos;
			baixas = listaBaixas;
			return result;
		}

		public string ReportCreate(List<Pedido> pedidos, List<BaixasPedido> baixas)
		{
			string report = string.Empty;

			StringBuilder builder = new StringBuilder();

			builder.AppendLine($"RELATÓRIO DO VENDEDOR {DateTime.Now.ToString("dd/MM/yyyyy")}\n");

			builder.AppendLine("PEDIDOS");
			builder.AppendLine("=========================");
			builder.AppendLine("PEDIDO | CLIENTE            | TOTAL");

			pedidos.ForEach(p =>
			{
				var nropedido = p.NROPEDID.ToString();
				var barraNroPedido = " | ".PadLeft(8);
				var nomecliente = new PessoaController().FindByCG_PESSOA_ID(p.CG_PESSOA_ID.Value).NOMFANTA;
				var barraNomeCLiente = " | ".PadLeft(20);
				var total = new BaixasPedidoController().ValorTotal(p.NROPEDID).ToString("C2");

				if (nomecliente.Length > 10)
					nomecliente = nomecliente.Substring(0, 10);

				builder.AppendLine($"{nropedido}{barraNroPedido}{nomecliente}{barraNomeCLiente}{total}");
			});

			builder.AppendLine("BAIXAS");
			builder.AppendLine("=========================");
			builder.AppendLine("PEDIDO | VALOR PAGO | VALOR DEVOL ");

			baixas.ForEach(b =>
			{
				var p = new PedidoController().FindById(b.FT_PEDIDO_ID.Value);

				var nropedido = p.NROPEDID.ToString();
				var barraNroPedido = " | ".PadLeft(7);
				var totalPago = b.VLRPGMT.ToString("C2");
				var barraTotalPago = " | ".PadLeft(10);
				var barraTotalDevol = b.VLRDEVOL.ToString("C2");

				builder.AppendLine($"{nropedido}{barraNroPedido}{totalPago}{barraTotalPago}{barraTotalDevol}");
			});

			report = builder.ToString();
			return report;
		}
	}
}