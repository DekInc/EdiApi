using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class STTrailer830 : EdiBase
    {
        public string Init { get; set; } = "SE";
        public string SegmentCount { get; set; } = "1";
        public string ControlNumber { get; set; } = "1";
        public STTrailer830(UInt16 RepType, string _SegmentTerminator, string _ControlNumber) : base(_SegmentTerminator)
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
