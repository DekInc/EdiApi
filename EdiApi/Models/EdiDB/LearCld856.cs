using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class LearCld856
    {
        public string NumberOfCustomerLoads { get; set; }
        public string UnitsShipped { get; set; }
        public string PackagingCode { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
