using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class SysTempSalidas
    {
        public int IdtempSalida { get; set; }
        public int? TransaccionId { get; set; }
        public int? PedidoId { get; set; }
        public int? InventarioId { get; set; }
        public int? DtllPedidoId { get; set; }
        public int? ItemInventarioId { get; set; }
        public string CodProducto { get; set; }
        public double? Cantidad { get; set; }
        public double? Precio { get; set; }
        public DateTime? Fecha { get; set; }
        public string Usuario { get; set; }
        public decimal? Pventa { get; set; }
        public string Lote { get; set; }
        public string Estilo { get; set; }
        public string DocFac { get; set; }
    }
}
