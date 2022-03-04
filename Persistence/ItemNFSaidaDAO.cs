using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using EloPedidos.Models;

namespace EloPedidos.Persistence
{
    public class ItemNFSaidaDAO
    {
        public ItemNFSaidaDAO()
        {
            Database.GetConnection().CreateTable<ItemNFSaida>();
        }

        public bool Delete(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                if (conn.Delete<ItemNFSaida>(id) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return false;
            }
        }

        public ItemNFSaida FindById(object id)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Find<ItemNFSaida>(id);
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        public bool Save(ItemNFSaida i)
        {
            var conn = Database.GetConnection();
            try
            {
                if (i.FT_ITEM_NFSAIDA_ID == null)
                    i.FT_ITEM_NFSAIDA_ID = GetLastId() == null ? 1 : GetLastId() + 1;

                if (FindById(i.FT_ITEM_NFSAIDA_ID) == null)
                    conn.Insert(i);
                else
                    conn.Update(i);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return false;
            }
        }

        public long? GetLastId()
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<ItemNFSaida>().Max(i => i.FT_ITEM_NFSAIDA_ID);
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Busca todos os items relacionados a NF de Nº da consulta 
        /// </summary>
        /// <param name="pNRONF"></param>
        /// <returns></returns>
        public IList<ItemNFSaida> FindAllByNRONF(int pNRONF)
        {
            var conn = Database.GetConnection();
            try
            {
                var nf = new Controllers.NFSaidaController().FindByNRONF(pNRONF);
                return conn.Table<ItemNFSaida>().Where(i => i.FT_NFSAIDA_ID == nf.FT_NFSAIDA_ID).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }

        /// <summary>
        ///  Busca todos os items relacionados a NF a partir do id na mesma
        /// </summary>
        /// <param name="pFT_NFSAIDA_ID"></param>
        /// <returns></returns>
        public IList<ItemNFSaida> FindAllByFT_NFSAIDA_ID(long pFT_NFSAIDA_ID)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<ItemNFSaida>()
                    .Where(i => i.FT_NFSAIDA_ID == pFT_NFSAIDA_ID).ToList();
            }
            catch (Exception ex)
            {
                Log.Error("error", ex.ToString());
                return null;
            }
        }
    }
}