using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbContactType
    {
        public GlbContactType()
        {
            GlbContact = new HashSet<GlbContact>();
        }

        public long IdContactType { get; set; }
        public string Description { get; set; }

        public ICollection<GlbContact> GlbContact { get; set; }
    }
}
