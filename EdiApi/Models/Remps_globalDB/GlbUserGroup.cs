using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUserGroup
    {
        public long IdUserGroup { get; set; }
        public long IdUsuario { get; set; }
        public long IdNotificationGroup { get; set; }
        public bool? IsActive { get; set; }
        public long ModificadoPor { get; set; }
        public DateTime Modificado { get; set; }

        public GlbGroupNotification IdNotificationGroupNavigation { get; set; }
        public GlbUser IdUsuarioNavigation { get; set; }
    }
}
