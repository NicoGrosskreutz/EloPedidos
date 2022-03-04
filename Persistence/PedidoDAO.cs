using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Utils;
using static EloPedidos.Models.Pedido;

namespace EloPedidos.Persistence
{
	public class PedidoDAO : ICrud<Pedido>
	{
		public PedidoDAO()
		{
			Database.GetConnection().CreateTable<Pedido>();
		}

		public bool Delete(object id)
		{
			var conn = Database.GetConnection();
			try
			{
				return (conn.Delete<Pedido>(id) > 0);
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return false;
			}
		}

		public List<Pedido> FindAll()
		{
			var conn = Database.GetConnection();
			try
			{
				var pedidos = conn.Table<Pedido>().ToList();
				pedidos.ForEach(aux => aux.Pessoa = new PessoaController().FindById(aux.ID_PESSOA.Value));
				return pedidos;
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return new List<Pedido>();
			}
		}

		public List<Pedido> FindOpenOrderByCG_PESSOA_ID(long pCG_PESSOA_ID)
		{
			//return Database.GetConnection().Table<Pedido>().Where(p => p.CG_PESSOA_ID == pCG_PESSOA_ID &&
			//		p.SITPEDID != 5 && p.SITPEDID != 3 &&
			//		!p.DSCPRZPG.Equals("0"))
			//		.OrderBy(p => p.DATEMISS).ToList();

			return Database.GetConnection().Table<Pedido>().Where(p => p.CG_PESSOA_ID == pCG_PESSOA_ID &&
					p.SITPEDID != (short)Pedido.SitPedido.Atendido && p.SITPEDID != (short)Pedido.SitPedido.Cancelado &&
					!p.DSCPRZPG.Equals("0"))
					.OrderBy(p => p.DATEMISS).ToList();
		}

		public List<Pedido> FindAll(bool cancelado, string municipio, DateTime dataI, DateTime dataF)
		{
			try
			{
				return FindAll().Where(p => p.INDCANC == cancelado &&
									   (p.CODMUNIC == municipio || municipio == "") &&
									   p.DATEMISS >= dataI &&
									   p.DATEMISS <= dataF)
									   .ToList();
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return null;
			}
		}

