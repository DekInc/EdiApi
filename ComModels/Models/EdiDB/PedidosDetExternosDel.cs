using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class PedidosDetExternosDel
    {
        public int Id { get; set; }
        public int? PedidoId { get; set; }
        public string CodProducto { get; set; }
        public string Producto { get; set; }
        public double? CantPedir { get; set; }
    }
}
