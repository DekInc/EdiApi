using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class PaylessEncuestaRepMm
    {
        public int Id { get; set; }
        public int? Anio { get; set; }
        public int? Mes { get; set; }
        public int? WeekOfYear { get; set; }
        public string FechaI { get; set; }
        public string FechaF { get; set; }
        public int? CantPedidos { get; set; }
        public int? CantEncuestas { get; set; }
        public string CodUser { get; set; }
        public string FechaCreacion { get; set; }
    }
    public partial class PaylessEncuestaRepMmGModel : PaylessEncuestaRepMm {
        public int Recid { get { return Id; } }
    }    
}
