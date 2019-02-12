using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class LIN856 : EdiBase
    {
        public const string Init = "LIN";
        public const string Self = "Item Identification";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier1 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId1 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier2 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId2 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier3 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId3 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier4 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId4 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier5 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId5 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier6 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId6 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier7 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId7 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier8 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId8 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier9 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId9 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier10 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId10 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier11 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId11 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier12 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId12 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier13 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId13 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier14 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId14 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier15 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId15 { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ProductIdQualifier16 { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ProductId16 { get; set; }

        public LIN856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "ProductIdQualifier1", "ProductId1",
                "ProductIdQualifier2", "ProductId2",
                "ProductIdQualifier3", "ProductId3",
                "ProductIdQualifier4", "ProductId4",
                "ProductIdQualifier5", "ProductId5",
                "ProductIdQualifier6", "ProductId6",
                "ProductIdQualifier7", "ProductId7",
                "ProductIdQualifier8", "ProductId8",
                "ProductIdQualifier9", "ProductId9",
                "ProductIdQualifier10", "ProductId10",
                "ProductIdQualifier11", "ProductId11",
                "ProductIdQualifier12", "ProductId12",
                "ProductIdQualifier13", "ProductId13",
                "ProductIdQualifier14", "ProductId14",
                "ProductIdQualifier15", "ProductId15",
                "ProductIdQualifier16", "ProductId16"
            };
        }
    }
}
