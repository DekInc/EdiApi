using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class IEA856 : EdiBase
    {
        public const string Init = "IEA";
        public const string Self = "Interchange Control Trailer";
        /// <summary>
        /// This is the number of functional groups
        /// (GS/GE Pairs) between the ISA & the IEA.
        /// </summary>
        public string NumIncludedGroups { get; set; }
        /// <summary>
        /// This control number must match the ISA
        /// control number.
        /// </summary>
        public string InterchangeControlNumber { get; set; }
        public IEA856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "NumIncludedGroups", "InterchangeControlNumber"
            };
        }
    }
}
