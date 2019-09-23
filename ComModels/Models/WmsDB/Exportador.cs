using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Exportador
    {
        public Exportador()
        {
            Transacciones = new HashSet<Transacciones>();
        }

        public int Exportadorid { get; set; }
        public string Nombrexp { get; set; }
        public string Direccion { get; set; }

        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
