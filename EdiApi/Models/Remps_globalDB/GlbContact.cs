using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbContact
    {
        public long IdContact { get; set; }
        public long IdContactType { get; set; }
        public long IdClient { get; set; }
        public long IdPartnerType { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string SkypeName { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbClient IdClientNavigation { get; set; }
        public GlbContactType IdContactTypeNavigation { get; set; }
    }
}
