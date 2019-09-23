using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Destconsigna
    {
        public Destconsigna()
        {
            Pedido = new HashSet<Pedido>();
            Transacciones = new HashSet<Transacciones>();
        }

        public int Destinoid { get; set; }
        public string Nombredest { get; set; }
        public string Direccion { get; set; }
        public int? Paisid { get; set; }

        public ICollection<Pedido> Pedido { get; set; }
        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
