using Android.Util;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace EloPedidos.Controllers
{
	public class RomaneioController
	{
		public List<RomaneioItem> GetRomaneio { get => this.FindAll(); }

		public RomaneioController()
		{
			Database.GetConnection().CreateTable<Romaneio>();
			Database.GetConnection().CreateTable<RomaneioItem>();
		}
		public List<RomaneioItem> FindAll()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<RomaneioItem>().ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}
		public List<RomaneioItem> FindBy_NOMPROD(string dscr)
		{
			var conn = Database.GetConnection();
			try
			{
				var prod = dscr.ToUpper();
				var lastID = conn.Table<RomaneioItem>().ToList().FirstOrDefault().ES_ESTOQUE_ROMANEIO_ID;
				return conn.Table<RomaneioItem>().Where(e => e.DSCRPROD.Contains(prod) && e.ES_ESTOQUE_ROMANEIO_ID == lastID).ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public Romaneio FindById(long ROMANEIOID)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Romaneio>().Where(r => r.ES_ESTOQUE_ROMANEIO_ID == ROMANEIOID).ToList().FirstOrDefault();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}
		public Romaneio FindLast()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Romaneio>().ToList().FirstOrDefault();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}
		public RomaneioItem FindItemById(long ITEMID)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<RomaneioItem>().Where(r => r.ES_ESTOQUE_ROMANEIO_ITEM_ID == ITEMID).ToList().FirstOrDefault();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public RomaneioItem FindByIdItem(long ITEMID)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<RomaneioItem>().Where(r => r.CG_PRODUTO_ID == ITEMID).ToList().FirstOrDefault();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public RomaneioItem FindByCB(string cb)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<RomaneioItem>().Where(r => r.BARCODE == cb).ToList().FirstOrDefault();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}
		public void Save(Romaneio r)
		{
			var conn = Database.GetConnection();
			if (r.ES_ESTOQUE_ROMANEIO_ID != 0)
				if (FindById(r.ES_ESTOQUE_ROMANEIO_ID) == null)
				{
					conn.Insert(r);
				}
				else if (FindById(r.ES_ESTOQUE_ROMANEIO_ID) != null)
				{
					conn.Update(r);
				}
		}
		public bool SaveItem(RomaneioItem r)
		{
			var conn = Database.GetConnection();
			bool result = false;
			try
			{
				if (FindItemById(r.ES_ESTOQUE_ROMANEIO_ITEM_ID) == null)
				{
					conn.Insert(r);
				}
				else if (FindItemById(r.ES_ESTOQUE_ROMANEIO_ITEM_ID) != null)
				{
					conn.Update(r);
				}

				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public Romaneio GetLastDate()
		{
			var conn = Database.GetConnection();
			return conn.Table<Romaneio>().OrderBy(r => r.DTHULTAT).ToList().LastOrDefault();
		}
		public long? getLastId()
		{
			try
			{
				var conn = Database.GetConnection();
				return conn.Table<RomaneioItem>().Max(p => p.ES_ESTOQUE_ROMANEIO_ITEM_ID);
			}
			catch
			{
				return null;
			}
		}

		public bool ComSocket(string request, string host, int port)
		{
			bool aux = false;
			var conn = Database.GetConnection();

			Thread t = new Thread(() =>
			{

				TcpClient client = null;
				NetworkStream netStream = null;

				try
				{
					client = new TcpClient();
					client.Connect(host, port);

					netStream = client.GetStream();

					byte[] msg = request.ToUTF8(true);
					netStream.Write(msg, 0, msg.Length);

					Empresa empresa = new EmpresaController().GetEmpresa();

					bool isAtt = false;
					bool firstLoop = true;
					bool loop = true;
					while (loop)
						if (netStream.CanRead)
						{
							byte[] bytes = new byte[client.ReceiveBufferSize];
							netStream.Read(bytes, 0, bytes.Length);
							string receiveMsg = bytes.UTF7ToString();

							if (receiveMsg.Contains("\0\0"))
								receiveMsg = receiveMsg.Split("\0\0")[0];

							receiveMsg = receiveMsg.Replace("CARGAROMANEIO@@", "");

							string[] lines = receiveMsg.Split("@@");

							if (lines.Length > 1)
							{
								string[] data = lines[0].Split(';');

								if (firstLoop)
								{
									Romaneio oldRomaneio = FindLast();
									if (oldRomaneio != null)
									{
										if (oldRomaneio.NROROMAN != data[1].ToLong())
										{
											Database.GetConnection().DropTable<Romaneio>();
											Database.GetConnection().CreateTable<Romaneio>();
											Database.GetConnection().DropTable<RomaneioItem>();
											Database.GetConnection().CreateTable<RomaneioItem>();
										}
										else
											isAtt = true;
									}
									firstLoop = false;
								}

								Romaneio r = new Romaneio()
								{
									ES_ESTOQUE_ROMANEIO_ID = data[0].ToLong(),
									NROROMAN = data[1].ToLong(),
									DATEMISS = DateTime.Parse(data[2]),
									SITROMAN = data[3].ToShort(),
									NROKMINI = data[4].ToDouble(),
									NROKMFIN = data[5].ToDouble(),
									IDTPLACA = data[6],
									DSCOBSER = data[7],
									DTHULTAT = DateTime.Parse(data[8]),
									USRULTAT = data[9]
								};

								this.Save(r);

								string[] itens = lines[1].Split("#");
								itens.ToList().ForEach((str) =>
								{
									string[] item = str.Split(';');
									var prod = new ProdutoDAO().FindById(item[2]);

									if (prod != null)
									{
										RomaneioItem i = null;
										if (!isAtt)
										{
											i = new RomaneioItem()
											{
												ES_ESTOQUE_ROMANEIO_ITEM_ID = item[0].ToLong(),
												ES_ESTOQUE_ROMANEIO_ID = item[1].ToLong(),
												CG_PRODUTO_ID = item[2].ToLong(),
												DSCRPROD = prod.DSCPROD,
												QTDPROD = item[3].ToDouble(),
												VLRCUSTO = item[4].ToDouble(),
												PRCVENDA = item[5].ToDouble(),
												QTDDEVCL = item[6].ToDouble(),
												QTDBRINDE = item[7].ToDouble(),
												QTDVENDA = item[8].ToDouble(),
												QTDCONT = item[9].ToDouble(),
												DTHULTAT = DateTime.Parse(item[10]),
												USRULTAT = item[11]
											};
										}
										else
										{
											if ((i = this.FindByIdItem(prod.CG_PRODUTO_ID.Value)) != null)
											{
												i.QTDPROD = item[3].ToDouble();
												i.VLRCUSTO = item[4].ToDouble();
												i.DTHULTAT = DateTime.Parse(item[10]);
												i.USRULTAT = item[11];
											}
											else
												i = new RomaneioItem()
												{
													ES_ESTOQUE_ROMANEIO_ITEM_ID = item[0].ToLong(),
													ES_ESTOQUE_ROMANEIO_ID = item[1].ToLong(),
													CG_PRODUTO_ID = item[2].ToLong(),
													DSCRPROD = prod.DSCPROD,
													QTDPROD = item[3].ToDouble(),
													VLRCUSTO = item[4].ToDouble(),
													PRCVENDA = item[5].ToDouble(),
													QTDDEVCL = item[6].ToDouble(),
													QTDBRINDE = item[7].ToDouble(),
													QTDVENDA = item[8].ToDouble(),
													QTDCONT = item[9].ToDouble(),
													DTHULTAT = DateTime.Parse(item[10]),
													USRULTAT = item[11]
												};
										}
										SaveItem(i);
									}
								});

								if (lines[2].Contains("erro"))
								{
									loop = false;

								}
								else if (lines[2].Contains("FIMROMANEIO"))
								{
									loop = false;
								}
								else
								{
									VendedorController vendedorC = new VendedorController();
									Vendedor vendedor = vendedorC.Vendedor;
									string id = vendedor.CG_VENDEDOR_ID.Value.ToString("D4");
									string str = lines[2];
									if (str.Contains("\0\0"))
										str = str.Split("\0\0")[0];

									string msg1 = $"CARGAROMANEIO{empresa.CODEMPRE}{id}{str}";
									byte[] bytesMsg = msg1.ToUTF8(true);
									netStream.Write(bytesMsg, 0, bytesMsg.Length);
								}
							}
							else
								loop = false;
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
	}
}