using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbClientIntegration
    {
        public long IdClientConsignee { get; set; }
        public long IdClient { get; set; }
        public long IdWms { get; set; }
        public long IdSap { get; set; }
        public string Name { get; set; }
        public string Dborigin { get; set; }
        public bool? IsActiveWms { get; set; }
        public bool? IsActiveSap { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbClient IdClientNavigation { get; set; }
    }
}
