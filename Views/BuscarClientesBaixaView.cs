using Android.App;
using Android.OS;
using Android.Widget;
using EloPedidos.Controllers;
using EloPedidos.Utils;

namespace EloPedidos.Views
{
    [Activity(Label = "BuscarClientesBaixa")]
    public class BuscarClientesBaixaView : BuscaClienteView
    {
        private new TextView txMUNIC;
        private new CheckBox ckMUNIC;

        private PessoaController pessoaController;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            pessoaController = new PessoaController();

            base.OnCreate(savedInstanceState);

            txMUNIC = FindViewById<TextView>(Resource.Id.txMUNIC);
            ckMUNIC = FindViewById<CheckBox>(Resource.Id.ckMUNIC);

            //label2.Visibility = Android.Views.ViewStates.Invisible;
            //ckSinc.Visibility = Android.Views.ViewStates.Invisible;
        }

        protected override void LoadList()
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.simplelist);
            pessoaController.FindAllWithOrders().ForEach(aux => adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} " +
                $"- {aux.NOMFANTA}"));

            listView.Adapter = adapter;
        }

        protected override void FindByName(string name)
        {
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.simplelist);
            pessoaController.FindWithOrdersByName(name).ForEach(aux => adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} " +
                $"- {aux.NOMFANTA}"));

            listView.Adapter = adapter;
        }
        //protected override void FindByName(string name, bool ckSinc)
        //{
        //    ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Resource.Layout.simplelist);
        //    pessoaController.FindWithOrdersByName(name).ForEach(aux => adapter.Add($"{Format.FormatText(aux.CODPESS, 6)} " +
        //        $"- {aux.NOMFANTA}"));

        //    listView.Adapter = adapter;
        //}
    }
}