using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCityTerminal
    {
        public long IdCityTerminal { get; set; }
        public long IdCity { get; set; }
        public long IdTerminal { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbCity IdCityNavigation { get; set; }
        public GlbTerminal IdTerminalNavigation { get; set; }
    }
}
