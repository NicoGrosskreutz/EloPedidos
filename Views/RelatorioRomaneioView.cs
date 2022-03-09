using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Adapter;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;

namespace EloPedidos.Views
{
    [Activity(Label = "RelatorioEstoqueView")]
    public class RelatorioRomaneioView : Activity
    {
        private List<EstoqueAdapterCls> estoqueLista { get; set; } = null;
        private ListView listView;
        private TextInputEditText txPesquisa;
        private TextView txtNROROM, txtVENDEDOR, txtPLACA, txtVLRTOTAL, lblFLTSLDO;
        private CheckBox ckFILTRAR;
        private Button btnFechar;
        private List<BaixasPedido> openOrders;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_relatorioromaneio);

            listView = FindViewById<ListView>(Resource.Id.listView);
            txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
            txtNROROM = FindViewById<TextView>(Resource.Id.txtNROROM);
            txtVENDEDOR = FindViewById<TextView>(Resource.Id.txtVENDEDOR);
            txtPLACA = FindViewById<TextView>(Resource.Id.txtPLACA);
            txtVLRTOTAL = FindViewById<TextView>(Resource.Id.txtVLRTOTAL);
            ckFILTRAR = FindViewById<CheckBox>(Resource.Id.ckFILTRAR);
            lblFLTSLDO = FindViewById<TextView>(Resource.Id.lblFLTSLDO);
            btnFechar = FindViewById<Button>(Resource.Id.btnFechar);

            estoqueLista = new List<EstoqueAdapterCls>();

            ckFILTRAR.Checked = true;

            txPesquisa.TextChanged += (sender, args) => LoadListByDscr(txPesquisa.Text);

            ckFILTRAR.CheckedChange += (sender, args) => LoadByBalance();

            lblFLTSLDO.Click += (sender, args) =>
            {
                if (ckFILTRAR.Checked == true)
                    ckFILTRAR.Checked = false;
                else
                    ckFILTRAR.Checked = true;
            };

            string nome = new VendedorController().Vendedor.NOMVEND;
            txtVENDEDOR.Text = nome;

            var romaneio = new RomaneioController().FindLast();
            if (romaneio != null)
            {
                txtNROROM.Text = romaneio.NROROMAN.ToString();
                txtPLACA.Text = romaneio.IDTPLACA;
                txtVLRTOTAL.Text = LoadValorRomaneio().ToString("C");

                btnFechar.Visibility = Android.Views.ViewStates.Visible;
                if (romaneio.SITROMAN == (short)Romaneio.SitRoman.Fechado)
                    btnFechar.Text = "REIMPRIMIR RESUMO";
                else
                    btnFechar.Text = "FECHAR ROMANEIO";

                LoadByBalance();
            }
            else
            {
                btnFechar.Visibility = Android.Views.ViewStates.Invisible;
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO DO SISTEMA");
                builder.SetMessage("NÃO HÁ NENHUM ROMANEIO CARREGADO");
                builder.SetNeutralButton("OK", (sender, args) =>
                {
                    Intent i = new Intent(ApplicationContext, typeof(PedidoView));
                    StartActivity(i);
                    Finish();
                });
            }

            btnFechar.Click += (s, a) => FecharRomaneio();
            btnFechar.LongClick += ReabrirRomaneio;

            listView.ItemLongClick += (sender, args) =>
            {
                ListView listPedidos = new ListView(this);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);
                listPedidos.Adapter = adapter;
                adapter = (ArrayAdapter<string>)listPedidos.Adapter;

                var listAdapter = (RelatorioRomaneioAdapter)listView.Adapter;
                var itemSelected = listAdapter[args.Position];
                List<Pedido> pedidos = new PedidoDAO().FindAll();

