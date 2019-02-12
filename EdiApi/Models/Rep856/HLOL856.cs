using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class HLOL856 : EdiBase
    {
        public const string Init = "HL";
        public const string Self = "Hierarchical Level";
        [StringLength(maximumLength: 12, MinimumLength = 1)]
        public string HierarchicalIdNumber { get; set; }
        [StringLength(maximumLength: 12, MinimumLength = 1)]
        public string HierarchicalParentIdNumber { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 1)]
        public string HierarchicalLevelCode { get; set; }
        public HLOL856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "HierarchicalIdNumber", "HierarchicalParentIdNumber",
                "HierarchicalLevelCode"
            };
        }
    }
}
