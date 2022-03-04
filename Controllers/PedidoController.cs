using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using static EloPedidos.Models.Pedido;

namespace EloPedidos.Controllers
{
	public class PedidoController
	{
		private PedidoDAO DAO;

		public PedidoController()
		{
			DAO = new PedidoDAO();
		}

		public int Count { get { return DAO.Count(); } }

		public int NotCanceledCount { get { return DAO.NotCanceledCount(); } }

		public int NotSyncCount { get { return DAO.NotSyncCount(); } }

		public List<Pedido> CanceledNotSync { get { return DAO.CanceledNotSync(); } }

		public bool Save(Pedido pedido)
		{
			var bController = new BaixasPedidoController();

			if (pedido.INDCANC)
				throw new Exception("NÃO É POSSÍVEL ALTERAR PEDIDO CANCELADO!");

			if (bController.FindByNROPEDID(pedido.NROPEDID) != null && pedido.SITPEDID != (short)Pedido.SitPedido.Aberto)
				throw new Exception("NÃO É POSSÍVEL ALTERAR UM PEDIDO COM BAIXA PARCIAL OU TOTAL!");

			return DAO.Save(pedido);
		}

		public Pedido FindById(long pFT_PEDIDO_ID)
		{
			return DAO.FindById(pFT_PEDIDO_ID);
		}
		public Pedido getLastPedido(long id) => DAO.getLastOpenOrder(id);
		public bool Delete(Pedido p)
		{
			return DAO.Delete(p.FT_PEDIDO_ID.Value);
		}

		public List<Pedido> FindByCG_PESSOA_ID(long pCG_PESSOA_ID)
			=> (List<Pedido>)DAO.FindByCG_PESSOA_ID(pCG_PESSOA_ID);

		public List<Pedido> FindBy_ID_PESSOA(long pCG_PESSOA_ID)
			=> (List<Pedido>)DAO.FindBy_ID_PESSOA(pCG_PESSOA_ID);

		/// <summary>
		///  Busca todos os pedidos abertos referentes ao cliente (CG_PESSOA_ID)
		/// </summary>
		/// <param name="pCG_PESSOA_ID"></param>
		/// <returns></returns>
		public List<Pedido> FindOpenOrderByCG_PESSOA_ID(long pCG_PESSOA_ID)
			=> DAO.FindOpenOrderByCG_PESSOA_ID(pCG_PESSOA_ID);

		public List<Pedido> FindAll()
		{
			return (List<Pedido>)DAO.FindAll();
		}

		public List<Pedido> FindAllToReceive(long pCG_PESSOA_ID)
		{
			List<Pedido> pedido = new List<Pedido>();
			pedido = this.FindByCG_PESSOA_ID(pCG_PESSOA_ID);
			BaixasPedidoController bController = new BaixasPedidoController();
			if (pedido.Count > 0)
			{
				List<Pedido> aux = new List<Pedido>();
				pedido.ForEach(p =>
				{
					if (p.SITPEDID != (short)Pedido.SitPedido.Atendido)
						if (p.SITPEDID != (short)Pedido.SitPedido.Cancelado)
							if (p.SITPEDID != (short)Pedido.SitPedido.Aberto)
							{
								var baixa = bController.FindByFT_PEDIDO_ID(p.FT_PEDIDO_ID.Value);
								if (baixa != null)
									if (baixa.VLRRECBR > 0)
										aux.Add(p);
							}
				});
				if (aux.Count > 0)
					pedido = aux;
			}


			return pedido;
		}

		public List<Pedido> Atrasados(long pCG_PESSOA_ID, out double valor)
		{
			double v = 0;
			List<Pedido> pedido = new List<Pedido>();
			pedido = this.FindByCG_PESSOA_ID(pCG_PESSOA_ID);
			BaixasPedidoController bController = new BaixasPedidoController();
			if (pedido.Count > 0)
			{
				List<Pedido> aux = new List<Pedido>();
				pedido.ForEach(p =>
				{
					var baixa = bController.FindByFT_PEDIDO_ID(p.FT_PEDIDO_ID.Value);
					if (p.SITPEDID != (short)Pedido.SitPedido.Atendido)
						if (p.SITPEDID != (short)Pedido.SitPedido.Cancelado)
							if (p.SITPEDID != (short)Pedido.SitPedido.Aberto)
								if (p.DATERET != null)
									if (p.DATERET < DateTime.Now)
										if (baixa != null)
											if (baixa.VLRRECBR > 0)
											{
												aux.Add(p);
												v += baixa.VLRRECBR;
											}
				});
				pedido = aux;
			}
			valor = v;

			return pedido;
		}

