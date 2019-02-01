using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCurrency
    {
        public GlbCurrency()
        {
            GlbEnterprise = new HashSet<GlbEnterprise>();
        }

        public long IdCurrency { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string LstOpe { get; set; }
        public int? SerSta { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbEnterprise> GlbEnterprise { get; set; }
    }
}
