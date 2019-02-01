using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbAddress
    {
        public GlbAddress()
        {
            GlbPartnerAddress = new HashSet<GlbPartnerAddress>();
        }

        public long IdAddress { get; set; }
        public long IdAddressType { get; set; }
        public long IdRegion { get; set; }
        public long IdCity { get; set; }
        public long IdCountry { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public string ZipOrPostcode { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbPartnerAddress> GlbPartnerAddress { get; set; }
    }
}
