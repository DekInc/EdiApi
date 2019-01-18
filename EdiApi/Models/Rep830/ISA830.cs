using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ISA830 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "ISA";
        public const string Self = "Interchange Control Structure";
        public string AuthorizationInformationQualifier { get; set; } = "00";
        public string AuthorizationInformation { get; set; } = "          ";
        public string SecurityInformationQualifier { get; set; } = "00";
        public string SecurityInformation { get; set; } = "          ";
        public string InterchangeSenderIdQualifier { get; set; } = "ZZ";
        public string InterchangeSenderId { get; set; } = "GLC503";
        public string InterchangeReceiverIdQualifier { get; set; } = "ZZ";
        public string InterchangeReceiverId { get; set; } = "HN02NC72       ";
        public string InterchangeDate { get; set; } = "YYMMDD";
        public string InterchangeTime { get; set; } = "HHMM";
        public string InterchangeControlStandardsId { get; set; } = "U";
        public string InterchangeControlVersion { get; set; } = "00204";
        public string InterchangeControlNumber { get; set; } = "000002409";
        public string AcknowledgmentRequested { get; set; } = "0";
        public string UsageIndicator { get; set; } = "T"; // T o P
        public string ComponentElementSeparator { get; set; } = "<";
        public ISA830 ISATrailerO { get; set; }        
        public ISA830(string _SegmentTerminator) : base(_SegmentTerminator) { InitOrden(); }
        public ISA830(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            switch (_RepType)
            {
                case 0:
                    InterchangeControlNumber = _ControlNumber;
                    ISATrailerO = new ISA830(SegmentTerminator);
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
