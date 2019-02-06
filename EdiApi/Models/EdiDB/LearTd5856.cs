using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearTd5856
    {
        public string RoutingSequenceCode { get; set; }
        public string IdCodeQualifier { get; set; }
        public string IdentificationCode { get; set; }
        public string TransportationMethodCode { get; set; }
        public string LocationQualifier { get; set; }
        public string LocationIdentifier { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
