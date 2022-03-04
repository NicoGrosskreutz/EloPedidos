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
    public class VencimentoDAO : ICrud<Vencimento>
    {
        public VencimentoDAO()
        {
            Database.GetConnection().CreateTable<Vencimento>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<Vencimento>(id) > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }

        public List<Vencimento> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Vencimento>().ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public Vencimento FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<Vencimento>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }
        public List<Vencimento> GetLastDatetime()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Vencimento>().OrderBy(p => p.DTHULTAT).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public bool Save(Vencimento t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.CG_PESSOA_DIAS_VCTO_ID == null)
                    t.CG_PESSOA_DIAS_VCTO_ID = GetLastId() == null ? 1 : GetLastId() + 1;
                
                if (FindById(t.CG_PESSOA_DIAS_VCTO_ID) == null)
                    conn.Insert(t);
                else if (FindById(t.CG_PESSOA_DIAS_VCTO_ID) != null)
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
                return conn.Table<Vencimento>().Max(v => v.CG_PESSOA_DIAS_VCTO_ID);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Busca o vencimento por CG_PESSOA_ID vinculada ao registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Vencimento FindByCG_PESSOA_ID(long id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Vencimento>()
                    .Where(v => v.CG_PESSOA_ID == id).FirstOrDefault();
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