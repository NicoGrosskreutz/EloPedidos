using System;
using SQLite;

namespace EloPedidos.Models
{
    /// <summary>
    ///  Classe responsável por serviços de Geolocalização
    /// </summary>
    [Table("CG_VENDEDOR_LOCALIZACAO")]
    public class Geolocator
    {
        [PrimaryKey, AutoIncrement, Unique]
        public long? Id { get; set; }
        public long? CG_VENDEDOR_ID { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string NOMMUNIC { get; set; }

        public DateTime DTHLOC { get; set; }
        
        /// <summary>
        ///  Indica se a informação foi enviada ao host remoto
        /// </summary>
        public bool INDENV { get; set; }

        [Ignore]
        public Vendedor Vendedor { get; set; }
    }
}
