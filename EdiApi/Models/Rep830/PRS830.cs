using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class PRS830 : EdiBase
    {
        public const string Init = "PRS";
        public const string Self = "Part release status";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string StatusCode { get; set; }
        public PRS830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "StatusCode"
            };
        }        
    }
}
