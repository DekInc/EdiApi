using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class NTE830 : EdiBase
    {
        public const string Init = "NTE";
        public const string Self = "Note/Special Instruction";
        [StringLength(maximumLength: 3, MinimumLength = 0)]
        public string ReferenceCode { get; set; }
        [StringLength(maximumLength: 60, MinimumLength = 0)]
        public string Message { get; set; }
        public NTE830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "ReferenceCode", "Message"
            };
        }
    }
}
