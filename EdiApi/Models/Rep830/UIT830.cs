using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class UIT830 : EdiBase
    {
        public string Init { get; set; } = "UIT";
        public string UnitOfMeasure { get; set; }
        public UIT830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "UnitOfMeasure"
            };
        }
    }
}
