using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class LearAth830
    {
        public string ResourceAuthCode { get; set; }
        public string StartDate { get; set; }
        public string Quantity { get; set; }
        public string NotUsed { get; set; }
        public string EndDate { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
