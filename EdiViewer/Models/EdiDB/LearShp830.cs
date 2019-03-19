using System;
using System.Collections.Generic;

namespace EdiViewer.Models.EdiDB
{
    public partial class LearShp830
    {
        public string QuantityQualifier { get; set; }
        public string Quantity { get; set; }
        public string DateTimeQualifier { get; set; }
        public string AccumulationStartDate { get; set; }
        public string AccumulationTime { get; set; }
        public string AccumulationEndDate { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
