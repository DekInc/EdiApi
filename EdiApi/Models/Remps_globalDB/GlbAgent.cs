using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbAgent
    {
        public long IdAgent { get; set; }
        public long IdPartnerType { get; set; }
        public string Name { get; set; }

        public GlbPartnerType IdPartnerTypeNavigation { get; set; }
    }
}
