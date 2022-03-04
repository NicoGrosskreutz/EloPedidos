using System;
using System.Globalization;
using System.Text;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Util;
using Android.Widget;
using Java.Lang;
using Java.Security;

namespace EloPedidos.Utils
{
    /// <summary>
    ///  Classe de extensões para tipos
    /// </summary>
    public static class Ext
    {
        public static string LOG_APP = "ELO_LOG";

        public static long ToLong(this string value)
        {
            return long.TryParse(value, out long aux) ? aux : 0;
        }

        public static int ToInt(this string value)
        {
            return int.TryParse(value, out int aux) ? aux : 0;
        }

        public static short ToShort(this string value)
        {
            return short.TryParse(value, out short aux) ? aux : (short)0;
        }

        public static double ToDouble(this string value)
        {
            return double.TryParse(value, NumberStyles.AllowDecimalPoint, new CultureInfo("pt-BR"), out double aux) ? aux : 0;
        }

        public static bool ToBool(this string value)
        {
            return bool.TryParse(value, out bool aux) ? aux : false;
        }

        public static bool stringToBoolean(this string value)
        {
            return value == "0" ? false : true;
        }

        /// <summary>
        ///  Retorna uma string formatada com duas casas decimais e vírgula em vez de ponto
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FormatDouble(this double value)
            => value == 0 ? "0,00" : System.Math.Round(value, 2).ToString("F").Replace(".", ",");

        public static bool IsEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsBlank(this string value) => string.IsNullOrWhiteSpace(value);

        /// <summary>
        ///  Converte a string para bytes com codificação UTF8
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToUTF8(this string value, bool RemoveAccents = false)
        {
            return Encoding.UTF8.GetBytes(RemoveAccents ? Format.RemoveAccents(value) : value);
        }

        /// <summary>
        ///  Converte a string para bytes com codificação UTF7
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToUTF7(this string value)
        {
            return Encoding.UTF7.GetBytes(value);
        }

        /// <summary>
        ///  Converte a string para bytes com codificação ASCII
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToASCII(this string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

        /// <summary>
        ///  Converte bytes com codificação UTF8 para string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string UTF8ToString(this byte[] bytes) => Encoding.UTF8.GetString(bytes);

        /// <summary>
        ///  Converte bytes com codificação UTF7 para string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string UTF7ToString(this byte[] bytes) => Encoding.UTF7.GetString(bytes);

        /// <summary>
        ///  Converte bytes com codificação ASCII para string
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ASCIIToString(this byte[] bytes) => Encoding.ASCII.GetString(bytes);

        /// <summary>
        ///  Automátiza habilitar/desbilitar de um EditText
        /// </summary>
        /// <param name="editText"></param>
        /// <param name="enabled"></param>
        /// <param name="inputTypes"></param>
        public static void EnableView(this EditText editText, bool enabled, InputTypes inputTypes = InputTypes.Null)
        {
            editText.Focusable = enabled;
            editText.FocusableInTouchMode = enabled;
            editText.InputType = inputTypes;
        }


        public static DateTime? ToDate(this string date)
        {
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-BR"), DateTimeStyles.None, out DateTime dateR))
                return dateR;
            else
                return null;
        }

        public static bool TestDate(this EditText editText) => editText.Text.ToDate().HasValue;

        public static void Clear(this EditText editText) => editText.Text = string.Empty;

        public static void Msg(this Android.App.Activity activity, string message)
            => activity.RunOnUiThread(
                () => Toast.MakeText(activity, message, ToastLength.Long).Show());

        public static void SavePreference(this Context context, string save, string key)
        {
            ISharedPreferences preferences = context.GetSharedPreferences("Sha_Pref", FileCreationMode.Private);
            ISharedPreferencesEditor editor = preferences.Edit();
            editor.PutString(key, save);
            editor.Apply();
        }
        public static string RestorePreference(this Context context, string key)
        {
            ISharedPreferences preferences = context.GetSharedPreferences("Sha_Pref", FileCreationMode.Private);
            string defaultValue = "";
            return preferences.GetString(key, defaultValue);
        }

        public static string CriptografarSenha(string senha)
        {
            string novaSenha = "";
            try
            {
                //MessageDigest messageDigest = MessageDigest.GetInstance("MD5");
                MessageDigest messageDigest = MessageDigest.GetInstance("SHA");
                messageDigest.Update(senha.ToUTF8());
                byte[] digest = messageDigest.Digest();

                StringBuffer buffer = new StringBuffer();

                for (int i = 0; i < senha.Length; i++)
                {
                    string h = Integer.ToHexString(0xFF & digest[i]);
                    while (h.Length < 2)
                        h = "0" + h;
                    buffer.Append(h);
                }
                novaSenha = buffer.ToString();
            }
            catch (System.Exception e)
            {
                Log.Error("Elo_Log", e.ToString());
                novaSenha = senha;
            }
            return novaSenha;
        }

        public static void SnackMsg(this Android.App.Activity activity, string message)
           => activity.RunOnUiThread(
               () =>
               {
                   Android.Views.View view = activity.FindViewById(Android.Resource.Id.Content);
                   Snackbar.Make(view, message, Snackbar.LengthLong).Show();
               });
    }
}