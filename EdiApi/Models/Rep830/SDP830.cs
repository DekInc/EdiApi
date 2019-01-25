using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class SDP830 : EdiBase
    {
        public const string Init = "SDP";
        public const string Self = "Ship/Delivery Pattern";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string CalendarPatternCode { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string PatternTimeCode { get; set; }
        public SDP830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "CalendarPatternCode", "PatternTimeCode"
            };
        }
    }
}
