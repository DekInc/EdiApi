using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Inventario
    {
        public Inventario()
        {
            ItemInventario = new HashSet<ItemInventario>();
        }

        public int InventarioId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public int? ClienteId { get; set; }
        public string Descripcion { get; set; }
        public string Barcode { get; set; }
        public int? Rack { get; set; }
        public double? Declarado { get; set; }
        public double? Auditado { get; set; }
        public double? Averia { get; set; }
        public double? Existencia { get; set; }
        public double? CantidadInicial { get; set; }
        public double? Valor { get; set; }
        public decimal? Articulos { get; set; }
        public double? Peso { get; set; }
        public double? Volumen { get; set; }
        public int? EstatusId { get; set; }
        public string Observacion { get; set; }
        public bool? IsNacional { get; set; }
        public bool? IsAgranel { get; set; }
        public bool? IsReservado { get; set; }
        public int? TipoBulto { get; set; }
        public int? DetfacId { get; set; }

        public Clientes Cliente { get; set; }
        public Racks RackNavigation { get; set; }
        public ICollection<ItemInventario> ItemInventario { get; set; }
    }
}
