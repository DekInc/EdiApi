using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Transportista
    {
        public Transportista()
        {
            Despachos = new HashSet<Despachos>();
            Transacciones = new HashSet<Transacciones>();
        }

        public int Transportistaid { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string CodTransp { get; set; }

        public ICollection<Despachos> Despachos { get; set; }
        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