                bool INDPEDVINC = false;
                bool INDVINC = false;
                foreach (Pedido pedido in pedidos)
                {
                    if (pedido.ES_ESTOQUE_ROMANEIO_ID == itemSelected.ES_ROMANEIO_ID)
                    {
                        INDPEDVINC = true;
                        List<ItemPedido> itemPedidos = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);

                        foreach (var i in itemPedidos)
                        {
                            if (i.CODPROD == itemSelected.CODPROD.ToString())
                            {
                                Produto prod = new ProdutoController().FindByCODPROD(long.Parse(i.CODPROD));
                                adapter.Add($"      Pedido: {pedido.NROPEDID} - QUANTIDADE: {i.QTDATPRO} - VALOR: {i.QTDATPRO * prod.PRCVENDA}");
                                INDVINC = true;
                            }
                        }
                    }
                }
                if (INDPEDVINC == false)
                    adapter.Add($"      NÃO HÁ NENHUM PEDIDO VINCULADO A ESSE ROMANEIO");
                else if (INDVINC == false)
                    adapter.Add($"      NÃO HÁ NENHUM PEDIDO VINCULADO A ESSE ITEM");


                listPedidos.Adapter = adapter;

                AdapterView.ItemLongClickEventArgs a = args;


                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle($"{new ProdutoController().FindByCODPROD(itemSelected.CODPROD).DSCPROD.ToUpper()}");
                builder.SetAdapter(adapter, (sender, args) =>
                {
                    //AdapterView.ItemClickEventArgs eventArgs = args
                    var position = args.Which;
                    var result = adapter.GetItem(position);
                    if (result != null)
                        if (!string.IsNullOrEmpty(result.ToString()))
                        {
                            Intent i = new Intent();
                            i.PutExtra("resultPedido", result.ToString().Split(" ")[7]);
                            SetResult(Result.Ok, i);
                            Finish();
                        }
                });
                builder.SetPositiveButton("CONCLUIDO", (s, e) =>
                {
                    return;
                });
                AlertDialog dialog = builder.Create();
                dialog.Show();
            };
        }

        private void FecharRomaneio(bool isChecked = false)
        {
            //Models.Config config = new ConfigController().GetConfig();
            //bool bloqroman = false;
            //if (!config.BLOQROMAN.HasValue)
            //	bloqroman = true;
            //else
            //	bloqroman = config.BLOQROMAN.Value;

            //if (VerificarLiberaçãoDeFechamento(out List<BaixasPedido> baixas) || isChecked || !bloqroman)
            //{
            if (btnFechar.Text.Contains("FECHAR"))
                new Utils.DialogFactory().CreateDialog(this,
                    "FECHAR ROMANEIO",
                    "GOSTARIA DE FECHAR O ROMANEIO ?",
                    "SIM",
                    () =>
                    {
                        var romaneio = new RomaneioController().FindLast();
                        romaneio.SITROMAN = (short)Romaneio.SitRoman.Fechado;
                        romaneio.DTHULTAT = DateTime.Now;
                        new RomaneioController().Save(romaneio);

                        this.Msg("ROMANEIO FECHADO");

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("IMPRESSÕES");
                        builder.SetMessage("GOSTARIA DE IMPRIMIR O RESUMO DO ROMANEIO ?");
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("SIM", (s, a) =>
                        {
                            string impressora = string.Empty;
                            if (this.RestorePreference("NOME") != "")
                                impressora = Ext.RestorePreference(this, "NOME");

                            Imprimir(impressora, romaneio);
                        });
                        builder.SetNegativeButton("NÂO", (s, a) => { return; });
                        AlertDialog dialog = builder.Create();
                        dialog.Show();

                        btnFechar.Text = "REIMPRIMIR RESUMO";
                    },
                    "NÂO",
                    () => { return; });

            else if (btnFechar.Text.Contains("REIMPRIMIR"))
                new Utils.DialogFactory().CreateDialog(this,
                "ROMANEIO",
                "GOSTARIA DE REIMPRIMIR O RESUMO DO ROMANEIO ?",
                "SIM",
                () =>
                {
                    var romaneio = new RomaneioController().FindLast();

                    string impressora = string.Empty;
                    if (this.RestorePreference("NOME") != "")
                        impressora = Ext.RestorePreference(this, "NOME");

                    Imprimir(impressora, romaneio);
                },
                "NÂO",
                () => { return; });
            //}
            //else
            //{
            //	new DialogFactory().CreateDialog(this,
            //		"ERRO AO FECHAR ROMANEIO",
            //		"AINDA HÁ PEDIDOS PARA REMARCAR O RETORNO",
            //		"REMARCAR AGORA",
            //		() => { RemarcarVencimento(baixas); },
            //		"VISUALIZAR PEDIDOS",
            //		() => { VisualizarPedidosAbertos(); },
            //		"CANCELAR",
            //		() => { return; });
            //}
        }

        public void RemarcarVencimento(List<BaixasPedido> baixas)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.dialog_remarcarVencimento, null, false);

            TextInputEditText txtDATARET = view.FindViewById<TextInputEditText>(Resource.Id.txtDATARET);
            Button btnSalvarREAG = view.FindViewById<Button>(Resource.Id.btnSalvar);
            CheckBox ckTODOS = view.FindViewById<CheckBox>(Resource.Id.ckTODOS);
            ListView listViewBaixa = view.FindViewById<ListView>(Resource.Id.listViewBaixa);

            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetCancelable(false);
            builder.SetView(view);
            builder.SetNeutralButton("FECHAR", (s, a) => { return; });
            AlertDialog dialog = builder.Create();
            dialog.Show();

            List<Pedido> pedidos = new List<Pedido>();
            List<Pedido> ordersToSave = new List<Pedido>();

            txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

            baixas.ForEach(b =>
            {
                var p = new PedidoController().FindById(b.FT_PEDIDO_ID.Value);
                if (p != null)
                    pedidos.Add(p);
            });

            if (pedidos.Count == 0)
            {
                dialog.Cancel();
                FecharRomaneio(true);
            }

            ckTODOS.CheckedChange += (s, a) =>
            {
                if (ckTODOS.Checked)
                {
                    ordersToSave.Clear();

                    var adapter = (AdapterListaPedidos)listViewBaixa.Adapter;

                    for (int i = 0; i < adapter.Count; i++)
                    {
                        var item = adapter[i];
                        listViewBaixa.SetItemChecked(i, true);
                        ordersToSave.Insert(i, item);
                    }
                }
                else
                {
                    ordersToSave.Clear();
                    var adapter = (AdapterListaPedidos)listViewBaixa.Adapter;
                    for (int i = 0; i < adapter.Count; i++)
                    {
                        listViewBaixa.SetItemChecked(i, false);
                    }
                }
            };

            listViewBaixa.Adapter = new AdapterListaPedidos(this, pedidos);

            listViewBaixa.ChoiceMode = ChoiceMode.Multiple;

            listViewBaixa.ItemClick += (s, a) =>
            {
                var adapter = (AdapterListaPedidos)listViewBaixa.Adapter;
                var item = adapter[a.Position];

                var isChecked = ordersToSave.Contains(item);

                if (!isChecked)
                {
                    listViewBaixa.SetItemChecked(a.Position, true);
                    ordersToSave.Add(item);
                }
                else
                {
                    ordersToSave.Remove(item);
                    listViewBaixa.SetItemChecked(a.Position, false);
                }
            };

            btnSalvarREAG.Click += (s, a) =>
            {
                if (!string.IsNullOrEmpty(txtDATARET.Text))
                    if (Format.DateToString(txtDATARET.Text, out string newData))
                    {
                        txtDATARET.Text = newData;

                        ordersToSave.ForEach(p =>
                        {
                            var baixa = new BaixasPedidoController().GerarBaixa(p);
                            new BaixasPedidoController().SalvarBaixaParcial(baixa, 0, out double resto, DateTime.Parse(newData));
                        });
                        if (ordersToSave.Count > 0)
                        {
                            dialog.Cancel();
                            this.SnackMsg("REAGENDAMENTO REALIZADO COM SUCESSO.");
                        }
                    }
            };
        }

        public void VisualizarPedidosAbertos()
        {
            if (openOrders != null)
            {
                if (openOrders.Count > 0)
                {
                    ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.simplelist, 0);
                    openOrders.ForEach(b =>
                    {
                        var pedido = new PedidoController().FindById(b.FT_PEDIDO_ID.Value);
                        if (pedido != null)
                        {
                            var cliente = new PessoaController().FindById(b.CG_PESSOA_ID.Value);
                            string pessoa = string.Empty;
                            if (cliente != null)
                            {
                                if (cliente.NOMFANTA.Length > 20)
                                    pessoa = cliente.NOMFANTA.Substring(0, 20);
                                else
                                    pessoa = cliente.NOMFANTA;
                            }

                            string str = $"Pedido: {pedido.NROPEDID} - {pessoa}";
                            adapter.Add(str);
                        }
                    });

                    ListView lv = new ListView(this);
                    lv.Adapter = adapter;

                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetCancelable(false);
                    builder.SetTitle("LISTA DE PEDIDOS");
                    builder.SetView(lv);
                    builder.SetNeutralButton("FECHAR", (s, a) => { return; });
                    AlertDialog dialog = builder.Create();
                    if (adapter.Count > 0)
                        dialog.Show();
                    else
                    {
                        dialog.Cancel();
                        this.Msg("NÃO FOI ENCONTRODO PEDIDOS EM ABERTOS");
                    }
                }
            }
        }

        private void Imprimir(string impressora, Romaneio romaneio)
        {
            if (impressora != string.Empty)
            {
                RunOnUiThread(() => this.Msg("ENVIANDO IMPRESSÃO PARA O DISPOSITIVO! AGUARDE..."));

                var printerController = new PrinterController();
                string text;

                text = printerController.FechamentoRomaneio(romaneio);

                string aux = text;

                var socket = printerController.GetSocket(impressora);

                if (socket != null)
                {
                    if (!socket.IsConnected)
                        printerController.ConnectPrinter(socket, impressora);
                    if (socket.IsConnected)
                    {
                        try
                        {
                            text = "ROMANEIO \n" + aux + "\n\n";
                            socket.OutputStream.Write(text.ToASCII(), 0, text.ToASCII().Length);
                        }
                        catch (Exception e)
                        {
                            SisLog.Logger(e.ToString());
                        }
                    }
                    else
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                AlertDialog.Builder b = new AlertDialog.Builder(this);
                                b.SetTitle("AVISO");
                                b.SetMessage("FAVOR, LIGUE A IMPRESSORA!\nTENTAR NOVAMENTE ?");
                                b.SetPositiveButton("SIM", (s, a) => { Imprimir(impressora, romaneio); });
                                b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                b.SetCancelable(false);
                                AlertDialog dialog = b.Create();
                                dialog.Show();
                            }
                            catch (Exception ex)
                            {
                                Log.Debug("ERROR_AD", ex.ToString());
                            }
                        });
                }
                else
                    RunOnUiThread(() =>
                    {
                        try
                        {
                            AlertDialog.Builder b = new AlertDialog.Builder(this);
                            b.SetTitle("AVISO");
                            b.SetMessage("IMPOSSÍVEL IMPRIMIR COM O DISPOSITIVO SELECIONADO!\nTENTAR NOVAMENTE ?");
                            b.SetPositiveButton("SIM", (s, a) => { Imprimir(impressora, romaneio); });
                            b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                            b.SetCancelable(false);
                            AlertDialog dialog = b.Create();
                            dialog.Show();
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("ERROR_AD", ex.ToString());
                        }
                    });
            }
            else
                RunOnUiThread(() =>
                {
                    try
                    {
                        AlertDialog.Builder b = new AlertDialog.Builder(this);
                        b.SetTitle("AVISO");
                        b.SetMessage("NENHUM DISPOSITIVO DE IMPRESSÃO SELECIONADO!\nTENTAR NOVAMENTE ?");
                        b.SetNeutralButton("OK", (s, a) => { return; });
                        b.SetCancelable(false);
                        AlertDialog dialog = b.Create();
                        dialog.Show();
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("ERROR_AD", ex.ToString());
                    }
                });
        }

        private void ReabrirRomaneio(object sender, EventArgs args)
        {
            var romaneio = new RomaneioController().FindLast();

            if (romaneio.SITROMAN == (short)Romaneio.SitRoman.Fechado)
                new Utils.DialogFactory().CreateDialog(this,
                    "ABRIR ROMANEIO",
                    "GOSTARIA DE REABRIR O ROMANEIO ?",
                    "SIM",
                    () =>
                    {
                        EditText editText = new EditText(this);

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("ABRIR ROMANEIO");
                        builder.SetMessage("DIGITE A SENHA PARA REABRIR O ROMANEIO");
                        builder.SetView(editText);
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("ABRIR", (s, a) =>
                        {
                            if (editText.Text.Equals("Mudar123"))
                            {
                                var romaneio = new RomaneioController().FindLast();
                                romaneio.SITROMAN = (short)Romaneio.SitRoman.Aberto;
                                new RomaneioController().Save(romaneio);

                                btnFechar.Text = "FECHAR ROMANEIO";

                                this.Msg("ROMANEIO ABERTO");
                            }
                            else
                                this.Msg("SENHA INCORRETA");
                        });
                        builder.SetNegativeButton("CANCELAR", (s, a) => { return; });
                        AlertDialog dialog = builder.Create();
                        dialog.Show();
                    },
                    "NÂO",
                    () => { return; });
        }
        private void LoadList()
        {
            this.listView.Adapter = null;
            estoqueLista.Clear();
            var prod = new RomaneioController().FindAll();
            var Roman = new RomaneioController().FindLast();
            prod.ForEach(i =>
            {
                var CODPROD = new ProdutoController().FindById(i.CG_PRODUTO_ID).CODPROD;
                estoqueLista.Add(new EstoqueAdapterCls()
                {
                    ES_ROMANEIO_ID = i.ES_ESTOQUE_ROMANEIO_ID,
                    CODPROD = CODPROD.Value,
                    DSCPROD = i.DSCRPROD,
                    QTDSALDO = (i.QTDPROD + i.QTDDEVCL - i.QTDVENDA - i.QTDBRINDE).ToString()
                });
            });

            var adapter = new RelatorioRomaneioAdapter(this, estoqueLista);
            this.listView.Adapter = adapter;
        }

        private void LoadListByDscr(string dscr)
        {
            if (ckFILTRAR.Checked == false)
            {
                listView.Adapter = null;
                estoqueLista.Clear();
                var prod = new RomaneioController().FindBy_NOMPROD(dscr);
                prod.ForEach(i =>
                {
                    var CODPROD = new ProdutoController().FindById(i.CG_PRODUTO_ID).CODPROD;
                    estoqueLista.Add(new EstoqueAdapterCls()
                    {
                        ES_ROMANEIO_ID = i.ES_ESTOQUE_ROMANEIO_ID,
                        CODPROD = CODPROD.Value,
                        DSCPROD = i.DSCRPROD,
                        QTDSALDO = (i.QTDPROD + i.QTDDEVCL - i.QTDVENDA - i.QTDBRINDE).ToString()
                    });
                });

                var adapter = new RelatorioRomaneioAdapter(this, estoqueLista);
                listView.Adapter = adapter;
            }
            else
                LoadByBalance();
        }
        private void LoadByBalance()
        {
            if (ckFILTRAR.Checked == true)
            {
                listView.Adapter = null;
                estoqueLista.Clear();
                var conn = Database.GetConnection();
                var prod = new RomaneioController().FindAll();

                prod.ForEach(i =>
                {
                    if ((i.QTDPROD + i.QTDDEVCL - i.QTDVENDA) > 0)
                    {
                        if (txPesquisa.Text == "")
                        {
                            var CODPROD = new ProdutoController().FindById(i.CG_PRODUTO_ID).CODPROD;
                            estoqueLista.Add(new EstoqueAdapterCls()
                            {
                                ES_ROMANEIO_ID = i.ES_ESTOQUE_ROMANEIO_ID,
                                CODPROD = CODPROD.Value,
                                DSCPROD = i.DSCRPROD,
                                QTDSALDO = (i.QTDPROD + i.QTDDEVCL - i.QTDVENDA - i.QTDBRINDE).ToString()
                            });
                        }
                        else
                        {
                            if (i.DSCRPROD.StartsWith(txPesquisa.Text.ToUpper()))
                            {
                                var CODPROD = new ProdutoController().FindById(i.CG_PRODUTO_ID).CODPROD;
                                estoqueLista.Add(new EstoqueAdapterCls()
                                {
                                    ES_ROMANEIO_ID = i.ES_ESTOQUE_ROMANEIO_ID,
                                    CODPROD = CODPROD.Value,
                                    DSCPROD = i.DSCRPROD,
                                    QTDSALDO = (i.QTDPROD + i.QTDDEVCL - i.QTDVENDA - i.QTDBRINDE).ToString()
                                });
                            }
                        }
                    }
                });

                var adapter = new RelatorioRomaneioAdapter(this, estoqueLista);
                listView.Adapter = adapter;
            }
            else if (txPesquisa.Text != "")
                LoadListByDscr(txPesquisa.Text);
            else
                LoadList();
        }
        private double LoadValorRomaneio()
        {
            double vlr = 0;
            var prod = new RomaneioController().FindAll();
            prod.ForEach(i =>
            {
                double vlrTotal = i.QTDPROD * i.PRCVENDA;
                vlr += vlrTotal;
            });
            return vlr;
        }

        private bool VerificarLiberaçãoDeFechamento(out List<BaixasPedido> b)
        {
            bool result = true;

            List<BaixasPedido> baixas = new Controllers.BaixasPedidoController().FindByDatRet(DateTime.Now);
            if (baixas.Count > 0)
            {
                openOrders = baixas;
                result = false;
            }

            b = baixas;
            return result;
        }
    }
}