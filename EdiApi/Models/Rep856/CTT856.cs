using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EdiApi
{
    public partial class CTT856 : EdiBase
    {
        public const string Init = "CTT";
        public const string Self = "Transaction Totals";
        public string NumberOfLineItems { get; set; }
        public CTT856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "NumberOfLineItems"
            };
        }
    }
}
