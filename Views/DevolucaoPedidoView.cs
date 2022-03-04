using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using EloPedidos.Adapter;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using Android.Bluetooth;
using Android.Support.V7.Widget;
using SkiaSharp;
using System.Threading;
using Android.Support.Design.Widget;
using Android.Graphics.Drawables;


namespace EloPedidos.Views
{
    [Activity(Label = "DevolucaoPedidoView")]
    public class DevolucaoPedidoView : Activity
    {
        private ListView listView, listViewBaixa, listDevice;
        private RelativeLayout rLayout;
        private EditText txDevice;
        private TextInputEditText txCODPESS, txData, txQTDPROD, txVLRBAIXA, txtDATARET;
        private Button btnLimpar, btnImprimir, btnSalvar, btnGravar;
        private TextView lbSaldo, lblNROPED, lblVLRREC, lblNOMPESS, lblSLDREC, txDSCPROD, lblPEDIDO;
        private CardView cv_radius;
        private ImageButton btnSINC;
        private ProgressBar progressBar;
        //private CheckBox ckTODOS;

        private System.Timers.Timer timer = null;
        bool connection = false;
        List<BaixasPedido> baixas = null;
        List<ItemPedido> itensPedidos = null;
        List<DevolucaoItem> itensDevolucao = null;
        private List<BluetoothDevice> Devices { get; set; } = null;
        public BluetoothAdapter Adapter { get { return BluetoothAdapter.DefaultAdapter; } }
        private Pedido SelectedOrder = null;
        private ItemPedido SelectedIten = null;
        private long? SelectedItenPosition = null;
        private long? SelectedOrderPosition = null;
        private bool permissionToBack = true;

        /// <summary>
        /// USADO PARA CONTROLAR O QUE ESTÁ SENDO MANDADO PARA IMPRESSÃO, 0 = LIVRE, 1 = AGUARDAR, 2 = CANCELADO
        /// </summary>
        private int ControleDeImpressoes { get; set; } = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_devolucao);
            LoadView();


            ConfigController configController = new ConfigController();
            PedidoController pController = new PedidoController();

            Task.Run(() =>
            {
                TestConection();
                timer = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
                timer.Elapsed += (s, a) => this.TestConection();
                timer.Enabled = true;
                timer.AutoReset = true;

                timer.Start();
            });

            progressBar.Visibility = ViewStates.Invisible;
            txDSCPROD.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
            txCODPESS.Focusable = true;
            txDSCPROD.Focusable = false;
            txCODPESS.Focusable = false;
            txDevice.Focusable = false;
            txData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

            if (this.RestorePreference("NOME") != "")
                txDevice.Text = Ext.RestorePreference(this, "NOME");

            btnSINC.Visibility = ViewStates.Invisible;
            btnSINC.Enabled = false;

            baixas = new List<BaixasPedido>();
            itensPedidos = new List<ItemPedido>();
            itensDevolucao = new List<DevolucaoItem>();

