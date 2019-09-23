using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Aduana
    {
        public Aduana()
        {
            Transacciones = new HashSet<Transacciones>();
        }

        public string Aduana1 { get; set; }
        public string Descrip { get; set; }
        public bool? Frontera { get; set; }
        public bool? Zolizip { get; set; }
        public string Nodo { get; set; }

        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
