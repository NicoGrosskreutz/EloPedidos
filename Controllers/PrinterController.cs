using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Bluetooth;
using Android.Text;
using Android.Util;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using Java.Lang.Reflect;
using Java.Util;

namespace EloPedidos.Controllers
{
    public class PrinterController
    {
        public string ESC => "\u001B";
        public string GS => "\u001D";
        public string InitializePrinter => ESC + "@";
        public string BoldOn => ESC + "E" + "\u0001";
        public string BoldOff => ESC + "E" + "\0";
        /// <summary>
        ///  Fonte com tamanho dobrado
        /// </summary>
        public string DoubleOn => GS + "!" + "\u0011";
        /// <summary>
        ///  Desativa tamanho dobrado da fonte
        /// </summary>
        public string DoubleOff => GS + "!" + "\0";
        public string Font1 => ESC + "!" + "\u0028";
        public string Font2 => ESC + "!" + "\0";

        public BluetoothAdapter Adapter { get { return BluetoothAdapter.DefaultAdapter; } }

        private BluetoothSocket auxBluetooth { get; set; } = null;

        public List<BluetoothDevice> Devices { get { return this.GetDevices(); } }

        /// <summary>
        ///  Recebe o nome do dispositivo e envia uma string para impressão ; 
        ///  fonte: https://forums.xamarin.com/discussion/86218/print-in-bluetooth-printer
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="text"></param>
        public BluetoothSocket GetSocket(string deviceName)
        {
            try
            {
                var address = Devices.Where(i => i.Name.Equals(deviceName)).FirstOrDefault().Address;

                BluetoothDevice device = Adapter.GetRemoteDevice(address);
                Method m = device.Class.GetMethod("createRfcommSocket", new Java.Lang.Class[] { Java.Lang.Integer.Type });
                BluetoothSocket socket = (BluetoothSocket)m.Invoke(device, 1);
                auxBluetooth = socket;

                return socket;

            }
            catch (Exception ex)
            {
                Log.Debug("PrinterController", ex.ToString());

                return null;
            }
        }
        public bool ConnectPrinter(BluetoothSocket socket, string deviceName)
        {
            try
            {
                bool result = true;

                if (socket == null)
                    result = false;

                if (socket != null && !socket.IsConnected)
                    socket.Connect();

                if (socket == null || !socket.IsConnected)
                {
                    var address = Devices.Where(i => i.Name.Equals(deviceName)).FirstOrDefault().Address;
                    BluetoothDevice device = Adapter.GetRemoteDevice(address);
                    socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                    socket.ConnectAsync();

                    for (int i = 0; i <= 5; i++)
                    {
                        if (socket.IsConnected)
                            break;

                        Thread.Sleep(1000);
                    }
                }

                if (!socket.IsConnected)
                    result = false;

                return result;
            }
            catch (Exception e)
            {
                SisLog.Logger(e.ToString(), "ConnectPrinter");
                return false;
            }
        }
        public void ClosePrinter()
        {
            try
            {
                if (auxBluetooth != null)
                    auxBluetooth.Close();
            }
            catch (Exception e)
            {
                SisLog.Logger(e.ToString(), "ClosePrinter");
            }
        }

        /// <summary>
        ///  Retorna uma lista de dispositivos pareados com o sistema Android
        /// </summary>
        /// <returns></returns>
        private List<BluetoothDevice> GetDevices()
        {
            if (Adapter != null)
            {
                if (!Adapter.IsEnabled)
                    Adapter.Enable();

                if (Adapter.StartDiscovery())
                    return Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
                else
                    return Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
            }
            else return null;
        }

        /// <summary>
        ///  Formata um pedido para impressão em A7
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public string FormatOrderForPrintA7(Pedido pedido)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                /* 47 - caracteres por linha - Impressão A7 */
                /* =============================================== */
                builder.Append(InitializePrinter);
                builder.Append(HeaderTemplateA7(pedido));
                builder.Append(ProductsTemplateA7(pedido));
                builder.Append(AuthorizationTemplateA7());

