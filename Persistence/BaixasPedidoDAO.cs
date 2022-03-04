using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Util;
using EloPedidos.Models;
using SQLite;

namespace EloPedidos.Persistence
{
    public class BaixasPedidoDAO : ICrud<BaixasPedido>
    {
        private SQLiteConnection conn = null;

        private TableQuery<BaixasPedido> Table => conn.Table<BaixasPedido>();

        public long Count => conn.Table<BaixasPedido>().LongCount();

        public BaixasPedidoDAO()
        {
            Database.GetConnection().CreateTable<BaixasPedido>();
            conn = Database.GetConnection();
        }

        public bool Delete(object id)
        {
            return conn.Delete<BaixasPedido>(id) > 0;
        }
        public bool DeletePagamento(object id)
        {
            return conn.Delete<Pagamento>(id) > 0;
        }
        public List<Pagamento> FindNotSinc()
        {
            return conn.Table<Pagamento>().Where(p => !p.INDSYNC).ToList();
        }
        public List<BaixasPedido> FindAll()
        {
            return Table.ToList();
        }
        

        public BaixasPedido FindById(object id)
        {
            return conn.Find<BaixasPedido>(id);
        }

        public bool Save(BaixasPedido t)
        {
            if (t.FT_PEDIDO_BAIXA_ID == null)
                t.FT_PEDIDO_BAIXA_ID = LastId().HasValue ? LastId() + 1 : 1;

            if (FindById(t.FT_PEDIDO_BAIXA_ID) == null)
                conn.Insert(t);
            else
                conn.Update(t);

            return true;
        }

        public List<BaixasPedido> FindByCG_PESSOA_ID(long CG_PESSOA_ID)
		{
            try
            {
                return conn.Table<BaixasPedido>().Where(b => b.CG_PESSOA_ID == CG_PESSOA_ID).ToList();
            }
            catch (Exception e)
            {
                Log.Error("Baixa", e.ToString());
                return null;
            }
        }
        public long? LastId()
        {
            try
            {
                var connSQL = Database.GetConnection();
                return connSQL.Table<BaixasPedido>().Max(b => b.FT_PEDIDO_BAIXA_ID);
			}
			catch(Exception e)
			{
                Log.Error("Baixa", e.ToString());
                return null;
            }
        }

        public BaixasPedido FindByFT_PEDIDO_ID(long pFT_PEDIDO_ID)
        {
            return Table.Where(b => b.FT_PEDIDO_ID == pFT_PEDIDO_ID).FirstOrDefault();
        }

        public BaixasPedido FindByNROPEDID(long pNROPEDID)
        {
            try
            {
                var pedido = new PedidoDAO().FindByNROPEDID(pNROPEDID);
                return this.FindByFT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);
            }
            catch (Exception ex)
            {
                Log.Error("Baixa", ex.ToString());
                return null;
            }
        }

        public List<BaixasPedido> FindByNROPED(long pNROPEDID)
        {
            var conn = Database.GetConnection();
            var pedido = new PedidoDAO().FindByNROPEDID(pNROPEDID);
            try
            {
                var baixas = Table
                    .Where(i => i.FT_PEDIDO_ID == pedido.FT_PEDIDO_ID)
                    .ToList();
                return baixas;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public List<BaixasPedido> FindByDataRET(DateTime dataRET)
		{
            var conn = Database.GetConnection();
            List<BaixasPedido> baixas = new List<BaixasPedido>();
			try
			{
                baixas = this.FindAll().Where(p => DateTime.Parse(p.DATVCTO.ToString("dd/MM/yyyy")) == DateTime.Parse(dataRET.ToString("dd/MM/yyyy"))).ToList();
                //baixas = this.FindAll().Where(p => p.DATVCTO == dataRET).ToList();
                return baixas;
			}
			catch(Exception e)
			{
                return baixas;
			}
        }

        public List<BaixasPedido> FindAllNotSync() => Table.Where(b => !b.INDSINC).ToList();
    }
}