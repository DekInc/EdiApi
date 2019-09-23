using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class LearSt856
    {
        public string IdCode { get; set; }
        public string ControlNumber { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
