using SQLite;
using System;

namespace EloPedidos.Models
{
    [Table("MUNICIPIO_RETURN")]
    public class MunicipioReturn
    {
        public long? CODMUNIC { get; set; }
        public string CODUF { get; set; }
        public string NROLATI { get; set; }
        public string NROLONG { get; set; }
        public string NOMMUNIC { get; set; }
    }
}