using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using SQLite;
using M = EloPedidos.Models;

namespace EloPedidos.Persistence
{
    public class ConfigDAO : ICrud<M.Config>
    {
        public ConfigDAO()
        {
            try
            {
                Database.GetConnection().CreateTable<M.Config>();
            }
            catch(Exception e)
			{
                Log.Error("", e.ToString());
			}
        }

        public bool Save(M.Config c)
        {
            SQLiteConnection conn = Database.GetConnection();
            try
            {
                if (FindAll().Count > 0)
                    c.CONFIG_ID = GetConfig().CONFIG_ID;

                if (c.CONFIG_ID == null)
                    conn.Insert(c);
                else
                    conn.Update(c);

                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }
        public bool Update(long id, int COD, string name)
        {
            var conn = Database.GetConnection();
            try
            {
                M.Config c = conn.Find<M.Config>(id);
                c.CODMUNPQ = COD;
                c.NOMMUNPQ = name;
                conn.Update(c);
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }
       

        public M.Config FindById(object id)
        {
            SQLiteConnection conn = Database.GetConnection();
            try
            {
                return conn.Find<M.Config>(id);
            }
            catch(Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Delete(object id)
        {
            SQLiteConnection conn = Database.GetConnection();
            try
            {
                if (conn.Delete<M.Config>(id) > 0)
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

        public List<M.Config> FindAll()
        {
            SQLiteConnection conn = Database.GetConnection();
            try
            {
                return conn.Table<M.Config>().ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public M.Config GetConfig()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<M.Config>()
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

    }
}