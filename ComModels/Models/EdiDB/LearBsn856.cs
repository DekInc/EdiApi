using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class LearBsn856
    {
        public string TransactionSetPurposeCode { get; set; }
        public string ShipIdentification { get; set; }
        public string BsnDate { get; set; }
        public string BsnTime { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