                return Format.RemoveAccents(builder.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return string.Empty;
            }
        }
        /// <summary>
        ///  Formata uma devolução para impressão em A7
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public string FormatOrderForPrintA7(Pedido pedido, string indDevolucao, long[] PEDIDOS_IDS = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                /* 47 - caracteres por linha - Impressão A7 */
                /* =============================================== */
                builder.Append(InitializePrinter);
                builder.Append(HeaderTemplateA7(pedido, indDevolucao));
                builder.Append(ProductsTemplateA7(pedido, indDevolucao, PEDIDOS_IDS));
                builder.Append(AuthorizationTemplateA7(indDevolucao));

                return Format.RemoveAccents(builder.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        ///  Formata relatório de pedidos para impressão em A7
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns></returns>
        public string FormatOrderForPrintA7(List<EmissaoAdapterCls> lista)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                /* 47 - caracteres por linha - Impressão A7 */
                /* =============================================== */
                builder.Append(InitializePrinter);
                builder.Append(RelatorioPedidoPrint(lista));

                return Format.RemoveAccents(builder.ToString());
            }
            catch (Exception ex)
            {
                Log.Error("Error", ex.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        ///  Formata a linha da tabela de produtos para a impressão
        /// </summary>
        /// <param name="produto"></param>
        /// <param name="unidade"></param>
        /// <param name="quantidade"></param>
        /// <param name="valorUnitario"></param>
        /// <returns></returns>
        public string FormatItems(string produto, string unidade, string quantidade, string valorUnitario, string valorTotal)
        {
            if (produto.Length > 13)
                produto = produto.Substring(0, 13);
            else
                produto = produto.PadRight(13);

            if (unidade.Length > 3)
                unidade = unidade.Substring(0, 3);
            else if (unidade.Length == 3)
                unidade = unidade.PadRight(0);
            else
                unidade = unidade.PadRight(3);

            if (quantidade.Length > 7)
                quantidade = quantidade.Substring(0, 7);
            else
                quantidade = quantidade.PadLeft(5);

            if (valorUnitario.Length > 8)
                valorUnitario = valorUnitario.Substring(0, 8);
            else
                valorUnitario = valorUnitario.PadLeft(6);

            if (valorTotal.Length > 10)
                valorTotal = valorTotal.Substring(0, 10);
            else
                valorTotal = valorTotal.PadLeft(8);

            return produto + " |" + unidade + " | " + quantidade + " | " + valorUnitario + " | " + valorTotal;
        }

        public string FormatItems(string produto, string quantidade, string valorUnitario, string valorTotal)
        {
            if (produto.Length > 22)
                produto = produto.Substring(0, 22);
            else
                produto = produto.PadRight(22);

            if (quantidade.Length > 4)
                quantidade = quantidade.Substring(0, 4);
            else
                quantidade = quantidade.PadLeft(4);

            if (valorUnitario.Length > 6)
                valorUnitario = valorUnitario.Substring(0, 6);
            else
                valorUnitario = valorUnitario.PadLeft(6);

            if (valorTotal.Length > 17)
                valorTotal = valorTotal.Substring(0, 7);
            else
                valorTotal = valorTotal.PadLeft(7);

            return produto + " | " + quantidade + " | " + valorUnitario + " | " + valorTotal;
        }

        public string FormatSaldoItens(string produto, string unidade, string quantidade, string saldo, string qntdevol)
        {
            if (produto.Length > 19)
                produto = produto.Substring(0, 19);
            else
                produto = produto.PadRight(19);

            if (unidade.Length == 3)
                unidade = unidade.PadRight(0);
            else if (unidade.Length > 2)
                unidade = unidade.Substring(0, 2);
            else
                unidade = unidade.PadRight(3);

            if (quantidade.Length > 6)
                quantidade = quantidade.Substring(0, 6);
            else
                quantidade = quantidade.PadLeft(4);

            if (saldo.Length > 7)
                saldo = saldo.Substring(0, 7);
            else
                saldo = saldo.PadLeft(5);

            if (qntdevol.Length > 6)
                qntdevol = qntdevol.Substring(0, 6);
            else
                qntdevol = qntdevol.PadLeft(5);

            return produto + " |" + unidade + " | " + quantidade + " | " + qntdevol + " | " + saldo;
        }

        public string formatItensDevol(string qtd, string produto, string valor, string qntdevol, string saldo)
        {
            if (qtd.Length > 3)
                qtd = qtd.Substring(0, 3);
            else
                qtd = qtd.PadLeft(3);

            if (produto.Length > 20)
                produto = produto.Substring(0, 20);
            else
                produto = produto.PadRight(20);

            if (valor.Length > 6)
                valor = valor.Substring(0, 6);
            else
                valor = valor.PadLeft(6);

            if (qntdevol.Length > 3)
                qntdevol = qntdevol.Substring(0, 3);
            else
                qntdevol = qntdevol.PadLeft(3);

            if (saldo.Length > 3)
                saldo = saldo.Substring(0, 3);
            else
                saldo = saldo.PadLeft(3);

            

            return qtd + " | " + produto + " | " + valor + " | " + qntdevol + " | " + saldo;
        }

        public string FormatItemsDevol(string produto, string unidade, string qntdevol)
        {
            if (produto.Length > 19)
                produto = produto.Substring(0, 19);
            else
                produto = produto.PadRight(19);

            if (unidade.Length == 3)
                unidade = unidade.PadRight(0);
            else if (unidade.Length > 2)
                unidade = unidade.Substring(0, 2);
            else
                unidade = unidade.PadRight(3);

            if (qntdevol.Length > 16)
                qntdevol = qntdevol.Substring(0, 15);
            else
                qntdevol = qntdevol.PadLeft(15);

            return produto + " |" + unidade + " | " + qntdevol;
        }

        public string FormatItemsRelat(string data, string pedido, string cliente, string valor)
        {
            if (data.Length > 11)
                data = data.Substring(0, 10);
            else
                data = data.PadRight(8);

            if (pedido.Length > 7)
                pedido = pedido.Substring(0, 7);
            else
                pedido = pedido.PadLeft(6);

            if (cliente.Length > 19)
                cliente = cliente.Substring(0, 19);
            else
                cliente = cliente.PadRight(18);

            if (valor.Length > 8)
                valor = valor.Substring(8);
            else
                valor = valor.PadRight(0);

            return data + " | " + pedido + "| " + cliente + " | " + valor;
        }
        public string FormatItemsBaixa(string pessoa, string value)
        {
            if (pessoa.Length > 19)
                pessoa = pessoa.Substring(0, 19);
            else
                pessoa = pessoa.PadRight(19);

            value = value.PadLeft(10);

            return pessoa + " | " + value;
        }

        public string FormatValorDevedor(string nro, double valor)
        {
            string str = string.Empty;

            string traco = "-".PadLeft(1);
            string preco = valor.ToString("N2").PadLeft(7);

            str = $"{BoldOn}Pedido :{BoldOff} {nro.PadLeft(6)} {traco} R$ {preco}";
            return str;
        }

        public string FormatItemsAReceber(string value)
        {
            value = value.PadLeft(10);

            return " " + value;
        }

        public string FormatLongTextA7(string text)
        {
            int count = 0;
            int resto = text.Length;
            StringBuilder builder = new StringBuilder();

            // Enquanto a substring restante for maior que 46 caracteres continua o loop
            while (text.Substring(count, resto).Length > 46)
            {
                builder.AppendLine(text.Substring(count, 46));
                resto -= 46;
                count += 46;
            }

            // Última linha se for menor que 46 caracteres
            builder.AppendLine(text.Substring(count, text.Length - count));

            return builder.ToString();
        }

        public string HeaderTemplateA7(Pedido pedido)
        {
            try
            {
                var empresa = new EmpresaController().Empresa;
                var vendedor = new VendedorController().Vendedor;
                var cliente = new PessoaController().FindById(pedido.ID_PESSOA.Value);
                StringBuilder builder = new StringBuilder();

                //            var myString = "".PadLeft(10);
                string fDSCDCPES = "";
                string fNROPEDID = "".PadLeft(34 - empresa.NOMFANTA.Length) + "PEDIDO:" + "0".PadLeft(6 - Convert.ToString(pedido.NROPEDID).Length) + pedido.NROPEDID;
                string fDATEMISS = "".PadLeft(16) + "EMISSAO:";

                //if (cliente.IDTDCPES.Equals(0)) { fDSCDCPES = "CNPJ:"; }
                //else
                //{
                //	if (cliente.IDTDCPES.Equals(1)) { fDSCDCPES = "CPF.:"; }
                //	else { if (cliente.IDTDCPES.Equals(2)) fDSCDCPES = "COD.:"; }
                //}
                if (cliente.IDTPESS.Length == 14) { fDSCDCPES = "CNPJ:"; }
                else
                {
                    if (cliente.IDTPESS.Length == 11) { fDSCDCPES = "CPF.:"; }
                    else { fDSCDCPES = "COD.:"; }
                }
                string NROTLFN = vendedor.NROTLFN;
                if (NROTLFN != null)
                {
                    if (NROTLFN.StartsWith("0800"))
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"(0000)\000\-0000");
                    if (NROTLFN.Length == 8)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"0000\-0000");
                    else if (NROTLFN.Length == 9)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"00000\-0000");
                    else if (NROTLFN.Length == 10)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"(00)0000\-0000");
                    else if (NROTLFN.Length == 11)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"(00)00000\-0000");
                }



