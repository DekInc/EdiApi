using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ST856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "ST";
        public const string Self = "Transaction Set Header";
        [StringLength(maximumLength: 3, MinimumLength = 0)]
        public string IdCode { get; set; } = "0";
        [StringLength(maximumLength: 9, MinimumLength = 0)]
        public string ControlNumber { get; set; } = "1";
        public SE830 StTrailerO { get; set; }
        public ST856(string _SegmentTerminator) : base(_SegmentTerminator) { InitOrden(); }
        public ST856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            InitOrden();
            switch (_RepType)
            {
                case 0:
                    IdCode = "856";
                    ControlNumber = _ControlNumber;
                    StTrailerO = new SE830(SegmentTerminator);
                    break;
            }
        }
        public void InitOrden() => Orden = new string[]{
                "Init",
                "IdCode", "ControlNumber"
            };
    }
}
