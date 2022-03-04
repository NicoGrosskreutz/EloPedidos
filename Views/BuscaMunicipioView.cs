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
using EloPedidos.Services;
using EloPedidos.Utils;

namespace EloPedidos.Views
{
    [Activity(Label = "BuscaMunicipioView")]
    public class BuscaMunicipioView : Activity
    {
        private TextInputEditText txPesquisa;
        private ListView listView;

        ArrayAdapter<string> adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_buscamunicipio);

            txPesquisa = FindViewById<TextInputEditText>(Resource.Id.txPesquisa);
            listView = FindViewById<ListView>(Resource.Id.listView);


            // Text to upper case;
            txPesquisa.SetFilters(new IInputFilter[] { new InputFilterAllCaps() });

            LoadList();

            txPesquisa.TextChanged += (s, a) =>
            {
                if (!string.IsNullOrEmpty(txPesquisa.Text))
                    LoadListByName(txPesquisa.Text);
                else
                    LoadList();
            };
            listView.ItemClick += (s, a) =>
            {
                var item = a.Parent.GetItemAtPosition(a.Position);

                if (item != null)
                    if (!string.IsNullOrEmpty(item.ToString()))
                    {
                        Intent i = new Intent();
                        i.PutExtra("municResult", item.ToString().Split('-')[0].Replace(" ", ""));
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
                var munic = new MunicipioController().FindAll();
                adapter = new ArrayAdapter<string>(ApplicationContext, Resource.Layout.simplelist);
                munic.ToList().ForEach((aux) => adapter.Add($"{Format.FormatText(aux.CODMUNIC, 8)} - {aux.NOMMUNIC}"));
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
				var munic = new MunicipioController().FindByName(name);
				adapter = new ArrayAdapter<string>(Application.Context, Resource.Layout.simplelist);
				munic.ToList().ForEach((aux) => adapter.Add($"{Format.FormatText(aux.CODMUNIC, 8)} - {aux.NOMMUNIC}"));
				listView.Adapter = adapter;
			}
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                this.Msg(ex.ToString());
            }
        }
    }

}