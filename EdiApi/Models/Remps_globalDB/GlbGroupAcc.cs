using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbGroupAcc
    {
        public GlbGroupAcc()
        {
            GlbSubGroupAcc = new HashSet<GlbSubGroupAcc>();
        }

        public long IdGroupAcc { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbSubGroupAcc> GlbSubGroupAcc { get; set; }
    }
}
