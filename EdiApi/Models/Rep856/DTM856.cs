using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class DTM856 : EdiBase
    {
        public const string Init = "DTM";
        public const string Self = "Date/Time Reference";
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string DateTimeQualifier { get; set; }        
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string DtmDate { get; set; }        
        [StringLength(maximumLength: 4, MinimumLength = 4)]
        public string DtmTime { get; set; }
        public DTM856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "DateTimeQualifier", "DtmDate", "DtmTime"
            };
        }
    }
}
