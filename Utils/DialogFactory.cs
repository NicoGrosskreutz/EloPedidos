using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using V7 = Android.Support.V7.App;

namespace EloPedidos.Utils
{
    public class DialogFactory
    {
        public void CreateDialog(Context context, string title, string message, string neutralButtonName, Action neutralAction)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetNeutralButton(neutralButtonName, (s, e) => neutralAction());
            builder.SetCancelable(false);
            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        public void CreateDialog(Context context, string title, string message, string positiveButtonName, Action positiveAction, string negativeButtonName, Action negativeAction)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton(positiveButtonName, (s, e) => positiveAction());
            builder.SetNegativeButton(negativeButtonName, (s, e) => negativeAction());
            builder.SetCancelable(false);
            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        public void CreateDialog(Context context, string title, string message, string positiveButtonName, Action positiveAction, string negativeButtonName, Action negativeAction, string neutralButtonName = null, Action neutralAction = null)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle(title);
            builder.SetMessage(message);
            builder.SetPositiveButton(positiveButtonName, (s, e) => positiveAction());
            builder.SetNegativeButton(negativeButtonName, (s, e) => negativeAction());

            if (neutralButtonName != null && neutralAction != null)
                builder.SetNeutralButton(neutralButtonName, (s, e) => neutralAction());

            builder.SetCancelable(false);
            AlertDialog dialog = builder.Create();
            dialog.Show();
        }

        public void PasswordDialog(Context context, string positiveButtonName, Action positiveAction, string negativeButtonName, Action negativeAction)
        {
            LayoutInflater inflater = LayoutInflater.From(context);
            View view = inflater.Inflate(Resource.Layout.dialog_senha, null);

            EditText passwordText = view.FindViewById<EditText>(Resource.Id.txSenha);

            V7.AlertDialog.Builder builder = new V7.AlertDialog.Builder(context);
            builder.SetView(view);
            builder.SetCancelable(false);
            builder.SetPositiveButton(positiveButtonName, (s, a) => 
            {
                string senha = Convert.ToBase64String(passwordText.Text.ToUTF8());

                if (senha.Equals("outro"))
                    positiveAction();
            });
            builder.SetNegativeButton(negativeButtonName, (s, a) => negativeAction());

            V7.AlertDialog dialog = builder.Create();
            dialog.Show();
        }
    }
}