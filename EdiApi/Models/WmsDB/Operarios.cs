using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Operarios
    {
        public Operarios()
        {
            Transacciones = new HashSet<Transacciones>();
        }

        public int Operarioid { get; set; }
        public string Nombre { get; set; }
        public int? Tipoper { get; set; }
        public decimal? HorasDia { get; set; }
        public decimal? SalarioDia { get; set; }
        public int? LocationId { get; set; }

        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
