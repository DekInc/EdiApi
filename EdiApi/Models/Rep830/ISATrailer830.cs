﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ISATrailer830 : EdiBase
    {
        public string Init { get; set; } = "IEA";
        public string SegmentCount { get; set; } = "1"; //Entre ISA e IEA
        public string ControlNumber { get; set; } = "2";
        public ISATrailer830(UInt16 RepType, string _SegmentTerminator, string _ControlNumber) : base(_SegmentTerminator)
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
