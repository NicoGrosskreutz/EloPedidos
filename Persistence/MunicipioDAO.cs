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
using EloPedidos.Models;

namespace EloPedidos.Persistence
{
    public class MunicipioDAO : ICrud<Municipio>
    {
        public MunicipioDAO()
        {
            Database.GetConnection().CreateTable<Municipio>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Municipio>(id) > 0)
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

        public List<Municipio> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Municipio>().OrderBy(m => m.NOMMUNIC).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Municipio> GetLastDateTime()
        {
            var conn = Database.GetConnection();
            try
            {
                List<Municipio> municipio = conn.Table<Municipio>().OrderBy(m => m.DTHULTAT).ToList();
                return municipio;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Municipio FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Municipio>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Save(Municipio t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.CODMUNIC == null)
                    t.CODMUNIC = GetLastCODMUNIC() == null ? 1 : GetLastCODMUNIC() + 1;

                if (FindById(t.CODMUNIC) == null)
                {
                    conn.Insert(t);
                    return true;
                }
                else if (FindById(t.CODMUNIC) != null)
                {
                    conn.Update(t);
                    return true;
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

        public long? GetLastCODMUNIC()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Municipio>().Max(m => m.CODMUNIC);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public string FindNameById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Municipio>(id).NOMMUNIC;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public IList<Municipio> FindByName(string name)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Municipio>().ToList()
                    .Where(m => Utils.Format.RemoveAccents(m.NOMMUNIC).ToLower().Contains(Utils.Format.RemoveAccents(name).ToLower()))
                    .ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public Municipio FindByNOMMUNIC(string name)
        {
            var conn = Database.GetConnection();
            Municipio municipio = conn.Table<Municipio>().ToList()
            .Where(m => m.NOMMUNIC.ToLower().StartsWith(name.ToLower())).FirstOrDefault();
            return municipio;
            
        }

    }
}