using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class LIN830 : EdiBase
    {
        public const string Init = "LIN";
        public const string Self = "Item Identification Detail";
        public string AssignedIdentification { get; set; }
        public string ProductIdQualifier { get; set; }
        public string ProductId { get; set; }
        public string ProductRefIdQualifier { get; set; }
        public string ProductRefId { get; set; }
        public string ProductPurchaseIdQualifier { get; set; }
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