		public List<Pedido> FindAllCanceled()
		{
			var conn = Database.GetConnection();
			return conn.Table<Pedido>().Where(p => p.INDCANC == true).ToList();
		}
		public List<Pedido> FindAll(bool cancelado, string municipio)
		{
			if (municipio != "")
				return FindAll().Where(p => p.INDCANC == cancelado &&
										   (p.CODMUNIC == municipio)).ToList();
			else
				return FindAll().Where(p => p.INDCANC == cancelado).ToList();
		}
		public List<Pedido> FindAll(bool cancelado)
		{
			return FindAll().Where(p => p.INDCANC == cancelado).ToList();

		}
		public List<Pedido> FindByName(string name)
		{
			var conn = Database.GetConnection();
			try
			{
				return FindAll().Where(p => p.Pessoa.NOMFANTA.ToLower().Contains(name.ToLower())).ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public List<Pedido> FindByName(string name, bool cancelado, string municipio)
		{
			return FindByName(name).Where(p => p.INDCANC == cancelado &&
												(p.CODMUNIC == municipio || municipio == "")).ToList();
		}
		public List<Pedido> FindByName(string name, bool cancelado)
		{
			return FindByName(name).Where(p => p.INDCANC == cancelado).ToList();
		}


		public List<Pedido> FindByName(string name, bool cancelado, string municipio, DateTime dataI, DateTime dataF)
		{
			return FindByName(name).Where(p => p.INDCANC == cancelado &&
										(p.CODMUNIC == municipio || municipio == "") &&
									   p.DATEMISS >= dataI &&
									   p.DATEMISS <= dataF)
									   .ToList();
		}

		public Pedido FindById(object id)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Find<Pedido>(id);
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public bool Save(Pedido t)
		{
			var conn = Database.GetConnection();
			try
			{
				if (FindById(t.FT_PEDIDO_ID) == null)
					t.FT_PEDIDO_ID = GetLastId() == null ? 1 : GetLastId() + 1;

				if (FindById(t.FT_PEDIDO_ID) == null)
				{
					conn.Insert(t);
					return true;
				}
				else
				{
					conn.Update(t);
					return true;
				}

			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return false;
			}
		}

		public bool Update(Pedido p)
		{
			try
			{
				var pedido = FindByNROPEDID(p.NROPEDID);
				if (pedido != null)
				{
					p.FT_PEDIDO_ID = pedido.FT_PEDIDO_ID;
					return this.Save(p);
				}
				else
					return this.Save(p);
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return false;
			}
		}

		public Pedido getLastOpenOrder(long id)
		{
			var conn = Database.GetConnection();
			try
			{
				DateTime dateNow = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
				//var pedidos = conn.Table<Pedido>().Where(p => p.CG_PESSOA_ID == id).OrderByDescending(p => p.DATEMISS).ToList();
				//var pedidos = conn.Table<Pedido>().Where(p => p.CG_PESSOA_ID == id && 
				//											  p.SITPEDID != 1 &&
				//											  p.SITPEDID != 3 &&
				//											  p.SITPEDID != 5 &&
				//											  p.DATEMISS != dateNow)
				//											.OrderByDescending(p => p.DATEMISS).ToList();

				var pedidos = conn.Table<Pedido>().Where(p => p.CG_PESSOA_ID == id &&
															  p.SITPEDID != (short)Pedido.SitPedido.Aberto &&
															  p.SITPEDID != (short)Pedido.SitPedido.Atendido &&
															  p.SITPEDID != (short)Pedido.SitPedido.Cancelado &&
															  p.DATEMISS != dateNow)
															.OrderByDescending(p => p.DATEMISS).ToList();


				Pedido lastPedido = null;
				if (pedidos.Count > 0)
				{
					foreach(Pedido p in pedidos)
					{
						var baixa = new BaixasPedidoController().GerarBaixa(p);
						if (baixa.VLRRECBR > 0)
						{
							int totalDays = int.Parse(dateNow.Subtract(p.DATEMISS).TotalDays.ToString());
							if (totalDays > 5)
							{
								lastPedido = p;
								break;
							}
						}
					}
					return lastPedido;
				}
				else
					return null;
			}
			catch (Exception ex)
			{
				string error = "Elo_Log";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public long? GetLastId()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>().Max(p => p.FT_PEDIDO_ID);
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public bool SetConfirmed(long pFT_PEDIDO_ID)
		{
			var conn = Database.GetConnection();
			try
			{
				var pedido = FindById(pFT_PEDIDO_ID);
				pedido.SITPEDID = (short)SitPedido.Confirmado;
				return this.Save(pedido);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return false;
			}
		}

		public bool SetAtendidoParcial(long pFT_PEDIDO_ID)
		{
			var conn = Database.GetConnection();
			try
			{
				var pedido = FindById(pFT_PEDIDO_ID);
				pedido.SITPEDID = (short)SitPedido.ParcialTotal;
				return this.Save(pedido);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return false;
			}
		}

		public bool SetAnswered(long pFT_PEDIDO_ID)
		{
			var conn = Database.GetConnection();
			try
			{
				var pedido = FindById(pFT_PEDIDO_ID);
				pedido.SITPEDID = (short)SitPedido.Atendido;
				return this.Save(pedido);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return false;
			}
		}

		public bool SetParcial(long pFT_PEDIDO_ID)
		{
			var conn = Database.GetConnection();
			try
			{
				var pedido = FindById(pFT_PEDIDO_ID);
				pedido.SITPEDID = (short)SitPedido.ParcialTotal;
				return this.Save(pedido);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return false;
			}
		}

		public Pedido FindByNROPEDID(long NROPEDID)
		{
			var conn = Database.GetConnection();
			try
			{
				var pedido = conn.Table<Pedido>()
					.Where(p => p.NROPEDID == NROPEDID)
					.FirstOrDefault();
				return pedido;
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null; // erro
			}
		}

		/// <summary>
		///  Busca todos os pedidos por cliente
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public List<Pedido> FindByCG_PESSOA_ID(long id)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>()
					.Where(p => p.CG_PESSOA_ID == id)
					.ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}
		public List<Pedido> FindBy_ID_PESSOA(long id)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>()
					.Where(p => p.ID_PESSOA == id)
					.ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public bool CancelarPedidoPorN(long numero, string MTVOCANCEL, string message, out Pedido pedido)
		{
			var conn = Database.GetConnection();
			try
			{
				Pedido p = conn.Table<Pedido>()
					.Where(pd => pd.NROPEDID == numero)
					.FirstOrDefault();

				if (p == null)
					throw new Exception("Número de pedido inválido");

				p.INDCANC = true;
				p.SITPEDID = (short)SitPedido.Cancelado;
				p.MSGPEDID = message;
				p.DSCMOTCA = MTVOCANCEL;
				p.DTHULTAT = DateTime.Now;
				p.USRULTAT = new OperadorController().Operador.USROPER;
				p.SYNCCANC = false;

				pedido = p;
				return this.Save(p);
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				pedido = null;
				return false;
			}
		}

		public bool SetSync(long id)
		{
			var conn = Database.GetConnection();
			try
			{
				Pedido p = conn.Find<Pedido>(id);
				p.INDSINC = true;
				return this.Save(p);
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return false;
			}
		}

		public List<Pedido> FindAllNotSync()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>()
					.Where(p => !p.INDSINC)
					.OrderBy(p => p.NROPEDID)
					.ToList();
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return null;
			}
		}

		public int Count()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>().Count();
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return 0;
			}
		}

		public int NotSyncCount()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>().Where(p => !p.INDSINC).Count();
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return 0;
			}
		}

		public List<Pedido> CanceledNotSync()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>().Where(p => p.INDCANC && !p.SYNCCANC).ToList();
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return new List<Pedido>();
			}
		}

		public int NotCanceledCount()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Pedido>().Where(p => !p.INDCANC).Count();
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return 0;
			}
		}
	}
}