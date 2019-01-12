using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class PRS830 : EdiBase
    {
        public string Init { get; set; } = "PRS";
        public string StatusCode { get; set; }
        public PRS830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "StatusCode"
            };
        }
    }
}
