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
    public class VendedorDAO : ICrud<Vendedor>
    {
        public VendedorDAO()
        {
            Database.GetConnection().CreateTable<Vendedor>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Vendedor>(id) > 0)
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

        public List<Vendedor> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Vendedor>().OrderBy(v => v.NOMVEND).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Vendedor FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Vendedor>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Save(Vendedor t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.CG_VENDEDOR_ID == null)
                    t.CG_VENDEDOR_ID = GetLastId() == null ? 1 : GetLastId() + 1;

                if (FindById(t.CG_VENDEDOR_ID) == null)
                    conn.Insert(t);
                else if (FindById(t.CG_VENDEDOR_ID) != null)
                    conn.Update(t);

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
                return conn.Table<Vendedor>().Max(v => v.CG_VENDEDOR_ID);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Vendedor GetVendedor(Operador o)
        {
            var conn = Database.GetConnection();
            try
            {
                Vendedor vendedor = conn.Table<Vendedor>().Where(v => v.USROPER.ToLower() == o.USROPER.ToLower()).FirstOrDefault();
                return vendedor;
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