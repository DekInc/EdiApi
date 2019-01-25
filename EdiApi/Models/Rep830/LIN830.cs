using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class LIN830 : EdiBase
    {
        public const string Init = "LIN";
        public const string Self = "Item Identification Detail";
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string AssignedIdentification { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string ProductIdQualifier { get; set; }
        [StringLength(maximumLength: 22, MinimumLength = 0)]
        public string ProductId { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string ProductRefIdQualifier { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 0)]
        public string ProductRefId { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string ProductPurchaseIdQualifier { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 0)]
        public string ProductPurchaseId { get; set; }
        public LIN830(string _SegmentTerminator) : base(_SegmentTerminator)
        {

            Orden = new string[]{
                "Init",
                "AssignedIdentification", "ProductIdQualifier",
                "ProductId", "ProductRefIdQualifier",
                "ProductRefId", "ProductPurchaseIdQualifier",
                "ProductPurchaseId"
            };            
        }
    }
}
