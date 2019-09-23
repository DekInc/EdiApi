using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class PaylessRutas
    {
        public int Id { get; set; }
        public int? NumRuta { get; set; }
        public string Horario { get; set; }
        public int? ClienteId { get; set; }
        public bool? CambioHorario { get; set; }
    }
}
