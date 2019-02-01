using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbSubGroupAcc
    {
        public GlbSubGroupAcc()
        {
            GlbProductClientSubGroupAcc = new HashSet<GlbProductClientSubGroupAcc>();
        }

        public long IdSubGroupAcc { get; set; }
        public long IdGroupAcc { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbGroupAcc IdGroupAccNavigation { get; set; }
        public ICollection<GlbProductClientSubGroupAcc> GlbProductClientSubGroupAcc { get; set; }
    }
}
