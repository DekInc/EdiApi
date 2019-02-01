using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbMediaNotification
    {
        public GlbMediaNotification()
        {
            GlbNotiCenter = new HashSet<GlbNotiCenter>();
        }

        public long IdMediaNotification { get; set; }
        public string MediaName { get; set; }
        public string MediaConfiguration { get; set; }

        public ICollection<GlbNotiCenter> GlbNotiCenter { get; set; }
    }
}
