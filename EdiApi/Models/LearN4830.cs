using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class LearN4830
    {
        public string CityName { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string LocationQualifier { get; set; }
        public string LocationId { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
