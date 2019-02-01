using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPricingDetail
    {
        public GlbPricingDetail()
        {
            GlbPricingDetailProductProperty = new HashSet<GlbPricingDetailProductProperty>();
        }

        public long IdPricingDetail { get; set; }
        public long IdPricing { get; set; }
        public long IdProduct { get; set; }
        public long IdUnitMeasure { get; set; }
        public double Price { get; set; }
        public int CostType { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbPricing IdPricingNavigation { get; set; }
        public GlbProduct IdProductNavigation { get; set; }
        public ICollection<GlbPricingDetailProductProperty> GlbPricingDetailProductProperty { get; set; }
    }
}
