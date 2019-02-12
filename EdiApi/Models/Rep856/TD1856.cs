using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class TD1856 : EdiBase
    {
        public const string Init = "TD1";
        public const string Self = "Carrier Details (Quantity and Weight)";
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string PackagingCode { get; set; }
        [StringLength(maximumLength: 7, MinimumLength = 1)]
        public string LadingQuantity { get; set; }
        public TD1856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "PackagingCode", "LadingQuantity"
            };
        }
    }
}
