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
    [Activity(Label = "DevolucaoPedidoViewOld")]
    public class DevolucaoPedidoViewOld : Activity
    {
        #region Var

        private ListView listView, listViewBaixa, listDevice;
        private RelativeLayout rLayout;
        private EditText txDevice;
        private TextInputEditText txCODPESS, txData, txQTDPROD, txVLRBAIXA, txtDATARET;
        private Button btnLimpar, btnImprimir, btnSalvar, btnGravar;
        private TextView lbSaldo, lblNROPED, lblVLRREC, lblNOMPESS, lblSLDREC, txDSCPROD, txtTODOS, lblPEDIDO;
        private CardView cv_radius;
        private ImageButton btnSINC;
        private ProgressBar progressBar;
        private CheckBox ckTODOS;

        protected DevolucaoItemController devolController { get; set; } = null;
        protected BaixasPedidoController baixaController { get; set; } = null;
        protected ItemPedidoController itemController { get; set; } = null;
        protected PedidoController pedidoController { get; set; } = null;
        private List<DevolucaoItem> devolucao { get; set; }
        private List<ItemPedido> itemPedido { get; set; }
        private List<DevolucaoItensAdapterCls> devolLista { get; set; } = null;
        private BaixasPedidoAdapterCls SelectedOrder { get; set; }
        #endregion
        #region Auxíliares

        protected long? FT_PEDIDO_ITEM_DEVOLUCAO_ID { get; set; } = null;
        protected long? FT_PEDIDO_ITEM_ID { get; set; } = null;
        protected long? FT_PEDIDO_ID { get; set; } = null;

        private long[] fT_PEDIDIO_IDS { get; set; } = null;
        protected List<ItemPedido> Itens { get; set; } = null;
        protected DevolucaoItensAdapterCls? SelectedItem { get; set; } = null;
        protected int? Position = null;

        public BluetoothAdapter Adapter { get { return BluetoothAdapter.DefaultAdapter; } }
        private List<BluetoothDevice> Devices { get; set; } = null;

        private System.Timers.Timer timer = null;

        private bool PermissionToRetrun = true;

        bool connection = false;

        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            ConfigController configController = new ConfigController();
            PedidoController pController = new PedidoController();
            timer = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);

            #region Load

            Itens = new List<ItemPedido>();
            devolucao = new List<DevolucaoItem>();
            itemPedido = new List<ItemPedido>();
            devolController = new DevolucaoItemController();
            baixaController = new BaixasPedidoController();
            itemController = new ItemPedidoController();
            pedidoController = new PedidoController();

            devolLista = new List<DevolucaoItensAdapterCls>();

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_devolucao);

            #endregion

            #region Components

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

            progressBar.Visibility = ViewStates.Invisible;

            #endregion


            Task.Run(() =>
            {
                TestConection();
                timer = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
                timer.Elapsed += (s, a) => this.TestConection();
                timer.Enabled = true;
                timer.AutoReset = true;

                timer.Start();
            });


            #region Configs

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

            //sincOrders();

            if (Intent.HasExtra("CG_PESSOA_ID"))
            {
                try
                {
                    string extra = Intent.GetStringExtra("CG_PESSOA_ID");
                    long codpress = new PessoaController().FindById(extra.Split(" ")[0].ToLong()).CODPESS.Value;
                    txCODPESS.Text = codpress.ToString();
                    LoadListViewBaixa(codpress, null, ckTODOS.Checked);
                    //LoadListView(extra.Split(" ")[1].ToLong());

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

                        SelectedOrder = pedido;
                        lblPEDIDO.Text = "Pedido: " + pedido.NROPEDID.ToString();
                        this.FT_PEDIDO_ID = extra.Split(" ")[3].ToLong();
                        txDSCPROD.Text = "";
                        txQTDPROD.Text = "";
                        lblSLDREC.Text = "SALDO A RECEBER DO CLIENTE";
                        txVLRBAIXA.Text = "";
                        txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

                        btnSINC.Visibility = ViewStates.Visible;
                        btnSINC.Enabled = true;
                        ValidarSaldoReceber(extra.Split(" ")[1].ToLong(), true);
                        LoadValorAReceber();
                        LoadListView(pedido.NROPEDID);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(Utils.Ext.LOG_APP, e.ToString());
                }
            }



            #endregion

            #region Events

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
                    this.SelectedOrder = null;
                    LoadListViewBaixa(txCODPESS.Text.ToLong(), null, ckTODOS.Checked);
                    LoadValorAReceber();
                }
            };

            ckTODOS.CheckedChange += (s, a) =>
            {
                if (!string.IsNullOrEmpty(txCODPESS.Text))
                {
                    listView.Adapter = null;
                    this.LoadListViewBaixa(txCODPESS.Text.ToLong(), null, ckTODOS.Checked);
                }
                if (!ckTODOS.Checked)
                {
                    var cliente = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());
                    if (cliente != null)
                    {
                        var pedido = new PedidoController().getLastPedido(cliente.CG_PESSOA_ID.Value);
                        if (pedido != null)
                            LoadListView(pedido.NROPEDID);
                    }
                }
            };

            txtTODOS.Click += (s, a) =>
            {
                if (ckTODOS.Checked)
                    ckTODOS.Checked = false;
                else
                    ckTODOS.Checked = true;
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
                var adapter = (BaixaPedidoAdapter)listViewBaixa.Adapter;
                var pedido = adapter[args.Position];
                var ftPEDIDO = new PedidoController().FindByNROPEDID(pedido.NROPEDID);
                this.FT_PEDIDO_ID = ftPEDIDO.FT_PEDIDO_ID.Value;
                txDSCPROD.Text = "";
                txQTDPROD.Text = "";
                lblNROPED.Text = "";
                lblVLRREC.Text = "";
                txVLRBAIXA.Text = "";
                txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

                lblSLDREC.Text = "SALDO A RECEBER DO CLIENTE";

                LoadListView(pedido.NROPEDID);
                LoadListViewBaixa(txCODPESS.Text.ToLong(), null, ckTODOS.Checked);

                SelectedOrder = adapter[args.Position];
                lblPEDIDO.Text = "Pedido: " + SelectedOrder.NROPEDID.ToString();
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
                var adapter = (DevolucaoItensAdapter)listView.Adapter;
                var item = SelectedItem = adapter[a.Position];

                var devol = devolController.FindByFT_PEDIDO_ITEM_ID(item.FT_PEDIDO_ITEM_ID);

                this.FT_PEDIDO_ITEM_DEVOLUCAO_ID = (devol == null ? null : devol.FT_PEDIDO_ITEM_DEVOLUCAO_ID);
                this.FT_PEDIDO_ITEM_ID = item.FT_PEDIDO_ITEM_ID;
                Position = a.Position;
                txDSCPROD.Text = item.DSCPROD;
                ShowKeyboard(txQTDPROD);
            };
            btnLimpar.Click += (s, a) => LimparTela();

            txData.FocusChange += (s, a) =>
            {
                if (!txData.HasFocus)
                {
                    if (Utils.Format.DateToString(txData.Text, out string newData))
                    {
                        txData.Text = newData;
                    }
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
                        var baixas = new BaixasPedidoController().baixasByDate(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")), cliente.CG_PESSOA_ID.Value);
                        baixas.ForEach(b =>
                        {
                            var p = new PedidoController().FindById(b.FT_PEDIDO_ID.Value);
                            if (p != null) pedidos.Add(p);
                        });

                        if (this.FT_PEDIDO_ID != null)
                        {
                            bool isAtt = false;
                            var p = new PedidoController().FindById(FT_PEDIDO_ID.Value);

                            if (p != null)
                            {
                                pedidos.ForEach(ped => { if (ped.NROPEDID == p.NROPEDID) isAtt = true; });

                                if (!isAtt)
                                    pedidos.Add(p);
                            }
                        }

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
                                            //txData.Text = newData;
                                            if (!string.IsNullOrEmpty(txtDATARET.Text) && Utils.Format.DateToString(txtDATARET.Text, out newData))
                                            {
                                                RunOnUiThread(() => txtDATARET.Text = newData);
                                                //txtDATARET.Text = newData;
                                                try
                                                {
                                                    var adapter = (BaixaPedidoAdapter)listViewBaixa.Adapter;
                                                    if (adapter.Count > 0)
                                                    {
                                                        //USADO PARA AUXILIAR AQUILO QUE SERÁ IMPRESSO
                                                        List<Pedido> pedidosAlterados = new List<Pedido>();
                                                        bool firstSave = false;

                                                        double pagamento = 0;
                                                        if (!string.IsNullOrEmpty(txVLRBAIXA.Text))
                                                            pagamento = txVLRBAIXA.Text.ToDouble();

                                                        bool HasDevolução = (devolLista.Where(d => !d.QTDDEVOLNOW.Equals("0")).ToList().Count > 0) ? true : false;

                                                        Thread t = new Thread(() =>
                                                        {
                                                            if (devolLista.Count > 0)
                                                                devolLista.Where(d => !d.QTDDEVOLNOW.Equals("0")).ToList().ForEach(d =>
                                                                {
                                                                    if (Salvar(d.FT_PEDIDO_ITEM_ID, d))
                                                                    {
                                                                        if (!firstSave)
                                                                        {
                                                                            var item = itemController.FindById(d.FT_PEDIDO_ITEM_ID);
                                                                            Pedido ped = pedidoController.FindById(item.FT_PEDIDO_ID.Value);
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
            btnGravar.Click += (sender, args) =>
            {
                if (!txDSCPROD.Text.IsEmpty())
                {
                    if (!txQTDPROD.Text.IsEmpty())
                    {
                        Pedido pedido = null;

                        if (SelectedOrder != null)
                            pedido = new PedidoController().FindByNROPEDID(SelectedOrder.NROPEDID);
                        else
                        {
                            var adapter = (BaixaPedidoAdapter)listViewBaixa.Adapter;
                            pedido = new PedidoController().FindByNROPEDID(adapter[0].NROPEDID);
                        }

                        if (pedido != null)
                        {
                            if (pedido.IDTFRMPG.Equals("1"))
                            {
                                var itemPedido = new ItemPedidoController().FindById(SelectedItem.FT_PEDIDO_ITEM_ID);
                                DevolucaoItemController dController = new DevolucaoItemController();

                                var qntdevol = "0";

                                qntdevol = txQTDPROD.Text;
                                if ((itemPedido.QTDATPRO - qntdevol.ToDouble()) >= 0)
                                {
                                    devolLista.RemoveAt((int)Position);
                                    devolLista.Insert((int)Position, new DevolucaoItensAdapterCls()
                                    {
                                        FT_PEDIDO_ITEM_ID = (long)itemPedido.FT_PEDIDO_ITEM_ID,
                                        CODPROD = itemPedido.CODPROD.ToLong(),
                                        DSCPROD = itemPedido.NOMPROD,
                                        QTDPROD = itemPedido.QTDPROD.ToString("####"),
                                        QTDDEVOL = (itemPedido.QTDPROD - itemPedido.QTDATPRO).ToString("####"),
                                        QTDDEVOLNOW = qntdevol
                                    });

                                    var adapter = new DevolucaoItensAdapter(this, devolLista);
                                    listView.Adapter = adapter;
                                    SelectedItem.QTDDEVOLNOW = qntdevol;
                                }
                                else
                                {
                                    this.Msg("QUANTIDADE A DEVOLVER ULTRAPASSA O SALDO ATUAL");

                                    devolLista.RemoveAt((int)Position);
                                    devolLista.Insert((int)Position, new DevolucaoItensAdapterCls()
                                    {
                                        FT_PEDIDO_ITEM_ID = (long)itemPedido.FT_PEDIDO_ITEM_ID,
                                        CODPROD = itemPedido.CODPROD.ToLong(),
                                        DSCPROD = itemPedido.NOMPROD,
                                        QTDPROD = itemPedido.QTDPROD.ToString("####"),
                                        QTDDEVOL = (itemPedido.QTDPROD - itemPedido.QTDATPRO).ToString("####"),
                                        QTDDEVOLNOW = "0"

                                    });

                                    var adapter = new DevolucaoItensAdapter(this, devolLista);
                                    listView.Adapter = adapter;
                                    SelectedItem.QTDDEVOLNOW = qntdevol;
                                }


                                txDSCPROD.Text = "";
                                txQTDPROD.Text = "";

                                HideKeyboard(txQTDPROD);

                                if (!ValidarSaldoReceber(pedido.NROPEDID))
                                {
                                    this.Msg("VALOR A DEVOLVER ULTRAPASSA O VALOR A RECEBER");

                                    devolLista.RemoveAt((int)Position);
                                    devolLista.Insert((int)Position, new DevolucaoItensAdapterCls()
                                    {
                                        FT_PEDIDO_ITEM_ID = (long)itemPedido.FT_PEDIDO_ITEM_ID,
                                        CODPROD = itemPedido.CODPROD.ToLong(),
                                        DSCPROD = itemPedido.NOMPROD,
                                        QTDPROD = itemPedido.QTDPROD.ToString("####"),
                                        QTDDEVOL = (itemPedido.QTDPROD - itemPedido.QTDATPRO).ToString("####"),
                                        QTDDEVOLNOW = "0"
                                    });

                                    var adapter = new DevolucaoItensAdapter(this, devolLista);
                                    listView.Adapter = adapter;
                                    SelectedItem.QTDDEVOLNOW = qntdevol;

                                    ValidarSaldoReceber(SelectedOrder.NROPEDID);
                                }
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
            };
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

            #endregion

            listView.ChoiceMode = ChoiceMode.Single;
            listView.Selected = true;
            listView.SetSelector(Resource.Drawable.selector);

            listViewBaixa.ChoiceMode = ChoiceMode.Single;
            listViewBaixa.Selected = true;
            listViewBaixa.SetSelector(Resource.Drawable.selector);
        }
        public void SelectText(EditText editText)
        {
            if (editText.IsFocused == true)
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
        private void Imprimir(Pedido p)
        {
            Task.Run(() =>
            {
                try
                {
                    var printerController = new PrinterController();
                    string text = string.Empty;
                    text = printerController.FormatOrderForPrintA7(new PedidoController().FindById(p.FT_PEDIDO_ID.Value), "devolucao", fT_PEDIDIO_IDS);

                    SisLog.Logger($"GEROU O TEXTO", "Imprimir");

                    string aux = text;
                    string segVIA = text;
                    BluetoothSocket socket = printerController.GetSocket(txDevice.Text);

                    SisLog.Logger($"INSTANSIOU O SOCKET", "Imprimir");
                    SisLog.Logger($"{socket != null}", "Imprimir");

                    if (socket != null)
                    {
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
                                        b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                        b.SetCancelable(false);
                                        AlertDialog dialog = b.Create();
                                        dialog.Show();
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Debug("ERROR", ex.ToString());
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
                                                        b.SetNegativeButton("CANCELAR", (s, a) => { return; });
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
                                            }
                                        });
                                        builder.SetNegativeButton("NÃO", (s, a) =>
                                        {
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
                                    b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                    b.SetCancelable(false);
                                    AlertDialog dialog = b.Create();
                                    dialog.Show();
                                }
                                catch (Exception ex)
                                {
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
                                b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                b.SetCancelable(false);
                                AlertDialog dialog = b.Create();
                                dialog.Show();
                            }
                            catch (Exception ex)
                            {
                            }
                        });

                }
                catch (Exception ex)
                {
                    RunOnUiThread(() =>
                    {
                        SisLog.Logger(ex.ToString(), "ERRO_IMPRESSAO");
                    });
                }
            }).Wait();
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

        private void ImprimirDialog(List<Pedido> pedidos)
        {
            try
            {
                if (!string.IsNullOrEmpty(txDevice.Text))
                {
                    try
                    {
                        if (pedidos.Count > 0)
                            for (int i = 0; i < pedidos.Count; i++)
                            {
                                SisLog.Logger($"MANDANDO PARA IMPRESSAO {i}° PEDIDO", "ImprimirDialog");
                                Pedido p = pedidos[i];
                                try
                                {
                                    Task.Run(() =>
                                    {
                                        RunOnUiThread(() =>
                                        {
                                            SisLog.Logger($"ENTROU NO RunOnUiThread", "ImprimirDialog");

                                            new DialogFactory().CreateDialog(this,
                                                "IMPRESSÕES !!!",
                                                $"GOSTARIA DE IMPRIMIR O PEDIDO {p.NROPEDID} ?",
                                                "SIM",
                                                () =>
                                                {
                                                    Task.Run(() => Imprimir(p));
                                                },
                                                "NÃO",
                                                () => { });
                                        });
                                    }).Wait();
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

                                        SisLog.Logger(e.ToString(), "ImprimirDialog_03");
                                    });
                                }
                            }
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

        private void LoadValorAReceber()
        {
            if (!string.IsNullOrEmpty(txCODPESS.Text))
            {
                var cliente = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());
                if (cliente != null)
                {
                    var pedidos = new PedidoController().FindByCG_PESSOA_ID(cliente.CG_PESSOA_ID.Value).OrderBy(p => p.DATEMISS).ToList();

                    double saldo = 0;

                    pedidos.ForEach(p =>
                    {
                        if (p.SITPEDID != (short)Pedido.SitPedido.Atendido)
                            if (p.SITPEDID != (short)Pedido.SitPedido.Aberto)
                                if (p.SITPEDID != (short)Pedido.SitPedido.Cancelado)
                                {
                                    var baixa = new BaixasPedidoController().GerarBaixa(p);
                                    saldo += baixa.VLRRECBR;
                                }
                    });
                    lbSaldo.Text = $"{saldo.ToString("C2")}";
                }
            }
        }

        /// <summary>
        /// RESPONSÁVEL PARA VERIFICAR SE O SALDO A RECEBER NÃO FICARÁ NEGATIVO
        /// </summary>
        /// <param name="nropedido"></param>
        /// <param name="isLoadView"></param>
        /// <returns></returns>
        public bool ValidarSaldoReceber(long nropedido, bool isLoadView = false)
        {
            bool result = true;
            double valorDevolvido = 0;

            Pedido pedido = new PedidoController().FindByNROPEDID(nropedido);
            if (pedido != null)
            {
                double receber = new BaixasPedidoController().toReceive(pedido.FT_PEDIDO_ID.Value);
                List<ItemPedido> itens = new ItemPedidoController().FindAllOrderItems(pedido.FT_PEDIDO_ID.Value);
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
                {
                    LoadListViewBaixa(new PessoaController().FindById(pedido.ID_PESSOA.Value).CODPESS.Value, itens, ckTODOS.Checked);
                    result = true;
                }
                else
                {
                    double auxReceber = new BaixasPedidoController().PrevisaoReceber(itens);
                    if (auxReceber.ToString().StartsWith("-"))
                        result = false;
                    else
                        LoadListViewBaixa(new PessoaController().FindById(pedido.ID_PESSOA.Value).CODPESS.Value, itens, ckTODOS.Checked);
                }
            }
            else
                result = false;

            return result;
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

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 1)
                if (resultCode == Result.Ok)
                {
                    LimparTela();
                    string result = data.GetStringExtra("result").Split(";")[0];
                    if (!result.IsEmpty())
                    {
                        //LoadDataByNROPEDID(result.ToLong());
                        txCODPESS.Text = result;
                        LoadListViewBaixa(result.ToLong(), null, ckTODOS.Checked);
                    }
                }
            // --> IMPRESSORA
            if (requestCode == 2)
                if (resultCode == Result.Ok)
                {
                    string resultImpressao = data.GetStringExtra("resultImpres");
                    if (!resultImpressao.IsEmpty())
                    {
                        if (resultImpressao.Length > 18)
                            resultImpressao = resultImpressao.Substring(0, 18);

                        txDevice.Text = resultImpressao;

                        new DialogFactory().CreateDialog(context: this,
                            title: "AVISO DO SISTEMA",
                            message: $"DESEJA DEFINIR {resultImpressao} COMO IMPRESSORA PADRÃO?",
                            positiveButtonName: "SIM",
                            positiveAction: () =>
                            {
                                ConfigController configController = new ConfigController();
                                Models.Config config = configController.GetConfig();

                                config.NOMIMPRE = resultImpressao;
                                configController.Save(config);
                            },
                            negativeButtonName: "NÃO",
                            negativeAction: () => { return; });
                    }
                }
        }

        public override void OnBackPressed()
        {
            if (this.PermissionToRetrun)
                base.OnBackPressed();
            else
                this.Msg("FAVOR AGUARDE A SINCRONIZAÇÃO DA BAIXA");
        }

        public void LoadListViewBaixa(long pCODPESS, List<ItemPedido> itemPedidos = null, bool isAll = false)
        {
            try
            {
                if (pCODPESS.ToString() != "0")
                {
                    lblPEDIDO.Text = "";
                    SelectedItem = null;
                    var cliente = new PessoaController().FindByCODPESS(pCODPESS);

                    if (cliente != null)
                    {
                        lblNOMPESS.Text = cliente.NOMFANTA;
                        listViewBaixa.Adapter = null;

                        BaixasPedidoController baixasPedidoController = new BaixasPedidoController();

                        List<BaixasPedidoAdapterCls> listBaixa = new List<BaixasPedidoAdapterCls>();

                        ///CASO TENHA PASSADO COMO PARAMETRO (itemPedidos) DEFINIRÁ ESSA VARIÁVEL COM O PEDIDO ALTERADO.
                        Pedido pedidoAlterado = null;
                        if (itemPedidos != null)
                            pedidoAlterado = new PedidoController().FindById(itemPedidos.First().FT_PEDIDO_ID.Value);


                        if (isAll)
                        {
                            ///TODOS OS PEDIDOS DO CLIENTE
                            var pedidos = new PedidoController().FindOpenOrderByCG_PESSOA_ID(cliente.CG_PESSOA_ID.Value).OrderBy(p => p.DATEMISS).ToList();

                            pedidos.ToList().ForEach(aux =>
                            {
                                if (pedidoAlterado != null)
                                {
                                    ///CASO SEJA O MESMO PEDIDO DIFINIDO NA VARIÁVEL (pedidoAlterado) ADICIONARÁ NA LISTA ESSE PEDIDO COM O SALDO A RECEBER ALTERADO.
                                    if (aux.NROPEDID == pedidoAlterado.NROPEDID)
                                        listBaixa.Add(new BaixasPedidoAdapterCls(itemPedidos));
                                    else
                                        listBaixa.Add(new BaixasPedidoAdapterCls(aux.NROPEDID));
                                }
                                else
                                    listBaixa.Add(new BaixasPedidoAdapterCls(aux.NROPEDID));
                            });
                        }
                        else
                        {
                            listBaixa.Clear();
                            var pedido = new PedidoController().getLastPedido(cliente.CG_PESSOA_ID.Value);
                            if (pedido != null)
                            {
                                if (pedidoAlterado != null)
                                {
                                    ///CASO SEJA O MESMO PEDIDO DIFINIDO NA VARIÁVEL (pedidoAlterado) ADICIONARÁ NA LISTA ESSE PEDIDO COM O SALDO A RECEBER ALTERADO.
                                    if (pedido.NROPEDID == pedidoAlterado.NROPEDID)
                                        listBaixa.Add(new BaixasPedidoAdapterCls(itemPedidos));
                                    else
                                        listBaixa.Add(new BaixasPedidoAdapterCls(pedido.NROPEDID));
                                }
                                else
                                    listBaixa.Add(new BaixasPedidoAdapterCls(pedido.NROPEDID));

                                lblPEDIDO.Text = "Pedido: " + pedido.NROPEDID.ToString();
                            }
                        }

                        double saldo = 0;
                        saldo = new BaixasPedidoController().totalReceberCliente(cliente.ID.Value, out string[] pd);

                        if (pedidoAlterado != null && pedidoAlterado.SITPEDID != (short)Pedido.SitPedido.Aberto)
                        {
                            var baixa = new BaixasPedidoController().GerarBaixa(pedidoAlterado).VLRRECBR;
                            var altarado = new BaixasPedidoAdapterCls(itemPedidos).VLRRECBR.Replace("R$ ", "").ToDouble();
                            saldo -= (baixa - altarado);
                        }

                        if (listBaixa.Count > 0)
                        {
                            BaixaPedidoAdapter adapterBaixa = new BaixaPedidoAdapter(this, listBaixa);

                            this.listViewBaixa.Adapter = adapterBaixa;
                        }

                        lblNOMPESS.Text = $"{cliente.NOMFANTA}";
                        lbSaldo.Text = "";
                        lbSaldo.Text = saldo.ToString("C2");
                    }

                    txtDATARET.Text = DateTime.Now.AddMonths(1).ToString("dd/MM/yyyy");

                    btnSINC.Visibility = ViewStates.Visible;
                    btnSINC.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString());
            }
        }

        public void LoadListView(long pNROPEDID)
        {
            try
            {
                if (pNROPEDID.ToString() != "0")
                {
                    if (listViewBaixa.Count > 0)
                    {
                        listView.Adapter = null;
                        devolLista.Clear();
                        var pedido = new PedidoController().FindByNROPEDID(pNROPEDID);
                        if (pedido != null)
                        {
                            var itens = new ItemPedidoController().FindAllOrderItems(pedido.FT_PEDIDO_ID.Value);

                            var prodController = new ProdutoController();
                            itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
                            {
                                devolLista.Add(new DevolucaoItensAdapterCls()
                                {
                                    FT_PEDIDO_ITEM_ID = i.FT_PEDIDO_ITEM_ID.Value,
                                    CODPROD = i.CODPROD.ToLong(),
                                    DSCPROD = i.NOMPROD,
                                    QTDPROD = i.QTDPROD.ToString("####"),
                                    QTDDEVOL = (i.QTDPROD - i.QTDATPRO).ToString("####"),
                                    QTDDEVOLNOW = "0"
                                });

                            });
                            var adapter = new DevolucaoItensAdapter(this, devolLista);
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
        protected void LimparTela()
        {
            txData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.FT_PEDIDO_ITEM_DEVOLUCAO_ID = null;
            this.FT_PEDIDO_ITEM_ID = null;
            this.FT_PEDIDO_ID = null;
            listViewBaixa.Adapter = null;
            listView.Adapter = null;
            SelectedItem = null;
            Itens = null;

            devolLista.Clear();
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

        public enum RequestCode
        {
            BuscaPedido = 1
        }

        public void NextFocus(View view)
        {
            if (CurrentFocus != null)
                CurrentFocus.ClearFocus();

            view.RequestFocus();
        }

        /// <summary>
        /// SALVA A DEVOLUÇÃO
        /// </summary>
        protected bool Salvar(long pFT_PEDIDO_ITEM_ID, DevolucaoItensAdapterCls devolItem)
        {
            try
            {
                bool auxSAVE = false;
                var CODPROD = itemController.FindById(pFT_PEDIDO_ITEM_ID).CODPROD;
                Produto p = new ProdutoController().FindByCODPROD(long.Parse(CODPROD));
                var item = itemController.FindById(pFT_PEDIDO_ITEM_ID);
                Pedido ped = pedidoController.FindById(item.FT_PEDIDO_ID.Value);

                if (CODPROD != null)
                {
                    Database.GetConnection().RunInTransaction(() =>
                    {
                        Produto prod = new ProdutoController().FindByCODPROD(long.Parse(CODPROD));
                        GeolocatorController geoController = new GeolocatorController();
                        BaixasPedidoController bController = new BaixasPedidoController();

                        var nompess = new PessoaController().FindById(ped.ID_PESSOA.Value).NOMPESS;

                        DevolucaoItem devolucao = new DevolucaoItem()
                        {
                            CG_VENDEDOR_ID = new VendedorController().Vendedor.CG_VENDEDOR_ID,
                            CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
                            FT_PEDIDO_ITEM_ID = devolItem.FT_PEDIDO_ITEM_ID,
                            CG_PRODUTO_ID = item.CG_PRODUTO_ID,
                            FT_PEDIDO_ID = ped.FT_PEDIDO_ID.Value,
                            NROPEDIDO = ped.NROPEDID,
                            NOMPESS = nompess,
                            CODPROD = CODPROD.ToLong(),
                            DATDEVOL = DateTime.Now,
                            QTDDEVOL = devolItem.QTDDEVOLNOW.ToDouble(),
                            DTHULTAT = DateTime.Now,
                            USRULTAT = new OperadorController().Operador.USROPER,
                            NOMPROD = prod.DSCPROD,
                            INDSINC = false,
                            Produto = prod
                        };

                        item.QTDATPRO = (item.QTDATPRO - devolucao.QTDDEVOL);
                        item.DTHULTAT = DateTime.Now;

                        itemController.Save(item);
                        devolController.SaveItemDevolucao(devolucao);

                        BaixasPedido baixa = bController.GerarBaixa(ped);
                        bController.SalvarBaixa(baixa, DateTime.Parse(txtDATARET.Text));

                        geoController.SaveDevolutionLocalization(devolucao.FT_PEDIDO_ITEM_DEVOLUCAO_ID.Value);

                        RomaneioItem romItem = new RomaneioController().FindByIdItem(prod.CG_PRODUTO_ID.Value);
                        if (romItem != null)
                        {
                            romItem.QTDDEVCL += devolItem.QTDDEVOLNOW.ToDouble();
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
                                QTDDEVCL = devolItem.QTDDEVOLNOW.ToDouble(),
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
                var adapter = (BaixaPedidoAdapter)listViewBaixa.Adapter;

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
                if (ids.Count > 0)
                    this.fT_PEDIDIO_IDS = ids.ToArray();

                if (!isWait)
                    FinalizarSalva(auxSAVE, INDDEVOL, false, pedidosAlterados);
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

        public bool ValidarDevolucao(List<DevolucaoItensAdapterCls> devolucao)
        {
            bool result = true;

            try
            {
                var pedido = pedidoController.FindById(this.FT_PEDIDO_ID.Value);
                double valorCalcular = baixaController.GerarBaixa(pedido).VLRRECBR;

                devolucao.ForEach(d =>
                {
                    Empresa e = new EmpresaController().Empresa;

                    var item = itemController.FindById(d.FT_PEDIDO_ITEM_ID);

                    double qntatual = item.QTDATPRO;
                    double qntdevol = d.QTDDEVOLNOW.ToDouble();

                    double valorDevol = item.VLRUNIT * qntdevol;

                    //if (pedido.PERCOMIS > 0)
                    valorDevol -= ((valorDevol / 100) * pedido.PERCOMIS);
                    //else
                    //valorDevol -= ((valorDevol / 100) * e.PERCOMIS);

                    if (qntatual < qntdevol)
                        result = false;
                    else if (valorDevol > valorCalcular)
                        result = false;

                    valorCalcular -= valorDevol;
                });

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString());
                return false;
            }
        }

        protected override void OnDestroy()
        {
            timer.Stop();
            base.OnDestroy();
        }

        public void loadDevices()
        {
            Devices = new List<BluetoothDevice>();
            if (Adapter.StartDiscovery())
                Devices = Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
            else
                Devices = Adapter.BondedDevices.OrderBy(i => i.Name).ToList();

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

                                this.SelectedOrder = null;
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
                        LoadListViewBaixa(txCODPESS.Text.ToLong(), null, ckTODOS.Checked);

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
    }
}