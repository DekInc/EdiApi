using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbRange
    {
        public GlbRange()
        {
            GlbPricingDetailProductProperty = new HashSet<GlbPricingDetailProductProperty>();
        }

        public long IdRange { get; set; }
        public double RangeMinValue { get; set; }
        public double RangeMaxValue { get; set; }
        public double Price { get; set; }
        public int ValueType { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbPricingDetailProductProperty> GlbPricingDetailProductProperty { get; set; }
    }
}
