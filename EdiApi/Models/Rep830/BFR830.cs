using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class BFR830 : EdiBase
    {
        public const string Init = "BFR";
        public const string Self = "Beginning Segment for Planning Schedule";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string TransactionSetPurposeCode { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string ForecastOrderNumber { get; set; }
        [StringLength(maximumLength: 4, MinimumLength = 0)]
        public string ReleaseNumber { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string ForecastTypeQualifier { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string ForecastQuantityQualifier { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string ForecastHorizonStart { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string ForecastHorizonEnd { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string ForecastGenerationDate { get; set; }
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string ForecastUpdatedDate { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string ContractNumber { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string PurchaseOrderNumber { get; set; }
        public BFR830(string _SegmentTerminator) : base(_SegmentTerminator) { InitOrden(); }
        public void InitOrden() => Orden = new string[]{
            "Init",
            "TransactionSetPurposeCode", "ForecastOrderNumber",
            "ReleaseNumber", "ForecastTypeQualifier",
            "ForecastQuantityQualifier", "ForecastHorizonStart",
            "ForecastHorizonEnd", "ForecastGenerationDate",
            "ForecastUpdatedDate", "ContractNumber",
            "PurchaseOrderNumber"
        };
    }
}
