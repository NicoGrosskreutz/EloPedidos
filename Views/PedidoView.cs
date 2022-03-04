using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Utils;
using EloPedidos.Models;
using EloPedidos.Persistence;
using A = Android.App;
using System.Threading;
using System.Threading.Tasks;
using static EloPedidos.Models.Pedido;
using Android.Support.V7.Widget;
using M = EloPedidos.Models;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Views.InputMethods;
using Android.Content.PM;
using Android.Bluetooth;
using System.Linq;
using Xamarin.Essentials;
using System.Globalization;
using Android.Support.Design.Widget;
using Plugin.CurrentActivity;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;
using System.IO;
using Android.Webkit;
using Format = EloPedidos.Utils.Format;
using ZXing.Mobile;
using Android.Media;
using Plugin.DeviceInfo;
using EloPedidos.Adapter;
using System.Text;
using EloPedidos.Services;
using static Android.App.ActivityManager;

namespace EloPedidos.Views
{
    //[Activity(Label = "EloPedidos", WindowSoftInputMode = SoftInput.AdjustResize, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    [Activity(Label = "EloPedidos")]
    public class PedidoView : AppCompatActivity
    {
        #region Var

        private RelativeLayout RLayout;
        private TextInputEditText txCODPESS;
        private TextView txNOMFANTA;
        private TextInputEditText txData;
        private TextInputEditText txtDATARET;
        private TextInputEditText txNROPEDID;
        private TextInputEditText txCODPROD;
        private TextInputEditText txQtd;
        private TextInputEditText txPreco;
        private TextInputEditText txTotal;
        private TextInputEditText txtVLRLMT;
        private TextView lbTotal, txtATRASADO, lblVLRABRT;
        private TextView txNOMPROD;
        private CheckBox ckBrinde;
        private CheckBox ckCXPC;
        private TextInputEditText txDSCOBSER;
        private EditText txDevice;
        private TextView txtSALDO;
        private CheckBox ckPrazo;
        private Button btnSalvarProd;
        private Button btnExcluirProd;
        private Button btnSalvar;
        private Button btnExcluir;
        private Button btnLimpar;
        private Button btnImprimir;
        public ListView listView;
        private ListView listDevice;
        private TextView lbSITPEDID;
        private ProgressBar progressBar;
        private CardView cv_radius;
        private FloatingActionButton floatingButton;
        private LinearLayout linearLayout1;
        private ImageButton btnSend;
        private FloatingActionButton fab;
        private LinearLayout imgBAIXA, imgESTOQUE, imgAGENDA, imgCADASTRO;
        private RadioButton rbCOMISSAO, rbBRINDE;
        private RadioGroup radioGroup;

        // VARIÁVEIS DAS OPÇÕES DE COMPARTILHAMENTO
        private TextView lblSHAREWPP, lblSHAREEMAIL, lblSHARESMS, lblPDF;
        private CardView cvShare;

        public bool ResultNROPEDID = false;
        #endregion
        #region atributos auxiliares da view
        /// <summary>
        /// Auxiliar para a tela
        /// </summary>
        private long? FT_PEDIDO_ID { get; set; } = null;
        /// <summary>
        /// Auxiliar da tela -> código do produto
        /// </summary>
        private long? CODPROD { get; set; } = null;
        /// <summary>
        /// Auxiliar da tela -> código para posição do item selecionado 
        /// </summary>
        private int? Position { get; set; } = null;
        /// <summary>
        /// Auxiliar da tela para manipulação dos items
        /// </summary>
        private List<ItemPedido> ItensPedido { get; set; }
        /// <summary>
        /// Auxiliar da view para exclusão de itens na atualização do pedido
        /// </summary>
        private List<ItemPedido> ItensExclusao { get; set; } = null;
        private long? ID_PESS { get; set; } = null;
        /// <summary>
        /// Variável para bloqueio do menu ao atualizar dados 
        /// </summary>
        private bool BloquearMenus { get; set; } = false;
        public BluetoothAdapter Adapter { get { return BluetoothAdapter.DefaultAdapter; } }
        private List<BluetoothDevice> Devices { get; set; } = null;

        public ArrayAdapter<string> adapterProducts;

        private System.Timers.Timer timer = null;

        bool auxCODPESS = false;

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //Task.Run(() => SincronizarPedidos());
            //Task.Run(() =>
            //{
            //	if (TestConection())
            //		new Sincronizador().SincronizarAllNotSync();
            //});

            ConfigController cController = new ConfigController();
            M.Config config = cController.GetConfig();

            Task.Run(() => { this.VerificarPermissao(); });

            ItensPedido = new List<ItemPedido>();

            ItensExclusao = new List<ItemPedido>();

            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_pedido);

            #region Componentes

            RLayout = FindViewById<RelativeLayout>(Resource.Id.relativeLayoutScroll);
            txCODPESS = FindViewById<TextInputEditText>(Resource.Id.txCODPESS);
            txNOMFANTA = FindViewById<TextView>(Resource.Id.txNOMFANTA);
            txtATRASADO = FindViewById<TextView>(Resource.Id.txtATRASADO);
            lblVLRABRT = FindViewById<TextView>(Resource.Id.lblVLRABRT);
            txData = FindViewById<TextInputEditText>(Resource.Id.txData);
            txtDATARET = FindViewById<TextInputEditText>(Resource.Id.txtDATARET);
            txNROPEDID = FindViewById<TextInputEditText>(Resource.Id.txNROPEDID);
            txCODPROD = FindViewById<TextInputEditText>(Resource.Id.txCODPROD);
            txQtd = FindViewById<TextInputEditText>(Resource.Id.txQtd);
            txPreco = FindViewById<TextInputEditText>(Resource.Id.txPreco);
            txTotal = FindViewById<TextInputEditText>(Resource.Id.txTotal);
            txtVLRLMT = FindViewById<TextInputEditText>(Resource.Id.txtVLRLMT);
            txDevice = FindViewById<EditText>(Resource.Id.txDevice);
            lbTotal = FindViewById<TextView>(Resource.Id.lbTotal);
            txNOMPROD = FindViewById<TextView>(Resource.Id.txNOMPROD);
            txtSALDO = FindViewById<TextView>(Resource.Id.txSALDO);
            ckBrinde = FindViewById<CheckBox>(Resource.Id.ckBrinde);
            txDSCOBSER = FindViewById<TextInputEditText>(Resource.Id.txDSCOBSER);
            ckPrazo = FindViewById<CheckBox>(Resource.Id.ckPrazo);
            ckCXPC = FindViewById<CheckBox>(Resource.Id.ckCXPC);
            btnSalvarProd = FindViewById<Button>(Resource.Id.btnSalvarProd);
            btnExcluirProd = FindViewById<Button>(Resource.Id.btnExcluirProd);
            btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
            btnExcluir = FindViewById<Button>(Resource.Id.btnExcluir);
            btnLimpar = FindViewById<Button>(Resource.Id.btnLimpar);
            btnImprimir = FindViewById<Button>(Resource.Id.btnImprimir);
            listView = FindViewById<ListView>(Resource.Id.listView);
            lbSITPEDID = FindViewById<TextView>(Resource.Id.lbSITPEDID);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            cv_radius = FindViewById<CardView>(Resource.Id.cv_radius);
            floatingButton = FindViewById<FloatingActionButton>(Resource.Id.floatingButton);
            linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            btnSend = FindViewById<ImageButton>(Resource.Id.btnSend);
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            lblSHAREWPP = FindViewById<TextView>(Resource.Id.lblSHAREWPP);
            lblSHAREEMAIL = FindViewById<TextView>(Resource.Id.lblSHAREEMAIL);
            lblSHARESMS = FindViewById<TextView>(Resource.Id.lblSHARESMS);
            lblPDF = FindViewById<TextView>(Resource.Id.lblPDF);
            cvShare = FindViewById<CardView>(Resource.Id.cvShare);
            imgBAIXA = FindViewById<LinearLayout>(Resource.Id.imgBAIXA);
            imgESTOQUE = FindViewById<LinearLayout>(Resource.Id.imgESTOQUE);
            imgAGENDA = FindViewById<LinearLayout>(Resource.Id.imgAGENDA);
            imgCADASTRO = FindViewById<LinearLayout>(Resource.Id.imgCADASTRO);
            rbCOMISSAO = FindViewById<RadioButton>(Resource.Id.rbCOMISSAO);
            rbBRINDE = FindViewById<RadioButton>(Resource.Id.rbBRINDE);
            radioGroup = FindViewById<RadioGroup>(Resource.Id.radioGroup);


            #endregion

            //MONITORA A VERSÃO DO APLICATIVO   ----> ex. VersionTracking.CurrentVersion
            VersionTracking.Track();

            Task.Run(() =>
            {
                versionController();
                this.TestConection();
                timer = new System.Timers.Timer(TimeSpan.FromSeconds(30).TotalMilliseconds);
                timer.Elapsed += (s, a) => this.TestConection();
                timer.Enabled = true;
                timer.AutoReset = true;

                timer.Start();
            });

            txData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dateEntry = DateTime.Parse(txData.Text);
            txtDATARET.Text = dateEntry.AddMonths(1).ToString("dd/MM/yyyy");

            txNROPEDID.Text = "0";
            NextFocus(txNROPEDID);

            imgBAIXA.Enabled = false;

            btnSalvarProd.Enabled = false;
            btnExcluirProd.Enabled = false;
            btnSalvar.Enabled = false;
            btnImprimir.Enabled = false;
            txDevice.Focusable = false;
            lblSHARESMS.Enabled = false;
            rbCOMISSAO.Checked = true;
            txNROPEDID.Focusable = false;

            if (config.CODEAN)
                fab.Visibility = ViewStates.Visible;
            else
                fab.Visibility = ViewStates.Invisible;

            if (this.RestorePreference("NOME") != "")
                txDevice.Text = Ext.RestorePreference(this, "NOME");

            cvShare.Visibility = ViewStates.Invisible;

            SisLog.CreateFolder(this);

