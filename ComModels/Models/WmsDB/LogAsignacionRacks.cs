using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class LogAsignacionRacks
    {
        public int LogId { get; set; }
        public int? Codusr { get; set; }
        public int? InventarioId { get; set; }
        public int? RackId { get; set; }
        public string Barcode { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
