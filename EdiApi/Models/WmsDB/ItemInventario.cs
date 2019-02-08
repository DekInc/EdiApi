using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class ItemInventario
    {
        public ItemInventario()
        {
            DtllItemTransaccion = new HashSet<DtllItemTransaccion>();
        }

        public int? InventarioId { get; set; }
        public int ItemInventarioId { get; set; }
        public string CodProducto { get; set; }
        public double? Declarado { get; set; }
        public double? Auditado { get; set; }
        public double? Averia { get; set; }
        public double? Existencia { get; set; }
        public double? Precio { get; set; }
        public string Descripcion { get; set; }
        public string Observacion { get; set; }
        public DateTime? Fechaitem { get; set; }
        public string ItemInventarioFrom { get; set; }
        public double? CantidadInicial { get; set; }
        public string NumeroOc { get; set; }
        public string Lote { get; set; }
        public DateTime? FechaVcmto { get; set; }
        public string Modelo { get; set; }
        public string CodEquivale { get; set; }
        public string Estilo { get; set; }
        public int? PaisOrig { get; set; }
        public string Color { get; set; }

        public Producto CodProductoNavigation { get; set; }
        public Inventario Inventario { get; set; }
        public ICollection<DtllItemTransaccion> DtllItemTransaccion { get; set; }
    }
}
