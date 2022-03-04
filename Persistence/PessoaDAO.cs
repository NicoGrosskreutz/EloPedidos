using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using EloPedidos.Models;
using SQLite;
using static EloPedidos.Models.Pedido;

namespace EloPedidos.Persistence
{
    public class PessoaDAO : ICrud<Pessoa>
    {
        private SQLiteConnection conn = null;

        public PessoaDAO()
        {
            conn = Database.GetConnection();
            conn.CreateTable<Pessoa>();
        }

        public bool Delete(object id)
        {
            try
            {
                if (conn.Delete<Pessoa>(id) > 0)
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

        public Pessoa FindByIDTPESS(string idtpess)
        {
            try
            {
                var conn = Database.GetConnection();
                return conn.Table<Pessoa>().Where(p => p.IDTPESS == idtpess).FirstOrDefault();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool DeleteByCODPESS(long pCODPESS)
        {
            try
            {
                Pessoa pessoa = conn.Table<Pessoa>()
                    .Where(p => p.CODPESS == pCODPESS)
                    .FirstOrDefault();

                if (conn.Delete<Pessoa>(pessoa.ID) > 0)
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

        public List<Pessoa> FindAll()
        {
            try
            {
                return conn.Table<Pessoa>().OrderBy(p => p.NOMPESS).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Pessoa> GetLastDateTime()
        {
            try
            {
                return conn.Table<Pessoa>().OrderBy(p => p.DTHULTAT).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Pessoa FindById(object id)
        {
            try
            {
                return conn.Find<Pessoa>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public List<Pessoa> FindByName(string name)
        {
            var conn = Database.GetConnection();
            try
            {
                return FindAll().Where(p => p.NOMPESS.ToLower().Contains(name.ToLower()) || p.NOMFANTA.ToLower().Contains(name.ToLower())).ToList();

            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Pessoa> FindByName(string name, string CODMUNIC)
        {
            var conn = Database.GetConnection();
            try
            {
                if (CODMUNIC != "")
                    return FindByName(name).Where(p => p.CODMUNIC == long.Parse(CODMUNIC)).ToList();
                else
                    return FindAll().Where(p => p.NOMPESS.ToLower().Contains(name.ToLower()) || p.NOMFANTA.ToLower().Contains(name.ToLower())).ToList();

            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Pessoa> FindAll(string CODMUNIC)
        {
            if (CODMUNIC != "")
                return FindAll().Where(p => p.CODMUNIC == int.Parse(CODMUNIC)).ToList();
            else
                return FindAll();

        }

        public bool Save(Pessoa p)
        {
            try
            {
                if (p.ID == null)
                {
                    conn.Insert(p);
                }
                else if (p.ID != null)
                {
                    if (p.ID > 0 && (FindById(p.ID) != null))
                    {
                        conn.Update(p);
                    }
                    else
                        conn.Insert(p);
                }

                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }

        public Pessoa FindByCODPESS(long? CODPESS)
        {
            try
            {
                return conn.Table<Pessoa>().ToList()
                    .Where(p => p.CODPESS == CODPESS)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Retorna o último registro salvo
        /// </summary>
        /// <returns></returns>
        public Pessoa GetLastRecord()
        {
            try
            {
                return conn.Table<Pessoa>()
                    .LastOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public List<Pessoa> FindAllWithOrder()
        {
            try
            {
                return conn.Query<Pessoa>("SELECT DISTINCT P.CG_PESSOA_ID, P.CODPESS, P.NOMPESS, P.NOMFANTA " +
                    "FROM CG_PESSOA AS P " +
                    "JOIN FT_PEDIDO PE " +
                    "ON P.CG_PESSOA_ID = PE.CG_PESSOA_ID " +
                    "WHERE (PE.SITPEDID <> 3 AND PE.SITPEDID <> 5) " +
                    "AND PE.DSCPRZPG <> '0' " +
                    "ORDER BY P.NOMPESS, P.NOMFANTA").ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return null;
            }
        }

        public List<Pessoa> FindWithOrdersByName(string name)
        {
            try
            {
                return conn.Query<Pessoa>("SELECT DISTINCT P.CG_PESSOA_ID, P.CODPESS, P.NOMPESS, P.NOMFANTA " +
                    "FROM CG_PESSOA AS P " +
                    "JOIN FT_PEDIDO PE " +
                    "ON P.CG_PESSOA_ID = PE.CG_PESSOA_ID " +
                    "WHERE PE.SITPEDID = 1 AND P.NOMFANTA = ? " +
                    "AND PE.DSCPRZPG <> '0' " +
                    "ORDER BY P.NOMPESS, P.NOMFANTA", $"%{name}%").ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return null;
            }
        }
    }
}