using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ATH830 : EdiBase
    {
        public const string Init = "ATH";
        public const string Self = "Resource Authorization";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string ResourceAuthCode { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string StartDate { get; set; }
        [StringLength(maximumLength: 10, MinimumLength = 0)]
        public string Quantity { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string EndDate { get; set; }
        public ATH830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "ResourceAuthCode", "StartDate",
                "Quantity", "NotUsed", "EndDate"
            };
        }
    }
}
