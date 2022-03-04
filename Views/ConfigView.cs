using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Util;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Persistence;
using EloPedidos.Services;
using EloPedidos.Utils;
using M = EloPedidos.Models;
using System.Globalization;
using Android.Views;
using Xamarin.Android;
using Xamarin.Essentials;
using EloPedidos.Models;
using System.Collections.Generic;
using Android.Support.Design.Widget;

namespace EloPedidos.Views
{
	[Activity(Label = "Configurações")]
	public class ConfigView : Activity
	{
		private LinearLayout btnRLRSLDO;
		private LinearLayout secret;
		private TextInputEditText txDNSInterno;
		private TextInputEditText txDNSExterno;
		private TextInputEditText txNROPEDID;
		private SwitchCompat swDNS;
		//private EditText txEmail;
		public TextInputEditText txCODMUNPQ;
		public TextView txNOMMUNPQ;
		//private SwitchCompat swEnviarEmail;
		private SwitchCompat swCODEAN;
		private Button btnSalvar;
		private RelativeLayout RLayout;
		private RadioButton rdEnviar;
		private RadioButton rdGravar;
		private RadioButton rdDesativado;
		private Button btnFechar;
		private Button btnTest, btnlogout, btnBackup;
		private TextView lbTest, lblTOTPED, lblTOTREC;
		private CheckBox ckBLOQROMAN;

		public BuscarPedidoView BPV;

		/// <summary>
		///  Utilizado para contar clicks
		/// </summary>
		private int Count = 0;


		/// <summary>
		///  Captura o ID da CONFIGURAÇÃO (caso haja uma) para que seja única.
		/// </summary>
		private long? CONFIG_ID { get; set; }

		/// <summary>
		///  Passa o foco para o próximo elemento desejado!
		/// </summary>
		/// <param name="v"></param>
		private void NextFocus(View v)
		{
			v.RequestFocus();
		}


		protected override void OnCreate(Bundle savedInstanceState)

		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_config);
			// Create your application here

			Task.Run(UpdateView);

			secret = FindViewById<LinearLayout>(Resource.Id.linearLayout);
			txDNSInterno = FindViewById<TextInputEditText>(Resource.Id.txDNSInterno);
			txDNSExterno = FindViewById<TextInputEditText>(Resource.Id.txDNSExterno);
			txNROPEDID = FindViewById<TextInputEditText>(Resource.Id.txNROPEDID);
			swDNS = FindViewById<SwitchCompat>(Resource.Id.swDNS);
			//txEmail = FindViewById<EditText>(Resource.Id.txEmail);
			txCODMUNPQ = FindViewById<TextInputEditText>(Resource.Id.txCODMUNPQ);
			txNOMMUNPQ = FindViewById<TextView>(Resource.Id.txNOMMUNPQ);
			//swEnviarEmail = FindViewById<SwitchCompat>(Resource.Id.swEnviarEmail);
			swCODEAN = FindViewById<SwitchCompat>(Resource.Id.swCODEAN);
			btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
			RLayout = FindViewById<RelativeLayout>(Resource.Id.relativeLayout);
			rdGravar = FindViewById<RadioButton>(Resource.Id.rdGravar);
			rdEnviar = FindViewById<RadioButton>(Resource.Id.rdEnviar);
			rdDesativado = FindViewById<RadioButton>(Resource.Id.rdDesativado);
			btnFechar = FindViewById<Button>(Resource.Id.btnFechar);
			btnlogout = FindViewById<Button>(Resource.Id.btnlogout);
			btnTest = FindViewById<Button>(Resource.Id.btnTest);
			lbTest = FindViewById<TextView>(Resource.Id.lbTest);
			lblTOTPED = FindViewById<TextView>(Resource.Id.lblTOTPED);
			lblTOTREC = FindViewById<TextView>(Resource.Id.lblTOTREC);
			btnRLRSLDO = FindViewById<LinearLayout>(Resource.Id.btnRLRSLDO);
			btnBackup = FindViewById<Button>(Resource.Id.btnBackup);
			ckBLOQROMAN = FindViewById<CheckBox>(Resource.Id.ckBLOQROMAN);

