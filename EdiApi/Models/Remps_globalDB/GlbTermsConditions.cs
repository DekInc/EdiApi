using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbTermsConditions
    {
        public long IdTermsConditions { get; set; }
        public long? IdShipmentType { get; set; }
        public string Description { get; set; }
        public bool? Applicable { get; set; }
        public double Amount { get; set; }
        public int TradingType { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbShipmentType IdShipmentTypeNavigation { get; set; }
    }
}