            txCODPESS.LongClick += (sender, eventArgs) =>
            {
                if (new PessoaDAO().FindAll().Count == 0)
                    this.Msg("NÃO HÁ CLIENTES CADASTRADOS!");
                else
                {
                    ListView listView;
                    TextInputEditText txPesquisa;
                    CheckBox ckMUNIC, ckINDINAT;
                    TextView txMUNIC, txINDINAT;

                    View view = this.LayoutInflater.Inflate(Resource.Layout.activity_buscacliente, null);

                    listView = view.FindViewById<ListView>(Resource.Id.listView);
                    txPesquisa = view.FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
                    ckMUNIC = view.FindViewById<CheckBox>(Resource.Id.ckMUNIC);
                    ckINDINAT = view.FindViewById<CheckBox>(Resource.Id.ckINDINAT);
                    txMUNIC = view.FindViewById<TextView>(Resource.Id.txMUNIC);
                    txINDINAT = view.FindViewById<TextView>(Resource.Id.txINDINAT);

                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetView(view);
                    builder.SetNeutralButton("CONCLUIDO", (s, a) => { return; });
                    AlertDialog dialog = builder.Create();
                    dialog.Show();

                    ShowKeyboard(txPesquisa);
                    txPesquisa.RequestFocus();

                    int MUNIC_ID = 0;
                    Municipio munic = null;
                    if (new ConfigController().GetConfig().CODMUNPQ != 0)
                    {
                        MUNIC_ID = new ConfigController().GetConfig().CODMUNPQ;
                        munic = new MunicipioController().FindById(MUNIC_ID);
                        string municipio = new MunicipioController().FindNameById(new ConfigController().GetConfig().CODMUNPQ);

                        if (municipio != null)
                        {
                            txMUNIC.Text = municipio.ToUpper();
                            ckMUNIC.Checked = true;
                            ckMUNIC.Visibility = ViewStates.Visible;
                        }
                        else
                        {
                            ckMUNIC.Checked = false;
                            ckMUNIC.Visibility = ViewStates.Invisible;
                            txMUNIC.Text = string.Empty;
                        }
                    }
                    else
                    {
                        ckMUNIC.Checked = false;
                        ckMUNIC.Visibility = ViewStates.Invisible;
                        txMUNIC.Text = string.Empty;
                    }
                    ckINDINAT.Checked = false;

                    List<Pessoa> clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);

                    var adapter = new AdapterBuscarCliente(this, clientes);
                    listView.Adapter = adapter;

                    txMUNIC.Click += (sender, args) =>
                    {
                        if (ckMUNIC.Checked == true)
                            ckMUNIC.Checked = false;
                        else
                            ckMUNIC.Checked = true;
                    };
                    txINDINAT.Click += (sender, args) =>
                    {
                        if (ckINDINAT.Checked == true)
                            ckINDINAT.Checked = false;
                        else
                            ckINDINAT.Checked = true;
                    };

                    txPesquisa.TextChanged += (s, a) =>
                    {
                        if (ckMUNIC.Checked)
                            clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
                        else
                            clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

                        var adapter = new AdapterBuscarCliente(this, clientes);
                        listView.Adapter = adapter;
                    };

                    ckMUNIC.CheckedChange += (s, a) =>
                    {
                        if (ckMUNIC.Checked)
                            clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
                        else
                            clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

                        var adapter = new AdapterBuscarCliente(this, clientes);
                        listView.Adapter = adapter;
                    };

                    ckINDINAT.CheckedChange += (s, a) =>
                    {
                        if (ckMUNIC.Checked)
                            clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
                        else
                            clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

                        var adapter = new AdapterBuscarCliente(this, clientes);
                        listView.Adapter = adapter;
                    };


                    listView.ItemClick += (s, a) =>
                    {
                        var adapter = (AdapterBuscarCliente)listView.Adapter;
                        var cliente = adapter[a.Position];
                        if (cliente.CODPESS != null)
                        {
                            txCODPESS.Text = cliente.CODPESS.ToString();
                            dialog.Cancel();
                        }
                    };
                }
            };

            txCODPESS.TextChanged += (s, a) =>
            {
                if (txCODPESS.Text != "")
                {
                    baixas.Clear();
                    LoadListViewBaixa(txCODPESS.Text.ToLong());
                    LoadSaldo(baixas);
                }
            };

            txtDATARET.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txtDATARET.Text))
                {
                    SelectText(txtDATARET);
                    if (!txtDATARET.HasFocus)
                    {
                        if (Format.DateToString(txtDATARET.Text, out string newDate))
                            txtDATARET.Text = newDate;
                        else
                        {
                            txtDATARET.Text = newDate;
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("AVISO DO SISTEMA!");
                            builder.SetMessage("FORMATO DA DATA ESTÁ INCORRETO\n(DD/MM/AAAA)");
                            builder.SetCancelable(false);
                            builder.SetNeutralButton("OK", (s, a) =>
                            {
                                txtDATARET.RequestFocus();
                                return;
                            });
                            AlertDialog dialog = builder.Create();
                            dialog.Show();
                        }
                    }
                    txtDATARET.SetSelectAllOnFocus(true);
                }

                if (txtDATARET.HasFocus)
                    DataPickerDialog(txtDATARET);
            };

            btnSINC.Click += (sender, args) =>
            {
                if (txCODPESS.Text != "")
                    sincronizarPedidos();
                else
                    this.Msg("FAVOR INFORMAR O CLIENTE !");
            };

            listViewBaixa.ItemClick += (s, args) =>
            {
                var adapter = (AdapterListaBaixas)listViewBaixa.Adapter;
                var pedido = adapter[args.Position];
                var ped = new PedidoController().FindById(pedido.FT_PEDIDO_ID.Value);
                SelectedOrder = ped;
                SelectedOrderPosition = args.Position;
                txDSCPROD.Text = "";
                txQTDPROD.Text = "";
                lblNROPED.Text = "";
                lblVLRREC.Text = "";
                txVLRBAIXA.Text = "";
                txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

                lblSLDREC.Text = "SALDO A RECEBER DO CLIENTE";

                LoadListView(ped.NROPEDID);
                LoadListViewBaixa(txCODPESS.Text.ToLong());
                LoadSaldo(baixas);
            };

            txDevice.LongClick += (sender, args) =>
            {
                loadDevices();

                listDevice = new ListView(this);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);
                listDevice.Adapter = adapter;
                adapter = (ArrayAdapter<string>)listDevice.Adapter;
                if (Devices != null)
                    Devices.ForEach((aux) => adapter.Add(aux.Name));

                listDevice.Adapter = adapter;

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("SELECIONE UMA IMPRESSORA");
                builder.SetAdapter(adapter, (sender, args) =>
                {
                    string nameDevice = adapter.GetItem(args.Which);
                    LoadDevice(nameDevice);
                });
                builder.SetPositiveButton("CONCLUIDO", (s, e) => { return; });
                AlertDialog dialog = builder.Create();
                dialog.Show();
            };

            listView.ItemClick += (s, a) =>
            {
                var adapter = (AdapterItensDevolucao)listView.Adapter;
                var item = adapter[a.Position];
                SelectedIten = item;
                SelectedItenPosition = a.Position;
                txDSCPROD.Text = item.NOMPROD;
                ShowKeyboard(txQTDPROD);
            };
            btnLimpar.Click += (s, a) => LimparTela();

            txData.FocusChange += (s, a) =>
            {
                if (!txData.HasFocus)
                {
                    if (Utils.Format.DateToString(txData.Text, out string newData))
                        txData.Text = newData;
                    else
                        this.Msg("DATA INVÁLIDA");
                }

                if (txData.HasFocus)
                    DataPickerDialog(txData);
            };

            txData.LongClick += (s, a) => DataPickerDialog(txData);
            txtDATARET.LongClick += (s, a) => DataPickerDialog(txtDATARET);

            txData.FocusChange += (s, a) =>
            {
                if (!txData.HasFocus)
                {
                    if (Utils.Format.DateToString(txData.Text, out string newData))
                        txData.Text = newData;
                    else
                        this.Msg("DATA INVÁLIDA");
                }

                if (txData.HasFocus)
                    DataPickerDialog(txData);
            };

            txData.LongClick += (s, a) => DataPickerDialog(txData);
            txtDATARET.LongClick += (s, a) => DataPickerDialog(txtDATARET);

            btnImprimir.Click += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txCODPESS.Text))
                {
                    var cliente = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());
                    if (cliente != null)
                    {
                        List<Pedido> pedidos = new List<Pedido>();
                        baixas.ForEach(b =>
                        {
                            var p = new PedidoController().FindById(b.FT_PEDIDO_ID.Value);
                            if (p != null) pedidos.Add(p);
                        });

                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("IMPRESSÕES");
                        builder.SetMessage("GOSTARIA DE IMPRIMIR OS PEDIDOS DESSE CLEINTE ?");
                        builder.SetPositiveButton("SIM", (s, a) => { ImprimirDialog(pedidos); });
                        builder.SetNegativeButton("NÃO", (s, a) => { return; });
                        AlertDialog dialog = builder.Create();
                        dialog.Show();
                    }
                }
            };

            txVLRBAIXA.FocusChange += (sender, args) =>
            {
                if (!args.HasFocus)
                {
                    HideKeyboard(txVLRBAIXA);
                    if (txVLRBAIXA.Text.Length > 0)
                        txVLRBAIXA.SetSelectAllOnFocus(true);
                }
            };

            btnGravar.Click += (sender, args) => GravarItem();

            txQTDPROD.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txQTDPROD.Text))
                    txQTDPROD.SetSelectAllOnFocus(true);
            };
            txCODPESS.FocusChange += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txCODPESS.Text))
                    txCODPESS.SetSelectAllOnFocus(true);
            };

            btnSalvar.Click += (sender, args) =>
            {
                if (new RomaneioController().FindLast() == null)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("AVISO");
                    builder.SetMessage("ERRO AO SALVAR AS DEVOLUÇÕES, FAVOR CARREGUE O ROMANEIO");
                    builder.SetCancelable(false);
                    builder.SetNeutralButton("OK", (s, a) => { return; });
                    AlertDialog dialog = builder.Create();
                    dialog.Show();
                }
                else if (new RomaneioController().FindLast().SITROMAN == (short)Romaneio.SitRoman.Fechado)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("AVISO");
                    builder.SetMessage("ERRO AO SALVAR AS DEVOLUÇÕES, O ROMANEIO ESTÁ FECHADO");
                    builder.SetCancelable(false);
                    builder.SetNeutralButton("OK", (s, a) => { return; });
                    AlertDialog dialog = builder.Create();
                    dialog.Show();
                }
                else
                    new DialogFactory().CreateDialog(this,
                            "AVISO DO SISTEMA",
                            "DESEJA SALVAR ?",
                            "SIM",
                            () =>
                            {
                                Task.Run(() =>
                                {
                                    RunOnUiThread(() =>
                                    {
                                        RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);
                                        if (!string.IsNullOrEmpty(txData.Text) && Utils.Format.DateToString(txData.Text, out string newData))
                                        {
                                            RunOnUiThread(() => txData.Text = newData);
                                            if (!string.IsNullOrEmpty(txtDATARET.Text) && Utils.Format.DateToString(txtDATARET.Text, out newData))
                                            {
                                                RunOnUiThread(() => txtDATARET.Text = newData);
                                                try
                                                {
                                                    var adapter = (AdapterListaBaixas)listViewBaixa.Adapter;
                                                    if (adapter.Count > 0)
                                                    {
                                                        //USADO PARA AUXILIAR AQUILO QUE SERÁ IMPRESSO
                                                        List<Pedido> pedidosAlterados = new List<Pedido>();
                                                        bool firstSave = false;

                                                        double pagamento = 0;
                                                        if (!string.IsNullOrEmpty(txVLRBAIXA.Text))
                                                            pagamento = txVLRBAIXA.Text.ToDouble();

                                                        bool HasDevolução = (itensDevolucao.Count > 0) ? true : false;

                                                        Thread t = new Thread(() =>
                                                        {
                                                            if (HasDevolução)
                                                                itensDevolucao.ForEach(d =>
                                                                {
                                                                    if (Salvar(d.FT_PEDIDO_ITEM_ID.Value, d))
                                                                    {
                                                                        if (!firstSave)
                                                                        {
                                                                            Pedido ped = new PedidoController().FindById(d.FT_PEDIDO_ID.Value);
                                                                            pedidosAlterados.Add(ped);
                                                                            firstSave = true;
                                                                        }
                                                                    }
                                                                });
                                                        });
                                                        t.Start();
                                                        t.Join();

                                                        if (pagamento > 0)
                                                            SalvarBaixa(HasDevolução, pedidosAlterados);
                                                        else
                                                        {
                                                            try
                                                            {
                                                                Pessoa cliente = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());
                                                                List<Pedido> pedidos = new PedidoController().FindOpenOrderByCG_PESSOA_ID(cliente.CG_PESSOA_ID.Value).OrderBy(p => p.DATEMISS).ToList();

                                                                Database.RunInTransaction(() =>
                                                                {
                                                                    pedidos.ForEach(p =>
                                                                    {
                                                                        bool isAtt = false;

                                                                        pedidosAlterados.ForEach(ped => { if (p.NROPEDID == ped.NROPEDID) isAtt = true; });
                                                                        if (!isAtt)
                                                                            pedidosAlterados.Add(p);

                                                                        BaixasPedido baixa = new BaixasPedidoController().GerarBaixa(p);
                                                                        //new BaixasPedidoController().SalvarBaixaParcial(baixa, 0, out double resto, DateTime.Parse(txtDATARET.Text));
                                                                        new BaixasPedidoController().SalvarBaixa(baixa, DateTime.Parse(txtDATARET.Text));
                                                                    });

                                                                    FinalizarSalva(false, HasDevolução, pedidos.Count > 0, pedidosAlterados);
                                                                });
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                SisLog.Logger(e.ToString(), "SALVAR_REAGENDAMENTO");
                                                                AlertDialog.Builder b = new AlertDialog.Builder(this);
                                                                b.SetTitle("AVISO");
                                                                b.SetMessage("ERRO AO FAZER REAGENDAMENTO");
                                                                b.SetNeutralButton("OK", (s, a) =>
                                                                {
                                                                    txtDATARET.SetSelectAllOnFocus(true);
                                                                    txtDATARET.RequestFocus();
                                                                    return;
                                                                });
                                                                b.SetCancelable(false);
                                                                AlertDialog dialog = b.Create();
                                                                dialog.Show();
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        AlertDialog.Builder builder2 = new AlertDialog.Builder(this);
                                                        builder2.SetTitle("AVISO DO SISTEMA");
                                                        builder2.SetMessage("NENHUM PEDIDO ENCONTRADO");
                                                        builder2.SetNeutralButton("OK", (s, a) => { return; });
                                                        AlertDialog dialog2 = builder2.Create();
                                                        dialog2.Show();
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log.Error(Utils.Ext.LOG_APP, ex.ToString());
                                                    SisLog.Logger("BOTAO_SALVAR", ex.ToString());
                                                }
                                            }
                                            else
                                            {
                                                AlertDialog.Builder b = new AlertDialog.Builder(this);
                                                b.SetTitle("AVISO");
                                                b.SetMessage("A DATA DO RETORNO ESTÁ INCORRETA");
                                                b.SetNeutralButton("OK", (s, a) =>
                                                {
                                                    txtDATARET.SetSelectAllOnFocus(true);
                                                    txtDATARET.RequestFocus();
                                                    return;
                                                });
                                                b.SetCancelable(false);
                                                AlertDialog dialog = b.Create();
                                                dialog.Show();
                                            }
                                        }
                                        else
                                        {
                                            AlertDialog.Builder b = new AlertDialog.Builder(this);
                                            b.SetTitle("AVISO");
                                            b.SetMessage("A DATA DO MOVIMENTO ESTÁ INCORRETA");
                                            b.SetNeutralButton("OK", (s, a) =>
                                            {
                                                txData.SetSelectAllOnFocus(true);
                                                txData.RequestFocus();
                                                return;
                                            });
                                            b.SetCancelable(false);
                                            AlertDialog dialog = b.Create();
                                            dialog.Show();
                                        }
                                    });

                                    RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);
                                });
                            },
                            "NÃO",
                            () => { });
            };

            listView.Selected = true;
            listView.SetSelector(Resource.Drawable.selector);

            listViewBaixa.Selected = true;
            listViewBaixa.SetSelector(Resource.Drawable.selector);

            if (Intent.HasExtra("CG_PESSOA_ID"))
            {
                try
                {
                    string extra = Intent.GetStringExtra("CG_PESSOA_ID");
                    long codpress = new PessoaController().FindById(extra.Split(" ")[0].ToLong()).CODPESS.Value;
                    txCODPESS.Text = codpress.ToString();

                    if (listViewBaixa.Count > 0)
                    {
                        int i = 0;
                        var adapter = (BaixaPedidoAdapter)listViewBaixa.Adapter;
                        var pedido = adapter[i];
                        for (bool aux = false; aux == false; i++)
                        {
                            pedido = adapter[i];
                            if (pedido.NROPEDID == extra.Split(" ")[1].ToLong())
                                aux = true;
                        }

                        //lblPEDIDO.Text = "Pedido: " + pedido.NROPEDID.ToString();
                        txDSCPROD.Text = "";
                        txQTDPROD.Text = "";
                        lblSLDREC.Text = "SALDO A RECEBER DO CLIENTE";
                        txVLRBAIXA.Text = "";
                        txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

                        btnSINC.Visibility = ViewStates.Visible;
                        btnSINC.Enabled = true;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(Utils.Ext.LOG_APP, e.ToString());
                }
            }

            if (Intent.HasExtra("CODPESS"))
            {
                try
                {
                    var cod = Intent.GetStringExtra("CODPESS");

                    if (!string.IsNullOrEmpty(cod))
                    {
                        Pessoa pessoa = new PessoaController().FindByCODPESS(cod.ToLong());
                        if (pessoa != null)
                            txCODPESS.Text = pessoa.CODPESS.ToString();
                    }
                }
                catch (Exception e)
                {
                    SisLog.Logger(Utils.Ext.LOG_APP, e.ToString());
                }
            }
        }
        public void FinalizarSalva(bool _BAIXASALVA, bool _DEVOLSALVA, bool REAGENDAMENTO = false, List<Pedido> pedidos = null)
        {
            try
            {
                var pessoa = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());
                if (!_BAIXASALVA && !_DEVOLSALVA && !REAGENDAMENTO)
                {
                    AlertDialog.Builder builderErro = new AlertDialog.Builder(this);
                    builderErro.SetTitle("AVISO DO SISTEMA");
                    builderErro.SetMessage("ERRO AO SALVAR ! VERIFIQUE");
                    builderErro.SetNeutralButton("OK", (s, a) => { return; });
                    builderErro.SetCancelable(false);
                    AlertDialog dialogErro = builderErro.Create();
                    dialogErro.Show();
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("IMPRESSÕES !!!");

                    if (_BAIXASALVA == true && _DEVOLSALVA == true)
                        builder.SetMessage("O PAGAMENTO E AS DEVOLUÇÕES FORAM SALVAS COM SUCESSO !!! GOSTARIA DE IMPRIMIR ?");
                    else if (_BAIXASALVA == true && _DEVOLSALVA == false)
                        builder.SetMessage("O PAGAMENTO FOI SALVO COM SUCESSO !!! GOSTARIA DE IMPRIMIR ?");
                    else if (_BAIXASALVA == false && _DEVOLSALVA == true)
                        builder.SetMessage("AS DEVOLUÇÕES FORAM SALVAS COM SUCESSO !!! GOSTARIA DE IMPRIMIR ?");
                    else if (!_BAIXASALVA && !_DEVOLSALVA && REAGENDAMENTO)
                        builder.SetMessage("VENCIMENTO ALTERADO COM SUCESSO!!! GOSTARIA DE IMPRIMIR ?");

                    builder.SetPositiveButton("SIM", (s, a) =>
                    {
                        try
                        {
                            ImprimirDialog(pedidos);
                        }
                        catch (Exception e)
                        {
                            SisLog.Logger(e.ToString());
                            this.Msg("FALHA NA IMPRESSÃO, TENTE NOVAMENTE");
                        }

                        LimparTela();
                        this.SelectedOrder = null;
                    });
                    builder.SetNegativeButton("NÃO", (s, a) =>
                    {
                        LimparTela();
                        return;
                    });

                    builder.SetCancelable(false);
                    AlertDialog dialog = builder.Create();
                    dialog.Show();
                }
            }
            catch (Exception ex)
            {
                SisLog.Logger("FINALIZAR SALVAR", ex.ToString());
            }
        }

        /// <summary>
        ///  Define uma mensagem se houver troco e ação após confirmação
        /// </summary>
        /// <param name="troco"></param>
        /// <param name="action"></param>
        public void MsgTroco(double troco, Action action)
        {
            if (troco > 0)
            {
                new DialogFactory().CreateDialog(this,
                    "AVISO DO SISTEMA",
                    $"TROCO: R$ {troco.FormatDouble()}",
                    "OK",
                    delegate { action(); });
            }
            else
                action();
        }
        /// <summary>
        ///  Define uma mensagem informando a quantidade faltate para abater o pedido.
        /// </summary>
        /// <param name="troco"></param>
        /// <param name="action"></param>
        public void MsgRest(double vlr, Action action)
        {
            new DialogFactory().CreateDialog(this,
            "AVISO DO SISTEMA",
             $"VALOR RESTANTE PARA ABATIMENTO: R$ {vlr.FormatDouble()}",
             "OK",
             delegate { action(); });
        }

        /// <summary>
        /// SALVA A BAIXA
        /// </summary>
        public void SalvarBaixa(bool INDDEVOL, List<Pedido> pedidosAlterados = null)
        {
            bool auxSAVE = true;
            bool isWait = false;
            List<long> ids = new List<long>();
            try
            {
                double VLRPAGAR = 0;
                var adapter = (AdapterListaBaixas)listViewBaixa.Adapter;

                int count = listViewBaixa.Adapter.Count;
                int i = 0;

                Pedido pedido = null;

                Pessoa cliente = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());

                if (cliente != null)
                {
                    var listaPedidos = new PedidoController().FindOpenOrderByCG_PESSOA_ID(cliente.CG_PESSOA_ID.Value);
                    if (listaPedidos.Count > 0)
                        foreach (var aux in listaPedidos)
                        {
                            var _baixa = new BaixasPedidoController().GerarBaixa(aux);
                            if (_baixa.VLRRECBR > 0)
                            {
                                pedido = aux;
                                break;
                            }
                        }
                }

                if (pedido != null)
                {
                    double _VLRPGMT = 0;
                    double _VLRRECBR = 0;

                    BaixasPedidoController bController = new BaixasPedidoController();

                    BaixasPedido baixa = bController.GerarBaixa(pedido);

                    if (!string.IsNullOrEmpty(txVLRBAIXA.Text))
                        _VLRPGMT = txVLRBAIXA.Text.ToDouble(); // Valor recebido para pagamento

                    _VLRRECBR = baixa.VLRRECBR; // Valor a receber

                    // se o valor recebido para pagamento for maior que o valor a receber do pedido :
                    if (_VLRPGMT > _VLRRECBR)
                    {
                        if (_VLRRECBR > 0)
                        {
                            isWait = true;

                            new DialogFactory().CreateDialog(this,
                                "AVISO DO SISTEMA",
                                "O VALOR DE RECEBIMENTO É MAIOR QUE O TOTAL DO PEDIDO!" +
                                "\nEM QUAL PEDIDO GOSTARIA DE FAZER A BAIXA ?",
                                $"PEDIDO ATUAL ({pedido.NROPEDID})",
                                () =>
                                {
                                    try
                                    {
                                        baixa.DATPGMT = DateTime.Now;
                                        baixa.DATVCTO = DateTime.Now;
                                        bController.SalvarBaixaTotal(baixa, _VLRPGMT, out double troco);
                                        MsgTroco(troco, () => { return; });

                                        bool isAtt = false;

                                        pedidosAlterados.ForEach(p =>
                                        {
                                            if (pedido.NROPEDID == p.NROPEDID)
                                                isAtt = true;
                                        });

                                        if (!isAtt)
                                            pedidosAlterados.Add(pedido);

                                        FinalizarSalva(true, INDDEVOL, false, pedidosAlterados);
                                    }
                                    catch (Exception e)
                                    {
                                        FinalizarSalva(false, INDDEVOL);
                                        SisLog.Logger(e.ToString());
                                    }
                                },
                                "TODOS OS PEDIDOS",
                                () =>
                                {
                                    try
                                    {
                                        isWait = false; /// => BOOLEAN CRIADO PARA COTROLAR OS ALERTDIALOG, CASO SEJA TRUE, SÓ IRÁ EXECULTAR A FUNÇÃO (FINALIZARSALVA) APÓS A APRESENTAÇÃO DO ALERTDIALOG

                                        List<Pedido> pedidos = new PedidoController().FindOpenOrderByCG_PESSOA_ID(cliente.CG_PESSOA_ID.Value).OrderBy(p => p.DATEMISS).ToList();

                                        VLRPAGAR = _VLRPGMT;

                                        foreach (Pedido p in pedidos)
                                        {
                                            if (VLRPAGAR > 0)
                                            {
                                                bool isAtt = false;

                                                pedidosAlterados.ForEach(ped => { if (p.NROPEDID == ped.NROPEDID) isAtt = true; });

                                                if (!isAtt)
                                                    pedidosAlterados.Add(p);


                                                baixa = bController.GerarBaixa(p);
                                                _VLRRECBR = baixa.VLRRECBR;

                                                ids.Add(p.FT_PEDIDO_ID.Value);

                                                double resto = 0;

                                                baixa.DATPGMT = DateTime.Now;

                                                if (VLRPAGAR >= _VLRRECBR)
                                                    bController.SalvarBaixaTotal(baixa, VLRPAGAR, out resto);
                                                else
                                                {
                                                    double vlrSobra = _VLRRECBR - VLRPAGAR;
                                                    if (vlrSobra >= 5)
                                                        bController.SalvarBaixaParcial(baixa, VLRPAGAR, out resto, DateTime.Parse(txtDATARET.Text));
                                                    else
                                                    {
                                                        isWait = true;

                                                        new DialogFactory().CreateDialog(this,
                                                            "AVISO!",
                                                            $"PEDIDO: {p.NROPEDID}\nGOSTARIA DE FAZER A BAIXA TOTAL E DAR R$ {vlrSobra.ToString("N2")} DE DESCONTO ? ",
                                                            "SIM",
                                                            () =>
                                                            {
                                                                bController.SalvarBaixaTotal(baixa, VLRPAGAR, out resto);

                                                                auxSAVE = true;
                                                                FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
                                                            },
                                                            "NÃO",
                                                            () =>
                                                            {
                                                                bController.SalvarBaixaParcial(baixa, VLRPAGAR, out resto, DateTime.Parse(txtDATARET.Text));

                                                                auxSAVE = true;
                                                                FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
                                                            });

                                                        break;
                                                    }
                                                }

                                                VLRPAGAR = resto;
                                            }
                                            else
                                                break;
                                        }

                                        if (!isWait)
                                        {
                                            if (VLRPAGAR > 0)
                                                MsgTroco(VLRPAGAR, () => { return; });

                                            auxSAVE = true;

                                            FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        FinalizarSalva(false, INDDEVOL, false, pedidosAlterados);
                                        SisLog.Logger(e.ToString());
                                    }
                                },
                                "CANCELAR",
                                () =>
                                {
                                    auxSAVE = false;
                                    FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
                                });
                        }
                        else
                        {
                            auxSAVE = false;
                            this.Msg("NÃO HÁ VALOR A RECEBER !");
                        }
                    }
                    else
                    {
                        bool isAtt = false;

                        pedidosAlterados.ForEach(p =>
                        {
                            if (pedido.NROPEDID == p.NROPEDID)
                                isAtt = true;
                        });

                        if (!isAtt)
                            pedidosAlterados.Add(pedido);

                        try
                        {
                            double vlrRest = _VLRRECBR - _VLRPGMT;

                            if (vlrRest > 5)
                            {
                                bController.SalvarBaixaParcial(baixa, _VLRPGMT, out _VLRPGMT, DateTime.Parse(txtDATARET.Text));
                                MsgRest(vlrRest, () => { return; });
                                auxSAVE = true;
                            }
                            else if (vlrRest == 0)
                            {
                                bController.SalvarBaixaTotal(baixa, _VLRPGMT, out _VLRPGMT);
                                auxSAVE = true;
                            }
                            else
                            {
                                isWait = true;

                                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                builder.SetTitle("AVISO DO SISTEMA");
                                builder.SetMessage($"GOSTARIA DE FAZER A BAIXA TOTAL E DAR R$ {vlrRest.ToString("N2")} DE DESCONTO ? ");
                                builder.SetCancelable(false);

                                builder.SetPositiveButton("SIM", (s, a) =>
                                {
                                    bController.SalvarBaixaTotal(baixa, _VLRPGMT, out _VLRPGMT);

                                    auxSAVE = true;
                                    FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
                                });

                                builder.SetNegativeButton("NÃO", (s, a) =>
                                {
                                    bController.SalvarBaixaParcial(baixa, _VLRPGMT, out _VLRPGMT, DateTime.Parse(txtDATARET.Text));
                                    MsgRest(vlrRest, () => { return; });

                                    auxSAVE = true;
                                    FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
                                });

                                AlertDialog dialog = builder.Create();
                                dialog.Show();
                                auxSAVE = true;
                            }
                        }
                        catch (Exception e)
                        {
                            FinalizarSalva(false, INDDEVOL, false, pedidosAlterados);
                            SisLog.Logger(e.ToString());
                        }
                    }

                }
                else
                    auxSAVE = false;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString());
                SisLog.Logger("SALVAR_PAGAMENTO", ex.ToString());
            }
            finally
            {
                if (!isWait)
                    FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
            }
        }

        /// <summary>
        /// SALVA A DEVOLUÇÃO
        /// </summary>
        protected bool Salvar(long pFT_PEDIDO_ITEM_ID, DevolucaoItem devolucao)
        {
            try
            {
                bool auxSAVE = false;
                var CODPROD = new ItemPedidoController().FindById(pFT_PEDIDO_ITEM_ID).CODPROD;
                Produto p = new ProdutoController().FindByCODPROD(long.Parse(CODPROD));
                //var item = new ItemPedidoController().FindById(pFT_PEDIDO_ITEM_ID);
                Pedido pedido = new PedidoController().FindById(devolucao.FT_PEDIDO_ID.Value);

                if (CODPROD != null)
                {
                    Database.GetConnection().RunInTransaction(() =>
                    {
                        Produto prod = new ProdutoController().FindByCODPROD(long.Parse(CODPROD));
                        GeolocatorController geoController = new GeolocatorController();
                        BaixasPedidoController bController = new BaixasPedidoController();

                        var item = itensPedidos.Where(i => i.FT_PEDIDO_ITEM_ID.Value == devolucao.FT_PEDIDO_ITEM_ID.Value).FirstOrDefault();
                        item.DTHULTAT = DateTime.Now;
                        item.QTDDEVOL = 0;
                        new ItemPedidoController().Save(item);
                        new DevolucaoItemController().SaveItemDevolucao(devolucao);

                        BaixasPedido baixa = bController.GerarBaixa(pedido);
                        bController.SalvarBaixa(baixa, DateTime.Parse(txtDATARET.Text));

                        geoController.SaveDevolutionLocalization(devolucao.FT_PEDIDO_ITEM_DEVOLUCAO_ID.Value);

                        RomaneioItem romItem = new RomaneioController().FindByIdItem(prod.CG_PRODUTO_ID.Value);
                        if (romItem != null)
                        {
                            romItem.QTDDEVCL += devolucao.QTDDEVOL;
                            RomaneioController eController = new RomaneioController();
                            eController.SaveItem(romItem);
                        }
                        else
                        {
                            long id = 1;
                            if (new RomaneioController().getLastId() != null)
                                id = new RomaneioController().getLastId().Value + 1;
                            else
                            {
                                for (long i = 1; i <= 100; i++)
                                {
                                    if (new RomaneioController().FindById(i) == null)
                                    {
                                        id = i;
                                        break;
                                    }
                                }
                            }

                            RomaneioItem romaneioItem = new RomaneioItem()
                            {
                                ES_ESTOQUE_ROMANEIO_ITEM_ID = id,
                                CG_PRODUTO_ID = devolucao.CG_PRODUTO_ID.Value,
                                DSCRPROD = devolucao.NOMPROD,
                                ES_ESTOQUE_ROMANEIO_ID = new RomaneioController().FindLast().ES_ESTOQUE_ROMANEIO_ID,
                                PRCVENDA = item.VLRUNIT,
                                USRULTAT = new OperadorController().Operador.USROPER,
                                DTHULTAT = DateTime.Parse(txData.Text),
                                QTDBRINDE = 0,
                                QTDCONT = 0,
                                QTDDEVCL = devolucao.QTDDEVOL,
                                QTDVENDA = 0,
                                QTDPROD = 0
                            };
                            new RomaneioController().SaveItem(romaneioItem);
                        }
                        auxSAVE = true;
                    });
                }

                return auxSAVE;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString());
                SisLog.Logger("SALVAR DEVOLUCAO", ex.ToString());
                return false;
            }
        }
        private void GravarItem()
        {
            if (SelectedIten == null)
                return;
            if (!txDSCPROD.Text.IsEmpty())
            {
                if (!txQTDPROD.Text.IsEmpty())
                {
                    Pedido pedido = null;

                    if (SelectedOrder != null)
                        pedido = SelectedOrder;

                    if (pedido != null)
                    {
                        if (pedido.IDTFRMPG.Equals("1"))
                        {
                            var item = new ItemPedidoController().FindById(SelectedIten.FT_PEDIDO_ITEM_ID.Value);
                            var nompess = new PessoaController().FindById(pedido.ID_PESSOA.Value).NOMPESS;
                            DevolucaoItemController dController = new DevolucaoItemController();
                            var qntdevol = "0";
                            qntdevol = txQTDPROD.Text;

                            if ((item.QTDATPRO - qntdevol.ToDouble()) >= 0)
                            {
                                Produto prod = new ProdutoController().FindByCODPROD(long.Parse(SelectedIten.CODPROD));
                                bool isUpdate = false;

                                DevolucaoItem devolucao = new DevolucaoItem()
                                {
                                    CG_VENDEDOR_ID = new VendedorController().Vendedor.CG_VENDEDOR_ID,
                                    CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
                                    FT_PEDIDO_ITEM_ID = SelectedIten.FT_PEDIDO_ITEM_ID,
                                    CG_PRODUTO_ID = SelectedIten.CG_PRODUTO_ID,
                                    FT_PEDIDO_ID = pedido.FT_PEDIDO_ID.Value,
                                    NROPEDIDO = pedido.NROPEDID,
                                    NOMPESS = nompess,
                                    CODPROD = SelectedIten.CODPROD.ToLong(),
                                    DATDEVOL = DateTime.Now,
                                    QTDDEVOL = qntdevol.ToDouble(),
                                    DTHULTAT = DateTime.Now,
                                    USRULTAT = new OperadorController().Operador.USROPER,
                                    NOMPROD = SelectedIten.NOMPROD,
                                    INDSINC = false,
                                    PositionIten = SelectedItenPosition.Value,
                                    Produto = prod
                                };
                                if (itensDevolucao.Count > 0)
                                {
                                    for (int i = 0; i < itensDevolucao.Count; i++)
                                    {
                                        if (itensDevolucao[i].PositionIten == devolucao.PositionIten)
                                        {
                                            itensDevolucao.RemoveAt(i);
                                            itensDevolucao.Insert(i, devolucao);
                                            isUpdate = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                    itensDevolucao.Add(devolucao);

                                if (!isUpdate)
                                {
                                    itensDevolucao.Add(devolucao);
                                    itensPedidos[int.Parse(SelectedItenPosition.Value.ToString())].QTDATPRO -= qntdevol.ToDouble();
                                    itensPedidos[int.Parse(SelectedItenPosition.Value.ToString())].QTDDEVOL = qntdevol.ToDouble();
                                }
                                else
                                {
                                    item.QTDATPRO -= qntdevol.ToDouble();
                                    item.QTDDEVOL = qntdevol.ToDouble();
                                    itensPedidos.RemoveAt(int.Parse(SelectedItenPosition.Value.ToString()));
                                    itensPedidos.Insert(int.Parse(SelectedItenPosition.Value.ToString()), item);
                                }

                                if (!ValidarSaldoReceber())
                                {
                                    this.Msg("VALOR A DEVOLVER ULTRAPASSA O VALOR A RECEBER");
                                    var aux = new ItemPedidoController().FindById(SelectedIten.FT_PEDIDO_ITEM_ID.Value);
                                    itensDevolucao.Remove(devolucao);
                                    itensPedidos.RemoveAt(int.Parse(SelectedItenPosition.Value.ToString()));
                                    itensPedidos.Insert(int.Parse(SelectedItenPosition.Value.ToString()), aux);
                                }

                                foreach (var b in baixas)
                                {
                                    double valorDevolvido = 0;
                                    if (b.FT_PEDIDO_ID.Value == pedido.FT_PEDIDO_ID.Value)
                                    {
                                        itensDevolucao.ForEach(d =>
                                        {
                                            var item = itensPedidos[int.Parse(d.PositionIten.ToString())];
                                            valorDevolvido += d.QTDDEVOL * item.VLRUNIT;
                                        });

                                        b.VLRDEVOL += valorDevolvido;
                                        b.VLRRECBR = new BaixasPedidoController().PrevisaoReceber(itensPedidos);
                                        break;
                                    }
                                }

                                LoadListViewBaixa(txCODPESS.Text.ToLong(), true, baixas);
                                LoadSaldo(baixas);

                                var adapter = new AdapterItensDevolucao(this, itensPedidos, itensDevolucao);
                                listView.Adapter = adapter;

                                txDSCPROD.Text = "";
                                txQTDPROD.Text = "";

                                SelectedItenPosition = null;
                                SelectedIten = null;
                            }
                            else
                                this.Msg("QUANTIDADE A DEVOLVER ULTRAPASSA O SALDO ATUAL");

                            HideKeyboard(txQTDPROD);
                        }
                        else if (pedido.IDTFRMPG.Equals("0"))
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("AVISO");
                            builder.SetMessage("PEDIDO SALVO COMO PAGAMENTO A VISTA !\nNÃO É POSSIVÉL FAZER DEVOLUÇÕES PARA UM PEDIDO PAGO");
                            builder.SetNeutralButton("OK", (s, a) => { return; });
                            builder.SetCancelable(false);
                            AlertDialog dialog = builder.Create();
                            dialog.Show();
                        }
                    }
                }
                else
                    this.Msg("INFORMAR A QUANTIDADE A SER DEVOLVIDA!");
            }
            else
                this.Msg("INFORMAR A PRODUTO A SER DEVOLVIDA!");
        }

        public bool ValidarSaldoReceber()
        {
            bool result = true;
            double valorDevolvido = 0;

            Pedido pedido = SelectedOrder;
            if (pedido != null)
            {
                itensDevolucao.ForEach(d =>
                {
                    var item = itensPedidos[int.Parse(d.PositionIten.ToString())];
                    valorDevolvido += d.QTDDEVOL * item.VLRUNIT;
                });

                BaixasPedido baixa = new BaixasPedidoController().GerarBaixa(pedido);
                if (baixa.TOTLPEDID == valorDevolvido && baixa.VLRPGMT == 0)
                    result = true;
                else
                {
                    double auxReceber = new BaixasPedidoController().PrevisaoReceber(itensPedidos);
                    if (auxReceber.ToString().StartsWith("-"))
                        result = false;
                }
            }
            else
                result = false;

            return result;
        }

        private void Imprimir(Pedido p)
        {
            Task.Run(() =>
            {
                try
                {
                    var printerController = new PrinterController();
                    string text = string.Empty;
                    text = printerController.FormatOrderForPrintA7(new PedidoController().FindById(p.FT_PEDIDO_ID.Value), "devolucao");

                    SisLog.Logger($"GEROU O TEXTO", "Imprimir");

                    string aux = text;
                    string segVIA = text;
                    BluetoothSocket socket = printerController.GetSocket(txDevice.Text);

                    SisLog.Logger($"INSTANSIOU O SOCKET", "Imprimir");
                    SisLog.Logger($"{socket != null}", "Imprimir");

                    if (socket != null)
                    {
                        if (!socket.IsConnected)
                            printerController.ConnectPrinter(socket, txDevice.Text);

                        SisLog.Logger($"{socket.IsConnected}", "Imprimir");

                        if (socket.IsConnected)
                        {
                            bool isImpressaoOk = true;
                            try
                            {
                                RunOnUiThread(() => this.Msg("ENVIANDO IMPRESSÃO PARA O DISPOSITIVO! AGUARDE..."));

                                SisLog.Logger($"IMPRIMINDO A 1° VIA", "Imprimir");

                                text = "1 VIA \n" + aux + "\n\n";
                                socket.OutputStream.Write(text.ToASCII(), 0, text.ToASCII().Length);

                                SisLog.Logger($"IMPRIMIU A 1° VIA", "Imprimir");
                            }
                            catch (Exception ex)
                            {
                                isImpressaoOk = false;
                                RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        AlertDialog.Builder b = new AlertDialog.Builder(this);
                                        b.SetTitle("AVISO");
                                        b.SetMessage("FALHA AO IMPRIMIR!\nTENTAR NOVAMENTE ?");
                                        b.SetPositiveButton("SIM", (s, a) => { Imprimir(p); });
                                        b.SetNegativeButton("CANCELAR", (s, a) => { ControleDeImpressoes = 0; return; });
                                        b.SetCancelable(false);
                                        AlertDialog dialog = b.Create();
                                        dialog.Show();
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Debug("ERROR", ex.ToString());
                                        ControleDeImpressoes = 0;
                                    }
                                });

                                SisLog.Logger(ex.ToString(), "IMPRESSAO");
                            }

                            if (isImpressaoOk)
                                RunOnUiThread(() =>
                                {
                                    SisLog.Logger($"ENTRANDO NO DIALOG PARA 2° VIA", "Imprimir");
                                    try
                                    {
                                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                        builder.SetTitle("IMPRESSÕES !!!");
                                        builder.SetMessage("GOSTARIA DE IMPRIMIR A 2° VIA ?");
                                        builder.SetPositiveButton("SIM", (s, a) =>
                                        {
                                            try
                                            {
                                                RunOnUiThread(() => this.Msg("ENVIANDO IMPRESSÃO PARA O DISPOSITIVO! AGUARDE..."));

                                                SisLog.Logger($"IMPRIMINDO A 2° VIA", "Imprimir");

                                                text = "2 VIA \n" + segVIA + "\n\n";
                                                if (socket != null)
                                                    socket.OutputStream.Write(text.ToASCII(), 0, text.ToASCII().Length);

                                                SisLog.Logger($"IMPRIMIU A 2° VIA", "Imprimir");

                                                printerController.ClosePrinter();

                                                SisLog.Logger($"FECHOU O SOCKET", "Imprimir");

                                                ControleDeImpressoes = 0;
                                            }
                                            catch (Exception ex)
                                            {
                                                RunOnUiThread(() =>
                                                {
                                                    try
                                                    {
                                                        AlertDialog.Builder b = new AlertDialog.Builder(this);
                                                        b.SetTitle("AVISO");
                                                        b.SetMessage("FALHA AO IMPRIMIR!\nTENTAR NOVAMENTE ?");
                                                        b.SetPositiveButton("SIM", (s, a) => { Imprimir(p); });
                                                        b.SetNegativeButton("CANCELAR", (s, a) => { ControleDeImpressoes = 0; return; });
                                                        b.SetCancelable(false);
                                                        AlertDialog dialog = b.Create();
                                                        dialog.Show();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log.Debug("ERRO_IMPRESSAO", ex.ToString());
                                                    }
                                                });

                                                SisLog.Logger(ex.ToString(), "ERRO_IMPRESSAO");
                                                ControleDeImpressoes = 0;
                                            }
                                        });
                                        builder.SetNegativeButton("NÃO", (s, a) =>
                                        {
                                            ControleDeImpressoes = 0;
                                            if (socket.IsConnected)
                                                printerController.ClosePrinter();

                                            return;
                                        });
                                        AlertDialog dialog = builder.Create();
                                        dialog.Show();
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Debug("ERRO_IMPRESSAO", ex.ToString());
                                        ControleDeImpressoes = 0;
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
                                    b.SetMessage("FAVOR, LIGUE A IMPRESSORA!\nTENTAR NOVAMENTE ?");
                                    b.SetPositiveButton("SIM", (s, a) => { Imprimir(p); });
                                    b.SetNegativeButton("CANCELAR", (s, a) => { ControleDeImpressoes = 0; return; });
                                    b.SetCancelable(false);
                                    AlertDialog dialog = b.Create();
                                    dialog.Show();
                                }
                                catch (Exception ex)
                                {
                                    ControleDeImpressoes = 0;
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
                                b.SetPositiveButton("SIM", (s, a) => { Imprimir(p); });
                                b.SetNegativeButton("CANCELAR", (s, a) => { ControleDeImpressoes = 0; return; });
                                b.SetCancelable(false);
                                AlertDialog dialog = b.Create();
                                dialog.Show();
                            }
                            catch (Exception ex)
                            {
                                ControleDeImpressoes = 0;
                            }
                        });

                }
                catch (Exception ex)
                {
                    RunOnUiThread(() =>
                    {
                        SisLog.Logger(ex.ToString(), "ERRO_IMPRESSAO");
                    });
                    ControleDeImpressoes = 0;
                }
            });
        }
        private void ImprimirDialog(List<Pedido> pedidos)
        {
            try
            {
                if (!string.IsNullOrEmpty(txDevice.Text))
                {
                    try
                    {
                        ControleDeImpressoes = 0;
                        if (pedidos.Count > 0)
                            Task.Run(() =>
                            {
                                permissionToBack = false;
                                for (int i = 0; i < pedidos.Count; i++)
                                {
                                    SisLog.Logger($"MANDANDO PARA IMPRESSAO {i}° PEDIDO", "ImprimirDialog");
                                    Pedido p = pedidos[i];
                                    try
                                    {
                                        Task.Run(() =>
                                        {
                                            if (ControleDeImpressoes == 1)
                                            {
                                                for (int y = 0; y < 20; i++)
                                                {
                                                    if (ControleDeImpressoes == 0)
                                                        break;
                                                    else
                                                        Thread.Sleep(1000);
                                                }
                                            }


                                            if (ControleDeImpressoes == 0)
                                            {
                                                ControleDeImpressoes = 1;
                                                Task.Run(() =>
                                                {
                                                    RunOnUiThread(() =>
                                                    {
                                                        SisLog.Logger($"ENTROU NO RunOnUiThread", "ImprimirDialog");

                                                        try
                                                        {
                                                            new DialogFactory().CreateDialog(this,
                                                            "IMPRESSÕES !!!",
                                                            $"GOSTARIA DE IMPRIMIR O PEDIDO {p.NROPEDID} ?",
                                                            "SIM",
                                                            () =>
                                                            {
                                                                Task.Run(() => Imprimir(p));
                                                            },
                                                            "NÃO",
                                                            () => { ControleDeImpressoes = 0; });
                                                        }
                                                        catch (Exception e)
                                                        {
                                                            SisLog.Logger(e.ToString(), "ImprimirDialog");
                                                        }

                                                    });
                                                });
                                            }
                                        });

                                        if (i + 1 == pedidos.Count)
                                            permissionToBack = true;
                                    }
                                    catch (Exception e)
                                    {
                                        permissionToBack = true;
                                        ControleDeImpressoes = 0;
                                        RunOnUiThread(() =>
                                        {
                                            SisLog.Logger(e.ToString(), "ImprimirDialog_03");
                                        });
                                    }
                                }
                            });
                    }
                    catch (Exception e)
                    {
                        RunOnUiThread(() =>
                        {
                            try
                            {
                                AlertDialog.Builder b = new AlertDialog.Builder(this);
                                b.SetTitle("AVISO");
                                b.SetMessage("FAVOR, LIGUE A IMPRESSORA!\nTENTAR NOVAMENTE ?");
                                b.SetPositiveButton("SIM", (s, a) => { ImprimirDialog(pedidos); });
                                b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                b.SetCancelable(false);
                                AlertDialog dialog = b.Create();
                                dialog.Show();
                            }
                            catch (Exception ex)
                            {
                            }

                            SisLog.Logger(e.ToString(), "ImprimirDialog_02");
                        });
                    }
                }
                else
                    RunOnUiThread(() =>
                    {
                        AlertDialog.Builder b = new AlertDialog.Builder(this);
                        b.SetTitle("AVISO");
                        b.SetMessage("NENHUM DISPOSITIVO DE IMPRESSÃO SELECIONADO!");
                        b.SetNeutralButton("OK", (s, a) => { return; });
                        b.SetCancelable(false);
                        AlertDialog dialog = b.Create();
                        dialog.Show();

                    });
            }
            catch (Exception e)
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        AlertDialog.Builder b = new AlertDialog.Builder(this);
                        b.SetTitle("AVISO");
                        b.SetMessage("FAVOR, LIGUE A IMPRESSORA!\nTENTAR NOVAMENTE ?");
                        b.SetPositiveButton("SIM", (s, a) => { ImprimirDialog(pedidos); });
                        b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                        b.SetCancelable(false);
                        AlertDialog dialog = b.Create();
                        dialog.Show();
                    }
                    catch (Exception ex)
                    {
                    }

                    SisLog.Logger(e.ToString(), "ImprimirDialog_01");
                });
            }
        }

        protected void LimparTela()
        {
            baixas.Clear();
            itensPedidos.Clear();
            itensDevolucao.Clear();
            SelectedItenPosition = null;
            SelectedOrderPosition = null;
            SelectedOrder = null;
            SelectedIten = null;
            listView.Adapter = null;
            listViewBaixa.Adapter = null;
            txData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbSaldo.Text = "R$ 0,00";
            txCODPESS.Text = "";
            txDSCPROD.Text = "";
            txQTDPROD.Text = "";
            txVLRBAIXA.Text = "";
            lblNROPED.Text = "";
            lblPEDIDO.Text = "";
            lblVLRREC.Text = "";
            lblNOMPESS.Text = "";
            lblSLDREC.Text = "SALDO A RECEBER DO CLIENTE";
            btnSINC.Visibility = ViewStates.Invisible;
            btnSINC.Enabled = false;
        }

        private void LoadDevice(string name)
        {
            new DialogFactory().CreateDialog(context: this,
                            title: "AVISO DO SISTEMA",
                            message: $"DESEJA DEFINIR {name} COMO IMPRESSORA PADRÃO?",
                            positiveButtonName: "SIM",
                            positiveAction: () =>
                            {
                                ConfigController configController = new ConfigController();
                                Models.Config config = configController.GetConfig();

                                config.NOMIMPRE = name;
                                configController.Save(config);
                                txDevice.Text = name;

                                this.SavePreference(name, "NOME");
                            },
                            negativeButtonName: "NÃO",
                            negativeAction: () => { return; });
        }

        protected override void OnDestroy()
        {
            if (timer != null)
                timer.Stop();
            base.OnDestroy();
        }
        public override void OnBackPressed()
        {
            if (permissionToBack)
                base.OnBackPressed();
            else
                this.Msg("Enviando para a impressora...");
        }

        public void loadDevices()
        {
            Devices = new List<BluetoothDevice>();
            if (Adapter.StartDiscovery())
                Devices = Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
            else
                Devices = Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
        }
        public void LoadListView(long pNROPEDID)
        {
            try
            {
                if (pNROPEDID.ToString() != "0")
                {
                    if (listViewBaixa.Count > 0)
                    {
                        itensPedidos.Clear();
                        listView.Adapter = null;
                        var pedido = new PedidoController().FindByNROPEDID(pNROPEDID);
                        if (pedido != null)
                        {
                            var itens = new ItemPedidoController().FindAllOrderItems(pedido.FT_PEDIDO_ID.Value);
                            itensPedidos = itens;

                            var adapter = new AdapterItensDevolucao(this, itensPedidos);
                            listView.Adapter = adapter;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString());
            }
        }

        protected virtual void sincronizarPedidos()
        {
            bool loop = false;

            new Thread(() =>
            {
                try
                {
                    new Sincronizador().SincronizarAllNotSync();

                    var controller = new ConfigController();
                    var config = controller.GetConfig();
                    DNS dns = controller.GetDNS();
                    Empresa empresa = new EmpresaController().GetEmpresa();
                    double vendedor = new VendedorController().GetVendedor().CG_VENDEDOR_ID.Value;

                    int count = 0;
                    loop = true;

                    RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);

                    Task.Run(() =>
                    {
                        while (loop)
                        {
                            progressBar.SetProgress(count, true);
                            Thread.Sleep(150);
                            count++;
                        }
                    });

                    RunOnUiThread(() => EnableView(false));

                    Database.RunInTransaction(() =>
                    {
                        if (!string.IsNullOrEmpty(dns.Host) && dns.Port != 0)
                        {
                            if (new ConfigController().TestServerConnection())
                            {
                                long cliente = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong()).CG_PESSOA_ID.Value;
                                PedidoController pController = new PedidoController();
                                Empresa empresa = new EmpresaController().GetEmpresa();
                                double vendedor = new VendedorController().GetVendedor().CG_VENDEDOR_ID.Value;

                                if (pController.ComSocketclientOrders($"CARGAPEDIDO{empresa.CODEMPRE}{vendedor.ToString("0000")}{cliente.ToString("000000")}          000000"))
                                    this.Msg("PEDIDOS SINCRONIZADOS COM SUCESSO !");
                                else
                                    this.Msg("FALHA AO SINCRONIZAR PEDIDOS !");
                            }
                            else
                                this.Msg("SEM CONEXÃO COM O SERVIDOR !");
                        }
                    });

                }
                catch (Exception ex)
                {
                    Log.Error(Utils.Ext.LOG_APP, ex.ToString());
                }
                finally
                {
                    RunOnUiThread(() =>
                    {
                        EnableView();
                        progressBar.Visibility = ViewStates.Invisible;
                        //LoadListViewBaixa(txCODPESS.Text.ToLong(), ckTODOS.Checked);
                        LoadListViewBaixa(txCODPESS.Text.ToLong());

                    });
                }
            }).Start();
        }
        protected void EnableView(bool enable = true)
        {
            txCODPESS.Enabled = enable;
            txData.Enabled = false;
            txDSCPROD.Enabled = enable;
            txQTDPROD.Enabled = enable;
            txVLRBAIXA.Enabled = enable;
            txtDATARET.Enabled = enable;
            txDevice.Enabled = enable;
            listView.Enabled = enable;
            listViewBaixa.Enabled = enable;
            btnImprimir.Enabled = enable;
            btnSalvar.Enabled = enable;
            btnGravar.Enabled = enable;
            btnLimpar.Enabled = enable;
            btnSINC.Enabled = enable;
        }
        private void DataPickerDialog(TextInputEditText view)
        {
            int dia = DateTime.Now.Day;
            int mes = DateTime.Now.Month - 1;
            int ano = DateTime.Now.Year;

            if (!string.IsNullOrEmpty(view.Text))
                if (Validations.DateValidator(view.Text))
                {
                    dia = view.Text.Substring(0, 2).ToInt();
                    mes = view.Text.Substring(3, 2).ToInt() - 1;
                    ano = view.Text.Substring(6, 4).ToInt();
                }
            new DatePickerDialog(this, (s, a) =>
            {
                var day = a.DayOfMonth.ToString("D2");
                var month = (a.MonthOfYear + 1).ToString("D2");
                var year = a.Year;

                string date = $"{day}/{month}/{year}";

                if (Validations.DateValidator(date))
                    view.Text = date;

            }, ano, mes, dia).Show();
        }
        public void SelectText(EditText editText)
        {
            if (editText.IsFocused == true)
                editText.SelectAll();
        }
        private void LoadSaldo(List<BaixasPedido> _baixas)
        {
            double saldo = 0;
            _baixas.ForEach(b => { saldo += b.VLRRECBR; });
            lbSaldo.Text = saldo.ToString("C2");
        }

        public void LoadListViewBaixa(long pCODPESS, bool isAll = false, List<BaixasPedido> listaBaixas = null)
        {
            try
            {
                if (listaBaixas != null)
                {
                    var adapter = new AdapterListaBaixas(this, listaBaixas);
                    listViewBaixa.Adapter = adapter;
                    if (SelectedOrderPosition.HasValue && listViewBaixa.Adapter.Count >= SelectedOrderPosition.Value - 1)
                        listViewBaixa.SetItemChecked(int.Parse(SelectedOrderPosition.ToString()), true);
                }
                else if (pCODPESS.ToString() != "0")
                {
                    baixas.Clear();
                    lblPEDIDO.Text = "";
                    var cliente = new PessoaController().FindByCODPESS(pCODPESS);

                    if (cliente != null)
                    {
                        lblNOMPESS.Text = cliente.NOMFANTA;
                        listViewBaixa.Adapter = null;

                        BaixasPedidoController baixasPedidoController = new BaixasPedidoController();
                        //if (!isAll)
                        //{
                        //    Pedido pedido = new PedidoController().getLastPedido(cliente.CG_PESSOA_ID.Value);
                        //    baixas.Add(baixasPedidoController.GerarBaixa(pedido));
                        //}
                        //else
                        //{
                        List<Pedido> pedidos = new PedidoController().FindOpenOrderByCG_PESSOA_ID(cliente.CG_PESSOA_ID.Value);
                        pedidos.ForEach(pa =>
                        {
                            if (baixasPedidoController.GerarBaixa(pa).VLRRECBR > 0)
                            {
                                var b = baixasPedidoController.GerarBaixa(pa);
                                if (b.FT_PEDIDO_BAIXA_ID.HasValue)
                                    baixas.Add(b);
                                else
                                {
                                    baixasPedidoController.Save(b);
                                    baixas.Add(b);
                                }
                            }
                        });
                        //}

                        if (baixas.Count > 0)
                        {
                            var adapter = new AdapterListaBaixas(this, baixas);
                            listViewBaixa.Adapter = adapter;
                        }
                        else
                            listViewBaixa.Adapter = null;
                    }

                    txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

                    btnSINC.Visibility = ViewStates.Visible;
                    btnSINC.Enabled = true;
                    if (SelectedOrderPosition.HasValue && listViewBaixa.Adapter.Count >= SelectedOrderPosition.Value - 1)
                        listViewBaixa.SetItemChecked(int.Parse(SelectedOrderPosition.ToString()), true);
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString());
            }
        }

        private List<Pessoa> BuscarClientes(string nome, bool indinat, Municipio municipio = null)
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            PessoaController pController = new PessoaController();
            pessoas = pController.FindAll();
            try
            {
                if (!string.IsNullOrEmpty(nome))
                    pessoas = pController.FindByName(nome);
                if (municipio != null)
                    pessoas = pessoas.Where(p => p.CODMUNIC == municipio.CODMUNIC).ToList();

                if (indinat)
                    pessoas = pessoas.Where(p => p.INDINAT).ToList();

                return pessoas;
            }
            catch (Exception e)
            {
                return pessoas;
            }
        }

        public static void HideKeyboard(EditText editText)
        {
            InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, HideSoftInputFlags.None);
        }
        public static void ShowKeyboard(EditText editText)
        {
            editText.RequestFocus();

            InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(editText, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
            editText.SelectAll();

        }
        private bool TestConection()
        {
            if (new ConfigController().TestServerConnection() != true)
            {
                connection = false;
                RunOnUiThread(() => cv_radius.SetCardBackgroundColor(Android.Graphics.Color.ParseColor("Red")));
                return false;
            }
            else
            {
                connection = true;
                RunOnUiThread(() => cv_radius.SetCardBackgroundColor(Android.Graphics.Color.ParseColor("Green")));
                return true;
            }
        }
        private void LoadView()
        {
            txCODPESS = FindViewById<TextInputEditText>(Resource.Id.txCODPESS);
            txData = FindViewById<TextInputEditText>(Resource.Id.txData);
            txDSCPROD = FindViewById<TextView>(Resource.Id.txDSCPROD);
            txQTDPROD = FindViewById<TextInputEditText>(Resource.Id.txQTDPROD);
            txVLRBAIXA = FindViewById<TextInputEditText>(Resource.Id.txVLRBAIXA);
            txtDATARET = FindViewById<TextInputEditText>(Resource.Id.txtDATARET);
            txDevice = FindViewById<EditText>(Resource.Id.txDevice);
            listView = FindViewById<ListView>(Resource.Id.listView);
            listViewBaixa = FindViewById<ListView>(Resource.Id.listViewBaixa);
            rLayout = FindViewById<RelativeLayout>(Resource.Id.relativeLayout);
            btnImprimir = FindViewById<Button>(Resource.Id.btnImprimir);
            btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
            btnGravar = FindViewById<Button>(Resource.Id.btnGravar);
            btnLimpar = FindViewById<Button>(Resource.Id.btnLimpar);
            lbSaldo = FindViewById<TextView>(Resource.Id.lbSaldo);
            lblSLDREC = FindViewById<TextView>(Resource.Id.lblSLDREC);
            lblNROPED = FindViewById<TextView>(Resource.Id.lblNROPED);
            lblVLRREC = FindViewById<TextView>(Resource.Id.lblVLRREC);
            lblPEDIDO = FindViewById<TextView>(Resource.Id.lblPEDIDO);
            lblNOMPESS = FindViewById<TextView>(Resource.Id.lblNOMPESS);
            cv_radius = FindViewById<CardView>(Resource.Id.cv_radius);
            btnSINC = FindViewById<ImageButton>(Resource.Id.btnSINC);
            //ckTODOS = FindViewById<CheckBox>(Resource.Id.ckTODOS);
            //txtTODOS = FindViewById<TextView>(Resource.Id.txtTODOS);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
        }
    }
}