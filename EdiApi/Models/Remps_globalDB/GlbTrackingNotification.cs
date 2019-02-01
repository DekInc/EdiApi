using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbTrackingNotification
    {
        public GlbTrackingNotification()
        {
            GlbNotiCenter = new HashSet<GlbNotiCenter>();
        }

        public long IdTrackingNotification { get; set; }
        public long IdNotificationGroup { get; set; }
        public long IdTrackEvent { get; set; }
        public long IdType { get; set; }
        public long IdCliente { get; set; }
        public bool? IsActive { get; set; }
        public long ModificadoPor { get; set; }
        public DateTime Modificado { get; set; }

        public GlbGroupNotification IdNotificationGroupNavigation { get; set; }
        public ICollection<GlbNotiCenter> GlbNotiCenter { get; set; }
    }
}
