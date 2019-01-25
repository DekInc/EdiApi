using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class UIT830 : EdiBase
    {
        public const string Init = "UIT";
        public const string Self = "Unit Detail";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
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
