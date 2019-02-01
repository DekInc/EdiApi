using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Regimen
    {
        public Regimen()
        {
            BodegaxRegimen = new HashSet<BodegaxRegimen>();
        }

        public int Idregimen { get; set; }
        public string Regimen1 { get; set; }
        public string Descripcion { get; set; }
        public bool? MostrarDetalle { get; set; }

        public ICollection<BodegaxRegimen> BodegaxRegimen { get; set; }
    }
}
