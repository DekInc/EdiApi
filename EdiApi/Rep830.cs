using EdiApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class ISATrailer : EdiBase
    {
        public string Init { get; set; } = "IEA";
        public string SegmentCount { get; set; } = "1"; //Entre ISA e IEA
        public string ControlNumber { get; set; } = "2";
        public ISATrailer(UInt16 RepType, string _ControlNumber) : base("")
        {
            Orden = new string[]{
                "Init",
                "SegmentCount", "ControlNumber"
            };
            switch (RepType)
            {
                case 0:
                    ControlNumber = _ControlNumber;
                    break;
            }
        }
    }
    public class ISA : EdiBase
    {
        public UInt16 RepType { get; set; }
        public string Init { get; set; } = "ISA";
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
        public ISATrailer ISATrailerO { get; set; }
        public ISA(UInt16 _RepType, string _ControlNumber = "000000001") : base("")
        {
            RepType = _RepType;
            switch (_RepType)
            {
                case 0:
                    InterchangeControlNumber = _ControlNumber;
                    ISATrailerO = new ISATrailer(RepType, _ControlNumber);
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
    public class GSTrailer : EdiBase
    {
        public string Init { get; set; } = "GE";
        public string SegmentCount { get; set; } = "0";
        public string ControlNumber { get; set; } = "1";
        public GSTrailer(UInt16 RepType, string _ControlNumber) : base("")
        {
            Orden = new string[]{
                "Init",
                "SegmentCount", "ControlNumber"
            };
            switch (RepType)
            {
                case 0:
                    ControlNumber = _ControlNumber;
                    break;
            }
        }
    }
    public class GS : EdiBase
    {
        public UInt16 RepType { get; set; }
        public string Init { get; set; } = "GS";
        public string FunctionalIdCode { get; set; } = "0";
        public string ApplicationSenderCode { get; set; } = "1";
        public string ApplicationReceiverCode { get; set; } = "2";
        public string GsDate { get; set; } = "3";
        public string GsTime { get; set; } = "4";
        public string GroupControlNumber { get; set; } = "5";
        public string ResponsibleAgencyCode { get; set; } = "6";
        public string Version { get; set; } = "7";
        public GSTrailer GSTrailerO { get; set; }
        public GS(UInt16 _RepType, string _ControlNumber = "000000001") : base("")
        {
            RepType = _RepType;
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
                    GSTrailerO = new GSTrailer(RepType, _ControlNumber);
                    break;
            }            
        }
    }

    public class STTrailer : EdiBase
    {
        public string Init { get; set; } = "SE";
        public string SegmentCount { get; set; } = "1";
        public string ControlNumber { get; set; } = "1";        
        public STTrailer(UInt16 RepType, string _ControlNumber) : base("")
        {
            Orden = new string[]{
                "Init",
                "SegmentCount", "ControlNumber"
            };
            switch (RepType)
            {
                case 0:
                    ControlNumber = _ControlNumber;
                    break;
            }
        }
    }

    public class ST : EdiBase
    {
        public UInt16 RepType { get; set; }
        public string Init { get; set; } = "ST";
        public string IdCode { get; set; } = "0";
        public string ControlNumber { get; set; } = "1";
        public STTrailer StTrailerO { get; set; }
        public ST(UInt16 _RepType, string _ControlNumber = "000000001") : base("")
        {
            RepType = _RepType;
            Orden = new string[]{
                "Init",
                "IdCode", "ControlNumber"
            };
            switch (_RepType) {
                case 0:
                    IdCode = "830";
                    ControlNumber = _ControlNumber;
                    StTrailerO = new STTrailer(RepType, ControlNumber);
                    break;
            }
        }
    }
    //Solo ver 2040
    public class LearRep830
    {
        static UInt16 RepType { set; get; } = 0;
        public LearIsa LearIsaO { get; set; }
        static string ControlNumber { set; get; } = "000000001";
        public ISA ISAO { get; set; }
        public GS GSO { get; set; }
        public ST STO { get; set; }
        public LearRep830(UInt16 _RepType, int _ControlNumber, LearIsa _LearIsaO)
        {
            RepType = _RepType;
            ControlNumber = $"{_ControlNumber:D9}";
            LearIsaO = _LearIsaO;
            ISAO = new ISA(RepType, ControlNumber)
            {
                AuthorizationInformationQualifier = LearIsaO.AuthorizationInformationQualifier,
                AuthorizationInformation = LearIsaO.AuthorizationInformation,
                SecurityInformationQualifier = LearIsaO.SecurityInformationQualifier,
                SecurityInformation = LearIsaO.SecurityInformation,
                InterchangeSenderIdQualifier = LearIsaO.InterchangeSenderIdQualifier,
                InterchangeSenderId = LearIsaO.InterchangeSenderId,
                InterchangeReceiverIdQualifier = LearIsaO.InterchangeReceiverIdQualifier,
                InterchangeReceiverId = LearIsaO.InterchangeReceiverId,
                InterchangeDate = DateTime.Now.ToString("yyMMdd"),
                InterchangeTime = DateTime.Now.ToString("HHmm"),
                InterchangeControlStandardsId = LearIsaO.InterchangeControlStandardsId,
                InterchangeControlVersion = LearIsaO.InterchangeControlVersion,
                AcknowledgmentRequested = LearIsaO.AcknowledgmentRequested,
                UsageIndicator = LearIsaO.UsageIndicator,
            };
            GSO = new GS(RepType, ControlNumber);
            STO = new ST(RepType, ControlNumber);
        }
        public override string ToString()
        {
            string Ret = string.Empty;
            Ret += ISAO.Ts();
            Ret += GSO.Ts();
            Ret += STO.Ts();
            Ret += STO.StTrailerO.Ts();
            Ret += GSO.GSTrailerO.Ts();
            Ret += ISAO.ISATrailerO.Ts();
            return Ret;
        }
    }
}
