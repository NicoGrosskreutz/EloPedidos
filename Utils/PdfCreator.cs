using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Pdf;
using Android.OS;
using Android.Print;
using Android.Print.Pdf;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Jsoup.Nodes;
using Java.IO;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Android.Graphics.Pdf.PdfDocument;
using static Android.Print.PrintAttributes;

namespace EloPedidos.Utils
{
	public class PdfCreator : Activity
	{
		Pedido Pedido;
		List<ItemPedido> itens;
		Empresa empresa;
		Pessoa cliente;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_pedido);
		}
		public void CreatePDF(Pedido _pedido, List<ItemPedido> _itens, Empresa _empresa, Pessoa _cliente)
		{
			this.Pedido = _pedido;
			this.itens = _itens;
			this.empresa = _empresa;
			this.cliente = _cliente;

			FileOutputStream fileOut = null;
			FileStream os = null;
			iText.Layout.Document document = null;

			Java.IO.File pdfDirPath = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "PDFs");
			pdfDirPath.Mkdir();
			Java.IO.File file = new Java.IO.File(pdfDirPath, $"Pedido{Pedido.NROPEDID}.pdf");


			try
			{
				fileOut = new FileOutputStream(file);
				os = new FileStream(file.ToString(), FileMode.OpenOrCreate);

				PdfWriter pdfWriter = new PdfWriter(os);
				iText.Kernel.Pdf.PdfDocument pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfWriter);

				document = new iText.Layout.Document(pdfDocument);

				//PrinterController pController = new PrinterController();
				//document.Add(new Paragraph(pController.FormatOrderForPrintA7(_pedido)));

				SetText(document);
			}
			catch (System.Exception e)
			{
				Log.Error(Utils.Ext.LOG_APP, e.Message);
			}
			finally
			{
				document.Close();
			}
		}

		public void SetText(iText.Layout.Document document)
		{
			try
			{
				Municipio municipio = new MunicipioController().FindById(empresa.CODMUNIC);
				Municipio municipioCliente = new MunicipioController().FindById(cliente.CODMUNIC);
				Vendedor vendedor = new VendedorController().GetVendedor();

				document.Add(new Paragraph($"PEDIDO: {this.Pedido.NROPEDID}                    EMISSÃO: {Pedido.DTHULTAT.ToString("dd/MM/yyyy")}"));

				document.Add(new Paragraph("============================= EMPRESA ============================="));
				document.Add(new Paragraph($"{empresa.NOMFANTA}"));
				document.Add(new Paragraph($"RUA: {empresa.DSCENDER}, {empresa.NROENDER}"));
				document.Add(new Paragraph($"{empresa.NOMBAIRR}, {municipio.NOMMUNIC}/ {municipio.CODUF}"));
				document.Add(new Paragraph($"{Utils.Format.MaskFone(empresa.NROFONE)}"));
				document.Add(new Paragraph($"VENDEDOR: {vendedor.NOMVEND}"));
				document.Add(new Paragraph($"TELEFONE: {Utils.Format.MaskFone(vendedor.NROTLFN)}"));

				document.Add(new Paragraph("============================= CLIENTE ============================="));
				document.Add(new Paragraph($"{cliente.NOMPESS.ToUpper()}"));
				document.Add(new Paragraph($"DOCUMENTO: {Utils.Format.MaskCPF_CNPJ(cliente.IDTDCPES.ToString(), cliente.IDTPESS)}"));
				document.Add(new Paragraph($"RUA: {cliente.DSCENDER.ToUpper()}, {cliente.NROENDER.ToUpper()}"));
				document.Add(new Paragraph($"{cliente.NOMBAIRR.ToUpper()}, {municipioCliente.NOMMUNIC}/ {municipioCliente.CODUF}"));

				document.Add(new Paragraph("============================= PRODUTOS ==========================="));
				itens.Where(i => !i.INDBRIND).ToList().ForEach(i =>
				{
					Produto produto = new ProdutoController().FindById(i.CG_PRODUTO_ID.Value);

					string nomeProduto = produto.DSCPROD;

					if (nomeProduto.Length > 14)
						nomeProduto = nomeProduto.Substring(0, 14);

					document.Add(new Paragraph($"{nomeProduto} ( {produto.IDTUNID} ) QTD: {i.QTDPROD.ToString()} - VLR UNIT.: {i.VLRUNIT.ToString("C2")} - VLR TOTAL: {(i.VLRUNIT * i.QTDPROD).ToString("C2")}"));
				});
				document.Add(new Paragraph($"TOTAL: {new PedidoController().GetTotalValue(Pedido.NROPEDID).ToString("C2")}"));

				document.Add(new Paragraph($"OBSERVAÇÕES:"));
				document.Add(new Paragraph($"{Pedido.DSCOBSER}"));
				document.Add(new Paragraph($"DATA DO RETORNO: {Pedido.DATERET.ToString("dd/MM/yyyy")}"));
				document.Add(new Paragraph("================================================================="));

				double toReceive = new PedidoController().totalReceberCliente(Pedido.CG_PESSOA_ID.Value, out string[] _Pedidos);
				if (toReceive > 0)
				{
					document.Add(new Paragraph($"SALDO DEVEDOR: {toReceive.ToString("C2")}"));
					_Pedidos.ToList().ForEach(p =>
					{
						Pedido ped = new PedidoController().FindByNROPEDID(p.ToLong());
						if (ped != null)
						{
							double valor = new BaixasPedidoController().toReceive(ped.FT_PEDIDO_ID.Value);
							if (valor > 0)
								document.Add(new Paragraph($"Pedido: {p} - {valor.ToString("C2")}"));
						}
					});
				}

				document.Add(new Paragraph(""));
				document.Add(new Paragraph("DESENVOLVIDO POR ELO.SOFTWARE"));
			}
			catch (System.Exception e)
			{
				document.Add(new Paragraph(e.Message));
			}
		}
	}
}