                string NOMVEND = vendedor.NOMVEND;
                Municipio municipioCliente = new MunicipioController().FindById(cliente.CODMUNIC);
                Municipio municipioEmpresa = new MunicipioController().FindById(empresa.CODMUNIC);

                if (NOMVEND.Length > 18)
                    NOMVEND = NOMVEND.Substring(0, 18);

                string DATARET = pedido.DATEMISS.ToString("HH:mm:ss").PadLeft(47 - (10 + NOMVEND.Length));

                builder.Append($"{ESC}")
                    .AppendLine("================================================")
                    .AppendLine($"{BoldOn}{empresa.NOMFANTA}{fNROPEDID}{BoldOff}")
                    .AppendFormat("{0}{1} \n", empresa.DSCENDER, empresa.NROENDER == 0 ? "" : $", {empresa.NROENDER}", $"{empresa.NOMBAIRR}");

                if (municipioEmpresa != null)
                    builder.AppendLine($"{municipioEmpresa.NOMMUNIC} / {municipioEmpresa.CODUF}");

                builder.AppendLine($"{Format.MaskFone(empresa.NROFONE)}{fDATEMISS}{BoldOn}{pedido.DATEMISS.ToString("dd/MM/yyyy")}{BoldOff}")
                    .AppendLine($"VENDEDOR: {NOMVEND}{BoldOn}{DATARET}{BoldOff}")
                    .AppendLine($"TELEFONE VENDEDOR: {BoldOn}{NROTLFN}{BoldOff}")
                    .AppendLine("================================================");

                builder.AppendLine($"{"COD.: " + cliente.CODPESS}");

                if (fDSCDCPES.Equals("CNPJ:"))
                    builder.AppendLine($"{fDSCDCPES + Format.MaskCPF_CNPJ(cliente.IDTPESS.ToString(), "0")}");
                else if (fDSCDCPES.Equals("CPF.:"))
                    builder.AppendLine($"{fDSCDCPES + Format.MaskCPF_CNPJ(cliente.IDTPESS.ToString(), "1")}");

                builder.AppendLine($"CLIENTE: {cliente.NOMPESS}")
                .AppendFormat("{0}{1} \n", cliente.DSCENDER, cliente.NROENDER.Equals("") ? "" : $", {cliente.NROENDER}", $"{cliente.NOMBAIRR}")
                .AppendLine($"BAIRRO:{cliente.NOMBAIRR}");
                if (municipioCliente != null)
                    builder.AppendLine($"CIDADE:{municipioCliente.NOMMUNIC} / {municipioCliente.CODUF}");
                if (!string.IsNullOrEmpty(cliente.NROCELUL))
                    builder.AppendLine($"TELEFONE: {Format.MaskFone(cliente.NROCELUL)}");

                return builder.ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public string HeaderTemplateA7(Pedido pedido, string indDevolucao)
        {
            try
            {
                DevolucaoItemController dController = new DevolucaoItemController();
                var baixa = new BaixasPedidoController().FindByFT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);
                DateTime DATRET;
                if (baixa != null)
                    DATRET = baixa.DATPGMT;
                else
                {
                    List<DevolucaoItem> dev = null;
                    if ((dev = new DevolucaoItemDAO().FindByFT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value)).Count > 0)
                        DATRET = dev.FirstOrDefault().DTHULTAT;
                    else
                        DATRET = DateTime.Now;
                }


                if (DATRET.ToString("dd/MM/yyyy") == "01/01/0001")
                    DATRET = DateTime.Now;

                var empresa = new EmpresaController().Empresa;
                var vendedor = new VendedorController().Vendedor;
                var cliente = new PessoaController().FindById(pedido.ID_PESSOA.Value);

                Municipio municipioCliente = new MunicipioController().FindById(cliente.CODMUNIC);
                Municipio municipioEmpresa = new MunicipioController().FindById(empresa.CODMUNIC);

                StringBuilder builder = new StringBuilder();

                string fDSCDCPES = "";
                string fNROPEDID = "".PadLeft(34 - empresa.NOMFANTA.Length) + "PEDIDO:" + "0".PadLeft(6 - Convert.ToString(pedido.NROPEDID).Length) + pedido.NROPEDID;
                string fDATERTNO = "".PadLeft(16) + "EMISSÃO:";

