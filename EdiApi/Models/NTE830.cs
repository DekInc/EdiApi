using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class NTE830 : EdiBase
    {
        public string Init { get; set; } = "NTE";
        public string ReferenceCode { get; set; }
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
