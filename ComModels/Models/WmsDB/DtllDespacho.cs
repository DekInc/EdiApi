using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DtllDespacho
    {
        public int? DespachoId { get; set; }
        public int DtllDespachoId { get; set; }
        public int? TransaccionId { get; set; }
        public DateTime? Fecha { get; set; }

        public Despachos Despacho { get; set; }
    }
}
