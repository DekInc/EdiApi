using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCountry
    {
        public GlbCountry()
        {
            GlbCity = new HashSet<GlbCity>();
        }

        public long IdCountry { get; set; }
        public long IdRegion { get; set; }
        public int? Code { get; set; }
        public string Name { get; set; }
        public string Abbrv { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbRegion IdRegionNavigation { get; set; }
        public ICollection<GlbCity> GlbCity { get; set; }
    }
}
