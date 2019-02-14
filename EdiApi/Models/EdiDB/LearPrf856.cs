using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearPrf856
    {
        public string PurchaseOrderNumber { get; set; }
        public string ReleaseNumber { get; set; }
        public string ChangeOrderSequenceNumber { get; set; }
        public string PurchaseOrderDate { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
