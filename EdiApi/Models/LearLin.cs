using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class LearLin
    {
        public int Id { get; set; }
        public int IdBfr { get; set; }
        public string AssignedIdentification { get; set; }
        public string ProductIdQualifier { get; set; }
        public string ProductId { get; set; }
        public string ProductRefIdQualifier { get; set; }
        public string ProductRefId { get; set; }
        public string ProductPurchaseIdQualifier { get; set; }
        public string ProductPurchaseId { get; set; }

        public LearBfr IdBfrNavigation { get; set; }
    }
}
