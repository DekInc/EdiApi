using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Embalaje
    {
        public Embalaje()
        {
            DetalleTransacciones = new HashSet<DetalleTransacciones>();
        }

        public string Embalaje1 { get; set; }
        public string Descrip { get; set; }

        public ICollection<DetalleTransacciones> DetalleTransacciones { get; set; }
    }
}
