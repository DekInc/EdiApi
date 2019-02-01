using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbSegment
    {
        public GlbSegment()
        {
            GlbProduct = new HashSet<GlbProduct>();
        }

        public long IdSegmento { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public long IdCatalogo1 { get; set; }
        public long IdCatalogo2 { get; set; }
        public long IdCatalogo3 { get; set; }
        public long IdCatalogo4 { get; set; }

        public ICollection<GlbProduct> GlbProduct { get; set; }
    }
}
