using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class UIT830 : EdiBase
    {
        public const string Init = "UIT";
        public const string Self = "Unit Detail";
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
