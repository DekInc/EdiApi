using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbRegion
    {
        public GlbRegion()
        {
            GlbCountry = new HashSet<GlbCountry>();
        }

        public long IdRegion { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }

        public ICollection<GlbCountry> GlbCountry { get; set; }
    }
}
