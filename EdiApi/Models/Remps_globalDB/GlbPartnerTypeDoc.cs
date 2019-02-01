using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbPartnerTypeDoc
    {
        public long IdPartnerTypeDoc { get; set; }
        public long IdPartnerType { get; set; }
        public long IdDocument { get; set; }
        public bool? Required { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbDocument IdDocumentNavigation { get; set; }
        public GlbPartnerType IdPartnerTypeNavigation { get; set; }
    }
}
