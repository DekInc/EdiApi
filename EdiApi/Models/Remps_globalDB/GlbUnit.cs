using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUnit
    {
        public GlbUnit()
        {
            GlbProduct = new HashSet<GlbProduct>();
            GlbUnitMeasure = new HashSet<GlbUnitMeasure>();
        }

        public long IdUnit { get; set; }
        public string Units { get; set; }
        public string Description { get; set; }

        public ICollection<GlbProduct> GlbProduct { get; set; }
        public ICollection<GlbUnitMeasure> GlbUnitMeasure { get; set; }
    }
}
