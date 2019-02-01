using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbTax
    {
        public GlbTax()
        {
            GlbPartnerTax = new HashSet<GlbPartnerTax>();
        }

        public long IdTax { get; set; }
        public long IdSalesCatalog { get; set; }
        public long IdPurchaseCatalog { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public int? Category { get; set; }
        public long IsSales { get; set; }
        public long IsPurchase { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbPartnerTax> GlbPartnerTax { get; set; }
    }
}
