using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCostCenter
    {
        public GlbCostCenter()
        {
            GlbProductClientSubGroupAcc = new HashSet<GlbProductClientSubGroupAcc>();
        }

        public long IdCostCenter { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public bool IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbProductClientSubGroupAcc> GlbProductClientSubGroupAcc { get; set; }
    }
}
