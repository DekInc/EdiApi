using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class PedidosDetExternos
    {
        public int Id { get; set; }
        public int? PedidoId { get; set; }
        public string CodProducto { get; set; }
        public double? CantPedir { get; set; }
        public string Producto { get; set; }
    }
}
