using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUnitMeasure
    {
        public GlbUnitMeasure()
        {
            GlbUnitMeasureConversion = new HashSet<GlbUnitMeasureConversion>();
        }

        public long IdUnitMeasure { get; set; }
        public long IdUnit { get; set; }
        public double UnitMeasureQty { get; set; }
        public int ConversionSystem { get; set; }

        public GlbUnit IdUnitNavigation { get; set; }
        public ICollection<GlbUnitMeasureConversion> GlbUnitMeasureConversion { get; set; }
    }
}
