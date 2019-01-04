using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public static class EdiCommon
    {
        public static string SegmentTerminator { get; set; } = "~";
        public static string ElementTerminator { get; set; } = "*";
        public static string CompositeTerminator { get; set; } = ">";
        public static string Ts(object O, IEnumerable<string> Orden)
        {
            string Ret = string.Empty;
            foreach (string OrdenO in Orden)
                Ret += O.GetType().GetProperty(OrdenO).GetValue(O, null) + EdiCommon.ElementTerminator;
            Ret = Ret.TrimEnd(EdiCommon.ElementTerminator[0]) + EdiCommon.SegmentTerminator + Environment.NewLine;
            return Ret;
        }
    }

    public class ISA
    {
        public string AuthorizationInformationQualifier { get; set; } = "0";
        public string AuthorizationInformation { get; set; } = "1";
        public string SecurityInformationQualifier { get; set; } = "2";
        public string SecurityInformation { get; set; } = "3";
        public string InterchangeSenderIdQualifier { get; set; } = "4";
        public string InterchangeSenderId { get; set; } = "5";
        public string InterchangeReceiverIdQualifier { get; set; } = "6";
        public string InterchangeReceiverId { get; set; } = "7";
        public string InterchangeDate { get; set; } = "8";
        public string InterchangeTime { get; set; } = "9";
        public string InterchangeControlStandardsId { get; set; } = "0";
        public string InterchangeControlVersion { get; set; } = "a";
        public string InterchangeControlNumber { get; set; } = "b";
        public string AcknowledgmentRequested { get; set; } = "c";
        public string UsageIndicator { get; set; } = "d";
        public string ComponentElementSeparator { get; set; } = ">";
        public string ISATrailer { get; set; } = "IEA*Algo" + Environment.NewLine;
        public readonly IEnumerable<string> Orden = new string[]{
            "AuthorizationInformationQualifier", "AuthorizationInformation",
            "SecurityInformationQualifier", "SecurityInformation",
            "InterchangeSenderIdQualifier", "InterchangeSenderId",
            "InterchangeReceiverIdQualifier", "InterchangeReceiverId",
            "InterchangeDate", "InterchangeTime",
            "InterchangeControlStandardsId", "InterchangeControlVersion",
            "InterchangeControlNumber", "AcknowledgmentRequested",
            "UsageIndicator", "ComponentElementSeparator"
        };

        public override string ToString()
        {            
            return EdiCommon.Ts(this, Orden);
        }
    }

    public class GS
    {
        public string FunctionalIdCode { get; set; } = "0";
        public string ApplicationSenderCode { get; set; } = "1";
        public string ApplicationReceiverCode { get; set; } = "2";
        public string GsDate { get; set; } = "3";
        public string GsTime { get; set; } = "4";
        public string GroupControlNumber { get; set; } = "5";
        public string ResponsibleAgencyCode { get; set; } = "6";
        public string Version { get; set; } = "7";
        public string GsTrailer { get; set; } = "GE*Algo" + Environment.NewLine;
        public readonly IEnumerable<string> Orden = new string[]{
            "FunctionalIdCode", "ApplicationSenderCode",
            "ApplicationReceiverCode", "GsDate",
            "GsTime", "GroupControlNumber",
            "ResponsibleAgencyCode", "Version"            
        };

        public override string ToString()
        {
            return EdiCommon.Ts(this, Orden);
        }
    }

    public class StTrailer
    {
        public string SegmentCount { get; set; } = "0";
        public string ControlNumber { get; set; } = "1";
        public readonly IEnumerable<string> Orden = new string[]{
            "SegmentCount", "ControlNumber"
        };

        public StTrailer(UInt16 RepType, string _ControlNumber)
        {
            switch (RepType)
            {
                case 0:
                    ControlNumber = _ControlNumber;
                    break;
            }
        }

        public override string ToString()
        {
            return EdiCommon.Ts(this, Orden);
        }
    }

    public class ST
    {
        public UInt16 RepType { get; set; }
        public string IdCode { get; set; } = "0";
        public string ControlNumber { get; set; } = "1";
        public StTrailer StTrailerO { get; set; }
        public readonly IEnumerable<string> Orden = new string[]{
            "IdCode", "ControlNumber"
        };

        public ST(UInt16 _RepType)
        {
            RepType = _RepType;            
            switch (_RepType) {
                case 0:
                    IdCode = "830";
                    ControlNumber = "0001";
                    StTrailerO = new StTrailer(RepType, ControlNumber);
                    break;
            }
        }

        public override string ToString()
        {
            return EdiCommon.Ts(this, Orden);
        }
    }

    public class LearRep830
    {        
        public ISA ISAO { get; set; } = new ISA();
        public GS GSO { get; set; } = new GS();
        public ST STO { get; set; } = new ST(0);
        public override string ToString()
        {
            string Ret = string.Empty;
            Ret += ISAO.ToString();
            Ret += GSO.ToString();
            Ret += STO.ToString();
            Ret += STO.StTrailerO.ToString();
            Ret += GSO.GsTrailer;
            Ret += ISAO.ISATrailer;
            return Ret;
        }
    }
}
