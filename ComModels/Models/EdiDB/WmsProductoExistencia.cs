using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class WmsProductoExistencia
    {
        public int Id { get; set; }
        public int? BodegaId { get; set; }
        public string CodProducto { get; set; }
        public int? Existencia { get; set; }
        public string CodUser { get; set; }
    }
}
