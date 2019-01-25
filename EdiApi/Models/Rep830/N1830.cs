using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class N1830 : EdiBase
    {
        public const string Init = "N1";
        public const string Self = "Name of Material Release Issuer & the Supplier";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string OrganizationId { get; set; }
        [StringLength(maximumLength: 35, MinimumLength = 0)]
        public string Name { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string IdCodeQualifier { get; set; }
        [StringLength(maximumLength: 17, MinimumLength = 0)]
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
