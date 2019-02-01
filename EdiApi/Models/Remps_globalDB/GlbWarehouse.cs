using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbWarehouse
    {
        public GlbWarehouse()
        {
            GlbProduct = new HashSet<GlbProduct>();
        }

        public long IdWarehouse { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string Code { get; set; }

        public ICollection<GlbProduct> GlbProduct { get; set; }
    }
}
