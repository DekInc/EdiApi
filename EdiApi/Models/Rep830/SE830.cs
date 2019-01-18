using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class SE830 : EdiBase
    {
        public const string Init = "SE";
        public const string Self = "Transaction Set Trailer";
        public string NumIncludedSegments { get; set; }
        public string TransactionSetControlNumber { get; set; }
        public SE830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "NumIncludedSegments", "TransactionSetControlNumber"
            };
        }
    }
}
