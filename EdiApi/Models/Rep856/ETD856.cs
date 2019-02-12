using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class ETD856 : EdiBase
    {
        public const string Init = "ETD";
        public const string Self = "Excess Transportation Detail";
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string ExcessTransportationReasonCode { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 1)]
        public string ExcessTransportationResponsibilityCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string ReferenceNumberQualifier { get; set; }
        [StringLength(maximumLength: 30, MinimumLength = 1)]
        public string ReferenceNumber { get; set; }
        public ETD856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "ExcessTransportationReasonCode", "ExcessTransportationResponsibilityCode",
                "ReferenceNumberQualifier", "ReferenceNumber"
            };
        }
    }
}
