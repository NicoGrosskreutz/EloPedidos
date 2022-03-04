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
    public class ProdutoDAO : ICrud<Produto>
    {
        public ProdutoDAO()
        {
            Database.GetConnection().CreateTable<Produto>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Produto>(id) > 0)
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

        public List<Produto> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Produto>()
                    .OrderBy(p => p.DSCPROD).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Produto> GetLastDateTime()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Produto>()
                    .OrderBy(p => p.DTHULTAT).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Produto FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Produto>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Save(Produto t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.CG_PRODUTO_ID == null)
                    t.CG_PRODUTO_ID = GetLastId() == null ? 1 : GetLastId() + 1;

                if (t.CODPROD == null)
                    t.CODPROD = GetLastId() == null ? 1 : GetLastId() + 1;

                if (t.CODPROD == 0)
                    t.CODPROD = GetLastId() == null ? 1 : GetLastId() + 1;

                if (t.CG_PRODUTO_ID != null)
                    if (FindById(t.CG_PRODUTO_ID) == null)
                    {
                        conn.Insert(t);
                        return true;
                    }
                    else if (FindById(t.CG_PRODUTO_ID) != null)
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

        public long? GetLastId()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Produto>()
                    .Max(p => p.CG_PRODUTO_ID);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Produto FindByCODPROD(long? CODPROD)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Produto>().ToList()
                    .Where(p => p.CODPROD == CODPROD)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public List<Produto> FindByDSCPROD(string description)
        {
            var conn = Database.GetConnection();
            try
            {
                
                return conn.Table<Produto>().ToList()
                    .Where(p => p.DSCPROD.ToLower().Contains(description.ToLower()))
                    .ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Produto> FindPROD()
        {
            var conn = Database.GetConnection();
            try
            {
                List<RomaneioItem> r = new Controllers.RomaneioController().FindAll();

                return conn.Table<Produto>().ToList();
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