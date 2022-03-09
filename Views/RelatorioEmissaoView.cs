using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Widget;
using EloPedidos.Adapter;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EloPedidos.Views
{
	[Activity(Label = "RelatorioEstoqueView")]
	public class RelatorioEmissaoView : Activity
	{
		private List<EmissaoAdapterCls> emissaoLista { get; set; } = null;
		private ListView listView;
		private TextInputEditText txPesquisa, txDATAF, txDATAI;
		private Button btnImprimir;

		/// <summary>
		/// Auxiliar para impressão
		/// </summary>
		public List<EmissaoAdapterCls> list { get; set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_relatorioEmissao);

			listView = FindViewById<ListView>(Resource.Id.listView);
			txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
			txDATAF = FindViewById<TextInputEditText>(Resource.Id.txDATAF);
			txDATAI = FindViewById<TextInputEditText>(Resource.Id.txDATAI);
			btnImprimir = FindViewById<Button>(Resource.Id.btnImprimir);

			emissaoLista = new List<EmissaoAdapterCls>();

			list = new List<EmissaoAdapterCls>();

			LoadList();

			//txPesquisa.TextChanged += (sender, args) => LoadListByDscr(txPesquisa.Text);
			txPesquisa.TextChanged += (sender, args) => Load(txPesquisa.Text, txDATAI.Text, txDATAF.Text);

			btnImprimir.Click += (sender, args) => Imprimir();

			listView.ItemClick += (sender, args) =>
			{
				var adapter = (EmissaoRelatorioAdapter)listView.Adapter;
				var result = adapter[args.Position].NROPED;
				Intent i = new Intent();
				i.PutExtra("resultPedido", result.ToString());
				SetResult(Result.Ok, i);
				Finish();
			};
			txDATAI.FocusChange += (sender, args) =>
			{
				if (Format.DateToString(txDATAI.Text, out string newDate))
					txDATAI.Text = newDate;
				else
				{
					Toast.MakeText(Application.Context, "DATA INVÁLIDA, POR FAVOR VERIFIQUE!", ToastLength.Short).Show();
				}

				if (txDATAI.HasFocus)
					DataPickerDialog(txDATAI);

				Load(txPesquisa.Text, txDATAI.Text, txDATAF.Text);
			};
			txDATAF.FocusChange += (sender, args) =>
			{
				if (Format.DateToString(txDATAF.Text, out string newDate))
					txDATAF.Text = newDate;
				else
				{
					Toast.MakeText(Application.Context, "DATA INVÁLIDA, POR FAVOR VERIFIQUE!", ToastLength.Short).Show();
				}

				if (txDATAF.HasFocus)
					DataPickerDialog(txDATAI);

				Load(txPesquisa.Text, txDATAI.Text, txDATAF.Text);
			};
			txDATAI.LongClick += (s, a) => DataPickerDialog(txDATAI);
			txDATAF.LongClick += (s, a) => DataPickerDialog(txDATAF);

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
		private void LoadList()
		{
			this.listView.Adapter = null;
			emissaoLista.Clear();
			var pedido = new PedidoController().FindAll();
			var romaneio = new RomaneioController().FindLast();
			if (romaneio != null)
			{
				if (pedido.Count > 0)
				{
					pedido.Where(p => p.ES_ESTOQUE_ROMANEIO_ID == romaneio.ES_ESTOQUE_ROMANEIO_ID).ToList().ForEach(p =>
					{
						var cliente = new PessoaController().FindById(p.ID_PESSOA.Value).NOMPESS;
						var vlrpedido = new BaixasPedidoController().ValorTotalToString(p.NROPEDID);
						string situacao = Enum.GetName(typeof(Pedido.SitPedido), p.SITPEDID);
						emissaoLista.Add(new EmissaoAdapterCls()
						{
							NROPED = p.NROPEDID,
							NOMCLIE = cliente,
							VLRPED = vlrpedido,
							IDTSIT = situacao.Equals("ParcialTotal") ? "Parcial Total" : situacao,
							DATAEMISS = p.DATEMISS.ToString()

						});
					});

					if (emissaoLista.Count > 0)
					{
						var adapter = new EmissaoRelatorioAdapter(this, emissaoLista);
						this.listView.Adapter = adapter;
						this.list = emissaoLista;
					}
					else
					{
						this.Msg($"NENHUM PEDIDO ENCONTRADO PARA O ROMANEIO N°{ romaneio.NROROMAN}");
					}
				}
			}
			else
			{
				this.Msg("FAVOR CARREGAR O RAMANEIO");
			}
		}

		private void LoadListByDscr(string name)
		{
			this.listView.Adapter = null;
			emissaoLista.Clear();
			list.Clear();
			var pedido = new PedidoDAO().FindByName(name);
			pedido.ForEach(p =>
			{
				var cliente = new PessoaController().FindById(p.ID_PESSOA.Value).NOMPESS;
				var vlrpedido = new BaixasPedidoController().ValorTotalToString(p.NROPEDID);
				string situacao = Enum.GetName(typeof(Pedido.SitPedido), p.SITPEDID);
				emissaoLista.Add(new EmissaoAdapterCls()
				{
					NROPED = p.NROPEDID,
					NOMCLIE = cliente,
					VLRPED = vlrpedido,
					IDTSIT = situacao.Equals("ParcialTotal") ? "Parcial Total" : situacao,
					DATAEMISS = p.DATEMISS.ToString()


				});
			});

			var adapter = new EmissaoRelatorioAdapter(this, emissaoLista);
			this.listView.Adapter = adapter;
			this.list = emissaoLista;
		}

		private void Load(string name, string dataini, string dataf)
		{
			List<Pedido> pedido;
			this.listView.Adapter = null;
			emissaoLista.Clear();
			list.Clear();
			pedido = new PedidoDAO().FindByName(name);
			var romaneio = new RomaneioController().FindLast();
			if (!dataini.IsEmpty() && !dataf.IsEmpty())
				pedido = pedido.Where(p => p.DATEMISS >= (DateTime)dataini.ToDate() &&
											p.DATEMISS <= (DateTime)dataf.ToDate()).ToList();



			pedido.Where(p => p.ES_ESTOQUE_ROMANEIO_ID == romaneio.ES_ESTOQUE_ROMANEIO_ID).ToList().ForEach(p =>
			{
				var cliente = new PessoaController().FindById(p.ID_PESSOA.Value).NOMPESS;
				var vlrpedido = new BaixasPedidoController().ValorTotalToString(p.NROPEDID);
				string situacao = Enum.GetName(typeof(Pedido.SitPedido), p.SITPEDID);
				emissaoLista.Add(new EmissaoAdapterCls()
				{
					NROPED = p.NROPEDID,
					NOMCLIE = cliente,
					VLRPED = vlrpedido,
					IDTSIT = situacao.Equals("ParcialTotal") ? "Parcial Total" : situacao,
					DATAEMISS = p.DATEMISS.ToString()


				});
			});

			if (emissaoLista.Count > 0)
			{
				var adapter = new EmissaoRelatorioAdapter(this, emissaoLista);
				this.listView.Adapter = adapter;
				this.list = emissaoLista;
			}
			else
			{
				this.Msg($"NENHUM PEDIDO ENCONTRADO PARA O ROMANEIO N°{ romaneio.NROROMAN}");
			}
		}

		private void Imprimir()
		{
			string NOMIMPRE = "";
			Models.Config config = new ConfigController().GetConfig();
			if (config.NOMIMPRE != "")
				NOMIMPRE = config.NOMIMPRE;

			if (this.list != null && this.list.Count > 0)
			{
				if (NOMIMPRE != "")
				{
					this.Msg("ENVIANDO IMPRESSÃO PARA O DISPOSITIVO! AGUARDE...");

					var printerController = new PrinterController();
					string text;
					text = printerController.FormatOrderForPrintA7(this.list);
					var socket = printerController.GetSocket(NOMIMPRE);

					if (socket != null)
					{
						if (!socket.IsConnected)
							printerController.ConnectPrinter(socket, NOMIMPRE);

						if (socket.IsConnected)
						{
							socket.OutputStream.Write(text.ToASCII(), 0, text.ToASCII().Length);
							printerController.ClosePrinter();
						}
						else
						{
							this.Msg("FAVOR, LIGUE A IMPRESSORA!");
						}
					}
					else
					{
						this.Msg("IMPOSSÍVEL IMPRIMIR COM O DISPOSITIVO SELECIONADO!");
					}
				}
				else
					this.Msg("NENHUM DISPOSITIVO DE IMPRESSÃO DEFINIDO!");
			}
			else
				this.Msg("NENHUM PEDIDO ENCONTRADO!");
		}
	}
}