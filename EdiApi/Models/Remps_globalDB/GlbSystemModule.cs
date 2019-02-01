using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbSystemModule
    {
        public long IdSystemModule { get; set; }
        public string ModuleName { get; set; }
        public byte[] IntallationKey { get; set; }
        public DateTime InstallationDate { get; set; }
        public string ModuleVersion { get; set; }
    }
}
