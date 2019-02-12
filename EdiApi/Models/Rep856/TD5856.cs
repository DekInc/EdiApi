using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class TD5856 : EdiBase
    {
        public const string Init = "TD5";
        public const string Self = "Carrier Details (Routing Sequence/Transit Time)";
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string RoutingSequenceCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string IdCodeQualifier { get; set; }
        [StringLength(maximumLength: 17, MinimumLength = 2)]
        public string IdentificationCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string TransportationMethodCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string LocationQualifier { get; set; }
        [StringLength(maximumLength: 7, MinimumLength = 1)]
        public string LocationIdentifier { get; set; }
        public TD5856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "RoutingSequenceCode", "IdCodeQualifier",
                "IdentificationCode", "TransportationMethodCode",
                "LocationQualifier", "LocationIdentifier",
            };
        }
    }
}
