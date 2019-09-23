using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
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
