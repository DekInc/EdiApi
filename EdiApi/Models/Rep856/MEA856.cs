using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class MEA856 : EdiBase
    {
        public const string Init = "MEA";
        public const string Self = "Measurements";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string MeasurementReferenceIdCode { get; set; }
        [StringLength(maximumLength: 3, MinimumLength = 1)]
        public string MeasurementDimensionQualifier { get; set; }
        [StringLength(maximumLength: 10, MinimumLength = 1)]
        public string MeasurementValue { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string UnitOfMeasure { get; set; }
        public MEA856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "MeasurementReferenceIdCode", "MeasurementDimensionQualifier",
                "MeasurementValue", "UnitOfMeasure"
            };
        }
    }
}
