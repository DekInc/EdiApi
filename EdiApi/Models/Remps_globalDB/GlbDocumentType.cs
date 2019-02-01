using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbDocumentType
    {
        public GlbDocumentType()
        {
            GlbDocument = new HashSet<GlbDocument>();
        }

        public long IdDocumentType { get; set; }
        public string Description { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public ICollection<GlbDocument> GlbDocument { get; set; }
    }
}
