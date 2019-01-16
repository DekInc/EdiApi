using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ISATrailer856 : EdiBase
    {
        public string Init { get; set; } = "IEA";
        [StringLength(maximumLength: 1, MinimumLength = 5)]
        public string SegmentCount { get; set; } = "1"; //Entre ISA e IEA
        [StringLength(maximumLength: 9, MinimumLength = 9)]
        public string ControlNumber { get; set; } = "2";
        public ISATrailer856(UInt16 RepType, string _SegmentTerminator, string _ControlNumber) : base(_SegmentTerminator)
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
