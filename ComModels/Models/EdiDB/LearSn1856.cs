using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class LearSn1856
    {
        public string NumberOfUnitsShipped { get; set; }
        public string UnitOfMeasurementCode { get; set; }
        public string QuantityShipped { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
