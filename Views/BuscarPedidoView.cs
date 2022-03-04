using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Persistence;
using EloPedidos.Models;
using EloPedidos.Services;
using EloPedidos.Utils;
using M = EloPedidos.Models;
using SQLite;
using Android.Views.InputMethods;
using Android.Support.Design.Widget;
using EloPedidos.Adapter;

namespace EloPedidos.Views
{
	[Activity(Label = "BuscarPedidoView")]
	public class BuscarPedidoView : Activity
	{
		protected TextInputEditText txPesquisa, txDataI, txDataF;
		protected ListView listView;
		protected CheckBox ckCancelados;
		protected CheckBox ckMunicipio;
		protected TextView lbCheckBoxMUNIC, lbCheckBox;
		//private Button relatorio;

		SQLiteConnection conn = Database.GetConnection();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your application here
			SetContentView(Resource.Layout.activity_buscapedido);

			txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
			lbCheckBoxMUNIC = FindViewById<TextView>(Resource.Id.lbCheckBoxMUNIC);
			lbCheckBox = FindViewById<TextView>(Resource.Id.lbCheckBox);
			listView = FindViewById<ListView>(Resource.Id.listView);
			ckCancelados = FindViewById<CheckBox>(Resource.Id.ckCancelados);
			ckMunicipio = FindViewById<CheckBox>(Resource.Id.ckMunicipio);
			txDataI = FindViewById<TextInputEditText>(Resource.Id.txDataI);
			txDataF = FindViewById<TextInputEditText>(Resource.Id.txDataF);
			//relatorio = FindViewById<Button>(Resource.Id.relatorio);

			ckCancelados.Checked = false;

			NextFocus(txPesquisa);

			M.Config config = new ConfigController().Config;
			if (!string.IsNullOrEmpty(config.NOMMUNPQ))
			{
				lbCheckBoxMUNIC.Text = config.NOMMUNPQ.ToString();
				ckMunicipio.Checked = true;
			}
			else
			{
				ckMunicipio.Checked = false;
				ckMunicipio.Visibility = ViewStates.Invisible;
				lbCheckBoxMUNIC.Text = "";
			}

			lbCheckBoxMUNIC.Click += (sender, args) =>
			{
				if (ckMunicipio.Checked == false)
					ckMunicipio.Checked = true;
				else
					ckMunicipio.Checked = false;
			};
			lbCheckBox.Click += (sender, args) =>
			{
				if (ckCancelados.Checked == false)
					ckCancelados.Checked = true;
				else
					ckCancelados.Checked = false;
			};

			Pesquisa();

			txPesquisa.TextChanged += (sender, args) => Pesquisa();
			ckCancelados.CheckedChange += (sender, args) => Pesquisa();
			ckMunicipio.CheckedChange += (sender, args) => Pesquisa();

			listView.ItemClick += (sender, args) =>
			{
				try
				{
					//var item = args.Parent.GetItemAtPosition(args.Position);

					//if (item != null)
					//	if (!string.IsNullOrEmpty(item.ToString()))
					//	{
					//		Intent i = new Intent();
					//		i.PutExtra("resultPedido", item.ToString().Split('-')[0].Replace(" ", ""));
					//		SetResult(Result.Ok, i);
					//		Finish();
					//	}

					var adapter = (AdapterBuscarPedidos)listView.Adapter;

					var item = adapter[args.Position];
					if (item != null)
					{
						Intent i = new Intent();
						i.PutExtra("resultPedido", item.NROPEDID.ToString());
						SetResult(Result.Ok, i);
						Finish();
					}
				}
				catch (Exception ex)
				{
					GetError(ex.ToString());
				}
			};


			txDataI.FocusChange += (sender, eventArgs) =>
			{
				if (Format.DateToString(txDataI.Text, out string newdata))
					txDataI.Text = newdata;
				else
				{
					txDataI.Text = newdata;

					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("AVISO DO SISTEMA!");
					builder.SetMessage("FORMATO DA DATA ESTÁ INCORRETO\n(DD/MM/AAAA)");
					builder.SetCancelable(false);
					builder.SetNeutralButton("OK", (s, a) =>
					{
						txDataI.RequestFocus();
						return;
					});
					AlertDialog dialog = builder.Create();
					dialog.Show();
					txDataI.SetSelectAllOnFocus(true);
				}

				if (txDataI.HasFocus)
					DataPickerDialog(txDataI);

				Pesquisa();
			};
			txDataF.FocusChange += (sender, eventArgs) =>
			{
				if (Format.DateToString(txDataI.Text, out string newdata))
					txDataF.Text = newdata;
				else
				{
					txDataF.Text = newdata;

					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("AVISO DO SISTEMA!");
					builder.SetMessage("FORMATO DA DATA ESTÁ INCORRETO\n(DD/MM/AAAA)");
					builder.SetCancelable(false);
					builder.SetNeutralButton("OK", (s, a) =>
					{
						txDataF.RequestFocus();
						return;
					});
					AlertDialog dialog = builder.Create();
					dialog.Show();
					txDataF.SetSelectAllOnFocus(true);
				}

				if (txDataF.HasFocus)
					DataPickerDialog(txDataF);
			};
			txDataI.LongClick += (s, a) => DataPickerDialog(txDataI);
			txDataF.LongClick += (s, a) => DataPickerDialog(txDataF);

