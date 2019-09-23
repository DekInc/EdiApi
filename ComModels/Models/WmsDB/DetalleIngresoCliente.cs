using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DetalleIngresoCliente
    {
        public int DtllReceiveId { get; set; }
        public int? TransaccionId { get; set; }
        public string CodProducto { get; set; }
        public string CodProductoCliente { get; set; }
        public string DescCliente { get; set; }
        public string Estilo { get; set; }
        public string Color { get; set; }
        public string Talla { get; set; }
        public double? Cantidad { get; set; }
        public double? Valor { get; set; }
        public double? Peso { get; set; }
        public double? Cbm { get; set; }
        public int? UndMedida { get; set; }
        public int? Bultos { get; set; }
        public int? TipoBulto { get; set; }
        public string Rack { get; set; }
    }
}
