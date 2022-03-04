using System;
using System.Text.RegularExpressions;

namespace EloPedidos.Utils
{
	public class Format
	{
		/// <summary>
		///  Formata o texto preenchendo com espaços em branco até completar a distância
		/// </summary>
		/// <param name="obj">Objeto a ser convertido para string</param>
		/// <param name="space">Distância (tamanho) da string total</param>
		public static string FormatText(object obj, int space)
		{
			string aux = obj.ToString();
			if (aux.Length < space)
				return aux.PadRight(space - aux.Length);
			else
				return aux;
		}

		/// <summary>
		/// IDTPESS 0 = CNPJ
		/// IDTPESS 1 = CPF
		/// </summary>
		/// <param name="IDTDCPES"></param>
		/// <param name="IDTPESS"></param>
		/// <returns></returns>
		public static string MaskCPF_CNPJ(string IDTPESS, string IDTDCPES)
		{
			switch (IDTDCPES)
			{
				case "0":
					return IDTPESS.ToLong().ToString(@"00\.000\.000\/0000\-00");
				case "1":
					return IDTPESS.ToLong().ToString(@"000\.000\.000\-00");
				default:
					return IDTPESS;
			}
		}

		public static string MaskFone(string fone)
		{
			switch (fone.Length)
			{
				case 10:
					return fone.ToLong().ToString(@"\(00\)0000\-0000");
				case 11:
					return fone.ToLong().ToString(@"\(00\)00000\-0000");
				default:
					return fone;
			}
		}

		public static string RemoveAccents(string text)
		{
			if (text.IsEmpty() || text.IsBlank())
				return text;

			var a = "[àáãâä]";
			var A = "[ÀÁÃÂÄ]";
			var c = "[ç]";
			var C = "[Ç]";
			var e = "[èéêë]";
			var E = "[ÈÉÊË]";
			var i = "[ìíîï]";
			var I = "[ÌÍÎÏ]";
			var o = "[òóôõºö]";
			var O = "[ÒÓÔÕÖ]";
			var u = "[ùúûü]";
			var U = "[ÙÚÛÜ]";

			text = Regex.Replace(text, a, "a");
			text = Regex.Replace(text, A, "A");
			text = Regex.Replace(text, c, "c");
			text = Regex.Replace(text, C, "C");
			text = Regex.Replace(text, e, "e");
			text = Regex.Replace(text, E, "E");
			text = Regex.Replace(text, i, "i");
			text = Regex.Replace(text, I, "I");
			text = Regex.Replace(text, o, "o");
			text = Regex.Replace(text, O, "O");
			text = Regex.Replace(text, u, "u");
			text = Regex.Replace(text, U, "U");

			return text;
		}

		public static bool DateToString(string data, out string newDate)
		{
			bool result = true;
			string aux = Utils.RemoveMasks.RemoveMasksToString(data);
			if (aux.Length >= 6)
			{
				if (!string.IsNullOrEmpty(aux))
				{
					string data1 = aux.Substring(0, 2);
					string data2 = aux.Substring(2, 2);
					string data3 = "";
					if (aux.Length == 6)
						data3 = aux.Substring(4, 2);
					else if (aux.Length == 8)
						data3 = aux.Substring(4, 4);
					else
						result = false;

					if (result)
					{
						if (data3.Length == 2)
							data = $"{data1}/{data2}/20{data3}";

						if (data3.Length == 4)
							data = $"{data1}/{data2}/{data3}";
					}
				}

				if (!Utils.Validations.DateValidator(data))
					result = false;
				
			}
			else
				result = false;

			if (result)
				newDate = data;
			else
				newDate = string.Empty;

			return result;
		}
	}
}