using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class TD3856 : EdiBase
    {
        public const string Init = "TD3";
        public const string Self = "Carrier Details (Equipment)";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string EquipmentDescriptionCode { get; set; }
        [StringLength(maximumLength: 4, MinimumLength = 1)]
        public string EquipmentInitial { get; set; }
        [StringLength(maximumLength: 10, MinimumLength = 1)]
        public string EquipmentNumber { get; set; }
        public TD3856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "EquipmentDescriptionCode", "EquipmentInitial",
                "EquipmentNumber"
            };
        }
    }
}
