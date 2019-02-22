using EdiApi.Models;
using EdiApi.Models.EdiDB;
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
        static int CheckSeg { set; get; } = 0;
        public List<string> EdiFile { get; set; } = new List<string>();
        static string EdiError { set; get; } = "";
        public LearIsa830 LearIsaO { get; set; }
        public LearGs830 LearGsO { get; set; }
        public LearBfr830 LearBfrO { get; set; }
        static string ControlNumber { set; get; } = "000000001";
        public ISA830 ISAO { get; set; } = new ISA830(EdiBase.SegmentTerminator);
        public static LearIsa830 LearIsa830root { set; get; }
        public GS830 GSO { get; set; } = new GS830(EdiBase.SegmentTerminator);
        public ST830 STO { get; set; } = new ST830(EdiBase.SegmentTerminator);
        public BFR830 BFRO { get; set; } = new BFR830(EdiBase.SegmentTerminator);
        public LIN830 LINO { get; set; }
        public List<LIN830> ListLIN { get; set; } = new List<LIN830>();
        public List<SDP830> ListSDP { get; set; } = new List<SDP830>();
        public List<SHP830> ListSHP { get; set; } = new List<SHP830>();
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
        private EdiDBContext DbO;
        private Models.WmsDB.WmsContext WmsDb;
        public LearPureEdi LearPureEdiO { get; set; }
        public LearRep830(ref EdiDBContext _DbO) { DbO = _DbO; }
        public LearRep830(ref EdiDBContext _DbO, ref Models.WmsDB.WmsContext _WmsDB) { DbO = _DbO; WmsDb = _WmsDB; }
        public LearRep830(UInt16 _RepType, int _ControlNumber, ref LearIsa830 _LearIsaO, ref LearGs830 _LearGsO, ref LearBfr830 _LearBfrO)
        {
            RepType = _RepType;
            ControlNumber = $"{_ControlNumber:D9}";
            LearIsaO = _LearIsaO;
            LearGsO = _LearGsO;
            LearBfrO = _LearBfrO;
            ISAO = new ISA830(RepType, ISA830.SegmentTerminator, ControlNumber)
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
            GSO = new GS830(RepType, ISA830.SegmentTerminator, ControlNumber) {
                FunctionalIdCode = LearGsO.FunctionalIdCode,
                ApplicationSenderCode = LearGsO.ApplicationSenderCode,
                ApplicationReceiverCode = LearGsO.ApplicationReceiverCode,
                GsDate = DateTime.Now.ToString(LearGsO.GsDate),
                GsTime = DateTime.Now.ToString(LearGsO.GsTime),
                ResponsibleAgencyCode = LearGsO.ResponsibleAgencyCode,
                Version = LearGsO.Version
            };
            STO = new ST830(RepType, ISA830.SegmentTerminator, ControlNumber);
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
            //LearBfrO.Time = DateTime.Now.ToString(LearIsaO.InterchangeTime);
            BFRO = new BFR830(ISA830.SegmentTerminator) {
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
            LINO = new LIN830(ISA830.SegmentTerminator) {
                AssignedIdentification = "",
                ProductIdQualifier = "BP",
                ProductId = "",
                ProductRefIdQualifier = "RF",
                ProductRefId = "",
                ProductPurchaseIdQualifier = "PO",
                ProductPurchaseId = ""
            };
            UITO = new UIT830(ISA830.SegmentTerminator) {
                UnitOfMeasure = "EA"
            };
            PRSO = new PRS830(ISA830.SegmentTerminator) {
                StatusCode = "7"
            };
            N1O = new N1830(ISA830.SegmentTerminator) {
                OrganizationId = "ST", // ST o VN
                Name = "", // ship to name
                IdCodeQualifier = "92", //6 o 92
                IdCode = "Avery?" //Plant code, lear
            };
            N4O = new N4830(ISA830.SegmentTerminator) {
                LocationQualifier = "PL",
                LocationId = "123" // 3 digits
            };
            SDPO = new SDP830(ISA830.SegmentTerminator)
            {
                CalendarPatternCode = "", // ????
                PatternTimeCode = "A" // A, F, G, Y ????
            };
            FSTO = new FST830(ISA830.SegmentTerminator)
            {
                Quantity = "",
                ForecastQualifier = "C",
                ForecastTimingQualifier = "W",
                FstDate = ""
            };
            ATHO = new ATH830(ISA830.SegmentTerminator) {
                ResourceAuthCode = "MT",
                StartDate = DateTime.Now.AddDays(-7).ToString(LearIsaO.InterchangeDate),
                Quantity = "0.0",
                EndDate = DateTime.Now.ToString(LearIsaO.InterchangeDate)
            };
            SHPO = new SHP830(ISA830.SegmentTerminator) {
                QuantityQualifier = "01",
                Quantity = "01",
                DateTimeQualifier = "011",
                AccumulationStartDate = DateTime.Now.AddDays(-7).ToString(LearIsaO.InterchangeDate), // ojo
                AccumulationTime = "",
                AccumulationEndDate = "" //Solo si SHP03 = 051
            };
            NTEO = new NTE830(ISA830.SegmentTerminator) {
                Message = "Free message"
            };
            CTTO = new CTT830(ISA830.SegmentTerminator) {
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
            ListSDP.Clear();
            ListSHP.Clear();
            ISAO = new ISA830(EdiBase.SegmentTerminator);
            GSO = new GS830(EdiBase.SegmentTerminator);
            STO = new ST830(EdiBase.SegmentTerminator);
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
                                return ParseMenError1(ISA830.Self, Nr, ISAO.Coli);
                            break;
                        case GS830.Init:
                            if (string.IsNullOrEmpty(ISAO.EdiStr))
                                return ParseMenError2(GS830.Self, ISA830.Self, Nr);
                            if (!GSO.Parse(EdiStr))
                                return ParseMenError1(GS830.Self, Nr, GSO.Coli);
                            GSO.Parent = ISAO;
                            ISAO.Childs.Add(GSO);
                            break;
                        case ST830.Init:
                            if (string.IsNullOrEmpty(GSO.EdiStr))
                                return ParseMenError2(ST830.Self, GS830.Self, Nr);
                            if (!STO.Parse(EdiStr))
                                return ParseMenError1(ST830.Self, Nr, STO.Coli);
                            STO.Parent = ISAO;
                            ISAO.Childs.Add(STO);
                            break;
                        case BFR830.Init:
                            if (string.IsNullOrEmpty(STO.EdiStr))
                                return ParseMenError2(BFR830.Self, ST830.Self, Nr);
                            BFR830 BFRnp = new BFR830(EdiBase.SegmentTerminator);
                            if (!BFRnp.Parse(EdiStr))
                                return ParseMenError1(BFR830.Self, Nr, BFRnp.Coli);
                            BFRnp.Parent = STO;
                            STO.Childs.Add(BFRnp);
                            break;
                        case NTE830.Init:
                            if (ListLIN.Count == 0)
                            {
                                if (string.IsNullOrEmpty(STO.EdiStr))
                                    return ParseMenError2(NTE830.Self, ST830.Self, Nr);
                                NTE830 NTEnp = new NTE830(EdiBase.SegmentTerminator);
                                if (!NTEnp.Parse(EdiStr))
                                    return ParseMenError1(NTE830.Self, Nr, NTEnp.Coli);
                                NTEnp.Parent = STO;
                                STO.Childs.Add(NTEnp);
                            } else
                            {
                                if (string.IsNullOrEmpty(ListLIN.LastOrDefault().EdiStr))
                                    return ParseMenError2(NTE830.Self, LIN830.Self, Nr);
                                NTE830 NTEnp = new NTE830(EdiBase.SegmentTerminator);
                                if (!NTEnp.Parse(EdiStr))
                                    return ParseMenError1(NTE830.Self, Nr, NTEnp.Coli);
                                NTEnp.Parent = ListLIN.LastOrDefault();
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
                                    return ParseMenError1(N1830.Self, Nr, N1np.Coli);
                                N1np.Parent = STO;
                                STO.Childs.Add(N1np);
                            } else
                            {
                                if (string.IsNullOrEmpty(ListLIN.LastOrDefault().EdiStr))
                                    return ParseMenError2(N1830.Self, LIN830.Self, Nr);
                                N1830 N1np = new N1830(EdiBase.SegmentTerminator);
                                if (!N1np.Parse(EdiStr))
                                    return ParseMenError1(N1830.Self, Nr, N1np.Coli);
                                N1np.Parent = ListLIN.LastOrDefault();
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
                                    return ParseMenError1(N4830.Self, Nr, N4np.Coli);
                                N4np.Parent = STO;
                                STO.Childs.Add(N4np);
                            } else
                            {
                                if (string.IsNullOrEmpty(ListLIN.LastOrDefault().EdiStr))
                                    return ParseMenError2(N4830.Self, LIN830.Self, Nr);
                                N4830 N4np = new N4830(EdiBase.SegmentTerminator);
                                if (!N4np.Parse(EdiStr))
                                    return ParseMenError1(N4830.Self, Nr, N4np.Coli);
                                N4np.Parent = ListLIN.LastOrDefault();
                                ListLIN.AddLastChild(N4np);
                            }
                            break;
                        case LIN830.Init:
                            if (string.IsNullOrEmpty(STO.EdiStr))
                                return ParseMenError2(LIN830.Self, ST830.Self, Nr);
                            LIN830 LINnp = new LIN830(EdiBase.SegmentTerminator);
                            if (!LINnp.Parse(EdiStr))
                                return ParseMenError1(LIN830.Self, Nr, LINnp.Coli);
                            LINnp.Parent = STO;
                            STO.Childs.Add(LINnp);
                            ListLIN.Add(LINnp);
                            break;
                        case UIT830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(UIT830.Self, LIN830.Self, Nr);
                            UIT830 UITnp = new UIT830(EdiBase.SegmentTerminator);
                            if (!UITnp.Parse(EdiStr))
                                return ParseMenError1(UIT830.Self, Nr, UITnp.Coli);
                            UITnp.Parent = ListLIN.LastOrDefault();
                            ListLIN.AddLastChild(UITnp);
                            break;
                        case PRS830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(PRS830.Self, LIN830.Self, Nr);
                            PRS830 PRSnp = new PRS830(EdiBase.SegmentTerminator);
                            if (!PRSnp.Parse(EdiStr))
                                return ParseMenError1(PRS830.Self, Nr, PRSnp.Coli);
                            PRSnp.Parent = ListLIN.LastOrDefault();
                            ListLIN.AddLastChild(PRSnp);
                            break;
                        case SDP830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(SDP830.Self, LIN830.Self, Nr);                            
                            SDP830 SDPnp = new SDP830(EdiBase.SegmentTerminator);
                            if (!SDPnp.Parse(EdiStr))
                                return ParseMenError1(SDP830.Self, Nr, SDPnp.Coli);
                            SDPnp.Parent = ListLIN.LastOrDefault();
                            ListLIN.AddLastChild(SDPnp);
                            ListSDP.Add(SDPnp);
                            break;
                        case FST830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(FST830.Self, LIN830.Self, Nr);
                            FST830 FSTnp = new FST830(EdiBase.SegmentTerminator);
                            if (!FSTnp.Parse(EdiStr))
                                return ParseMenError1(FST830.Self, Nr, FSTnp.Coli);
                            FSTnp.RealQty = "0";
                            if (ListSDP.Count == 0)
                            {
                                FSTnp.Parent = ListLIN.LastOrDefault();
                                ListLIN.AddLastChild(FSTnp);
                            }
                            else
                            {
                                FSTnp.Parent = ListSDP.LastOrDefault();
                                ListSDP.AddLastChild(FSTnp);
                            }
                            break;
                        case ATH830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(ATH830.Self, LIN830.Self, Nr);
                            ATH830 ATHnp = new ATH830(EdiBase.SegmentTerminator);
                            if (!ATHnp.Parse(EdiStr))
                                return ParseMenError1(ATH830.Self, Nr, ATHnp.Coli);
                            ATHnp.Parent = ListLIN.LastOrDefault();
                            ListLIN.AddLastChild(ATHnp);
                            break;
                        case SHP830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(SHP830.Self, LIN830.Self, Nr);
                            SHP830 SHPnp = new SHP830(EdiBase.SegmentTerminator);
                            if (!SHPnp.Parse(EdiStr))
                                return ParseMenError1(SHP830.Self, Nr, SHPnp.Coli);
                            SHPnp.Parent = ListLIN.LastOrDefault();
                            ListLIN.AddLastChild(SHPnp);
                            ListSHP.Add(SHPnp);
                            break;
                        case REF830.Init:
                            if (ListLIN.Count == 0)
                                return ParseMenError2(REF830.Self, LIN830.Self, Nr);
                            REF830 REFnp = new REF830(EdiBase.SegmentTerminator);
                            if (!REFnp.Parse(EdiStr))
                                return ParseMenError1(REF830.Self, Nr, REFnp.Coli);
                            if (ListSHP.Count == 0)
                            {
                                REFnp.Parent = ListLIN.LastOrDefault();
                                ListLIN.AddLastChild(REFnp);
                            }
                            else
                            {
                                REFnp.Parent = ListSHP.LastOrDefault();
                                ListSHP.AddLastChild(REFnp);
                            }
                            break;
                        case CTT830.Init:
                            if (string.IsNullOrEmpty(ISAO.EdiStr))
                                return ParseMenError2(CTT830.Self, ISA830.Self, Nr);
                            CTT830 CTTnp = new CTT830(EdiBase.SegmentTerminator);
                            if (!CTTnp.Parse(EdiStr))
                                return ParseMenError1(CTT830.Self, Nr, CTTnp.Coli);
                            CTTnp.Parent = ISAO;
                            ISAO.Childs.Add(CTTnp);
                            break;
                        case SE830.Init:
                            if (string.IsNullOrEmpty(ISAO.EdiStr))
                                return ParseMenError2(SE830.Self, ISA830.Self, Nr);
                            SE830 SEnp = new SE830(EdiBase.SegmentTerminator);
                            if (!SEnp.Parse(EdiStr))
                                return ParseMenError1(SE830.Self, Nr, SEnp.Coli);
                            SEnp.Parent = ISAO;
                            ISAO.Childs.Add(SEnp);
                            break;
                        case GE830.Init:
                            if (string.IsNullOrEmpty(ISAO.EdiStr))
                                return ParseMenError2(GE830.Self, ISA830.Self, Nr);
                            GE830 GEnp = new GE830(EdiBase.SegmentTerminator);
                            if (!GEnp.Parse(EdiStr))
                                return ParseMenError1(GE830.Self, Nr, GEnp.Coli);
                            GEnp.Parent = ISAO;
                            ISAO.Childs.Add(GEnp);
                            break;
                        case IEA830.Init:
                            if (string.IsNullOrEmpty(ISAO.EdiStr))
                                return ParseMenError2(IEA830.Self, ISA830.Self, Nr);
                            IEA830 IEAnp = new IEA830(EdiBase.SegmentTerminator);
                            if (!IEAnp.Parse(EdiStr))
                                return ParseMenError1(IEA830.Self, Nr, IEAnp.Coli);
                            IEAnp.Parent = ISAO;
                            ISAO.Childs.Add(IEAnp);
                            break;
                        default:
                            return ParseMenError2("(desconocido)", EdiStr, Nr);
                    }
                }
            }
            return string.Empty;
        }        

        public void SaveEdiPure(ref string _EdiPure, string _FileName, int _CheckSeg)
        {
            LearPureEdiO = new LearPureEdi()
            {
                HashId = EdiBase.GetHashId(),
                EdiStr = _EdiPure,
                Fingreso = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                Reprocesar = true,
                NombreArchivo = _FileName,
                CheckSeg = _CheckSeg,
                InOut = "I"
            };
            DbO.LearPureEdi.Add(LearPureEdiO);
            DbO.SaveChanges();
        }
        public void UpdateEdiPure()
        {
            LearPureEdiO.Reprocesar = false;
            LearPureEdiO.Fprocesado = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat);
            LearPureEdiO.Log = ApplicationSettings.SavedSegments.ToString() + " segmentos analizados, procesados y guardados";
            DbO.LearPureEdi.Update(LearPureEdiO);
            DbO.SaveChanges();
        }
        public void SaveAll() {
            ISAO.SaveAll830(ref DbO);
            DbO.SaveChanges();
        }
        public string AutoSendInventary830(ref EdiRepSent EdiSent)
        {
            string ThisDate = DateTime.Now.ToString(ApplicationSettings.ToDateTimeFormat);
            string ThisTime = DateTime.Now.ToString(ApplicationSettings.ToTimeFormat);
            int ControlNumber = 1, NSeg = 0;
            IEnumerable<int> IeMaxRep =
                (from Rs in DbO.EdiRepSent
                 where Rs.Tipo == "830"
                 select Convert.ToInt32(Rs.Code));
            if (IeMaxRep.Count() > 0)
                ControlNumber = IeMaxRep.Max() + 1;
            ISAO = new ISA830(EdiBase.SegmentTerminator)
            {
                AuthorizationInformationQualifier = "00",
                AuthorizationInformation = "          ",
                SecurityInformationQualifier = "00",
                SecurityInformation = "          ",
                InterchangeSenderIdQualifier = "ZZ",
                InterchangeSenderId = "GLC504         ",
                InterchangeReceiverIdQualifier = "ZZ",
                InterchangeReceiverId = "ICN3660        ",
                InterchangeDate = ThisDate,
                InterchangeTime = ThisTime,
                InterchangeControlStandardsId = "U",
                InterchangeControlVersion = "00231",
                AcknowledgmentRequested = "0",
                UsageIndicator = "P",
                InterchangeControlNumber = $"{ControlNumber.ToString("D9")}"
            };
            NSeg++;
            GS830 Gs = new GS830(EdiBase.SegmentTerminator)
            {
                FunctionalIdCode = "PS",
                ApplicationSenderCode = "GLC504",
                ApplicationReceiverCode = "ICN3660",
                GsDate = ThisDate,
                GsTime = ThisTime,
                ResponsibleAgencyCode = "X",
                Version = "002040",
                GroupControlNumber = $"{ControlNumber.ToString("D4")}"
            };
            NSeg++;
            //Gs.Parent = ISAO;
            //ISAO.Childs.Add(Gs);
            ISAO.AddParentChild(Gs);
            ST830 St = new ST830(EdiBase.SegmentTerminator) {
                IdCode = "830",
                ControlNumber = $"{ControlNumber.ToString("D4")}"
            };
            NSeg++;
            ISAO.AddParentChild(St);
            BFR830 Bfr = new BFR830(EdiBase.SegmentTerminator) {
                TransactionSetPurposeCode = "00",
                ForecastOrderNumber = "",
                ReleaseNumber = "0000",
                ForecastTypeQualifier = "ZZ",
                ForecastQuantityQualifier = "A",
                ForecastHorizonStart = ThisDate,
                ForecastHorizonEnd = ThisDate,
                ForecastGenerationDate = ThisDate,
                ForecastUpdatedDate = "",
                ContractNumber = "",
                PurchaseOrderNumber = ""
            };
            NSeg++;
            St.AddParentChild(Bfr);
            IEnumerable<FE830DataAux> IeExistencias = ManualDB.SP_GetExistencias(ref DbO, 618);
            IEnumerable<LearEquivalencias> learEquivalencias = DbO.LearEquivalencias;
            foreach (FE830DataAux Producto in IeExistencias)
            {
                IEnumerable<LearEquivalencias> Exists = learEquivalencias.Where(E => E.CodProducto == Producto.CodProducto
                || E.CodProducto + "-" == Producto.CodProducto);
                if (Exists.Count() == 0)
                    continue;
                LIN830 Lin = new LIN830(EdiBase.SegmentTerminator) {
                    AssignedIdentification = "",
                    ProductIdQualifier = "BP",
                    ProductId = Producto.CodProducto,
                    ProductRefIdQualifier = "",
                    ProductRefId = "",
                    ProductPurchaseIdQualifier = "",
                    ProductPurchaseId = "",
                };
                NSeg++;
                St.AddParentChild(Lin);
                UIT830 Uit = new UIT830(EdiBase.SegmentTerminator) {
                    UnitOfMeasure = "FT"
                };
                NSeg++;
                Lin.AddParentChild(Uit);
                PRS830 Prs = new PRS830(EdiBase.SegmentTerminator) {
                    StatusCode = "9"
                };
                NSeg++;
                Lin.AddParentChild(Prs);
                N1830 N1 = new N1830(EdiBase.SegmentTerminator) {
                    OrganizationId = "ST",
                    Name = "GLC HONDURAS",
                    IdCodeQualifier = "92",
                    IdCode = "GLC504"
                };
                NSeg++;
                Lin.AddParentChild(N1);
                N4830 N4 = new N4830(EdiBase.SegmentTerminator) {
                    LocationQualifier = "WH",
                    LocationId = "GLC504"
                };
                NSeg++;
                Lin.AddParentChild(N4);
                SDP830 Sdp = new SDP830(EdiBase.SegmentTerminator) {
                    CalendarPatternCode = "Z",
                    PatternTimeCode = "Z"
                };
                NSeg++;
                Lin.AddParentChild(Sdp);
                FST830 Fst = new FST830(EdiBase.SegmentTerminator) {
                    Quantity = Math.Round(Convert.ToDecimal(Producto.Existencia), 0).ToString(),
                    ForecastQualifier = "Z",
                    ForecastTimingQualifier = "Z",
                    FstDate = ThisDate
                }; // Solo el monto actual
                NSeg++;
                Sdp.AddParentChild(Fst);
            }
            CTT830 Ctt = new CTT830(EdiBase.SegmentTerminator)
            {
                TotalLineItems = IeExistencias.Count().ToString(),
                HashTotal = ""
            };
            ISAO.AddParentChild(Ctt);
            SE830 Se = new SE830(EdiBase.SegmentTerminator) {
                NumIncludedSegments = NSeg.ToString(),
                TransactionSetControlNumber = $"{ControlNumber.ToString("D4")}"
            };
            ISAO.AddParentChild(Se);
            GE830 Ge = new GE830(EdiBase.SegmentTerminator) {
                NumTransactionSetsIncluded = "1",
                GroupControl = $"{ControlNumber.ToString("D4")}"
            };
            ISAO.AddParentChild(Ge);
            IEA830 Iea = new IEA830(EdiBase.SegmentTerminator) {
                NumIncludedGroups = "1",
                InterchangeControlNumber = $"{ControlNumber.ToString("D9")}"
            };
            ISAO.AddParentChild(Iea);
            string ActualEdiStr = ISAO.Ts();
            EdiSent.Log = "Reporte enviado";
            EdiSent.Code = ControlNumber.ToString();
            EdiSent.EdiStr = ActualEdiStr;
            EdiBase E1 = new EdiBase(EdiBase.SegmentTerminator);
            E1.HashId = EdiSent.HashId;
            ISAO.Parent = E1;
            ISAO.SaveAll830(ref DbO);
            DbO.EdiRepSent.Update(EdiSent);
            LearPureEdi Pe = new LearPureEdi() {
                EdiStr = ActualEdiStr,
                HashId = E1.HashId,
                Fingreso = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                Fprocesado = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                Reprocesar = false,
                NombreArchivo = "Inventario",
                Log = $"{NSeg} segmentos analizados, procesados y guardados",
                CheckSeg = NSeg,
                Shp = false,
                InOut = "O"
            };
            DbO.LearPureEdi.Add(Pe);
            DbO.SaveChanges();
            return EdiSent.EdiStr;
        }
    }
}
