using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloPedidos.Utils
{
    public class Sincronizador
    {
        PedidoController pController = new PedidoController();
        DevolucaoItemController dController = new DevolucaoItemController();
        DevolucaoItemDAO devDAO = new DevolucaoItemDAO();
        BaixasPedidoDAO baiDAO = new BaixasPedidoDAO();
        ItemPedidoController iController = new ItemPedidoController();
        BaixasPedidoController bController = new BaixasPedidoController();
        PessoaController pessController = new PessoaController();

        public void SincronizarAllNotSync()
        {
            Task.Run(() =>
            {
                try
                {
                    ///CLIENTE
                    List<Pessoa> pessoas = new PessoaController().FindAll().Where(p => !p.INDSINC).ToList();
                    pessoas.ForEach(p => { pessController.SincPessoa(p, out string message); });
                }
                catch (Exception e)
                {
                    Log.Error(Ext.LOG_APP, e.ToString());
                }
            }).Wait();

            Task.Run(() =>
            {
                try
                {
                    ///PEDIDOS
                    List<Pedido> pedidosToSinc = pController.FindAllNotSync();
                    pedidosToSinc.ForEach(p => { pController.ComSocket(p, out string error); });
                }
                catch (Exception e)
                {
                    Log.Error(Ext.LOG_APP, e.ToString());
                }
            }).Wait();

            Task.Run(() =>
            {
                try
                {
                    ///PEDIDOS CANCELADOS
                    List<Pedido> cancel = new PedidoController().CanceledNotSync;
                    cancel.Where(p => p.INDCANC && !p.SYNCCANC).ToList().ForEach(p => { pController.syncCancellation(p); });
                }
                catch (Exception e)
                {
                    Log.Error(Ext.LOG_APP, e.ToString());
                }
            }).Wait();
           
            Task.Run(() =>
            {
                try
                {
                    ///PAGAMENTOS
                    var pagamentos = baiDAO.FindNotSinc();
                    pagamentos.ForEach(p =>
                    {
                        var pedido = new PedidoController().FindById(p.FT_PEDIDO_ID.Value);
                        if (pedido != null)
                            pController.ComSocketReceivementAndDevolution(pedido, out string message, p);
                    });
                }
                catch (Exception e)
                {
                    Log.Error(Ext.LOG_APP, e.ToString());
                }
            }).Wait();

            //Task.Run(() =>
            //{
            //    try
            //    {
            //        ///DEVOLUÇÕES
            //        var devolucoes = devDAO.Table.Where(d => !d.INDSINC).ToList();
            //        for (int i = 0; i < devolucoes.Count; i++)
            //        {
            //            var d = devolucoes[i];
            //            var aux = devDAO.FindById(d.FT_PEDIDO_ITEM_DEVOLUCAO_ID.Value);
            //            if (aux != null)
            //            {
            //                bool isSync = aux.INDSINC;
            //                if (!isSync)
            //                {
            //                    var pedido = new PedidoController().FindById(d.FT_PEDIDO_ID.Value);
            //                    if (pedido != null)
            //                        pController.ComSocketReceivementAndDevolution(pedido, out string message);
            //                }
            //            }
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        Log.Error(Ext.LOG_APP, e.ToString());
            //    }
            //}).Wait();

            Task.Run(() =>
            {
                try
                {
                    ///REAGENDAMENTOS
                    var baixas = baiDAO.FindAllNotSync();
                    baixas.ForEach(d =>
                    {
                        var pedido = new PedidoController().FindById(d.FT_PEDIDO_ID.Value);
                        if (pedido != null)
                            pController.ComSocketReceivementAndDevolution(pedido, out string message);
                    });
                }
                catch (Exception e)
                {
                    Log.Error(Ext.LOG_APP, e.ToString());
                }
            }).Wait();
        }
    }
}