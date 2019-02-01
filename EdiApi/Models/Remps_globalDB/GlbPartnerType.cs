using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPartnerType
    {
        public GlbPartnerType()
        {
            GlbAgent = new HashSet<GlbAgent>();
            GlbCarrier = new HashSet<GlbCarrier>();
            GlbClient = new HashSet<GlbClient>();
            GlbPartnerAddress = new HashSet<GlbPartnerAddress>();
            GlbPartnerTax = new HashSet<GlbPartnerTax>();
            GlbPartnerTypeDoc = new HashSet<GlbPartnerTypeDoc>();
        }

        public long IdPartnerType { get; set; }
        public string Type { get; set; }

        public ICollection<GlbAgent> GlbAgent { get; set; }
        public ICollection<GlbCarrier> GlbCarrier { get; set; }
        public ICollection<GlbClient> GlbClient { get; set; }
        public ICollection<GlbPartnerAddress> GlbPartnerAddress { get; set; }
        public ICollection<GlbPartnerTax> GlbPartnerTax { get; set; }
        public ICollection<GlbPartnerTypeDoc> GlbPartnerTypeDoc { get; set; }
    }
}
