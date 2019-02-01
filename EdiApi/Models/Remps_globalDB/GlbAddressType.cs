using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbAddressType
    {
        public GlbAddressType()
        {
            GlbPartnerAddress = new HashSet<GlbPartnerAddress>();
        }

        public long IdAddressType { get; set; }
        public string Type { get; set; }

        public ICollection<GlbPartnerAddress> GlbPartnerAddress { get; set; }
    }
}
