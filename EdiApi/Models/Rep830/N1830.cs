using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class N1830 : EdiBase
    {
        public string Init { get; set; } = "N1";
        public string OrganizationId { get; set; }
        public string Name { get; set; }
        public string IdCodeQualifier { get; set; }
        public string IdCode { get; set; }
        public N1830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "OrganizationId", "Name",
                "IdCodeQualifier", "IdCode"
            };
        }
    }
}
