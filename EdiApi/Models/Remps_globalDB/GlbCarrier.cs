using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCarrier
    {
        public GlbCarrier()
        {
            GlbSignature = new HashSet<GlbSignature>();
        }

        public long IdCarrier { get; set; }
        public long IdPartnerType { get; set; }
        public long IdType { get; set; }
        public string CarrierAgent { get; set; }

        public GlbPartnerType IdPartnerTypeNavigation { get; set; }
        public GlbType IdTypeNavigation { get; set; }
        public ICollection<GlbSignature> GlbSignature { get; set; }
    }
}
