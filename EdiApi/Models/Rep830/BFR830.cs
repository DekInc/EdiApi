using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class BFR830 : EdiBase
    {
        public const string Init = "BFR";
        public const string Self = "Beginning Segment for Planning Schedule";
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
