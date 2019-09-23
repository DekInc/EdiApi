using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Paises
    {
        public Paises()
        {
            Transacciones = new HashSet<Transacciones>();
        }

        public int Paisid { get; set; }
        public string Nompais { get; set; }

        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
