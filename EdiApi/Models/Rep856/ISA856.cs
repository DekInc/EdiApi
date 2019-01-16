using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    /// <summary>
    /// The ISA Segment is the Interchange Control Header used to start &
    /// identify an interchange of one or more functional groups & interchange
    /// related control segments.This is the only “fixed length” record in the
    /// ANSI X12 standard. 
    /// </summary>
    public class ISA856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "ISA";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string AuthorizationInformationQualifier { get; set; } = "00";
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string AuthorizationInformation { get; set; } = "          ";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string SecurityInformationQualifier { get; set; } = "00";
        [StringLength(maximumLength: 10, MinimumLength = 10)]
        public string SecurityInformation { get; set; } = "          ";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string InterchangeSenderIdQualifier { get; set; } = "ZZ";
        [StringLength(maximumLength: 15, MinimumLength = 15)]
        public string InterchangeSenderId { get; set; } = "GLC503";
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string InterchangeReceiverIdQualifier { get; set; } = "ZZ";
        [StringLength(maximumLength: 15, MinimumLength = 15)]
        public string InterchangeReceiverId { get; set; } = "HN02NC72       ";
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string InterchangeDate { get; set; } = "YYMMDD";
        [StringLength(maximumLength: 4, MinimumLength = 4)]
        public string InterchangeTime { get; set; } = "HHMM";
        [StringLength(maximumLength: 1, MinimumLength = 1)]
        public string InterchangeControlStandardsId { get; set; } = "U";
        [StringLength(maximumLength: 5, MinimumLength = 5)]
        public string InterchangeControlVersion { get; set; } = "00204";
        [StringLength(maximumLength: 9, MinimumLength = 9)]
        public string InterchangeControlNumber { get; set; } = "000002409";
        [StringLength(maximumLength: 1, MinimumLength = 1)]
        public string AcknowledgmentRequested { get; set; } = "0";
        [StringLength(maximumLength: 1, MinimumLength = 1)]
        public string UsageIndicator { get; set; } = "T"; // T o P
        [StringLength(maximumLength: 1, MinimumLength = 1)]
        public string ComponentElementSeparator { get; set; } = "<";
        public ISATrailer856 ISATrailerO { get; set; }
        public ISA856() : base("") { }
        public ISA856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            switch (_RepType)
            {
                case 0:
                    InterchangeControlNumber = _ControlNumber;
                    ISATrailerO = new ISATrailer856(RepType, SegmentTerminator, _ControlNumber);
                    Orden = new string[]{
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
                    break;
            }
        }
    }
}
