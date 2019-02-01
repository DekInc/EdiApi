using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPartnerAddress
    {
        public long IdPartnerAddress { get; set; }
        public long IdPartner { get; set; }
        public long IdPartnerType { get; set; }
        public long IdAddressType { get; set; }
        public long IdAddress { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbAddress IdAddressNavigation { get; set; }
        public GlbAddressType IdAddressTypeNavigation { get; set; }
        public GlbPartnerType IdPartnerTypeNavigation { get; set; }
    }
}
