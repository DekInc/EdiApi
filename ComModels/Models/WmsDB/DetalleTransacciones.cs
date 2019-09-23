using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DetalleTransacciones
    {
        public DetalleTransacciones()
        {
            DtllItemTransaccion = new HashSet<DtllItemTransaccion>();
        }

        public int TransaccionId { get; set; }
        public int DtllTrnsaccionId { get; set; }
        public int? InventarioId { get; set; }
        public int? Rack { get; set; }
        public int? RackFrom { get; set; }
        public int? DtllPedidoId { get; set; }
        public decimal? Conteo { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? Fechaitem { get; set; }
        public bool? IsEscaneado { get; set; }
        public string Embalaje { get; set; }

        public Embalaje EmbalajeNavigation { get; set; }
        public Racks RackFromNavigation { get; set; }
        public Racks RackNavigation { get; set; }
        public Transacciones Transaccion { get; set; }
        public ICollection<DtllItemTransaccion> DtllItemTransaccion { get; set; }
    }
}
