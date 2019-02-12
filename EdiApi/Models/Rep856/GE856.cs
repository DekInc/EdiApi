using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class GE856 : EdiBase
    {
        public const string Init = "GE";
        public const string Self = "Functional Group Trailer";
        /// <summary>
        /// This is the total count of
        /// transaction sets(ST through SE) included in this functional group.
        /// </summary>
        public string NumTransactionSetsIncluded { get; set; }
        public string GroupControl { get; set; }
        public GE856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "NumTransactionSetsIncluded", "GroupControl"
            };
        }
    }
}
