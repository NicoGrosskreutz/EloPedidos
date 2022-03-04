using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using EloPedidos.Models;
using SQLite;

namespace EloPedidos.Persistence
{
    public class DevolucaoItemDAO : ICrud<DevolucaoItem>
    {
        public SQLiteConnection conn { get; private set; } = null;
        public TableQuery<DevolucaoItem> Table { get; private set; } = null;

        public DevolucaoItemDAO()
        {
            Database.GetConnection().CreateTable<DevolucaoItem>();

            conn = Database.GetConnection();
            Table = conn.Table<DevolucaoItem>();
        }

        public bool Delete(object id)
        {
            try
            {
                return conn.Delete(id) > 0;
            } 
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return false;
            }
        }

        public bool Delete(DevolucaoItem item)
        {
            try
            {
                return conn.Delete(item) > 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return false;
            }
        }

        public List<DevolucaoItem> FindAll()
        {
            try
            {
                return Table.OrderBy(x => x.DATDEVOL).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return null;
            }
        }

        public DevolucaoItem FindById(object id)
        {
            try
            {
                return conn.Find<DevolucaoItem>(id);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return null;
            }
        }

        public DevolucaoItem FindByFT_PEDIDO_ITEM_ID(long pFT_PEDIDO_ITEM_ID)
        {
            try
            {
                return Table.Where(i => i.FT_PEDIDO_ITEM_ID == pFT_PEDIDO_ITEM_ID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return null;
            }
        }


        public bool Save(DevolucaoItem t)
        {
            try
            {
                if (t.FT_PEDIDO_ITEM_DEVOLUCAO_ID == null)
                    t.FT_PEDIDO_ITEM_DEVOLUCAO_ID = GetLastId().HasValue ? GetLastId() + 1 : 1;

                if (FindById(t.FT_PEDIDO_ITEM_DEVOLUCAO_ID) == null)
                    return conn.Insert(t) > 0;
                else
                    return conn.Update(t) > 0;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return false;
            }
        }

        public long? GetLastId()
        {
            try
            {
                return Table.Max(i => i.FT_PEDIDO_ITEM_DEVOLUCAO_ID);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return null;
            }
        }

        public List<DevolucaoItem> FindByFT_PEDIDO_ID(long pFT_PEDIDO_ID)
        {
            return Table.Where(i => i.FT_PEDIDO_ID == pFT_PEDIDO_ID).ToList();
        }
        public List<DevolucaoItem> FindNOTSYNC(long pFT_PEDIDO_ID)
        {
            return Table.Where(i => i.FT_PEDIDO_ID == pFT_PEDIDO_ID && !i.INDSINC).ToList();
        }
    }
}