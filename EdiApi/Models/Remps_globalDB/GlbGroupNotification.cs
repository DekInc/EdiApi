using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbGroupNotification
    {
        public GlbGroupNotification()
        {
            GlbTrackingNotification = new HashSet<GlbTrackingNotification>();
            GlbUserGroup = new HashSet<GlbUserGroup>();
        }

        public long IdNotificationGroup { get; set; }
        public string GroupName { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbTrackingNotification> GlbTrackingNotification { get; set; }
        public ICollection<GlbUserGroup> GlbUserGroup { get; set; }
    }
}