		public double totalReceberCliente(long pCG_PESSOA_ID, out string[] _Pedidos)
		{
			double valor = 0;
			PedidoController pController = new PedidoController();
			ItemPedidoController iController = new ItemPedidoController();
			List<string> arreyPedidos = new List<string>();

			try
			{
				List<Pedido> pedidos = pController.FindByCG_PESSOA_ID(pCG_PESSOA_ID);
				if (pedidos.Count > 0)
				{
					pedidos.ForEach(p =>
					{
						if (p.SITPEDID != (short)Pedido.SitPedido.Atendido)
							if (p.SITPEDID != (short)Pedido.SitPedido.Aberto)
								if (p.SITPEDID != (short)Pedido.SitPedido.Cancelado)
								{
									double receber = new BaixasPedidoController().toReceive(p.FT_PEDIDO_ID.Value);
									if (receber > 0)
									{
										arreyPedidos.Add(p.NROPEDID.ToString());
										valor += receber;
									}
								}
					});
				}
			}
			catch
			{
				valor = 0;
			}
			_Pedidos = arreyPedidos.ToArray();
			return valor;
		}

		public List<Pedido> FindAll(bool cancelado)
		{
			return DAO.FindAll(cancelado);
		}
		public List<Pedido> FindAll(bool cancelado, string municipio)
		{
			return DAO.FindAll(cancelado, municipio);
		}

		public List<Pedido> FindAll(bool cancelado, string municipio, DateTime dataI, DateTime dataF)
		{
			return DAO.FindAll(cancelado, municipio, dataI, dataF);
		}

		public List<Pedido> FindByName(string name, bool cancelado)
		{
			return DAO.FindByName(name, cancelado);
		}


		public List<Pedido> FindByName(string name, bool cancelado, string municipio)
		{
			return DAO.FindByName(name, cancelado, municipio);
		}


		public List<Pedido> FindByName(string name, bool cancelado, string municipio, DateTime dataI, DateTime dataF)
		{
			return DAO.FindByName(name, cancelado, municipio, dataI, dataF);
		}


		public Pedido FindByNROPEDID(long pNROPEDID)
		{
			return DAO.FindByNROPEDID(pNROPEDID);
		}

		/// <summary>
		///  Retorna o valor total do pedido
		/// </summary>
		/// <param name="pNROPEDID"></param>
		/// <returns></returns>
		public double GetTotalValue(long pNROPEDID)
		{
			ItemPedidoController iController = new ItemPedidoController();

			var pedido = FindByNROPEDID(pNROPEDID);
			var itens = iController.FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);

			double total = 0;

			//itens.Where(i => !i.INDBRIND)
			//	.ToList().ForEach(i => total += iController.GetTotalValue(i));

			itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
			{
				total += (i.QTDPROD * i.VLRUNIT) - i.VLRDSCTO;
			});

