using EdiApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    //Solo ver 2040
    public class LearRep830
    {
        static UInt16 RepType { set; get; } = 0;
        public LearIsa LearIsaO { get; set; }
        public LearGs LearGsO { get; set; }
        public LearBfr LearBfrO { get; set; }
        static string ControlNumber { set; get; } = "000000001";
        public ISA830 ISAO { get; set; }
        public GS830 GSO { get; set; }
        public ST830 STO { get; set; }
        public BFR830 BFRO { get; set; }
        public LIN830 LINO { get; set; }
        public UIT830 UITO { get; set; }
        public PRS830 PRSO { get; set; }
        public N1830 N1O { get; set; }
        public N4830 N4O { get; set; }
        public SDP830 SDPO { get; set; }
        public FST830 FSTO { get; set; }
        public ATH830 ATHO { get; set; }
        public SHP830 SHPO { get; set; }
        public NTE830 NTEO { get; set; }
        public CTT830 CTTO { get; set; }
        public LearRep830(UInt16 _RepType, int _ControlNumber, ref LearIsa _LearIsaO, ref LearGs _LearGsO, ref LearBfr _LearBfrO)
        {
            RepType = _RepType;
            ControlNumber = $"{_ControlNumber:D9}";
            LearIsaO = _LearIsaO;
            LearGsO = _LearGsO;
            LearBfrO = _LearBfrO;
            ISAO = new ISA830(RepType, LearIsaO.SegmentTerminator, ControlNumber)
            {
                AuthorizationInformationQualifier = LearIsaO.AuthorizationInformationQualifier,
                AuthorizationInformation = LearIsaO.AuthorizationInformation,
                SecurityInformationQualifier = LearIsaO.SecurityInformationQualifier,
                SecurityInformation = LearIsaO.SecurityInformation,
                InterchangeSenderIdQualifier = LearIsaO.InterchangeSenderIdQualifier,
                InterchangeSenderId = LearIsaO.InterchangeSenderId,
                InterchangeReceiverIdQualifier = LearIsaO.InterchangeReceiverIdQualifier,
                InterchangeReceiverId = LearIsaO.InterchangeReceiverId,
                InterchangeDate = DateTime.Now.ToString(LearIsaO.InterchangeDate),
                InterchangeTime = DateTime.Now.ToString(LearIsaO.InterchangeTime),
                InterchangeControlStandardsId = LearIsaO.InterchangeControlStandardsId,
                InterchangeControlVersion = LearIsaO.InterchangeControlVersion,
                AcknowledgmentRequested = LearIsaO.AcknowledgmentRequested,
                UsageIndicator = LearIsaO.UsageIndicator,
            };
            GSO = new GS830(RepType, LearIsaO.SegmentTerminator, ControlNumber) {
                FunctionalIdCode = LearGsO.FunctionalIdCode,
                ApplicationSenderCode = LearGsO.ApplicationSenderCode,
                ApplicationReceiverCode = LearGsO.ApplicationReceiverCode,
                GsDate = DateTime.Now.ToString(LearGsO.GsDate),
                GsTime = DateTime.Now.ToString(LearGsO.GsTime),
                ResponsibleAgencyCode = LearGsO.ResponsibleAgencyCode,
                Version = LearGsO.Version
            };
            STO = new ST830(RepType, LearIsaO.SegmentTerminator, ControlNumber);
            LearBfrO.TransactionSetPurposeCode = "00";
            LearBfrO.ForecastOrderNumber = "";
            LearBfrO.ReleaseNumber = "0000";
            LearBfrO.ForecastTypeQualifier = "SH";
            LearBfrO.ForecastQuantityQualifier = "C";
            LearBfrO.ForecastHorizonStart = DateTime.Now.AddDays(-7).ToString(LearIsaO.InterchangeDate);
            LearBfrO.ForecastHorizonEnd = DateTime.Now.ToString(LearIsaO.InterchangeDate);
            LearBfrO.ForecastGenerationDate = DateTime.Now.ToString(LearIsaO.InterchangeDate);
            LearBfrO.ForecastUpdatedDate = DateTime.Now.ToString(LearIsaO.InterchangeDate);
            LearBfrO.ContractNumber = "";
            LearBfrO.PurchaseOrderNumber = "";
            LearBfrO.Time = DateTime.Now.ToString(LearIsaO.InterchangeTime);
            BFRO = new BFR830(LearIsaO.SegmentTerminator) {
                TransactionSetPurposeCode = LearBfrO.TransactionSetPurposeCode,
                ForecastOrderNumber = LearBfrO.ForecastOrderNumber,
                ReleaseNumber = LearBfrO.ReleaseNumber,
                ForecastTypeQualifier = LearBfrO.ForecastTypeQualifier,
                ForecastQuantityQualifier = LearBfrO.ForecastQuantityQualifier,
                ForecastHorizonStart = LearBfrO.ForecastHorizonStart,
                ForecastHorizonEnd = LearBfrO.ForecastHorizonEnd,
                ForecastGenerationDate = LearBfrO.ForecastGenerationDate,
                ForecastUpdatedDate = LearBfrO.ForecastUpdatedDate,
                ContractNumber = LearBfrO.ContractNumber,
                PurchaseOrderNumber = LearBfrO.PurchaseOrderNumber                
            };
            LINO = new LIN830(LearIsaO.SegmentTerminator) {
                AssignedIdentification = "",
                ProductIdQualifier = "BP",
                ProductId = "",
                ProductRefIdQualifier = "RF",
                ProductRefId = "",
                ProductPurchaseIdQualifier = "PO",
                ProductPurchaseId = ""
            };
            UITO = new UIT830(LearIsaO.SegmentTerminator) {
                UnitOfMeasure = "EA"
            };
            PRSO = new PRS830(LearIsaO.SegmentTerminator) {
                StatusCode = "7"
            };
            N1O = new N1830(LearIsaO.SegmentTerminator) {
                OrganizationId = "ST", // ST o VN
                Name = "", // ship to name
                IdCodeQualifier = "92", //6 o 92
                IdCode = "Avery?" //Plant code, lear
            };
            N4O = new N4830(LearIsaO.SegmentTerminator) {
                LocationQualifier = "PL",
                LocationId = "123" // 3 digits
            };
            SDPO = new SDP830(LearIsaO.SegmentTerminator)
            {
                CalendarPatternCode = "", // ????
                PatternTimeCode = "A" // A, F, G, Y ????
            };
            FSTO = new FST830(LearIsaO.SegmentTerminator)
            {
                Quantity = "",
                ForecastQualifier = "C",
                ForecastTimingQualifier = "W",
                FstDate = ""
            };
            ATHO = new ATH830(LearIsaO.SegmentTerminator) {
                ResourceAuthCode = "MT",
                StartDate = DateTime.Now.AddDays(-7).ToString(LearIsaO.InterchangeDate),
                Quantity = "0.0",
                EndDate = DateTime.Now.ToString(LearIsaO.InterchangeDate)
            };
            SHPO = new SHP830(LearIsaO.SegmentTerminator) {
                QuantityQualifier = "01",
                Quantity = "01",
                DateTimeQualifier = "011",
                AccumulationStartDate = DateTime.Now.AddDays(-7).ToString(LearIsaO.InterchangeDate), // ojo
                AccumulationTime = "",
                AccumulationEndDate = "" //Solo si SHP03 = 051
            };
            NTEO = new NTE830(LearIsaO.SegmentTerminator) {
                Message = "Free message"
            };
            CTTO = new CTT830(LearIsaO.SegmentTerminator) {
                HashTotal = "1"
            };            
        }
        public override string ToString()
        {
            string Ret = string.Empty;
            Ret += ISAO.Ts();
            Ret += GSO.Ts();
            Ret += STO.Ts();
            Ret += BFRO.Ts();
            Ret += LINO.Ts();
            Ret += UITO.Ts();
            Ret += PRSO.Ts();
            Ret += N1O.Ts();
            Ret += N4O.Ts();
            Ret += SDPO.Ts();
            Ret += FSTO.Ts();
            Ret += ATHO.Ts();
            Ret += SHPO.Ts();
            Ret += NTEO.Ts();
            Ret += CTTO.Ts();
            Ret += STO.StTrailerO.Ts();
            Ret += GSO.GSTrailerO.Ts();
            Ret += ISAO.ISATrailerO.Ts();
            return Ret;
        }
    }
}
