using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class N4830 : EdiBase
    {
        public string Init { get; set; } = "N4";
        public string CityName { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string LocationQualifier { get; set; }
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
