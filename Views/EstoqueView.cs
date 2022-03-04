using Android;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Adapter;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZXing.Mobile;

namespace EloPedidos.Views
{
	[Activity(Label = "EstoqueView")]
	public class EstoqueView : Activity
	{
		ListView listView;
		TextInputEditText txtSearch;
		FloatingActionButton fab;

		private List<RomaneioItem> estoque { get; set; }
		private List<Estoque> contagem { get; set; }
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_estoque);

			listView = FindViewById<ListView>(Resource.Id.listView);
			txtSearch = FindViewById<TextInputEditText>(Resource.Id.txtSearch);
			fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

			contagem = new List<Estoque>();

			Models.Config config = new ConfigController().Config;
			if (config.CODEAN)
				fab.Visibility = ViewStates.Visible;
			else
				fab.Visibility = ViewStates.Invisible;


			loadList();

			txtSearch.TextChanged += (sender, args) => loadList(txtSearch.Text);

			listView.ItemClick += (sender, args) =>
			{
				var adapter = (EstoqueAdapter)listView.Adapter;
				var aux = adapter[args.Position];
				var item = new RomaneioController().FindItemById(aux.ES_ESTOQUE_ROMANEIO_ITEM_ID);

				editItem(item);
			};

			fab.Click += async (sender, args) =>
			{
				this.CheckPermission();

				await contagemEstoqueAsync();
			};

		}
		private async System.Threading.Tasks.Task<ZXing.Result> scanAsync()
		{
			string arrey = "";

			contagem.ForEach(c =>
			{
				arrey = arrey + c.NOMPROD + " - " + c.QTDPROD + "\n";
			});

			MobileBarcodeScanner scanner = new MobileBarcodeScanner(this);

			MobileBarcodeScanningOptions options = new MobileBarcodeScanningOptions();
			options.UseFrontCameraIfAvailable = true;
			scanner.TopText = arrey;

			return await scanner.Scan();
		}
		private async System.Threading.Tasks.Task contagemEstoqueAsync()
		{
			ZXing.Result result = null;
			MediaPlayer _player;

			try
			{
				result = await scanAsync();
			}
			catch (Exception e)
			{
				Log.Error("BarCode_Exception", e.ToString());
			}
			finally
			{
				_player = MediaPlayer.Create(this, Resource.Raw.beep);
				_player.Start();

				if (result == null)
				{
					if (contagem.Count > 0)
					{
						string arrey = "";

						contagem.ForEach(c =>
						{
							arrey = arrey + c.NOMPROD + " - " + c.QTDPROD + "\n";
						});

						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("CONTAGEM DO ESTOQUE");
						builder.SetMessage(arrey);
						builder.SetNeutralButton("OK", (sender, args) => { return; });
						AlertDialog dialog = builder.Create();
						dialog.Show();

						contagem.Clear();
					}
				}
				else
				{
					var aux = result.Text;
					Produto produto = new ProdutoController().FindProdutoByBC(aux);

					if (produto != null)
					{
						RomaneioItem i = new RomaneioController().FindByIdItem(produto.CG_PRODUTO_ID.Value);
						if (i != null)
						{
							Estoque e = new Estoque();
							e.ITEM_ESTOQUE_ID = contagem.Count + 1;
							e.BARCODE = long.Parse(result.ToString());
							e.NOMPROD = i.DSCRPROD;
							e.QTDPROD = 1;

							if (contagem.Count > 0)
							{
								Estoque auxEstoque = contagem.Find(aux => aux.BARCODE == e.BARCODE);
								if (auxEstoque != null)
								{
									contagem.Remove(auxEstoque);

									auxEstoque.QTDPROD += 1;
									contagem.Add(auxEstoque);
								}
								else
									contagem.Add(e);
							}
							else
								contagem.Add(e);

							await contagemEstoqueAsync();
						}
						else
						{
							AlertDialog.Builder builder = new AlertDialog.Builder(this);
							builder.SetTitle("ALERTA");
							builder.SetMessage($"Captura : {aux} \n\nNENHUM PRODUTO ENCONTRADO");
							builder.SetPositiveButton("CONTINUAR", async (sender, args) => { await contagemEstoqueAsync(); });
							builder.SetNeutralButton("FINALIZAR", (sender, args) =>
							{

								string arrey = "";

								contagem.ForEach(c =>
								{
									arrey = arrey + c.NOMPROD + " - " + c.QTDPROD + "\n";
								});

								AlertDialog.Builder builder = new AlertDialog.Builder(this);
								builder.SetTitle("CONTAGEM DO ESTOQUE");
								builder.SetMessage(arrey);
								builder.SetNeutralButton("OK", (sender, args) => { return; });
								AlertDialog dialog = builder.Create();
								dialog.Show();

								contagem.Clear();
							});
							AlertDialog dialog = builder.Create();
							dialog.Show();
						}
					}
					else
					{
						AlertDialog.Builder builder = new AlertDialog.Builder(this);
						builder.SetTitle("ALERTA");
						builder.SetMessage($"Captura : {aux} \n\nNENHUM PRODUTO ENCONTRADO");
						builder.SetPositiveButton("CONTINUAR", async (sender, args) => { await contagemEstoqueAsync(); });
						builder.SetNeutralButton("FINALIZAR", (sender, args) =>
						{

							string arrey = "";

							contagem.ForEach(c =>
							{
								arrey = arrey + c.NOMPROD + " - " + c.QTDPROD + "\n";
							});

							AlertDialog.Builder builder = new AlertDialog.Builder(this);
							builder.SetTitle("CONTAGEM DO ESTOQUE");
							builder.SetMessage(arrey);
							builder.SetNeutralButton("OK", (sender, args) => { return; });
							AlertDialog dialog = builder.Create();
							dialog.Show();

							contagem.Clear();
						});
						AlertDialog dialog = builder.Create();
						dialog.Show();
					}
				}
			}
		}

		private void CheckPermission()
		{
			if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Camera) != Android.Content.PM.Permission.Granted)
				Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, 6);
			if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(ApplicationContext, Manifest.Permission.Flashlight) != Android.Content.PM.Permission.Granted)
				Android.Support.V4.App.ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Flashlight }, 6);
		}

		private void loadList(string name = "")
		{
			if (name == "")
				estoque = new RomaneioController().FindAll();
			else
				estoque = new RomaneioController().FindBy_NOMPROD(name);

			if (estoque.Count > 0)
			{
				EstoqueAdapter adapter = new EstoqueAdapter(this, estoque.OrderBy(i => i.DSCRPROD).ToList());
				listView.Adapter = adapter;
			}
		}

		private void editItem(RomaneioItem item)
		{
			ProdutoController pController = new ProdutoController();
			View viewAdd = this.LayoutInflater.Inflate(Resource.Layout.dialog_estoque, null);
			Produto produto = pController.FindById(item.CG_PRODUTO_ID);

			RomaneioController rController = new RomaneioController();

			EditText txtNOMPROD = viewAdd.FindViewById<EditText>(Resource.Id.txtNOMPROD);
			EditText txtBARCODE = viewAdd.FindViewById<EditText>(Resource.Id.txtBARCODE);
			TextView lblQTDPRO = viewAdd.FindViewById<TextView>(Resource.Id.lblQTDPRO);

			txtNOMPROD.Text = (item.DSCRPROD != "") ? item.DSCRPROD : "";
			if (produto != null)
				txtBARCODE.Text = (produto.CODEAN != "") ? produto.CODEAN : "";

			lblQTDPRO.Text = item.QTDPROD.ToString();

			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.SetTitle("CONTROLE DE ESTOQUE");
			builder.SetView(viewAdd);
			builder.SetPositiveButton("SALVAR", (sender, args) =>
			{
				if (!string.IsNullOrEmpty(txtNOMPROD.Text))
				{
					item.DSCRPROD = txtNOMPROD.Text;
					produto.CODEAN = (!string.IsNullOrEmpty(txtBARCODE.Text)) ? txtBARCODE.Text : "";
					item.DTHULTAT = DateTime.Now;
					item.USRULTAT = new OperadorController().GetOperador().NOMOPER;

					if (rController.SaveItem(item))
					{
						if(pController.Save(produto))
							Toast.MakeText(this, "SALVO COM SUCESSO", ToastLength.Long).Show();

						loadList();
						return;
					}
				}
				else
					Toast.MakeText(this, "FAVOR PREENCHA AO MENOS O NOME DO PRODUTO", ToastLength.Long).Show();

			});
			builder.SetNegativeButton("CANCELAR", (sender, args) => { return; });

			AlertDialog dialog = builder.Create();
			dialog.Show();
		}
	}
}