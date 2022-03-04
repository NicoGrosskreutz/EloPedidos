using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EloPedidos.Models;
using EloPedidos.Controllers;
using EloPedidos.Adapter;
using EloPedidos.Utils;
using Android.Content.PM;
using Android.Support.Design.Widget;
using EloPedidos.Persistence;

namespace EloPedidos.Views
{
	[Activity(Label = "RelatorioDevolucoesView", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
	public class RelatorioDevolucoesView : Activity
	{
		private List<DevolucoesAdapterCls> devolucaoLista { get; set; } = null;
		private ListView listView;
		private TextInputEditText txPesquisa, txNROPEDID, txCLIENTE;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_relatoriodevolucoes);

			listView = FindViewById<ListView>(Resource.Id.listView);
			txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
			txNROPEDID = FindViewById<TextInputEditText>(Resource.Id.txNROPEDID);
			txCLIENTE = FindViewById<TextInputEditText>(Resource.Id.txCLIENTE);

			devolucaoLista = new List<DevolucoesAdapterCls>();

			LoadList();

			txPesquisa.TextChanged += (sender, args) => LoadListByPesquisa(txPesquisa.Text, txNROPEDID.Text, txCLIENTE.Text);
			txNROPEDID.TextChanged += (sender, args) => LoadListByPesquisa(txPesquisa.Text, txNROPEDID.Text, txCLIENTE.Text);
			txCLIENTE.TextChanged += (sender, args) => LoadListByPesquisa(txPesquisa.Text, txNROPEDID.Text, txCLIENTE.Text);

			txPesquisa.LongClick += (sender, eventArgs) =>
			{
				if (new ProdutoController().FindAll().Count == 0)
					this.Msg("NÃO HÁ PRODUTOD CADASTRADOS!");
				else
				{
					Intent i = new Intent(ApplicationContext, typeof(BuscarProdutoView));
					StartActivityForResult(i, 1);
				}
			};

			txNROPEDID.LongClick += (sender, eventArgs) =>
			{
				if (new PedidoController().FindAll().Count == 0)
					this.Msg("NÃO HÁ PEDIDOS CADASTRADOS!");
				else
				{
					Intent i = new Intent(ApplicationContext, typeof(BuscarPedidoView));
					StartActivityForResult(i, 2);
				}
			};
			txCLIENTE.LongClick += (sender, eventArgs) =>
			{
				if (new PessoaDAO().FindAll().Count == 0)
					this.Msg("NÃO HÁ CLIENTES CADASTRADOS!");
				else
				{
					ListView listView;
					TextInputEditText txPesquisa;
					CheckBox ckMUNIC;
					TextView txMUNIC;

					View view = this.LayoutInflater.Inflate(Resource.Layout.activity_buscacliente, null);

					listView = view.FindViewById<ListView>(Resource.Id.listView);
					txPesquisa = view.FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
					ckMUNIC = view.FindViewById<CheckBox>(Resource.Id.ckMUNIC);
					txMUNIC = view.FindViewById<TextView>(Resource.Id.txMUNIC);

					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetView(view);
					builder.SetNeutralButton("CONCLUIDO", (s, a) => { return; });
					AlertDialog dialog = builder.Create();
					dialog.Show();

					txPesquisa.RequestFocus();

					int MUNIC_ID = 0;

					if (new ConfigController().GetConfig().CODMUNPQ != 0)
					{
						MUNIC_ID = new ConfigController().GetConfig().CODMUNPQ;
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

					List<Pessoa> clientes = new PessoaController().FindAll();

					if (clientes.Count > 0)
					{
						List<Pessoa> aux = new List<Pessoa>();
						if (!string.IsNullOrEmpty(txMUNIC.Text))
						{
							clientes.ForEach(p =>
							{
								var munic = new MunicipioController().FindById(p.CODMUNIC);
								if (munic != null && munic.CODMUNIC == MUNIC_ID)
									aux.Add(p);
							});
							clientes.Clear();
							clientes = aux;
						}

						var adapter = new AdapterBuscarCliente(this, clientes);
						listView.Adapter = adapter;
					}

					txMUNIC.Click += (sender, args) =>
					{
						if (ckMUNIC.Checked == true)
							ckMUNIC.Checked = false;
						else
							ckMUNIC.Checked = true;
					};

					txPesquisa.TextChanged += (s, a) =>
					{
						List<Pessoa> aux = new List<Pessoa>();
						if (!string.IsNullOrEmpty(txPesquisa.Text))
						{
							clientes = new PessoaController().FindByName(txPesquisa.Text);
							if (ckMUNIC.Checked == true && !string.IsNullOrEmpty(txMUNIC.Text))
							{
								clientes.ForEach(c =>
								{
									var munic = new MunicipioController().FindById(c.CODMUNIC);
									if (munic != null && munic.CODMUNIC == MUNIC_ID)
										aux.Add(c);
								});
								clientes.Clear();
								clientes = aux;
							}
						}
						else
						{
							clientes = new PessoaController().FindAll();
							if (ckMUNIC.Checked == true && !string.IsNullOrEmpty(txMUNIC.Text))
							{
								clientes.ForEach(c =>
								{
									var munic = new MunicipioController().FindById(c.CODMUNIC);
									if (munic != null && munic.CODMUNIC == MUNIC_ID)
										aux.Add(c);
								});
								clientes.Clear();
								clientes = aux;
							}
						}

						var adapter = new AdapterBuscarCliente(this, clientes);
						listView.Adapter = adapter;
					};

					ckMUNIC.CheckedChange += (s, a) =>
					{
						List<Pessoa> aux = new List<Pessoa>();

						if (!string.IsNullOrEmpty(txPesquisa.Text))
							clientes = new PessoaController().FindByName(txPesquisa.Text);
						else
							clientes = new PessoaController().FindAll();

						if (ckMUNIC.Checked && !string.IsNullOrEmpty(txMUNIC.Text))
						{
							clientes.ForEach(c =>
							{
								var munic = new MunicipioController().FindById(c.CODMUNIC);
								if (munic != null && munic.CODMUNIC == MUNIC_ID)
									aux.Add(c);
							});
							clientes.Clear();
							clientes = aux;
						}

						var adapter = new AdapterBuscarCliente(this, clientes);
						listView.Adapter = adapter;
					};

					listView.ItemClick += (s, a) =>
					{
						var adapter = (AdapterBuscarCliente)listView.Adapter;
						var cliente = adapter[a.Position];

						txCLIENTE.Text = (cliente.CODPESS != null) ? cliente.NOMPESS.ToString() : "";
						dialog.Cancel();
					};
				}
			};
		}

		private void LoadList()
		{
			this.listView.Adapter = null;
			devolucaoLista.Clear();
			var devolucoes = new DevolucaoItemController().FindAll();
			devolucoes.ForEach(d =>
			{
				string prod;
				if (d.NOMPROD != "")
					prod = d.NOMPROD;
				else
				{
					prod = new ProdutoController().FindByCODPROD(d.CODPROD).DSCPROD;
				}
				if (d.FT_PEDIDO_ID != null)
				{
					var ped = new PedidoController().FindById(d.FT_PEDIDO_ID.Value);
					string pess = new PessoaController().FindById(ped.ID_PESSOA.Value).NOMPESS;
					devolucaoLista.Add(new DevolucoesAdapterCls()
					{
						NROPED = d.NROPEDIDO.ToString(),
						CODPROD = d.CODPROD.ToString(),
						NOMPESS = pess,
						DSCRPRO = prod,
						QTDDEVOL = d.QTDDEVOL.ToString()
					});
				}
			});

			var adapter = new RelatorioDevolucoesAdapter(this, devolucaoLista);
			this.listView.Adapter = adapter;

		}
		private void LoadListByPesquisa(string name, string nropedid, string nompess)
		{
			this.listView.Adapter = null;
			devolucaoLista.Clear();
			List<DevolucaoItem> devolucoes = new DevolucaoItemController().FindAll();
			List<DevolucaoItem> aux = new List<DevolucaoItem>();

			devolucoes = devolucoes.Where(d => (d.NROPEDIDO != null && (d.NROPEDIDO.ToString().ToLower().StartsWith(nropedid.ToLower()) || nropedid == ""))).ToList();
			devolucoes = devolucoes.Where(d => (d.NOMPESS != null && (d.NOMPESS.ToLower().StartsWith(nompess.ToLower()) || nompess == ""))).ToList();
			devolucoes = devolucoes.Where(d => (d.NOMPROD != null && (d.NOMPROD.ToLower().StartsWith(name.ToLower()) || name == ""))).ToList();

			devolucoes.ForEach(d =>
			{
				string prod;
				if (d.NOMPROD != "")
					prod = d.NOMPROD;
				else
				{
					prod = new ProdutoController().FindByCODPROD(d.CODPROD).DSCPROD;
				}
				if (d.FT_PEDIDO_ID != null)
				{
					var ped = new PedidoController().FindById(d.FT_PEDIDO_ID.Value);
					string pess = new PessoaController().FindById(ped.ID_PESSOA.Value).NOMPESS;
					devolucaoLista.Add(new DevolucoesAdapterCls()
					{
						NROPED = d.NROPEDIDO.ToString(),
						CODPROD = d.CODPROD.ToString(),
						NOMPESS = pess,
						DSCRPRO = prod,
						QTDDEVOL = d.QTDDEVOL.ToString()
					});
				}
			});

			var adapter = new RelatorioDevolucoesAdapter(this, devolucaoLista);
			this.listView.Adapter = adapter;
		}

		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 1)
				if (resultCode == Result.Ok)
				{
					string resultProd = data.GetStringExtra("resultProd");

					if (!string.IsNullOrEmpty(resultProd))
					{
						string prod = new ProdutoController().FindByCODPROD(resultProd.ToLong()).DSCPROD;
						txPesquisa.Text = prod;
					}
				}

			if (requestCode == 2)
				if (resultCode == Result.Ok)
				{
					string resultPedido = data.GetStringExtra("resultPedido");
					if (!string.IsNullOrEmpty(resultPedido))
						txNROPEDID.Text = resultPedido;
				}
		}

	}
}