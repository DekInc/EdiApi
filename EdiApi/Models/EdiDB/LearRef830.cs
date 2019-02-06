using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearRef830
    {
        public string RefNumberQualifier { get; set; }
        public string RefNumber { get; set; }
        public string Description { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
