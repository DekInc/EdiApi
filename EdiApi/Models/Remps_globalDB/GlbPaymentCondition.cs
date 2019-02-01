using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPaymentCondition
    {
        public GlbPaymentCondition()
        {
            GlbClient = new HashSet<GlbClient>();
        }

        public long IdPaymentCondition { get; set; }
        public string Condition { get; set; }
        public int CreditDays { get; set; }

        public ICollection<GlbClient> GlbClient { get; set; }
    }
}
