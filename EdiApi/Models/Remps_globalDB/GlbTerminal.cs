using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbTerminal
    {
        public GlbTerminal()
        {
            GlbCityTerminal = new HashSet<GlbCityTerminal>();
        }

        public long IdTerminal { get; set; }
        public long IdCountry { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbCityTerminal> GlbCityTerminal { get; set; }
    }
}
