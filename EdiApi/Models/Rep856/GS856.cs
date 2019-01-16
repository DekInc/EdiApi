using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    /// <summary>
    /// The GS Segment indicates that beginning of a functional group & provides
    /// control information.
    /// </summary>
    public class GS856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "GS";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string FunctionalIdCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 15)]
        public string ApplicationSenderCode { get; set; }
        [StringLength(maximumLength: 2, MinimumLength = 15)]
        public string ApplicationReceiverCode { get; set; }
        [StringLength(maximumLength: 8, MinimumLength = 8)]
        public string GsDate { get; set; }
        [StringLength(maximumLength: 4, MinimumLength = 8)]
        public string GsTime { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 9)]
        public string GroupControlNumber { get; set; }
        /// <summary>
        /// 'X' indicates ANSI ASC X12.
        /// </summary>
        [StringLength(maximumLength: 1, MinimumLength = 2)]
        public string ResponsibleAgencyCode { get; set; }
        [StringLength(maximumLength: 1, MinimumLength = 12)]
        public string Version { get; set; }
        public GSTrailer856 GSTrailerO { get; set; }
        public GS856() : base("") { }
        public GS856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            GroupControlNumber = _ControlNumber;
            Orden = new string[]{
                "Init",
                "FunctionalIdCode", "ApplicationSenderCode",
                "ApplicationReceiverCode", "GsDate",
                "GsTime", "GroupControlNumber",
                "ResponsibleAgencyCode", "Version"
            };
            switch (_RepType)
            {
                case 0:
                    //ControlNumber = _ControlNumber;
                    GSTrailerO = new GSTrailer856(RepType, SegmentTerminator, _ControlNumber);
                    break;
            }
        }
    }
}
