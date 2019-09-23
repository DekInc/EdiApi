using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DtllPedido
    {
        public int? PedidoId { get; set; }
        public int DtllPedidoId { get; set; }
        public decimal? Cantidad { get; set; }
        public string Observacion { get; set; }
        public string CodProducto { get; set; }
        public string NoDocumento { get; set; }
        public DateTime? FechaDocumento { get; set; }
        public int? DocumentoId { get; set; }
        public decimal? Uxb { get; set; }
        public string NumeroOc { get; set; }
        public string Lote { get; set; }
        public string Estilo { get; set; }
        public string Modelo { get; set; }
        public string CodEquivale { get; set; }
        public DateTime? FechaVcmto { get; set; }

        public Pedido Pedido { get; set; }
    }
}
