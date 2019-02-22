using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class FST830 : EdiBase
    {
        public const string Init = "FST";
        public const string Self = "Forecast Schedule";
        [StringLength(maximumLength: 10, MinimumLength = 0)]
        public string Quantity { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string ForecastQualifier { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string ForecastTimingQualifier { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string FstDate { get; set; }
        public string RealQty { get; set; }
        public FST830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "Quantity", "ForecastQualifier",
                "ForecastTimingQualifier", "FstDate"
            };
        }
    }
}
