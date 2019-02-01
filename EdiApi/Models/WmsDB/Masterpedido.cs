using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Masterpedido
    {
        public int PedidomasterId { get; set; }
        public DateTime? FecMaster { get; set; }
        public DateTime? FecCarga { get; set; }
        public DateTime? EtaPto { get; set; }
        public DateTime? EtaWhs { get; set; }
        public DateTime? EtaZarpe { get; set; }
        public int? Statusped { get; set; }
        public int? Userid { get; set; }
        public DateTime? FecAdi { get; set; }
        public DateTime? FecLlegada { get; set; }
        public int? PedIdOrigen { get; set; }
        public int? CmpIdOrigen { get; set; }
    }
}
