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
    public class EmpresaDAO : ICrud<Empresa>
    {
        public EmpresaDAO()
        {
            Database.GetConnection().CreateTable<Empresa>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Empresa>(id) > 0)
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

        public List<Empresa> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Empresa>().OrderBy(em => em.NOMFANTA).ToList();
            }
            catch(Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Empresa FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Empresa>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Save(Empresa t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (null == FindById(t.CODEMPRE))
                    conn.Insert(t);
                else
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

        public Empresa GetEmpresa()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Empresa>().FirstOrDefault();
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