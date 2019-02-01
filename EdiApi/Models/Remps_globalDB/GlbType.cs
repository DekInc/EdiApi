using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbType
    {
        public GlbType()
        {
            GlbCarrier = new HashSet<GlbCarrier>();
            GlbCompanyTransport = new HashSet<GlbCompanyTransport>();
            GlbPricing = new HashSet<GlbPricing>();
            GlbShipmentType = new HashSet<GlbShipmentType>();
        }

        public long IdType { get; set; }
        public string Description { get; set; }

        public ICollection<GlbCarrier> GlbCarrier { get; set; }
        public ICollection<GlbCompanyTransport> GlbCompanyTransport { get; set; }
        public ICollection<GlbPricing> GlbPricing { get; set; }
        public ICollection<GlbShipmentType> GlbShipmentType { get; set; }
    }
}
