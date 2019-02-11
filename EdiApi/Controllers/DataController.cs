using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models;
using EdiApi.Models.EdiDB;
using EdiApi.Models.WmsDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : Controller
    {
        public EdiDBContext DbO;
        public WmsContext WmsDbO;
        public Models.Remps_globalDB.Remps_globalContext Remps_globalDbO;
        public static readonly string G1;        
        public DataController(EdiDBContext _DbO, WmsContext _WmsDbO, Models.Remps_globalDB.Remps_globalContext _Remps_globalDB) {
            DbO = _DbO;
            WmsDbO = _WmsDbO;
            Remps_globalDbO = _Remps_globalDB;            
            
        }        
        private IEnumerable<Rep830Info> GetExToIe1(Exception E1)
        {
            yield return new Rep830Info() { errorMessage = E1.ToString() };
        }
        private IEnumerable<TsqlDespachosWmsComplex> GetExToIe2(Exception E1)
        {
            yield return new TsqlDespachosWmsComplex() { ErrorMessage = E1.ToString() };
        }        
        [HttpGet]
        public IEnumerable<Rep830Info> GetPureEdi(string HashId = "")
        {   
            try
            {
                int LcCount = DbO.LearCodes.Where(Lc => Lc.Str == "GS.FunctionalIdCode").Count();
                LcCount++;
                IEnumerable<Rep830Info> LRet =
                        from Pe in DbO.LearPureEdi
                        from IsaF in DbO.LearIsa830
                        where IsaF.ParentHashId == Pe.HashId
                        && (string.IsNullOrEmpty(HashId) || Pe.HashId == HashId)
                        && !string.IsNullOrEmpty(Pe.NombreArchivo)
                        orderby Pe.Fingreso descending
                        select new Rep830Info()
                        {
                            From = IsaF.InterchangeSenderId,
                            To = IsaF.InterchangeReceiverId,
                            HashId = Pe.HashId,
                            Fingreso = Pe.Fingreso,
                            Fprocesado = Pe.Fprocesado,
                            Reprocesar = Pe.Reprocesar,
                            NombreArchivo = Pe.NombreArchivo,
                            Log = Pe.Log,
                            CheckSeg = Pe.CheckSeg,
                            NumReporte = IsaF.InterchangeControlNumber,
                            Estado = Pe.Log.Contains("segmentos analizados, procesados y guardados")? "Exitoso" : "Error"                            
                        };
                return LRet;
            }
            catch (Exception e1)
            {
                return (GetExToIe1(e1));
            }
        }        
        [HttpGet]
        public ActionResult<FE830Data> GetFE830Data(string HashId)
        {
            FE830Data FE830DataRet = new FE830Data
            {
                ISA = (from Isa in DbO.LearIsa830
                      where Isa.ParentHashId == HashId                      
                      select Isa).FirstOrDefault(),
                ListEdiSegName = DbO.EdiSegName
            };
            if (FE830DataRet.ISA == null)
                return FE830DataRet;
            FE830DataRet.GS = (from Gs in DbO.LearGs830
                              where Gs.ParentHashId == FE830DataRet.ISA.HashId
                              select Gs).FirstOrDefault();
            FE830DataRet.ListSt = from St in DbO.LearSt830
                                  where St.ParentHashId == FE830DataRet.ISA.HashId
                                  select St;
            FE830DataRet.ListStBfr = from Bfr in DbO.LearBfr830
                                   from St in DbO.LearSt830
                                   where Bfr.ParentHashId == St.HashId 
                                   && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Bfr;
            FE830DataRet.ListStN1 = from N1 in DbO.LearN1830
                                    from St in DbO.LearSt830
                                    where N1.ParentHashId == St.HashId 
                                    && St.ParentHashId == FE830DataRet.ISA.HashId
                                    select N1;
            FE830DataRet.ListLinN1 = from LN1 in DbO.LearN1830
                                    from Lin in DbO.LearLin830
                                    from St in DbO.LearSt830
                                    where LN1.ParentHashId == Lin.HashId
                                    && Lin.ParentHashId == St.HashId
                                    && St.ParentHashId == FE830DataRet.ISA.HashId
                                    select LN1;
            FE830DataRet.ListStN4 = from N4 in DbO.LearN4830
                                    from St in DbO.LearSt830
                                    where N4.ParentHashId == St.HashId 
                                    && St.ParentHashId == FE830DataRet.ISA.HashId
                                    select N4;
            FE830DataRet.ListLinN4 = from LN4 in DbO.LearN4830
                                    from Lin in DbO.LearLin830
                                    from St in DbO.LearSt830
                                    where LN4.ParentHashId == Lin.HashId
                                    && Lin.ParentHashId == St.HashId
                                    && St.ParentHashId == FE830DataRet.ISA.HashId
                                    select LN4;
            FE830DataRet.ListStNte = from Nte in DbO.LearNte830
                                    from St in DbO.LearSt830
                                    where Nte.ParentHashId == St.HashId 
                                    && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Nte;
            FE830DataRet.ListLinNte = from Nte2 in DbO.LearNte830
                                      from Lin in DbO.LearLin830
                                     from St in DbO.LearSt830
                                     where Nte2.ParentHashId == Lin.HashId 
                                     && Lin.ParentHashId == St.HashId 
                                     && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Nte2;
            FE830DataRet.ListStLin = from Lin in DbO.LearLin830
                                     from St in DbO.LearSt830
                                     where Lin.ParentHashId == St.HashId 
                                     && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Lin;
            FE830DataRet.ListLinUit = from Uit in DbO.LearUit830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Uit.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Uit;
            FE830DataRet.ListLinPrs = from Prs in DbO.LearPrs830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Prs.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Prs;
            FE830DataRet.ListLinSdp = from Sdp in DbO.LearSdp830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Sdp.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Sdp;
            FE830DataRet.ListLinFst = from Fst in DbO.LearFst830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Fst.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      orderby Fst.FstDate
                                      select Fst;
            FE830DataRet.ListSdpFst = from Fst in DbO.LearFst830
                                      from Sdp in DbO.LearSdp830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Fst.ParentHashId == Sdp.HashId
                                      && Sdp.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Fst;
            FE830DataRet.ListLinAth = from Ath in DbO.LearAth830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Ath.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Ath;
            FE830DataRet.ListLinShp = from Shp in DbO.LearShp830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Shp.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Shp;
            FE830DataRet.ListLinRef = from Ref in DbO.LearRef830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Ref.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Ref;
            FE830DataRet.ListShpRef = from Ref in DbO.LearRef830
                                      from Shp in DbO.LearShp830
                                      from Lin in DbO.LearLin830
                                      from St in DbO.LearSt830
                                      where Ref.ParentHashId == Shp.HashId
                                      && Shp.ParentHashId == Lin.HashId
                                      && Lin.ParentHashId == St.HashId
                                      && St.ParentHashId == FE830DataRet.ISA.HashId
                                      select Ref;
            FE830DataRet.ListCodes = from c in DbO.LearCodes
                                     orderby c.Str, c.Param
                                     select c;
            FE830DataRet.ListProdExist = ManualDB.SP_GetExistencias(ref WmsDbO, 618);            
            return FE830DataRet;
        }
        [HttpGet]
        public ActionResult<string> TestThisService()
        {
            int? a = 10;
            int? b = 11;
            int? c = 12;
            a = null;
            int? d = a ?? b ?? c;
            int a5 = 5;
            string Ret = "";
            if (Convert.ToBoolean((.002f) - (.1f)))
                Ret = "Hello";
            else if (a5 == 5)
                Ret = "World";
            else Ret = "Bye";
            return $"Funciona ok {d.ToString()} {Ret}";
        }
        [HttpGet]
        public IEnumerable<TsqlDespachosWmsComplex> GetSN() {
            try
            {
                IEnumerable<TsqlDespachosWmsComplex> ListSN = ManualDB.SP_GetSN(ref WmsDbO);
                return ListSN;
            }
            catch (Exception e1)
            {
                return GetExToIe2(e1);
            }
        }
        [HttpGet]
        public string SendForm856(string listDispatch)
        {
            string ThisDate = DateTime.Now.ToString(ApplicationSettings.ToDateTimeFormat);
            string ThisTime = DateTime.Now.ToString(ApplicationSettings.ToTimeFormat);
            IEnumerable<string> ListDispatch = listDispatch.Split('|');
            IEnumerable<TsqlDespachosWmsComplex> ListSN = ManualDB.SP_GetSNDet(ref WmsDbO);
            ListSN = from Ls in ListSN
                     from Ld in ListDispatch
                     where Ls.DespachoId == Convert.ToInt32(Ld)
                     select Ls;
            int InterchangeControlNumber = (from Isa in DbO.LearIsa856 select Convert.ToInt32(Isa.InterchangeControlNumber)).Max() + 1;
            int ContHl = 1;
            foreach (string Despacho in ListDispatch)
            {
                TsqlDespachosWmsComplex DespachoInfo = ListSN.Where(Sn => Sn.DespachoId == Convert.ToInt32(Despacho)).Fod();
                if (DespachoInfo == null) continue;
                ISA856 Isa = new ISA856(0, EdiBase.SegmentTerminator, $"{InterchangeControlNumber:9}") {
                    AuthorizationInformationQualifier = "00",
                    AuthorizationInformation = "          ",
                    SecurityInformationQualifier = "00",
                    SecurityInformation = "          ",
                    InterchangeSenderIdQualifier = "ZZ",
                    InterchangeSenderId = "GLC504",
                    InterchangeReceiverIdQualifier = "ZZ",
                    InterchangeReceiverId = "HN02NC72       ",
                    InterchangeDate = ThisDate,
                    InterchangeControlStandardsId = "U",
                    InterchangeControlVersion = "0231",
                    AcknowledgmentRequested = "0",
                    UsageIndicator = "P"
                };
                GS856 Gs = new GS856(0, EdiBase.SegmentTerminator, $"{InterchangeControlNumber}") {
                    FunctionalIdCode = "SH",
                    ApplicationSenderCode = "GLC504",
                    ApplicationReceiverCode = "HN02NC72       ",
                    GsDate = ThisDate,
                    GsTime = ThisTime,
                    ResponsibleAgencyCode = "X",
                    Version = "002040"
                };
                ST856 St = new ST856(0, EdiBase.SegmentTerminator, $"{InterchangeControlNumber}");
                Isa.Childs.Add(Gs);
                Isa.Childs.Add(St);
                BSN856 Bsn = new BSN856(EdiBase.SegmentTerminator) {
                    TransactionSetPurposeCode = "00",
                    ShipIdentification = Despacho,
                    BsnDate = ThisDate,
                    BsnTime = ThisTime
                };
                Isa.Childs.Add(Bsn);
                DTM856 Dtm = new DTM856(EdiBase.SegmentTerminator) {
                    DateTimeQualifier = "011",
                    DtmDate = ThisDate,
                    DtmTime = ThisTime
                };
                Isa.Childs.Add(Dtm);
                HLSL856 Hls = new HLSL856(EdiBase.SegmentTerminator) {
                    HierarchicalIdNumber = ContHl.ToString(),
                    HierarchicalLevelCode = "S"
                };
                MEA856 Mea1 = new MEA856(EdiBase.SegmentTerminator)
                {
                    MeasurementReferenceIdCode = "PD",
                    MeasurementDimensionQualifier = "G",
                    MeasurementValue = DespachoInfo.Weight.ToString(),
                    UnitOfMeasure = "KG"
                };
                MEA856 Mea2 = new MEA856(EdiBase.SegmentTerminator)
                {
                    MeasurementReferenceIdCode = "PD",
                    MeasurementDimensionQualifier = "N",
                    MeasurementValue = Mea1.MeasurementValue,
                    UnitOfMeasure = "KG"
                };
                Hls.Childs.Add(Mea1);
                Hls.Childs.Add(Mea2);
                TD1856 Td1 = new TD1856(EdiBase.SegmentTerminator) {
                    PackagingCode = "PLT71",
                    LadingQuantity = "1"
                };
                Hls.Childs.Add(Td1);
                TD5856 Td5 = new TD5856(EdiBase.SegmentTerminator) {
                    RoutingSequenceCode = "B",
                    IdCodeQualifier = "ZZ",
                    IdentificationCode = DespachoInfo.DocumentoMotorista,
                    TransportationMethodCode = "M",
                    LocationQualifier = "PP",
                    LocationIdentifier = "ORMSBY"
                };
                Hls.Childs.Add(Td5);
                TD3856 Td3 = new TD3856(EdiBase.SegmentTerminator)
                {
                    EquipmentDescriptionCode = "TL",
                    EquipmentInitial = "",
                    EquipmentNumber = DespachoInfo.NoContenedor
                };
                Hls.Childs.Add(Td3);
                REF856 Ref1 = new REF856(EdiBase.SegmentTerminator) {
                    ReferenceNumberQualifier = "VN",
                    ReferenceNumber = DespachoInfo.NumeroOc
                };
                Hls.Childs.Add(Ref1);
                REF856 Ref2 = new REF856(EdiBase.SegmentTerminator)
                {
                    ReferenceNumberQualifier = "PK",
                    ReferenceNumber = DespachoInfo.NoMarchamo
                };
                Hls.Childs.Add(Ref2);
                N1856 N11 = new N1856(EdiBase.SegmentTerminator) {
                    EntityIdentifierCode = "ST",
                    IdCodeQualifier = "92",
                    IdCode = "HN02NC72"
                };
                Hls.Childs.Add(N11);
                N1856 N12 = new N1856(EdiBase.SegmentTerminator)
                {
                    EntityIdentifierCode = "SF",
                    IdCodeQualifier = "92",
                    IdCode = "GLC504"
                };
                Hls.Childs.Add(N12);
                HLOL856 HlO1 = new HLOL856(EdiBase.SegmentTerminator) {

                };
            }
            return string.Empty;
        }
    }
}