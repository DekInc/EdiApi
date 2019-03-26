using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PedidosDetExternos
    {
        public int Id { get; set; }
        public int? PedidoId { get; set; }
        public string CodProducto { get; set; }
        public double? CantPedir { get; set; }
    }
}
