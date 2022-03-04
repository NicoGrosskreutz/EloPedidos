using System;
using System.Text.RegularExpressions;

namespace EloPedidos.Utils
{
    public static class RemoveMasks
    {
        /// <summary>
        ///     Método que retorna uma string sem máscara, pode ser cpf, cnpj, cep ou fone
        /// </summary>
        /// <returns>The masks.</returns>
        /// <param name="maskedText">Masked text.</param>
        public static string RemoveMasksToString(string maskedText)
        {
            return new Regex("[^0-9a-zA-Z]").Replace(maskedText, "");
        }

        /// <summary>
        ///     Remove a máscara da string e tenta converte-lá para long, caso possível retorna o valor, senão retorna '0'.
        /// </summary>
        /// <returns>The masks to long.</returns>
        /// <param name="maskedText">Masked text.</param>
        public static long RemoveMasksToLong(string maskedText)
        {
            try
            {
                return long.Parse(RemoveMasksToString(maskedText));
            }
            catch
            {
                Console.WriteLine("Erro na conversão de valores!");
                return 0;
            }
        }
    }
}
