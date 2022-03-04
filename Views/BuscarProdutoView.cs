using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Services;
using EloPedidos.Utils;

namespace EloPedidos.Views
{
    [Activity(Label = "EloPedidos")]
    public class BuscarProdutoView : Activity
    {
        private TextInputEditText txPesquisa;
        private TextView txtROMAN;
        private ListView listView;
        private CheckBox ckROMAN;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_buscarproduto);

            txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
            listView = FindViewById<ListView>(Resource.Id.listView);
            ckROMAN = FindViewById<CheckBox>(Resource.Id.ckROMAN);
            txtROMAN = FindViewById<TextView>(Resource.Id.txtROMAN);

            txPesquisa.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });

            //LoadList();
            LoadListEstoque("");
            txPesquisa.RequestFocus();

            ckROMAN.Checked = true;

            ckROMAN.CheckedChange += (sender, Args) =>
            {
				if (ckROMAN.Checked == true)
				{
                    if (txPesquisa.Text != "")
                        LoadListEstoque(txPesquisa.Text);
                    else
                        LoadListEstoque("");
                }
				else
				{
                    if (txPesquisa.Text != "")
                        LoadListByName(txPesquisa.Text);
                    else
                        LoadList();


                }
                    
            };
            txtROMAN.Click += (sender, Args) =>
            {
                if (ckROMAN.Checked == true)
                    ckROMAN.Checked = false;
                else
                    ckROMAN.Checked = true;
            };

            txPesquisa.TextChanged += (s, a) => 
            {
                if (ckROMAN.Checked == false)
                {
                    if (!string.IsNullOrEmpty(txPesquisa.Text))
                        LoadListByName(txPesquisa.Text);
                    else
                        LoadList();
                }
				else
				{
                    if (!string.IsNullOrEmpty(txPesquisa.Text))
                        LoadListEstoque(txPesquisa.Text);
                    else
                        LoadListEstoque("");
                }
            };
            listView.ItemClick += (s, a) => 
            {
                var item = a.Parent.GetItemAtPosition(a.Position);

                if (item != null)
                    if (!string.IsNullOrEmpty(item.ToString()))
                    {
                        Intent i = new Intent();
                        i.PutExtra("resultProd", item.ToString().Split('-')[0].Replace(" ", ""));
                        SetResult(Result.Ok, i);
                        Finish();
                    }
            };
        }

        public override void OnBackPressed()
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void LoadList()
        {
            try
            {
                var prod = new ProdutoController().FindAll();
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);
                prod.ToList().ForEach((aux) => adapter.Add($"{Format.FormatText(aux.CODPROD, 6)} - {aux.DSCPROD} - {aux.IDTUNID}"));
                listView.Adapter = adapter;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                this.Msg(ex.ToString());
            }
        }

        private void LoadListByName(string name)
        {
            try
            {
                var prod = new ProdutoController().FindByDSCPROD(name);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);
                prod.ToList().ForEach((aux) =>
                
                adapter.Add($"{Format.FormatText(aux.CODPROD, 6)} - {aux.DSCPROD} - {aux.IDTUNID}"));



                listView.Adapter = adapter;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                this.Msg(ex.ToString());
            }
        }
        private void LoadListEstoque(string dscrPROD)
		{
			try
			{
                List<RomaneioItem> prod;
                if (dscrPROD == "")
                    prod = new RomaneioController().FindAll();
                else
                {
                    var conn = Database.GetConnection();
                    prod = conn.Table<RomaneioItem>().ToList().Where(p => p.DSCRPROD.ToLower().Contains(dscrPROD.ToLower())).ToList();

                    //prod = new EstoqueController().FindBy_NOMPROD(dscrPROD);
                }
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);
                prod.ToList().ForEach((aux) =>
                {
                    if (aux.CG_PRODUTO_ID != 0)
                    {
                        double QTDSALDO = (aux.QTDPROD + aux.QTDDEVCL - aux.QTDVENDA - aux.QTDBRINDE);

                        var prod = new ProdutoController().FindById(aux.CG_PRODUTO_ID);
                        if(prod != null)
                            adapter.Add($"{Format.FormatText(prod.CODPROD.ToString(), 6)} - {aux.DSCRPROD} - {prod.IDTUNID} - SALDO: {QTDSALDO}");
                    }
                });
                listView.Adapter = adapter;
			}
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                this.Msg(ex.ToString());
            }
        }

        //protected override void OnDestroy()
        //{
        //    base.OnDestroy();
        //    SendBroadcast(new Intent(this, typeof(GeolocatorBroadCast)));
        //}
    }
}