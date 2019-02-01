using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbProductClientSubGroupAcc
    {
        public long IdProductClinetSubGroupAcc { get; set; }
        public long IdProduct { get; set; }
        public long IdClient { get; set; }
        public long IdSubGroupAcc { get; set; }
        public long? IdCatalog { get; set; }
        public long? IdCostCenter { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbCostCenter IdCostCenterNavigation { get; set; }
        public GlbProduct IdProductNavigation { get; set; }
        public GlbSubGroupAcc IdSubGroupAccNavigation { get; set; }
    }
}
