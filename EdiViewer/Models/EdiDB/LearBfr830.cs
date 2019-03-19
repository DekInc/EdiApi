using System;
using System.Collections.Generic;

namespace EdiViewer.Models.EdiDB
{
    public partial class LearBfr830
    {
        public string TransactionSetPurposeCode { get; set; }
        public string ForecastOrderNumber { get; set; }
        public string ReleaseNumber { get; set; }
        public string ForecastTypeQualifier { get; set; }
        public string ForecastQuantityQualifier { get; set; }
        public string ForecastHorizonStart { get; set; }
        public string ForecastHorizonEnd { get; set; }
        public string ForecastGenerationDate { get; set; }
        public string ForecastUpdatedDate { get; set; }
        public string ContractNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
