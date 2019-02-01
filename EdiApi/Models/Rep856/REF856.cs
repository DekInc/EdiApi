using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class REF856 : EdiBase
    {
        public const string Init = "REF";
        public const string Self = "Reference Numbers";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ReferenceNumberQualifier { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ReferenceNumber { get; set; }
        public REF856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "ReferenceNumberQualifier", "ReferenceNumber"
            };
        }
    }
}
