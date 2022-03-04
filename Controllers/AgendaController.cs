using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using static EloPedidos.Models.Pedido;

namespace EloPedidos.Controllers
{
	public class AgendaController
	{
		/// <summary>
		///  Recebe os pedidos do servidor
		/// </summary>
		/// <param name="pedido"></param>
		/// <returns></returns>
		public List<Agenda> ComSocketOrders(string request)
		{
			string outStr = string.Empty;

			DNS dns = new ConfigController().GetDNS();

			TcpClient client = null;
			NetworkStream netStream = null;
			StringBuilder builder = null;

			List<Agenda> agenda = new List<Agenda>();
			Pedido auxPedido = null;
			int position = 0;

			try
			{
				client = new TcpClient();
				client.Connect(dns.Host, dns.Port);

				netStream = client.GetStream();

				byte[] msg = request.ToUTF8(true);
				netStream.Write(msg, 0, msg.Length);

				Empresa empresa = new EmpresaController().GetEmpresa();

				bool loop = true;
				string dados = string.Empty;

				while (loop)
					if (netStream.CanRead)
					{
						byte[] bytes = new byte[client.ReceiveBufferSize];
						netStream.Read(bytes, 0, bytes.Length);
						string receiveMsg = bytes.UTF7ToString();

						if (receiveMsg.Contains("\0\0"))
							receiveMsg = receiveMsg.Split("\0\0")[0];

						receiveMsg = receiveMsg.Replace("CARGAPEDIDO@@", "");

						if (receiveMsg.ToUpper().Contains("ERRO"))
							return agenda;
						if (receiveMsg.Contains("FIMPEDIDO"))
						{
							dados = dados + receiveMsg;
							loop = false;
						}
						else
							dados = dados + receiveMsg;
					}
				string[] lines = dados.Split("@@");

				long? cG_Pessoa_id = null;

				Database.GetConnection().RunInTransaction(() =>
				{

					//lines.ToList().ForEach((str) =>
					foreach (string str in lines)
					{
						if (str != lines[lines.Length - 1])
						{
							if (position % 2 == 0)
							{
								string[] data = str.Split(';');

								if (new PessoaController().FindByCG_PESSOA_ID(data[2].ToLong()) != null)
								{
									cG_Pessoa_id = data[2].ToLong();
									Pessoa pessoa = new PessoaController().FindByCG_PESSOA_ID(cG_Pessoa_id.Value);

									Pedido p = new Pedido()
									{
										NROPEDID = data[1].ToLong(),
										CG_PESSOA_ID = data[2].ToLong(),
										ID_PESSOA = (pessoa != null) ? pessoa.ID : null,
										DSCPRZPG = data[3],
										DSCOBSER = data[4],
										DATEMISS = DateTime.Parse(data[5]),
										DATERET = (!string.IsNullOrEmpty(data[6])) ? DateTime.Parse(data[6]) : DateTime.Parse(data[5]).AddMonths(1),
										CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
										CODMUNIC = new PessoaController().FindByCG_PESSOA_ID(data[2].ToLong()).CODMUNIC.ToString(),
										INDSINC = true,
										IDTFRMPG = "1",
										DSCLOCDG = data[7],
										PERCOMIS = data[8].ToDouble(),
										ES_ESTOQUE_ROMANEIO_ID = data[11].ToLong(),
										SITPEDID = short.Parse(data[12]),
										CG_VENDEDOR_ID = data[13].ToLong(),
										USRULTAT = data[15],
										DTHULTAT = DateTime.Parse(data[16])
									};

									if (new PedidoController().FindByNROPEDID(data[1].ToLong()) == null)
										new PedidoController().Save(p);
									else
										new PedidoDAO().Update(p);

									DateTime dataEmissao = new PedidoController().FindByNROPEDID(data[1].ToLong()).DATEMISS;

									auxPedido = new PedidoController().FindByNROPEDID(data[1].ToLong());

									Agenda a = new Agenda()
									{
										FT_PEDIDO_ID = auxPedido.FT_PEDIDO_ID.Value,
										NROPEDID = data[1].ToLong(),
										ID_PESSOA = (pessoa != null) ? pessoa.ID : null,
										CG_PESSOA_ID = data[2].ToLong(),
										DATEMISS = dataEmissao,
										DATERET = data[6],
										VLRAREC = data[10].ToDouble()
									};
									if (pessoa != null)
										a.NOMFANT = pessoa.NOMFANTA;

									agenda.Add(a);


									BaixasPedido b = new BaixasPedido()
									{
										FT_PEDIDO_ID = auxPedido.FT_PEDIDO_ID.Value,
										CG_PESSOA_ID = data[2].ToLong(),
										CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
										DATVCTO = (!string.IsNullOrEmpty(data[6])) ? DateTime.Parse(data[6]) : DateTime.Parse(data[5]).AddMonths(1),
										TOTLPEDID = data[9].ToDouble(),
										VLRRECBR = data[10].ToDouble(),
										CG_VENDEDOR_ID = data[13].ToLong(),
										USRULTAT = data[15],
										DTHULTAT = DateTime.Parse(data[16]),
										INDSINC = true,
									};

									if (new BaixasPedidoController().FindByFT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value) != null)
										b.FT_PEDIDO_BAIXA_ID = new BaixasPedidoController().FindByFT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value).FT_PEDIDO_BAIXA_ID.Value;

									new BaixasPedidoDAO().Save(b);
								}
								else
									cG_Pessoa_id = null;
							}
							else
							{
								if (cG_Pessoa_id != null)
								{
									string[] itens = str.Split("#");

									List<ItemPedido> itensToDelete = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value);
									itensToDelete.ForEach(i => new ItemPedidoController().Delete(i.FT_PEDIDO_ITEM_ID.Value));

									itens.ToList().ForEach(i =>
									{
										if (!string.IsNullOrEmpty(i))
										{
											string[] dataitem = i.Split(";");

											Produto prod = new ProdutoController().FindById(dataitem[1].ToLong());
											ItemPedido ip = new ItemPedido()
											{
												CG_PRODUTO_ID = dataitem[1].ToLong(),
												QTDPROD = dataitem[2].ToDouble(),
												QTDATPRO = dataitem[3].ToDouble(),
												VLRUNIT = dataitem[5].ToDouble(),
												CODPROD = (prod != null) ? prod.CODPROD.Value.ToString() : "0",
												NOMPROD = (prod != null) ? prod.DSCPROD : "PRODUTO NÃO ENCONTRADO",
												IDTUNID = (prod != null) ? prod.IDTUNID : "UN",
												INDBRIND = false,
												FT_PEDIDO_ID = auxPedido.FT_PEDIDO_ID

											};

											new ItemPedidoController().Save(ip);

											if (ip.QTDPROD > ip.QTDATPRO)
											{
												DevolucaoItem d = new DevolucaoItem()
												{
													CG_VENDEDOR_ID = new VendedorController().Vendedor.CG_VENDEDOR_ID,
													CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
													FT_PEDIDO_ITEM_ID = ip.FT_PEDIDO_ITEM_ID,
													CG_PRODUTO_ID = ip.CG_PRODUTO_ID,
													FT_PEDIDO_ID = auxPedido.FT_PEDIDO_ID.Value,
													NROPEDIDO = auxPedido.NROPEDID,
													NOMPESS = new PessoaController().FindById(auxPedido.ID_PESSOA.Value).NOMPESS,
													CODPROD = ip.CODPROD.ToLong(),
													QTDDEVOL = ip.QTDPROD - ip.QTDATPRO,
													USRULTAT = new OperadorController().Operador.USROPER,
													NOMPROD = ip.NOMPROD,
													INDSINC = true,
													Produto = prod
												};

												DevolucaoItem itemDevol = new DevolucaoItemController().FindByCGPRODUTO(dataitem[1].ToLong(), auxPedido.FT_PEDIDO_ID.Value);
												if (itemDevol != null)
													d.FT_PEDIDO_ITEM_DEVOLUCAO_ID = itemDevol.FT_PEDIDO_ITEM_DEVOLUCAO_ID;

												new DevolucaoItemController().SaveItemDevolucao(d);

												BaixasPedido baixas = new BaixasPedidoController().FindByFT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value);
												var totDevol = (ip.QTDPROD - ip.QTDATPRO) * dataitem[5].ToDouble();
												baixas.VLRDEVOL += totDevol;
												new BaixasPedidoDAO().Save(baixas);
											}
										}
									});

									//if (new RomaneioController().FindLast() != null)
									//{
									//	if (new RomaneioController().FindLast().ES_ESTOQUE_ROMANEIO_ID == auxPedido.ES_ESTOQUE_ROMANEIO_ID)
									//	{
									//		List<ItemPedido> itemPedidos = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value);
									//		foreach (var i in itemPedidos)
									//		{
									//			var itemRoman = new RomaneioController().FindByIdItem(i.CG_PRODUTO_ID.Value);
									//			if (itemRoman != null)
									//			{
									//				itemRoman.QTDVENDA = i.QTDATPRO;
									//				new RomaneioController().SaveItem(itemRoman);
									//			}
									//		}
									//	}
									//}

									BaixasPedido baixasPedido = new BaixasPedidoController().FindByFT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value);
									List<ItemPedido> itemPedido = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(auxPedido.FT_PEDIDO_ID.Value);

									var orderBalance = new BaixasPedidoController().OrderBalance(auxPedido.FT_PEDIDO_ID.Value);
									double valorPAGO = orderBalance - baixasPedido.VLRRECBR;

									baixasPedido.VLRPGMT = valorPAGO;
									new BaixasPedidoDAO().Save(baixasPedido);

									cG_Pessoa_id = null;
								}
							}
						}
						position++;
					};
				});

				return agenda;
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return agenda;
			}
			finally
			{
				if (client != null) client.Close();
				if (netStream != null) netStream.Close();
			}
		}

		public List<Pedido> FindByData(DateTime date)
		{
			var baixas = new BaixasPedidoDAO().FindByDataRET(date);

			List<Pedido> pedido = new List<Pedido>();
			baixas.ForEach(b =>
			{
				var p = new PedidoController().FindById(b.FT_PEDIDO_ID.Value);
				if (p != null)
					pedido.Add(p);
			});
			return pedido;
		}
	}
}