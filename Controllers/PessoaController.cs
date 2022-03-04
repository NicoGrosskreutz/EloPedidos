using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Android.Content;
using Android.Util;
using Android.Widget;
using EloPedidos.Utils;
using EloPedidos.Models;
using EloPedidos.Persistence;
using System.Linq;
using System.Text;

namespace EloPedidos.Controllers
{
	public class PessoaController
	{
		private PessoaDAO DAO;

		public PessoaController()
		{
			DAO = new PessoaDAO();
		}

		public bool Save(Pessoa pessoa)
		{
			return DAO.Save(pessoa);
		}

		public Pessoa GetLastRecord()
		{
			return DAO.GetLastRecord();
		}

		public List<Pessoa> GetAllNotSinc()
		{
			var conn = Database.GetConnection();
			return conn.Table<Pessoa>().Where(p => p.INDSINC == false).ToList();
		}

		public bool DeleteByCODPESS(long pCODPESS)
		{
			return DAO.DeleteByCODPESS(pCODPESS);
		}

		public Pessoa FindById(long id)
		{
			return DAO.FindById(id);
		}

		public Pessoa FindByCODPESS(long pCODPESS)
		{
			return DAO.FindByCODPESS(pCODPESS);
		}
		public List<Pessoa> FindByName(string name)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pessoa>().ToList().Where(p => p.NOMPESS.ToLower().StartsWith(name.ToLower()) || p.NOMFANTA.ToLower().StartsWith(name.ToLower())).ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return new List<Pessoa>();
			}
		}

		public List<Pessoa> FindByName(string name = "", string CODMUNID = "")
		{
			try
			{
				if (CODMUNID != "")
					return FindByName(name).Where(p => p.CODMUNIC == CODMUNID.ToLong()).ToList();
				else
					return FindByName(name);
			}
			catch (Exception ex)
			{
				Log.Error(Utils.Ext.LOG_APP, ex.ToString());
				return null;
			}
		}

		public Pessoa FindByIDTPESS(string idtpess) => DAO.FindByIDTPESS(idtpess);
		public List<Pessoa> FindAll()
		{
			try
			{
				return DAO.FindAll();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return new List<Pessoa>();
			}
		}
		public bool Delete(object id)
		{
			return DAO.Delete(id);
		}

		public Pessoa FindByCG_PESSOA_ID(long id)
		{
			var conn = Database.GetConnection();
			try
			{
				Pessoa pessoa = conn.Table<Pessoa>().Where(p => p.CG_PESSOA_ID == id).FirstOrDefault();
				return pessoa;
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public bool SetSinc(Pessoa pessoa)
		{
			var conn = Database.GetConnection();
			try
			{
				pessoa.INDSINC = true;
				if (this.Save(pessoa))
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				Log.Error("erroSinc", ex.ToString());
				return false;
			}
		}

		public DateTime GetLastDateTime()
		{
			DateTime DTHULTAT = DAO.GetLastDateTime().Max(p => p.DTHULTAT);
			return DTHULTAT;
		}

		public List<Pessoa> FindAllWithOrders() => DAO.FindAllWithOrder();

		public List<Pessoa> FindWithOrdersByName(string name) => DAO.FindWithOrdersByName(name);

		public bool ComSocket(string request, string host, int port)
		{
			bool aux = false;

			Thread t = new Thread(() =>
			{

				TcpClient client = null;
				NetworkStream netStream = null;
				bool loop = true;

				try
				{
					client = new TcpClient();
					client.Connect(host, port);

					byte[] msg = request.ToUTF8(true);

					netStream = client.GetStream();
					netStream.Write(msg, 0, msg.Length);

					string dados = string.Empty;
					if (netStream.CanRead)
						while (loop)
						{
							byte[] bytes = new byte[client.ReceiveBufferSize];
							netStream.Read(bytes, 0, client.ReceiveBufferSize);
							string receiveMsg = bytes.UTF7ToString();

							if (receiveMsg.Contains("\0\0"))
								receiveMsg = receiveMsg.Split("\0\0")[0];

							if (receiveMsg.Contains("&amp;"))
								receiveMsg = receiveMsg.Replace("&amp", "");

							receiveMsg = receiveMsg.Replace("CARGAPESSOA@@", "");

							if (receiveMsg.ToUpper().Contains("@ERRO"))
								throw new Exception(receiveMsg.ToUpper());

							if (receiveMsg.Contains("FIMPES"))
							{
								dados = dados + receiveMsg;
								loop = false;
							}
							else
								dados = dados + receiveMsg;
						}

					dados = dados.Replace("@@FIMPES", "");
					dados = dados.Replace("FIMPES@@", "");

					string[] lines = dados.Split("@@");
					foreach (var str in lines)
					{
						if (!string.IsNullOrEmpty(str))
						{
							string[] data = str.Split(';');

							Pessoa p = new Pessoa()
							{
								CG_PESSOA_ID = data[0].ToLong(),
								CODEMPRE = data[1],
								CODPESS = data[2].ToLong(),
								NOMPESS = data[3],
								NOMFANTA = data[4],
								TIPPESS = data[5].ToShort(),
								IDTDCPES = data[6].ToShort(),
								IDTPESS = data[7],
								NROINEST = data[8],
								DSCENDER = data[10],
								NROENDER = data[11],
								CPLENDER = data[12],
								NOMBAIRR = data[13],
								CODMUNIC = data[14].ToLong(),
								NROFONER = data[15],
								NROFONEC = data[16],
								NROCELUL = data[17],
								VLRLIMIT = data[18].ToDouble(),
								VLRLIMPD = data[19].ToDouble(),
								NROCEP = data[20].ToLong(),
								IDTSEXO = data[21].ToShort(),
								NOMPESCT = data[22],
								CG_FORMA_PAGAMENTO_ID = data[23].ToLong(),
								INDINAT = data[24].ToBool(),
								DSCEMNFE = data[25],
								DTHULTAT = DateTime.Parse(data[26]),
								USRULTAT = data[27],
								INDSINC = true
							};

							if (this.FindByCG_PESSOA_ID(data[0].ToLong()) != null)
								p.ID = this.FindByCG_PESSOA_ID(data[0].ToLong()).ID.Value;

							this.Save(p);
						}
					}

					aux = true;
				}
				catch (Exception ex)
				{
					string error = "";
					Log.Error(error, ex.ToString());
					aux = false;
				}
				finally
				{
					if (client != null) client.Close();
					if (netStream != null) netStream.Close();
				}

			});

			t.Start();
			t.Join();

			return aux;
		}

		public string tipoDocumento(string doc)
		{
			string result = "2";

			switch (doc.Length)
			{
				case 11:
					result = "1";
					break;
				case 14:
					result = "0";
					break;
			}
			return result;
		}
		public bool SincPessoa(Pessoa pessoa, out string message)
		{
			bool aux = false;
			string strReceived = string.Empty;

			Thread t = new Thread(() =>
			{
				TcpClient client = null;
				NetworkStream netStream = null;
				StringBuilder builder = null;

				try
				{
					client = new TcpClient();
					DNS dns = new ConfigController().GetDNS();
					client.Connect(dns.Host, dns.Port);

					netStream = client.GetStream();

					Empresa empresa = new EmpresaController().GetEmpresa();
					Vendedor vendedor = new VendedorController().GetVendedor();

					var PESS_ID = "";
					var COD_PESS = "";

					if (pessoa.CG_PESSOA_ID != null)
					{
						PESS_ID = pessoa.CG_PESSOA_ID.Value.ToString();
						COD_PESS = pessoa.CODPESS.ToString();
					}

					builder = new StringBuilder();

					builder.Append("MANUTENIRCLIENTE")
					.Append(empresa.CODEMPRE)
					.Append((pessoa.CG_PESSOA_ID != null) ? PESS_ID : "")
					.Append(";")
					.Append((pessoa.CODPESS != null) ? COD_PESS : "")
					.Append(";")
					.Append(pessoa.IDTDCPES)
					.Append(";")
					.Append(pessoa.IDTPESS)
					.Append(";")
					.Append(pessoa.NOMPESS)
					.Append(";")
					.Append(pessoa.NOMFANTA)
					.Append(";")
					.Append(pessoa.NOMPESCT)
					.Append(";")
					.Append(pessoa.DSCENDER)
					.Append(";")
					.Append(pessoa.NROENDER)
					.Append(";")
					.Append(pessoa.CPLENDER)
					.Append(";")
					.Append(pessoa.NOMBAIRR)
					.Append(";")
					.Append(pessoa.CODMUNIC)
					.Append(";")
					.Append(pessoa.NROCEP)
					.Append(";")
					.Append(pessoa.NROCELUL)
					.Append(";")
					.Append(pessoa.NROINEST)
					.Append(";")
					.Append("0") // INDICADOR ISENTO
					.Append(";")
					.Append(pessoa.DSCEMNFE)
					.Append(";")
					.Append(vendedor.CODVEND)
					.Append(";")
					.Append(pessoa.DTHULTAT)
					.Append(";")
					.Append(pessoa.USRULTAT);

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

						if (received.Contains("MANUTENIRCLIENTE@@"))
							received = received.Replace("MANUTENIRCLIENTE@@", "");

						/* Caso o pedido retorne Ok */
						if (received.ToLower().Contains("manutenirok"))
						{
							received = received.Replace("MANUTENIROK", "").Replace("@@", "");
							pessoa.CG_PESSOA_ID = received.Split(";")[0].ToLong();
							pessoa.CODPESS = received.Split(";")[1].ToLong();

							aux = true;
							this.SetSinc(pessoa);
						}
						else if (received.ToLower().Contains("pessoa já cadastrada"))
						{
							strReceived = received;
							this.Delete(pessoa.ID.Value);
						}
						else if (received.ToLower().Contains("não é possível adicionar ou alterar registros"))
						{
							strReceived = received;
							this.Delete(pessoa.ID.Value);
						}
						else
						{
							strReceived = received;
							aux = false;
						}
					}

					builder.Clear();
				}
				catch (Exception ex)
				{
					string error = "";
					Log.Error(error, ex.ToString());
					aux = false;
				}
				finally
				{
					if (client != null) client.Close();
					if (netStream != null) netStream.Close();
				}

			});

			t.Start();
			t.Join();

			message = strReceived;
			return aux;
		}

		public Pessoa ConsultarCNPJ(string request, string host, int port)
		{
			Pessoa p = null;

			Thread t = new Thread(() =>
			{
				TcpClient client = null;
				NetworkStream netStream = null;

				try
				{
					client = new TcpClient();
					client.Connect(host, port);

					byte[] msg = request.ToUTF8(true);

					netStream = client.GetStream();
					netStream.Write(msg, 0, msg.Length);

					if (netStream.CanRead)
					{
						byte[] bytes = new byte[client.ReceiveBufferSize];
						netStream.Read(bytes, 0, client.ReceiveBufferSize);
						string receiveMsg = bytes.UTF7ToString();

						if (receiveMsg.Contains("\0\0"))
							receiveMsg = receiveMsg.Split("\0\0")[0];

						if (receiveMsg.Contains("&amp;"))
							receiveMsg = receiveMsg.Replace("&amp", "");

						receiveMsg = receiveMsg.Replace("CONSULTARCNPJ@@", "");

						if (!receiveMsg.StartsWith("ERRO") || !receiveMsg.IsEmpty())
						{
							string[] lines = receiveMsg.Split("@@");

							if (lines.Length > 0)
							{
								string[] data = lines[1].Split(';');

								p = new Pessoa()
								{
									NOMPESS = data[0],
									NOMFANTA = data[1],
									DSCENDER = data[2],
									NROENDER = data[3],
									NOMBAIRR = data[4],
									CODMUNIC = data[5].ToLong(),
									NOMMUNIC = data[6],
									NROCEP = data[7].ToLong(),
									NROINEST = data[8]
								};

							}
						}
					}
				}
				catch (Exception ex)
				{
					string error = "";
					Log.Error(error, ex.ToString());
				}
				finally
				{
					if (client != null) client.Close();
					if (netStream != null) netStream.Close();
				}

			});

			t.Start();
			t.Join();

			return p;
		}
	}
}