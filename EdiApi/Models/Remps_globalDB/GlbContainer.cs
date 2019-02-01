using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbContainer
    {
        public long IdContainer { get; set; }
        public string Code { get; set; }
        public string Size { get; set; }
        public string ContainerType { get; set; }
        public string ContainerDesc { get; set; }
        public double? Length { get; set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
