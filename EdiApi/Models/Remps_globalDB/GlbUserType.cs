using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUserType
    {
        public GlbUserType()
        {
            GlbUser = new HashSet<GlbUser>();
        }

        public long IdUserType { get; set; }
        public string Type { get; set; }

        public ICollection<GlbUser> GlbUser { get; set; }
    }
}
