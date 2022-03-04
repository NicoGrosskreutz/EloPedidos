using SQLite;
using System;

namespace EloPedidos.Models
{
    [Table("CG_MUNICIPIO")]
    public class Municipio
    {
        [PrimaryKey, Column("CODMUNIC")]
        public long? CODMUNIC { get; set; } = null;
        public string NOMMUNIC { get; set; }
        public string CODUF { get; set; }
        public long CODMUNGV { get; set; }
        public long NROCEP { get; set; }
        public double VLRLAT { get; set; }
        public double VLRLONG { get; set; }
        public DateTime DTHULTAT { get; set; }
        public string USRULTAT { get; set; }
    }
}