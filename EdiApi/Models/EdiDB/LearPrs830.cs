using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearPrs830
    {
        public string StatusCode { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
