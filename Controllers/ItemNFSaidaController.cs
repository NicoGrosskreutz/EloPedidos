using Android.App;
using Android.Content;
using EloPedidos.Models;
using EloPedidos.Persistence;
using System;
using System.Collections.Generic;

namespace EloPedidos.Controllers
{
    public class ItemNFSaidaController
    {
        private Context Context;
        private ItemNFSaidaDAO DAO;
        
        public ItemNFSaidaController()
        {
            this.Context = Application.Context;
            this.DAO = new ItemNFSaidaDAO();
        }

        public bool Save(ItemNFSaida item)
        {
            if (item.FT_NFSAIDA_ID == null)
                return false;
            else
                return DAO.Save(item);
        }

        public bool Delete(long pFT_ITEM_NFSAIDA_ID) => DAO.Delete(pFT_ITEM_NFSAIDA_ID);

        public long? GetLastId() 
            => DAO.GetLastId();

        public ItemNFSaida FindById(long? id) => DAO.FindById(id);

        /// <summary>
        ///  Busca todos pelo ID da NF
        /// </summary>
        /// <param name="pFT_NFSAIDA_ID"></param>
        /// <returns></returns>
        public List<ItemNFSaida> FindAllByFT_NFSAIDA_ID(long pFT_NFSAIDA_ID)
            => (List<ItemNFSaida>)DAO.FindAllByFT_NFSAIDA_ID(pFT_NFSAIDA_ID);

        /// <summary>
        ///  Busca todos pelo Nº da NF
        /// </summary>
        /// <param name="pNRONF"></param>
        /// <returns></returns>
        public List<ItemNFSaida> FindAllByNRONF(int pNRONF)
            => (List<ItemNFSaida>)DAO.FindAllByNRONF(pNRONF);

        /// <summary>
        ///  Deleta todos os itens de uma NF
        /// </summary>
        /// <param name="pFT_NFSAIDA_ID"></param>
        /// <returns></returns>
        public bool DeleteAll(long pFT_NFSAIDA_ID)
        {
            try
            {
                var list = FindAllByFT_NFSAIDA_ID(pFT_NFSAIDA_ID);

                if (list != null)
                    list.ForEach((aux) =>
                    {
                        if (!Delete(aux.FT_ITEM_NFSAIDA_ID.Value))
                            throw new Exception("Erro ao excluir item!");
                    });

                return true;
            }
            catch 
            {
                return false;
            }
        }
    }
}