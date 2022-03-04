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
using EloPedidos.Controllers;
using EloPedidos.Models;

namespace EloPedidos.Persistence
{
    public class ItemPedidoDAO : ICrud<ItemPedido>
    {
        public ItemPedidoDAO()
        {
            Database.GetConnection().CreateTable<ItemPedido>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection(); 
            try
            {
                if (conn.Delete<ItemPedido>(id) > 0)
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

        public List<ItemPedido> FindAll()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<ItemPedido>().ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public ItemPedido FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<ItemPedido>(id);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public ItemPedido FindByPROD_IDePED_ID(long cG_PRODUTO_ID, long fT_PEDIDO_ID)
        {
            var conn = Database.GetConnection();
            List<ItemPedido> item = conn.Table<ItemPedido>().Where(i => i.FT_PEDIDO_ID == fT_PEDIDO_ID).ToList();
            return item.Where(i => i.CG_PRODUTO_ID == cG_PRODUTO_ID).FirstOrDefault();
        }

        public bool Save(ItemPedido t)
        {
            var conn = Database.GetConnection();
            try
            {
                if (t.FT_PEDIDO_ITEM_ID == null)
                    t.FT_PEDIDO_ITEM_ID = GetLastId() == null ? 1 : GetLastId() + 1;

                if (FindById(t.FT_PEDIDO_ITEM_ID) == null)
                    conn.Insert(t);
                else if (FindById(t.FT_PEDIDO_ITEM_ID) != null)
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
                return conn.Table<ItemPedido>()
                    .Max(i => i.FT_PEDIDO_ITEM_ID);
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }

        public List<ItemPedido> FindItemsByFT_PEDIDO_ID(long id)
        {
            var conn = Database.GetConnection();
            try
            {
                var items = conn.Table<ItemPedido>()
                    .Where(i => i.FT_PEDIDO_ID == id)
                    .ToList();

                items.ForEach((aux) => aux.Produto = new ProdutoController().FindById(aux.CG_PRODUTO_ID.Value));

                return items;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
        }


        /// <summary>
        ///  Exclui todos os items de todos os pedidos
        /// </summary>
        /// <returns></returns>
        public bool DeleteAll()
        {
            var conn = Database.GetConnection();
            try
            {
                conn.Table<ItemPedido>().ToList().ForEach((aux) => {
                    this.Delete(aux.FT_PEDIDO_ITEM_ID);
                });

                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return false;
            }
        }
    }
}