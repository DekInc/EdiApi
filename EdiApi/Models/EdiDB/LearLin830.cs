using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearLin830
    {
        public string AssignedIdentification { get; set; }
        public string ProductIdQualifier { get; set; }
        public string ProductId { get; set; }
        public string ProductRefIdQualifier { get; set; }
        public string ProductRefId { get; set; }
        public string ProductPurchaseIdQualifier { get; set; }
        public string ProductPurchaseId { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
        public string Comments { get; set; }
    }
}
