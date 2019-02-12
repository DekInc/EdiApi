using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class SN1856 : EdiBase
    {
        public const string Init = "SN1";
        public const string Self = "Item Detail (Shipment)";
        [StringLength(maximumLength: 10, MinimumLength = 1)]
        public string NumberOfUnitsShipped { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string UnitOfMeasurementCode { get; set; }
        [StringLength(maximumLength: 9, MinimumLength = 1)]
        public string QuantityShipped { get; set; }
        public SN1856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "NumberOfUnitsShipped", "UnitOfMeasurementCode",
                "QuantityShipped"
            };
        }
    }
}
