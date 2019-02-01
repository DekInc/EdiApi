using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbProduct
    {
        public GlbProduct()
        {
            GlbPricingDetail = new HashSet<GlbPricingDetail>();
            GlbProductClientSubGroupAcc = new HashSet<GlbProductClientSubGroupAcc>();
        }

        public long IdProduct { get; set; }
        public long IdSegment { get; set; }
        public long IdUnit { get; set; }
        public long IdUpdateMethod { get; set; }
        public long IdCatalog { get; set; }
        public long IdWarehouse { get; set; }
        public long IdLocation { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string EnglishDescription { get; set; }
        public bool Service { get; set; }
        public string Comments { get; set; }
        public bool? FixedAsset { get; set; }
        public bool? PurchaseItem { get; set; }
        public bool? SaleItem { get; set; }
        public bool? IventoryItem { get; set; }
        public bool? IsActive { get; set; }
        public bool AllowNotification { get; set; }
        public long ModificadoPor { get; set; }
        public DateTime Modificado { get; set; }

        public GlbLocation IdLocationNavigation { get; set; }
        public GlbSegment IdSegmentNavigation { get; set; }
        public GlbUnit IdUnitNavigation { get; set; }
        public GlbWarehouse IdWarehouseNavigation { get; set; }
        public ICollection<GlbPricingDetail> GlbPricingDetail { get; set; }
        public ICollection<GlbProductClientSubGroupAcc> GlbProductClientSubGroupAcc { get; set; }
    }
}
