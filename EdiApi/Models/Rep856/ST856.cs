using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ST856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public string Init { get; set; } = "ST";
        public string IdCode { get; set; } = "0";
        public string ControlNumber { get; set; } = "1";
        public STTrailer856 StTrailerO { get; set; }
        public ST856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            Orden = new string[]{
                "Init",
                "IdCode", "ControlNumber"
            };
            switch (_RepType)
            {
                case 0:
                    IdCode = "830";
                    ControlNumber = _ControlNumber;
                    StTrailerO = new STTrailer856(RepType, SegmentTerminator, ControlNumber);
                    break;
            }
        }
    }
}
