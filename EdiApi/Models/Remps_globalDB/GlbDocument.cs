using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbDocument
    {
        public GlbDocument()
        {
            GlbPartnerTypeDoc = new HashSet<GlbPartnerTypeDoc>();
            GlbSharedDocument = new HashSet<GlbSharedDocument>();
        }

        public long IdDocument { get; set; }
        public long IdDocumentType { get; set; }
        public string Description { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbDocumentType IdDocumentTypeNavigation { get; set; }
        public ICollection<GlbPartnerTypeDoc> GlbPartnerTypeDoc { get; set; }
        public ICollection<GlbSharedDocument> GlbSharedDocument { get; set; }
    }
}
