using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    /*
     * To indicate the start of a transaction set and to
    assign a control number.
    */    
    public class ST856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public string Init { get; set; } = "ST";
        /// <summary>
        /// Use 856
        /// </summary>
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string IdCode { get; set; } = "0";
        /// <summary>
        /// A unique control number assigned to each transaction set within
        /// a functional group, starting with 0001 and incrementing by 1
        /// for each subsequent transaction set.Same as SE02.
        /// </summary>
        [StringLength(maximumLength: 4, MinimumLength = 9)]
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
                    IdCode = "856";
                    ControlNumber = _ControlNumber;
                    StTrailerO = new STTrailer856(RepType, SegmentTerminator, ControlNumber);
                    break;
            }
        }
    }
}
