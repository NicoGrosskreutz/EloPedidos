using System;
namespace EloPedidos.Utils
{
    public static class InsertMasks
    {
        /// <summary>
        ///     Insere uma máscara para telefone no formato (##)####-#### ou (##)#####-####, recebendo uma string de 10 ou 11 dígitos,
        ///   referentes aos formatos atuais de telefone.
        /// </summary>
        /// <returns>Retorna uma string com a máscara de telefone.</returns>
        /// <param name="fone">Fone.</param>
        public static string FoneMask(string fone)
        {
            if (string.IsNullOrEmpty(fone)) return "";

            string aux = "";
            
            if (fone.Length == 10)
            {
                aux += "(";
                aux += fone.Substring(0, 2);
                aux += ")";
                aux += fone.Substring(2, 4);
                aux += "-";
                aux += fone.Substring(6, 4);
            }
            else if (fone.Length == 11)
            {
                aux += "(";
                aux += fone.Substring(0, 2);
                aux += ")";
                aux += fone.Substring(2, 5);
                aux += "-";
                aux += fone.Substring(7, 4);
            }
            else
            {
                Console.WriteLine("Formato incorreto para fone !");
                return fone;
            }

            return aux;
        }

        /// <summary>
        ///     Insere uma máscara para o documento sento este cpf ou cnpj.
        /// </summary>
        /// <returns>Retorna o valor com máscara para o determinado documento.</returns>
        /// <param name="document">Document.</param>
        public static string CpfCnpjMask(string document)
        {
            string aux = "";

            if (document.Length == 11 && 
                !document.Contains(".") && !document.Contains("-"))
            {
                aux += document.Substring(0, 3) + "." + document.Substring(3, 3) + "." 
                    + document.Substring(6, 3) + "-" + document.Substring(9, 2);
            }
            else if (document.Length == 14 && !document.Contains(".") && 
                !document.Contains("-") && !document.Contains("/"))
            {
                aux += document.Substring(0, 2) + "." + document.Substring(2, 3) + "."
                    + document.Substring(5, 3) + "-" + document.Substring(8, 4) + "/" 
                    + document.Substring(12, 2);
            }
            else
            {
                Console.WriteLine("Formato incorreto para documento !");
                return document;
            }

            return aux;
        }

        /// <summary>
        ///     Insere máscara para cep.
        /// </summary>
        /// <returns>The mask.</returns>
        /// <param name="cep">Cep.</param>
        public static string CepMask(string cep)
        {
            string aux = "";

            if (cep.Length == 8)
            {
                aux += cep.Substring(0, 2) + "." + cep.Substring(2, 3) 
                    + "-" + cep.Substring(5, 3);
            }
            else
            {
                Console.WriteLine("Formato incorreto para cep!");
                return cep;
            }

            return aux;
        }

        /// <summary>
        ///  Máscara para inscrição estadual
        /// </summary>
        /// <param name="inscricaoEstadual"></param>
        /// <returns></returns>
        public static string InscEstMask(string IE)
        {
            if (IE.Length == 10 && (!IE.Contains(".") && !IE.Contains("-")))
            {
                return $"{IE.Substring(0, 3)}.{IE.Substring(3, 5)}-{IE.Substring(8, 2)}";
            }
            else
            {
                Console.WriteLine("Formato incorreto para inscrição estadual");
                return IE;
            }
        }
    }
}
