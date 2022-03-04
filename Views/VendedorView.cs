using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Utils;
using EloPedidos.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EloPedidos.Views
{
	[Activity(Label = "VendedorView")]

	public class VendedorView : Activity
	{
		private ListView listView;
		private TextInputEditText txtDATAI, txtDATAF;
		private Button btnMAPA, btnBUSCAR;
		private ProgressBar progressBar;

		private Vendedor? selecionado { get; set; } = null;
		private List<Vendedor> listaVendedores { get; set; }
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Activity_Vendedores);
			loadAttributes();
			LoadData();
			LoadListVendedores();

			listView.ChoiceMode = ChoiceMode.Single;
			listView.Selected = true;
			listView.SetSelector(Resource.Drawable.selector);

			listaVendedores = new List<Vendedor>();
			progressBar.Visibility = ViewStates.Invisible;

			#region eventos

			txtDATAI.FocusChange += (s, a) =>
			{
				if (!string.IsNullOrEmpty(txtDATAI.Text))
				{
					string newData = string.Empty;

					if (Utils.Format.DateToString(txtDATAI.Text, out newData))
						txtDATAI.Text = newData;
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO");
						builder.SetMessage("FORMATO DA DATA ESTÁ INVALIDO\n(DD/MM/AAAA)");
						builder.SetNeutralButton("OK", (s, a) =>
						{
							txtDATAI.RequestFocus();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}

					txtDATAI.SetSelectAllOnFocus(true);
				}
				if (txtDATAI.HasFocus)
					DataPickerDialog(txtDATAI);
			};

			txtDATAI.LongClick += (s, a) => DataPickerDialog(txtDATAI);
			txtDATAF.LongClick += (s, a) => DataPickerDialog(txtDATAF);

			txtDATAF.FocusChange += (s, a) =>
			{
				if (!string.IsNullOrEmpty(txtDATAF.Text))
				{
					string newData = string.Empty;

					if (Utils.Format.DateToString(txtDATAF.Text, out newData))
						txtDATAF.Text = newData;
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO");
						builder.SetMessage("FORMATO DA DATA ESTÁ INVALIDO\n(DD/MM/AAAA)");
						builder.SetNeutralButton("OK", (s, a) =>
						{
							txtDATAF.RequestFocus();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
					txtDATAF.SetSelectAllOnFocus(true);
				}
				if (txtDATAF.HasFocus)
					DataPickerDialog(txtDATAF);
			};

			btnBUSCAR.Click += (s, a) =>
			{
				if (!string.IsNullOrEmpty(txtDATAI.Text) && !txtDATAI.Text.Contains("/"))
				{
					string newData = string.Empty;

					if (Utils.Format.DateToString(txtDATAI.Text, out newData))
						txtDATAI.Text = newData;
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO");
						builder.SetMessage("FORMATO DA DATA ESTÁ INVALIDO\n(DD/MM/AAAA)");
						builder.SetNeutralButton("OK", (s, a) =>
						{
							txtDATAI.RequestFocus();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
					txtDATAI.SetSelectAllOnFocus(true);
				}

				if (!string.IsNullOrEmpty(txtDATAF.Text) && !txtDATAF.Text.Contains("/"))
				{
					string newData = string.Empty;

					if (Utils.Format.DateToString(txtDATAF.Text, out newData))
						txtDATAF.Text = newData;
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO");
						builder.SetMessage("FORMATO DA DATA ESTÁ INVALIDO\n(DD/MM/AAAA)");
						builder.SetNeutralButton("OK", (s, a) =>
						{
							txtDATAF.RequestFocus();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
					txtDATAF.SetSelectAllOnFocus(true);
				}


				VendedorController vController = new VendedorController();
				this.Msg("ATUALIZANDO A LISTA DE VENDEODRES");
				progressBar.Visibility = ViewStates.Visible;

				DNS dns = new ConfigController().GetDNS();
				Empresa empresa = new EmpresaController().GetEmpresa();

				Task.Run(() =>
				{
					RunOnUiThread(() => EnableViews(false));

					if (new ConfigController().TestServerConnection())
					{
						if (vController.ComSocket($"CARGAVENDEDOR{empresa.CODEMPRE}", dns.Host, dns.Port))
							RunOnUiThread(() => this.Msg("VENDEDORES ATUALIZAODS COM SUCESSO"));
						else
							RunOnUiThread(() => this.Msg("ERRO AO ATUALIZAR VENDEDORES"));
					}
					else
						RunOnUiThread(() => this.Msg("ERRO AO ATUALIZAR VENDEDORES\nSEM CONEXÃO COM O SERVIDOR"));


					RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);
					RunOnUiThread(() => EnableViews());
					RunOnUiThread(() => LoadListVendedores());
				});
			};

			listView.ItemClick += (sender, args) =>
			{
				this.selecionado = this.listaVendedores[args.Position];
			};

			btnMAPA.Click += (s, a) =>
			{
				if(!string.IsNullOrEmpty(txtDATAI.Text) && !txtDATAI.Text.Contains("/"))
				{
					string newData = string.Empty;

					if (Utils.Format.DateToString(txtDATAI.Text, out newData))
						txtDATAI.Text = newData;
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO");
						builder.SetMessage("FORMATO DA DATA ESTÁ INVALIDO\n(DD/MM/AAAA)");
						builder.SetNeutralButton("OK", (s, a) =>
						{
							txtDATAI.RequestFocus();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
					txtDATAI.SetSelectAllOnFocus(true);
				}

				if (!string.IsNullOrEmpty(txtDATAF.Text) && !txtDATAF.Text.Contains("/"))
				{
					string newData = string.Empty;

					if (Utils.Format.DateToString(txtDATAF.Text, out newData))
						txtDATAF.Text = newData;
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO");
						builder.SetMessage("FORMATO DA DATA ESTÁ INVALIDO\n(DD/MM/AAAA)");
						builder.SetNeutralButton("OK", (s, a) =>
						{
							txtDATAF.RequestFocus();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
					txtDATAF.SetSelectAllOnFocus(true);
				}


				goMap();
			};

			#endregion
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

		private void LoadData()
		{
			DateTime dataIn = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
			DateTime dataFi = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

			//txtDATAI.Text = dataIn.ToString("dd/MM/yyyy");
			txtDATAI.Text = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy");
			txtDATAF.Text = DateTime.Now.ToString("dd/MM/yyyy");
			//txtDATAF.Text = dataFi.ToString("dd/MM/yyyy");
		}
		private void loadAttributes()
		{
			listView = FindViewById<ListView>(Resource.Id.listView);
			txtDATAI = FindViewById<TextInputEditText>(Resource.Id.txtDATAI);
			txtDATAF = FindViewById<TextInputEditText>(Resource.Id.txtDATAF);
			btnMAPA = FindViewById<Button>(Resource.Id.btnMAPA);
			btnBUSCAR = FindViewById<Button>(Resource.Id.btnBUSCAR);
			progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
		}

		private void goMap()
		{
			if (!string.IsNullOrEmpty(txtDATAI.Text) && !string.IsNullOrEmpty(txtDATAF.Text))
			{
				DateTime dataInicial = DateTime.Parse(txtDATAI.Text);
				DateTime dataFinal = DateTime.Parse($"{txtDATAF.Text} 23:59:59");

				if (dataInicial < dataFinal)
				{
					Task.Run(() =>
					{
						RunOnUiThread(() => EnableViews(false));
						string rotas = getLocations();

						if (!string.IsNullOrEmpty(rotas))
						{
							RunOnUiThread(() =>
							{
								Intent map = new Intent(this, typeof(gMapView));
								map.PutExtra("location", rotas);
								StartActivity(map);
							});
						}
						else
						{
							RunOnUiThread(() =>
							{
								AlertDialog.Builder builder = new AlertDialog.Builder(this);
								builder.SetTitle("AVISO");
								builder.SetMessage("NENHUMA ROTA ENCONTRADA");
								builder.SetNeutralButton("OK", (s, a) => 
								{
									return;
								});
								builder.SetCancelable(false);
								AlertDialog dialog = builder.Create();
								dialog.Show();
							});
						}

						RunOnUiThread(() => EnableViews());
					});
				}
				else
				{
					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("AVISO");
					builder.SetMessage("DATA INICIAL MAIOR QUE A DATA FINAL");
					builder.SetNeutralButton("OK", (s, a) =>
					{
						txtDATAF.RequestFocus();
					});
					builder.SetCancelable(false);
					AlertDialog dialog = builder.Create();
					dialog.Show();
				}
			}
			else
			{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("AVISO");
				builder.SetMessage("FAVOR INFORME AS DATAS");
				builder.SetNeutralButton("OK", (s, a) =>
				{
					txtDATAF.RequestFocus();
				});
				builder.SetCancelable(false);
				AlertDialog dialog = builder.Create();
				dialog.Show();
			}
		}
		private string getLocations()
		{
			RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);

			VendedorController vController = new VendedorController();
			DNS dns = new ConfigController().GetDNS();
			Empresa empresa = new EmpresaController().GetEmpresa();

			string rotas = string.Empty;
			

			if (selecionado != null)
			{
				Vendedor vendedor = this.selecionado;
				string id = string.Format("{0:0000}", vendedor.CG_VENDEDOR_ID);

				string dataInicial = DateTime.Parse(txtDATAI.Text).ToString();
				string dataFinal = DateTime.Parse(txtDATAF.Text).ToString();

				string str = $"CONSULTARROTAVENDEDOR{id};{dataInicial};{dataFinal}";

				rotas = vController.getRotas(str, dns.Host, dns.Port);

				if (rotas.Contains("SEM ROTA"))
					rotas = string.Empty;
			}
			else
			{
				RunOnUiThread(() =>
				{
					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("AVISO");
					builder.SetMessage("FAVOR SELECIONE UM VENDEDOR");
					builder.SetNeutralButton("OK", (s, a) =>
					{
						txtDATAF.RequestFocus();
					});
					builder.SetCancelable(false);
					AlertDialog dialog = builder.Create();
					dialog.Show();
				});
			}
			RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);

			return rotas;
		}

		private void LoadListVendedores()
		{
			new Thread(() =>
			{
				RunOnUiThread(() => progressBar.Visibility = ViewStates.Visible);

				RunOnUiThread(() => listView.Adapter = null);
				ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.simplelist);

				List<Vendedor> vededores = this.getVendedores();

				vededores.ForEach(v => adapter.Add(v.NOMVEND));

				RunOnUiThread(() => listView.Adapter = adapter);

				RunOnUiThread(() => progressBar.Visibility = ViewStates.Invisible);

			}).Start();
		}

		private List<Vendedor> getVendedores()
		{
			VendedorController vController = new VendedorController();

			this.listaVendedores = vController.FindAll();
			return this.listaVendedores;
		}

		private void LimpaTela()
		{
			selecionado = null;
			txtDATAI.Text = string.Empty;
			txtDATAF.Text = string.Empty;

			this.LoadData();
		}

		private void EnableViews(bool enable = true)
		{
			txtDATAI.Enabled = enable;
			txtDATAF.Enabled = enable;
			btnBUSCAR.Enabled = enable;
			btnMAPA.Enabled = enable;
			listView.Enabled = enable;
		}


	}
}