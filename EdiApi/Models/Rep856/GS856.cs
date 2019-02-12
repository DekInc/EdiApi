using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public partial class GS856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "GS";
        public const string Self = "Functional Group Header";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string FunctionalIdCode { get; set; } = "0";
        [StringLength(maximumLength: 15, MinimumLength = 0)]
        public string ApplicationSenderCode { get; set; } = "1";
        [StringLength(maximumLength: 15, MinimumLength = 0)]
        public string ApplicationReceiverCode { get; set; } = "2";
        [StringLength(maximumLength: 8, MinimumLength = 0)]
        public string GsDate { get; set; } = "3";
        [StringLength(maximumLength: 8, MinimumLength = 0)]
        public string GsTime { get; set; } = "4";
        [StringLength(maximumLength: 9, MinimumLength = 0)]
        public string GroupControlNumber { get; set; } = "5";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string ResponsibleAgencyCode { get; set; } = "6";
        [StringLength(maximumLength: 12, MinimumLength = 0)]
        public string Version { get; set; } = "7";
        public GE830 GSTrailerO { get; set; }
        public GS856(string _SegmentTerminator) : base(_SegmentTerminator) { InitOrden(); }
        public GS856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            GroupControlNumber = _ControlNumber;
            InitOrden();
            switch (_RepType)
            {
                case 0:
                    //ControlNumber = _ControlNumber;
                    GSTrailerO = new GE830(SegmentTerminator);
                    break;
            }
        }
        private void InitOrden() => Orden = new string[]{
            "Init",
            "FunctionalIdCode", "ApplicationSenderCode",
            "ApplicationReceiverCode", "GsDate",
            "GsTime", "GroupControlNumber",
            "ResponsibleAgencyCode", "Version"
        };
    }
}
