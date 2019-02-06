using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearEtd856
    {
        public string ExcessTransportationReasonCode { get; set; }
        public string ExcessTransportationResponsibilityCode { get; set; }
        public string ReferenceNumberQualifier { get; set; }
        public string ReferenceNumber { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
