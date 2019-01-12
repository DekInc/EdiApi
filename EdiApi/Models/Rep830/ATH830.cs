using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ATH830 : EdiBase
    {
        public string Init { get; set; } = "ATH";
        public string ResourceAuthCode { get; set; }
        public string StartDate { get; set; }
        public string Quantity { get; set; }
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
