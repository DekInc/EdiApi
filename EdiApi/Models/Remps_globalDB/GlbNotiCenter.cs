using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbNotiCenter
    {
        public long IdNoticenter { get; set; }
        public long IdMediaNotification { get; set; }
        public long IdTrackingNotification { get; set; }
        public bool Sent { get; set; }
        public DateTime SentDate { get; set; }

        public GlbMediaNotification IdMediaNotificationNavigation { get; set; }
        public GlbTrackingNotification IdTrackingNotificationNavigation { get; set; }
    }
}
