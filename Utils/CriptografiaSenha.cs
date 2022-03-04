using System;
using System.Text;

namespace EloPedidos.Utils
{
    static class CriptografiaSenha
    {
        // Criptografica criada por Eloisio e traanscrita para este software
        public static string CodificarSenha(string senha)
        {
            int CharacterNumber;
            string senhaCod = "";
            int x = 3;
            try
            {
                for (int i = 0; i < senha.Length; i++)
                {
                    char chr = Convert.ToChar(senha.Substring(i, 1));
                    CharacterNumber = (int) chr;
                    CharacterNumber += x;
                    CharacterNumber = (CharacterNumber > 255 ? 255 : CharacterNumber);
                    senhaCod = senhaCod + (char)CharacterNumber;
                    
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Erro na conversão: " + e.Message);
            }
            return senhaCod;
        }
    }
}
