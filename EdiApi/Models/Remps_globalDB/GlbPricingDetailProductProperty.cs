using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPricingDetailProductProperty
    {
        public long IdPricingDetailProductProperty { get; set; }
        public long IdPricingDetail { get; set; }
        public long IdRange { get; set; }
        public long IdProduct { get; set; }
        public bool RangeNeeded { get; set; }
        public double Price { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbPricingDetail IdPricingDetailNavigation { get; set; }
        public GlbRange IdRangeNavigation { get; set; }
    }
}