                if (cliente.IDTPESS.Length == 14) { fDSCDCPES = "CNPJ:"; }
                else
                {
                    if (cliente.IDTPESS.Length == 11) { fDSCDCPES = "CPF.:"; }
                    else { fDSCDCPES = "COD.:"; }
                }

                string NROTLFN = vendedor.NROTLFN;
                if (NROTLFN != null)
                {
                    if (NROTLFN.StartsWith("0800"))
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"(0000)\000\-0000");
                    if (NROTLFN.Length == 8)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"0000\-0000");
                    else if (NROTLFN.Length == 9)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"00000\-0000");
                    else if (NROTLFN.Length == 10)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"(00)0000\-0000");
                    else if (NROTLFN.Length == 11)
                        NROTLFN = Convert.ToUInt64(NROTLFN).ToString(@"(00)00000\-0000");
                }

                string NOMVEND = vendedor.NOMVEND;
                if (NOMVEND.Length > 18)
                    NOMVEND = NOMVEND.Substring(0, 18);

                string HORARET = DATRET.ToString("HH:mm:ss").PadLeft(47 - (10 + NOMVEND.Length));

                string DSCEND = empresa.DSCENDER;
                if (DSCEND.Length > 27)
                    DSCEND.Substring(0, 27);

                string DATAPEDIDO = pedido.DATEMISS.ToString("dd/MM/yyyy").PadLeft(46 - (DSCEND.Length + empresa.NROENDER.ToString().Length));

                builder.Append($"{ESC}")
                    .AppendLine("================================================")
                    .AppendLine($"{BoldOn}{empresa.NOMFANTA}{fNROPEDID}{BoldOff}")
                    .AppendLine($"{empresa.DSCENDER}, {empresa.NROENDER}{DATAPEDIDO}");
                if (municipioEmpresa != null)
                    builder.AppendLine($"{municipioEmpresa.NOMMUNIC} / {municipioEmpresa.CODUF}");
                builder.AppendLine($"{Format.MaskFone(empresa.NROFONE)}{fDATERTNO}{BoldOn}{DATRET.ToString("dd/MM/yyyy")}{BoldOff}")
                    .AppendLine($"VENDEDOR: {NOMVEND}{HORARET}")
                    .AppendLine($"TELEFONE VENDEDOR: {BoldOn}{NROTLFN}{BoldOff}")
                    .AppendLine("================================================");

                builder.AppendLine($"{"COD.: " + cliente.CODPESS}");

                if (fDSCDCPES.Equals("CNPJ:"))
                    builder.AppendLine($"{fDSCDCPES + Format.MaskCPF_CNPJ(cliente.IDTPESS.ToString(), "0")}");
                else if (fDSCDCPES.Equals("CPF.:"))
                    builder.AppendLine($"{fDSCDCPES + Format.MaskCPF_CNPJ(cliente.IDTPESS.ToString(), "1")}");

                builder.AppendLine($"CLIENTE: {cliente.NOMPESS}")
                    .AppendFormat("{0}{1} \n", cliente.DSCENDER, cliente.NROENDER.Equals("") ? "" : $", {cliente.NROENDER}", $"{cliente.NOMBAIRR}")
                    .AppendLine($"BAIRRO:{cliente.NOMBAIRR}");
                if (municipioCliente != null)
                    builder.AppendLine($"CIDADE:{municipioCliente.NOMMUNIC} / {municipioCliente.CODUF}");
                if (!string.IsNullOrEmpty(cliente.NROCELUL))
                    builder.AppendLine($"TELEFONE: {Format.MaskFone(cliente.NROCELUL)}");

                return builder.ToString();

            }
            catch (Exception e)
            {
                SisLog.Logger(e.ToString(), "HeaderTemplateA7_devol");
                return "";
            }
        }


        //public string ProductsTemplateA7(Pedido pedido)
        //{
        //    try
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        string fDATERTNO = "".PadLeft(16) + "RETORNO:";
        //        string DATARET = pedido.DATERET.ToString("dd/MM/yyyy");

        //        builder.AppendLine($"=========== {BoldOn}PRODUTOS{BoldOff} ==========================")
        //               .AppendLine($"{BoldOn}PRODUTO{BoldOff}       | {BoldOn}UN{BoldOff} |  {BoldOn}QTDE{BoldOff} |  {BoldOn}PRECO{BoldOff} | {BoldOn}V. TOTAL{BoldOff} ")
        //               .AppendLine("______________|____|_______|________|__________");

        //        var itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);

        //        double total = 0;
        //        double totalItens = 0;
        //        bool fINDBRIND = false;

        //        itens.ForEach((aux) =>
        //        {
        //            total = Math.Round((aux.VLRUNIT * aux.QTDPROD) - aux.VLRDSCTO, 2);
        //            if (aux.INDBRIND.Equals(true))
        //                fINDBRIND = true;
        //            else
        //            {
        //                if (aux.Produto.IDTUNID == "KG")
        //                {
        //                    builder.AppendLine(FormatItems(aux.Produto.DSCPROD,
        //                    aux.Produto.IDTUNID,
        //                    aux.QTDPROD.ToString("0").Replace(".", ","),
        //                    aux.VLRUNIT.ToString("N2"),
        //                    total.ToString("N2")));

        //                    totalItens += total;
        //                }
        //                else
        //                {
        //                    builder.AppendLine(FormatItems(aux.Produto.DSCPROD,
        //                    aux.Produto.IDTUNID,
        //                    aux.QTDPROD.ToString("####"),
        //                    aux.VLRUNIT.ToString("N2"),
        //                    total.ToString("N2")));

        //                    totalItens += total;
        //                }
        //            }
        //        });

        //        builder.AppendLine("______________|____|_______|________|__________");

        //        string totalStr = "TOTAL: " + totalItens.ToString("0.00").Replace(".", ",");
        //        builder.AppendLine($"{BoldOn}{totalStr.PadLeft(46)}{BoldOff}");

        //        if (fINDBRIND.Equals(true))
        //        {
        //            builder.AppendLine($"=========== {BoldOn}BRINDES{BoldOff} ==========================")
        //                   .AppendLine($" {BoldOn}PRODUTO{BoldOff}      | {BoldOn}UN{BoldOff} |  {BoldOn}QTDE{BoldOff} |  {BoldOn}PRECO{BoldOff} | {BoldOn}V. TOTAL{BoldOff} ")
        //                   .AppendLine("______________|____|_______|________|__________");
        //            itens.ForEach((aux) =>
        //            {
        //                total = Math.Round((aux.VLRUNIT * aux.QTDPROD) - aux.VLRDSCTO, 2);
        //                if (aux.INDBRIND.Equals(true))
        //                {
        //                    builder.AppendLine(FormatItems(aux.Produto.DSCPROD,
        //                        aux.Produto.IDTUNID,
        //                        aux.QTDPROD.ToString("0").Replace(".", ","),
        //                        aux.VLRUNIT.ToString("N2"),
        //                        total.ToString("N2")));
        //                }
        //            });
        //            builder.AppendLine("______________|____|_______|________|__________");
        //        }

        //        if (!pedido.DSCOBSER.IsEmpty())
        //            builder.AppendLine($"OBSERVACAO:")
        //            .AppendLine($"{FormatLongTextA7(pedido.DSCOBSER)}");

        //        builder.AppendLine("");
        //        builder.AppendLine("DATA RETORNO");
        //        builder.AppendLine($"  {DATARET}   ___/___/______    ___/___/______  ");


        //        double toReceive = new BaixasPedidoController().totalReceberCliente(pedido.CG_PESSOA_ID.Value, out string[] _Pedidos);
        //        if (toReceive > 0)
        //        {
        //            builder.AppendLine($"{BoldOn}SALDO DEVEDOR:    R$ {toReceive.ToString("N2").PadLeft(7)}{BoldOff}");
        //            _Pedidos.ToList().ForEach(p =>
        //            {
        //                Pedido ped = new PedidoController().FindByNROPEDID(p.ToLong());
        //                if (ped != null)
        //                {

        //                    double valor = new BaixasPedidoController().toReceive(ped.FT_PEDIDO_ID.Value);
        //                    if (valor > 0)
        //                        builder.AppendLine(FormatValorDevedor(ped.NROPEDID.ToString(), valor));
        //                }
        //            });
        //        }

        //        return builder.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        return "";
        //    }
        //}

        public string ProductsTemplateA7(Pedido pedido)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                string fDATERTNO = "".PadLeft(16) + "RETORNO:";
                string DATARET = pedido.DATERET.ToString("dd/MM/yyyy");

                builder.AppendLine($"=========== {BoldOn}PRODUTOS{BoldOff} ==========================")
                       .AppendLine($"{BoldOn}PRODUTO{BoldOff}                | {BoldOn}QTDE{BoldOff} |  {BoldOn}PRECO{BoldOff} |   {BoldOn}TOTAL{BoldOff}")
                       .AppendLine("_______________________|______|________|_______");

                var itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);

                double total = 0;
                double totalItens = 0;
                bool fINDBRIND = false;

                itens.ForEach((aux) =>
                {
                    total = Math.Round((aux.VLRUNIT * aux.QTDPROD) - aux.VLRDSCTO, 2);
                    if (aux.INDBRIND.Equals(true))
                        fINDBRIND = true;
                    else
                    {
                        if (aux.Produto.IDTUNID == "KG")
                        {
                            builder.AppendLine(FormatItems(aux.Produto.DSCPROD,
                            aux.QTDPROD.ToString("0").Replace(".", ","),
                            aux.VLRUNIT.ToString("N2"),
                            total.ToString("N2")));

                            totalItens += total;
                        }
                        else
                        {
                            builder.AppendLine(FormatItems(aux.Produto.DSCPROD,
                            aux.QTDPROD.ToString("####"),
                            aux.VLRUNIT.ToString("N2"),
                            total.ToString("N2")));

                            totalItens += total;
                        }
                    }
                });

                builder.AppendLine("_______________________|______|________|_______");

                string totalStr = "TOTAL: " + totalItens.ToString("0.00").Replace(".", ",");
                builder.AppendLine($"{BoldOn}{totalStr.PadLeft(46)}{BoldOff}");

                if (fINDBRIND.Equals(true))
                {
                    builder.AppendLine($"=========== {BoldOn}PRODUTOS{BoldOff} ==========================")
                        .AppendLine($"{BoldOn}PRODUTO{BoldOff}                | {BoldOn}QTDE{BoldOff} |  {BoldOn}PRECO{BoldOff} |   {BoldOn}TOTAL{BoldOff}")
                        .AppendLine("_______________________|______|________|_______");
                    itens.ForEach((aux) =>
                    {
                        total = Math.Round((aux.VLRUNIT * aux.QTDPROD) - aux.VLRDSCTO, 2);
                        if (aux.INDBRIND.Equals(true))
                        {
                            builder.AppendLine(FormatItems(aux.Produto.DSCPROD,
                                aux.QTDPROD.ToString("0").Replace(".", ","),
                                aux.VLRUNIT.ToString("N2"),
                                total.ToString("N2")));
                        }
                    });
                    builder.AppendLine("______________|____|_______|________|__________");
                }

                if (!pedido.DSCOBSER.IsEmpty())
                    builder.AppendLine($"OBSERVACAO:")
                    .AppendLine($"{FormatLongTextA7(pedido.DSCOBSER)}");

                builder.AppendLine("");
                builder.AppendLine("DATA RETORNO");
                builder.AppendLine($"  {DATARET}   ___/___/______    ___/___/______  ");


                double toReceive = new BaixasPedidoController().totalReceberCliente(pedido.ID_PESSOA.Value, out string[] _Pedidos);
                if (toReceive > 0)
                {
                    builder.AppendLine($"{BoldOn}SALDO DEVEDOR:    R$ {toReceive.ToString("N2").PadLeft(7)}{BoldOff}");
                    _Pedidos.ToList().ForEach(p =>
                    {
                        Pedido ped = new PedidoController().FindByNROPEDID(p.ToLong());
                        if (ped != null)
                        {

                            double valor = new BaixasPedidoController().toReceive(ped.FT_PEDIDO_ID.Value);
                            if (valor > 0)
                                builder.AppendLine(FormatValorDevedor(ped.NROPEDID.ToString(), valor));
                        }
                    });
                }

                return builder.ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }

        //public string ProductsTemplateA7(Pedido pedido, string indDevolucao, long[] ftPEDIDO_ID = null)
        //{
        //    try
        //    {
        //        StringBuilder builder = new StringBuilder();
        //        BaixasPedidoController bController = new BaixasPedidoController();
        //        var baixa = bController.GerarBaixa(pedido);
        //        var pessoa = new PessoaController().FindById(pedido.ID_PESSOA.Value);

        //        double rest = 0;
        //        rest = baixa.VLRRECBR;

        //        var itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value).Where(i => !i.INDBRIND).ToList();

        //        if (itens.Count > 0)
        //        {
        //            builder.AppendLine($"======== {BoldOn}RESUMO DO PEDIDO{BoldOff} =====================")
        //                   .AppendLine($"{BoldOn}PRODUTO{BoldOff}             | {BoldOn}UN{BoldOff} | {BoldOn}QTDE{BoldOff} | {BoldOn}DEVOL{BoldOff} | {BoldOn}SALDO{BoldOff}")
        //                   .AppendLine("____________________|____|______|_______|______");

        //            DevolucaoItemController devolC = new DevolucaoItemController();

        //            double saldoINI = 0;
        //            string qntDEVOLV = "0";
        //            string saldo = "0";

        //            itens.ForEach(i =>
        //            {
        //                saldoINI = Math.Round(i.QTDPROD);
        //                if (i.QTDATPRO == 0)
        //                    saldo = "0";
        //                else
        //                    saldo = i.QTDATPRO.ToString("###");

        //                qntDEVOLV = Math.Round(i.QTDPROD - i.QTDATPRO).ToString("###");


        //                if (i.IDTUNID != null)
        //                {
        //                    builder.AppendLine(FormatSaldoItens(i.NOMPROD,
        //                        i.IDTUNID,
        //                        saldoINI.ToString("###"),
        //                        saldo,
        //                        qntDEVOLV));
        //                }
        //                else
        //                {
        //                    builder.AppendLine(FormatSaldoItens(i.NOMPROD,
        //                        "UN",
        //                        saldoINI.ToString("###"),
        //                        saldo,
        //                        qntDEVOLV));
        //                }


        //            });

        //            builder.AppendLine("____________________|____|______|_______|______");
        //            builder.AppendLine("\n");
        //        }

        //        var pagamentos = bController.FindAllBaixas(pedido.CG_PESSOA_ID.Value);
        //        double totalPAGO = 0;
        //        pagamentos.ForEach(p => { if (p.FT_PEDIDO_ID.Value == pedido.FT_PEDIDO_ID.Value) totalPAGO += p.VLRPGMT; });

        //        string valorPedido = string.Empty;
        //        string valorDevolucao = string.Empty;
        //        double comissaoCliente = 0;
        //        string totalRecebido = string.Empty;
        //        string ultimoPagamento = string.Empty;
        //        string receber = string.Empty;
        //        string valorVendido = string.Empty;
        //        string totalReceber = string.Empty;

        //        valorPedido = bController.ValorTotal(pedido.NROPEDID).ToString("N2");
        //        valorDevolucao = baixa.VLRDEVOL.ToString("N2");
        //        bController.OrderBalance(pedido.FT_PEDIDO_ID.Value, out comissaoCliente);
        //        totalRecebido = baixa.VLRPGMT.ToString("N2");
        //        ultimoPagamento = totalPAGO.ToString("N2");
        //        receber = baixa.VLRRECBR.ToString("N2");
        //        valorVendido = (bController.ValorTotal(pedido.NROPEDID) - baixa.VLRDEVOL).ToString("N2");
        //        totalReceber = new PedidoController().totalReceberCliente(pessoa.CG_PESSOA_ID.Value, out string[] _pedidos).ToString("N2");

        //        var formatValroVendido = valorVendido.PadLeft(8);
        //        var formatComissaoCliente = comissaoCliente.ToString("N2").PadLeft(8);
        //        var formatValorVendido = ((bController.ValorTotal(pedido.NROPEDID) - baixa.VLRDEVOL) - comissaoCliente).ToString("N2").PadLeft(8);
        //        var formatTotalRecebido = totalRecebido.PadLeft(8);
        //        var formatUltimoPagamento = ultimoPagamento.PadLeft(8);
        //        var formatReceber = receber.PadLeft(8);
        //        var formatTotalReceber = totalReceber.PadLeft(8);


        //        builder.AppendLine($"VALOR VENDIDO ....... :  R$ {formatValroVendido}");
        //        builder.AppendLine($"COMISSÃO DO CLIENTE . :  R$ {formatComissaoCliente}");
        //        builder.AppendLine($"A RECEBER ........... :  R$ {formatValorVendido}");
        //        builder.AppendLine($"TOTAL RECEBIDO ...... :  R$ {formatTotalRecebido}");
        //        builder.AppendLine($"ULTIMO PAGAMENTO .... :  R$ {formatUltimoPagamento}");
        //        builder.AppendLine($"SALDO A RECEBER ..... :  R$ {formatReceber}");
        //        builder.AppendLine("\n");

        //        builder.AppendLine($"TOTAL A RECEBER ..... :  R$ {formatTotalReceber}");

        //        if (baixa.VLRRECBR > 0)
        //        {
        //            string retorno = "".PadLeft(29) + "RETORNO:";
        //            builder.AppendLine("\n");
        //            builder.AppendLine($"{retorno} {BoldOn}{baixa.DATVCTO.ToString("dd/MM/yyyy")}{BoldOff}");
        //        }

        //        return builder.ToString();
        //    }
        //    catch (Exception e)
        //    {
        //        return "";
        //    }
        //}

        public string ProductsTemplateA7(Pedido pedido, string indDevolucao, long[] ftPEDIDO_ID = null)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                BaixasPedidoController bController = new BaixasPedidoController();
                var baixa = bController.GerarBaixa(pedido);
                var pessoa = new PessoaController().FindById(pedido.ID_PESSOA.Value);

                double rest = 0;
                rest = baixa.VLRRECBR;

                var itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value).Where(i => !i.INDBRIND).ToList();

                if (itens.Count > 0)
                {
                    builder.AppendLine($"======== {BoldOn}RESUMO DO PEDIDO{BoldOff} =====================")
                           .AppendLine($"{BoldOn}QTD{BoldOff} | {BoldOn}PRODUTO{BoldOff}              |  {BoldOn}PREÇO{BoldOff} | {BoldOn}DEV{BoldOff} | {BoldOn}SLD{BoldOff}")
                           .AppendLine("____|______________________|________|_____|____");

                    DevolucaoItemController devolC = new DevolucaoItemController();

                    double saldoINI = 0;
                    string qntDEVOLV = "0";
                    string saldo = "0";

                    itens.ForEach(i =>
                    {
                        saldoINI = Math.Round(i.QTDPROD);
                        if (i.QTDATPRO == 0)
                            saldo = "0";
                        else
                            saldo = i.QTDATPRO.ToString("###");

                        qntDEVOLV = Math.Round(i.QTDPROD - i.QTDATPRO).ToString("###");


                        if (i.IDTUNID != null)
                        {
                            builder.AppendLine(formatItensDevol(i.QTDPROD.ToString(),
                                i.NOMPROD,
                                i.VLRUNIT.ToString("N2"),
                                qntDEVOLV,
                                saldo));
                        }
                        else
                        {
                            builder.AppendLine(formatItensDevol(i.QTDPROD.ToString(),
                                i.NOMPROD,
                                i.VLRUNIT.ToString("N2"),
                                qntDEVOLV,
                                saldo));
                        }


                    });

                    builder.AppendLine("____|______________________|________|_____|____");
                    builder.AppendLine("\n");
                }

                var pagamentos = bController.FindAllBaixas(pedido.CG_PESSOA_ID.Value);
                double totalPAGO = 0;
                pagamentos.ForEach(p => { if (p.FT_PEDIDO_ID.Value == pedido.FT_PEDIDO_ID.Value) totalPAGO += p.VLRPGMT; });

                string valorPedido = string.Empty;
                string valorDevolucao = string.Empty;
                double comissaoCliente = 0;
                string totalRecebido = string.Empty;
                string ultimoPagamento = string.Empty;
                string receber = string.Empty;
                string valorVendido = string.Empty;
                string totalReceber = string.Empty;

                valorPedido = bController.ValorTotal(pedido.NROPEDID).ToString("N2");
                valorDevolucao = baixa.VLRDEVOL.ToString("N2");
                bController.OrderBalance(pedido.FT_PEDIDO_ID.Value, out comissaoCliente);
                totalRecebido = baixa.VLRPGMT.ToString("N2");
                ultimoPagamento = totalPAGO.ToString("N2");
                receber = baixa.VLRRECBR.ToString("N2");
                valorVendido = (bController.ValorTotal(pedido.NROPEDID) - baixa.VLRDEVOL).ToString("N2");
                totalReceber = new PedidoController().totalReceberCliente(pessoa.ID.Value, out string[] _pedidos).ToString("N2");

                var formatValroVendido = valorVendido.PadLeft(8);
                var formatComissaoCliente = comissaoCliente.ToString("N2").PadLeft(8);
                var formatValorVendido = ((bController.ValorTotal(pedido.NROPEDID) - baixa.VLRDEVOL) - comissaoCliente).ToString("N2").PadLeft(8);
                var formatTotalRecebido = totalRecebido.PadLeft(8);
                var formatUltimoPagamento = ultimoPagamento.PadLeft(8);
                var formatReceber = receber.PadLeft(8);
                var formatTotalReceber = totalReceber.PadLeft(8);


                builder.AppendLine($"VALOR VENDIDO ....... :  R$ {formatValroVendido}");
                builder.AppendLine($"COMISSÃO DO CLIENTE . :  R$ {formatComissaoCliente}");
                builder.AppendLine($"A RECEBER ........... :  R$ {formatValorVendido}");
                builder.AppendLine($"TOTAL RECEBIDO ...... :  R$ {formatTotalRecebido}");
                builder.AppendLine($"ULTIMO PAGAMENTO .... :  R$ {formatUltimoPagamento}");
                builder.AppendLine($"SALDO A RECEBER ..... :  R$ {formatReceber}");
                builder.AppendLine("\n");

                builder.AppendLine($"TOTAL A RECEBER ..... :  R$ {formatTotalReceber}");

                if (baixa.VLRRECBR > 0)
                {
                    string retorno = "".PadLeft(29) + "RETORNO:";
                    builder.AppendLine("\n");
                    builder.AppendLine($"{retorno} {BoldOn}{baixa.DATVCTO.ToString("dd/MM/yyyy")}{BoldOff}");
                }

                return builder.ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }


        public string AuthorizationTemplateA7()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("================================================")
                    .AppendLine("AUTORIZACAO DO CLIENTE:\n")
                    .AppendLine("ASSINATURA: ___________________________________")
                    .AppendLine("")
                    .AppendLine("================================================")
                    .AppendLine($"{BoldOn}DESENVOLVIDO POR: ELO.SOFTWARE{BoldOff}".PadLeft(46))
                    .AppendLine("")
                    .AppendLine("")
                    .AppendLine("")
                    .AppendLine("");

            if (new EmpresaController().Empresa.CODEMPRE == "DM")
                builder.AppendLine($"{BoldOn}JESUS TE AMA                       BOAS VENDAS{BoldOff}");


            return builder.ToString();
        }
        public string AuthorizationTemplateA7(string indDevolucao)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("================================================")
                    .AppendLine("AUTORIZACAO DO VENDEDOR:\n")
                    .AppendLine("ASSINATURA: ___________________________________")
                    .AppendLine("")
                    .AppendLine("================================================")
                    .AppendLine($"{BoldOn}DESENVOLVIDO POR: ELO.SOFTWARE{BoldOff}".PadLeft(46))
                    .AppendLine("")
                    .AppendLine("")
                    .AppendLine("")
                    .AppendLine("");

            return builder.ToString();
        }

        public string RelatorioPedidoPrint(List<EmissaoAdapterCls> list)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"{BoldOn}RELATORIO PEDIDOS{BoldOff}".PadLeft(36))
                   .AppendLine("================================================")
                   .AppendLine($"{BoldOn}EMISSAO{BoldOff}    | {BoldOn}N.PED.{BoldOff}| {BoldOn}CLIENTE{BoldOff}            |  {BoldOn}VALOR{BoldOff}");

            list.ForEach(i =>
            {
                builder.AppendLine(FormatItemsRelat(
                    i.DATAEMISS,
                    i.NROPED.ToString(),
                    i.NOMCLIE,
                    i.VLRPED));
            });

            builder.AppendLine("================================================")
                    .AppendLine("")
                    .AppendLine($"{BoldOn}DESENVOLVIDO POR: ELO.SOFTWARE{BoldOff}".PadLeft(46))
                    .AppendLine("")
                    .AppendLine("")
                    .AppendLine("")
                    .AppendLine("");

            return builder.ToString();
        }

        public string FechamentoRomaneio(Romaneio romaneio)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                Empresa empresa = new EmpresaController().GetEmpresa();
                Municipio municipioEmpresa = new MunicipioController().FindById(empresa.CODMUNIC);
                Vendedor vendedor = new VendedorController().GetVendedor();

                builder.Append($"{ESC}")
                    .AppendLine("================================================")
                    .AppendLine($"{BoldOn}{empresa.NOMFANTA}{BoldOff}")
                    .AppendFormat("{0}{1} \n", empresa.DSCENDER, empresa.NROENDER == 0 ? "" : $", {empresa.NROENDER}", $"{empresa.NOMBAIRR}");
                if (municipioEmpresa != null)
                    builder.AppendLine($"{BoldOn}CIDADE: {BoldOff}{Format.RemoveAccents(municipioEmpresa.NOMMUNIC)} / {municipioEmpresa.CODUF}");
                builder.AppendLine($"{BoldOn}TELEFONE: {BoldOff}{Format.MaskFone(empresa.NROFONE)}")
                    .AppendLine($"{BoldOn}VENDEDOR:{BoldOff} {vendedor.NOMVEND}")
                    .AppendLine("================================================");


                string NROROMAN = $"ROMANEIO: {romaneio.NROROMAN}";
                string dthaber = $"ABERTURA: {romaneio.DATEMISS.ToString("dd/MM/yyyy")}";
                string dthfech = $"FECHAMENTO: {romaneio.DTHULTAT.ToString("dd/MM/yyyy")}";
                string resumo = "RESUMO DO ROMANEIO";

                List<RomaneioItem> itens = new RomaneioController().FindAll().OrderBy(p => p.DSCRPROD).ToList();

                builder.AppendLine($"{BoldOn}{NROROMAN.PadLeft(28)}{BoldOff}")
                    .AppendLine("\n")
                    .AppendLine($"{BoldOn}{dthaber}{BoldOff}{BoldOn}{dthfech.PadLeft(27)}{BoldOff}")
                    .AppendLine("\n")
                    .AppendLine($"{BoldOn}{resumo.PadLeft(32)}{BoldOff}")
                    .AppendLine("\n");

                builder.AppendLine($" {BoldOn}QTD.INI.{BoldOff} |{BoldOn} PRODUTO               {BoldOff} | {BoldOn}   SALDO {BoldOff} ")
                                       .AppendLine("__________|________________________|___________");

                foreach (var i in itens)
                {
                    var qtdInicial = i.QTDPROD;
                    var qtdDevol = i.QTDDEVCL;
                    var qtdVenda = i.QTDVENDA;
                    var qtdBrinde = i.QTDBRINDE;

                    Produto produto = new ProdutoController().FindById(i.CG_PRODUTO_ID);
                    if (produto != null)
                    {
                        string strQTDINICIAL = qtdInicial.ToString().PadLeft(9);
                        string firstBarra = "|";
                        string dscprod = produto.DSCPROD;
                        string saldo = (qtdInicial - (qtdVenda + qtdBrinde) + qtdDevol).ToString().PadLeft(8);
                        string barra = "|";

                        if (dscprod.Length >= 21)
                            dscprod = $" {dscprod.Substring(0, 21)}";
                        else
                        {
                            barra = barra.PadLeft(21 - dscprod.Length);
                            dscprod = $" {dscprod.PadLeft(1)} ";
                        }

                        builder.AppendLine($"{strQTDINICIAL} {firstBarra} {dscprod} {barra} {saldo} ");
                    }
                }

                builder.AppendLine("\n");
                builder.AppendLine(AuthorizationTemplateA7(""));

            }
            catch (Exception e)
            {
                SisLog.Logger(e.ToString());
            }

            return Format.RemoveAccents(builder.ToString());
        }
    }
}