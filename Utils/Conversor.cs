using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EloPedidos.Utils
{
    public static class Conversor
    {
        public static long TryParseLong(string number)
        {
            return long.TryParse(number, out long aux) ? aux : 0;
        }

        public static short TryParseShort(string number)
        {
            return short.TryParse(number, out short aux) ? aux : (short)0;
        }

        public static double TryParseDouble(string number)
        {
            return double.TryParse(number, out double aux) ? aux : 0;
        }

        public static int TryParseInt(string number)
        {
            return int.TryParse(number, out int aux) ? aux : 0;
        }

        public static DateTime? TryParseDate(string date)
        {
            if (DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("pt-br"), DateTimeStyles.None, out DateTime dateR))
                return dateR;
            else
                return null;
        }

        public static byte[] GetUTF8StringBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static byte[] GetASCIIStringBytes(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }

        public static string StringASCIIToUTF8(string strASCII)
        {
            if (string.IsNullOrEmpty(strASCII))
            {
                return string.Empty;
            }
            else
            {
                byte[] bytes = Encoding.ASCII.GetBytes(strASCII);
                return Encoding.UTF8.GetString(bytes);
            }
        }
    }
}