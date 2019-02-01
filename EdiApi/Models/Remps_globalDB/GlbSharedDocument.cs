using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbSharedDocument
    {
        public long IdSharedDocument { get; set; }
        public long IdDocument { get; set; }
        public long IdClient { get; set; }
        public long IdEmployee { get; set; }
        public long IdTransport { get; set; }
        public long IdRcontrol { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public byte[] Path { get; set; }
        public byte[] ObjectUrl { get; set; }
        public string WebkitRelativePath { get; set; }
        public long LastModified { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbDocument IdDocumentNavigation { get; set; }
    }
}
