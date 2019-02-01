using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbSignature
    {
        public long IdSignature { get; set; }
        public long IdPartner { get; set; }
        public byte[] Signature { get; set; }

        public GlbCarrier IdPartnerNavigation { get; set; }
    }
}
