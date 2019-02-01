using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCity
    {
        public GlbCity()
        {
            GlbCityTerminal = new HashSet<GlbCityTerminal>();
            GlbEnterprise = new HashSet<GlbEnterprise>();
        }

        public long IdCity { get; set; }
        public long IdCountry { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbCountry IdCountryNavigation { get; set; }
        public ICollection<GlbCityTerminal> GlbCityTerminal { get; set; }
        public ICollection<GlbEnterprise> GlbEnterprise { get; set; }
    }
}
