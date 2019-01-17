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
        public List<string> EdiFile { get; set; } = new List<string>();
        static string EdiError { set; get; } = "";
        public LearIsa LearIsaO { get; set; }
        public LearGs LearGsO { get; set; }
        public LearBfr LearBfrO { get; set; }
        static string ControlNumber { set; get; } = "000000001";
        public ISA830 ISAO { get; set; } = new ISA830(EdiBase.SegmentTerminator);
        public GS830 GSO { get; set; } = new GS830(EdiBase.SegmentTerminator);
        public ST830 STO { get; set; } = new ST830(EdiBase.SegmentTerminator);
        public BFR830 BFRO { get; set; } = new BFR830(EdiBase.SegmentTerminator);
        public LIN830 LINO { get; set; }
        public List<LIN830> ListLIN { get; set; } = new List<LIN830>();
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
        public LearRep830() { }
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
            EdiError = CTTO.Validate();
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
        private string ParseMenError1(string _TypeStr, int _Nr, int _Di)
        {
            return $"El segmento {_TypeStr} tiene errores en linea {_Nr} y columna {_Di}";
        }
        private string ParseMenError2(string _TypeStr, string _TypeMissingStr, int _Nr)
        {
            return $"Error al analizar segmento {_TypeStr}. No existe {_TypeMissingStr}. Error en linea {_Nr}";
        }
        public string Parse()
        {
            string Ident = "";
            ListLIN.Clear();
            ISA830 ISAO = new ISA830(EdiBase.SegmentTerminator);
            GS830 GSO = new GS830(EdiBase.SegmentTerminator);
            ST830 STO = new ST830(EdiBase.SegmentTerminator);
            string EdiStr = string.Empty;
            for (int Nr = 0; Nr < EdiFile.Count; Nr++)
            {
                EdiStr = EdiFile[Nr];
                Ident = EdiStr.IndexOf(EdiBase.ElementTerminator) > 0 ? EdiStr.Substring(0, EdiStr.IndexOf(EdiBase.ElementTerminator)) : string.Empty;
                if (Ident != string.Empty)
                {
                    switch (Ident)
                    {
                        case ISA830.Init:
                            if (!ISAO.Parse(EdiStr))
                                return ParseMenError1(ISA830.Self, Nr, ISAO.Di);
                            break;
                        case GS830.Init:
                            if (string.IsNullOrEmpty(ISAO.EdiStr))
                                return ParseMenError2(GS830.Self, ISA830.Self, Nr);
                            if (!GSO.Parse(EdiStr))
                                return ParseMenError1(GS830.Self, Nr, GSO.Di);
                            ISAO.Childs.Add(GSO);
                            break;
                        case ST830.Init:
                            if (string.IsNullOrEmpty(GSO.EdiStr))
                                return ParseMenError2(ST830.Self, GS830.Self, Nr);
                            if (!STO.Parse(EdiStr))
                                return ParseMenError1(ST830.Self, Nr, STO.Di);
                            ISAO.Childs.Add(STO);
                            break;
                        case BFR830.Init:
                            if (string.IsNullOrEmpty(STO.EdiStr))
                                return ParseMenError2(BFR830.Self, ST830.Self, Nr);
                            BFR830 BFRnp = new BFR830(EdiBase.SegmentTerminator);
                            if (!BFRnp.Parse(EdiStr))
                                return ParseMenError1(BFR830.Self, Nr, BFRnp.Di);
                            STO.Childs.Add(BFRnp);
                            break;
                        case NTE830.Init:
                            if (ListLIN.Count == 0)
                            {
                                if (string.IsNullOrEmpty(STO.EdiStr))
                                    return ParseMenError2(NTE830.Self, ST830.Self, Nr);
                                NTE830 NTEnp = new NTE830(EdiBase.SegmentTerminator);
                                if (!NTEnp.Parse(EdiStr))
                                    return ParseMenError1(NTE830.Self, Nr, NTEnp.Di);
                                STO.Childs.Add(NTEnp);
                            } else
                            {
                                if (string.IsNullOrEmpty(ListLIN.LastOrDefault().EdiStr))
                                    return ParseMenError2(NTE830.Self, LIN830.Self, Nr);
                                NTE830 NTEnp = new NTE830(EdiBase.SegmentTerminator);
                                if (!NTEnp.Parse(EdiStr))
                                    return ParseMenError1(NTE830.Self, Nr, NTEnp.Di);
                                ListLIN.AddLastChild(NTEnp);
                            }
                            break;
                        case N1830.Init:
                            if (ListLIN.Count == 0)
                            {
                                if (string.IsNullOrEmpty(STO.EdiStr))
                                    return ParseMenError2(N1830.Self, ST830.Self, Nr);
                                N1830 N1np = new N1830(EdiBase.SegmentTerminator);
                                if (!N1np.Parse(EdiStr))
                                    return ParseMenError1(N1830.Self, Nr, N1np.Di);
                                STO.Childs.Add(N1np);
                            } else
                            {
                                if (string.IsNullOrEmpty(ListLIN.LastOrDefault().EdiStr))
                                    return ParseMenError2(N1830.Self, LIN830.Self, Nr);
                                N1830 N1np = new N1830(EdiBase.SegmentTerminator);
                                if (!N1np.Parse(EdiStr))
                                    return ParseMenError1(N1830.Self, Nr, N1np.Di);
                                ListLIN.AddLastChild(N1np);
                            }
                            break;
                        case N4830.Init:
                            if (ListLIN.Count == 0)
                            {
                                if (string.IsNullOrEmpty(STO.EdiStr))
                                    return ParseMenError2(N4830.Self, ST830.Self, Nr);
                                N4830 N4np = new N4830(EdiBase.SegmentTerminator);
                                if (!N4np.Parse(EdiStr))
                                    return ParseMenError1(N4830.Self, Nr, N4np.Di);
                                STO.Childs.Add(N4np);
                            } else
                            {
                                if (string.IsNullOrEmpty(ListLIN.LastOrDefault().EdiStr))
                                    return ParseMenError2(N4830.Self, LIN830.Self, Nr);
                                N4830 N4np = new N4830(EdiBase.SegmentTerminator);
                                if (!N4np.Parse(EdiStr))
                                    return ParseMenError1(N4830.Self, Nr, N4np.Di);
                                ListLIN.AddLastChild(N4np);
                            }
                            break;
                        case LIN830.Init:
                            if (string.IsNullOrEmpty(STO.EdiStr))
                                return ParseMenError2(LIN830.Self, ST830.Self, Nr);
                            LIN830 LINnp = new LIN830(EdiBase.SegmentTerminator);
                            if (!LINnp.Parse(EdiStr))
                                return ParseMenError1(LIN830.Self, Nr, LINnp.Di);
                            STO.Childs.Add(LINnp);
                            ListLIN.Add(LINnp);
                            break;
                        case UIT830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(UIT830.Self, LIN830.Self, Nr);
                            UIT830 UITnp = new UIT830(EdiBase.SegmentTerminator);
                            if (!UITnp.Parse(EdiStr))
                                return ParseMenError1(UIT830.Self, Nr, UITnp.Di);
                            ListLIN.AddLastChild(UITnp);
                            break;
                        case PRS830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(PRS830.Self, LIN830.Self, Nr);
                            PRS830 PRSnp = new PRS830(EdiBase.SegmentTerminator);
                            if (!PRSnp.Parse(EdiStr))
                                return ParseMenError1(PRS830.Self, Nr, PRSnp.Di);
                            ListLIN.AddLastChild(PRSnp);
                            break;
                        case FST830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(FST830.Self, LIN830.Self, Nr);
                            FST830 FSTnp = new FST830(EdiBase.SegmentTerminator);
                            if (!FSTnp.Parse(EdiStr))
                                return ParseMenError1(FST830.Self, Nr, FSTnp.Di);
                            ListLIN.AddLastChild(FSTnp);
                            break;
                        case SDP830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(SDP830.Self, LIN830.Self, Nr);
                            SDP830 SDPnp = new SDP830(EdiBase.SegmentTerminator);
                            if (!SDPnp.Parse(EdiStr))
                                return ParseMenError1(SDP830.Self, Nr, SDPnp.Di);
                            ListLIN.AddLastChild(SDPnp);
                            break;
                        case ATH830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(ATH830.Self, LIN830.Self, Nr);
                            ATH830 ATHnp = new ATH830(EdiBase.SegmentTerminator);
                            if (!ATHnp.Parse(EdiStr))
                                return ParseMenError1(ATH830.Self, Nr, ATHnp.Di);
                            ListLIN.AddLastChild(ATHnp);
                            break;
                        case SHP830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(SHP830.Self, LIN830.Self, Nr);
                            SHP830 SHPnp = new SHP830(EdiBase.SegmentTerminator);
                            if (!SHPnp.Parse(EdiStr))
                                return ParseMenError1(SHP830.Self, Nr, SHPnp.Di);
                            ListLIN.AddLastChild(SHPnp);
                            break;
                        default:
                            break;
                    }
                }
            }
            return string.Empty;
        }
    }
}
