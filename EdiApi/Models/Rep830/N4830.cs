using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class N4830 : EdiBase
    {
        public const string Init = "N4";
        public const string Self = "Geographic Location";
        [StringLength(maximumLength: 19, MinimumLength = 0)]
        public string CityName { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string Province { get; set; }
        [StringLength(maximumLength: 9, MinimumLength = 0)]
        public string PostalCode { get; set; }
        [StringLength(maximumLength: 4, MinimumLength = 0)]
        public string CountryCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string LocationQualifier { get; set; }
        [StringLength(maximumLength: 25, MinimumLength = 0)]
        public string LocationId { get; set; }
        public N4830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "CityName", "Province",
                "PostalCode", "CountryCode",
                "LocationQualifier", "LocationId"
            };
        }
    }
}
