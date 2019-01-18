using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class REF830 : EdiBase
    {
        public const string Init = "REF";
        public const string Self = "Reference Numbers";
        public string RefNumberQualifier { get; set; }
        public string RefNumber { get; set; }
        public string Description { get; set; }
        public REF830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "RefNumberQualifier", "RefNumber", "Description"
            };
        }
    }
}
