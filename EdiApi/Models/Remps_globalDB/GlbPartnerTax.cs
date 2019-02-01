using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPartnerTax
    {
        public long IdPartnerTax { get; set; }
        public long IdPartnerType { get; set; }
        public long IdPartner { get; set; }
        public long IdTax { get; set; }
        public double TaxBase { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbPartnerType IdPartnerTypeNavigation { get; set; }
        public GlbTax IdTaxNavigation { get; set; }
    }
}
