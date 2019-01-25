using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class REF830 : EdiBase
    {
        public const string Init = "REF";
        public const string Self = "Reference Numbers";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string RefNumberQualifier { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 0)]
        public string RefNumber { get; set; }
        [StringLength(maximumLength: 80, MinimumLength = 0)]
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
