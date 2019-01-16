using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    /// <summary>
    /// To indicate the end of the transaction set and provide the
    /// count of the transmitted segments(including the beginning
    /// (ST) and ending (SE) segments).
    /// </summary>
    public class STTrailer856 : EdiBase
    {
        public string Init { get; set; } = "SE";
        /// <summary>
        /// Number of Included Segments
        /// </summary>
        [StringLength(maximumLength: 1, MinimumLength = 6)]
        public string SegmentCount { get; set; } = "1";
        /// <summary>
        /// Same as "ST02"
        /// </summary>
        [StringLength(maximumLength: 4, MinimumLength = 9)]
        public string ControlNumber { get; set; } = "1";
        public STTrailer856(UInt16 RepType, string _SegmentTerminator, string _ControlNumber) : base(_SegmentTerminator)
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
