using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUserEnterprise
    {
        public long IdUserEnterprise { get; set; }
        public long IdUser { get; set; }
        public long IdEnterprise { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbEnterprise IdEnterpriseNavigation { get; set; }
        public GlbUser IdUserNavigation { get; set; }
    }
}
