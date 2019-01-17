using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class SDP830 : EdiBase
    {
        public const string Init = "SDP";
        public const string Self = "Ship/Delivery Pattern";
        public string CalendarPatternCode { get; set; }
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