            #region Configs
            // Filters
            txNOMFANTA.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
            txNOMPROD.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
            txDSCOBSER.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });

            txPreco.Enabled = false;
            txTotal.Enabled = false;
            txQtd.Enabled = false;
            txTotal.EnableView(false);
            ckPrazo.Checked = true;

            progressBar.Visibility = ViewStates.Invisible;
            // ==================

            txCODPROD.SetTextSize(ComplexUnitType.Sp, 14);
            txQtd.SetTextSize(ComplexUnitType.Sp, 14);
            txTotal.SetTextSize(ComplexUnitType.Sp, 14);
            #endregion
            #region Eventos

            bool ResultCODPRESS = false;
            bool ResultCODPROD = false;
            string codpess = "";
            int Count = 0;

            txtDATARET.SetSelectAllOnFocus(true);
            txData.SetSelectAllOnFocus(true);
            txNROPEDID.SetSelectAllOnFocus(true);

            txCODPESS.TextChanged += (sender, eventArgs) =>
            {
                if (txCODPESS.Text != "" && auxCODPESS || txCODPESS.Text != "" && !auxCODPESS)
                {
                    txtATRASADO.Text = string.Empty;
                    lblVLRABRT.Text = string.Empty;

                    if (!string.IsNullOrEmpty(txCODPESS.Text))
                    {
                        PessoaDAO pDAO = new PessoaDAO();
                        Pessoa p = pDAO.FindByCODPESS(long.Parse(txCODPESS.Text));
                        if (p != null)
                        {
                            txNOMFANTA.SetTextColor(Color.DarkGray);
                            txNOMFANTA.Text = $"{p.NOMFANTA}\n{p.DSCENDER} {p.NROENDER}";
                            txtVLRLMT.Text = p.VLRLIMPD.ToString("C2");
                            imgBAIXA.Enabled = true;
                            ID_PESS = p.ID;

                            loadToReceive();

                            if (ResultCODPRESS == true)
                            {
                                ResultCODPRESS = false;
                                codpess = txCODPESS.Text;
                                txCODPROD.RequestFocus();
                                ShowKeyboard(txCODPROD);
                            }
                            else if (auxCODPESS)
                            {
                                codpess = txCODPESS.Text;
                                txCODPROD.RequestFocus();
                                ShowKeyboard(txCODPROD);
                            }
                        }
                        else
                        {
                            imgBAIXA.Enabled = false;
                            lblVLRABRT.Visibility = ViewStates.Invisible;
                            txtATRASADO.Visibility = ViewStates.Invisible;
                            txNOMFANTA.Text = string.Empty;
                            codpess = "";
                        }
                    }
                    else
                        txNOMFANTA.Text = string.Empty;
                }
                else if (string.IsNullOrEmpty(txCODPESS.Text))
                {
                    if (codpess != "")
                        txCODPESS.Text = codpess;
                    else
                        txNOMFANTA.Text = string.Empty;
                }
            };

            txCODPESS.Click += (s, a) =>
            {
                if (!txCODPESS.Text.IsEmpty())
                {
                    if (Count == 0)
                        Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(1.0)); Count = 0; });

                    ++Count;

                    if (Count == 2)
                    {
                        Intent i = new Intent(this, typeof(ClienteView));
                        i.PutExtra("CODPESS", txCODPESS.Text);
                        StartActivity(i);
                    }
                }
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

            txCODPESS.FocusChange += (sender, eventsArgs) =>
            {
                if (!txCODPESS.HasFocus && !string.IsNullOrEmpty(txCODPESS.Text))
                {
                    Pessoa pessoa = new PessoaController().FindByCODPESS(txCODPESS.Text.ToLong());
                    if (pessoa == null)
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("AVISO");
                        builder.SetMessage("CLIENTE NÃO ENCONTRADO");
                        builder.SetCancelable(false);
                        builder.SetNeutralButton("OK", (s, a) =>
                        {
                            txCODPESS.SetSelectAllOnFocus(true);
                            txCODPESS.RequestFocus();
                            ShowKeyboard(txCODPESS);
                        });
                        AlertDialog dialog = builder.Create();
                        dialog.Show();
                    }
                    else
                    {
                        if (!validarCliente(pessoa))
                        {
                            txCODPESS.SetSelectAllOnFocus(true);
                            txCODPESS.RequestFocus();
                            ShowKeyboard(txCODPESS);
                        }
                    }
                }

                if (txCODPESS.Text.Length > 0)
                    txCODPESS.SetSelectAllOnFocus(true);
            };

            txCODPROD.FocusChange += (sender, eventsArgs) =>
            {
                if (txCODPROD.Text.Length > 0)
                    txCODPROD.SetSelectAllOnFocus(true);
            };

            txNROPEDID.FocusChange += (sender, eventsArgs) =>
            {
                if (txNROPEDID.Text.Length > 0)
                {
                    if (new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong()) == null)
                        txNROPEDID.Text = "0";

                    txNROPEDID.SetSelectAllOnFocus(true);
                }
            };

            txQtd.FocusChange += (sender, eventsArgs) =>
            {
                if (txQtd.Text.Length > 0)
                    txQtd.SetSelectAllOnFocus(true);
            };

            txCODPROD.TextChanged += (sender, eventArgs) =>
            {
                if (!string.IsNullOrEmpty(txCODPROD.Text))
                {
                    Produto p = new ProdutoDAO().FindByCODPROD(long.Parse(txCODPROD.Text));

                    if (p != null)
                    {
                        EnableControls(false);
                        txNOMPROD.Text = "";
                        txPreco.Text = "";
                        txQtd.Text = "";
                        ckBrinde.Checked = false;

                        EnableControls();
                        txNOMPROD.SetTextColor(Color.DarkGray);
                        if (new RomaneioController().FindByIdItem(p.CG_PRODUTO_ID.Value) != null)
                        {
                            var itemROMAN = new RomaneioController().FindByIdItem(p.CG_PRODUTO_ID.Value);
                            txNOMPROD.Text = $"{p.DSCPROD}";
                            txtSALDO.Text = (itemROMAN.QTDPROD + itemROMAN.QTDDEVCL - itemROMAN.QTDVENDA).ToString();
                        }
                        else
                            txNOMPROD.Text = $"{p.DSCPROD}";

                        txPreco.Text = p.PRCVENDA.ToString("0.00").Replace(".", ",");
                        txQtd.Enabled = true;
                        txPreco.Enabled = true;
                        txPreco.Focusable = true;

                        txPreco.SetSelectAllOnFocus(true);

                        if (ResultCODPROD == true)
                        {
                            txQtd.Enabled = true;
                            txQtd.RequestFocus();
                            ShowKeyboard(txQtd);
                            ResultCODPROD = false;

                        }
                        if (txQtd.IsFocused)
                        {
                            txQtd.RequestFocus();
                            ShowKeyboard(txQtd);
                        }
                    }
                    else
                    {
                        txNOMPROD.Text = "";
                        txPreco.Text = "";
                        txQtd.Text = "";
                        ckBrinde.Checked = false;
                        txNOMPROD.SetTextColor(Color.Red);
                        txNOMPROD.Text = "";

                        txPreco.Enabled = false;
                        txQtd.Enabled = false;
                        this.Msg("PRODUTO NÃO ENCONTRADO! VERIFIQUE");

                    }
                }
                else
                {
                    txQtd.Enabled = false;
                    txPreco.Enabled = false;
                    txPreco.Text = "";
                    txNOMPROD.Text = "";
                    EnableControls(false);
                }
            };

            txData.FocusChange += (sender, eventArgs) =>
            {
                if (!string.IsNullOrEmpty(txData.Text))
                {
                    SelectText(txData);
                    if (!txData.HasFocus)
                    {
                        if (Format.DateToString(txData.Text, out string newDate))
                        {
                            txData.Text = newDate;
                            DateTime dateEntry = DateTime.Parse(newDate);
                            txtDATARET.Text = dateEntry.AddMonths(1).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            txData.Text = newDate;
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("AVISO DO SISTEMA!");
                            builder.SetMessage("FORMATO DA DATA ESTÁ INCORRETO\n(DD/MM/AAAA)");
                            builder.SetCancelable(false);
                            builder.SetNeutralButton("OK", (s, a) =>
                            {
                                txData.RequestFocus();
                                return;
                            });
                            AlertDialog dialog = builder.Create();
                            dialog.Show();
                        }
                    }
                    txData.SetSelectAllOnFocus(true);
                }
                if (txData.HasFocus)
                    DataPickerDialog(txData);
            };

            txData.LongClick += (s, a) => DataPickerDialog(txData);
            txtDATARET.LongClick += (s, a) => DataPickerDialog(txData);

            txtDATARET.FocusChange += (sender, eventArgs) =>
            {
                if (!string.IsNullOrEmpty(txtDATARET.Text))
                {
                    SelectText(txtDATARET);

                    if (!txtDATARET.HasFocus)
                    {
                        if (Utils.Format.DateToString(txtDATARET.Text, out string newDate))
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

                        txCODPESS.Text = (cliente.CODPESS != null) ? cliente.CODPESS.ToString() : "";
                        this.ID_PESS = cliente.ID;
                        dialog.Cancel();

                        if (cliente.CODPESS != null)
                        {
                            if (!validarCliente(cliente))
                            {
                                txCODPESS.SetSelectAllOnFocus(true);
                                txCODPESS.RequestFocus();
                                ShowKeyboard(txCODPESS);
                            }
                            else
                            {
                                txCODPROD.RequestFocus();
                                ShowKeyboard(txCODPROD);
                            }
                        }
                        else
                        {
                            AlertDialog.Builder b = new AlertDialog.Builder(this);
                            b.SetTitle("AVISO");
                            b.SetMessage("CLIENTE NÃO SINCRONIZADO COM O SERVIDOR\nDESEJA CONTINUAR ?");
                            b.SetPositiveButton("SIM", (s, a) =>
                            {
                                if (validarCliente(cliente))
                                {
                                    txNOMFANTA.SetTextColor(Color.DarkGray);
                                    txNOMFANTA.Text = $"{cliente.NOMFANTA}\n{cliente.DSCENDER} {cliente.NROENDER}";
                                    txCODPESS.SetSelectAllOnFocus(true);

                                    txCODPROD.RequestFocus();
                                    ShowKeyboard(txCODPROD);
                                }
                                else
                                {
                                    txCODPESS.SetSelectAllOnFocus(true);
                                    txCODPESS.RequestFocus();
                                    ShowKeyboard(txCODPESS);
                                }
                            });
                            b.SetNegativeButton("NÃO", (s, a) => { return; });
                            AlertDialog d = b.Create();
                            d.Show();
                        }
                    };
                }
            };

            txCODPROD.LongClick += (sender, eventArgs) =>
            {
                if (new ProdutoController().FindAll().Count == 0)
                    this.Msg("NENHUM PRODUTO REGISTRADO!");
                else
                {
                    TextInputEditText txPesquisa;
                    ListView listView;
                    CheckBox ckROMAN;

                    View view = this.LayoutInflater.Inflate(Resource.Layout.activity_buscarproduto, null);

                    txPesquisa = view.FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
                    listView = view.FindViewById<ListView>(Resource.Id.listView);
                    ckROMAN = view.FindViewById<CheckBox>(Resource.Id.ckROMAN);


                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetView(view);
                    builder.SetNeutralButton("CONCLUIDO", (s, a) => { return; });

                    AlertDialog dialog = builder.Create();
                    dialog.Show();

                    ckROMAN.Checked = true;
                    ShowKeyboard(txPesquisa);
                    txPesquisa.ResetPivot();

                    List<RomaneioItem> romaneio = new RomaneioController().FindAll(); ;

                    List<Produto> prod = new List<Produto>();

                    romaneio.ToList().ForEach((aux) =>
                    {
                        if (aux.CG_PRODUTO_ID != 0)
                        {
                            var item = new ProdutoController().FindById(aux.CG_PRODUTO_ID);
                            if (item != null)
                                prod.Add(item);
                        }
                    });

                    listView.Adapter = new AdapterBuscarProduto(this, prod);

                    txPesquisa.TextChanged += (s, a) =>
                    {
                        prod.Clear();
                        listView.Adapter = null;

                        if (txPesquisa.Text != "")
                        {
                            if (ckROMAN.Checked)
                            {
                                romaneio.ToList().ForEach((aux) =>
                                {
                                    if (aux.CG_PRODUTO_ID != 0)
                                    {
                                        var item = new ProdutoController().FindById(aux.CG_PRODUTO_ID);
                                        if (item != null)
                                            if (item.DSCPROD.ToLower().Contains(txPesquisa.Text.ToLower()))
                                                prod.Add(item);
                                    }
                                });
                            }
                            else
                                prod = (List<Produto>)new ProdutoController().FindByDSCPROD(txPesquisa.Text);
                        }
                        else
                        {
                            if (ckROMAN.Checked)
                            {
                                romaneio.ToList().ForEach((aux) =>
                                {
                                    if (aux.CG_PRODUTO_ID != 0)
                                    {
                                        var item = new ProdutoController().FindById(aux.CG_PRODUTO_ID);
                                        if (item != null)
                                            prod.Add(item);
                                    }
                                });
                            }
                            else
                                prod = (List<Produto>)new ProdutoController().FindAll();
                        }

                        listView.Adapter = new AdapterBuscarProduto(this, prod);
                    };

                    ckROMAN.CheckedChange += (s, a) =>
                    {
                        prod.Clear();
                        listView.Adapter = null;

                        if (txPesquisa.Text != "")
                        {
                            if (ckROMAN.Checked)
                            {
                                romaneio.ToList().ForEach((aux) =>
                                {
                                    if (aux.CG_PRODUTO_ID != 0)
                                    {
                                        var item = new ProdutoController().FindById(aux.CG_PRODUTO_ID);
                                        if (item != null)
                                            if (item.DSCPROD.ToLower().Contains(txPesquisa.Text.ToLower()))
                                                prod.Add(item);
                                    }
                                });
                            }
                            else
                                prod = (List<Produto>)new ProdutoController().FindByDSCPROD(txPesquisa.Text);

                        }
                        else
                        {
                            if (ckROMAN.Checked)
                            {
                                romaneio.ToList().ForEach((aux) =>
                                {
                                    if (aux.CG_PRODUTO_ID != 0)
                                    {
                                        var item = new ProdutoController().FindById(aux.CG_PRODUTO_ID);
                                        if (item != null)
                                            prod.Add(item);
                                    }
                                });
                            }
                            else
                                prod = (List<Produto>)new ProdutoController().FindAll();
                        }

                        listView.Adapter = new AdapterBuscarProduto(this, prod);
                    };

                    listView.ItemClick += (s, a) =>
                    {
                        var adapter = (AdapterBuscarProduto)listView.Adapter;
                        var produto = adapter[a.Position];

                        ResultCODPROD = true;
                        txCODPROD.Text = produto.CODPROD.ToString();
                        dialog.Cancel();
                        ShowKeyboard(txQtd);
                    };
                }

            };

            txQtd.TextChanged += (sender, eventArgs) =>
            {
                if (!string.IsNullOrEmpty(txPreco.Text))
                {
                    Produto p = new ProdutoController().FindByCODPROD(txCODPROD.Text.ToLong());

                    if (!ckCXPC.Checked)
                        txTotal.Text = Math.Round(txPreco.Text.ToDouble() * txQtd.Text.ToDouble(), 2).ToString("0.00").Replace(".", ",");
                    else
                        if (p != null)
                        txTotal.Text = Math.Round(txPreco.Text.ToDouble() * (p.QTDUNID * txQtd.Text.ToDouble()), 2).ToString("0.00").Replace(".", ",");
                }
                else
                    txTotal.Text = string.Empty;

                if (!string.IsNullOrEmpty(txQtd.Text) && !txQtd.Text.Equals("0"))
                    btnSalvarProd.Enabled = true;
                else
                    btnSalvarProd.Enabled = false;
            };

            txQtd.FocusChange += (sender, args) =>
            {
                if (args.HasFocus)
                {
                    txQtd.RequestFocus();
                    ShowKeyboard(txQtd);
                }
            };

            txNROPEDID.LongClick += (sender, eventArgs) =>
            {
                if (new PedidoController().FindAll().Count == 0)
                    this.Msg("NENHUM PEDIDO REGISTRADO!");
                else
                {
                    Intent i = new Intent(ApplicationContext, typeof(BuscarPedidoView));
                    StartActivityForResult(i, 3);
                    ResultNROPEDID = true;
                }

            };

            btnSalvarProd.Click += (sender, eventArgs) =>
            {
                if (string.IsNullOrEmpty(txCODPROD.Text))
                    this.Msg("NECESSÁRIO INFORMAR PRODUTO!");
                else if (string.IsNullOrEmpty(txQtd.Text))
                    this.Msg("NECESSÁRIO INFORMAR QUANTIDADE!");
                else
                {
                    if (new RomaneioController().GetRomaneio.Count > 0)
                        SalvarProd();
                    else
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("AVISO DO SISTEMA");
                        builder.SetMessage("NÃO HÁ NENHUM ROMANEIO CARREGADO, DESEJA FAZER A CARGA ROMANEIO?");

                        builder.SetPositiveButton("SIM", (s, e) =>
                        {
                            Task.Run(() => CargaRomaneio());
                        });
                        builder.SetNegativeButton("NÃO", (s, e) => { return; });
                        AlertDialog alertDialog = builder.Create();
                        alertDialog.Show();
                    }
                }
            };

            listView.ItemLongClick += (sender, args) =>
            {
                /* Evento para um item selecionado na lista -> tanto para exclusão como atualização */

                var adapter = (AdapterItensPedido)listView.Adapter;
                var item = adapter[args.Position];

                Produto produto = new ProdutoController().FindByCODPROD(item.CODPROD.ToLong());

                txCODPROD.Text = item.CODPROD;
                txNOMPROD.Text = item.NOMPROD;
                txQtd.Text = item.QTDPROD.ToString();
                txPreco.Text = item.VLRUNIT.ToString("N2");
                txTotal.Text = (item.QTDPROD * item.VLRUNIT).ToString("N2");

                CODPROD = long.Parse(txCODPROD.Text);

                if (CODPROD != null)
                    btnExcluirProd.Enabled = true;

                Position = args.Position;

                btnSalvarProd.Text = "Atualizar";

                txPreco.Enabled = true;
                txPreco.Focusable = true;
            };

            btnExcluirProd.Click += (sender, args) =>
            {
                /* Remove um produto da lista e devolve a mesma atualizada */

                if (CODPROD != null)
                {
                    if (Position != null)
                        RemoveProd(Position.Value);

                    CODPROD = null;
                    Position = null;
                    LimparDadosProduto();
                    btnExcluirProd.Enabled = false;
                    btnSalvarProd.Text = "Salvar";
                }
                else
                    this.Msg("NENHUM PRODUTO SELECIONADO!");

                if (this.ItensPedido.Count > 0)
                {
                    btnSalvar.Enabled = true;
                    txDSCOBSER.Enabled = true;
                }
            };

            txPreco.TextChanged += (sender, args) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(txPreco.Text))
                        txTotal.Text = Math.Round(txPreco.Text.ToDouble() * txQtd.Text.ToDouble(), 2)
                        .ToString("0.00").Replace(".", ",");
                    else
                        txTotal.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    GetError(ex.ToString());
                }
            };

            txPreco.FocusChange += (s, a) =>
            {
                if (!a.HasFocus)
                    if (!string.IsNullOrEmpty(txPreco.Text))
                    {
                        double valor = txPreco.Text.ToDouble();
                        txPreco.Text = valor.ToString("N2");

                        txPreco.SetSelectAllOnFocus(true);

                        Produto produto = new ProdutoController().FindByCODPROD(txCODPROD.Text.ToLong());

                        if (produto != null)
                        {
                            if (valor < produto.PRCVENDA)
                            {
                                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                builder.SetTitle("AVISO");
                                builder.SetMessage($"NÃO É PERMITIDO INCLUIR UM PREÇO ABAIXO DO PREÇO PADRÃO ({produto.PRCVENDA.ToString("C2")})");
                                builder.SetCancelable(false);
                                builder.SetNeutralButton("OK", (s, a) =>
                                {
                                    txPreco.Text = produto.PRCVENDA.ToString();
                                    txPreco.RequestFocus();
                                    return;
                                });
                                AlertDialog dialog = builder.Create();
                                dialog.Show();
                            }
                        }
                    }
            };

            btnLimpar.Click += (sender, args) =>
            {
                A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                builder.SetTitle("AVISO DO SISTEMA");
                builder.SetMessage("DESEJA MESMO LIMPAR DADOS ?");
                builder.SetPositiveButton("OK", (s, a) =>
                {
                    codpess = "";
                    LimparTela();
                });
                builder.SetNegativeButton("CANCELAR", (s, a) => { return; });
                A.AlertDialog alert = builder.Create();
                alert.Show();
            };

            btnSalvar.Click += (sender, args) =>
            {
                Pedido p;
                var romaneio = new RomaneioController().FindLast();
                if (romaneio != null)
                {
                    if (romaneio.SITROMAN == (short)Romaneio.SitRoman.Aberto)
                    {
                        if ((p = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong())) != null)
                        {
                            if (p.INDSINC) // indica se já foi transmitido para o servidor
                            {
                                new DialogFactory().CreateDialog(this,
                                    title: "AVISO DO SISTEMA",
                                    message: "ESTE PEDIDO JÁ FOI TRANSMITIDO, DESEJA CONTINUAR ?",
                                    positiveButtonName: "OK",
                                    positiveAction: () => SalvarPedido(),
                                    negativeButtonName: "CANCELAR",
                                    negativeAction: () => { return; });
                            }
                            else
                                SalvarPedido();
                        }
                        else
                        {
                            var sController = new SequenciaController();

                            if (sController.Sequencia != null)
                            {
                                double valorTotal = lbTotal.Text.Substring(3).ToDouble();

                                long NROPEDID;
                                long NROAT = sController.Sequencia.NROPEDAT;

                                if (NROAT == 0)
                                    NROPEDID = sController.Sequencia.NROPEDIN;
                                else
                                    NROPEDID = NROAT + 1;

                                if ((p = new PedidoController().FindByNROPEDID(NROPEDID)) != null)
                                {
                                    new DialogFactory().CreateDialog(this,
                                    title: "AVISO DO SISTEMA",
                                    message: $"NÃO FOI POSSÍVEL SALVAR O PEDIDO !!!\nTENTE NOVAMENTE",
                                    neutralButtonName: "OK",
                                    neutralAction: () => 
                                    {
                                        if (TestConection())
                                        {
                                            Task.Run(() =>
                                            {
                                                RunOnUiThread(() =>
                                                {
                                                    BloquearMenus = true;
                                                    progressBar.Visibility = ViewStates.Visible;
                                                });

                                                DNS dns = new ConfigController().GetDNS();
                                                SequenciaController sequenciaC = new SequenciaController();
                                                string id = new VendedorController().Vendedor.CG_VENDEDOR_ID.Value.ToString("D4");
                                                string strSequencia = $"CARGAVENDESEQ{id}000000";

                                                if (sequenciaC.Sequencia != null)
                                                {
                                                    DateTime DateTimeLastSEQ = sequenciaC.GetLastDateTime();
                                                    strSequencia = $"CARGAVENDESEQ{id}000000{DateTimeLastSEQ}";
                                                }
                                                sequenciaC.ComSocket(strSequencia, dns.Host, dns.Port);

                                                RunOnUiThread(() =>
                                                {
                                                    BloquearMenus = false;
                                                    progressBar.Visibility = ViewStates.Invisible;
                                                });
                                            });
                                        }
                                    });
                                }
                                else
                                    SalvarPedido();
                            }
                            else
                                SalvarPedido();
                        }
                    }
                    else
                        new DialogFactory().CreateDialog(this,
                                    "AVISO DO SISTEMA",
                                    "ROMANEIO ESTÁ FECHADO, FAVOR CARREGUE UM NOVO ROMANEIO",
                                    "OK",
                                    () => { return; });
                }
                else
                    new DialogFactory().CreateDialog(this,
                                    "AVISO DO SISTEMA",
                                    "NENHUM ROMANEIO CARREGADO, FAVOR CLIQUE EM CARGA ROMANEIO",
                                    "OK",
                                    () => { return; });

            };

            btnSend.Click += (s, a) =>
            {
                if (cvShare.Visibility == ViewStates.Invisible)
                {
                    cvShare.Visibility = ViewStates.Visible;
                    EnableView(false);
                    BloquearMenus = true;
                }
                else
                {
                    cvShare.Visibility = ViewStates.Invisible;
                    EnableView();
                    BloquearMenus = false;
                }
            };

            lblSHAREEMAIL.Click += (s, a) =>
            {
                cvShare.Visibility = ViewStates.Invisible;

                long id = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong()).FT_PEDIDO_ID.Value;
                sendOrder(id, "email");
                EnableView();
                BloquearMenus = false;
            };

            lblSHARESMS.Click += (s, a) =>
            {
                cvShare.Visibility = ViewStates.Invisible;

                long id = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong()).FT_PEDIDO_ID.Value;
                sendOrder(id, "sms");
                EnableView();
                BloquearMenus = false;
            };

            lblSHAREWPP.Click += (s, a) =>
            {
                cvShare.Visibility = ViewStates.Invisible;

                long id = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong()).FT_PEDIDO_ID.Value;
                sendOrder(id, "wpp");
                EnableView();
                BloquearMenus = false;
            };

            lblPDF.Click += (s, a) =>
            {
                cvShare.Visibility = ViewStates.Invisible;

                long id = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong()).FT_PEDIDO_ID.Value;
                sendOrder(id, "pdf");
                EnableView();
                BloquearMenus = false;
            };

            btnExcluir.Click += (sender, args) =>
            {
                if (new PedidoController().FindAll().Count > 0)
                {
                    if (txNROPEDID.Text != "" || txNROPEDID.Text != "0")
                    {
                        var pedido = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong());
                        if (pedido != null)
                        {
                            EditText strMTOCANC = new EditText(this);

                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("CANCELAMENTO DE PEDIDO");
                            builder.SetMessage("INFORME O MOTIVO DO CANCELAMENTO");
                            builder.SetView(strMTOCANC);
                            builder.SetPositiveButton("CONCLUIR", (s, a) =>
                            {
                                if (strMTOCANC.Text != "")
                                {
                                    if (new PedidoController().CancelarPedidoPorN(pedido.NROPEDID, strMTOCANC.Text, out string message))
                                    {
                                        Romaneio romaneio = new RomaneioController().FindLast();
                                        if (pedido.ES_ESTOQUE_ROMANEIO_ID == romaneio.ES_ESTOQUE_ROMANEIO_ID)
                                        {
                                            List<ItemPedido> itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);
                                            foreach (var item in itens)
                                            {
                                                RomaneioItem itemRoman = new RomaneioController().FindByIdItem(item.CG_PRODUTO_ID.Value);
                                                if (itemRoman != null)
                                                {
                                                    if (item.INDBRIND)
                                                        itemRoman.QTDBRINDE -= item.QTDATPRO;
                                                    else
                                                        itemRoman.QTDVENDA -= item.QTDATPRO;

                                                    itemRoman.DTHULTAT = DateTime.Now;


                                                    new RomaneioController().SaveItem(itemRoman);
                                                }
                                            }
                                        }

                                        BaixasPedido ba = null;
                                        if ((ba = new BaixasPedidoController().FindByFT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value)) != null)
                                            new BaixasPedidoController().Delete(ba);

                                        this.SnackMsg(message);
                                        LimparTela();
                                    }
                                }
                                else
                                    this.Msg("Erro ao cancelar ! Favor informar o motivo do cancelamento");
                            });
                            builder.SetNegativeButton("CANCELAR", (s, a) => { return; });
                            AlertDialog dialog = builder.Create();
                            dialog.Show();

                            strMTOCANC.RequestFocus();
                            ShowKeyboard(strMTOCANC);
                        }
                    }
                }
                else
                    this.Msg("NÃO HÁ NENHUM REGISTRO SALVO!");
            };

            txPreco.KeyPress += (sender, args) => Validations.DecimalEntry(txPreco, args);

            txTotal.TextChanged += (sender, args) =>
            {
                if (txTotal.Text.Contains('-'))
                    txTotal.SetTextColor(Color.Red);
                else
                    txTotal.SetTextColor(Color.ParseColor("#005500"));
            };

            //txNROPEDID.TextChanged += txNROPEDID_TextChanged;

            txNROPEDID.FocusChange += (s, a) =>
            {
                if (!a.HasFocus)
                {
                    if (txNROPEDID.Text.IsEmpty() || txNROPEDID.Text.IsBlank())
                        txNROPEDID.Text = "0";

                    if (!ValidarNROPEDID(out string msg))
                        this.Msg(msg);
                }
            };

            btnImprimir.Click += (sender, args) =>
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetCancelable(false);
                builder.SetTitle("IMPRESSÕES");
                builder.SetMessage("SELECIONE O QUE DESEJA IMPRIMIR");
                builder.SetPositiveButton("PEDIDO", (s, a) => Imprimir());
                builder.SetNegativeButton("DEVOLUÇÕES/ BAIXA", (s, a) => Imprimir(null, true));
                builder.SetNeutralButton("VOLTAR", (s, a) => { return; });
                AlertDialog dialog = builder.Create();
                dialog.Show();
                //Imprimir();
            };

            lbSITPEDID.TextChanged += (sender, args) => EnableViewSitPedido();

            floatingButton.Click += (sender, args) =>
            {
                progressBar.Visibility = ViewStates.Visible;
                EnableView(false);

                new Thread(() =>
                {
                    RunOnUiThread(() =>
                    {
                        this.Msg("ESTAMOS BAIXANDO A NOVA VERSÃO DO APLICATIVO, FAVOR AGUARDE !");

                        Task.Run(() =>
                        {
                            if (downLoadNewVersion())
                            {
                                RunOnUiThread(() =>
                                {
                                    try
                                    {
                                        FTPController fTPController = new FTPController();

                                        linearLayout1.Visibility = ViewStates.Invisible;

                                        this.Msg("DOWNLOAD REALIZADO COM SUCESSO !");

                                        Intent intent;

                                        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                                        {
                                            Java.IO.File apkPath = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware/EloPedidos.EloPedidos.apk");
                                            Android.Net.Uri apkURI = Android.Support.V4.Content.FileProvider.GetUriForFile(this, this.ApplicationContext.PackageName + ".EloPedidos.EloPedidos.provider", apkPath);

                                            intent = new Intent(Intent.ActionInstallPackage);
                                            intent.SetData(apkURI);
                                            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
                                        }
                                        else
                                        {
                                            Android.Net.Uri apkUri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware/EloPedidos.EloPedidos.apk"));
                                            intent = new Intent(Intent.ActionView);
                                            intent.SetDataAndType(apkUri, "application/vnd.android.package-archive");
                                            intent.SetFlags(ActivityFlags.NewTask);
                                        }
                                        StartActivity(intent);
                                        Finish();
                                    }
                                    catch (Exception e)
                                    {
                                        Log.Error("Elo_Log", e.ToString());
                                    }

                                    progressBar.Visibility = ViewStates.Invisible;
                                    EnableView(true);
                                });
                            }
                            else
                                RunOnUiThread(() => this.Msg("FALHA AO REALIZAR O DOWNLOAD DA ATUALIZAÇÃO! \nTENTE NOVAMENTE"));
                        });
                    });
                }).Start();
            };

            fab.Click += (s, a) =>
            {
                if (CheckPermission())
                    salvarItensByBC();
            };

            ckCXPC.CheckedChange += (s, a) =>
            {
                if (!txQtd.Text.IsEmpty() && !txCODPROD.Text.IsEmpty())
                {
                    Produto p = new ProdutoController().FindByCODPROD(txCODPROD.Text.ToLong());

                    if (!ckCXPC.Checked)
                        txTotal.Text = Math.Round(txPreco.Text.ToDouble() * txQtd.Text.ToDouble(), 2).ToString("0.00").Replace(".", ",");
                    else
                        if (p != null)
                        txTotal.Text = Math.Round(txPreco.Text.ToDouble() * (p.QTDUNID * txQtd.Text.ToDouble()), 2).ToString("0.00").Replace(".", ",");
                }
            };


            imgBAIXA.Click += (s, a) =>
            {
                Intent intent = new Intent(this, typeof(DevolucaoPedidoView));
                intent.PutExtra("CODPESS", txCODPESS.Text);
                StartActivity(intent);
            };

            imgESTOQUE.Click += (s, a) =>
            {
                if (new RomaneioController().GetRomaneio.Count > 0)
                {
                    Intent intent = new Intent(this, typeof(EstoqueView));
                    StartActivity(intent);
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("AVISO DO SISTEMA");
                    builder.SetMessage("NÃO HÁ NENHUM ROMANEIO CARREGADO, DESEJA FAZER A CARGA ROMANEIO?");
                    builder.SetCancelable(false);
                    builder.SetPositiveButton("SIM", (s, e) => { Task.Run(() => CargaRomaneio()); });
                    builder.SetNegativeButton("NÃO", (s, e) => { return; });
                    AlertDialog alertDialog = builder.Create();
                    alertDialog.Show();
                }

            };

            imgAGENDA.Click += (s, a) =>
            {
                Intent intent = new Intent(this, typeof(AgendaView));
                StartActivity(intent);
            };

            imgCADASTRO.Click += (s, a) =>
            {
                Intent intent = new Intent(this, typeof(ClienteView));
                StartActivity(intent);
            };

            #endregion

            #region reloadlistview
            if (savedInstanceState != null)
            {
                if (savedInstanceState.GetStringArray("rec_LISTAPROD") != null)
                {
                    listView.Adapter = null;

                    var itensP = savedInstanceState.GetStringArray("rec_LISTAPROD");

                    if (itensP.Length > 0)
                    {
                        itensP.ToList().ForEach(p =>
                        {
                            string[] dados = p.Split(" - ");

                            Produto produto = new ProdutoController().FindByCODPROD(dados[0].ToLong());
                            string fIDTUNID = string.IsNullOrEmpty(produto.IDTUNID) ? "" : "x " + produto.IDTUNID;

                            bool INDBRINDE = (dados[3] == "0") ? false : true;

                            ItensPedido.Add(new ItemPedido()
                            {
                                CG_PRODUTO_ID = produto.CG_PRODUTO_ID.Value,
                                FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                                CODPROD = dados[0],
                                NOMPROD = produto.DSCPROD,
                                IDTUNID = produto.IDTUNID,
                                QTDUNID = dados[1].ToDouble(),
                                QTDPROD = dados[1].ToDouble(),
                                VLRUNIT = dados[2].ToDouble(),
                                INDBRIND = INDBRINDE,
                                SITPEDID = 1,
                                Produto = produto
                            });

                            var adapter = new AdapterItensPedido(this, ItensPedido);
                            listView.Adapter = adapter;
                        });
                    }
                }
            }
            #endregion
        }

        protected override void OnResume()
        {
            var config = new ConfigController().GetConfig();
            if (config.CODEAN)
                fab.Visibility = ViewStates.Visible;
            else
                fab.Visibility = ViewStates.Invisible;

            base.OnResume();
        }
        private bool validarCliente(Pessoa pessoa)
        {
            bool result = true;

            if (pessoa.IDTDCPES == 2)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage("CLIENTE SEM DOCUMENTO CADASTRADO\nFAVOR CADASTRAR O NUMERO DE DOCUMENTO");
                builder.SetCancelable(false);
                builder.SetPositiveButton("IR PARA O CADASTRO", (s, a) =>
                {
                    Intent intent = new Intent(this, typeof(ClienteView));
                    intent.PutExtra("CODPESS", txCODPESS.Text);
                    StartActivity(intent);
                });
                builder.SetNegativeButton("OK", (s, a) =>
                {
                    return;
                });
                AlertDialog dialog = builder.Create();
                dialog.Show();

                result = false;
            }
            else if (pessoa.INDINAT)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage("CLIENTE ESTÁ INATIVO\nNÃO É POSSIVEL EMITIR PEDIDO PARA ELE");
                builder.SetCancelable(false);
                builder.SetNeutralButton("OK", (s, a) => { return; });
                AlertDialog dialog = builder.Create();
                dialog.Show();

                result = false;
            }

            return result;
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
        private async void salvarItensByBC()
        {
            Produto p = await capturarBarCodeAsync();
            if (p != null)
            {
                double qtdprod = 0;
                if (ckCXPC.Checked)
                    qtdprod = p.QTDUNID;
                else
                    qtdprod = 1;

                if (listView.Adapter != null)
                {
                    var adapter = (AdapterItensPedido)listView.Adapter;
                    if (adapter.Count > 0)
                    {
                        for (int i = 0; i < adapter.Count; i++)
                        {
                            var item = adapter[i];

                            if (item.CODPROD == p.CODPROD.ToString())
                            {
                                RomaneioItem romItem = new RomaneioController().FindByIdItem(p.CG_PRODUTO_ID.Value);
                                double qtdroman = romItem.QTDPROD - romItem.QTDVENDA + romItem.QTDDEVCL;

                                if (qtdroman >= (item.QTDPROD + qtdprod))
                                {
                                    long? fFT_PEDIDO_ITEM_ID = item.FT_PEDIDO_ITEM_ID;

                                    ItensPedido.RemoveAt(i);
                                    ItensPedido.Insert(i, new ItemPedido()
                                    {
                                        FT_PEDIDO_ITEM_ID = fFT_PEDIDO_ITEM_ID,
                                        CG_PRODUTO_ID = p.CG_PRODUTO_ID,
                                        FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                                        CODPROD = p.CODPROD.ToString(),
                                        NOMPROD = p.DSCPROD,
                                        IDTUNID = p.IDTUNID,
                                        QTDPROD = (item.QTDPROD + qtdprod),
                                        QTDUNID = (item.QTDPROD + qtdprod),
                                        VLRUNIT = p.PRCVENDA,
                                        INDBRIND = false,
                                        SITPEDID = 1,
                                        Produto = p
                                    });

                                    listView.Adapter = null;
                                    listView.Adapter = new AdapterItensPedido(this, ItensPedido);

                                    salvarItensByBC();
                                }
                                else
                                {
                                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                    builder.SetTitle("AVISO");
                                    builder.SetMessage("QUANTIDADE MAIOR DO QUE A ENCONTRADA NO ESTOQUE!\nNÃO FOI POSSÍVEL SALVAR O PRODUTO");
                                    builder.SetNeutralButton("OK", (s, e) =>
                                    {
                                        txCODPROD.Text = "";
                                        txQtd.Text = "";
                                        txCODPROD.RequestFocus();
                                        return;
                                    });

                                    AlertDialog alertDialog = builder.Create();
                                    alertDialog.Show();
                                }
                            }
                        }
                    }
                    else
                    {
                        RomaneioItem romItem = new RomaneioController().FindByIdItem(p.CG_PRODUTO_ID.Value);
                        double qtdroman = romItem.QTDPROD - romItem.QTDVENDA + romItem.QTDDEVCL;

                        if (qtdroman >= qtdprod)
                        {
                            ItensPedido.Add(new ItemPedido()
                            {
                                CG_PRODUTO_ID = p.CG_PRODUTO_ID,
                                FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                                CODPROD = p.CODPROD.ToString(),
                                NOMPROD = p.DSCPROD,
                                IDTUNID = p.IDTUNID,
                                QTDUNID = qtdprod,
                                QTDPROD = qtdprod,
                                VLRUNIT = p.PRCVENDA,
                                INDBRIND = false,
                                SITPEDID = 1,
                                Produto = p
                            });

                            listView.Adapter = null;
                            listView.Adapter = new AdapterItensPedido(this, ItensPedido);

                            salvarItensByBC();
                        }
                        else
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("AVISO");
                            builder.SetMessage("QUANTIDADE MAIOR DO QUE A ENCONTRADA NO ESTOQUE!\nNÃO FOI POSSÍVEL SALVAR O PRODUTO");
                            builder.SetNeutralButton("OK", (s, e) =>
                            {
                                txCODPROD.Text = "";
                                txQtd.Text = "";
                                txCODPROD.RequestFocus();
                                return;
                            });

                            AlertDialog alertDialog = builder.Create();
                            alertDialog.Show();
                        }
                    }
                }
                else
                {
                    RomaneioItem romItem = new RomaneioController().FindByIdItem(p.CG_PRODUTO_ID.Value);
                    double qtdroman = romItem.QTDPROD - romItem.QTDVENDA + romItem.QTDDEVCL;

                    if (qtdroman >= qtdprod)
                    {
                        ItensPedido.Add(new ItemPedido()
                        {
                            CG_PRODUTO_ID = p.CG_PRODUTO_ID,
                            FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                            CODPROD = p.CODPROD.ToString(),
                            NOMPROD = p.DSCPROD,
                            IDTUNID = p.IDTUNID,
                            QTDUNID = qtdprod,
                            QTDPROD = qtdprod,
                            VLRUNIT = p.PRCVENDA,
                            INDBRIND = false,
                            SITPEDID = 1,
                            Produto = p
                        });

                        listView.Adapter = null;
                        listView.Adapter = new AdapterItensPedido(this, ItensPedido);

                        salvarItensByBC();
                    }
                    else
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("AVISO");
                        builder.SetMessage("QUANTIDADE MAIOR DO QUE A ENCONTRADA NO ESTOQUE!\nNÃO FOI POSSÍVEL SALVAR O PRODUTO");
                        builder.SetNeutralButton("OK", (s, e) =>
                        {
                            txCODPROD.Text = "";
                            txQtd.Text = "";
                            txCODPROD.RequestFocus();
                            return;
                        });

                        AlertDialog alertDialog = builder.Create();
                        alertDialog.Show();
                    }
                }
            }
        }

        private void loadToReceive()
        {
            txtATRASADO.Text = string.Empty;
            lblVLRABRT.Text = string.Empty;

            PedidoController pController = new PedidoController();

            try
            {
                lblVLRABRT.Visibility = ViewStates.Invisible;
                txtATRASADO.Visibility = ViewStates.Invisible;

                Pessoa p = new PessoaController().FindByCODPESS(long.Parse(txCODPESS.Text));
                double valorAbatimento = pController.totalReceberCliente(p.CG_PESSOA_ID.Value, out string[] _Pedidos);
                if (valorAbatimento != 0)
                {
                    lblVLRABRT.Visibility = ViewStates.Visible;
                    int count = new PedidoController().FindAllToReceive(p.CG_PESSOA_ID.Value).Count;
                    if (count > 1)
                        lblVLRABRT.Text = $"EXITEM {count} PEDIDOS EM ABERTO NO VALOR DE {valorAbatimento.ToString("C2")}";
                    else
                        lblVLRABRT.Text = $"EXITE {count} PEDIDO EM ABERTO NO VALOR DE {valorAbatimento.ToString("C2")}";
                }

                double valorAtrasado = 0;
                var pedidosAtrasados = pController.Atrasados(p.CG_PESSOA_ID.Value, out valorAtrasado);
                if (pedidosAtrasados.Count > 0)
                {
                    txtATRASADO.Visibility = ViewStates.Visible;
                    if (pedidosAtrasados.Count > 1)
                        txtATRASADO.Text = $"EXITEM {pedidosAtrasados.Count} PEDIDOS ATRASADOS NO VALOR DE {valorAtrasado.ToString("C2")}";
                    else
                        txtATRASADO.Text = $"EXITE {pedidosAtrasados.Count} PEDIDO ATRASADO NO VALOR DE {valorAtrasado.ToString("C2")}";
                }
            }
            catch (Exception e)
            {
                Log.Error("Elo_Log", e.Message);
                txtATRASADO.Text = string.Empty;
                lblVLRABRT.Text = string.Empty;
                lblVLRABRT.Visibility = ViewStates.Invisible;
                txtATRASADO.Visibility = ViewStates.Invisible;
            }
        }

        private bool CheckPermission()
        {
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, 6);
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Flashlight) != Android.Content.PM.Permission.Granted)
                Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Flashlight }, 7);

            return true;
        }

        private async Task<Produto> capturarBarCodeAsync()
        {
            ZXing.Result result = null;
            Produto produto = null;
            ProdutoController pController = new ProdutoController();
            MediaPlayer _player;

            string topText = "";

            if (listView.Adapter != null)
            {
                var adapter = (AdapterItensPedido)listView.Adapter;
                if (adapter.Count > 0)
                {
                    for (int i = 0; i < adapter.Count; i++)
                    {
                        var item = adapter[i];
                        Produto p = new ProdutoController().FindByCODPROD(item.CODPROD.ToLong());
                        if (p != null)
                            topText = $"{topText}{p.DSCPROD} - {item.QTDPROD}\n";
                    }
                }
            }

            try
            {
                MobileBarcodeScanner scanner = new MobileBarcodeScanner(this);

                MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();
                options.UseFrontCameraIfAvailable = true;
                scanner.TopText = topText;

                result = await scanner.Scan();
            }
            catch (Exception e)
            {
                Log.Error("BarCode_Exception", e.ToString());
            }
            finally
            {
                if (result != null)
                {
                    _player = MediaPlayer.Create(this, Resource.Raw.beep);
                    _player.Start();

                    var aux = result.Text;
                    produto = new ProdutoController().FindProdutoByBC(aux);
                    if (produto != null)
                    {
                        RomaneioItem r = new RomaneioController().FindByIdItem(produto.CG_PRODUTO_ID.Value);
                        if (r == null)
                        {
                            produto = null;

                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("ALERTA");
                            builder.SetMessage($"PRODUTO NÃO ENCONTRADO NO ROMANEIO");
                            builder.SetPositiveButton("OK", (sender, args) => { return; });
                            AlertDialog dialog = builder.Create();
                            dialog.Show();
                        }
                    }
                }
            }
            return produto;
        }

        public void SalvarProd()
        {
            Produto p = new ProdutoController().FindByCODPROD(long.Parse(txCODPROD.Text));
            int auxPosition = 0;
            bool isnew = true;
            double saldo = 0;
            double vlrPreco = txPreco.Text.ToDouble();
            double vlrVenda = p.PRCVENDA;

            double quantidade = 0;
            if (!ckCXPC.Checked)
                quantidade = double.Parse(txQtd.Text);
            else
                quantidade = double.Parse(txQtd.Text) * p.QTDUNID;

            var adp = (AdapterItensPedido)this.listView.Adapter;

            if (adp != null)
                if (adp.Count > 0)
                    for (int i = 0; i < adp.Count; i++)
                    {
                        var item = adp[i];

                        if (item.CODPROD == p.CODPROD.ToString())
                        {
                            isnew = false;
                            auxPosition = i;
                            break;
                        }
                    }

            RomaneioItem romaneioItem = null;
            if ((romaneioItem = new RomaneioController().FindByIdItem(p.CG_PRODUTO_ID.Value)) == null)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage("PRODUTO NÃO ENCONTRADO NO ROMANEIO!");
                builder.SetCancelable(false);
                builder.SetPositiveButton("OK", (s, e) =>
                {
                    txCODPROD.Text = "";
                    txQtd.Text = "";
                    txCODPROD.RequestFocus();
                });
                AlertDialog alertDialog = builder.Create();
                alertDialog.Show();
            }
            else if (quantidade > (romaneioItem.QTDPROD - (romaneioItem.QTDVENDA + romaneioItem.QTDBRINDE) + romaneioItem.QTDDEVCL))
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage("QUANTIDADE MAIOR DO QUE A ENCONTRADA NO ESTOQUE!\nNÃO FOI POSSÍVEL SALVAR O PRODUTO");
                builder.SetCancelable(false);
                builder.SetNeutralButton("OK", (s, e) =>
                {
                    txQtd.SelectAll();
                    txQtd.SetSelectAllOnFocus(true);
                    txQtd.RequestFocus();
                    return;
                });

                AlertDialog alertDialog = builder.Create();
                alertDialog.Show();
            }
            else if (txPreco.Text == "0,00" || string.IsNullOrEmpty(txPreco.Text))
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage("NECESSÁRIO INFORMAR O PREÇO DESSE PRODUTO!");
                builder.SetPositiveButton("OK", (s, e) =>
                {
                    txCODPROD.Text = "";
                    txQtd.Text = "";
                    txCODPROD.RequestFocus();

                });
                AlertDialog alertDialog = builder.Create();
                alertDialog.Show();
            }
            else if (vlrPreco < vlrVenda)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage($"NÃO É PERMITIDO INCLUIR UM PREÇO ABAIXO DO PREÇO PADRÃO ({p.PRCVENDA.ToString("C2")})");
                builder.SetCancelable(false);
                builder.SetNeutralButton("OK", (s, a) =>
                {
                    txPreco.Text = p.PRCVENDA.ToString();
                    txPreco.RequestFocus();
                    return;
                });
                AlertDialog dialog = builder.Create();
                dialog.Show();
            }
            else if (isnew)
            {
                ItensPedido.Add(new ItemPedido()
                {
                    CG_PRODUTO_ID = p.CG_PRODUTO_ID,
                    FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                    CODPROD = txCODPROD.Text,
                    NOMPROD = txNOMPROD.Text,
                    IDTUNID = p.IDTUNID,
                    QTDUNID = quantidade,
                    QTDPROD = quantidade,
                    VLRUNIT = txPreco.Text.ToDouble(),
                    INDBRIND = ckBrinde.Checked,
                    SITPEDID = 1,
                    Produto = p
                });

                var adapter = new Adapter.AdapterItensPedido(this, ItensPedido);
                listView.Adapter = adapter;
            }
            /* Senão, atualiza a lista com as alterações nos produtos */
            else
            {
                var codProd = txCODPROD.Text;
                var nomprod = txNOMPROD.Text;
                var valorprod = txPreco.Text.ToDouble();
                bool isbrindE = ckBrinde.Checked;

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("AVISO");
                builder.SetMessage("ESSE ITEM JÁ ESTÁ NO LISTA\nDESEJA ATUALIZAR OS DAODS OU SALVAR COMO UM NOVO ITEM?");
                builder.SetNeutralButton("CANCELAR", (s, a) => { return; });
                builder.SetPositiveButton("SALVAR", (s, a) =>
                {
                    ItensPedido.Add(new ItemPedido()
                    {
                        CG_PRODUTO_ID = p.CG_PRODUTO_ID,
                        FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                        CODPROD = codProd,
                        NOMPROD = nomprod,
                        IDTUNID = p.IDTUNID,
                        QTDUNID = quantidade,
                        QTDPROD = quantidade,
                        VLRUNIT = valorprod,
                        INDBRIND = isbrindE,
                        SITPEDID = 1,
                        Produto = p
                    });

                    var adapter = new Adapter.AdapterItensPedido(this, ItensPedido);
                    listView.Adapter = adapter;

                    ListTotalValue();
                });
                builder.SetNegativeButton("ATUALIZAR", (s, a) =>
                {
                    long? fFT_PEDIDO_ITEM_ID = ItensPedido[auxPosition].FT_PEDIDO_ITEM_ID;
                    ItensPedido.RemoveAt(auxPosition);
                    ItensPedido.Insert(auxPosition, new ItemPedido()
                    {
                        FT_PEDIDO_ITEM_ID = fFT_PEDIDO_ITEM_ID,
                        CG_PRODUTO_ID = p.CG_PRODUTO_ID,
                        FT_PEDIDO_ID = this.FT_PEDIDO_ID,
                        IDTUNID = p.IDTUNID,
                        CODPROD = codProd,
                        NOMPROD = nomprod,
                        QTDPROD = quantidade,
                        QTDUNID = quantidade,
                        VLRUNIT = valorprod,
                        INDBRIND = isbrindE,
                        SITPEDID = 1,
                        Produto = p
                    });

                    var adapter = new Adapter.AdapterItensPedido(this, ItensPedido);
                    listView.Adapter = adapter;

                    ListTotalValue();
                });
                builder.SetCancelable(false);
                AlertDialog dialog = builder.Create();
                dialog.Show();
            }

            if (this.ItensPedido.Count > 0)
            {
                txDSCOBSER.Enabled = true;
                btnSalvar.Enabled = true;
            }

            LimparDadosProduto();

            ListTotalValue();

            btnSalvarProd.Text = "SALVAR";

            NextFocus(txCODPROD);
        }
        public void Imprimir(long? id = null, bool isDEVOLUCAO = false)
        {
            Task.Run(() =>
            {
                try
                {
                    if (id != null || this.FT_PEDIDO_ID != null || isDEVOLUCAO)
                    {
                        if (txDevice.Text != string.Empty)
                        {
                            try
                            {
                                RunOnUiThread(() => this.Msg("ENVIANDO IMPRESSÃO PARA O DISPOSITIVO! AGUARDE..."));

                                var printerController = new PrinterController();
                                string text;
                                Pedido p;
                                if (id == null)
                                    p = new PedidoController().FindById(this.FT_PEDIDO_ID.Value);
                                else
                                    p = new PedidoController().FindById(id.Value);

                                if (p != null)
                                {
                                    if (!isDEVOLUCAO)
                                        text = printerController.FormatOrderForPrintA7(p);
                                    else
                                        text = printerController.FormatOrderForPrintA7(p, "devolucao");

                                    string aux = text;
                                    string segVIA = text;

                                    BluetoothSocket socket = printerController.GetSocket(txDevice.Text);

                                    if (socket != null)
                                    {
                                        if (!socket.IsConnected)
                                            printerController.ConnectPrinter(socket, txDevice.Text);

                                        if (socket.IsConnected)
                                        {
                                            bool isImpressaoOk = true;
                                            try
                                            {
                                                text = "1 VIA \n" + aux + "\n\n";
                                                socket.OutputStream.Write(text.ToASCII(), 0, text.ToASCII().Length);
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
                                                        b.SetPositiveButton("SIM", (s, a) => { Imprimir(id, isDEVOLUCAO); });
                                                        b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                                        b.SetCancelable(false);
                                                        AlertDialog dialog = b.Create();
                                                        dialog.Show();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log.Debug("ERROR", ex.ToString());
                                                    }
                                                    SisLog.Logger(ex.ToString(), "IMPRESSAO");
                                                });
                                            }

                                            if (isImpressaoOk)
                                                RunOnUiThread(() =>
                                                {
                                                    try
                                                    {
                                                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                                        builder.SetTitle("IMPRESSÕES !!!");
                                                        builder.SetMessage("GOSTARIA DE IMPRIMIR A 2° VIA ?");
                                                        builder.SetPositiveButton("SIM", (s, a) =>
                                                        {
                                                            try
                                                            {
                                                                text = "2 VIA \n" + segVIA + "\n\n";
                                                                socket.OutputStream.Write(text.ToASCII(), 0, text.ToASCII().Length);
                                                                printerController.ClosePrinter();
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
                                                                        b.SetPositiveButton("SIM", (s, a) => { Imprimir(id, isDEVOLUCAO); });
                                                                        b.SetNegativeButton("CANCELAR", (s, a) => { return; });
                                                                        b.SetCancelable(false);
                                                                        AlertDialog dialog = b.Create();
                                                                        dialog.Show();
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Log.Debug("ERROR", ex.ToString());
                                                                    }
                                                                    SisLog.Logger(ex.ToString(), "IMPRESSAO");
                                                                });
                                                            }
                                                        });
                                                        builder.SetNegativeButton("NÃO", (s, a) =>
                                                        {
                                                            printerController.ClosePrinter();
                                                            return;
                                                        });
                                                        AlertDialog dialog = builder.Create();
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
                                                    b.SetMessage("FAVOR, LIGUE A IMPRESSORA!\nTENTAR NOVAMENTE ?");
                                                    b.SetPositiveButton("SIM", (s, a) => { Imprimir(id, isDEVOLUCAO); });
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
                                                b.SetPositiveButton("SIM", (s, a) => { Imprimir(id, isDEVOLUCAO); });
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
                                            b.SetMessage("PEDIDO NÃO ENCONTRADO !\nTENTAR NOVAMENTE ?");
                                            b.SetPositiveButton("SIM", (s, a) => { Imprimir(id, isDEVOLUCAO); });
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
                            catch (Exception e)
                            {
                                RunOnUiThread(() =>
                                {
                                    SisLog.Logger(e.ToString(), "ERRO_IMPRESSAO");
                                });
                            }
                        }
                        else
                            RunOnUiThread(() =>
                            {
                                try
                                {
                                    AlertDialog.Builder b = new AlertDialog.Builder(this);
                                    b.SetTitle("AVISO");
                                    b.SetMessage("NENHUM DISPOSITIVO DE IMPRESSÃO SELECIONADO!\nTENTAR NOVAMENTE ?");
                                    b.SetPositiveButton("SIM", (s, a) => { Imprimir(id, isDEVOLUCAO); });
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
                                b.SetMessage("NENHUM PEDIDO SELECIONADO!");
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
                catch (Exception e)
                {
                    RunOnUiThread(() =>
                    {
                        SisLog.Logger(e.ToString(), "ERRO_IMPRESSAO");
                    });
                }
            });
        }
        public static void ShowKeyboard(EditText editText)
        {
            InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(editText, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
            editText.SelectAll();
        }
        public static void HideKeyboard(EditText editText)
        {
            InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, HideSoftInputFlags.None);
            editText.ClearFocus();
        }
        public void SelectText(EditText editText)
        {
            if (editText.IsFocused == true)
                editText.SelectAll();
        }

        /// <summary>
        ///  Remover um elemento do ListView
        /// </summary>
        /// <param name="position"></param>
        private void RemoveProd(int position)
        {
            try
            {
                ItensPedido.RemoveAt(position);
                listView.Adapter = null;
                listView.Adapter = new AdapterItensPedido(this, ItensPedido);

                ListTotalValue();
                EnableControlsPedid();
                NextFocus(txCODPROD);
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        /// <summary>
        ///  Retorna para o textView o valor total da soma da lista de produtos
        /// </summary>
        private void ListTotalValue()
        {
            try
            {
                if (listView.Adapter != null)
                {
                    double valor = 0;

                    for (int i = 0; i < listView.Adapter.Count; i++)
                    {
                        if (!ItensPedido[i].INDBRIND)
                        {
                            var adapter = (AdapterItensPedido)listView.Adapter;
                            var item = adapter[i];

                            valor += (item.QTDPROD * item.VLRUNIT);
                        }
                    }
                    lbTotal.Text = Math.Round(valor, 2).ToString("C");
                }
            }
            catch (Exception ex)
            {
                Log.Error(Ext.LOG_APP, ex.Message);
                GetError(ex.ToString());
            }
        }

        /// <summary>
        ///  Limpa os campos referentes ao cadastro de produto para o pedido
        /// </summary>
        private void LimparDadosProduto()
        {
            txCODPROD.Text = string.Empty;
            txQtd.Text = string.Empty;
            txPreco.Text = string.Empty;
            txTotal.Text = string.Empty;
            txtSALDO.Text = string.Empty;
            ckBrinde.Checked = false;
        }

        /// <summary>
        ///  Libera os controles da tela se houver um pedido na mesma ou se houver items na lista
        /// </summary>
        private void EnableControlsPedid()
        {
            try
            {
                if (FT_PEDIDO_ID != null)
                    if (listView.Adapter != null)
                        if (listView.Adapter.Count > 0)
                        {
                            btnExcluir.Enabled = true;
                            btnImprimir.Enabled = true;
                        }
                        else
                        {
                            btnExcluir.Enabled = false;
                            btnImprimir.Enabled = false;
                        }
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        /// <summary>
        ///  Habilita ou desabilita os controles da tela
        /// </summary>
        /// <param name="enable"></param>
        private void EnableControls(bool enable = true)
        {
            txQtd.Enabled = enable;
            ckBrinde.Enabled = enable;
        }

        /// <summary>
        ///  Limpa todos os dados da tela
        /// </summary>
        private void LimparTela()
        {
            try
            {
                for (int i = 0; i < RLayout.ChildCount; i++)
                {
                    if (RLayout.GetChildAt(i).GetType() == typeof(EditText))
                    {
                        var field = (EditText)RLayout.GetChildAt(i);
                        field.Text = string.Empty;
                    }
                    else if (RLayout.GetChildAt(i).GetType() == typeof(TextInputEditText))
                    {
                        var field = (TextInputEditText)RLayout.GetChildAt(i);
                        field.Text = string.Empty;
                    }
                }

                FT_PEDIDO_ID = null;
                CODPROD = null;
                Position = null;
                listView.Adapter = null;
                adapterProducts = null;
                ID_PESS = null;

                txData.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime dateEntry = DateTime.Parse(txData.Text);
                txtDATARET.Text = dateEntry.AddMonths(1).ToString("dd/MM/yyyy");
                lbTotal.Text = "0,00";
                btnSalvarProd.Text = "Salvar";

                txDSCOBSER.Text = string.Empty;
                ckPrazo.Checked = true;
                btnImprimir.Enabled = true;
                ckPrazo.Enabled = false;

                txCODPROD.Text = string.Empty;
                txCODPESS.Text = string.Empty;
                txNOMFANTA.Text = string.Empty;
                txQtd.Text = string.Empty;
                txPreco.Text = string.Empty;
                txTotal.Text = string.Empty;
                ckBrinde.Checked = false;
                ckCXPC.Checked = false;
                txtATRASADO.Text = string.Empty;
                lblVLRABRT.Text = string.Empty;
                lblVLRABRT.Visibility = ViewStates.Invisible;
                txtATRASADO.Visibility = ViewStates.Invisible;
                imgBAIXA.Enabled = false;
                txNROPEDID.Text = "0";
                rbCOMISSAO.Checked = true;

                if (this.CurrentFocus != null)
                    this.CurrentFocus.ClearFocus();

                txCODPESS.RequestFocus();

                this.ItensPedido.Clear();
                this.ItensExclusao.Clear();

                btnSend.Visibility = ViewStates.Invisible;

                lbSITPEDID.Text = string.Empty;

                cvShare.Visibility = ViewStates.Invisible;
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        /// <summary>
        ///  Habilita / Desabilita a tela de pedidos baseado na situação do pedido
        /// </summary>
        /// <param name="enable"></param>
        public void EnableViewSitPedido()
        {
            bool enable = false;

            switch (lbSITPEDID.Text)
            {
                case "Aberto":
                    enable = true;
                    lbSITPEDID.SetTextColor(Color.ParseColor("#212121"));
                    break;
                case "Confirmado":
                    enable = true;
                    lbSITPEDID.SetTextColor(Color.ParseColor("#005500"));
                    break;
                case "Parcial Total":
                    enable = true;
                    lbSITPEDID.SetTextColor(Color.ParseColor("#005500"));
                    break;
                case "Atendido":
                    enable = false;
                    lbSITPEDID.SetTextColor(Color.ParseColor("#005500"));
                    break;
                case "Cancelado":
                    enable = false;
                    lbSITPEDID.SetTextColor(Color.ParseColor("#AA0000"));
                    break;
                case "":
                    enable = true;
                    break;
            }

            for (int i = 0; i < RLayout.ChildCount; i++)
                RLayout.GetChildAt(i).Enabled = enable;

            txNROPEDID.Enabled = btnLimpar.Enabled = btnExcluir.Enabled = btnImprimir.Enabled = true;

            txNOMPROD.Enabled = false;
            txtSALDO.Enabled = false;
        }


        public bool ValidarPedido()
        {
            bool result = true;


            Pessoa p = null;
            PessoaController controller = new PessoaController();
            BaixasPedidoController bController = new BaixasPedidoController();


            if (this.ID_PESS == null)
            {
                A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                builder.SetTitle("AVISO DO SISTEMA");
                builder.SetMessage("ERRO AO SALVAR PEDIDO! INFORME O CLIENTE.");
                builder.SetPositiveButton("OK", (s, a) =>
                {
                    NextFocus(txCODPESS);
                });
                builder.SetCancelable(false);
                A.AlertDialog alert = builder.Create();
                alert.Show();


                result = false;
            }
            else if (!ValidarNROPEDID(out string error))
            {
                A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                builder.SetTitle("AVISO DO SISTEMA");
                builder.SetMessage("ERRO AO SALVAR PEDIDO!\n" + error);
                builder.SetPositiveButton("OK", (s, a) =>
                {
                    NextFocus(txNROPEDID);
                });
                builder.SetCancelable(false);
                A.AlertDialog alert = builder.Create();
                alert.Show();

                result = false;
            }
            else if ((p = controller.FindById(this.ID_PESS.Value)) != null)
            {
                var sController = new SequenciaController();

                if (sController.Sequencia != null)
                {
                    double valorTotal = lbTotal.Text.Substring(3).ToDouble();

                    long FINDNROPED;
                    long NROAT = sController.Sequencia.NROPEDAT;

                    if (NROAT == 0)
                        FINDNROPED = sController.Sequencia.NROPEDIN;
                    else
                        FINDNROPED = NROAT + 1;


                    var baixa = new BaixasPedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong());

                    if (p.VLRLIMPD > 0 && p.VLRLIMPD < valorTotal)
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("AVISO");
                        builder.SetMessage("O VALOR DO PEDIDO ULTRAPASSOU O VALOR TOTAL DE LIMITE DO CLIENTE!");
                        builder.SetCancelable(false);
                        builder.SetNeutralButton("OK", (s, a) =>
                        {
                            NextFocus(txCODPESS);
                        });
                        AlertDialog dialog = builder.Create();
                        dialog.Show();

                        result = false;
                    }
                    else if (baixa != null)
                        if (baixa.VLRPGMT > valorTotal)
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("AVISO");
                            builder.SetMessage("ERRO! O VALOR ATUALIZADO DO PEDIDO É MENOR DO QUE O VALOR JÁ PAGO PELO CLIENTE. VERIFIQUE");
                            builder.SetCancelable(false);
                            builder.SetNeutralButton("OK", (s, a) =>
                            {
                                NextFocus(txCODPESS);
                            });
                            AlertDialog dialog = builder.Create();
                            dialog.Show();
                            progressBar.Visibility = ViewStates.Invisible;

                            result = false;
                        }
                    if (listView.Adapter != null)
                    {
                        if (listView.Adapter.Count == 0)
                        {
                            A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                            builder.SetTitle("AVISO DO SISTEMA");
                            builder.SetMessage("NENHUM PRODUTO INFORMADO! VERIFIQUE.");
                            builder.SetPositiveButton("OK", (s, a) => { NextFocus(txCODPROD); });
                            builder.SetCancelable(false);
                            A.AlertDialog alert = builder.Create();
                            alert.Show();

                            result = false;
                        }
                        else
                        {
                            DateTime dataEM = DateTime.Parse(txData.Text.ToString());
                            DateTime dataRE = DateTime.Parse(txtDATARET.Text.ToString());

                            if (dataEM < dataRE)
                            {
                                if (!validarDADOSPED(p.ID, new VendedorController().Vendedor.CG_VENDEDOR_ID, FINDNROPED, dataEM, dataRE, new OperadorController().Operador.USROPER))
                                {
                                    A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                                    builder.SetTitle("AVISO DO SISTEMA");
                                    builder.SetMessage("ERRO AO SALVAR PEDIDO! VERIFIQUE.");
                                    builder.SetPositiveButton("OK", (s, a) => { return; });
                                    builder.SetCancelable(false);
                                    A.AlertDialog alert = builder.Create();
                                    alert.Show();

                                    result = false;
                                }
                                else
                                {
                                    Pedido pedidoSalvo = null;

                                    if (this.FT_PEDIDO_ID != null)
                                        pedidoSalvo = new PedidoController().FindById(this.FT_PEDIDO_ID.Value);
                                    else
                                        pedidoSalvo = new PedidoController().FindByNROPEDID(txNROPEDID.Text.ToLong());

                                    if (pedidoSalvo != null)
                                    {
                                        if (pedidoSalvo.SITPEDID != 1)
                                        {
                                            A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                                            builder.SetTitle("AVISO DO SISTEMA");
                                            builder.SetMessage("PEDIDO NÃO ESTÁ EM ABERTO, NÃO É POSSIVÉL ALTERAR");
                                            builder.SetPositiveButton("OK", (s, a) => { return; });
                                            builder.SetCancelable(false);
                                            A.AlertDialog alert = builder.Create();
                                            alert.Show();

                                            result = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                                builder.SetTitle("AVISO DO SISTEMA");
                                builder.SetMessage("DATA DE RETORNO ESTÁ MENOR DO QUE A DATA DE EMISSÃO");
                                builder.SetPositiveButton("OK", (s, a) => { NextFocus(txtDATARET); });
                                builder.SetCancelable(false);
                                A.AlertDialog alert = builder.Create();
                                alert.Show();

                                result = false;
                            }
                        }
                    }
                    else
                    {
                        A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                        builder.SetTitle("AVISO DO SISTEMA");
                        builder.SetMessage("NENHUM PRODUTO INFORMADO! VERIFIQUE.");
                        builder.SetPositiveButton("OK", (s, a) => { NextFocus(txCODPROD); });
                        builder.SetCancelable(false);
                        A.AlertDialog alert = builder.Create();
                        alert.Show();

                        result = false;
                    }
                }
                else
                {
                    A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                    builder.SetTitle("AVISO DO SISTEMA");
                    builder.SetMessage("NÃO FOI POSSIVEL SALVAR O PEDIDO!\nFAVOR CARREGUE A SEQUENCIA DO VENDEDOR !");
                    builder.SetPositiveButton("OK", (s, a) => { return; });
                    builder.SetCancelable(false);
                    A.AlertDialog alert = builder.Create();
                    alert.Show();

                    result = false;
                }
            }
            else
            {
                A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                builder.SetTitle("AVISO DO SISTEMA");
                builder.SetMessage("CÓDIGO DE CLIENTE INVÁLIDO! VERIFIQUE.");
                builder.SetPositiveButton("OK", (s, a) => { NextFocus(txCODPESS); });
                builder.SetCancelable(false);
                A.AlertDialog alert = builder.Create();
                alert.Show();

                result = false;
            }

            return result;
        }

        /// <summary>
        ///  Método para salvar o pedido
        /// </summary>
        /// <returns></returns>
        private bool SalvarPedido()
        {
            bool result = false; // Retorno do método

            try
            {
                if (ValidarPedido())
                {
                    Database.GetConnection().RunInTransaction(() =>
                    {
                        PessoaController controller = new PessoaController();
                        PedidoController pController = new PedidoController();
                        BaixasPedidoController bController = new BaixasPedidoController();

                        Pessoa p = controller.FindById(this.ID_PESS.Value);

                        var sController = new SequenciaController();

                        if (sController.Sequencia != null)
                        {
                            double valorTotal = lbTotal.Text.Substring(3).ToDouble();

                            long NROPEDID;
                            long NROAT = sController.Sequencia.NROPEDAT;

                            if (NROAT == 0)
                                NROPEDID = sController.Sequencia.NROPEDIN;
                            else
                                NROPEDID = NROAT + 1;

                            DateTime dataEM = DateTime.Now;
                            DateTime dataRE = DateTime.Parse(txtDATARET.Text.ToString());

                            TimeSpan time = dataRE.Subtract(DateTime.Parse(dataEM.ToString("dd/MM/yyyy")));

                            Romaneio romaneio = new RomaneioController().FindLast();

                            if (NROPEDID <= sController.Sequencia.NROPEDFI)
                            {
                                Pedido pedido = new Pedido()
                                {
                                    CG_PESSOA_ID = (p != null) ? p.CG_PESSOA_ID : null,
                                    ID_PESSOA = (p != null) ? p.ID : null,
                                    CG_VENDEDOR_ID = new VendedorController().Vendedor.CG_VENDEDOR_ID,
                                    NOMPESS = txNOMFANTA.Text,
                                    NROPEDID = (txNROPEDID.Text.Equals("0") ? NROPEDID : txNROPEDID.Text.ToLong()),
                                    CODEMPRE = new EmpresaController().Empresa.CODEMPRE,
                                    DATEMISS = dataEM,
                                    DATERET = txtDATARET.Text.ToDate().Value,
                                    CODMUNIC = p.CODMUNIC.ToString(),
                                    DSCPRZPG = ckPrazo.Checked ? time.Days.ToString() : "0",
                                    IDTFRMPG = ckPrazo.Checked ? "1" : "0",
                                    DSCOBSER = txDSCOBSER.Text,
                                    SITPEDID = (short)Pedido.SitPedido.Aberto,
                                    ES_ESTOQUE_ROMANEIO_ID = romaneio.ES_ESTOQUE_ROMANEIO_ID,
                                    DTHULTAT = DateTime.Now,
                                    USRULTAT = new OperadorController().Operador.USROPER,
                                    INDSINC = false
                                };
                                if (radioGroup.CheckedRadioButtonId == Resource.Id.rbCOMISSAO)
                                    pedido.PERCOMIS = new EmpresaController().GetEmpresa().PERCOMIS;
                                else if (radioGroup.CheckedRadioButtonId == Resource.Id.rbBRINDE)
                                    pedido.PERCOMIS = 0;

                                bool atualizacao = false;

                                Pedido pedidoSalvo = null;

                                if (this.FT_PEDIDO_ID != null)
                                    pedidoSalvo = pController.FindById(this.FT_PEDIDO_ID.Value);

                                if (pedidoSalvo != null)
                                {
                                    pedido.FT_PEDIDO_ID = pedidoSalvo.FT_PEDIDO_ID;
                                    pedido.PERCOMIS = pedidoSalvo.PERCOMIS;
                                    pedido.SITPEDID = pedidoSalvo.SITPEDID;
                                    pedido.DATEMISS = pedidoSalvo.DATEMISS;

                                    atualizacao = true;
                                }

                                if (pController.Save(pedido))
                                {
                                    Task.Run(() =>
                                    {
                                        if (!atualizacao)
                                            new GeolocatorController().SaveOrderLocalization(pedido.FT_PEDIDO_ID.Value);
                                    });

                                    List<ItemPedido> auxItem = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value);

                                    if (atualizacao)
                                    {
                                        ///RETIRA AS ALTERAÇÕES FEITAS NO ROMANEIO PELO PEDIDO
                                        ItemPedido itemGravado = null;
                                        if (auxItem.Count > 0)
                                            auxItem.ForEach(i =>
                                            {
                                                Produto produto = new ProdutoController().FindByCODPROD(i.CODPROD.ToLong());
                                                RomaneioItem itemRomaneio = new RomaneioController().FindByIdItem(i.CG_PRODUTO_ID.Value);
                                                itemGravado = i;
                                                if (produto != null)
                                                {
                                                    if (itemRomaneio != null)
                                                    {
                                                        if (itemGravado.INDBRIND)
                                                            itemRomaneio.QTDBRINDE -= itemGravado.QTDATPRO;
                                                        else
                                                            itemRomaneio.QTDVENDA -= itemGravado.QTDATPRO;

                                                        itemRomaneio.DTHULTAT = DateTime.Now;

                                                        new RomaneioController().SaveItem(itemRomaneio);
                                                    }
                                                }

                                                new ItemPedidoController().Delete(itemGravado.FT_PEDIDO_ITEM_ID.Value);
                                            });
                                    }

                                    ItensPedido.ForEach((aux) =>
                                    {
                                        Produto produto = new ProdutoController().FindByCODPROD(aux.CODPROD.ToLong());
                                        RomaneioItem itemRomaneio = new RomaneioController().FindByIdItem(produto.CG_PRODUTO_ID.Value);

                                        aux.FT_PEDIDO_ID = pedido.FT_PEDIDO_ID;
                                        aux.QTDATPRO = aux.QTDPROD;
                                        aux.DTHULTAT = DateTime.Now;
                                        aux.USRULTAT = new OperadorController().Operador.USROPER;
                                        if (!new ItemPedidoController().Save(aux))
                                            this.Msg("ERRO AO SALVAR ITEMS! VERIFIQUE.");

                                        if (produto != null)
                                        {
                                            ///FAZ AS ALTERAÇÕES NO ROMANEIO PELO PEDIDO
                                            if (itemRomaneio != null)
                                            {
                                                if (aux.INDBRIND)
                                                    itemRomaneio.QTDBRINDE += aux.QTDPROD;
                                                else
                                                    itemRomaneio.QTDVENDA += aux.QTDPROD;

                                                itemRomaneio.DTHULTAT = DateTime.Now;

                                                RomaneioController eController = new RomaneioController();
                                                eController.SaveItem(itemRomaneio);
                                            }
                                        }
                                    });

                                    // Somente atualiza para o próximo número se não for uma atualização
                                    if (!atualizacao)
                                    {
                                        this.FT_PEDIDO_ID = pedido.FT_PEDIDO_ID;

                                        SequenciaDAO sDAO = new SequenciaDAO();
                                        var s = sDAO.GetSequencia();
                                        s.NROPEDAT = NROPEDID;
                                        sDAO.Save(s);
                                    }

                                    this.Msg("PEDIDO SALVO COM SUCESSO!");

                                    adapterProducts = null;
                                    result = true;

                                    RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);

                                    Task.Run(() =>
                                    {
                                        RunOnUiThread(() =>
                                        {
                                            ImprimirPedido(pedido.FT_PEDIDO_ID.Value);
                                            LimparTela();
                                            progressBar.Visibility = ViewStates.Invisible;
                                            VerificarSequencia();
                                        });
                                    });
                                }
                            }
                            else
                                RunOnUiThread(() =>
                                {
                                    new DialogFactory().CreateDialog(this,
                                        "ERRO AO SALVAR PEDIDO",
                                        "NÚMERO DO PEDIDO ATUAL MAIOR QUE O NÚMERO FINAL DA SEQUENCIA\nFAVOR INFORMAR O ADMINISTRADOR DA EMPRESA PARA ATUALIZAR O NÚMERO DA SEQUENCIA",
                                        "OK",
                                        () => { return; });
                                });
                        }
                    });

                    return result;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                AlertDialog.Builder b = new AlertDialog.Builder(this);
                b.SetTitle("AVISO");
                b.SetMessage(ex.Message);
                b.SetNeutralButton("OK", (s, a) => { return; });
                AlertDialog dialog = b.Create();
                dialog.Show();

                return false;
            }
            finally
            {
                progressBar.Visibility = ViewStates.Invisible;
            }
        }
        public void VerificarSequencia()
        {
            SequenciaController sController = new SequenciaController();
            var Sequencia = sController.Sequencia;
            if (Sequencia != null)
            {
                long dif = Sequencia.NROPEDFI - Sequencia.NROPEDAT;
                if (dif <= 5)
                {
                    RunOnUiThread(() =>
                    {
                        if (dif == 1)
                            new DialogFactory().CreateDialog(this,
                                "ALERTA DO SISTEMA",
                                $"FALTA {dif} PEDIDO PARA EXCEDER O LIMITE DA SEQUENCIA DO NUMERO DO PEDIDO\nFAVOR INFORMAR O ADMINISTRADOR DA EMPRESA PARA ATUALZIAR A SEQUENCIA",
                                "OK",
                                () => { return; });
                        else
                            new DialogFactory().CreateDialog(this,
                            "ALERTA DO SISTEMA",
                            $"FALTAM {dif} PEDIDOS PARA EXCEDER O LIMITE DA SEQUENCIA DO NUMERO DO PEDIDO\nFAVOR INFORMAR O ADMINISTRADOR DA EMPRESA PARA ATUALZIAR A SEQUENCIA",
                            "OK",
                            () => { return; });
                    });
                }
            }
        }

        private void ComSocket(Pedido pedido, bool isATT = false)
        {
            PedidoController pController = new PedidoController();
            BaixasPedidoController bController = new BaixasPedidoController();

            Task.Run(() =>
            {
                RunOnUiThread(() =>
                {
                    try
                    {
                        if (new ConfigController().TestServerConnection())
                        {
                            Task.Run(() =>
                            {
                                if (!pController.ComSocket(pedido, out string msg))
                                {
                                    try
                                    {
                                        if (msg == "TEMPO PARA SINCRONIZAR PEDIDO ESGOTADO")
                                            this.SnackMsg("TEMPO PARA SINCRONIZAR PEDIDO ESGOTADO !");
                                        else
                                            new DialogFactory().CreateDialog(this,
                                                "AVISO DO SISTEMA",
                                                "ERRO AO SINCRONIZAR PEDIDO!\n" + msg,
                                                "OK",
                                                () => { return; });
                                    }
                                    catch (Exception e)
                                    {
                                        SisLog.Logger(e.ToString(), "sinc devolucao");
                                    }
                                }
                                else
                                {
                                    BaixasPedido ba = null;
                                    if (isATT)
                                        if ((ba = new BaixasPedidoController().FindByFT_PEDIDO_ID(pedido.FT_PEDIDO_ID.Value)) != null)
                                            new BaixasPedidoController().Delete(ba);

                                    BaixasPedido baixa = bController.GerarBaixa(pedido);
                                    baixa.INDSINC = true;
                                    bController.Save(baixa);

                                    this.SnackMsg("PEDIDO SINCRONIZADO COM O SERVIDOR !");
                                }
                            });
                        }
                        else
                            this.SnackMsg("PEDIDO NÃO SINCRONIZADO COM O SERVIDOR !");
                    }
                    catch (Exception e)
                    {
                        Log.Error(Utils.Ext.LOG_APP, e.ToString());
                    }
                    finally
                    {
                        RunOnUiThread(() =>
                        {
                            ImprimirPedido(pedido.FT_PEDIDO_ID.Value);
                            progressBar.Visibility = ViewStates.Invisible;
                            LimparTela();
                        });
                    }
                });
            });

        }

        private bool validarDADOSPED(long? cgPESSOA, long? cgVENDEDOR, long nroPEDIDO, DateTime dataemiss, DateTime dataret, string usuario)
        {
            if (cgPESSOA != null &&
                cgVENDEDOR != null &&
                nroPEDIDO != 0 &&
                dataemiss < dataret &&
                usuario != "")
                return true;
            else
                return false;
        }


        public bool ValidarNROPEDID(out string message)
        {
            var sequencia = new SequenciaController().Sequencia;
            if (sequencia != null)
            {
                long nropedido = txNROPEDID.Text.ToLong();

                if (sequencia.NROPEDAT != 0 &&
                    !txNROPEDID.Text.Equals("0") &&
                    !txNROPEDID.Text.IsEmpty() &&
                    sequencia.NROPEDIN > sequencia.NROPEDAT)
                {
                    message = $"O NÚMERO DO PEDIDO É MENOR QUE O NÚMERO INICIAL DA SEQUENCIA: ({sequencia.NROPEDIN}) !";
                    return false;
                }
                else if (sequencia.NROPEDAT != 0 &&
                    !txNROPEDID.Text.Equals("0") &&
                    !txNROPEDID.Text.IsEmpty() &&
                    sequencia.NROPEDFI < sequencia.NROPEDAT + 1)
                {
                    message = $"O NÚMERO DO PEDIDO É MAIOR QUE O NÚMERO FINAL DA SEQUENCIA: ({sequencia.NROPEDFI}) !";
                    return false;
                }
                else if (new PedidoController().FindByNROPEDID(nropedido) == null &&
                    !txNROPEDID.Text.Equals("0") &&
                    (nropedido != 0))
                {
                    message = $"O NÚMERO DO PEDIDO É INVÁLIDO!";
                    return false;
                }
                else
                {
                    message = string.Empty;
                    return true;
                }
            }
            else
            {
                message = $"FAVOR CARREGAR A SEQUENCIA DO VENDEDOR!";
                return false;
            }
        }

        public void SincronizarPedidos()
        {
            if (new ConfigController().TestServerConnection())
            {
                List<Pedido> pedidosToSinc = new PedidoController().FindAllNotSync();
                pedidosToSinc.ForEach(p =>
                {
                    if (!new PedidoController().ComSocket(p, out string error))
                    {
                        RunOnUiThread(() =>
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle($"NÃO FOI POSSÍVEL SINCRONIZAR O PEDIDO {p.NROPEDID}!");
                            builder.SetMessage($"{error}");
                            builder.SetNeutralButton("OK", (s, a) => { return; });
                            builder.SetPositiveButton("EXCLUIR PEDIDO", (s, a) =>
                            {
                                List<ItemPedido> itens = new ItemPedidoController().FindAllOrderItems(p.FT_PEDIDO_ID.Value);
                                itens.ForEach(i => new ItemPedidoController().Delete(i.FT_PEDIDO_ITEM_ID.Value));
                                new PedidoDAO().Delete(p.FT_PEDIDO_ID.Value);
                            });
                            AlertDialog dialog = builder.Create();
                            dialog.Show();
                        });
                    }
                });
                List<Pedido> cancel = new PedidoController().CanceledNotSync;
                if (cancel.Count > 0)
                {
                    cancel.Where(p => p.INDCANC && !p.SYNCCANC).ToList().ForEach(p =>
                    {
                        Task.Run(() =>
                        {
                            new PedidoController().syncCancellation(p);
                        });
                    });
                }
                string message = "";
                List<Pessoa> pessoas = new PessoaController().FindAll().Where(p => !p.INDSINC).ToList();
                if (pessoas.Count > 0)
                {
                    pessoas.ForEach(p =>
                    {
                        if (!new PessoaController().SincPessoa(p, out message))
                        {
                            RunOnUiThread(() =>
                            {
                                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                                builder.SetTitle("NÃO FOI POSSÍVEL SALVAR O CLIENTE !");

                                if (message.ToLower().Contains("pessoa já cadastrada"))
                                    builder.SetMessage($"{p.NOMFANTA}\n{message}\nO CADASTRO FOI EXCLUIDO");
                                else
                                {
                                    builder.SetMessage($"{p.NOMFANTA}\n{message}");
                                    builder.SetPositiveButton("EXCLUIR CADASTRO", (s, a) => { new PessoaController().Delete(p.ID); });
                                }
                                builder.SetNeutralButton("OK", (s, a) => { return; });
                                AlertDialog dialog = builder.Create();
                                dialog.Show();
                            });
                        }
                    });
                }
            }
        }

        /// <summary>
        ///  Carrega os dados para a view atravéz do número do pedido
        /// </summary>
        /// <param name="pNROPEDID"></param>
        private void LoadDataByNROPEDID(long pNROPEDID)
        {
            try
            {
                Pedido p;
                if ((p = new PedidoController().FindByNROPEDID(pNROPEDID)) != null)
                {
                    LimparTela();

                    p.Pessoa = new PessoaController().FindById(p.ID_PESSOA.Value);

                    this.FT_PEDIDO_ID = p.FT_PEDIDO_ID;
                    this.ID_PESS = p.ID_PESSOA;

                    txNROPEDID.Text = p.NROPEDID.ToString();
                    txCODPESS.Text = $"{p.Pessoa.CODPESS}";
                    ckPrazo.Checked = p.DSCPRZPG.Equals("0") ? false : true;
                    txDSCOBSER.Text = p.DSCOBSER;
                    txData.Text = p.DATEMISS.ToString("dd/MM/yyyy");
                    txtDATARET.Text = p.DATERET.ToString("dd/MM/yyyy");

                    string situacao = Enum.GetName(typeof(SitPedido), p.SITPEDID);
                    lbSITPEDID.Text = situacao.Equals("ParcialTotal") ? "Parcial Total" : situacao;

                    List<ItemPedido> itens = new ItemPedidoController()
                        .FindAllOrderItems(p.FT_PEDIDO_ID.Value);

                    this.ItensPedido.Clear();

                    ItensPedido = itens;

                    this.listView.Adapter = null;

                    listView.Adapter = new AdapterItensPedido(this, ItensPedido);

                    ListTotalValue();

                    btnSalvar.Enabled = true;
                    btnImprimir.Enabled = true;
                    txDSCOBSER.Enabled = true;

                    btnSend.Visibility = ViewStates.Visible;

                    EnableView();
                }
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        /// <summary>
        ///  Limpa o foco do elemento atual
        /// </summary>
        private void FocusClear()
        {
            if (this.CurrentFocus != null)
                this.CurrentFocus.ClearFocus();
        }

        /// <summary>
        ///  Passa o foco para o próximo elemento desejado!
        /// </summary>
        /// <param name="v"></param>
        private void NextFocus(View v)
        {
            FocusClear();
            v.RequestFocus();
        }

        /// <summary>
        /// Abre um dialog com opção de impressão de pedido referente ao ID carregado na View
        /// </summary>
        private void ImprimirPedido(long id)
        {
            new DialogFactory().CreateDialog(context: this,
                title: "IMPRESSAO",
                message: "PEDIDO SALVO COM SUCESSO! DESEJA IMPRIMIR ?",
                positiveButtonName: "SIM",
                positiveAction: () =>
                {
                    Imprimir(id);
                },
                negativeButtonName: "NÃO",
                negativeAction: () => { return; });
        }

        /// <summary>
        ///  Cria o menu para o app
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            MenuInflater menuInflater = this.MenuInflater;
            menuInflater.Inflate(Resource.Menu.main_menu, menu);

            Operador operador = new OperadorController().GetOperador();
            if (!operador.DSCFUNC.Contains("ADM"))
                menu.GetItem(5).SetEnabled(false);
            else
                menu.GetItem(5).SetEnabled(true);

            return true;
        }

        /// <summary>
        ///  Controla a ação do botão ao ser clicado (Menu)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (BloquearMenus)
            {
                this.Msg("POR FAVOR, AGUARDE!");
                return false;
            }

            switch (item.ItemId)
            {
                case Resource.Id.cargaRomaneio:
                    {

                        if (new RomaneioController().FindAll().Count == 0 || new RomaneioController().FindAll() == null)
                            Task.Run(() => CargaRomaneio());
                        else
                        {
                            A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                            builder.SetTitle("CARGA ROMANEIO");
                            builder.SetMessage($"DESEJA EXCLUIR O ROMANEIO N°{new RomaneioController().FindLast().NROROMAN} E CARREGAR UM NOVO ?");
                            builder.SetPositiveButton("SIM", (s, a) =>
                            {
                                Task.Run(() => new Sincronizador().SincronizarAllNotSync()).Wait();

                                try
                                {
                                    Database.RunInTransaction(() =>
                                    {
                                        var conn = Database.GetConnection();
                                        List<Pedido> pedidos = new PedidoController().FindAll().Where(p => p.DATEMISS.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                                        List<BaixasPedido> baixas = new BaixasPedidoController().FindAll().Where(p => p.DTHULTAT.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                                        List<Pagamento> listaBaixas = new BaixasPedidoController().FindAllBaixas().Where(p => p.DTHULTAT.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                                        List<DevolucaoItem> devolucoes = new DevolucaoItemController().FindAll().Where(p => p.DTHULTAT.ToString("dd/MM/yyyy") != DateTime.Now.ToString("dd/MM/yyyy")).ToList();

                                        if (pedidos.Count > 0)
                                            pedidos.ForEach(p =>
                                            {
                                                List<ItemPedido> itens = new ItemPedidoController().FindItemsBy_FT_PEDIDO_ID(p.FT_PEDIDO_ID.Value);
                                                if (itens.Count > 0)
                                                    itens.ForEach(i => conn.Delete(i));

                                                conn.Delete(p);
                                            });

                                        if (baixas.Count > 0)
                                            baixas.ForEach(b => conn.Delete(b));
                                        if (listaBaixas.Count > 0)
                                            listaBaixas.ForEach(lb => conn.Delete(lb));
                                        if (devolucoes.Count > 0)
                                            devolucoes.ForEach(d => conn.Delete(d));
                                    });
                                }
                                catch (Exception e)
                                {
                                    Log.Error("CARGAROMANEIO", e.ToString());
                                    Toast.MakeText(this, "ERRO AO EXCLUIR OS PEDIDOS ANTIGOS", ToastLength.Long).Show();
                                }

                                Empresa empresa = new EmpresaController().GetEmpresa();
                                double vendedor = new VendedorController().GetVendedor().CG_VENDEDOR_ID.Value;

                                Task.Run(() => CargaRomaneio());
                            });
                            builder.SetNegativeButton("NÃO", (s, a) =>
                            {
                                return;
                            });
                            A.AlertDialog alert = builder.Create();
                            alert.Show();
                            NextFocus(txCODPESS);
                        }
                        break;
                    }
                case Resource.Id.configuracao:
                    {
                        Intent configuracao = new Intent(this, typeof(Views.ConfigView));
                        StartActivity(configuracao);
                        break;
                    }
                case Resource.Id.atualizar:
                    {
                        AtualizarDados();
                        break;
                    }
                case Resource.Id.consultarPedidos:
                    {
                        Intent consulta = new Intent(this, typeof(Views.BuscarPedidoView));

                        if (new PedidoController().Count > 0)
                            StartActivityForResult(consulta, 3);
                        else
                            this.Msg("NÃO EXISTEM PEDIDOS SALVOS!");

                        break;
                    }
                case Resource.Id.relatorioPedido:
                    {
                        if (new PedidoController().Count > 0)
                        {
                            Intent relatorio = new Intent(this, typeof(ResumoPedidoView));
                            StartActivity(relatorio);
                        }
                        else
                            this.Msg("NÃO EXISTEM PEDIDOS SALVOS!");

                        break;
                    }
                case Resource.Id.relatorioEstoque:
                    {
                        if (new RomaneioController().GetRomaneio.Count > 0)
                        {
                            Intent relatorioEstoque = new Intent(this, typeof(RelatorioRomaneioView));
                            StartActivityForResult(relatorioEstoque, 3);
                        }
                        else
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetTitle("AVISO DO SISTEMA");
                            builder.SetMessage("NÃO HÁ NENHUM ROMANEIO CARREGADO, DESEJA FAZER A CARGA ROMANEIO?");

                            builder.SetPositiveButton("SIM", (s, e) =>
                            {
                                Task.Run(() => CargaRomaneio());
                            });
                            builder.SetNegativeButton("NÃO", (s, e) => { return; });
                            AlertDialog alertDialog = builder.Create();
                            alertDialog.Show();
                        }
                        break;
                    }
                case Resource.Id.relatorioEmissao:
                    {
                        if (new PedidoController().Count > 0)
                        {
                            Intent relatorioemissao = new Intent(this, typeof(RelatorioEmissaoView));
                            StartActivityForResult(relatorioemissao, 3);
                        }
                        else
                            this.Msg("NÃO EXISTEM PEDIDOS SALVOS!");

                        break;
                    }
                case Resource.Id.relatorioDevolucoes:
                    {
                        if (new DevolucaoItemController().FindAll().Count > 0)
                        {
                            if (new PedidoController().Count > 0)
                            {
                                Intent relatorioDevolucoes = new Intent(this, typeof(RelatorioDevolucoesView));
                                StartActivity(relatorioDevolucoes);
                            }
                        }
                        else
                            this.Msg("NÃO EXISTE NENHUMA DEVOLUÇÃO SALVA");

                        break;
                    }
                case Resource.Id.relatorioBaixa:
                    {
                        if (new BaixasPedidoController().FindAllBaixas().Count > 0)
                        {
                            Intent intent = new Intent(this, typeof(RelatorioBaixasView));
                            StartActivity(intent);
                        }
                        else
                            this.Msg("NÃO EXISTE NENHUMA BAIXA SALVA");

                        break;
                    }
                case Resource.Id.goRotas:
                    {
                        Intent intent = new Intent(this, typeof(VendedorView));
                        StartActivity(intent);
                        break;
                    }
            }

            return true;
        }

        private void CargaRomaneio()
        {
            var controller = new ConfigController();
            Task.Run(() =>
            {
                RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);
                RunOnUiThread(() => EnableView(false));

                if (controller.TestServerConnection())
                {
                    DNS dns = controller.GetDNS();
                    EmpresaController empresaC = new EmpresaController();
                    Empresa empresa = empresaC.Empresa;
                    VendedorController vendedorC = new VendedorController();
                    Vendedor vendedor = vendedorC.Vendedor;
                    SequenciaController sequenciaC = new SequenciaController();
                    OperadorController operadorC = new OperadorController();

                    string id = vendedor.CG_VENDEDOR_ID.Value.ToString("D4");

                    new AgendaController().ComSocketOrders($"CARGAPEDIDO{empresa.CODEMPRE}{id}      {DateTime.Now.ToString("dd/MM/yyyy")}000000");

                    RomaneioController eController = new RomaneioController();

                    if (eController.ComSocket($"CARGAROMANEIO{empresa.CODEMPRE}{id}000000", dns.Host, dns.Port))
                        RunOnUiThread(() => this.Msg("CARGA ROMANEIO REALIZADA COM SUCESSO."));
                    else
                        RunOnUiThread(() => this.Msg("ERRO AO SINCRONIZAR ROMANEIO! CONTATAR O ADMINISTRADOR."));
                }
                else
                    RunOnUiThread(() => this.Msg("SEM CONEXÃO COM O SERVIDOR"));

                RunOnUiThread(() =>
                {
                    progressBar.Visibility = ViewStates.Invisible;
                    EnableView(true);
                });
            });
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            ///  --> CLIENTE/ PRODUTO
            if (requestCode == 1)
                if (resultCode == Result.Ok)
                {
                    string result = data.GetStringExtra("result");

                    if (!string.IsNullOrEmpty(result))
                        txCODPESS.Text = result;

                    string resultProd = data.GetStringExtra("resultProd");

                    if (!string.IsNullOrEmpty(resultProd))
                        txCODPROD.Text = resultProd;
                }

            // Código 3 --> carregar para a view
            if (requestCode == 3)
                if (resultCode == Result.Ok)
                {
                    string result = data.GetStringExtra("resultPedido");
                    if (!string.IsNullOrEmpty(result))
                        LoadDataByNROPEDID(long.Parse(result));
                }

            // --> IMPRESSORA
            if (requestCode == 4)
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
                                M.Config config = configController.GetConfig();

                                config.NOMIMPRE = resultImpressao;
                                configController.Save(config);
                            },
                            negativeButtonName: "NÃO",
                            negativeAction: () => { return; });

                    }
                }
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
                                M.Config config = configController.GetConfig();

                                config.NOMIMPRE = name;
                                configController.Save(config);
                                txDevice.Text = name;

                                this.SavePreference(name, "NOME");
                            },
                            negativeButtonName: "NÃO",
                            negativeAction: () => { return; });
        }

        private void GetError(string message)
        {
            string error = "";
            Log.Error(error, message);
            this.Msg(message);
        }

        /// <summary>
        ///  Atualiza os dados do app com o servidor
        /// </summary>
        protected virtual void AtualizarDados()
        {
            bool loop = false;

            new Thread(() =>
            {
                try
                {
                    var controller = new ConfigController();
                    var config = controller.Config;
                    DNS dns = controller.GetDNS();

                    int count = 0;
                    loop = true;
                    BloquearMenus = true;

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
                                //Task.Run(() => SincronizarPedidos()).Wait();
                                Task.Run(() => new Sincronizador().SincronizarAllNotSync()).Wait();

                                this.Msg("SINCRONIZANDO DADOS COM O SERVIDOR! AGUARDE...");

                                EmpresaController empresaC = new EmpresaController();

                                Empresa empresa = empresaC.Empresa;
                                VendedorController vendedorC = new VendedorController();
                                Vendedor vendedor = vendedorC.Vendedor;
                                SequenciaController sequenciaC = new SequenciaController();
                                OperadorController operadorC = new OperadorController();

                                string id = vendedor.CG_VENDEDOR_ID.Value.ToString("D4");
                                string sequencia = "000000";

                                EmpresaController eController = new EmpresaController();
                                if (eController.ComSocket($"CARGAEMPRESA{empresa.CODEMPRE}", dns.Host, dns.Port))
                                {
                                    Operador operador = operadorC.Operador;

                                    if (operadorC.ComSocket($"CARGAOPERADOR{empresa.CODEMPRE}{operador.USROPER}", dns.Host, dns.Port))
                                    {
                                        if (vendedorC.ComSocket($"CARGAVENDEDOR{empresa.CODEMPRE}{operador.USROPER}", dns.Host, dns.Port))
                                        {
                                            string strSequencia = $"CARGAVENDESEQ{id}000000";

                                            if (sequenciaC.Sequencia != null)
                                            {
                                                DateTime DateTimeLastSEQ = sequenciaC.GetLastDateTime();
                                                strSequencia = $"CARGAVENDESEQ{id}000000{DateTimeLastSEQ}";
                                            }
                                            if (sequenciaC.ComSocket(strSequencia, dns.Host, dns.Port))
                                            {
                                                MunicipioController municipioC = new MunicipioController();
                                                string strMunicipio = $"CARGAMUNICIPIO";

                                                if (municipioC.FindAll().Count > 0)
                                                {
                                                    DateTime DateTimeLastMUNIC = municipioC.GetLastDateTime();
                                                    strMunicipio = $"CARGAMUNICIPIO{sequencia}{DateTimeLastMUNIC}";
                                                }

                                                if (municipioC.ComSocket(strMunicipio, dns.Host, dns.Port))
                                                {
                                                    ProdutoController produtoC = new ProdutoController();
                                                    string strProduto = $"CARGAPRODUTO{empresa.CODEMPRE}{id}";
                                                    if (produtoC.FindAll().Count > 0)
                                                    {
                                                        DateTime DateTimeLastPROD = produtoC.GetLastDateTime();
                                                        strProduto = $"CARGAPRODUTO{empresa.CODEMPRE}{id}{DateTimeLastPROD}";
                                                    }

                                                    if (produtoC.ComSocket(strProduto, dns.Host, dns.Port))
                                                    {
                                                        PessoaController pessoaC = new PessoaController();
                                                        string strPessoa = $"CARGAPESSOA{empresa.CODEMPRE}{id}";
                                                        if (pessoaC.FindAll().Count > 0)
                                                        {
                                                            DateTime DateTimeLastPESS = pessoaC.GetLastDateTime();
                                                            strPessoa = $"CARGAPESSOA{empresa.CODEMPRE}{id}{sequencia}{DateTimeLastPESS}";
                                                        }

                                                        if (pessoaC.ComSocket(strPessoa, dns.Host, dns.Port))
                                                        {
                                                            string strVencimento = $"CARGAPESSOAVCTO{empresa.CODEMPRE}{id}";
                                                            VencimentoController vencimentoC = new VencimentoController();

                                                            if (vencimentoC.GetVencimentos().Count > 0)
                                                            {
                                                                DateTime DateTimeLastVCTO = vencimentoC.GetLastDateTime();
                                                                strVencimento = $"CARGAPESSOAVCTO{empresa.CODEMPRE}{id}{sequencia}{DateTimeLastVCTO}";
                                                            }
                                                            if (vencimentoC.ComSocket(strVencimento, dns.Host, dns.Port))
                                                            {
                                                                this.Msg("SINCRONIA COM SERVIDOR REALIZADA COM SUCESSO!");
                                                            }
                                                            else
                                                                this.Msg("ERRO AO SINCRONIZAR VENCIMENTOS! VERIFIQUE.");
                                                        }
                                                        else
                                                            this.Msg("ERRO AO SINCRONIZAR CLIENTES! VERIFIQUE.");
                                                    }
                                                    else
                                                        this.Msg("ERRO AO SINCRONIZAR PRODUTOS! VERIFIQUE.");
                                                }
                                                else
                                                    this.Msg("ERRO AO SINCRONIZAR MUNICIPIO! VERIFIQUE.");
                                            }
                                            else
                                                this.Msg("ERRO AO SINCRONIZAR SEQUÊNCIA DE PEDIDOS! VERIFIQUE.");
                                        }
                                        else
                                            this.Msg("ERRO AO SINCRONIZAR O VENDEDOR! VERIFIQUE.");
                                    }
                                    else
                                        this.Msg("ERRO AO SINCRONIZAR O OPERADOR! VERIFIQUE.");
                                }
                                else
                                    this.Msg("ERRO AO SINCRONIZAR A EMPRESA! VERIFIQUE.");
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    GetError(ex.ToString());
                }
                finally
                {
                    RunOnUiThread(() =>
                    {
                        loop = false;
                        BloquearMenus = false;
                        progressBar.SetProgress(0, false);
                        progressBar.Visibility = ViewStates.Invisible;
                        EnableView(true);

                        LimparTela();

                        btnSalvarProd.Enabled = false;
                        btnExcluirProd.Enabled = false;
                        btnSalvar.Enabled = false;
                        btnImprimir.Enabled = false;

                        txPreco.Enabled = false;
                        txTotal.EnableView(false);
                        txQtd.Enabled = false;
                        txQtd.Enabled = false;

                        ckPrazo.Enabled = false;
                        ckBrinde.Enabled = false;
                        txDSCOBSER.Enabled = false;
                        ckCXPC.Checked = false;
                    });
                }
            }).Start();
        }

        /// <summary>
        ///  Habilita ou desabilita os controles da tela
        /// </summary>
        /// <param name="enable"></param>
        protected void EnableView(bool enable = true)
        {
            try
            {
                for (int i = 0; i < RLayout.ChildCount; i++)
                {
                    if (RLayout.GetChildAt(i).GetType() == typeof(EditText))
                    {
                        var field = (EditText)RLayout.GetChildAt(i);
                        field.Enabled = enable;
                    }

                    if (RLayout.GetChildAt(i).GetType() == typeof(Button))
                    {
                        var button = (Button)RLayout.GetChildAt(i);
                        button.Enabled = enable;
                    }
                }

                listView.Enabled = enable;
                txNOMFANTA.Enabled = false;
                txNOMPROD.Enabled = false;
                txtSALDO.Enabled = false;
                btnSend.Enabled = true;

                txNROPEDID.Enabled = enable;
                txCODPESS.Enabled = enable;
                txCODPROD.Enabled = enable;
                txtDATARET.Enabled = enable;
                txData.Enabled = false;
                txQtd.Enabled = enable;
                txDSCOBSER.Enabled = enable;
                ckBrinde.Enabled = enable;
                ckCXPC.Enabled = enable;
                ckPrazo.Enabled = false;
                btnExcluirProd.Enabled = enable;
                btnSalvarProd.Enabled = enable;
                btnSalvar.Enabled = enable;
                btnExcluir.Enabled = enable;
                btnLimpar.Enabled = enable;
                btnImprimir.Enabled = enable;

                if (enable)
                    BloquearMenus = false;
                else
                    BloquearMenus = true;
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        private bool TestConection()
        {
            if (!new ConfigController().TestServerConnection())
            {
                RunOnUiThread(() => cv_radius.SetCardBackgroundColor(Android.Graphics.Color.ParseColor("Red")));
                return false;
            }
            else
            {
                RunOnUiThread(() => cv_radius.SetCardBackgroundColor(Android.Graphics.Color.ParseColor("Green")));
                return true;
            }
        }

        private void versionController()
        {
            if (new ConfigController().TestServerConnection("elosoftware.dyndns.org", 8560))
            {
                try
                {
                    DateTime curretnVersion = DateTime.Parse(new FTPController().getCurrentVersion());
                    DateTime apkVersion = DateTime.Parse(new FTPController().getApkVersion()).AddHours(3);
                    DateTime appVersion = FTPController.getAppVersion(out string version).AddHours(3);

                    if (curretnVersion > appVersion)
                        linearLayout1.Visibility = ViewStates.Visible;
                    else
                        linearLayout1.Visibility = ViewStates.Invisible;
                }
                catch (Exception ex)
                {
                    Log.Error("Elo_Log", ex.ToString());
                }
            }
        }
        private bool downLoadNewVersion()
        {
            bool result = false;
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 1);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 2);

            if (new ConfigController().TestServerConnection("elosoftware.dyndns.org", 8560))
                if (new FTPController().updateApp())
                    result = true;


            return result;
        }

        public void SendPdf()
        {
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 1);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 2);

            string str = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "PDFs" + Java.IO.File.Separator + "pdfsend.pdf";
            try
            {
                Intent intent = new Intent(Intent.ActionSend);
                intent.SetType("application/pdf");
                intent.SetPackage("com.whatsapp");
                intent.PutExtra(Intent.ExtraText, "asdknasidn");
                intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse(str));
                StartActivity(Intent.CreateChooser(intent, "COMPARTILHAR PELO WHATSAPP"));
            }
            catch (Exception e)
            {
                Log.Error("EloVendas", e.ToString());
            }
        }

        public bool sendOrder(long pFT_PEDIDO_ID, string tipoEnvio)
        {
            bool result = false;
            try
            {
                Pedido pedido = new PedidoController().FindById(pFT_PEDIDO_ID);
                Empresa empresa = new EmpresaController().GetEmpresa();
                List<ItemPedido> itens = new ItemPedidoController().FindAllOrderItems(pFT_PEDIDO_ID);
                Pessoa cliente = new PessoaController().FindById(pedido.ID_PESSOA.Value);

                string menssagem = new PedidoController().menssagemParaEnvio(pedido, itens);

                if (tipoEnvio == "pdf")
                {
                    Task.Run(() =>
                    {
                        RunOnUiThread(() =>
                        {
                            PdfCreator pdfCreator = new PdfCreator();
                            pdfCreator.CreatePDF(pedido, itens, empresa, cliente);
                            this.Msg("PDF GERADO COM SUCESSO");

                            Intent intent;
                            Android.Net.Uri apkUri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "PDFs"));
                            //intent = new Intent(Intent.ActionView);
                            intent = new Intent();
                            intent.SetDataAndType(apkUri, "file/*");
                            StartActivity(Intent.CreateChooser(intent, "Open folder"));
                            //StartActivity(intent);
                        });
                    });
                }
                else if (tipoEnvio == "email")
                {
                    EditText txtDSCEMAIL = new EditText(this);
                    txtDSCEMAIL.InputType = InputTypes.TextVariationEmailAddress;
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("ENTRE COM UM EMAIL");
                    builder.SetView(txtDSCEMAIL);
                    builder.SetPositiveButton("ENVIAR", (s, a) =>
                    {
                        if (!string.IsNullOrEmpty(txtDSCEMAIL.Text))
                        {
                            string assunto = $"{empresa.NOMFANTA} - PEDIDO N°{pedido.NROPEDID}";

                            Intent intent = new Intent(Intent.ActionSend);
                            intent.PutExtra(Intent.ExtraEmail, new string[] { txtDSCEMAIL.Text });
                            intent.PutExtra(Intent.ExtraSubject, assunto);
                            intent.PutExtra(Intent.ExtraText, menssagem);
                            intent.SetPackage("com.google.android.gm");
                            intent.SetType("message/rfc822");

                            StartActivity(Intent.CreateChooser(intent, "Selecione um aplicativo para o envio!"));
                        }
                    });
                    builder.SetNegativeButton("CANCELAR", (a, s) => { return; });
                    AlertDialog dialog = builder.Create();
                    dialog.Show();
                    ShowKeyboard(txtDSCEMAIL);
                }
                else if (tipoEnvio == "wpp")
                {
                    if (menssagem.Contains("&"))
                        menssagem = menssagem.Replace("&", "E");

                    string url = "https://api.whatsapp.com/send?phone=" + "&text=" + menssagem.Replace(" ", "%20");
                    Intent i = new Intent(Intent.ActionView);
                    i.SetData(Android.Net.Uri.Parse(url));
                    StartActivity(i);
                }

                result = true;
            }
            catch (Exception e)
            {
                result = false;
            }
            return result;
        }
        public void VerificarPermissaoSMS()
        {
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.SendSms) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.SendSms }, 20);
        }

        public void loadDevices()
        {
            Devices = new List<BluetoothDevice>();
            if (Adapter.StartDiscovery())
                Devices = Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
            else
                Devices = Adapter.BondedDevices.OrderBy(i => i.Name).ToList();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            if (listView.Adapter != null)
            {
                List<string> listaP = new List<string>();
                var adapter = (AdapterItensPedido)listView.Adapter;

                for (int i = 0; i <= adapter.Count - 1; i++)
                {
                    string INDBRINDE = "0";

                    var item = adapter[i];

                    if (item.INDBRIND)
                        INDBRINDE = "1";

                    listaP.Add($"{item.CODPROD} - {item.QTDPROD} - {item.VLRUNIT} - {INDBRINDE}");
                }

                outState.PutStringArray("rec_LISTAPROD", listaP.ToArray());
            }
            base.OnSaveInstanceState(outState);
        }
    }

}