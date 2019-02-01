using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class DtllReceivexItemInventario
    {
        public int DtllReceivexItemInventarioId { get; set; }
        public int? TransaccionId { get; set; }
        public int? DtllReceiveId { get; set; }
        public int? ItemInventarioId { get; set; }
    }
}
