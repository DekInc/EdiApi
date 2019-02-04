using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class PRF856 : EdiBase
    {
        public const string Init = "PRF";
        public const string Self = "Purchase Order Reference";
        [StringLength(maximumLength: 13, MinimumLength = 1)]
        public string PurchaseOrderNumber { get; set; }        
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ReleaseNumber { get; set; }        
        [StringLength(maximumLength: 8, MinimumLength = 1)]
        public string ChangeOrderSequenceNumber { get; set; }        
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string PurchaseOrderDate { get; set; }
        public PRF856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "PurchaseOrderNumber", "ReleaseNumber",
                "ChangeOrderSequenceNumber", "PurchaseOrderDate"
            };
        }
    }
}
