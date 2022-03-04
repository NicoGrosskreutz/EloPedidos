using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace EloPedidos.Models
{
    [Table("TB_EMPRESA")]
    public class Empresa
    {
        /// <summary>
        /// Código empresa
        /// </summary>
        [PrimaryKey, Column("CODEMPRE"), MaxLength(2)]
        public string CODEMPRE { get; set; }

        /// <summary>
        /// Razão social
        /// </summary>
        public string NOMRZSOC { get; set; }

        /// <summary>
        /// Nome fantasia
        /// </summary>
        public string NOMFANTA { get; set; }

        /// <summary>
        /// Número telefone
        /// </summary>
        public string NROFONE { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        public string DSCENDER { get; set; }

        /// <summary>
        /// Número endereço
        /// </summary>
        public int NROENDER { get; set; }

        /// <summary>
        /// Complemento endereço
        /// </summary>
        public string CPLENDER { get; set; }

        /// <summary>
        /// Bairro
        /// </summary>
        public string NOMBAIRR { get; set; }
        
        /// <summary>
        ///  Codigo municipio (id)
        /// </summary>
        public long CODMUNIC { get; set; }

        /// <summary>
        ///  Id do vendedor
        /// </summary>
        public long CG_VENDEDOR_ID { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        public long NROCEP { get; set; }

        /// <summary>
        /// CNPJ
        /// </summary>
        public string NROCNPJ { get; set; }

        /// <summary>
        /// Inscrição estadual
        /// </summary>
        public string NROINEST { get; set; }

        /// <summary>
        /// Data/hora atualização
        /// </summary>
        public string DTHULTAT { get; set; }

        /// <summary>
        /// Usuário que realizou última atualização
        /// </summary>
        public string USRULTAT { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string DSCEMAIL { get; set; }

        /// <summary>
        /// Percentual de comissão
        /// </summary>
        public double PERCOMIS { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        public string SNHEMAIL { get; set;}

        /// <summary>
        /// SMTP
        /// </summary>
        public string NOMSSMTP { get; set; }

        /// <summary>
        ///  Porta SMTP
        /// </summary>
        public int NROPORTA { get; set; }

        /// <summary>
        /// Autenticação
        /// </summary>
        public bool INDAUTSV { get; set; }

        /// <summary>
        /// SSL
        /// </summary>
        public bool INDSSLSV { get; set; }

        /// <summary>
        /// TLS
        /// </summary>
        public bool INDTLSSV { get; set; }
        
        /// <summary>
        /// Email principal
        /// </summary>
        public string EMLPRINC { get; set; }

        /// <summary>
        /// A partir de qual Valor Venda aplica a segunda comissão
        /// </summary>
        public double VLR2COM { get; set; }

        /// <summary>
        /// Percentual segunda comissão
        /// </summary>
        public double PER2COM { get; set; }

        /// <summary>
        /// Vendedor
        /// </summary>
        [Ignore]
        public Vendedor Vendedor { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        [Ignore]
        public Municipio Municipio { get; set; }
    }
}