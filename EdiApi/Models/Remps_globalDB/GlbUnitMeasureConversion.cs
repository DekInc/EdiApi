using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUnitMeasureConversion
    {
        public long IdUnitMeasureConversion { get; set; }
        public long IdUnitMeasureFrom { get; set; }
        public long IdUnitMeasureTo { get; set; }

        public GlbUnitMeasure IdUnitMeasureFromNavigation { get; set; }
    }
}
