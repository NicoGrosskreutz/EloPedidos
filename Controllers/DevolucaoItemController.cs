using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Util;
using Android.Widget;
using EloPedidos.Adapter;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;

namespace EloPedidos.Controllers
{
    public class DevolucaoItemController
    {
        private DevolucaoItemDAO DAO;

        public DevolucaoItemController()
        {
            DAO = new DevolucaoItemDAO();
        }

        public bool SaveItemDevolucao(DevolucaoItem dev)
        {
            return DAO.Save(dev);
        }

        public bool SaveSync(DevolucaoItem dev)
        {
            dev.INDSINC = true;
            return SaveItemDevolucao(dev);
        }

        public bool Delete(long pFT_PEDIDO_ITEM_DEVOLUCAO_ID)
            => DAO.Delete(pFT_PEDIDO_ITEM_DEVOLUCAO_ID);

        public bool Delete(DevolucaoItem devol)
            => DAO.Delete(devol);

        public DevolucaoItem FindById(long pFT_PEDIDO_ITEM_ID)
            => DAO.FindById(pFT_PEDIDO_ITEM_ID);

        public double GetDevolutionValue(long pNROPEDID)
        {
            try
            {
                var pedido = new PedidoController().FindByNROPEDID(pNROPEDID);
                var itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);
                double totalDevolucao = 0;

                itens.ForEach(i => totalDevolucao += (i.VLRUNIT * (i.QTDPROD - i.QTDATPRO)));

                return totalDevolucao;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return 0;
            }
        }

        public List<DevolucaoItem> FindAll() => DAO.FindAll();

        public List<DevolucaoItem> FindNOTSYNC(long pFT_PEDIDO_ID) => DAO.FindNOTSYNC(pFT_PEDIDO_ID);
        public List<DevolucaoItem> FindDevolution(long? pFT_PEDIDO_ID)
        {
            try
            {
                return this.FindAll().Where(d => d.FT_PEDIDO_ID.Value == pFT_PEDIDO_ID.Value).ToList();
            }
            catch
            {
                return new List<DevolucaoItem>();

            }
        }

        public DevolucaoItem FindByFT_PEDIDO_ITEM_ID(long pFT_PEDIDO_ITEM_ID)
            => DAO.FindByFT_PEDIDO_ITEM_ID(pFT_PEDIDO_ITEM_ID);


        public DevolucaoItem FindByCGPRODUTO(long cG_PRODUTO_ID, long fT_PEDIDO_ID)
        {
            var conn = Database.GetConnection();
            return conn.Table<DevolucaoItem>().Where(i => i.FT_PEDIDO_ID == fT_PEDIDO_ID && i.CG_PRODUTO_ID == cG_PRODUTO_ID).FirstOrDefault();
        }

        public bool ValidarSaldoReceber(long nropedido, List<DevolucaoItensAdapterCls> devolLista)
        {
            bool result = true;
            double valorDevolvido = 0;

            Pedido pedido = new PedidoController().FindByNROPEDID(nropedido);
            if (pedido != null)
            {
                List<ItemPedido> itens = new ItemPedidoController().FindAllOrderItems(pedido.FT_PEDIDO_ID.Value);
                double auxReceber = new BaixasPedidoController().PrevisaoReceber(itens);
                foreach (ItemPedido i in itens)
                {
                    devolLista.Where(d => d.QTDDEVOLNOW != "0").ToList().ForEach(d =>
                    {
                        if (i.FT_PEDIDO_ITEM_ID.Value == d.FT_PEDIDO_ITEM_ID)
                            i.QTDATPRO = d.QTDPROD.ToDouble() - (d.QTDDEVOL.ToDouble() + d.QTDDEVOLNOW.ToDouble());
                    });
                    valorDevolvido += ((i.QTDPROD - i.QTDATPRO) * i.VLRUNIT);
                }

                BaixasPedido baixa = new BaixasPedidoController().GerarBaixa(pedido);
                if (baixa.TOTLPEDID == valorDevolvido && baixa.VLRPGMT == 0)
                    result = true;
                else
                    if (auxReceber.ToString().StartsWith("-"))
                        result = false;
            }
            else
                result = false;

            return result;
        }
    }
}