using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using EloPedidos.Models;

namespace EloPedidos.Persistence
{
    public class NFSaidaDAO : ICrud<NFSaida>
    {
        /// <summary>
        ///  Retorna o número de notas registradas no app
        /// </summary>
        public int Count
        {
            private set
            {
                Count = FindAll().Count;
            }
            get
            {
                return this.Count;
            }
        }

        public NFSaidaDAO()
        {
            Database.GetConnection().CreateTable<NFSaida>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<NFSaida>(id) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return false;
            }
        }

        public IList<NFSaida> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<NFSaida>().ToList();
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        public NFSaida FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<NFSaida>(id);
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        public NFSaida FindByNRONF(int pNRONF)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<NFSaida>().Where(nf => nf.NRONF == pNRONF).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        public bool Save(NFSaida t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.FT_NFSAIDA_ID == null)
                    t.FT_NFSAIDA_ID = GetLastId().HasValue ? GetLastId().Value + 1 : 1;

                if (FindById(t.FT_NFSAIDA_ID) == null)
                    conn.Insert(t);
                else
                    conn.Update(t);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return false;
            }
        }

        public long? GetLastId()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<NFSaida>().Max(nf => nf.FT_NFSAIDA_ID);
            }
            catch(Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Retorna o número atual da sequencia das notas fiscais
        /// </summary>
        /// <returns></returns>
        public int? GetNRONF()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<NFSaida>().Max(nf => nf.NRONF) + 1;
            }
            catch(Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Retorna todas as notas não transmitidas
        /// </summary>
        /// <returns></returns>
        public IList<NFSaida> FindAllNotSinc()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<NFSaida>().Where(nf => !nf.INDSINC).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }
    }
}