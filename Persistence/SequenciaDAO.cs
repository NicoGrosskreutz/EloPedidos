using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using EloPedidos.Models;

namespace EloPedidos.Persistence
{
    public class SequenciaDAO : ICrud<Sequencia>
    {
        public SequenciaDAO()
        {
            Database.GetConnection().CreateTable<Sequencia>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Sequencia>(id) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }

        public List<Sequencia> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Sequencia>().ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Sequencia FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Sequencia>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        
        public bool Save(Sequencia t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.CG_VENDEDOR_SEQ_PEDIDO_ID == null)
                    t.CG_VENDEDOR_SEQ_PEDIDO_ID = GetLastId() == null ? 1 : GetLastId() + 1;

                if (FindById(t.CG_VENDEDOR_SEQ_PEDIDO_ID) == null)
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

        public long? GetLastId()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Sequencia>().Max(s => s.CG_VENDEDOR_SEQ_PEDIDO_ID);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Sequencia GetSequencia()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Sequencia>()
                    .OrderByDescending(s => s.DTHULTAT).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public List<Sequencia> GetLastDateTime()
        {
            var conn = Database.GetConnection();
            try
            {
                List<Sequencia> sequencia = conn.Table<Sequencia>().OrderBy(m => m.DTHULTAT).ToList();
                return sequencia;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        /// <summary>
        ///  Atualiza o numero do pedido na tabela.
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderNumber()
        {
            var conn = Database.GetConnection();
            try
            {
                var s = GetSequencia();
                if (s != null)
                {
                    long? nroatual = conn.Table<Pedido>().Max(p => p.NROPEDID);

                    s.NROPEDAT = nroatual.Value;
                    return Save(s);
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }

    }
}