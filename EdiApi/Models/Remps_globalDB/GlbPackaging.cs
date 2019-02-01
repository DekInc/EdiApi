using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPackaging
    {
        public long IdPackaging { get; set; }
        public string Description { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
