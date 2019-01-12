using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class FST830 : EdiBase
    {
        public string Init { get; set; } = "FST";
        public string Quantity { get; set; }
        public string ForecastQualifier { get; set; }
        public string ForecastTimingQualifier { get; set; }
        public string FstDate { get; set; }
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
