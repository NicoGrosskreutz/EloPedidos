using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using EloPedidos.Models;
using EloPedidos.Services;
using A = Android.App;

namespace EloPedidos.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", Icon = "@mipmap/logo", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class Pedido2View : AppCompatActivity
    {
        private EditText txData, txNRONF, txCODPESS, txNOMFANTA, txCODPROD, txCPLPROD,
            txIDTUNID, txQTDUNID, txVLRUNIT, txVLRDSCTO, txTotal, txDSCPRZPG, txDSCOBSER;
        private TextView lbDSCPROD, lbTotal;
        private ListView listView;
        private Button btnSalvar, btnExcluir, btnImprimir, btnLimpar, btnSalvarProd, btnExcluirProd;
        private RelativeLayout layout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_pedido2);

            // ============
            txData = FindViewById<EditText>(Resource.Id.txData);
            txNRONF = FindViewById<EditText>(Resource.Id.txNRONF);
            txCODPESS = FindViewById<EditText>(Resource.Id.txCODPESS);
            txNOMFANTA = FindViewById<EditText>(Resource.Id.txNOMFANTA);
            txCODPROD = FindViewById<EditText>(Resource.Id.txCODPROD);
            txCPLPROD = FindViewById<EditText>(Resource.Id.txCPLPROD);
            txIDTUNID = FindViewById<EditText>(Resource.Id.txIDTUNID);
            txQTDUNID = FindViewById<EditText>(Resource.Id.txQTDUNID);
            txVLRUNIT = FindViewById<EditText>(Resource.Id.txVLRUNIT);
            txVLRDSCTO = FindViewById<EditText>(Resource.Id.txVLRDSCTO);
            txTotal = FindViewById<EditText>(Resource.Id.txTotal);
            txDSCPRZPG = FindViewById<EditText>(Resource.Id.txDSCPRZPG);
            txDSCOBSER = FindViewById<EditText>(Resource.Id.txDSCOBSER);
            // ============
            lbDSCPROD = FindViewById<TextView>(Resource.Id.lbDSCPROD);
            lbTotal = FindViewById<TextView>(Resource.Id.lbTotal);
            // ============
            listView = FindViewById<ListView>(Resource.Id.listView);
            // ============
            btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
            btnExcluir = FindViewById<Button>(Resource.Id.btnExcluir);
            btnLimpar = FindViewById<Button>(Resource.Id.btnLimpar);
            btnImprimir = FindViewById<Button>(Resource.Id.btnImprimir);
            btnSalvarProd = FindViewById<Button>(Resource.Id.btnSalvarProd);
            btnExcluirProd = FindViewById<Button>(Resource.Id.btnExcluirProd);
            // ============
            layout = FindViewById<RelativeLayout>(Resource.Id.relativeLayout);
            // ============

            // Setando Upper Case
            for (int i = 0; i < layout.ChildCount; i++)
            {
                if (layout.GetChildAt(i).GetType() == typeof(EditText))
                {
                    var field = (EditText)layout.GetChildAt(i);
                    field.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
                }

                if (layout.GetChildAt(i).GetType() == typeof(TextView))
                {
                    var field = (TextView)layout.GetChildAt(i);
                    field.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
                }
            }

            txData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbDSCPROD.Text = string.Empty;

            // Eventos
            txCODPESS.LongClick += (s, e) => StartActivityForResult(new Intent(this, typeof(BuscaClienteView)), (int)Code.BuscaCliente);
            txCODPESS.TextChanged += (s, e) => 
            {
                if (!string.IsNullOrEmpty(txCODPESS.Text))
                {
                    Pessoa p = null;
                    if ((p = new PessoaController(this).FindByCODPESS(long.Parse(txCODPESS.Text))) != null)
                    {
                        txNOMFANTA.Text = p.NOMFANTA;
                        txNOMFANTA.SetTextColor(Color.DarkGray);
                    }
                    else if (p == null)
                    {
                        txNOMFANTA.Text = "CLIENTE NÃO ENCONTRADO!";
                        txNOMFANTA.SetTextColor(Color.Red);
                    }
                }
                else
                    txNOMFANTA.Text = string.Empty;
            };
            txCODPROD.LongClick += (s, e) => StartActivityForResult(new Intent(this, typeof(BuscarProdutoView)), (int)Code.BuscaProduto);
            txCODPROD.TextChanged += (s, e) => 
            {
                if (!string.IsNullOrEmpty(txCODPROD.Text))
                {
                    Produto p = null;
                    if ((p = new ProdutoController(this).FindByCODPROD(long.Parse(txCODPROD.Text))) != null)
                    {
                        LoadProdByCODPROD(p.CODPROD);
                        lbDSCPROD.SetTextColor(Color.DarkGray);
                    }
                    else if (p == null)
                    {
                        lbDSCPROD.Text = "PRODUTO NÃO ENCONTRADO!";
                        lbDSCPROD.SetTextColor(Color.Red);
                        LimparDadosProd();
                    }
                }
                else
                {
                    lbDSCPROD.Text = string.Empty;
                    LimparDadosProd();
                }
            };
        }

        /// <summary>
        /// Carrega informações referentes ao código do produto
        /// </summary>
        /// <param name="pCODPROD"></param>
        private void LoadProdByCODPROD(long? pCODPROD)
        {
            if (pCODPROD != null)
            {
                Produto p = null;
                if ((p = new ProdutoController(this).FindByCODPROD(pCODPROD.Value)) != null)
                {
                    lbDSCPROD.Text = p.DSCPROD;
                    txIDTUNID.Text = p.IDTUNID;
                    txVLRUNIT.Text = p.PRCVENDA.ToString("F").Replace("." , ",");
                }
            }
        }

        private void LimparDadosProd()
        {
            txCPLPROD.Text = string.Empty;
            txVLRUNIT.Text = string.Empty;
            txIDTUNID.Text = string.Empty;
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
        ///  Cria o menu para o app
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            base.OnCreateOptionsMenu(menu);
            MenuInflater menuInflater = this.MenuInflater;
            menuInflater.Inflate(Resource.Menu.main_menu, menu);
            return true;
        }

        /// <summary>
        ///  Controla a ação do botão ao ser clicado (Menu)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.cadastrarCliente:
                    Intent cadastrarCliente = new Intent(this, typeof(Views.ClienteView));
                    StartActivity(cadastrarCliente);
                    break;

                case Resource.Id.novoPedido:
                    Intent novoPedido = new Intent(this, /* typeof(Views.PedidoView) */ typeof(Views.Pedido2View));
                    StartActivity(novoPedido);
                    break;

                case Resource.Id.configuracao:
                    Intent configuracao = new Intent(this, typeof(Views.ConfigView));
                    StartActivity(configuracao);
                    break;

                case Resource.Id.atualizar:
                    SicronizarPedidos();
                    break;

                case Resource.Id.atualizarDados:
                    AtualizarDados();
                    break;
            }

            return true;
        }

        private void Msg(string msg)
            => RunOnUiThread(()
                => Toast.MakeText(Application.Context, msg, ToastLength.Long).Show());

        private void GetError(string msg)
        {
            string error = "";
            Log.Error(error, msg);
            Msg(msg);
        }

        /// <summary>
        ///  Atualiza os dados do app com o servidor
        /// </summary>
        protected virtual void AtualizarDados()
        {
            new Thread(() =>
            {
                try
                {
                    var config = new ConfigController(this).GetConfig();
                    DNS dns = new DNS()
                    {
                        Host = config.INDDNS ? config.DNSEXT.Split(':')[0] : config.DNSINT.Split(':')[0],
                        Port = int.TryParse(config.INDDNS ? config.DNSEXT.Split(':')[1] : config.DNSINT.Split(':')[1], out int aux) ? aux : 0,
                        DNSInfo = config.INDDNS ? DNS.IndDNS.DNSExterno : DNS.IndDNS.DNSInterno
                    };

                    if (!string.IsNullOrEmpty(dns.Host) && aux != 0)
                    {
                        if (new NetworkController().TestConnection(Application.Context))
                        {
                            Msg("SINCRONIZANDO DADOS COM O SERVIDOR! AGUARDE...");

                            EmpresaController empresaC = new EmpresaController(Application.Context);
                            if (empresaC.ComSocket($"CARGAEMPRESA{new EmpresaController(this).GetEmpresa().CODEMPRE}", dns.Host, dns.Port))
                            {
                                Empresa empresa = empresaC.GetEmpresa();

                                OperadorController operadorC = new OperadorController(Application.Context);
                                if (operadorC.ComSocket($"CARGAOPERADOR{empresa.CODEMPRE}{new OperadorController(this).GetOperador().USROPER}", dns.Host, dns.Port))
                                {
                                    Operador operador = operadorC.GetOperador();

                                    VendedorController vendedorC = new VendedorController(Application.Context);
                                    if (vendedorC.ComSocket($"CARGAVENDEDOR{empresa.CODEMPRE}{operador.USROPER}", dns.Host, dns.Port))
                                    {
                                        Vendedor vendedor = vendedorC.GetVendedor();

                                        SequenciaController sequenciaC = new SequenciaController(Application.Context);

                                        string id = string.Format("{0:0000}", vendedor.CG_VENDEDOR_ID);
                                        string sequencia = "000000";

                                        if (sequenciaC.ComSocket($"CARGAVENDESEQ{id}{sequencia}", dns.Host, dns.Port))
                                        {
                                            MunicipioController municipioC = new MunicipioController(Application.Context);
                                            if (municipioC.ComSocket($"CARGAMUNICIPIO", dns.Host, dns.Port))
                                            {
                                                ProdutoController produtoC = new ProdutoController(Application.Context);
                                                if (produtoC.ComSocket($"CARGAPRODUTO{empresa.CODEMPRE}", dns.Host, dns.Port))
                                                {
                                                    PessoaController pessoaC = new PessoaController(Application.Context);
                                                    if (pessoaC.ComSocket($"CARGAPESSOA{empresa.CODEMPRE}", dns.Host, dns.Port))
                                                    {
                                                        VencimentoController vencimentoC = new VencimentoController(Application.Context);
                                                        if (vencimentoC.ComSocket($"CARGAPESSOAVCTO{empresa.CODEMPRE}", dns.Host, dns.Port))
                                                            Msg("SINCRONIZAÇÃO COM SERVIDOR REALIZADA COM SUCESSO!");
                                                        else
                                                            Msg("ERRO AO SINCRONIZAR VENCIMENTOS! VERIFIQUE.");
                                                    }
                                                    else
                                                        Msg("ERRO AO SINCRONIZAR CLIENTES! VERIFIQUE.");
                                                }
                                                else
                                                    Msg("ERRO AO SINCRONIZAR PRODUTOS! VERIFIQUE.");
                                            }
                                            else
                                                Msg("ERRO AO SINCRONIZAR MUNICIPIO! VERIFIQUE.");
                                        }
                                        else
                                            Msg("ERRO AO SINCRONIZAR SEQUÊNCIA DE PEDIDOS! VERIFIQUE.");
                                    }
                                    else
                                        Msg("ERRO AO SINCRONIZAR VENDEDOR! VERIFIQUE.");
                                }
                                else
                                    Msg("ERRO AO SINCRONIZAR OPERADOR! VERIFIQUE.");
                            }
                            else
                                Msg("ERRO AO SINCRONIZAR EMPRESA! VERIFIQUE.");
                        }
                        else
                            Msg("SEM CONEXÃO COM INTERNET! VERIFIQUE.");
                    }
                    else
                        Msg("ERRO NAS CONFIGURAÇÕES DO SISTEMA! VERIFIQUE.");
                }
                catch (Exception ex)
                {
                    GetError(ex.ToString());
                }
            }).Start();
        }

        /// <summary>
        ///  Sincroniza os pedidos com o sistema no servidor
        /// </summary>
        protected virtual void SicronizarPedidos()
        {
            try
            {
                A.AlertDialog.Builder builder = new A.AlertDialog.Builder(this);
                builder.SetMessage("Deseja transmitir ?");

                builder.SetPositiveButton("OK", (s, e) =>
                {
                    A.AlertDialog.Builder builder2 = new A.AlertDialog.Builder(this);
                    builder2.SetMessage("Qual sincronização deseja realizar ?");

                    builder2.SetPositiveButton("PEDIDO ATUAL", (sender, eventArgs) =>
                    {
                        // Continuar aqui
                    });

                    builder2.SetNeutralButton("TODOS OS PEDIDOS", (sender, eventArgs) =>
                    {
                        // Continuar aqui
                    });

                    A.AlertDialog alertDialog = builder2.Create();
                    alertDialog.Show();
                });

                builder.SetNegativeButton("Cancelar", (s, e) =>
                {
                    return;
                });

                A.AlertDialog alert = builder.Create();
                alert.Show();
            }
            catch (Exception ex)
            {
                GetError(ex.ToString());
            }
        }

        /// <summary>
        ///   Formata corretamente uma string para double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double TryParseDouble(string value)
        {
            return double.TryParse(value.Replace(",", "."), out double aux) ? aux : 0;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // Busca cliente
            if (requestCode == (int)Code.BuscaCliente)
                if (resultCode == Result.Ok)
                {
                    string result = data.GetStringExtra("result");

                    if (!string.IsNullOrEmpty(result))
                        txCODPESS.Text = result;
                }

            // Busca produto
            if (requestCode == (int)Code.BuscaProduto)
                if (resultCode == Result.Ok)
                {
                    string resultProd = data.GetStringExtra("resultProd");

                    if (!string.IsNullOrEmpty(resultProd))
                        txCODPROD.Text = resultProd;
                }
        }

        public enum Code
        {
            BuscaCliente = 1,
            BuscaProduto = 2,
            BuscarNFe = 3
        };

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SendBroadcast(new Intent(this, typeof(GeolocatorBroadCast)));
        }
    }
}