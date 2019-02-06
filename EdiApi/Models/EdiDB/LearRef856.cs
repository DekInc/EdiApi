using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearRef856
    {
        public string ReferenceNumberQualifier { get; set; }
        public string ReferenceNumber { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
