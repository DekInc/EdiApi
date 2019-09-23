using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DtllItemTransaccion
    {
        public int DtllItemTransaccionId { get; set; }
        public int? TransaccionId { get; set; }
        public int? DtllTransaccionId { get; set; }
        public int? ItemInventarioId { get; set; }
        public int? Cantidad { get; set; }
        public double? Precio { get; set; }
        public int? Rack { get; set; }
        public int? RackFrom { get; set; }
        public double? QtySlInitial { get; set; }
        public double? QtySlFinal { get; set; }

        public DetalleTransacciones DtllTransaccion { get; set; }
        public ItemInventario ItemInventario { get; set; }
        public Racks RackFromNavigation { get; set; }
        public Racks RackNavigation { get; set; }
    }
}
