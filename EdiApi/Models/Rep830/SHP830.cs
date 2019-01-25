using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class SHP830 : EdiBase
    {
        public const string Init = "SHP";
        public const string Self = "Shipped/Received Information";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string QuantityQualifier { get; set; }
        [StringLength(maximumLength: 10, MinimumLength = 0)]
        public string Quantity { get; set; }
        [StringLength(maximumLength: 3, MinimumLength = 0)]
        public string DateTimeQualifier { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string AccumulationStartDate { get; set; }
        [StringLength(maximumLength: 4, MinimumLength = 0)]
        public string AccumulationTime { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string AccumulationEndDate { get; set; }
        public SHP830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "QuantityQualifier", "Quantity",
                "DateTimeQualifier", "AccumulationStartDate",
                "AccumulationTime", "AccumulationEndDate"
            };
        }
    }
}
