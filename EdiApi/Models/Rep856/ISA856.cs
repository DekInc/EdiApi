using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ISA856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "ISA";
        public const string Self = "Interchange Control Structure";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string AuthorizationInformationQualifier { get; set; } = "00";
        [StringLength(maximumLength: 18, MinimumLength = 0)]
        public string AuthorizationInformation { get; set; } = "          ";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string SecurityInformationQualifier { get; set; } = "00";
        [StringLength(maximumLength: 10, MinimumLength = 0)]
        public string SecurityInformation { get; set; } = "          ";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string InterchangeSenderIdQualifier { get; set; } = "ZZ";
        [StringLength(maximumLength: 15, MinimumLength = 0)]
        public string InterchangeSenderId { get; set; } = "GLC503";
        [StringLength(maximumLength: 2, MinimumLength = 0)]
        public string InterchangeReceiverIdQualifier { get; set; } = "ZZ";
        [StringLength(maximumLength: 15, MinimumLength = 0)]
        public string InterchangeReceiverId { get; set; } = "HN02NC72       ";
        [StringLength(maximumLength: 6, MinimumLength = 0)]
        public string InterchangeDate { get; set; } = "YYMMDD";
        [StringLength(maximumLength: 4, MinimumLength = 0)]
        public string InterchangeTime { get; set; } = "HHMM";
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string InterchangeControlStandardsId { get; set; } = "U";
        [StringLength(maximumLength: 5, MinimumLength = 0)]
        public string InterchangeControlVersion { get; set; } = "00204";
        [StringLength(maximumLength: 9, MinimumLength = 0)]
        public string InterchangeControlNumber { get; set; } = "000002409";
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string AcknowledgmentRequested { get; set; } = "0";
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string UsageIndicator { get; set; } = "T"; // T o P
        [StringLength(maximumLength: 1, MinimumLength = 0)]
        public string ComponentElementSeparator { get; set; } = "<";
        public IEA830 ISATrailerO { get; set; }
        public ISA856(string _SegmentTerminator) : base(_SegmentTerminator) { InitOrden(); }
        public ISA856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            switch (_RepType)
            {
                case 0:
                    InterchangeControlNumber = _ControlNumber;
                    ISATrailerO = new IEA830(SegmentTerminator);
                    InitOrden();
                    break;
            }
        }
        private void InitOrden() => Orden = new string[]{
                "Init",
                "AuthorizationInformationQualifier", "AuthorizationInformation",
                "SecurityInformationQualifier", "SecurityInformation",
                "InterchangeSenderIdQualifier", "InterchangeSenderId",
                "InterchangeReceiverIdQualifier", "InterchangeReceiverId",
                "InterchangeDate", "InterchangeTime",
                "InterchangeControlStandardsId", "InterchangeControlVersion",
                "InterchangeControlNumber", "AcknowledgmentRequested",
                "UsageIndicator", "ComponentElementSeparator"
            };
    }
}
