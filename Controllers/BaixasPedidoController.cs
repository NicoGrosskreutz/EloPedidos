using Android.App;
using Android.Util;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EloPedidos.Controllers
{
    public class BaixasPedidoController
    {
        private BaixasPedidoDAO DAO = null;

        public long Count => DAO.Count;

        public BaixasPedidoController()
        {
            Database.GetConnection().CreateTable<BaixasPedido>();
            Database.GetConnection().CreateTable<Pagamento>();
            DAO = new BaixasPedidoDAO();
        }
        public bool Save(BaixasPedido baixa)
        {
            try
            {
                ValidarBaixa(baixa);
                baixa.USRULTAT = new OperadorController().Operador.USROPER;
                baixa.DTHULTAT = DateTime.Now;
                return DAO.Save(baixa);
            }
            catch
            {
                throw;
            }
        }

        public bool Delete(BaixasPedido b) => DAO.Delete(b.FT_PEDIDO_BAIXA_ID);
        public bool DeletePagamento(Pagamento b) => DAO.DeletePagamento(b.FT_PAGAMENTO_ID);
        public void SalvarPagamento(Pagamento b)
        {
            var conn = Database.GetConnection();
            if (b.FT_PAGAMENTO_ID == null)
                conn.Insert(b);
            else
                conn.Update(b);
        }

        public double totalReceberCliente(long ID_PESS, out string[] _Pedidos) => new PedidoController().totalReceberCliente(ID_PESS, out _Pedidos);

        public List<Pagamento> FindAllBaixas()
        {
            var conn = Database.GetConnection();
            return conn.Table<Pagamento>().ToList();
        }

        public List<Pagamento> FindAllBaixas(long CG_PESSOA_ID)
        {
            var conn = Database.GetConnection();
            var lista = conn.Table<Pagamento>().Where(p => p.CG_PESSOA_ID == CG_PESSOA_ID).ToList();
            return lista.Where(p => p.DTHPGMTO.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
        }
        public Pagamento FindLastPGMTO(long CG_PESSOA_ID)
        {
            var conn = Database.GetConnection();
            var lista = conn.Table<Pagamento>().Where(p => p.CG_PESSOA_ID == CG_PESSOA_ID).OrderByDescending(p => p.DTHPGMTO).ToList();
            return lista.Where(p => p.DTHPGMTO.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy")).FirstOrDefault();
        }

        public List<Pagamento> FindNotSinc() => DAO.FindNotSinc();

        public List<BaixasPedido> baixasByDate(DateTime date, long cG_PESSOA_ID)
        {
            List<BaixasPedido> baixasDoCliente = DAO.FindByCG_PESSOA_ID(cG_PESSOA_ID);
            return baixasDoCliente.Where(b => b.DTHULTAT.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")).ToList();
        }
        public bool SaveSync(BaixasPedido baixa, string message)
        {
            var conn = Database.GetConnection();
            baixa.INDSINC = true;
            baixa.ENVMSG = message;
            return DAO.Save(baixa);
        }

        public bool SaveReceivement(BaixasPedido baixa)
        {
            try
            {
                ValidarBaixa(baixa);
                baixa.USRULTAT = new OperadorController().Operador.USROPER;
                baixa.DTHULTAT = DateTime.Now;
                return DAO.Save(baixa);
            }
            catch
            {
                throw;
            }
        }

        public BaixasPedido FindById(long pFT_PEDIDO_BAIXA_ID) => DAO.FindById(pFT_PEDIDO_BAIXA_ID);

        public BaixasPedido FindByNROPEDID(long pNROPEDID) => DAO.FindByNROPEDID(pNROPEDID);
        public List<BaixasPedido> FindByNROPED(long pNROPEDID) => DAO.FindByNROPED(pNROPEDID);

        public BaixasPedido FindByFT_PEDIDO_ID(long pFT_PEDIDO_ID) => DAO.FindByFT_PEDIDO_ID(pFT_PEDIDO_ID);

        public List<BaixasPedido> FindAll() => DAO.FindAll();

        /// <summary>
        ///     Gera uma nova baixa baseada no pedido
        /// </summary>
        /// <param name="pedido">Pedido a gerar a baixa</param>
        /// <param name="FT_BAIXA_PEDIDO_ID">Se diferente de null, adiciona o id</param>
        /// <returns></returns>
        public BaixasPedido GerarBaixa(Pedido pedido, long? FT_PEDIDO_BAIXA_ID = null)
        {
            BaixasPedido baixa = null;

            if (FT_PEDIDO_BAIXA_ID.HasValue)
                return FindById(FT_PEDIDO_BAIXA_ID.Value);
            else if ((baixa = FindByNROPEDID(pedido.NROPEDID)) != null)
                return baixa;
            else
                return new BaixasPedido()
                {
                    FT_PEDIDO_BAIXA_ID = FT_PEDIDO_BAIXA_ID,
                    CG_PESSOA_ID = pedido.CG_PESSOA_ID,
                    FT_PEDIDO_ID = pedido.FT_PEDIDO_ID,
                    CG_VENDEDOR_ID = new VendedorController().Vendedor.CG_VENDEDOR_ID,
                    CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
                    SITBAIXA = (int)BaixasPedido.SitBaixa.Aberto,
                    DATPGMT = DateTime.Now,
                    TOTLPEDID = ValorTotal(pedido.NROPEDID),
                    VLRRECBR = this.OrderBalance(pedido.FT_PEDIDO_ID.Value),
                    DATVCTO = pedido.DATERET,
                    DTHULTAT = DateTime.Now,
                    Pedido = pedido
                };
        }

        public Pagamento GerarRegistroPGMTO(Pedido pedido)
        {
            return new Pagamento()
            {
                FT_PEDIDO_BAIXA_ID = this.GerarBaixa(pedido).FT_PEDIDO_BAIXA_ID,
                CG_PESSOA_ID = pedido.CG_PESSOA_ID,
                FT_PEDIDO_ID = pedido.FT_PEDIDO_ID,
                INDSYNC = false,
                VLRPGMT = 0,
                DTHULTAT = DateTime.Now,
                DTHPGMTO = DateTime.Now,
                USRULTAT = new VendedorController().GetVendedor().USROPER
            };
        }

        /// <summary>
        /// Retorna o valor total do pedido
        /// </summary>
        /// <param name="pNROPEDID"></param>
        /// <returns></returns>
        public double ValorTotal(long pNROPEDID)
        {
            return Math.Round(new PedidoController()
                .GetTotalValue(pNROPEDID), 2);
        }

        /// <summary>
        ///  Retorna o valor total do pedido já formatado
        /// </summary>
        /// <param name="pNROPEDID"></param>
        /// <returns></returns>
        public string ValorTotalToString(long pNROPEDID)
            => ValorTotal(pNROPEDID).FormatDouble();

        private void ValidarBaixa(BaixasPedido baixa)
        {
            if (!baixa.FT_PEDIDO_ID.HasValue) throw new Exception("O VÍNCULO COM O PEDIDO É OBRIGATÓRIO!");
            if (!baixa.CG_PESSOA_ID.HasValue) throw new Exception("A BAIXA DEVE OBRIGATÓRIAMENTE CONTER UM CLIENTE!");
        }

        /// <summary>
        ///  Salva a baixa total e retorna o restante do troco em uma variavel
        /// </summary>
        /// <param name="baixa"></param>
        /// <param name="valorPagamento"></param>
        /// <param name="troco">Troco do pagamento</param>
        /// <returns></returns>
        public bool SalvarBaixaTotal(BaixasPedido baixa, double valorPagamento, out double troco)
        {
            try
            {
                bool atualizar = baixa.FT_PEDIDO_BAIXA_ID.HasValue;
                new PedidoController().SetAnswered(baixa.FT_PEDIDO_ID.Value);
                double vlrDevol = this.FindValorDevol(new PedidoController().FindById(baixa.FT_PEDIDO_ID.Value));

                troco = valorPagamento - baixa.VLRRECBR;

                if (valorPagamento >= baixa.VLRRECBR)
                {
                    baixa.VLRPGMT += baixa.VLRRECBR;
                    baixa.ULTPGMTO = baixa.VLRRECBR;
                }
                else
                {
                    baixa.VLRPGMT += valorPagamento;
                    baixa.ULTPGMTO = valorPagamento;
                }

                baixa.VLRRECBR = 0;
                baixa.INDPAGO = "N";
                baixa.INDSINC = false;
                baixa.SITBAIXA = (int)BaixasPedido.SitBaixa.Atendido;
                baixa.DTHULTAT = DateTime.Now;
                baixa.DATVCTO = DateTime.Now;
                baixa.DATPGMT = DateTime.Now;
                baixa.VLRDEVOL = vlrDevol;
                this.Save(baixa);

                if (baixa.ULTPGMTO > 0)
                {
                    Pagamento pagamento = GerarRegistroPGMTO(new PedidoController().FindById(baixa.FT_PEDIDO_ID.Value));
                    pagamento.VLRPGMT = baixa.ULTPGMTO;
                    SalvarPagamento(pagamento);
                }

                SaveReceivementLocalization(baixa);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Baixa", ex.ToString());
                throw;
            }
        }

        /// <summary>
        ///  Salva uma baixa parcial
        /// </summary>
        /// <param name="baixa"></param>
        /// <param name="valorPagamento"></param>
        /// <returns></returns>
        public bool SalvarBaixaParcial(BaixasPedido baixa, double valorPagamento, out double resto, DateTime? vencimento = null)
        {
            try
            {
                double vlrDevol = this.FindValorDevol(new PedidoController().FindById(baixa.FT_PEDIDO_ID.Value));
                baixa.VLRDEVOL = vlrDevol;

                baixa.VLRPGMT += valorPagamento;
                baixa.ULTPGMTO = valorPagamento;
                baixa.DATPGMT = DateTime.Now;
                baixa.VLRRECBR = (OrderBalance(baixa.FT_PEDIDO_ID.Value) - baixa.VLRPGMT);
                baixa.INDPAGO = "S";
                baixa.DTHULTAT = DateTime.Now;
                baixa.INDSINC = false;

                valorPagamento = 0;

                resto = 0;

                if (vencimento.HasValue)
                    baixa.DATVCTO = vencimento.Value;

                this.Save(baixa);
                new PedidoController().SetAtendidoParcial(baixa.FT_PEDIDO_ID.Value);

                if (baixa.ULTPGMTO > 0)
                {
                    Pagamento pagamento = GerarRegistroPGMTO(new PedidoController().FindById(baixa.FT_PEDIDO_ID.Value));
                    pagamento.VLRPGMT = baixa.ULTPGMTO;
                    SalvarPagamento(pagamento);
                }

                SaveReceivementLocalization(baixa);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Baixa", ex.ToString());
                throw;
            }
        }

        public bool SalvarBaixa(BaixasPedido baixa, DateTime vencimento)
        {
            try
            {
                double vlrDevol = this.FindValorDevol(new PedidoController().FindById(baixa.FT_PEDIDO_ID.Value));
                baixa.VLRDEVOL = vlrDevol;
                baixa.VLRPGMT += 0;
                baixa.ULTPGMTO = 0;
                baixa.INDPAGO = "S";
                baixa.INDSINC = false;
                baixa.VLRRECBR = (OrderBalance(baixa.FT_PEDIDO_ID.Value) - baixa.VLRPGMT);
                baixa.DATVCTO = vencimento;

                this.Save(baixa);
                SaveReceivementLocalization(baixa);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Baixa", ex.ToString());
                throw;
            }
        }

        public double FindValorDevol(Pedido p)
        {
            double result = 0;

            List<ItemPedido> itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(p.FT_PEDIDO_ID.Value);

            itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
            {
                var qtdDevol = i.QTDPROD - i.QTDATPRO;
                result += qtdDevol * i.VLRUNIT;
            });

            return result;
        }

        /// <summary>
        ///  Salva a localizacao da baixa do pedido
        /// </summary>
        /// <param name="baixa"></param>
        public void SaveReceivementLocalization(BaixasPedido baixa)
        {
            try
            {
                new GeolocatorController().SaveReceivementLocalizationAsync(baixa.FT_PEDIDO_BAIXA_ID.Value);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        ///  Retorna o saldo total do pedido (total do pedido - comissão do vendedor)
        /// </summary>
        /// <param name="pFT_PEDIDO_ID"></param>
        /// <returns></returns>
        public double OrderBalance(long pFT_PEDIDO_ID)
        {
            try
            {
                Pedido p = new PedidoController().FindById(pFT_PEDIDO_ID);
                Empresa e = new EmpresaController().Empresa;
                var itens = new ItemPedidoController().FindAllOrderItems(pFT_PEDIDO_ID);

                double total = 0;
                itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
                {
                    total += (i.VLRUNIT * i.QTDATPRO);
                });

                return total -= ((total / 100) * p.PERCOMIS);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return 0;
            }
        }

        public double toReceive(long FT_PEDIDO_ID)
        {
            double valor = 0.0;
            try
            {
                Pedido p = new PedidoController().FindById(FT_PEDIDO_ID);
                Empresa e = new EmpresaController().Empresa;
                var itens = new ItemPedidoController().FindAllOrderItems(FT_PEDIDO_ID);

                var baixa = this.FindByFT_PEDIDO_ID(FT_PEDIDO_ID);

                if (baixa != null)
                    valor = baixa.VLRRECBR;
                else
                    valor = OrderBalance(FT_PEDIDO_ID);
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
            }
            return valor;
        }

        public double PrevisaoReceber(List<ItemPedido> itens)
        {
            var receber = this.toReceive(itens.FirstOrDefault().FT_PEDIDO_ID.Value);

            Empresa e = new EmpresaController().Empresa;
            double total = 0;
            double totalComiss = 0;
            double recebido = 0;
            try
            {
                itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
                {
                    Pedido p = new PedidoController().FindById(i.FT_PEDIDO_ID.Value);

                    totalComiss += ((i.VLRUNIT * i.QTDATPRO) / 100) * p.PERCOMIS;

                    total += (i.VLRUNIT * i.QTDATPRO);
                });


                var baixa = this.FindByFT_PEDIDO_ID(itens.FirstOrDefault().FT_PEDIDO_ID.Value);

                if (baixa != null)
                    recebido = baixa.VLRPGMT;

                total -= totalComiss;

            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
            }
            return total - recebido;
        }

        /// <summary>
        ///  Retorna o saldo total do pedido (total do pedido - comissão do vendedor)
        /// </summary>
        /// <param name="pFT_PEDIDO_ID"></param>
        /// <returns></returns>
        public double OrderBalance(long pFT_PEDIDO_ID, out double PorcentagemCliente)
        {
            try
            {
                Pedido p = new PedidoController().FindById(pFT_PEDIDO_ID);
                Empresa e = new EmpresaController().Empresa;
                var itens = new ItemPedidoController().FindAllOrderItems(pFT_PEDIDO_ID);

                double total = 0;
                itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
                {
                    total += (i.VLRUNIT * i.QTDATPRO);
                });

                PorcentagemCliente = ((total / 100) * p.PERCOMIS);
                return total -= PorcentagemCliente;
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                PorcentagemCliente = 0;
                return 0;
            }
        }

        public List<BaixasPedido> FindByDatRet(DateTime data) => DAO.FindByDataRET(data);
    }
}