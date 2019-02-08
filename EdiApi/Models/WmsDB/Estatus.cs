using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class Estatus
    {
        public Estatus()
        {
            Bodegas = new HashSet<Bodegas>();
            Clientes = new HashSet<Clientes>();
            Producto = new HashSet<Producto>();
        }

        public int EstatusId { get; set; }
        public string Estatus1 { get; set; }
        public string Descripcion { get; set; }
        public int? Orden { get; set; }
        public bool? UpdateOut { get; set; }

        public ICollection<Bodegas> Bodegas { get; set; }
        public ICollection<Clientes> Clientes { get; set; }
        public ICollection<Producto> Producto { get; set; }
    }
}
