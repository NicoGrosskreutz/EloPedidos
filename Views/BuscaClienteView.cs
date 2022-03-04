using System;
using System.Linq;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Persistence;
using EloPedidos.Services;
using EloPedidos.Utils;
using M = EloPedidos.Models;
using System.Collections.Generic;
using Android.Support.Design.Widget;
using EloPedidos.Models;
using EloPedidos.Adapter;

namespace EloPedidos.Views
{
    [Activity(Label = "BuscaClienteView")]
    public class BuscaClienteView : Activity
    {
        protected ListView listView;
        protected TextInputEditText txPesquisa;
        protected CheckBox ckMUNIC, ckINDINAT;
        protected TextView txMUNIC;
        protected TextView txCLIENTE, txINDINAT;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_buscacliente);

            listView = FindViewById<ListView>(Resource.Id.listView);
            txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
            ckMUNIC = FindViewById<CheckBox>(Resource.Id.ckMUNIC);
            txMUNIC = FindViewById<TextView>(Resource.Id.txMUNIC);
            txCLIENTE = FindViewById<TextView>(Resource.Id.txCLIENTE);
            ckINDINAT = FindViewById<CheckBox>(Resource.Id.ckINDINAT);
            txINDINAT = FindViewById<TextView>(Resource.Id.txINDINAT);

            NextFocus(txPesquisa);

            txPesquisa.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });

            Municipio munic = null;
            M.Config config = new ConfigController().Config;
            if (!string.IsNullOrEmpty(config.NOMMUNPQ))
            {
                txMUNIC.Text = config.NOMMUNPQ.ToString();
                munic = new MunicipioController().FindById(new ConfigController().GetConfig().CODMUNPQ);
                ckMUNIC.Checked = true;
            }
            else
            {
                ckMUNIC.Checked = false;
                ckMUNIC.Visibility = ViewStates.Invisible;
                txMUNIC.Text = "";
            }
            ckINDINAT.Checked = false;

            List<Pessoa> clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
            if (ckMUNIC.Checked)
                clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
            else
                clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

            var adapter = new AdapterBuscarCliente(this, clientes);
            listView.Adapter = adapter;

            txPesquisa.TextChanged += (s, a) =>
            {
                if (ckMUNIC.Checked)
                    clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
                else
                    clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

                var adapter = new AdapterBuscarCliente(this, clientes);
                listView.Adapter = adapter;
            };


            listView.ItemClick += (s, a) =>
            {
                var result = a.Parent.GetItemAtPosition(a.Position);

                var adapter = (AdapterBuscarCliente)listView.Adapter;
                var cliente = adapter[a.Position];

                Intent i = new Intent();
                i.PutExtra("result", cliente.ID.ToString());
                SetResult(Result.Ok, i);
                Finish();
            };
            ckMUNIC.CheckedChange += (s, a) =>
            {
                if (ckMUNIC.Checked)
                    clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
                else
                    clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

                var adapter = new AdapterBuscarCliente(this, clientes);
                listView.Adapter = adapter;
            };

            ckINDINAT.CheckedChange += (s, a) =>
            {
                if (ckMUNIC.Checked)
                    clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, munic);
                else
                    clientes = BuscarClientes(txPesquisa.Text, ckINDINAT.Checked, null);

                var adapter = new AdapterBuscarCliente(this, clientes);
                listView.Adapter = adapter;
            };

            txMUNIC.Click += (sender, args) =>
            {
                if (ckMUNIC.Checked == false)
                    ckMUNIC.Checked = true;
                else
                    ckMUNIC.Checked = false;
            };

            txINDINAT.Click += (sender, args) =>
            {
                if (ckINDINAT.Checked == false)
                    ckINDINAT.Checked = true;
                else
                    ckINDINAT.Checked = false;
            };
        }
        protected void NextFocus(View v)
        {
            v.RequestFocus();
        }
        protected void GetError(string message)
        {
            string error = "";
            Log.Error(error, message);
            this.Msg(message);
        }
        public override void OnBackPressed()
        {
            SetResult(Result.Canceled);
            Finish();
        }

        /// <summary>
        ///  Carrega os dados para o list view
        /// </summary>
        protected virtual void LoadList()
        {
            try
            {
                var pessoas = new PessoaDAO().FindAll();
                if (pessoas.Count > 0)
                {
                    MunicipioController mCOntroller = new MunicipioController();
                    ArrayAdapter<string> adapter = new ArrayAdapter<string>(Application.Context, Resource.Layout.simplelist);
                    pessoas.ToList().ForEach((aux) =>
                    {
                        if (mCOntroller.FindById(aux.CODMUNIC) != null)
                        {
                            adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} - {aux.NOMPESS} - {Format.FormatText(aux.IDTPESS, 6)} - {mCOntroller.FindById(aux.CODMUNIC).NOMMUNIC}");

                        }
                        else
                        {
                            adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} - {aux.NOMPESS} - {Format.FormatText(aux.IDTPESS, 6)}");

                        }
                    });
                    listView.Adapter = adapter;
                }
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
            }
        }


        protected virtual void FindByName(string name)
        {
            try
            {
                var pesquisa = new PessoaDAO().FindByName(name);
                MunicipioController mCOntroller = new MunicipioController();
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(Application.Context, Resource.Layout.simplelist);
                pesquisa.ToList().ForEach((aux) =>
                {
                    if (mCOntroller.FindById(aux.CODMUNIC) != null)
                    {
                        adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} - {aux.NOMPESS} - {Format.FormatText(aux.IDTPESS, 6)} - {mCOntroller.FindById(aux.CODMUNIC).NOMMUNIC}");

                    }
                    else
                    {
                        adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} - {aux.NOMPESS} - {Format.FormatText(aux.IDTPESS, 6)}");

                    }
                });
                listView.Adapter = adapter;
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                RunOnUiThread(()
                    => Toast.MakeText(ApplicationContext, ex.ToString(), ToastLength.Long).Show());
            }
        }

        private List<Pessoa> BuscarClientes(string nome, bool indinat, Municipio municipio = null)
        {
            List<Pessoa> pessoas = new List<Pessoa>();
            PessoaController pController = new PessoaController();
            pessoas = pController.FindAll();
            try
            {
                if (!string.IsNullOrEmpty(nome))
                    pessoas = pController.FindByName(nome);
                if (municipio != null)
                    pessoas = pessoas.Where(p => p.CODMUNIC == municipio.CODMUNIC).ToList();

                if (indinat)
                    pessoas = pessoas.Where(p => p.INDINAT).ToList();

                return pessoas;
            }
            catch (Exception e)
            {
                return pessoas;
            }
        }
    }
}