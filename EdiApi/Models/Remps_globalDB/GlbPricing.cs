using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPricing
    {
        public GlbPricing()
        {
            GlbPricingDetail = new HashSet<GlbPricingDetail>();
        }

        public long IdPricing { get; set; }
        public long IdClient { get; set; }
        public long IdAgent { get; set; }
        public long IdShipmentType { get; set; }
        public long IdType { get; set; }
        public long IdCurrency { get; set; }
        public long IdServiceLevel { get; set; }
        public long IdIncoterm { get; set; }
        public long IdContainer { get; set; }
        public long IdCompanyTransport { get; set; }
        public long IdCountryOrigin { get; set; }
        public long IdCountryDestination { get; set; }
        public long IdCityOrigin { get; set; }
        public long IdCityDestination { get; set; }
        public long IdTerminalOrigin { get; set; }
        public long IdTerminalDestination { get; set; }
        public int PricingType { get; set; }
        public int TradingType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbType IdTypeNavigation { get; set; }
        public ICollection<GlbPricingDetail> GlbPricingDetail { get; set; }
    }
}
