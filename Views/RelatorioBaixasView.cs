using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EloPedidos.Adapter;
using EloPedidos.Utils;
using Android.Support.V7.Widget;
using Android.Support.Design.Widget;
using EloPedidos.Persistence;

namespace EloPedidos.Views
{
	[Activity(Label = "RelatorioBaixasView")]
	public class RelatorioBaixasView : Activity
	{
		private EditText txtVLRTOTAL;
		private TextInputEditText txtCODPESS;
		private TextView lblNOMPESS, lblDOC, lblNOMCID, lblGERAL;
		private ListView listView;
		private CheckBox ckGERAL;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_relatorioBaixas);
			LoadView();

			LoadListView();
			txtCODPESS.Focusable = false;

			txtCODPESS.LongClick += (s, a) =>
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

						txtCODPESS.Text = (cliente.CODPESS != null) ? cliente.CODPESS.ToString() : "";
						dialog.Cancel();
						LoadListView(cliente.ID.Value);
					};
				}
			};

			txtCODPESS.TextChanged += (s, a) =>
			{
				if (txtCODPESS.Text != "")
				{
					var cliente = new PessoaController().FindByCODPESS(txtCODPESS.Text.ToLong());
					lblNOMPESS.Text = cliente.NOMFANTA;
					lblDOC.Text = cliente.IDTPESS.ToString();
					Municipio municipio = new MunicipioController().FindById(cliente.CODMUNIC);
					if (municipio != null)
						lblNOMCID.Text = municipio.NOMMUNIC;

					string auxIDTPESS = RemoveMasks.RemoveMasksToString(cliente.IDTPESS.ToString());

					if (auxIDTPESS.Length == 14)
						lblDOC.Text = Convert.ToUInt64(cliente.IDTPESS.ToString()).ToString(@"00\.000\.000\/0000\-00");
					else if (auxIDTPESS.Length == 11)
						lblDOC.Text = Convert.ToUInt64(cliente.IDTPESS.ToString()).ToString(@"000\.000\.000\-00");
					else
						lblDOC.Text = cliente.IDTPESS.ToString();

					LoadListView(cliente.ID.Value);
				}

			};
			ckGERAL.CheckedChange += (s, a) =>
			{
				if (ckGERAL.Checked)
				{
					txtCODPESS.Text = "";
					lblNOMPESS.Text = "";
					lblDOC.Text = "";
					lblNOMCID.Text = "";
					LoadListView();
				}
			};
			lblGERAL.Click += (s, a) =>
			{
				if (ckGERAL.Checked)
					ckGERAL.Checked = false;
				else
					ckGERAL.Checked = true;
			};

			listView.ItemClick += (s, a) =>
			{
				var adapter = (RelatorioBaixasAdapter)listView.Adapter;
				var item = adapter[a.Position];
				Pedido pedido = new PedidoController().FindById(item.FT_PEDIDO_ID.Value);
				if (pedido != null)
				{
					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle($"PEDIDO N° {pedido.NROPEDID}");
					builder.SetNeutralButton("OK", (s, a) => { return; });
					AlertDialog dialog = builder.Create();
					dialog.Show();
				}
			};
		}
		private void LoadView()
		{
			txtCODPESS = FindViewById<TextInputEditText>(Resource.Id.txtCODPESS);
			txtVLRTOTAL = FindViewById<EditText>(Resource.Id.txtVLRTOTAL);
			lblNOMPESS = FindViewById<TextView>(Resource.Id.lblNOMPESS);
			lblDOC = FindViewById<TextView>(Resource.Id.lblDOC);
			lblGERAL = FindViewById<TextView>(Resource.Id.lblGERAL);
			lblNOMCID = FindViewById<TextView>(Resource.Id.lblNOMCID);
			listView = FindViewById<ListView>(Resource.Id.listView);
			ckGERAL = FindViewById<CheckBox>(Resource.Id.ckGERAL);
		}
		private void LoadListView(long? id = null)
		{
			txtVLRTOTAL.Text = "";
			listView.Adapter = null;

			List<Pagamento> baixas = new BaixasPedidoController().FindAllBaixas().Where(b => b.VLRPGMT.ToString("C2") != "R$ 0,00").OrderByDescending(d => d.DTHULTAT).ToList();
			if (id == null)
			{
				listView.Adapter = new RelatorioBaixasAdapter(this, baixas);
				LoadVlrTotal(baixas);
			}
			else
			{
				ckGERAL.Checked = false;
				List<Pagamento> b = new List<Pagamento>();
				List<Pedido> pedidos = new PedidoController().FindBy_ID_PESSOA(id.Value);
				if (pedidos.Count > 0)
				{
					baixas.ForEach(ba =>
					{
						pedidos.ForEach(p =>
						{
							if (p.FT_PEDIDO_ID.Value == ba.FT_PEDIDO_ID.Value)
								b.Add(ba);
						});
					});

					if (b.Count > 0)
					{
						listView.Adapter = new RelatorioBaixasAdapter(this, b);
						LoadVlrTotal(b);
					}
				}

			}
		}
		private void LoadVlrTotal(List<Pagamento> b)
		{
			double vlr = 0;
			b.ForEach(a =>
			{
				vlr += a.VLRPGMT;
			});
			txtVLRTOTAL.Text = vlr.ToString("C2");
		}


		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 1)
				if (resultCode == Result.Ok)
				{
					string result = data.GetStringExtra("result");
					if (!result.IsEmpty())
					{
						txtCODPESS.Text = result;
					}
				}

		}
	}
}