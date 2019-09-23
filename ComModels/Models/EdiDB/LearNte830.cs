using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class LearNte830
    {
        public string ReferenceCode { get; set; }
        public string Message { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
