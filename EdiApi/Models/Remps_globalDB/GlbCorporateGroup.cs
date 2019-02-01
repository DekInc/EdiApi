using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCorporateGroup
    {
        public GlbCorporateGroup()
        {
            GlbClient = new HashSet<GlbClient>();
        }

        public long IdCorporateGroup { get; set; }
        public string Name { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbClient> GlbClient { get; set; }
    }
}
