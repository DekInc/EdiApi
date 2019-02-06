using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearFst830
    {
        public string Quantity { get; set; }
        public string ForecastQualifier { get; set; }
        public string ForecastTimingQualifier { get; set; }
        public string FstDate { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
