using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbLocation
    {
        public GlbLocation()
        {
            GlbProduct = new HashSet<GlbProduct>();
        }

        public long IdLocation { get; set; }
        public string Description { get; set; }

        public ICollection<GlbProduct> GlbProduct { get; set; }
    }
}
