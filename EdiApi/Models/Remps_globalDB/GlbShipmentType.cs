using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbShipmentType
    {
        public GlbShipmentType()
        {
            GlbTermsConditions = new HashSet<GlbTermsConditions>();
        }

        public long IdShipmentType { get; set; }
        public string Description { get; set; }
        public long IdType { get; set; }

        public GlbType IdTypeNavigation { get; set; }
        public ICollection<GlbTermsConditions> GlbTermsConditions { get; set; }
    }
}
