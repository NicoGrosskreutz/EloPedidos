using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using M = EloPedidos.Models;
using A = Android.App;
using Android.Text;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android;
using System.Threading;
using System.Threading.Tasks;
using EloPedidos.Utils;
using EloPedidos.Services;
using Android.Support.Design.Widget;
using Java.IO;

namespace EloPedidos.Views
{
    [Activity(Label = "@string/app_name")]
    public class LoginView : AppCompatActivity
    {
        private TextInputEditText txDNSInterno, txDNSExterno, txEmpresa, txOperador;
        private Android.Support.V7.Widget.SwitchCompat swDNS;
        private Button btnEntrar, btnLimpar;
        private ProgressBar progressBar;
        private LinearLayout linearLayout1;
        private FloatingActionButton floatingButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            #region CheckPermissions

            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReceiveBootCompleted) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReceiveBootCompleted }, 1);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessNetworkState) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessNetworkState }, 2);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessFineLocation }, 3);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessCoarseLocation }, 4);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessLocationExtraCommands) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessLocationExtraCommands }, 5);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessWifiState) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessWifiState }, 6);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Internet) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Internet }, 7);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.AccessMockLocation) != Android.Content.PM.Permission.Granted)
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.AccessMockLocation }, 8);
            if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
				ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 9);
			if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
				ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 10);


            #endregion

            // Create your application here
            SetContentView(Resource.Layout.activity_login);

            txDNSInterno = FindViewById<TextInputEditText>(Resource.Id.txDNSInterno);
            txDNSExterno = FindViewById<TextInputEditText>(Resource.Id.txDNSExterno);
            txEmpresa = FindViewById<TextInputEditText>(Resource.Id.txEmpresa);
            txOperador = FindViewById<TextInputEditText>(Resource.Id.txOperador);
            swDNS = FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.swDNS);
            btnEntrar = FindViewById<Button>(Resource.Id.btnEntrar);
            btnLimpar = FindViewById<Button>(Resource.Id.btnLimpar);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            linearLayout1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            floatingButton = FindViewById<FloatingActionButton>(Resource.Id.floatingButton);

            progressBar.Visibility = ViewStates.Invisible;

            txOperador.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });
            txEmpresa.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });

            linearLayout1.Visibility = ViewStates.Invisible;

            Task.Run(() => versionController());

            btnEntrar.Click += (sender, eventArgs) =>
            {
                ConfigController configC = new ConfigController();
                bool loop = true;

                CreateFolder();

                try
                {   
                    if (txDNSInterno.Text.IsBlank() && txDNSExterno.Text.IsBlank())
                        this.Msg("APENAS UM DOS CAMPOS DE DNS PODEM FICAR EM BRANCO!");
                    else
                    {
                        DNS[] DNSArray = GetDNS();

                        if (DNSArray == null)
                        {
                            this.Msg("ERRO AO DEFINIR ENDEREÇO(S) DNS! VERIFIQUE.");
                            return;
                        }

                        DNS dns = null;

                        DNSArray.ToList().ForEach((aux) => 
                        {
                            if (swDNS.Checked)
                            {
                                if (aux != null)
                                    if (aux.DNSInfo == DNS.IndDNS.DNSExterno)
                                        dns = aux;
                            }
                            else
                            {
                                if (aux != null)
                                    if (aux.DNSInfo == DNS.IndDNS.DNSInterno)
                                        dns = aux;
                            }
                        });

                        if (dns == null)
                        {
                            this.Msg("DNS SELECIONADO NÃO FOI INFORMADO!");
                            return;
                        }

                        if (!configC.TestServerConnection(dns.Host, dns.Port))
                        {
                            this.Msg("SEM CONEXÃO COM SERVIDOR! VERIFIQUE.");
                            return;
                        }

                        progressBar.Visibility = ViewStates.Visible;

                        int count = 0;
                        Task.Run(() =>
                        {
                            while (loop)
                            {
                                progressBar.SetProgress(count, true);
                                count++;
                                Thread.Sleep(150);
                            }
                        });

                        // Desabilita componentes
                        EnableButtons(false);

                        new Thread(() =>
                        {
                            try
                            {
                                if (new NetworkController().TestConnection())
                                {
                                    this.Msg("SINCRONIZANDO DADOS COM O SERVIDOR! AGUARDE...");

                                    EmpresaController empresaC = new EmpresaController();
                                    if (empresaC.ComSocket($"CARGAEMPRESA{txEmpresa.Text}", dns.Host, dns.Port))
                                    {
                                        Empresa empresa = empresaC.Empresa;

                                        OperadorController operadorC = new OperadorController();
                                        if (operadorC.ComSocket($"CARGAOPERADOR{empresa.CODEMPRE}{txOperador.Text}", dns.Host, dns.Port))
                                        {
                                            Operador operador = operadorC.Operador;

                                            VendedorController vendedorC = new VendedorController();
                                            if (vendedorC.SocketConnection($"CARGAVENDEDOR{empresa.CODEMPRE}{operador.USROPER}", dns.Host, dns.Port))
                                            {
                                                Vendedor vendedor = vendedorC.Vendedor;

                                                SequenciaController sequenciaC = new SequenciaController();

                                                string id = string.Format("{0:0000}", vendedor.CG_VENDEDOR_ID);
                                                string sequencia = "000000";

                                                if (sequenciaC.ComSocket($"CARGAVENDESEQ{id}{sequencia}", dns.Host, dns.Port))
                                                {
                                                    MunicipioController municipioC = new MunicipioController();
                                                    if (municipioC.ComSocket($"CARGAMUNICIPIO", dns.Host, dns.Port))
                                                    {
                                                        ProdutoController produtoC = new ProdutoController();
                                                        if (produtoC.ComSocket($"CARGAPRODUTO{empresa.CODEMPRE}{id}", dns.Host, dns.Port))
                                                        {
                                                            PessoaController pessoaC = new PessoaController();
                                                            if (pessoaC.ComSocket($"CARGAPESSOA{empresa.CODEMPRE}{id}", dns.Host, dns.Port))
                                                            {
                                                                VencimentoController vencimentoC = new VencimentoController();
                                                                if (vencimentoC.ComSocket($"CARGAPESSOAVCTO{empresa.CODEMPRE}{id}", dns.Host, dns.Port))
                                                                {
                                                                    M.Config config = new M.Config()
                                                                    {
                                                                        INDSINC = true,
                                                                        DTHSINC = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                                                                        VERSAODB = 1,
                                                                        DNSINT = txDNSInterno.Text,
                                                                        DNSEXT = txDNSExterno.Text,
                                                                        INDDNS = swDNS.Checked,
                                                                        ENVLOC = true,
                                                                        CODEAN = false,
                                                                        GRVLOG = true
                                                                    };

                                                                    if (configC.Save(config))
                                                                    {
                                                                        this.Msg("SINCRONIZAÇÃO COM SERVIDOR REALIZADA COM SUCESSO!");
                                                                        //Intent i = new Intent(this, typeof(PedidoView));
                                                                        Intent i = new Intent(this, typeof(MainActivity));
                                                                        StartActivity(i);
                                                                        Finish();
                                                                    }
                                                                    else
                                                                    {
                                                                        this.Msg("FALHA AO SICRONIZAR DADOS COM SERVIDOR! VERIFIQUE.");
                                                                        progressBar.Visibility = ViewStates.Invisible;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    this.Msg("ERRO AO SINCRONIZAR VENCIMENTOS! VERIFIQUE.");
                                                                    progressBar.Visibility = ViewStates.Invisible;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                this.Msg("ERRO AO SINCRONIZAR CLIENTES! VERIFIQUE.");
                                                                progressBar.Visibility = ViewStates.Invisible;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            this.Msg("ERRO AO SINCRONIZAR PRODUTOS! VERIFIQUE.");
                                                            progressBar.Visibility = ViewStates.Invisible;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        this.Msg("ERRO AO SINCRONIZAR MUNICIPIO! VERIFIQUE.");
                                                        progressBar.Visibility = ViewStates.Invisible;
                                                    }
                                                }
                                                else
                                                {
                                                    this.Msg("ERRO AO SINCRONIZAR SEQUÊNCIA DE PEDIDOS! VERIFIQUE.");
                                                    progressBar.Visibility = ViewStates.Invisible;
                                                }
                                            }
                                            else
                                            {
                                                this.Msg("ERRO AO SINCRONIZAR VENDEDOR! VERIFIQUE.");
                                                progressBar.Visibility = ViewStates.Invisible;
                                            }
                                        }
                                        else
                                        {
                                            this.Msg("ERRO AO SINCRONIZAR OPERADOR! VERIFIQUE.");
                                            progressBar.Visibility = ViewStates.Invisible;
                                        }
                                    }
                                    else
                                    {
                                        this.Msg("ERRO AO SINCRONIZAR EMPRESA! VERIFIQUE.");
                                        progressBar.Visibility = ViewStates.Invisible;
                                    }
                                }
                                else
                                {
                                    this.Msg("SEM CONEXÃO COM INTERNET! VERIFIQUE.");
                                    progressBar.Visibility = ViewStates.Invisible;
                                }
                            }
                            finally
                            {
                                RunOnUiThread(() => EnableButtons());
                            }
                        }).Start();
                    }
                }
                catch (Exception ex)
                {
                    EnableButtons();
                    progressBar.Visibility = ViewStates.Invisible;

                    loop = false;
                    GetError(ex.ToString());
                    return;
                }
                finally
                {
                    loop = false;
                }
            };
            btnLimpar.Click += (sender, eventArgs) => 
            {
                A.AlertDialog.Builder alert = new A.AlertDialog.Builder(this);
                alert.SetTitle("AVISO DO SISTEMA");
                alert.SetMessage("DESEJA MESMO LIMPAR A TELA ?");

                alert.SetPositiveButton("LIMPAR", (s, e) => 
                {
                    txDNSInterno.Text = string.Empty;
                    txDNSExterno.Text = string.Empty;
                    swDNS.Checked = false;
                    txEmpresa.Text = string.Empty;
                    txOperador.Text = string.Empty;
                });

                alert.SetNegativeButton("NÃO", (s, e) => { return; });

                A.AlertDialog alertDialog = alert.Create();
                alertDialog.Show();
            };

            floatingButton.Click += (sender, args) =>
            {
                progressBar.Visibility = ViewStates.Visible;
                EnableButtons(false);

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
                                    EnableButtons(true);
                                });
                            }
                            else
                                RunOnUiThread(() => this.Msg("FALHA AO REALIZAR O DOWNLOAD DA ATUALIZAÇÃO! \nTENTE NOVAMENTE"));
                        });
                    });
                }).Start();
            };
        }

        public void CreateFolder()
        {
            try
            {
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.ReadExternalStorage) != Android.Content.PM.Permission.Granted)
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 11);
                if (ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage) != Android.Content.PM.Permission.Granted)
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 12);


                FTPController fTPController = new FTPController();
                File folder = new File(Android.OS.Environment.ExternalStorageDirectory + File.Separator + "EloSoftware");

                if (!folder.Exists())
                {
                    folder.Mkdir();
                    if (new ConfigController().TestServerConnection("elosoftware.dyndns.org", 8560))
                        fTPController.updateApp();
                }
                else
                    Log.Debug("Elo_LOG", "A Pasta Já Existe !");
            }
            catch (Exception e)
            {
                Log.Debug("Elo_LOG", e.Message);
            }

            Thread.Sleep(300);
        }


        /// <summary>
        ///  Retorna um array contento os DNS preenchidos na view
        /// </summary>
        /// <returns>DNS[]</returns>
        private DNS[] GetDNS()
        {
            DNS[] dns = new DNS[2];

            if (!string.IsNullOrWhiteSpace(txDNSInterno.Text))
            {
                DNS aux1 = new DNS();

                if (txDNSInterno.Text.Contains(':'))
                {
                    try
                    {
                        aux1.Host = txDNSInterno.Text.Split(':')[0];
                        aux1.Port = int.Parse(txDNSInterno.Text.Split(':')[1]);
                        aux1.DNSInfo = DNS.IndDNS.DNSInterno;
                        dns[0] = aux1;
                    }
                    catch
                    {
                        this.Msg("ENDEREÇO DNS INVÁLIDO! VERIFIQUE.");
                        return null;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(txDNSExterno.Text))
            {
                DNS aux2 = new DNS();

                if (txDNSExterno.Text.Contains(':'))
                {
                    try
                    {
                        aux2.Host = txDNSExterno.Text.Split(':')[0];
                        aux2.Port = int.Parse(txDNSExterno.Text.Split(':')[1]);
                        aux2.DNSInfo = DNS.IndDNS.DNSExterno;
                        dns[1] = aux2;
                    }
                    catch
                    {
                        this.Msg("ENDEREÇO DNS INVÁLIDO! VERIFIQUE.");
                        return null;
                    }
                }
            }

            if (dns.Length > 0)
                return dns;
            else
                return null;
        }

        public void EnableButtons(bool enable = true)
        {
            if (enable)
            {
                btnEntrar.Enabled = true;
                btnLimpar.Enabled = true;
                txDNSInterno.Enabled = true;
                txDNSExterno.Enabled = true;
                txEmpresa.Enabled = true;
                txOperador.Enabled = true;
                swDNS.Enabled = true;
            }
            else
            {
                btnEntrar.Enabled = false;
                btnLimpar.Enabled = false;
                txDNSInterno.Enabled = false;
                txDNSExterno.Enabled = false;
                txEmpresa.Enabled = false;
                txOperador.Enabled = false;
                swDNS.Enabled = false;
            }
        }

        /// <summary>
        ///  Trata saídas de erro 
        /// </summary>
        /// <param name="error"></param>
        private void GetError(string message)
        {
            string error = "";
            Log.Error(error, message);
            this.Msg(message);
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
                    //Activity activity = CrossCurrentActivity.Current.Activity;
                    //Android.Views.View view = activity.FindViewById(Android.Resource.Id.Content);
                    //Snackbar.Make(view, "HÁ UMA NOVA ATUALIZAÇÃO DISPONÍVEL !", 100000).Show() ;
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
    }
}