			//relatorio.Click += (s, a) =>
			//{
			//	List<Pedido> pedidos = new List<Pedido>();
			//	List<BaixasPedido> baixas = new List<BaixasPedido>();

			//	new BuscarPedidoController().FindOrderToReport(out pedidos, out baixas);

			//	string report = new BuscarPedidoController().ReportCreate(pedidos, baixas);

			//	string url = "https://api.whatsapp.com/send?phone=" + "+55" + "043988591397" + "&text=" + report.Replace(" ", "%20");
			//	Intent i = new Intent(Intent.ActionView);
			//	i.SetData(Android.Net.Uri.Parse(url));
			//	StartActivity(i);
			//};
		}

		public static void HideKeyboard(EditText editText)
		{
			InputMethodManager inputMethodManager = Application.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
			inputMethodManager.HideSoftInputFromWindow(editText.WindowToken, HideSoftInputFlags.None);
		}

		public override void OnBackPressed()
		{
			SetResult(Result.Canceled);
			Finish();
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
		public void Pesquisa()
		{
			M.Config config = new ConfigController().Config;
			string CODMUNIC;
			if (ckMunicipio.Checked == true)
				CODMUNIC = config.CODMUNPQ.ToString();
			else
				CODMUNIC = "";
			try
			{
				if (!txDataI.TestDate() && !txDataI.Text.IsEmpty())
				{
					this.Msg("DATA INVÁLIDA");
				}
				else if (!txDataF.TestDate() && !txDataF.Text.IsEmpty())
				{
					this.Msg("DATA INVÁLIDA");
				}


				else
				{
					var controller = new PedidoController();
					var municController = new MunicipioController();


					if (!txPesquisa.Text.IsEmpty() && !txDataI.Text.IsEmpty() && !txDataF.Text.IsEmpty())
					{
						FormatList(controller.FindByName(txPesquisa.Text, ckCancelados.Checked, CODMUNIC, txDataI.Text.ToDate().Value, txDataF.Text.ToDate().Value));
					}


					else if (txPesquisa.Text.IsEmpty() && !txDataI.Text.IsEmpty() && !txDataF.Text.IsEmpty())
					{
						FormatList(controller.FindAll(ckCancelados.Checked, CODMUNIC, txDataI.Text.ToDate().Value, txDataF.Text.ToDate().Value));
					}

					else if (!txPesquisa.Text.IsEmpty() && txDataI.Text.IsEmpty() && txDataF.Text.IsEmpty())
					{
						FormatList(controller.FindByName(txPesquisa.Text, ckCancelados.Checked, CODMUNIC));
					}

					else if (txPesquisa.Text.IsEmpty() && txDataI.Text.IsEmpty() && txDataF.Text.IsEmpty())
					{
						FormatList(controller.FindAll(ckCancelados.Checked, CODMUNIC));
					}

				}
			}
			catch (Exception ex)
			{
				GetError(ex.ToString());
			}
		}


		/// <summary>
		///  Substituição para os dois métodos de busca acima
		/// </summary>
		/// <param name="name"></param>
		/// <param name="cancelado"></param>
		protected virtual void LoadList(string name, bool cancelado)
		{
			try
			{
				var controller = new PedidoController();
				List<Pedido> pedidos = null;

				if (string.IsNullOrEmpty(name))
					pedidos = controller.FindAll(cancelado);
				else
					pedidos = controller.FindByName(name, cancelado);

				FormatList(pedidos);
			}
			catch (Exception ex)
			{
				GetError(ex.ToString());
			}
		}


		protected void FormatList(List<Pedido> pedidos)
		{
			try
			{
				listView.Adapter = null;

				var adapter = new AdapterBuscarPedidos(this, pedidos.OrderByDescending(p => p.DATEMISS).ToList());
				listView.Adapter = adapter;

				//ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);

				////pedidos.OrderBy(p => p.NROPEDID).ToList().ForEach((aux) =>

				//pedidos.OrderBy(p => p.NROPEDID).ToList().ForEach(aux =>
				//{
				//	adapter.Add($"{aux.NROPEDID} - {aux.Pessoa.NOMFANTA} " +
				//		$"- R$ {new ItemPedidoController().GetTotalValue(aux.FT_PEDIDO_ID.Value).ToString("0.00").Replace(".", ",")} - {aux.DATEMISS.ToString("dd/MM/yyyy")}");

				//});

				//listView.Adapter = adapter;
			}
			catch (Exception ex)
			{
				GetError(ex.ToString());
			}
		}

		protected void GetError(string message)
		{
			string error = "";
			Log.Error(error, message);
			this.Msg(message);
		}

		//protected override void OnDestroy()
		//{
		//	base.OnDestroy();
		//	SendBroadcast(new Intent(this, typeof(GeolocatorBroadCast)));
		//}

		protected void FocusClear()
		{
			if (this.CurrentFocus != null)
				this.CurrentFocus.ClearFocus();
		}

		protected void NextFocus(View v)
		{
			FocusClear();
			v.RequestFocus();
		}
	}

}