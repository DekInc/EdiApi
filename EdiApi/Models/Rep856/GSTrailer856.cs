using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class GSTrailer856 : EdiBase
    {
        public string Init { get; set; } = "GE";
        [StringLength(maximumLength: 1, MinimumLength = 6)]
        public string SegmentCount { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 9)]
        public string ControlNumber { get; set; }
        public GSTrailer856(UInt16 RepType, string _SegmentTerminator, string _ControlNumber) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "SegmentCount", "ControlNumber"
            };
            switch (RepType)
            {
                case 0:
                    ControlNumber = _ControlNumber;
                    break;
            }
        }
    }
}
