using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class N1856 : EdiBase
    {
        public const string Init = "N1";
        public const string Self = "Detail (Shipment Hierarchical Level)";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string EntityIdentifierCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string IdCodeQualifier { get; set; }
        [StringLength(maximumLength: 17, MinimumLength = 2)]
        public string IdCode { get; set; }
        public N1856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "EntityIdentifierCode", "IdCodeQualifier",
                "IdCode"
            };
        }
    }
}
