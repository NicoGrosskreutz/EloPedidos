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
    public class OperadorDAO : ICrud<Operador>
    {
        public OperadorDAO()
        {
            Database.GetConnection().CreateTable<Operador>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Operador>(id) > 0)
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

        public List<Operador> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Operador>().OrderBy(o => o.NOMOPER).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Operador FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Operador>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Save(Operador t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (null == FindById(t.USROPER))
                    conn.Insert(t);
                else
                    conn.Update(t);

                return true;
            }
            catch(Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }

        public Operador GetOperador()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Operador>()
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