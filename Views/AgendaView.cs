using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EloPedidos.Views
{
	[Activity(Label = "AgendaView")]
	public class AgendaView : Activity
	{
		private TextInputEditText txtDATARET;
		private Button btnBUSCAR;
		private ListView listView;
		private ImageButton btnSINC;
		private ProgressBar progressBar;

		public List<Agenda> agenda { get; set; } = null;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_agenda);

			txtDATARET = FindViewById<TextInputEditText>(Resource.Id.txtDATARET);
			btnBUSCAR = FindViewById<Button>(Resource.Id.btnBUSCAR);
			listView = FindViewById<ListView>(Resource.Id.listView);
			btnSINC = FindViewById<ImageButton>(Resource.Id.btnSINC);
			progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

			progressBar.Visibility = ViewStates.Invisible;

			agenda = new List<Agenda>();

			txtDATARET.Text = DateTime.Now.ToString("dd/MM/yyyy");

			btnBUSCAR.Click += (sender, args) =>
			{
				LoadList();
			};


			btnSINC.Click += (sender, args) =>
			{
				if (!string.IsNullOrEmpty(txtDATARET.Text))
				{
					if (agenda != null)
						agenda.Clear();
					listView.Adapter = null;

					SelectText(txtDATARET);
					//if (!txtDATARET.HasFocus)
					//{
					if (Utils.Format.DateToString(txtDATARET.Text, out string newDate))
					{
						txtDATARET.Text = newDate;

						sincronizarPedidos();
					}
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
					//}
					txtDATARET.SetSelectAllOnFocus(true);
				}
			};

			txtDATARET.FocusChange += (s, a) =>
			{
				if (txtDATARET.HasFocus)
					DataPickerDialog(txtDATARET);
			};
			txtDATARET.LongClick += (s, a) => DataPickerDialog(txtDATARET);

			listView.ItemClick += (sender, args) =>
			{
				var adapter = (AgendaAdapter)listView.Adapter;
				var item = $"{adapter[args.Position].ID_PESSOA} {adapter[args.Position].NROPEDID} {adapter[args.Position].VLRAREC} {adapter[args.Position].FT_PEDIDO_ID}";

				//Intent i = new Intent(this, typeof(DevolucaoPedidoView));
				Intent i = new Intent(this, typeof(DevolucaoPedidoView));
				i.PutExtra("CG_PESSOA_ID", item.ToString());
				SetResult(Result.Ok, i);
				StartActivity(i);
			};
		}
		private void LoadListView()
		{
			AgendaAdapter agendaAdapter = new AgendaAdapter(this, agenda.OrderBy(p => p.NOMFANT).Where(a => a.VLRAREC > 0).ToList());
			this.listView.Adapter = agendaAdapter;
		}

		public void SelectText(EditText editText)
		{
			if (editText.IsFocused == true)
				editText.SelectAll();
		}

		private void LoadList()
        {
			if (!string.IsNullOrEmpty(txtDATARET.Text))
			{
				if (agenda != null)
					agenda.Clear();
				listView.Adapter = null;

				SelectText(txtDATARET);
				//if (!txtDATARET.HasFocus)
				//{
				if (Utils.Format.DateToString(txtDATARET.Text, out string newDate))
				{
					txtDATARET.Text = newDate;

					Empresa empresa = new EmpresaController().GetEmpresa();
					double vendedor = new VendedorController().GetVendedor().CG_VENDEDOR_ID.Value;

					List<Pedido> pedidos = new AgendaController().FindByData(DateTime.Parse(newDate));

					if (pedidos.Count > 0)
					{
						foreach (var p in pedidos)
						{
							var baixa = new BaixasPedidoController().GerarBaixa(p);
							Pessoa pessoa = new PessoaController().FindById(p.ID_PESSOA.Value);
							Agenda a = new Agenda()
							{
								FT_PEDIDO_ID = p.FT_PEDIDO_ID.Value,
								NROPEDID = p.NROPEDID,
								ID_PESSOA = p.ID_PESSOA,
								CG_PESSOA_ID = p.CG_PESSOA_ID,
								DATEMISS = p.DATEMISS,
								DATERET = baixa.DATVCTO.ToString("dd/MM/yyyy"),
								VLRAREC = baixa.VLRRECBR
							};
							if (pessoa != null)
								a.NOMFANT = pessoa.NOMFANTA;

							agenda.Add(a);
						}
						if (agenda.Count > 0)
							LoadListView();
					}
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO DO SISTEMA");
						builder.SetMessage("NENHUM PEDIDO ENCONTRADO PARA RETORNAR NA DATA INFORMADA!");
						builder.SetNeutralButton("OK", (sender, args) =>
						{
							txtDATARET.RequestFocus();
							return;
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
				}
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
				//}
				txtDATARET.SetSelectAllOnFocus(true);
			}
		}

		protected override void OnResume()
		{
			if (!string.IsNullOrEmpty(txtDATARET.Text))
			{
				if (agenda != null)
					agenda.Clear();
				listView.Adapter = null;

				SelectText(txtDATARET);

				if (Utils.Format.DateToString(txtDATARET.Text, out string newDate))
				{
					txtDATARET.Text = newDate;

					Empresa empresa = new EmpresaController().GetEmpresa();
					double vendedor = new VendedorController().GetVendedor().CG_VENDEDOR_ID.Value;

					List<Pedido> pedidos = new AgendaController().FindByData(DateTime.Parse(newDate));

					if (pedidos.Count > 0)
					{
						foreach (var p in pedidos)
						{
							var baixa = new BaixasPedidoController().GerarBaixa(p);
							Pessoa pessoa = new PessoaController().FindById(p.ID_PESSOA.Value);
							Agenda a = new Agenda()
							{
								FT_PEDIDO_ID = p.FT_PEDIDO_ID.Value,
								ID_PESSOA = p.ID_PESSOA.Value,
								NROPEDID = p.NROPEDID,
								CG_PESSOA_ID = p.CG_PESSOA_ID,
								DATEMISS = p.DATEMISS,
								DATERET = baixa.DATVCTO.ToString("dd/MM/yyyy"),
								VLRAREC = baixa.VLRRECBR
							};
							if (pessoa != null)
								a.NOMFANT = pessoa.NOMFANTA;

							agenda.Add(a);
						}
						if (agenda.Count > 0)
							LoadListView();
					}
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("AVISO DO SISTEMA");
						builder.SetMessage("NENHUM PEDIDO ENCONTRADO PARA RETORNAR NA DATA INFORMADA!");
						builder.SetNeutralButton("OK", (sender, args) =>
						{
							txtDATARET.RequestFocus();
							return;
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
				}
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
				txtDATARET.SetSelectAllOnFocus(true);
			}

			base.OnResume();
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
				{
					view.Text = date;
					LoadList();
				}

			}, ano, mes, dia).Show();
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
								List<Agenda> lista = new AgendaController().ComSocketOrders($"CARGAPEDIDO{empresa.CODEMPRE}{vendedor.ToString("0000")}      {txtDATARET.Text}000000");

								if (lista.Count > 0)
								{
									foreach (var a in lista)
									{
										if (a.DATERET == txtDATARET.Text)
											agenda.Add(a);
									}
								}
								else
								{
									RunOnUiThread(() =>
									{
										AlertDialog.Builder builder = new AlertDialog.Builder(this);
										builder.SetTitle("AVISO DO SISTEMA");
										builder.SetMessage("NENHUM PEDIDO ENCONTRADO PARA RETORNAR NA DATA INFORMADA!");
										builder.SetNeutralButton("OK", (sender, args) =>
										{
											txtDATARET.RequestFocus();
											return;
										});
										AlertDialog dialog = builder.Create();
										dialog.Show();
									});
								}
							}
						}
					});
				}
				catch (Exception ex)
				{
					Log.Error("Elo_Log", ex.ToString());
				}
				finally
				{
					RunOnUiThread(() =>
					{
						EnableView();
						progressBar.Visibility = ViewStates.Invisible;
						LoadListView();
					});
				}
			}).Start();
		}
		protected void EnableView(bool enable = true)
		{
			txtDATARET.Enabled = enable;
			btnBUSCAR.Enabled = enable;
			listView.Enabled = enable;
			btnSINC.Enabled = enable;
		}
	}
}