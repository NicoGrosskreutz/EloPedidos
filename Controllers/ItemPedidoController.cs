using System.Collections.Generic;
using System.Linq;
using Android.Content;
using EloPedidos.Models;
using EloPedidos.Persistence;

namespace EloPedidos.Controllers
{
    public class ItemPedidoController
    {
        private ItemPedidoDAO DAO;

        public ItemPedidoController()
        {
            DAO = new ItemPedidoDAO();
        }

        public List<ItemPedido> FindAll()
		{
            var conn = Database.GetConnection();
            return conn.Table<ItemPedido>().Where(i => i.INDBRIND != false).ToList();
		}
        public bool Save(ItemPedido itemPedido)
        {
            return DAO.Save(itemPedido);
        }

        public double GetTotalValue(ItemPedido itemPedido)
        {
            return ((itemPedido.QTDPROD * itemPedido.VLRUNIT) - itemPedido.VLRDSCTO);
        }

        public bool Delete(long pFT_PEDIDO_ITEM_ID)
        {
            return DAO.Delete(pFT_PEDIDO_ITEM_ID);
        }
        public bool Delete(ItemPedido item)
        {
            return DAO.Delete(item);
        }

        public List<ItemPedido> FindItemsBy_FT_PEDIDO_ID(long pFT_PEDIDO_ID)
        {
            return DAO.FindItemsByFT_PEDIDO_ID(pFT_PEDIDO_ID);
        }

        /// <summary>
        ///  Retorna todos os itens do pedido referente ao seu id
        /// </summary>
        public List<ItemPedido> FindAllOrderItems(long pFT_PEDIDO_ID)
        {
            return DAO.FindItemsByFT_PEDIDO_ID(pFT_PEDIDO_ID);
        }

        public ItemPedido FindById(long pFT_PEDIDO_ITEM_ID)
        {
            return DAO.FindById(pFT_PEDIDO_ITEM_ID);
        }

        public ItemPedido FindByPROD_IDePED_ID(long cG_PRODUTO_ID, long fT_PEDIDO_ID)
        {
            return DAO.FindByPROD_IDePED_ID(cG_PRODUTO_ID, fT_PEDIDO_ID);
        }

        public ItemPedido FindByCGPRODUTO(long cG_PRODUTO_ID)
        {
            var conn = Database.GetConnection();
            return conn.Table<ItemPedido>().Where(i => i.CG_PRODUTO_ID.Value == cG_PRODUTO_ID).FirstOrDefault();
        }

        /// <summary>
        ///  Retorna o valor total da soma dos itens de um pedido
        /// </summary>
        /// <returns></returns>
        public double GetTotalValue(long pFT_PEDIDO_ID)
        {
            List<ItemPedido> itens = DAO.FindItemsByFT_PEDIDO_ID(pFT_PEDIDO_ID);

            double total = 0;
            List<double> totalItem = new List<double>();

            itens.Where(i => !i.INDBRIND)
                .ToList().ForEach((i) => totalItem.Add((i.QTDPROD * i.VLRUNIT) - i.VLRDSCTO));

            totalItem.ForEach((t) => total += t);

            return total;
        }
    }
}