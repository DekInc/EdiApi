using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCompanyTransport
    {
        public long IdCompanyTransport { get; set; }
        public long IdType { get; set; }
        public long IdClient { get; set; }
        public long IdPartnerType { get; set; }
        public string Name { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbType IdTypeNavigation { get; set; }
    }
}
