using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearN1856
    {
        public string EntityIdentifierCode { get; set; }
        public string IdCodeQualifier { get; set; }
        public string IdCode { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