			txNOMMUNPQ.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });

			RLayout.Visibility = Android.Views.ViewStates.Invisible;

			lbTest.Visibility = Android.Views.ViewStates.Invisible;

			btnRLRSLDO.Visibility = Android.Views.ViewStates.Visible;

			lblTOTPED.Visibility = ViewStates.Invisible;
			lblTOTREC.Visibility = ViewStates.Invisible;

			List<BaixasPedido> baixas = new BaixasPedidoController().FindAll();
			if (baixas.Count > 0)
			{
				double tot = 0;
				double totrec = 0;

				baixas.ForEach(b =>
				{
					tot += b.TOTLPEDID - b.VLRDEVOL;
					totrec += b.VLRRECBR;
				});

				lblTOTPED.Text = $"Valor total dos pedidos: {tot.ToString("C")}";
				lblTOTREC.Text = $"Valor total a receber: {totrec.ToString("C")}";

			}

			this.OnViewLoad();
			NextFocus(txCODMUNPQ);
			///* Eventos */
			txCODMUNPQ.TextChanged += (sender, eventArgs) =>
			{
				BuscarPedidoView buscarpedido = new BuscarPedidoView();

				if (!string.IsNullOrEmpty(txCODMUNPQ.Text) || !string.IsNullOrWhiteSpace(txCODMUNPQ.Text))
				{
					string fNOMMUNIC = new MunicipioController().FindNameById(long.Parse(txCODMUNPQ.Text));
					txNOMMUNPQ.Text = !string.IsNullOrEmpty(fNOMMUNIC) ? fNOMMUNIC : "";
				};

			};
			txCODMUNPQ.FocusChange += (sender, eventArgs) =>
			{
				if (!txCODMUNPQ.HasFocus)
					if (txCODMUNPQ.Text != "0" && !string.IsNullOrEmpty(txCODMUNPQ.Text) && string.IsNullOrEmpty(txNOMMUNPQ.Text))
					{
						Toast.MakeText(Application.Context, "NENHUM MUNICIPIO REGISTRADO! VERIFIQUE", ToastLength.Short).Show();
					};

			};

			btnBackup.Click += (s, a) =>
			{
				this.Msg("ENVIANDO BACKUP PARA O SERVIDOR");
				EnableView(false);

				Task.Run(() =>
				{
					if (new FTPController().BackupDataBase())
						RunOnUiThread(() =>
						{
							this.Msg("BACKUP ENVIADO PARA O SERVIDOR");
							EnableView(true);
						});
					else
						RunOnUiThread(() =>
						{
							this.Msg("BACKUP NÃO ENVIADO PARA O SERVIDOR");
							EnableView(true);
						});
				});
			};

			txCODMUNPQ.LongClick += (sender, eventArgs) =>
			{
				if (new MunicipioController().FindAll().Count == 0)
					this.Msg("NENHUM MUNICIPIO REGISTRADO!");
				else
				{
					Intent i = new Intent(Application.Context, typeof(BuscaMunicipioView));
					StartActivityForResult(i, 1);
				}
			};

			btnSalvar.Click += (sender, eventArgs) =>
			{
				try
				{
					var cController = new ConfigController();


					M.Config c = new M.Config()
					{
						CONFIG_ID = CONFIG_ID,
						DNSINT = txDNSInterno.Text,
						DNSEXT = txDNSExterno.Text,
						INDDNS = swDNS.Checked,
						//DSCEMAIL = txEmail.Text,
						//INDEMAIL = swEnviarEmail.Checked,
						VERSAODB = cController.Config.VERSAODB,
						ENVLOC = cController.Config.ENVLOC,
						GRVLOC = cController.Config.GRVLOC,
						INDDESAT = cController.Config.INDDESAT,
						GRVLOG = cController.Config.GRVLOG,
						CODMUNPQ = int.Parse(txCODMUNPQ.Text),
						NOMMUNPQ = txNOMMUNPQ.Text,
						CODEAN = swCODEAN.Checked,
						NOMIMPRE = (cController.GetConfig().NOMIMPRE != "") ? cController.GetConfig().NOMIMPRE : "",
						BLOQROMAN = ckBLOQROMAN.Checked,
					};

					if (txCODMUNPQ.Text != "" || txCODMUNPQ.Text.Equals("0") || new MunicipioController().FindById(long.Parse(txCODMUNPQ.Text)) != null)
					{
						if (cController.Save(c))
							RunOnUiThread(() => Toast.MakeText(Application.Context, "SALVO COM SUCESSO!", ToastLength.Short).Show());
					}
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						AlertDialog alerta = builder.Create();
						alerta.SetTitle("AVISO");
						alerta.SetMessage("MUNICIPIO NÃO ENCONTRADO");
						alerta.SetButton("OK", (s, ev) =>
						{
							Toast.MakeText(this, "", ToastLength.Short);
						});
						alerta.Show();
						NextFocus(txCODMUNPQ);
						txCODMUNPQ.SelectAll();
					}
				}

				catch (Exception ex)
				{
					string error = "";
					Log.Error(error, ex.ToString());
					Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long).Show();
					return;
				}
			};

			secret.Click += (sender, eventArgs) =>
			{
				if (Count == 0)
					Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(1.5)); Count = 0; });

				++Count;

				if (Count == 5)
				{
					RLayout.Visibility = Android.Views.ViewStates.Visible;
				}
			};
			btnFechar.Click += (sender, eventArgs) => RLayout.Visibility = Android.Views.ViewStates.Invisible;
			btnlogout.Click += (sender, eventArgs) =>
			{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("AVISO");
				builder.SetMessage("TEM CERTEZA QUE DESEJA REINICIAR O SISTEMA ? ?");

				builder.SetPositiveButton("OK", (s, e) =>
				{
					try
					{
						Database.ResetDatabase();

						Intent i = new Intent(this, typeof(LoginView));
						StartActivity(i);
						Finish();
					}
					catch(SQLite.SQLiteException ex)
					{
						this.Msg("ERRO AO REINICIAR SISTIMA");
					}
				});

				builder.SetNegativeButton("CANCELAR", (s, e) =>
				{
					return;
				});

				AlertDialog alertDialog = builder.Create();
				alertDialog.Show();
			};
			rdEnviar.Click += (sender, eventArgs) => RadioButtonCheckedChange();
			rdGravar.Click += (sender, eventArgs) => RadioButtonCheckedChange();
			rdDesativado.Click += (sender, eventArgs) => RadioButtonCheckedChange();
			btnTest.Click += (s, e) =>
			{
				lbTest.Text = "";
				lbTest.Visibility = Android.Views.ViewStates.Visible;

				string host = "";
				string port = "";

				if (swDNS.Checked)
				{
					if (!string.IsNullOrEmpty(txDNSExterno.Text))
					{
						if (txDNSExterno.Text.Contains(":"))
						{
							host = txDNSExterno.Text.Split(":")[0];
							port = txDNSExterno.Text.Split(":")[1];
						}
					}
				}
				else
				{
					if (!string.IsNullOrEmpty(txDNSInterno.Text))
						if (txDNSInterno.Text.Contains(":"))
						{
							host = txDNSInterno.Text.Split(":")[0];
							port = txDNSInterno.Text.Split(":")[1];
						}
				}

				if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(port))
				{

					if (new ConfigController().TestServerConnection(host, int.Parse(port)))
					{
						lbTest.Text = "OK";
						lbTest.SetTextColor(Android.Graphics.Color.ParseColor("#005500"));
					}
					else
					{
						lbTest.Text = "SEM CONEXÃO";
						lbTest.SetTextColor(Android.Graphics.Color.ParseColor("#550000"));
					}
				}
				else
				{
					lbTest.Text = "SEM CONEXÃO";
					lbTest.SetTextColor(Android.Graphics.Color.ParseColor("#550000"));
				}
			};
			btnRLRSLDO.Click += (s, e) =>
			{
				if (Count == 0)
					Task.Run(() => { Thread.Sleep(TimeSpan.FromSeconds(1.5)); Count = 0; });

				++Count;

				if (Count == 5)
				{
					if (lblTOTPED.Visibility != ViewStates.Visible)
					{
						lblTOTPED.Visibility = ViewStates.Visible;
						lblTOTREC.Visibility = ViewStates.Visible;
					}
					else
					{
						lblTOTPED.Visibility = ViewStates.Invisible;
						lblTOTREC.Visibility = ViewStates.Invisible;
					}
				}
			};
		}

		public void EnableView(bool enable = true)
		{
			txDNSInterno.Enabled = enable;
			txDNSExterno.Enabled = enable;
			txCODMUNPQ.Enabled = enable;
			btnBackup.Enabled = enable;
			btnFechar.Enabled = enable;
			btnlogout.Enabled = enable;
			btnTest.Enabled = enable;
			btnSalvar.Enabled = enable;
			btnRLRSLDO.Enabled = enable;
		}

		private void RadioButtonCheckedChange()
		{
			var controller = new ConfigController();
			var config = controller.Config;

			if (rdEnviar.Checked)
			{
				config.ENVLOC = true;
				config.GRVLOC = false;
				config.INDDESAT = false;
			}
			else if (rdGravar.Checked)
			{
				config.ENVLOC = false;
				config.GRVLOC = true;
				config.INDDESAT = false;
			}
			else if (rdDesativado.Checked)
			{
				config.ENVLOC = false;
				config.GRVLOC = false;
				config.INDDESAT = true;
			}

			controller.Save(config);
		}

		private void ReiniciarSistema()
		{
			new DialogFactory().PasswordDialog(this,
				"OK",
				() =>
				{
					new DialogFactory().CreateDialog(this, "REINICIAR", "REINICIAR APLICATIVO ?", "SIM",
						() =>
						{
							try
							{
								Database.GetConnection().RunInTransaction(() =>
								{
									Database.ResetDatabase();

									Intent i = new Intent(this, typeof(LoginView));
									StartActivity(i);
									Finish();
								});
							}
							catch (Exception ex)
							{
								Log.Error("Error", ex.ToString());
								this.Msg(ex.Message);
							}
						},
						"NÃO",
						() => { });
				},
				"CANCELAR",
				() => { });
		}

		private void OnViewLoad()
		{
			try
			{
				M.Config config = new ConfigController().Config;

				if (config != null)
				{
					this.CONFIG_ID = config.CONFIG_ID;
					txDNSInterno.Text = config.DNSINT;
					txDNSExterno.Text = config.DNSEXT;
					swDNS.Checked = config.INDDNS;
					swCODEAN.Checked = config.CODEAN;
					txCODMUNPQ.Text = config.CODMUNPQ.ToString();
					txNOMMUNPQ.Text = "";

					if (config.ENVLOC)
						rdEnviar.Checked = true;
					else if (config.GRVLOC)
						rdGravar.Checked = true;
					else if (config.INDDESAT)
						rdDesativado.Checked = true;

					if (!txCODMUNPQ.Text.Equals(""))
					{
						string fNOMMUNIC = new MunicipioController().FindNameById(long.Parse(txCODMUNPQ.Text));
						txNOMMUNPQ.Text = !string.IsNullOrEmpty(fNOMMUNIC) ? fNOMMUNIC : "";
					}

					if (!config.BLOQROMAN.HasValue)
						ckBLOQROMAN.Checked = true;
					else
						ckBLOQROMAN.Checked = config.BLOQROMAN.Value;
				}
				Sequencia sequencia = null;
				if ((sequencia = new SequenciaController().Sequencia) != null)
				{
					long FINDNROPED;
					long NROAT = sequencia.NROPEDAT;

					if (NROAT == 0)
						FINDNROPED = sequencia.NROPEDIN;
					else
						FINDNROPED = NROAT + 1;

					txNROPEDID.Text = FINDNROPED.ToString();
				}
				else
					txNROPEDID.Text = "SEM SEQUENCIA";
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				RunOnUiThread(() => Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Long).Show());
				return;
			}
		}


		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 1)
				if (resultCode == Result.Ok)
				{
					string municResult = data.GetStringExtra("municResult");
					if (!string.IsNullOrEmpty(municResult))
						txCODMUNPQ.Text = data.GetStringExtra("municResult");
				}
		}

		//protected override void OnDestroy()
		//{
		//    base.OnDestroy();
		//    SendBroadcast(new Intent(this, typeof(GeolocatorBroadCast)));
		//}

		public Task UpdateView()
		{
			while (true)
			{
				var controller = new ConfigController();
				var config = controller.Config;

				RunOnUiThread(() =>
				{
					if (config != null)
					{
						if (config.ENVLOC)
							rdEnviar.Checked = true;
						else if (config.GRVLOC)
							rdGravar.Checked = true;
						else if (config.INDDESAT)
							rdDesativado.Checked = true;
					}
				});

				controller.Save(config);

				Thread.Sleep(TimeSpan.FromSeconds(5));
			}
		}
	}
}