			return total;
		}
		public void sincronizarCancelados()
		{
			List<Pedido> pedidos = DAO.FindAllCanceled();
			if (pedidos.Count > 0)
			{
				pedidos.Where(p => p.SYNCCANC == false).ToList().ForEach(c => { this.syncCancellation(c); });
			}
		}

		public bool CancelarPedidoPorN(long pNROPEDID, string MTVOCANCEL, out string message)
		{
			try
			{
				var pedido = FindByNROPEDID(pNROPEDID);

				if (pedido == null)
				{
					message = "PEDIDO NÃO ENCONTRADO!";
					return false;
				}
				else if (pedido.INDCANC)
				{
					message = "PEDIDO JÁ CANCELADO!";
					return false;
				}
				else if (pedido.SITPEDID == (short)SitPedido.Confirmado || pedido.SITPEDID == (short)SitPedido.Atendido ||
					pedido.SITPEDID == (short)SitPedido.ParcialTotal)
				{
					message = "PEDIDO JÁ CONFIRMADO OU ATENDIDO! NÃO É POSSÍVEL ALTERAR.";
					return false;
				}
				else if (new BaixasPedidoController().GerarBaixa(pedido).VLRPGMT > 0 || new BaixasPedidoController().GerarBaixa(pedido).VLRDEVOL > 0)
				{
					message = "NÃO É POSSÍVEL CANCELAR PEDIDO COM BAIXA ABERTA OU TOTAL";
					return false;
				}
				else
				{
					message = "PEDIDO CANCELADO!";
					pedido.DSCMOTCA = MTVOCANCEL;

					if (DAO.CancelarPedidoPorN(pNROPEDID, MTVOCANCEL, message, out Pedido newPedido))
						return true;
					else
						return false;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				message = "Erro";
				return false;
			}
		}

		public bool SetSync(long pFT_PEDIDO_ID)
		{
			return DAO.SetSync(pFT_PEDIDO_ID);
		}

		/// <summary>
		/// Pedido Atendido
		/// </summary>
		public bool SetAnswered(long pFT_PEDIDO_ID)
		{
			return DAO.SetAnswered(pFT_PEDIDO_ID);
		}
		public bool SetParcial(long pFT_PEDIDO_ID)
		{
			return DAO.SetParcial(pFT_PEDIDO_ID);
		}

		/// <summary>
		/// Pedido Confirmado
		/// </summary>
		/// <param name="pFT_PEDIDO_ID"></param>
		/// <returns></returns>
		public bool SetConfirmed(long pFT_PEDIDO_ID)
		{
			return DAO.SetConfirmed(pFT_PEDIDO_ID);
		}
		public bool SetAtendidoParcial(long pFT_PEDIDO_ID)
		{
			return DAO.SetAtendidoParcial(pFT_PEDIDO_ID);
		}

		public List<Pedido> FindAllNotSync()
			=> DAO.FindAllNotSync();

		/// <summary>
		///  Formata um listView com os items do pedido
		/// </summary>
		public ArrayAdapter<string> FormatListViewItems(Pedido p)
		{
			try
			{
				List<ItemPedido> list = new ItemPedidoController().FindAllOrderItems(p.FT_PEDIDO_ID.Value);

				ArrayAdapter<string> adapter = new ArrayAdapter<string>(Application.Context, Resource.Layout.simplelist);

				var pController = new ProdutoController();
				list.ForEach(aux =>
				{
					aux.Produto = pController.FindById(aux.CG_PRODUTO_ID.Value);
					string brinde = aux.INDBRIND ? "S" : "N";
					string idtunid = aux.Produto.IDTUNID.IsEmpty() ? "" : "x " + aux.Produto.IDTUNID;
					adapter.Add($"{aux.Produto.CODPROD} - {aux.Produto.DSCPROD} - {aux.QTDPROD} {idtunid} - " +
						$"R$ {aux.VLRUNIT.ToString("0.00").Replace(".", ",")} - " +
						$"R$ {((aux.QTDPROD * aux.VLRUNIT) - aux.VLRDSCTO).ToString("0.00").Replace(".", ",")} - {brinde}");
				});

				return adapter;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		///  Transmite o pedido para o servidor
		/// </summary>
		/// <param name="pedido"></param>
		/// <returns></returns>
		public bool ComSocket(Pedido pedido, out string message)
		{
			string outStr = string.Empty;
			bool result = false;
			Thread t = null;

			if (new ConfigController().TestServerConnection())
			{

				DNS dns = new ConfigController().GetDNS();
				TcpClient client = null;
				NetworkStream netStream = null;
				StringBuilder builder = null;

				bool isClienteSincronizado = true;

				if (pedido.ID_PESSOA != null)
				{
					if (pedido.CG_PESSOA_ID == null)
					{
						var pessoa = new PessoaController().FindById(pedido.ID_PESSOA.Value);
						if (pessoa != null)
						{
							if (!new PessoaController().SincPessoa(pessoa, out string msg))
								isClienteSincronizado = false;
							else
							{
								var p = new PessoaController().FindById(pedido.ID_PESSOA.Value);
								pedido.CG_PESSOA_ID = p.CG_PESSOA_ID;
								this.Save(pedido);
							}
						}
					}
				}

				if (isClienteSincronizado)
				{
					t = new Thread(() =>
					{
						try
						{
							var localizacao = "";

							if (pedido.LOCALIZACAO_ID.HasValue)
								localizacao = new GeolocatorController().GetLocalizationString(pedido.LOCALIZACAO_ID.Value);

							client = new TcpClient(dns.Host, dns.Port);

							netStream = client.GetStream();
							netStream.ReadTimeout = 120000;
							//client.ReceiveTimeout = 4000;

							builder = new StringBuilder();

							builder.Append("GERARPEDIDO")
							.Append(pedido.CODEMPRE)
							.Append("@@")
							.Append(pedido.NROPEDID)
							.Append(";")
							.Append(pedido.CG_PESSOA_ID)
							.Append(";")
							.Append(pedido.DSCPRZPG)
							.Append(";")
							.Append(pedido.CG_VENDEDOR_ID)
							.Append(";")
							.Append(pedido.DSCOBSER)
							.Append(";")
							.Append(pedido.DATEMISS.ToString("dd/MM/yyyy"))
							.Append(";")
							.Append(localizacao)
							.Append(";")
							.Append(pedido.PERCOMIS) // Percentual de comissão
							.Append(";")
							.Append(pedido.DTHULTAT.ToString())
							.Append(";")
							.Append(pedido.USRULTAT)
							.Append("@@");

							var items = new ItemPedidoController().FindAllOrderItems(pedido.FT_PEDIDO_ID.Value);

							/* Parte responsável por organizar a string a ser enviada (itens do pedido) */
							items.ForEach((aux) =>
							{
								builder.Append(aux.CG_PRODUTO_ID)
								.Append(";")
								.Append(/* DSCCOMPL */ "")
								.Append(";")
								.Append(aux.QTDUNID)
								.Append(";")
								.Append(aux.QTDPROD)
								.Append(";")
								.Append(aux.VLRUNIT)
								.Append(";")
								.Append(aux.VLRDSCTO)
								.Append(";")
								.Append(aux.INDBRIND ? 1 : 0) // Indica se o produto é brinde
								.Append("#");
							});

							if (builder.ToString().EndsWith("#"))
							{
								string temp = builder.ToString().Substring(0, builder.Length - 1);
								builder.Clear();
								builder.Append(temp);
								builder.Append("@@FIM@@");
							}

							// Envia os bytes para o servidor
							byte[] msg = builder.ToString().ToUTF8(true);
							netStream.Write(msg, 0, msg.Length);

							if (netStream.CanRead)
							{
								byte[] bytes = new byte[client.ReceiveBufferSize];
								netStream.Read(bytes, 0, client.ReceiveBufferSize);
								string received = bytes.UTF7ToString();

								if (received.Contains("\0\0"))
									received = received.Split("\0\0")[0];

								/* Caso o pedido retorne Ok */
								if (received.ToLower().Contains("pedidook"))
								{
									this.SetSync(pedido.FT_PEDIDO_ID.Value);
									result = true;
								}
								else if (received.ToLower().Contains("confirmado"))
								{
									result = this.SetConfirmed(pedido.FT_PEDIDO_ID.Value);
									this.SetSync(pedido.FT_PEDIDO_ID.Value);
								}
								else if (received.ToLower().Contains("atendido"))
								{
									result = this.SetAnswered(pedido.FT_PEDIDO_ID.Value);
									this.SetSync(pedido.FT_PEDIDO_ID.Value);
								}
								else if (received.ToLower().Contains("não pode ser alterado"))
								{
									this.SetSync(pedido.FT_PEDIDO_ID.Value);
								}
								/* Caso erro */
								else if (received.ToLower().Contains("pedidoerro"))
								{
									outStr = pedido.MSGPEDID = received.Split("@@")[1];
									result = false;
								}
								else
									result = false;
							}

							builder.Clear();
						}
						catch (Exception ex)
						{
							Log.Error("error", ex.ToString());
							if (!ex.Message.ToUpper().StartsWith("UNABLE TO READ"))
								outStr = ex.Message;
							else
								outStr = "TEMPO PARA SINCRONIZAR PEDIDO ESGOTADO";

							result = false;
						}
						finally
						{
							if (client != null) client.Close();
							if (netStream != null) netStream.Close();
						}
					});
				}
				else
				{
					result = false;
					outStr = "NÃO FOI POSSIVEL SINCRONIZAR O CLIENTE";
				}
			}
			else
			{
				outStr = "SEM CONEXÃO COM O SERVIDOR";
				result = false;
			}

			if (t != null)
			{
				t.Start();
				t.Join();
			}

			message = outStr;
			return result;
		}

		/// <summary>
		///  Transmite todos os pedidos
		/// </summary>
		/// <returns></returns>
		public bool ConveryAll()
		{
			try
			{
				FindAll().ForEach((aux) => this.ComSocket(aux, out string msg));
				return true;
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return false;
			}
		}

		/// <summary>
		///  Transmite todos os pedidos
		/// </summary>
		/// <returns></returns>
		public bool ConveryAll(out string[] output)
		{
			try
			{
				List<string> errors = new List<string>();

				FindAll().ForEach((aux) =>
				{
					this.ComSocket(aux, out string msg);
					errors.Add(msg);
				});

				output = errors.ToArray();
				return true;
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				output = new string[] { ex.ToString() };
				return false;
			}
		}

		/// <summary>
		///  Transmite todos os pedidos não sincronizados
		/// </summary>
		public bool ConveryAllNotSync()
		{
			try
			{
				string error = string.Empty;

				FindAllNotSync().ForEach(aux =>
				{
					this.ComSocket(aux, out string msg);
					error += msg + "\n";
				});

				if (error.Length > 0)
					throw new Exception($"OCORRERAM ERROS AO TENTAR SINCRONIZAR DADOS! \n{error}");

				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// Sincroniza a baixa e a devolucao de um pedido
		/// </summary> 
		/// <returns></returns>
		public bool ComSocketReceivementAndDevolution(Pedido p, out string message, Pagamento pagamento = null)
		{
			TcpClient client = null;
			NetworkStream stream = null;

			bool result = false;
			string strMSg = "";

			Thread t = new Thread(() =>
			{
				try
				{
					var dns = new ConfigController().GetDNS();

					client = new TcpClient(dns.Host, dns.Port);
					stream = client.GetStream();

					var gController = new GeolocatorController();

					StringBuilder builder = new StringBuilder();

					string localization = "";
					string dthloc = "";

					BaixasPedidoController baixaController = new BaixasPedidoController();
					BaixasPedido baixa = baixaController.FindByNROPEDID(p.NROPEDID);

					bool semBaixa = true;
					if (baixa != null)
						if (!baixa.INDSINC)
							semBaixa = false;

					DevolucaoItemController devolucaoController = new DevolucaoItemController();
					List<DevolucaoItem> itensDevolucao = devolucaoController.FindNOTSYNC(p.FT_PEDIDO_ID.Value);

					if (!semBaixa || itensDevolucao.Count > 0 || pagamento != null)
					{
						localization = "";
						dthloc = "";

						DevolucaoItem devol = itensDevolucao.Where(i => i.LOCALIZACAO_ID != null).FirstOrDefault();

						if (baixa != null)
						{
							if (baixa.LOCALIZACAO_ID != null)
								localization = gController.GetLocalizationStringLATLONG(baixa.LOCALIZACAO_ID.Value);
							else
							{
								if (devol != null)
									if (devol.LOCALIZACAO_ID != null)
										localization = gController.GetLocalizationStringLATLONG(devol.LOCALIZACAO_ID.Value);
							}

							if (!baixa.DATPGMT.ToString().StartsWith("01/01/0001 00:00:00"))
								dthloc = baixa.DATPGMT.ToString("dd/MM/yyyy HH:mm:ss");
							else
							{
								if (devol != null)
								{
									if (!devol.DATDEVOL.ToString("dd/MM/yyyyy").StartsWith("01/01/0001 00:00:00"))
										dthloc = devol.DATDEVOL.ToString("dd/MM/yyyy HH:mm:ss");
									else
										dthloc = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
								}
								else
									dthloc = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
							}

						}
						if (localization == ",")
							localization = "";

						string INDPARC = "S";
						if(baixa != null)
							if (baixa.VLRRECBR == 0)
								INDPARC = "N";

						///Pagamento
						builder.Append("BAIXARPEDIDO")
						   .Append(new EmpresaController().Empresa.CODEMPRE)
						   .Append("@@")
						   .Append(p.NROPEDID)
						   .Append(";")
						   .Append(localization)
						   .Append(";")
						   .Append(dthloc)
						   .Append(";")
						   .Append(baixa == null ? "" : baixa.DATVCTO.ToString("dd/MM/yyyy"))
						   .Append(";")
						   .Append(baixa == null ? "" : baixa.DTHULTAT.ToString("dd/MM/yyyy HH:mm:ss"))
						   .Append(";")
						   .Append(baixa == null ? "" : baixa.USRULTAT)
						   .Append("@@")
						   .Append((pagamento == null || pagamento.INDSYNC) ? "0" : pagamento.VLRPGMT.ToString())
						   .Append(";")
						   .Append(pagamento == null ? "S" :  INDPARC)
						   .Append("@@");

						///Devolução
						if (itensDevolucao.Count > 0)
							itensDevolucao.ForEach(i =>
							{
								var item = new DevolucaoItemController().FindById(i.FT_PEDIDO_ITEM_DEVOLUCAO_ID.Value);
								if (item != null && !item.INDSINC)
								{
									var codprod = new DevolucaoItemController().FindById(i.FT_PEDIDO_ITEM_DEVOLUCAO_ID.Value).CODPROD;
									///Devoluções
									builder.Append(codprod)
												.Append(";")
												.Append(i.QTDDEVOL)
												.Append("#");
								}
							});
						else
							builder.Append(" ");

						string aux = builder.ToString();

						builder.Clear();

						if (aux.EndsWith("#"))
							aux = aux.Substring(0, aux.Length - 1);

						aux += "@@FIM@@";

						byte[] bytes = aux.ToUTF8(true);
						stream.Write(bytes, 0, bytes.Length);

						if (stream.CanRead)
						{
							bytes = new byte[client.ReceiveBufferSize];
							stream.Read(bytes, 0, client.ReceiveBufferSize);
							string received = bytes.UTF7ToString();

							if (received.Contains("\0\0"))
								received = received.Split("\0\0")[0];

							if (received.ToUpper().Contains("BAIXAOK"))
							{
								if (baixa != null)
									baixaController.SaveSync(baixa, received);

								if (itensDevolucao.Count > 0)
									itensDevolucao.ForEach(i => { devolucaoController.SaveSync(i); });

								if(pagamento != null)
                                {
									pagamento.INDSYNC = true;
									baixaController.SalvarPagamento(pagamento);
								}


								result = true;
							}
							else if (received.ToUpper().Contains("QUANTIDADE DEVOLVIDA MAIOR QUE SALDO PRODUTO NO PEDIDO"))
							{
								if (baixa != null)
									baixaController.SaveSync(baixa, received);

								if (itensDevolucao.Count > 0)
									itensDevolucao.ForEach(i => { devolucaoController.SaveSync(i); });

								if (pagamento != null)
								{
									pagamento.INDSYNC = true;
									baixaController.SalvarPagamento(pagamento);
								}

								result = true;
							}
							else if (received.ToUpper().Contains("ATENDIDO"))
							{

									string error = received.ToUpper().Replace("BAIXARPEDIDOERRO", "").Replace("@@", "");
									strMSg = error;

									if (baixa != null)
										baixaController.SaveSync(baixa, received);

									if (itensDevolucao.Count > 0)
										itensDevolucao.ForEach(i => { devolucaoController.SaveSync(i); });

									if (pagamento != null)
									{
										pagamento.INDSYNC = true;
										baixaController.SalvarPagamento(pagamento);
									}

									result = true;

							}
							else if (received.ToUpper().Contains("ERRO"))
							{
								string error = received.ToUpper().Replace("BAIXARPEDIDOERRO", "").Replace("@@", "");
								strMSg = error;

								result = false;
							}
						}
					}
					else
						result = true;
				}
				catch (Exception ex)
				{
					Log.Error("Error", ex.ToString());

					if (ex.Message.ToUpper().StartsWith("UNABLE TO READ"))
						strMSg = "TEMPO PARA SINCRONIZAR PEDIDO ESGOTADO";
					else if (ex.Message.ToUpper().StartsWith("NETWORK"))
						strMSg = "VOCÊ ESTÁ SEM INTERNET";
					else
						strMSg = ex.Message;

					result = false;
				}
				finally
				{
					if (stream != null) stream.Close();
					if (client != null) client.Close();
				}
			});

			t.Start();
			t.Join();

			message = strMSg;
			return result;
		}

		/// <summary>
		///  Recebe os pedidos do servidor
		/// </summary>
		/// <param name="pedido"></param>
		/// <returns></returns>
		public bool ComSocketclientOrders(string request)
		{
			string outStr = string.Empty;

			DNS dns = new ConfigController().GetDNS();

			TcpClient client = null;
			NetworkStream netStream = null;

			bool result = false;

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
							return false;
						if (receiveMsg.Contains("FIMPEDIDO"))
						{
							dados = dados + receiveMsg;
							loop = false;
						}
						else
							dados = dados + receiveMsg;
					}
				string[] lines = dados.Split("@@");
				Pedido auxPedido = null;
				long? cG_Pessoa_id = null;

				Database.GetConnection().RunInTransaction(() =>
				{
					int position = 0;

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

									if (this.FindByNROPEDID(data[1].ToLong()) == null)
										this.Save(p);
									else
										DAO.Update(p);

									auxPedido = new PedidoController().FindByNROPEDID(data[1].ToLong());

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
										if (i != "")
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

											if (ip.QTDPROD != ip.QTDATPRO)
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

				result = true;
				return result;
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return result;
			}
			finally
			{
				if (client != null) client.Close();
				if (netStream != null) netStream.Close();
			}
		}

		public bool syncCancellation(Pedido pedido)
		{
			bool result = false;

			if (pedido.INDSINC)
			{
				Thread t = new Thread(() =>
				{
					TcpClient client = null;
					NetworkStream netStream = null;
					StringBuilder builder = null;
					DNS dns = new ConfigController().GetDNS();

					try
					{
						client = new TcpClient();
						client.Connect(dns.Host, dns.Port);

						netStream = client.GetStream();

						builder = new StringBuilder();

						builder.Append("CANCELARPEDIDO")
						.Append(new EmpresaController().GetEmpresa().CODEMPRE)
						.Append(";")
						.Append(pedido.NROPEDID)
						.Append(";")
						.Append(pedido.DSCMOTCA)
						.Append(";")
						.Append(pedido.USRULTAT)
						.Append(";")
						.Append(pedido.DTHULTAT.ToString("dd/MM/yyyy HH:mm:ss"));

						byte[] msg = builder.ToString().ToUTF8(true);
						netStream.Write(msg, 0, msg.Length);

						if (netStream.CanRead)
						{
							byte[] bytes = new byte[client.ReceiveBufferSize];
							netStream.Read(bytes, 0, client.ReceiveBufferSize);
							string received = bytes.UTF7ToString();

							if (received.Contains("\0\0"))
								received = received.Split("\0\0")[0];

						/* Caso o pedido retorne Ok */
							if (received.ToUpper().StartsWith("CANCELARPEDIDO@@CANCELARPEDIDOOK"))
							{
								pedido.SYNCCANC = true;
								Database.GetConnection().Update(pedido);
								result = true;
							}
							else if (received.ToUpper().StartsWith("CANCELARPEDIDOERRO@@PEDIDO JÁ ESTÁ CANCELADO!@@"))
							{
								pedido.SYNCCANC = true;
								pedido.SITPEDID = (short)Pedido.SitPedido.Cancelado;
								Database.GetConnection().Update(pedido);
								result = true;
							}
							else
								result = false;
						}

						builder.Clear();
					}
					catch (Exception e)
					{
						Log.Error("SyncError", e.ToString());
						result = false;
					}
					finally
					{
						if (client != null) client.Close();
						if (netStream != null) netStream.Close();
					}
				});
				t.Start();
				t.Join();
			}

			return result;
		}

		public string menssagemParaEnvio(Pedido pedido, List<ItemPedido> itens)
		{
			string menssagem = string.Empty;

			Empresa empresa = new EmpresaController().Empresa;
			Pessoa cliente = new PessoaController().FindById(pedido.ID_PESSOA.Value);
			Municipio municipio = new MunicipioController().FindById(empresa.CODMUNIC);
			Municipio municipioCliente = new MunicipioController().FindById(cliente.CODMUNIC);
			Vendedor vendedor = new VendedorController().GetVendedor();

			StringBuilder b = new StringBuilder();

			b.Append($"PEDIDO: {pedido.NROPEDID}\n");
			b.Append($"EMISSÃO: {pedido.DTHULTAT.ToString("dd/MM/yyyy")}\n");

			b.Append("\n");
			b.Append("========== EMPRESA ===========\n");
			b.Append($"{empresa.NOMFANTA}\n");
			b.Append($"RUA: {empresa.DSCENDER}, {empresa.NROENDER}\n");
			b.Append($"{empresa.NOMBAIRR}, {municipio.NOMMUNIC}/ {municipio.CODUF}\n");
			b.Append($"{Utils.Format.MaskFone(empresa.NROFONE)}\n");
			b.Append($"VENDEDOR: {vendedor.NOMVEND}\n");
			b.Append($"TELEFONE: {Utils.Format.MaskFone(vendedor.NROTLFN)}\n");

			b.Append("\n");
			b.Append("=========== CLIENTE ===========\n");
			b.Append($"{cliente.NOMPESS.ToUpper()}\n");
			b.Append($"DOCUMENTO: {Utils.Format.MaskCPF_CNPJ(cliente.IDTDCPES.ToString(), cliente.IDTPESS)}\n");
			b.Append($"RUA: {cliente.DSCENDER.ToUpper()}, {cliente.NROENDER.ToUpper()}\n");
			b.Append($"{cliente.NOMBAIRR.ToUpper()}, {municipioCliente.NOMMUNIC}/ {municipioCliente.CODUF}\n");

			b.Append("\n");
			b.Append("========== PRODUTOS ==========\n");
			itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
			{
				Produto produto = new ProdutoController().FindById(i.CG_PRODUTO_ID.Value);

				string nomeProduto = produto.DSCPROD;

				if (nomeProduto.Length > 14)
					nomeProduto = nomeProduto.Substring(0, 14);

				b.Append($"{nomeProduto} ( {produto.IDTUNID} ) QTD: {i.QTDPROD.ToString()} - VLR UNIT.: {i.VLRUNIT.ToString("C2")} - VLR TOTAL: {(i.VLRUNIT * i.QTDPROD).ToString("C2")}\n");
				b.Append("\n");
			});

			b.Append($"TOTAL: {new PedidoController().GetTotalValue(pedido.NROPEDID).ToString("C2")}\n");
			b.Append("\n");

			b.Append($"OBSERVAÇÕES:\n");
			b.Append($"{pedido.DSCOBSER}\n");
			b.Append($"DATA DO RETORNO: {pedido.DATERET.ToString("dd/MM/yyyy")}\n");
			b.Append("\n");

			double toReceive = this.totalReceberCliente(pedido.CG_PESSOA_ID.Value, out string[] _Pedidos);
			if (toReceive > 0)
			{
				b.AppendLine($"SALDO DEVEDOR: {toReceive.ToString("C2")}");
				_Pedidos.ToList().ForEach(p =>
				{
					Pedido ped = new PedidoController().FindByNROPEDID(p.ToLong());
					if (ped != null)
					{
						double valor = new BaixasPedidoController().toReceive(ped.FT_PEDIDO_ID.Value);
						if (valor > 0)
							b.AppendLine($"Pedido: {p} - {valor.ToString("C2")}");
					}
				});
			}

			menssagem = b.ToString();
			return menssagem;
		}
	}

}