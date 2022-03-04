using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EloPedidos.Views
{
	[Activity(Label = "ResumoPedidoView")]
	public class ResumoPedidoView : Activity
	{
		private TextView txNOMPESS, txData, txtFRMPGTO, txtDTAVCMTO, txtSTATUS, txtVLRTOTAL, txtVLRDEVOL, txtCOMISSAO, txtVLRREC, txtSALDO;
		private TextInputEditText txtNROPEDIDO;
		private RelativeLayout relativeLayout1, relativeLayout2, relativeLayout3;
		private CardView cvSALDO, cvVLRREC, cvVLRDEVOL;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_resumoPedido);

			LoadView();
			txtNROPEDIDO.Focusable = false;
			txtNROPEDIDO.LongClick += (s, a) => adPEDIDOS();
			txtNROPEDIDO.TextChanged += (s, a) => CarregarDados(txtNROPEDIDO.Text);
			cvSALDO.Click += (s, a) => LoadNextVist();
			cvVLRREC.Click += (s, a) => LoadReceivement();
			cvVLRDEVOL.Click += (s, a) => LoadDevolution();

		}
		private void LoadView()
		{
			txNOMPESS = FindViewById<TextView>(Resource.Id.txNOMPESS);
			txData = FindViewById<TextView>(Resource.Id.txData);
			txtFRMPGTO = FindViewById<TextView>(Resource.Id.txtFRMPGTO);
			txtDTAVCMTO = FindViewById<TextView>(Resource.Id.txtDTAVCMTO);
			txtSTATUS = FindViewById<TextView>(Resource.Id.txtSTATUS);
			txtVLRTOTAL = FindViewById<TextView>(Resource.Id.txtVLRTOTAL);
			txtVLRDEVOL = FindViewById<TextView>(Resource.Id.txtVLRDEVOL);
			txtCOMISSAO = FindViewById<TextView>(Resource.Id.txtCOMISSAO);
			txtVLRREC = FindViewById<TextView>(Resource.Id.txtVLRREC);
			txtSALDO = FindViewById<TextView>(Resource.Id.txtSALDO);
			txtNROPEDIDO = FindViewById<TextInputEditText>(Resource.Id.txtNROPEDIDO);
			relativeLayout1 = FindViewById<RelativeLayout>(Resource.Id.relativeLayout1);
			relativeLayout2 = FindViewById<RelativeLayout>(Resource.Id.relativeLayout2);
			relativeLayout3 = FindViewById<RelativeLayout>(Resource.Id.relativeLayout3);
			cvSALDO = FindViewById<CardView>(Resource.Id.cvSALDO);
			cvVLRREC = FindViewById<CardView>(Resource.Id.cvVLRREC);
			cvVLRDEVOL = FindViewById<CardView>(Resource.Id.cvVLRDEVOL);
		}
		private void adPEDIDOS()
		{
			if (new PedidoController().FindAll().Count == 0)
				this.Msg("NENHUM PEDIDO REGISTRADO!");
			else
			{
				Intent i = new Intent(ApplicationContext, typeof(BuscarPedidoView));
				StartActivityForResult(i, 3);
			}
		}
		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode == 3)
				if (resultCode == Result.Ok)
				{
					string result = data.GetStringExtra("resultPedido");
					if (!string.IsNullOrEmpty(result))
						txtNROPEDIDO.Text = result;
				}
		}
		private void CarregarDados(string NROPEDIDO)
		{
			relativeLayout3.Visibility = ViewStates.Invisible;
			relativeLayout1.Visibility = ViewStates.Visible;
			relativeLayout2.Visibility = ViewStates.Visible;

			Pedido pedido = new PedidoController().FindByNROPEDID(NROPEDIDO.ToLong());
			BaixasPedido baixa = new BaixasPedidoController().FindByNROPEDID(NROPEDIDO.ToLong());

			if(pedido != null)
			{
				Pessoa pessoa = new PessoaController().FindById(pedido.ID_PESSOA.Value);
				if(pessoa != null)
				{
					if (pessoa.NOMFANTA.Length < 25)
						txNOMPESS.Text = pessoa.NOMFANTA;
					else
						txNOMPESS.Text = pessoa.NOMFANTA.Substring(0, 25);
				}
				txData.Text = pedido.DATEMISS.ToString("dd/MM/yyyy");
				txtDTAVCMTO.Text = pedido.DATERET.ToString("dd/MM/yyyy");


				switch (pedido.IDTFRMPG)
				{
					case "1":
						txtFRMPGTO.Text = "A PRAZO";
						break;
					case "0":
						txtFRMPGTO.Text = "A VISTA";
						break;
					case "":
						txtFRMPGTO.Text = "A PRAZO";
						break;
				}

				switch (pedido.SITPEDID)
				{
					case 1:
						txtSTATUS.Text = "Pedido Aberto";
						break;
					case 2:
						txtSTATUS.Text = "Pedido Confirmado";
						break;
					case 3:
						txtSTATUS.Text = "Parcial Total";
						break;
					case 4:
						txtSTATUS.Text = "Pedido Atendido";
						break;
					case 5:
						txtSTATUS.Text = "Pedido Cancelado";
						break;
				}
			}
			if(baixa != null)
			{
				string saldo = new BaixasPedidoController().toReceive(baixa.FT_PEDIDO_ID.Value).ToString("C2");
				string devolucoes = baixa.VLRDEVOL.ToString("C2");
				string recebido = baixa.VLRPGMT.ToString("C2");
				double porcentagemCliente = 0;
				new BaixasPedidoController().OrderBalance(pedido.FT_PEDIDO_ID.Value, out porcentagemCliente);

				//var total = BaixaController.ValorTotal(p.NROPEDID);
				string total = new BaixasPedidoController().ValorTotal(pedido.NROPEDID).ToString("C2");

				txtVLRTOTAL.Text = $"VALOR TOTAL: {total}";
				txtVLRDEVOL.Text = $"TOTAL DEVOLUÇÕES: {devolucoes}";
				txtCOMISSAO.Text = $"VALOR COMISSÃO: {porcentagemCliente.ToString("C2")}";
				txtSALDO.Text = $"SALDO A RECEBER: {saldo}";
				txtVLRREC.Text = $"TOTAL RECEBIDO: {recebido}";
			}
            else
            {
				double porcentagemCliente = 0;
				new BaixasPedidoController().OrderBalance(pedido.FT_PEDIDO_ID.Value, out porcentagemCliente);
				string saldo = new BaixasPedidoController().toReceive(pedido.FT_PEDIDO_ID.Value).ToString("C2");
				string total = new BaixasPedidoController().ValorTotal(pedido.NROPEDID).ToString("C2");

				txtVLRTOTAL.Text = $"VALOR TOTAL: {total}";
				txtVLRDEVOL.Text = "TOTAL DEVOLUÇÕES: R$ 0,00";
				txtCOMISSAO.Text = $"VALOR COMISSÃO: {porcentagemCliente.ToString("C2")}";
				txtSALDO.Text = $"SALDO A RECEBER: {saldo}";
				txtVLRREC.Text = "TOTAL RECEBIDO: R$ 0,00";
			}
		}

		private void LoadNextVist()
		{
			BaixasPedido baixa = new BaixasPedidoController().FindByNROPEDID(txtNROPEDIDO.Text.ToLong());
			if(baixa != null)
			{
				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("PROXIMO RETORNO");
				builder.SetMessage($"RETORNO AGENDADO PARA O DIA {baixa.DATVCTO.ToString("dd/MM/yyyy")}");
				builder.SetNeutralButton("OK", (s, a) => { return; });
				AlertDialog dialog = builder.Create();
				dialog.Show();
			}
		}
		private void LoadReceivement()
		{
			Pedido pedido = new PedidoController().FindByNROPEDID(txtNROPEDIDO.Text.ToLong());
			List<Pagamento> baixas = new BaixasPedidoController().FindAllBaixas().Where(b => b.FT_PEDIDO_ID.Value == pedido.FT_PEDIDO_ID.Value && b.VLRPGMT > 0 && b.VLRPGMT.ToString("C2") != "R$ 0,00").ToList();
			if (baixas.Count > 0)
			{
				ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);

				baixas.ForEach(b =>
				{
					string item = $"        {b.DTHULTAT.ToString("dd/MM/yyyy")} --- RECEBIDO : {b.VLRPGMT.ToString("C2")}";
					adapter.Add(item);
				});


				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("PAGAMENTOS");
				builder.SetAdapter(adapter, (s, a) => { return; });
				builder.SetNeutralButton("OK", (s, a) => { return; });
				AlertDialog dialog = builder.Create();
				dialog.Show();
			}
		}
		private void LoadDevolution()
		{
			Pedido pedido = new PedidoController().FindByNROPEDID(txtNROPEDIDO.Text.ToLong());
			List<DevolucaoItem> devolucoes = new DevolucaoItemController().FindDevolution(pedido.FT_PEDIDO_ID.Value);

			if (devolucoes.Count > 0)
			{
				ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);

				devolucoes.ForEach(d =>
				{
					string item = $"{d.NOMPROD} --- QTD. DEVOLVIDA : {d.QTDDEVOL}";
					adapter.Add($"    {d.NOMPROD.ToUpper()} \n        QTD. DEVOLVIDA : {d.QTDDEVOL} \n\n");
				});


				AlertDialog.Builder builder = new AlertDialog.Builder(this);
				builder.SetTitle("DEVOLUÇÕES");
				builder.SetAdapter(adapter, (s, a) => { return; });
				builder.SetNeutralButton("OK", (s, a) => { return; });
				AlertDialog dialog = builder.Create();
				dialog.Show();
			}
		}
	}
}