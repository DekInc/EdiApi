using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class BSN856 : EdiBase
    {
        public const string Init = "BSN";
        public const string Self = "Beginning Segment for Ship Notice";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string TransactionSetPurposeCode { get; set; }        
        [StringLength(maximumLength: 30, MinimumLength = 2)]
        public string ShipIdentification { get; set; }        
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string BsnDate { get; set; }        
        [StringLength(maximumLength: 4, MinimumLength = 4)]
        public string BsnTime { get; set; }
        public BSN856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "TransactionSetPurposeCode", "ShipIdentification",
                "BsnDate", "BsnTime"
            };
        }
    }
}
