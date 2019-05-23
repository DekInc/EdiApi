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
using System.Text;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public EdiDBContext DbO;
        public WmsContext WmsDbO;
        public WmsContext WmsDbOLong;
        public static readonly string G1;
        public readonly IConfiguration Config;
        IConfiguration IMapConfig => Config.GetSection("IMapConfig");
        IConfiguration IEdiFtpConfig => Config.GetSection("EdiFtp");
        string IMapHost => (string)IMapConfig.GetValue(typeof(string), "Host");
        int IMapPortIn => Convert.ToInt32(IMapConfig.GetValue(typeof(string), "PortIn"));
        int IMapPortOut => Convert.ToInt32(IMapConfig.GetValue(typeof(string), "PortOut"));
        bool IMapSSL => Convert.ToBoolean(IMapConfig.GetValue(typeof(string), "SSL"));
        string IMapUser => (string)IMapConfig.GetValue(typeof(string), "User");
        string IMapPassword => (string)IMapConfig.GetValue(typeof(string), "Password");
        string FtpHost => (string)IEdiFtpConfig.GetValue(typeof(string), "Host");
        string FtpHostFailover => (string)IEdiFtpConfig.GetValue(typeof(string), "HostFailover");
        string FtpUser => (string)IEdiFtpConfig.GetValue(typeof(string), "EdiUser");
        string FtpPassword => (string)IEdiFtpConfig.GetValue(typeof(string), "EdiPassword");
        string FtpDirIn => (string)IEdiFtpConfig.GetValue(typeof(string), "DirIn");
        string FtpDirOut => (string)IEdiFtpConfig.GetValue(typeof(string), "DirOut");
        string FtpDirChecked => (string)IEdiFtpConfig.GetValue(typeof(string), "DirChecked");
        object MaxEdiComs => Config.GetSection("MaxEdiComs").GetValue(typeof(object), "Value");
        public DataController(EdiDBContext _DbO, WmsContext _WmsDbO, WmsContext _WmsDbOLong, IConfiguration _Config) {
            DbO = _DbO;
            WmsDbO = _WmsDbO;
            WmsDbOLong = _WmsDbOLong;
            Config = _Config;
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
        public string GetVersion() {
            return "1.1.2.0";
        }
        [HttpGet]
        public string UpdateLinComments(string LinHashId, string TxtLinComData, string ListFst) {
            try
            {
                LearLin830 Lin = DbO.LearLin830.Where(L => L.HashId == LinHashId).Fod();
                if (Lin == null)
                    return "No existe el elemento Lin";
                Lin.Comments = TxtLinComData;
                DbO.Update(Lin);
                foreach (string FstStr in ListFst.Split('|'))
                {
                    if (string.IsNullOrEmpty(FstStr)) break;
                    string[] FstArr = FstStr.Split('Ç');
                    if (FstArr.Length != 2) break;
                    LearFst830 learFst830 = DbO.LearFst830.Where(F => F.HashId == FstArr[0]).Fod();
                    learFst830.RealQty = FstArr[1];
                    DbO.LearFst830.Update(learFst830);
                }
                DbO.SaveChanges();
                return "ok";
            }
            catch (Exception Me1)
            {
                return Me1.ToString();
            }
        }
        [HttpGet]
        public IEnumerable<EdiComs> GetEdiComs()
        {
            return DbO.EdiComs.OrderByDescending(O => O.Id);
        }
        [HttpGet]
        public string GetConfig(string HashId)
        {
            if (HashId == "P1234567890")
            {
                return DbO.Database.GetDbConnection().ConnectionString;
            }
            return string.Empty;
        }
        [HttpGet]
        public IEnumerable<EdiRepSent> GetEdiRepSent()
        {
            return DbO.EdiRepSent.OrderByDescending(O => O.Id);
        }
        [HttpGet]
        public IEnumerable<LearPureEdi> GetLearPureEdi()
        {
            return DbO.LearPureEdi.OrderByDescending(O => O.Fingreso.ToDateEsp());
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
                            InOut = (Pe.InOut == "I" ? "Pedido" : "Inventario"),
                            From = IsaF.InterchangeSenderId,
                            To = IsaF.InterchangeReceiverId,
                            HashId = Pe.HashId,
                            Fingreso = (IsaF.InterchangeDate + IsaF.InterchangeTime).ToDate().ToString(ApplicationSettings.DateTimeFormat),
                            Fprocesado = Pe.Fprocesado,
                            Reprocesar = Pe.Reprocesar,
                            NombreArchivo = Pe.NombreArchivo,
                            Log = Pe.Log,
                            CheckSeg = Pe.CheckSeg,
                            NumReporte = IsaF.InterchangeControlNumber,
                            Estado = Pe.Log.Contains("segmentos analizados, procesados y guardados") ? "Exitoso" : "Error"
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
                                     orderby Lin.ProductId ascending
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
            FE830DataRet.ListProdExist = ManualDB.SP_GetExistencias(ref DbO, 618);
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
        public IEnumerable<TsqlDespachosWmsComplex> GetSN(bool NoEnviado = false) {
            try
            {
                IEnumerable<TsqlDespachosWmsComplex> ListSN = ManualDB.SP_GetSN(ref DbO);
                List<EdiRepSent> ListSent =
                    (from S in DbO.EdiRepSent
                     where S.Tipo == "856"
                     && (DateTime.Now - S.Fecha.ToDateEsp()).TotalDays < 32
                     select S).ToList();
                if (ListSent.Count > 0 && NoEnviado)
                {
                    ListSN = from Sn in ListSN where ListSent.Where(S => S.Code == Sn.DespachoId.ToString()).Count() == 0 select Sn;
                    //ListSN = ListSN.Where(Sn => ListSent.Where(S => S.Code == Sn.DespachoId.ToString()).Count() == 0);
                }
                return ListSN;
            }
            catch (Exception e1)
            {
                return GetExToIe2(e1);
            }
        }
        [HttpGet]
        public string SendForm856(string listDispatch, string Idusr)
        {
            try
            {
                string ThisDate = DateTime.Now.ToString(ApplicationSettings.ToDateTimeFormat);
                string ThisTime = DateTime.Now.ToString(ApplicationSettings.ToTimeFormat);
                IEnumerable<string> ListDispatch = listDispatch.Split('|');
                IEnumerable<TsqlDespachosWmsComplex> ListSNO = ManualDB.SP_GetSNDet(ref DbO, 0);
                List<LearEquivalencias> ListEquivalencias = DbO.LearEquivalencias.ToList();
                List<TsqlDespachosWmsComplex> ListSN = (from Ls in ListSNO
                                                        from Ld in ListDispatch
                                                        where Ls.DespachoId == Convert.ToInt32(Ld)
                                                        select Ls).ToList();
                //Obtener ultimo reporte
                List<string> LastRepHashId =
                    (from Pe in DbO.LearPureEdi
                     where Pe.InOut == "I"
                     && !Pe.Shp
                     && Pe.Fingreso.ToDateEsp() >= DateTime.Now.AddDays(-15)
                     orderby Pe.Fingreso.ToDateEsp() descending
                     select Pe.HashId).ToList();
                if (LastRepHashId.Count == 0)
                    return "No existen pedidos";
                //List<string> LastRepHashId = LastRep.Fod().HashId;
                DateTime DateCon = DateTime.Now;
                switch (DateCon.DayOfWeek)
                {
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Saturday:
                        DateCon = DateCon.AddDays(-1);
                        break;
                    case DayOfWeek.Monday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Friday:
                        break;
                    case DayOfWeek.Sunday:
                        DateCon = DateCon.AddDays(-2);
                        break;
                }
                foreach (string Despacho in ListDispatch)
                {
                    int InterchangeControlNumber = 1;
                    if (DbO.LearIsa856.Count() > 0)
                        InterchangeControlNumber = (from Isa1 in DbO.LearIsa856 select Convert.ToInt32(Isa1.InterchangeControlNumber)).Max() + 1;
                    int ContHl = 1;
                    ISA856 Isa = new ISA856(EdiBase.SegmentTerminator);
                    int NSeg = 0;
                    IEnumerable<TsqlDespachosWmsComplex> ListProductos = ListSN.Where(Sn => Sn.DespachoId == Convert.ToInt32(Despacho));
                    TsqlDespachosWmsComplex DespachoInfo = ListProductos.Fod();
                    if (DespachoInfo == null) continue;
                    Isa = new ISA856(0, EdiBase.SegmentTerminator, $"{InterchangeControlNumber.ToString("D9")}")
                    {
                        AuthorizationInformationQualifier = "00",
                        AuthorizationInformation = "          ",
                        SecurityInformationQualifier = "00",
                        SecurityInformation = "          ",
                        InterchangeSenderIdQualifier = "ZZ",
                        InterchangeSenderId = "GLC503         ",
                        InterchangeReceiverIdQualifier = "ZZ",
                        InterchangeReceiverId = "HN02NC72       ",
                        InterchangeDate = ThisDate,
                        InterchangeTime = ThisTime,
                        InterchangeControlStandardsId = "U",
                        InterchangeControlVersion = "00204",
                        AcknowledgmentRequested = "0",
                        UsageIndicator = "P"
                    };
                    GS856 Gs = new GS856(0, EdiBase.SegmentTerminator, $"{InterchangeControlNumber.ToString("D4")}")
                    {
                        FunctionalIdCode = "SH",
                        ApplicationSenderCode = "GLC503",
                        ApplicationReceiverCode = "HN02NC72",
                        GsDate = ThisDate,
                        GsTime = ThisTime,
                        ResponsibleAgencyCode = "X",
                        Version = "002040"
                    };
                    NSeg++;
                    ST856 St = new ST856(0, EdiBase.SegmentTerminator, $"{InterchangeControlNumber.ToString("D4")}");
                    Isa.Childs.Add(Gs);
                    Isa.Childs.Add(St);
                    NSeg++;
                    BSN856 Bsn = new BSN856(EdiBase.SegmentTerminator)
                    {
                        TransactionSetPurposeCode = "00",
                        ShipIdentification = Despacho,
                        BsnDate = ThisDate,
                        BsnTime = ThisTime
                    };
                    NSeg++;
                    St.Childs.Add(Bsn);
                    DTM856 Dtm = new DTM856(EdiBase.SegmentTerminator)
                    {
                        DateTimeQualifier = "011",
                        DtmDate = ThisDate,
                        DtmTime = ThisTime
                    };
                    NSeg++;
                    St.Childs.Add(Dtm);
                    HLSL856 Hls = new HLSL856(EdiBase.SegmentTerminator)
                    {
                        HierarchicalIdNumber = ContHl.ToString(),
                        HierarchicalLevelCode = "S"
                    };
                    NSeg++;
                    St.Childs.Add(Hls);
                    MEA856 Mea1 = new MEA856(EdiBase.SegmentTerminator)
                    {
                        MeasurementReferenceIdCode = "PD",
                        MeasurementDimensionQualifier = "G",
                        MeasurementValue = Math.Round((decimal)DespachoInfo.Weight, 2).ToString(),
                        UnitOfMeasure = "KG"
                    };
                    NSeg++;
                    MEA856 Mea2 = new MEA856(EdiBase.SegmentTerminator)
                    {
                        MeasurementReferenceIdCode = "PD",
                        MeasurementDimensionQualifier = "N",
                        MeasurementValue = Mea1.MeasurementValue,
                        UnitOfMeasure = "KG"
                    };
                    NSeg++;
                    Hls.Childs.Add(Mea1);
                    Hls.Childs.Add(Mea2);
                    TD1856 Td1 = new TD1856(EdiBase.SegmentTerminator)
                    {
                        PackagingCode = "PLT71",
                        LadingQuantity = "1"
                    };
                    NSeg++;
                    Hls.Childs.Add(Td1);
                    TD5856 Td5 = new TD5856(EdiBase.SegmentTerminator)
                    {
                        RoutingSequenceCode = "B",
                        IdCodeQualifier = "ZZ",
                        IdentificationCode = DespachoInfo.DocumentoMotorista,
                        TransportationMethodCode = "M",
                        LocationQualifier = "PP",
                        LocationIdentifier = "ORMSBY"
                    };
                    NSeg++;
                    Hls.Childs.Add(Td5);
                    TD3856 Td3 = new TD3856(EdiBase.SegmentTerminator)
                    {
                        EquipmentDescriptionCode = "TL",
                        EquipmentInitial = DespachoInfo.NoContenedor.GiveFirstStr(),
                        EquipmentNumber = DespachoInfo.NoContenedor
                    };
                    NSeg++;
                    Hls.Childs.Add(Td3);
                    REF856 Ref1 = new REF856(EdiBase.SegmentTerminator)
                    {
                        ReferenceNumberQualifier = "VN",
                        ReferenceNumber = string.IsNullOrEmpty(DespachoInfo.NumeroOc) ? "-" : DespachoInfo.NumeroOc
                    };
                    NSeg++;
                    Hls.Childs.Add(Ref1);
                    REF856 Ref2 = new REF856(EdiBase.SegmentTerminator)
                    {
                        ReferenceNumberQualifier = "PK",
                        ReferenceNumber = DespachoInfo.NoMarchamo
                    };
                    NSeg++;
                    Hls.Childs.Add(Ref2);
                    N1856 N11 = new N1856(EdiBase.SegmentTerminator)
                    {
                        EntityIdentifierCode = "ST",
                        IdCodeQualifier = "92",
                        IdCode = "HN02NC72"
                    };
                    NSeg++;
                    Hls.Childs.Add(N11);
                    N1856 N12 = new N1856(EdiBase.SegmentTerminator)
                    {
                        EntityIdentifierCode = "SF",
                        IdCodeQualifier = "92",
                        IdCode = "GLC503"
                    };
                    NSeg++;
                    Hls.Childs.Add(N12);
                    foreach (TsqlDespachosWmsComplex ComplexProd in ListProductos)
                    {
                        double? Cda = (from Sno in ListSNO
                                       where Sno.CodProducto == ComplexProd.CodProducto
                                       select Sno.Quantity).Sum();
                        ContHl++;
                        LearEquivalencias LearEquivalencia = (
                            from Le in ListEquivalencias
                            where Le.CodProducto == ComplexProd.CodProducto
                            select Le).Fod();
                        if (LearEquivalencia == null)
                        {
                            return (new Exception("No existe el producto en la tabla de equivalencias")).ToString();
                        }
                        List<LearFst830> ListFst = new List<LearFst830>();
                        foreach (string LastRep in LastRepHashId)
                        {
                            ListFst = (
                                from IsaT in DbO.LearIsa830
                                from StT in DbO.LearSt830
                                from L in DbO.LearLin830
                                from Sdp in DbO.LearSdp830
                                from F in DbO.LearFst830
                                where IsaT.ParentHashId == LastRep
                                && StT.ParentHashId == IsaT.HashId
                                && L.ParentHashId == StT.HashId
                                && Sdp.ParentHashId == L.HashId
                                && F.ParentHashId == Sdp.HashId
                                //&& F.FstDate.ToShortDate() == new DateTime(DateCon.Year, DateCon.Month, DateCon.Day)
                                && L.ProductId == LearEquivalencia.CodProductoLear
                                orderby L.ProductId
                                select F).ToList();
                            if (ListFst.Count > 0)
                                break;
                        }
                        Cda = ListFst.Where(F1 => !string.IsNullOrEmpty(F1.RealQty)).Select(F => Convert.ToDouble(F.RealQty.Replace(",", ""))).Sum();
                        ListFst = ListFst.Where(F => F.FstDate.ToShortDate() == new DateTime(DateCon.Year, DateCon.Month, DateCon.Day)).ToList();
                        if (ListFst.Count == 0)
                            return "No existe el reporte EDI 830 de cantidades para el producto " + LearEquivalencia.CodProductoLear;
                        HLOL856 HlO1 = new HLOL856(EdiBase.SegmentTerminator)
                        {
                            HierarchicalIdNumber = ContHl.ToString(),
                            HierarchicalParentIdNumber = "1",
                            HierarchicalLevelCode = "I"
                        };
                        NSeg++;
                        Hls.Childs.Add(HlO1);
                        LIN856 Lin = new LIN856(EdiBase.SegmentTerminator)
                        {
                            ProductIdQualifier1 = "BP",
                            ProductId1 = LearEquivalencia.CodProductoLear
                        };
                        NSeg++;
                        HlO1.Childs.Add(Lin);
                        SN1856 Sn1 = new SN1856(EdiBase.SegmentTerminator)
                        {
                            NumberOfUnitsShipped = ListFst.Fod().RealQty.Replace(",", ""), //DespachoInfo.Quantity.ToString(), 
                            UnitOfMeasurementCode = LearEquivalencia.Unit,
                            QuantityShipped = Cda.ToString()
                        };
                        NSeg++;
                        Lin.Childs.Add(Sn1);
                        PRF856 Prf = new PRF856(EdiBase.SegmentTerminator)
                        {
                            PurchaseOrderNumber = string.IsNullOrEmpty(DespachoInfo.NumeroOc) ? "-" : DespachoInfo.NumeroOc,
                            ReleaseNumber = DespachoInfo.DocumentoFiscal,
                            ChangeOrderSequenceNumber = "0",
                            PurchaseOrderDate = ComplexProd.FechaSalida.Value.ToString(ApplicationSettings.ToDateTimeFormat)
                        };
                        NSeg++;
                        Lin.Childs.Add(Prf);
                        CLD856 Cld = new CLD856(EdiBase.SegmentTerminator)
                        {
                            NumberOfCustomerLoads = "01",
                            UnitsShipped = ((int)Math.Round((decimal)DespachoInfo.Bulks, 0)).ToString(),
                            PackagingCode = "PLT90"
                        };
                        NSeg++;
                        Lin.Childs.Add(Cld);
                    }
                    CTT856 Ctt = new CTT856(EdiBase.SegmentTerminator)
                    {
                        NumberOfLineItems = $"{ContHl}"
                    };
                    NSeg++;
                    Isa.Childs.Add(Ctt);
                    Isa.Childs.Add(new SE856(EdiBase.SegmentTerminator)
                    {
                        NumIncludedSegments = $"{NSeg}",
                        TransactionSetControlNumber = InterchangeControlNumber.ToString("D4")
                    });
                    Isa.Childs.Add(new GE856(EdiBase.SegmentTerminator)
                    {
                        NumTransactionSetsIncluded = "1",
                        GroupControl = InterchangeControlNumber.ToString("D4")
                    });
                    Isa.Childs.Add(new IEA856(EdiBase.SegmentTerminator)
                    {
                        NumIncludedGroups = "1",
                        InterchangeControlNumber = Isa.InterchangeControlNumber
                    });
                    try
                    {
                        Isa.SaveAll856(ref DbO);
                    }
                    catch (Exception e2)
                    {
                        return e2.ToString();
                    }
                    string EdiStrO = Isa.Ts();
                    int CodUsr =
                        (from U in WmsDbO.Usrsystem
                         where U.Idusr == Idusr
                         select U.Codusr).Fod();
                    EdiRepSent Rep856N = new EdiRepSent()
                    {
                        Tipo = "856",
                        Fecha = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                        Log = "Reporte enviado",
                        Code = Despacho,
                        EdiStr = EdiStrO,
                        HashId = Isa.HashId,
                        CodUsr = CodUsr
                    };
                    string FtpRes = SendEdiFtp(EdiStrO, "856 notif envio");
                    //string FtpRes = "ok";
                    if (FtpRes != "ok")
                        return FtpRes;
                    DbO.EdiRepSent.Add(Rep856N);
                    DbO.SaveChanges();
                }
                return "ok";
            }
            catch (Exception ex2)
            {
                return ex2.ToString();
            }
        }
        private string SendEdiFtp(string _EdiStr, string Tipo)
        {
            ComRepoFtp ComRepoFtpO = new ComRepoFtp(
                        FtpHost,
                        FtpHostFailover,
                        FtpUser,
                        FtpPassword,
                        FtpDirIn,
                        FtpDirOut,
                        FtpDirChecked,
                        MaxEdiComs);
            if (!ComRepoFtpO.Ping(ref DbO))
            {
                ComRepoFtpO.UseHost2 = true;
                if (!ComRepoFtpO.Ping(ref DbO))
                {
                    return "Error, no se puede conectar con el servidor FTP primario o secundario";
                }
            }
            ComRepoFtpO.Put(_EdiStr, ref DbO, Tipo);
            return "ok";
        }
        [HttpGet]
        public string Login(string User, string Password)
        {
            try
            {
                Usrsystem UserO = (from U in WmsDbO.Usrsystem
                                   where U.Idusr == User
                                   && U.Usrpasswd == Password
                                   select U).Fod();
                if (UserO == null)
                    return string.Empty;
                EdiUsrSystem UserO2 = (from U2 in DbO.EdiUsrSystem
                                       where U2.CodUsr == UserO.Codusr
                                       select U2).Fod();
                if (UserO2 == null)
                    return string.Empty;
                UserO2.HashId = EdiBase.GetHashId();
                DbO.EdiUsrSystem.Update(UserO2);
                DbO.SaveChanges();
                return UserO2.HashId;
            }
            catch (Exception e1)
            {
                return "Error: " + e1.ToString();
            }
        }                    
        [HttpGet]
        public string LastRep() {
            try
            {
                List<EdiRepSent> ListRep = DbO.EdiRepSent.ToList();
                if (ListRep.Count > 0)
                {
                    ListRep = (
                        from R in ListRep
                        where R.Tipo == "830"
                        orderby R.Id descending
                        select R
                        ).ToList();
                    if (ListRep.Count > 0)
                    {
                        return ListRep.Fod().Fecha;
                    }
                    else return string.Empty;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }            
        }
        ////////Extranet
        [HttpGet]
        public RetData<IEnumerable<ClientesModel>> GetClientsOrders()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<int?> ListIdClients = (
                    from Pe in DbO.PedidosExternos
                    orderby Pe.ClienteId
                    select Pe.ClienteId
                    ).Distinct();
                IEnumerable<ClientesModel> ListClients = (
                    from Lc in ListIdClients
                    from C in WmsDbO.Clientes
                    where C.ClienteId == Lc
                    select new ClientesModel() { ClienteId = C.ClienteId, Nombre = C.Nombre }
                    );
                return new RetData<IEnumerable<ClientesModel>>
                {
                    Data = ListClients,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<ClientesModel>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<ExistenciasExternModel>> GetStock(object ClientId)
        {
            DateTime StartTime = DateTime.Now;
            RetData<IEnumerable<ExistenciasExternModel>> RetDataO = new RetData<IEnumerable<ExistenciasExternModel>>();
            try
            {
                RetDataO = new RetData<IEnumerable<ExistenciasExternModel>>
                {
                    Data = ManualDB.SP_GetExistenciasExtern(ref DbO, Convert.ToInt32(ClientId)),
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception exm1)
            {
                RetDataO = new RetData<IEnumerable<ExistenciasExternModel>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = exm1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            return RetDataO;
        }
        [HttpPost]
        public string LoginExtern(UserModel UserEnc)
        {
            try
            {
                string UserDecrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(UserEnc.User)));
                string PasswordDecrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(UserEnc.Password)));
                UsuariosExternos UserO = (
                    from U in DbO.UsuariosExternos
                    where U.CodUsr == UserDecrypted
                    && U.UsrPassword == PasswordDecrypted
                    select U
                    ).Fod();
                if (UserO == null)
                    return string.Empty;
                UserO.HashId = EdiBase.GetHashId();
                DbO.UsuariosExternos.Update(UserO);
                DbO.SaveChanges();
                return $"UserO.HashId|{UserO.ClienteId}";
            }
            catch (Exception e1)
            {
                return "Error: " + e1.ToString();
            }
        }
        [HttpPost]
        public RetData<PaylessTiendas> GetClient(object TiendaId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                PaylessTiendas Tienda = (
                    from T in DbO.PaylessTiendas
                    where T.TiendaId == Convert.ToInt32(TiendaId)
                    select T
                    ).Fod();
                return new RetData<PaylessTiendas>
                {
                    Data = Tienda,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };                
            }
            catch (Exception e1)
            {
                return new RetData<PaylessTiendas>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternos(object ClienteId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == Convert.ToInt32(ClienteId)
                    orderby Pe.Id descending
                    select Pe
                    );
                IEnumerable<PedidosDetExternos> ListDePe = ManualDB.SP_GetPedidosDetExternos(ref DbO, Convert.ToInt32(ClienteId));                
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>
                {
                    Data = new Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>(ListPe, ListDePe),
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetPedidosExternosDet(object PedidoId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriDetModel> ListDePe = ManualDB.SP_GetPedidosExternosDetById(ref DbO, Convert.ToInt32(PedidoId));
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                    Data = ListDePe,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternosGuardados(object ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == Convert.ToInt32(ClienteId)
                    && Pe.IdEstado == 1
                    orderby Pe.Id descending
                    select Pe
                    );
                IEnumerable<PedidosDetExternos> ListDePe = ManualDB.SP_GetPedidosDetExternosGuardados(ref DbO, Convert.ToInt32(ClienteId));
                ListDePe = (
                    from Dp in ListDePe
                    from Pe in ListPe
                    where Dp.PedidoId == Pe.Id
                    select Dp
                    );
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> {
                    Data = new Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>(ListPe, ListDePe),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternosPendientes(object ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == Convert.ToInt32(ClienteId)
                    && Pe.IdEstado == 2
                    orderby Pe.Id descending
                    select Pe
                    );
                IEnumerable<PedidosDetExternos> ListDePe = ManualDB.SP_GetPedidosDetExternos(ref DbO, Convert.ToInt32(ClienteId)).ToList();                
                ListDePe = (
                    from Dp in ListDePe
                    from Pe in ListPe
                    where Dp.PedidoId == Pe.Id
                    select Dp
                    );
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> {
                    Data = new Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>(ListPe, ListDePe),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>> GetPedidosExternosPendientesAdmin()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                List<Clientes> ListClients = new List<Clientes>();
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.IdEstado == 2
                    orderby Pe.Id descending
                    select Pe
                    );
                foreach (PedidosExternos PedC in ListPe)
                {
                    ListClients.Add(
                        (from C in WmsDbO.Clientes
                        where C.ClienteId == PedC.ClienteId
                        select new Clientes() { ClienteId = C.ClienteId, Nombre = C.Nombre }).Fod()
                    );
                }
                IEnumerable<PedidosDetExternos> ListDePe = (
                    from Dp in DbO.PedidosDetExternos
                    from Pe in DbO.PedidosExternos
                    where Dp.PedidoId == Pe.Id
                    && Pe.IdEstado == 2
                    orderby Dp.CodProducto
                    select Dp
                    );
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>>
                {
                    Data = new Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>(ListPe, ListDePe, ListClients),
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternosAdmin(object PedidoId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.Id == Convert.ToInt32(PedidoId)
                    orderby Pe.Id descending
                    select Pe
                    );
                IEnumerable<PedidosDetExternos> ListDePe = (
                    from Dp in DbO.PedidosDetExternos
                    from Pe in DbO.PedidosExternos
                    where Dp.PedidoId == Pe.Id
                    && Dp.PedidoId == Convert.ToInt32(PedidoId)
                    orderby Dp.Id descending
                    select Dp
                    );
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>
                {
                    Data = new Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>(ListPe, ListDePe),
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<PedidosDetExternos>> GetPedidosDetExternos(object PedidoId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                return new RetData<IEnumerable<PedidosDetExternos>>
                {
                    Data = (PedidoId == null ? DbO.PedidosDetExternos : DbO.PedidosDetExternos.Where(Pe => Pe.PedidoId == Convert.ToInt32(PedidoId))),
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<PedidosDetExternos>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<PedidosExternos> SetPedidoExterno(IEnumerable<PaylessProdPrioriDetModel> ListDis, int ClienteId, int IdEstado, string cboPeriod)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                if (ListDis.Count() == 0)
                    throw new Exception("No hay productos en la lista. WebAPI.");
                string DateProm = "";
                foreach (PaylessProdPrioriDetModel Pem in ListDis)
                    if (!string.IsNullOrEmpty(Pem.dateProm)) DateProm = Pem.dateProm;
                IEnumerable<PedidosExternos> ListPe = DbO.PedidosExternos.Where(O => O.ClienteId == ClienteId && O.Periodo == cboPeriod && O.FechaPedido == DateProm && O.IdEstado == 1);
                int PedidoId = 0;
                PedidosExternos PedidoExterno = new PedidosExternos();
                if (ListPe != null) {
                    if (ListPe.Count() > 0) {
                        PedidoExterno = ListPe.Fod();
                        PedidoExterno.IdEstado = IdEstado;
                    } else {
                        PedidoExterno = new PedidosExternos() {
                            ClienteId = ClienteId,
                            FechaCreacion = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                            IdEstado = IdEstado,
                            FechaPedido = DateProm,
                            Id = PedidoId,
                            Periodo = cboPeriod
                        };
                    }
                }                
                if (PedidoExterno.Id == 0)
                    DbO.PedidosExternos.Add(PedidoExterno);
                else
                {
                    DbO.PedidosExternos.Update(PedidoExterno);
                    foreach (PedidosDetExternos Pde in DbO.PedidosDetExternos.Where(Pd => Pd.PedidoId == PedidoExterno.Id))
                        DbO.PedidosDetExternos.Remove(Pde);
                }
                DbO.SaveChanges();
                List<string> ListPedidosHechos = new List<string>();
                foreach (PaylessProdPrioriDetModel PedidoDet in ListDis)
                {
                    if (ListPedidosHechos.Where(P1 => P1 == PedidoDet.Barcode).Count() == 0) {
                        PedidosDetExternos PedidoExtDet = new PedidosDetExternos() {
                            CodProducto = PedidoDet.Barcode,
                            PedidoId = PedidoExterno.Id,
                            CantPedir = Convert.ToDouble(PedidoDet.CantPedir)
                        };
                        DbO.PedidosDetExternos.Add(PedidoExtDet);
                        ListPedidosHechos.Add(PedidoDet.Barcode);
                    }
                }
                DbO.SaveChanges();
                return new RetData<PedidosExternos>
                {
                    Data = PedidoExterno,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<PedidosExternos>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<PedidosWmsModel>> GetPedidosWms(object ClienteId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PedidosWmsModel> ListDis = ManualDB.SP_GetPedidosWms(ref DbO, Convert.ToInt32(ClienteId));
                ListDis = (
                    from Ld in ListDis
                    group Ld by Ld.PedidoId into G
                    select new PedidosWmsModel() {
                        Cantidad = 0,
                        ClienteId = G.Fod().ClienteId,
                        CodProducto = "",
                        Estatus = G.Fod().Estatus,
                        FechaPedido = G.Fod().FechaPedido,
                        NomBodega = G.Fod().NomBodega,
                        Observacion = G.Fod().Observacion,
                        PedidoBarcode = G.Fod().PedidoBarcode,
                        PedidoId = G.Fod().PedidoId,
                        Regimen = G.Fod().Regimen
                    }
                ).Distinct();
                return new RetData<IEnumerable<PedidosWmsModel>>
                {
                    Data = ListDis,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<PedidosWmsModel>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<PedidosDetExternos> SetPedidoDetExterno(PedidosDetExternos PedidoDetExterno)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                DbO.PedidosDetExternos.Add(PedidoDetExterno);
                DbO.SaveChanges();
                return new RetData<PedidosDetExternos>
                {
                    Data = PedidoDetExterno,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<PedidosDetExternos>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<TsqlDespachosWmsComplex>> GetPedidosDet(object PedidoId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<TsqlDespachosWmsComplex> ListPedidoDet = ManualDB.SP_GetSNDet(ref DbO, Convert.ToInt32(PedidoId));
                ListPedidoDet = ListPedidoDet.Where(Dp => Dp.PedidoId == Convert.ToInt32(PedidoId));
                return new RetData<IEnumerable<TsqlDespachosWmsComplex>>
                {
                    Data = ListPedidoDet,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<TsqlDespachosWmsComplex>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }            
        }
        [HttpPost]
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetPaylessProdPriori(object Period)
        {
            DateTime StartTime = DateTime.Now;
            try
            { // Para no olvidar, hago Fod del master porque no permito ingresar más de 1 archivo por periodo.
                IEnumerable<PaylessProdPrioriDetModel> ListPaylessProdPrioriDet = ManualDB.SP_GetPaylessProdPrioriByPeriod(ref DbO, Period.ToString());
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>>()
                {
                    Data = ListPaylessProdPrioriDet,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>>()
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetPaylessProdPrioriAll(string TiendaId) {
            DateTime StartTime = DateTime.Now;
            try { // Para no olvidar, hago Fod del master porque no permito ingresar más de 1 archivo por periodo.
                IEnumerable<PaylessProdPrioriDetModel> ListPaylessProdPrioriDet = ManualDB.SP_GetPaylessProdPrioriAll(ref DbO, TiendaId);
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>>() {
                    Data = ListPaylessProdPrioriDet,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<string> SetPaylessProdPriori(IEnumerable<PaylessUploadFileModel> ListUpload, int ClienteId, string Periodo, string codUsr, string transporte, bool ChkUpDelete)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PaylessProdPrioriM> ListPaylessProdPrioriM = DbO.PaylessProdPrioriM.Where(Pp => Pp.Periodo == Periodo && Pp.ClienteId == ClienteId);
                PaylessProdPrioriM NewMas = new PaylessProdPrioriM() {
                    ClienteId = ClienteId,
                    Periodo = Periodo,
                    InsertDate = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                    CodUsr = codUsr
                };
                if (ChkUpDelete) {
                    if (ListPaylessProdPrioriM.Count() > 0) {
                        IEnumerable<PaylessProdPrioriDet> ListPaylessProdPrioriDet = DbO.PaylessProdPrioriDet.Where(Pd => Pd.IdPaylessProdPrioriM == ListPaylessProdPrioriM.Fod().Id);
                        DbO.PaylessProdPrioriDet.RemoveRange(ListPaylessProdPrioriDet);
                        DbO.PaylessProdPrioriM.RemoveRange(ListPaylessProdPrioriM);
                    }
                    DbO.PaylessProdPrioriM.Add(NewMas);
                } else {
                    if (ListPaylessProdPrioriM.Count() > 0)
                    {
                        NewMas = ListPaylessProdPrioriM.Fod();
                        DbO.PaylessProdPrioriM.Update(NewMas);
                    } else DbO.PaylessProdPrioriM.Add(NewMas);
                }                
                PaylessTransporte TransporteO = new PaylessTransporte() {
                    Transporte = transporte
                };
                IEnumerable<PaylessTransporte> ListTrans = DbO.PaylessTransporte.Where(T => T.Transporte == transporte);
                if (ListTrans.Count() > 0)
                    TransporteO = ListTrans.Fod();
                else
                    DbO.PaylessTransporte.Add(TransporteO);                
                DbO.SaveChanges();
                PaylessPeriodoTransporte NewPeriodoTransporte = new PaylessPeriodoTransporte() {
                    Periodo = Periodo,
                    IdTransporte = TransporteO.Id
                };
                IEnumerable<PaylessPeriodoTransporte> ListCheckPerTrans = DbO.PaylessPeriodoTransporte.Where(Pt => Pt.Periodo == Periodo && Pt.IdTransporte == TransporteO.Id);
                if (ListCheckPerTrans.Count() > 0)
                    NewPeriodoTransporte = ListCheckPerTrans.Fod();
                else
                    DbO.PaylessPeriodoTransporte.Add(NewPeriodoTransporte);
                DbO.SaveChanges();
                foreach (PaylessUploadFileModel Uf in ListUpload)
                {
                    DbO.PaylessProdPrioriDet.Add(new PaylessProdPrioriDet()
                    {
                        Oid = Uf.Oid,
                        Barcode = Uf.Barcode,
                        Estado = Uf.Estado,
                        Pri = Uf.Pri,
                        PoolP = Uf.Poolp,
                        Producto = Uf.Producto,
                        Talla = Uf.Talla,
                        Lote = Uf.Lote,
                        Categoria = Uf.Categoria,
                        Departamento = Uf.Departamento,
                        Cp = Uf.Cp,
                        Pickeada = Uf.Pickeada,
                        Etiquetada = Uf.Etiquetada,
                        Preinspeccion = Uf.Preinspeccion,
                        Cargada = Uf.Cargada,
                        M3 = Uf.M3,
                        Peso = Uf.Peso,
                        IdPaylessProdPrioriM = NewMas.Id,
                        IdTransporte = TransporteO.Id
                    });
                }
                DbO.SaveChanges();
                return new RetData<string>
                {
                    Data = "",
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<string>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<string>> GetPaylessPeriodPriori()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                List<string> ListProdPriori = DbO.PaylessProdPrioriM.Select(O => O.Periodo).ToList();
                if (ListProdPriori.Count > 0)
                    ListProdPriori = ListProdPriori.Distinct().ToList();
                return new RetData<IEnumerable<string>>
                {
                    Data = ListProdPriori,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<string>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<string>> GetPaylessPeriodPrioriByClient(int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                List<string> ListProdPriori = DbO.PaylessProdPrioriM.Where(O2 => O2.ClienteId == ClienteId) .Select(O => O.Periodo).ToList();
                if (ListProdPriori.Count > 0)
                    ListProdPriori = ListProdPriori.Distinct().ToList();
                return new RetData<IEnumerable<string>> {
                    Data = ListProdPriori,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<string>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<PaylessProdPrioriArchM> SetPaylessProdPrioriFile(IEnumerable<PaylessProdPrioriArchDet> ListUpload, int IdTransporte, string Periodo, string codUsr)
        {
            DateTime StartTime = DateTime.Now;
            try
            {

                IEnumerable<PaylessProdPrioriArchM> ListPaylessProdPrioriArchM = DbO.PaylessProdPrioriArchM.Where(Pp => Pp.Periodo == Periodo && Pp.IdTransporte == IdTransporte);
                if (ListPaylessProdPrioriArchM.Count() > 0)
                {
                    IEnumerable<PaylessProdPrioriArchDet> ListPaylessProdPrioriArchDet = DbO.PaylessProdPrioriArchDet.Where(Pd => Pd.IdM == ListPaylessProdPrioriArchM.Fod().Id);
                    DbO.PaylessProdPrioriArchDet.RemoveRange(ListPaylessProdPrioriArchDet);
                }
                PaylessProdPrioriArchM NewMas = new PaylessProdPrioriArchM();
                if (ListPaylessProdPrioriArchM.Count() > 0)
                {
                    NewMas = ListPaylessProdPrioriArchM.Fod();
                    NewMas.UpdateDate = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat);
                    DbO.PaylessProdPrioriArchM.Update(NewMas);
                }
                else
                {
                    NewMas = new PaylessProdPrioriArchM()
                    {
                        IdTransporte = IdTransporte,
                        Periodo = Periodo,
                        InsertDate = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                        CodUsr = codUsr
                    };
                    DbO.PaylessProdPrioriArchM.Add(NewMas);
                }                
                DbO.SaveChanges();
                foreach (PaylessProdPrioriArchDet Uf in ListUpload)
                {
                    DbO.PaylessProdPrioriArchDet.Add(new PaylessProdPrioriArchDet()
                    {
                        IdM = NewMas.Id,
                        Barcode = Uf.Barcode
                    });
                }
                DbO.SaveChanges();
                return new RetData<PaylessProdPrioriArchM>
                {
                    Data = NewMas,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<PaylessProdPrioriArchM>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>> GetPaylessPeriodPrioriFile()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                List<PaylessProdPrioriArchMModel> ListArchMaMo = (
                    from Pe in DbO.PaylessProdPrioriArchM
                    from T in DbO.PaylessTransporte
                    where Pe.IdTransporte == T.Id
                    orderby Pe.Id descending
                    select new PaylessProdPrioriArchMModel {
                        IdTransporte = Pe.IdTransporte,
                        Transporte = T.Transporte,
                        Id = Pe.Id,
                        InsertDate = Pe.InsertDate,
                        UpdateDate = Pe.UpdateDate,
                        Periodo = Pe.Periodo,
                        PorValid = Pe.PorcValidez
                        
                    }).ToList();                
                for(int Ci = 0; Ci < ListArchMaMo.Count(); Ci++)
                {
                    if (ListArchMaMo[Ci].PorValid == null) {
                        IEnumerable<string> ListExcelOriginal = (
                            from Fu in DbO.PaylessProdPrioriM
                            from FuD in DbO.PaylessProdPrioriDet
                            from Pe in DbO.PaylessProdPrioriArchM
                            where Pe.Id == ListArchMaMo[Ci].Id
                            && Fu.Periodo == Pe.Periodo
                            && FuD.IdTransporte == Pe.IdTransporte
                            && FuD.IdPaylessProdPrioriM == Fu.Id
                            select FuD.Barcode
                            ).Distinct();
                        IEnumerable<string> ListExcelEscaneados = (
                            from Fu in DbO.PaylessProdPrioriM
                            from FuD in DbO.PaylessProdPrioriDet
                            from Pe in DbO.PaylessProdPrioriArchM
                            from Dp in DbO.PaylessProdPrioriArchDet
                            where Pe.Id == ListArchMaMo[Ci].Id
                            && Dp.IdM == Pe.Id
                            && Fu.Periodo == Pe.Periodo
                            && FuD.IdTransporte == Pe.IdTransporte
                            && FuD.IdPaylessProdPrioriM == Fu.Id
                            && FuD.Barcode == Dp.Barcode
                            select Dp.Barcode
                            ).Distinct();
                        IEnumerable<string> ListEscaneados = (
                            from Pe in DbO.PaylessProdPrioriArchM
                            from Dp in DbO.PaylessProdPrioriArchDet
                            where Pe.Id == ListArchMaMo[Ci].Id
                            && Dp.IdM == Pe.Id
                            select Dp.Barcode
                            ).Distinct();
                        double PorcValidez = ((double)ListExcelEscaneados.Count() / (double)ListExcelOriginal.Count());
                        if (ListEscaneados.Count() > ListExcelOriginal.Count())
                            PorcValidez -= (double)(ListEscaneados.Count() - ListExcelOriginal.Count())/(double)ListExcelOriginal.Count();
                        PaylessProdPrioriArchM Am = DbO.PaylessProdPrioriArchM.Where(O => O.Id == ListArchMaMo[Ci].Id).Fod();
                        Am.PorcValidez = Math.Round(PorcValidez * 100, 3);
                        DbO.PaylessProdPrioriArchM.Update(Am);
                        ListArchMaMo[Ci].PorValid = Am.PorcValidez;
                    }
                }
                DbO.SaveChanges();
                //////////////////////////
                //////////////////////////
                ///////////////////
                /// REVISA EL RETURN
                return new RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>>
                {
                    Data = new Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>(ListArchMaMo, null),
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> GetPaylessPeriodPrioriFileExists(string Period, int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriArchM> ListM = (
                    from M in DbO.PaylessProdPrioriArchM
                    where M.Periodo == Period
                    //&& M.ClienteId == ClienteId Hilmer3
                    select M
                    );
                IEnumerable<PaylessProdPrioriArchDet> ListD = (
                    from M in DbO.PaylessProdPrioriArchM
                    from D in DbO.PaylessProdPrioriArchDet
                    where D.IdM == M.Id
                    select D
                    );
                return new RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> {
                    Data = new Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>(ListM, ListD),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> GetPaylessPeriodPrioriFileExists2(string TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriArchM> ListM = (
                    from M in DbO.PaylessProdPrioriArchM
                    //where M.Periodo == Period
                    //&& M.ClienteId == ClienteId Hilmer3
                    select M
                    );
                IEnumerable<PaylessProdPrioriArchDet> ListD = (
                    from M in DbO.PaylessProdPrioriArchM
                    from D in DbO.PaylessProdPrioriArchDet
                    where D.IdM == M.Id
                    && D.Barcode.Substring(0, 4) == TiendaId
                    select D
                    );
                return new RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> {
                    Data = new Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>(ListM, ListD),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetPaylessFileDif(int idProdArch, int idData)
        {
            DateTime StartTime = DateTime.Now;
            if (idProdArch == 0)
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "Sin datos",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };            
            try
            {
                switch (idData) {
                    case 1:
                        IEnumerable<PaylessProdPrioriDetModel> Ret = ManualDB.SP_GetPaylessProdPrioriFileDif(ref DbO, idData, idProdArch);
                        return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                            Data = Ret,
                            Info = new RetInfo() {
                                CodError = 0,
                                Mensaje = "ok",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    case 2:
                        List<PaylessProdPrioriDetModel> ListOriginal = ManualDB.SP_GetPaylessProdPrioriFileDif(ref DbO, 2, idProdArch).ToList();
                        List<PaylessProdPrioriDetModel> ListExcelEscaneado = ManualDB.SP_GetPaylessProdPrioriFileDif(ref DbO, 1, idProdArch).ToList();
                        ListExcelEscaneado.ForEach(Or => {
                            if (ListOriginal.Where(Oi => Oi.Barcode == Or.Barcode).Count() > 0)
                                ListOriginal.Remove(ListOriginal.Where(Oi => Oi.Barcode == Or.Barcode).Fod());
                        });
                        return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                            Data = ListOriginal,
                            Info = new RetInfo() {
                                CodError = 0,
                                Mensaje = "ok",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    case 3:
                        List<string> ListEscaneados = (
                           from Pe in DbO.PaylessProdPrioriArchM
                           from Dp in DbO.PaylessProdPrioriArchDet
                           where Pe.Id == idProdArch
                           && Dp.IdM == Pe.Id
                           select Dp.Barcode
                           ).Distinct().ToList();
                        List<string> ListExcelEscaneados3 = (
                            from Fu in DbO.PaylessProdPrioriM
                            from FuD in DbO.PaylessProdPrioriDet
                            from Pe in DbO.PaylessProdPrioriArchM
                            from Dp in DbO.PaylessProdPrioriArchDet
                            where Pe.Id == idProdArch
                            && Dp.IdM == Pe.Id
                            && Fu.Periodo == Pe.Periodo
                            && FuD.IdTransporte == Pe.IdTransporte
                            && FuD.IdPaylessProdPrioriM == Fu.Id
                            && FuD.Barcode == Dp.Barcode
                            select FuD.Barcode
                            ).Distinct().ToList();
                        ListExcelEscaneados3.ForEach(Ee => ListEscaneados.Remove(Ee));
                        return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                            Data = ListEscaneados.Select(E => new PaylessProdPrioriDetModel() { Barcode = E }),
                            Info = new RetInfo() {
                                CodError = 0,
                                Mensaje = "ok",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    default:
                        break;
                }
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Error de programación",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<IenetUsers>> GetClients()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                List<IenetUsers> ListUsers = DbO.IenetUsers.ToList();
                if (ListUsers.Count() > 0)
                {
                    foreach (IenetUsers Ue in ListUsers)
                    {
                        Ue.UsrPassword = string.Empty;
                        IEnumerable<Clientes> ListClients = from C in WmsDbO.Clientes where C.ClienteId == Ue.ClienteId select C;
                        if (ListClients.Count() > 0)
                            Ue.NomUsr = ListClients.Fod().Nombre;
                    }
                }
                ListUsers = (from Lu in ListUsers select new IenetUsers() { ClienteId = Lu.ClienteId, NomUsr = Lu.NomUsr }).ToList();
                return new RetData<IEnumerable<IenetUsers>>
                {
                    Data = ListUsers,
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<IenetUsers>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<Clientes>> GetAllClients(object HashId)
        {
            string HashIdDescrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(Convert.ToString(HashId))));
            HashIdDescrypted = HashIdDescrypted.Split('|')[1];
            DateTime StartTime = DateTime.Now;
            try
            {
                string CUser = (from U in DbO.IenetUsers where U.HashId == HashIdDescrypted select U.CodUsr).Fod();
                if (!string.IsNullOrEmpty(CUser))
                {
                    List<Clientes> ListClients = WmsDbO.Clientes.ToList();
                    return new RetData<IEnumerable<Clientes>>
                    {
                        Data = ListClients,
                        Info = new RetInfo()
                        {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                else
                {
                    return new RetData<IEnumerable<Clientes>>
                    {
                        Info = new RetInfo()
                        {
                            CodError = -1,
                            Mensaje = "Error de seguridad en la obtención de los datos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<Clientes>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<PaylessTiendas>> GetAllPaylessStores(object HashId) {
            string HashIdDescrypted = Encoding.UTF8.GetString(CryptoHelper.DecryptData(Convert.FromBase64String(Convert.ToString(HashId))));
            HashIdDescrypted = HashIdDescrypted.Split('|')[1];
            DateTime StartTime = DateTime.Now;
            try {
                string CUser = (from U in DbO.IenetUsers where U.HashId == HashIdDescrypted select U.CodUsr).Fod();
                if (!string.IsNullOrEmpty(CUser)) {
                    List<PaylessTiendas> ListTiendas = DbO.PaylessTiendas.ToList();
                    return new RetData<IEnumerable<PaylessTiendas>> {
                        Data = ListTiendas,
                        Info = new RetInfo() {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                } else {
                    return new RetData<IEnumerable<PaylessTiendas>> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error de seguridad en la obtención de los datos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessTiendas>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<bool> ChangePedidoState(int PedidoId, int ClienteId) {            
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosExternos> ListPe = DbO.PedidosExternos.Where(O => O.Id == Convert.ToInt32(PedidoId) && O.ClienteId == ClienteId);
                if (ListPe.Count() > 0) {
                    PedidosExternos Pe = ListPe.Fod();
                    if (Pe != null) {
                        IEnumerable<PedidosExternos> ListPeV = DbO.PedidosExternos.Where(O => O.ClienteId == ClienteId && O.IdEstado == 1);
                        if (ListPeV.Count() > 0) {
                            return new RetData<bool> {
                                Data = false,
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "Error, ya existe un pedido con el estado 'guardado' en el sistema",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                        Pe.IdEstado = 1;
                        DbO.PedidosExternos.Update(Pe);
                        DbO.SaveChanges();
                        return new RetData<bool> {
                            Data = true,
                            Info = new RetInfo() {
                                CodError = 0,
                                Mensaje = "ok",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    }
                }
                return new RetData<bool> {
                    Data = false,
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Error desconocido y casi imposible",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<bool> {
                    Data = false,
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PaylessReportes>> GetPaylessReportes() {            
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessReportes> ListReps = DbO.PaylessReportes;
                return new RetData<IEnumerable<PaylessReportes>> {
                    Data = ListReps,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessReportes>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<WmsFileModel>> GetWmsFile(string Period, int IdTransport) { 
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriDetModel> ListData = ManualDB.SP_GetPaylessProdPrioriByPeriodAndIdTransport(ref DbO, Period, IdTransport);                
                IEnumerable<WmsFileModel> ListRep = (
                    from Ex in ListData
                    group new { Ex} by new { Ex.Barcode } into G
                    select new WmsFileModel() {
                        Barcode = G.Fod().Ex.Barcode,
                        Descripcion = G.Fod().Ex.Categoria,
                        Piezas = 1,
                        Unidad = 1,
                        Cantidad = 1,
                        CodigoLocalizacion = "STAGE-01",
                        Peso = G.Sum(Lin => Lin.Ex.Peso),
                        Volumen = G.Fod().Ex.M3,
                        Cliente = G.Fod().Ex.dateProm,
                        UOM = 1,
                        Exportador = 2,
                        PaisOrigen = 166,
                        Cp = string.Join(" ", G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Cp)).Select(O2 => O2.Ex.Cp).Distinct().ToArray()),
                        Cont = G.Count(),
                        Modelo = G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Talla)).Select(O2 => O2.Ex.Talla).Distinct().Count() == 1? G.Fod().Ex.Talla : "Varios",
                        Lote = G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Producto)).Select(O2 => O2.Ex.Producto).Distinct().Count() == 1 ? G.Fod().Ex.Producto : "Varios",
                        Estilo = G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Lote)).Select(O2 => O2.Ex.Lote).Distinct().Count() == 1 ? G.Fod().Ex.Lote : "Varios",
                        Transporte = G.Fod().Ex.Transporte
                    });
                return new RetData<IEnumerable<WmsFileModel>> {
                    Data = ListRep,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<WmsFileModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PaylessReportWeekGModel>> GetReportWeek(int Tipo) {
            DateTime StartTime = DateTime.Now;
            try {
                //Jueves
                List<PaylessReportWeekGModel> Ret = new List<PaylessReportWeekGModel>();
                List<PaylessReportes> ListReportesExistentes = (
                    from R in DbO.PaylessReportes
                    orderby R.Periodo.ToDateFromEspDate() descending
                    select R
                    ).ToList();
                DateTime DateInit, DateEnd, DateMid;
                DateTime CurrentPeriod = DateTime.Now;
                if (ListReportesExistentes != null) {
                    if (ListReportesExistentes.Count() > 0) {
                        CurrentPeriod = ListReportesExistentes.Fod().Periodo.ToDateFromEspDate().AddDays(7);
                        if (CurrentPeriod.DayOfWeek == DayOfWeek.Monday) {
                            DateInit = CurrentPeriod;
                        } else {
                            DateInit = (CurrentPeriod.AddDays(DayOfWeek.Monday - CurrentPeriod.DayOfWeek));
                        }
                    } else {
                        DateInit = (CurrentPeriod.AddDays(DayOfWeek.Monday - CurrentPeriod.DayOfWeek));
                    }
                } else {
                    DateInit = (CurrentPeriod.AddDays(DayOfWeek.Monday - CurrentPeriod.DayOfWeek));
                }
                DateEnd = DateInit.AddDays(DayOfWeek.Friday - DateInit.DayOfWeek);
                DateMid = DateInit.AddDays(DayOfWeek.Wednesday - DateInit.DayOfWeek);                
                List<PedidosExternos> ListPedidosM = (
                    from Pe in DbO.PedidosExternos
                    where Pe.FechaPedido.Substring(0, 10).ToDateFromEspDate() >= DateInit 
                    && Pe.FechaPedido.Substring(0, 10).ToDateFromEspDate() <= DateEnd
                    && Pe.IdEstado == 2
                    orderby Pe.FechaPedido
                    select Pe
                    ).ToList();
                if (ListPedidosM.Count() == 3) {
                    DateInit = ListPedidosM[2].FechaPedido.Substring(0, 10).ToDateFromEspDate();
                    DateMid = ListPedidosM[1].FechaPedido.Substring(0, 10).ToDateFromEspDate();
                    DateEnd = ListPedidosM[0].FechaPedido.Substring(0, 10).ToDateFromEspDate();
                }
                if (ListPedidosM != null) {
                    if (ListPedidosM.Count() > 0) {
                        Clientes ClienteAct = (from C in WmsDbO.Clientes where C.ClienteId == ListPedidosM.Fod().ClienteId select C)?.Fod();
                        if (ClienteAct != null) {
                            PaylessTiendas Tienda = (from T in DbO.PaylessTiendas where T.ClienteId == ListPedidosM.Fod().ClienteId select T)?.Fod();
                            IEnumerable<PedidosDetExternos> ListDetTotal = (
                                from D in DbO.PedidosDetExternos
                                from M in ListPedidosM
                                where D.PedidoId == M.Id
                                select D
                                );
                            Ret.Add(new PaylessReportWeekGModel() {
                                TiendaId = Tienda.TiendaId ?? 0,
                                Location = ClienteAct?.Nombre,
                                Manager = Tienda?.Lider,
                                Tel = Tienda?.Tel
                            });
                            for (int i = 0; i < 3; i++) {
                                switch (i) {
                                    case 0:                                            
                                        Ret[Ret.Count - 1].TotalBox = Convert.ToInt32(Math.Round(ListDetTotal.Sum(O => O.CantPedir.Value), 0));                                        
                                        Ret[Ret.Count - 1].Date1 = DateInit.ToString(ApplicationSettings.DateTimeFormatShort);
                                        Ret[Ret.Count - 1].Date2 = DateMid.ToString(ApplicationSettings.DateTimeFormatShort);
                                        Ret[Ret.Count - 1].Date3 = DateEnd.ToString(ApplicationSettings.DateTimeFormatShort);
                                        IEnumerable<PedidosDetExternos> ListDet1 = ManualDB.SP_GetPedidosDetExternosByDate(ref DbO, DateInit, DateMid, 0);
                                        if (ListDet1.Count() > 0) {
                                            //Accesories = ListDet1.Where(O1 => O1.)
                                            Ret[Ret.Count - 1].Total1 = Convert.ToInt32(Math.Round(ListDet1.Sum(O => O.CantPedir.Value), 0));
                                            PedidosExternos PedidoMonday = ListPedidosM.Where(P => P.FechaPedido.Substring(0, 10).ToDateFromEspDate() == DateInit)?.Fod();
                                            if (PedidoMonday != null)
                                                Ret[Ret.Count - 1].Time1 = PedidoMonday.FechaPedido.ToDateEsp().ToString(ApplicationSettings.ToTimeFormatExcel);
                                            else {
                                                Ret[Ret.Count - 1].Date1 = "SIN PEDIDO";
                                                Ret[Ret.Count - 1].Time1 = string.Empty;
                                            }
                                            Ret[Ret.Count - 1].Accessories += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("acce", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Kids += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("niñ", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Man += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("caball", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Ladies += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("dama", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                        } else {
                                            Ret[Ret.Count - 1].Total1 = 0;
                                            Ret[Ret.Count - 1].Time1 = string.Empty;
                                        }
                                        break;
                                    case 1:
                                        IEnumerable<PedidosDetExternos> ListDet2 = ManualDB.SP_GetPedidosDetExternosByDate(ref DbO, DateMid, DateEnd, 0);
                                        if (ListDet2.Count() > 0) {
                                            Ret[Ret.Count - 1].Total2 = Convert.ToInt32(Math.Round(ListDet2.Sum(O => O.CantPedir.Value), 0));
                                            PedidosExternos PedidoWednesday = ListPedidosM.Where(P => P.FechaPedido.Substring(0, 10).ToDateFromEspDate() == DateMid)?.Fod();
                                            if (PedidoWednesday != null)
                                                Ret[Ret.Count - 1].Time2 = PedidoWednesday.FechaPedido.ToDateEsp().ToString(ApplicationSettings.ToTimeFormatExcel);
                                            else
                                                Ret[Ret.Count - 1].Time2 = string.Empty;
                                            Ret[Ret.Count - 1].Accessories += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("acce", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Kids += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("niñ", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Man += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("caball", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Ladies += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("dama", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                        } else {
                                            Ret[Ret.Count - 1].Total2 = 0;
                                            Ret[Ret.Count - 1].Time2 = string.Empty;
                                        }
                                        break;
                                    case 2:
                                        IEnumerable<PedidosDetExternos> ListDet3 = ManualDB.SP_GetPedidosDetExternosByDate(ref DbO, DateEnd, DateEnd.AddDays(2), 0);                                        
                                        if (ListDet3.Count() > 0) {
                                            Ret[Ret.Count - 1].Total3 = Convert.ToInt32(Math.Round(ListDet3.Sum(O => O.CantPedir.Value), 0));
                                            PedidosExternos PedidoFriday = ListPedidosM.Where(P => P.FechaPedido.Substring(0, 10).ToDateFromEspDate() == DateEnd)?.Fod();
                                            if (PedidoFriday != null)
                                                Ret[Ret.Count - 1].Time3 = PedidoFriday.FechaPedido.ToDateEsp().ToString(ApplicationSettings.ToTimeFormatExcel);
                                            else
                                                Ret[Ret.Count - 1].Time3 = string.Empty;
                                            Ret[Ret.Count - 1].Accessories += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("acce", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Kids += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("niñ", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Man += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("caball", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                            Ret[Ret.Count - 1].Ladies += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("dama", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
                                        } else {
                                            Ret[Ret.Count - 1].Total3 = 0;
                                            Ret[Ret.Count - 1].Time3 = string.Empty;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                return new RetData<IEnumerable<PaylessReportWeekGModel>> {
                    Data = Ret,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessReportWeekGModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<string> SetGroupAccess(int IdGroup, int IdAccess) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<IenetGroupsAccesses> ListCheck = from Ga in DbO.IenetGroupsAccesses where Ga.IdIenetGroup == IdGroup && Ga.IdIenetAccess == IdAccess select Ga;
                if (ListCheck.Count() > 0) {
                    return new RetData<string> {
                        Data = "Ya existe",
                        Info = new RetInfo() {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                DbO.IenetGroupsAccesses.Add(new IenetGroupsAccesses() {
                    IdIenetGroup = IdGroup,
                    IdIenetAccess = IdAccess
                });
                DbO.SaveChanges();
                return new RetData<string> {
                    Data = "El registro se guardo.",
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<PaylessPeriodoTransporteModel>> GetTransportByPeriod(string Period) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessPeriodoTransporteModel> ListTransport = (
                    from M in DbO.PaylessPeriodoTransporte
                    from T in DbO.PaylessTransporte
                    where M.Periodo == Period
                    && T.Id == M.IdTransporte
                    select new PaylessPeriodoTransporteModel() {
                        IdTransporte = T.Id,
                        Periodo = Period,
                        Transporte = T.Transporte
                    });
                return new RetData<IEnumerable<PaylessPeriodoTransporteModel>> {
                    Data = ListTransport,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessPeriodoTransporteModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<Bodegas>> GetWmsBodegas(int LocationId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<Bodegas> List = (
                    from B in WmsDbO.Bodegas
                    from E in WmsDbO.Estatus
                    where B.EstatusId == E.EstatusId
                    && B.Locationid == LocationId
                    orderby B.NomBodega
                    select B
                    );
                return new RetData<IEnumerable<Bodegas>> {
                    Data = List,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<Bodegas>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<IEnumerable<Regimen>> GetWmsRegimen(int BodegaId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<Regimen> List = (
                    from R in WmsDbO.Regimen
                    from B in WmsDbO.BodegaxRegimen
                    where B.Regimen == R.Idregimen
                    && B.BodegaId == BodegaId
                    orderby R.Regimen1
                    select R
                    );
                return new RetData<IEnumerable<Regimen>> {
                    Data = List,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<Regimen>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<string> SetIngresoExcelWms2(IEnumerable<WmsFileModel> ListProducts, int cboBodega, int cboRegimen) {
            DateTime StartTime = DateTime.Now;
            string LaFecha = "";
            //int MaxInventarioId = 0;
            if (ListProducts.Select(P => P.Barcode).Distinct().Count() != ListProducts.Count())
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Error, el CodProducto debe ser único",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            List<string> ListSql = new List<string>();
            try {
                List<Producto> ListProductsWms = (from P in WmsDbO.Producto select new Producto() { CodProducto = P.CodProducto, Existencia = P.Existencia }).ToList();
                List<Clientes> ListUploadClients = new List<Clientes>();
                List<int> ListDocXTran = new List<int>();
                if (ListProducts.Count() == 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, no hay filas a cargar",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Where(O => string.IsNullOrEmpty(O.ReciboAlmacen)).Count() > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, el recibo de almacen está vacio para un registro",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Select(O => O.ReciboAlmacen).Distinct().Count() > 1)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, hay más de un recibo de almacen",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                string ReciboAlmacen = ListProducts.Fod().ReciboAlmacen;
                List<DocumentosxTransaccion> ListC1 = (
                    from D in WmsDbO.DocumentosxTransaccion
                    from T in WmsDbO.Transacciones
                    where D.TransaccionId == T.TransaccionId
                    && D.InformeAlmacen == ReciboAlmacen
                    select D
                    ).ToList();
                if (ListC1.Count > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, existen informes de almacen duplicados, el numero de informe Almacen ya existe.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                List<int> ListC2Verif = ListProducts.Select(O3 => O3.Embalaje).Distinct().ToList();
                IEnumerable<UnidadMedida> ListEmbalajes = (
                    from Um in WmsDbO.UnidadMedida
                    from Lp in ListProducts
                    where Um.UnidadMedidaId == Lp.Embalaje
                    select Um
                    ).Distinct().ToList();
                List<int> ListC2 = ListEmbalajes.Select(E => E.UnidadMedidaId).Distinct().ToList();
                if (ListC2.Count != ListC2Verif.Count)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, el código de embalaje no existe",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                //where C.Nombre.Contains("payless", StringComparison.InvariantCultureIgnoreCase)
                List<Clientes> ListClientes = (from C in WmsDbO.Clientes orderby C.Nombre select C).ToList();
                Transacciones TNew = new Transacciones();
                //System.IO.StreamWriter Salida = new System.IO.StreamWriter("SetIngresoExcelWms2.sql", false);
                //Salida.WriteLine("");
                //Salida.Close();
                foreach (WmsFileModel Product in ListProducts) {
                    bool AllRackFull = true;
                    ListSql = new List<string> {
                        "SET XACT_ABORT ON" + Environment.NewLine,
                        "BEGIN TRANSACTION TRAN1" + Environment.NewLine,
                        "DECLARE @MaxTransaccionId INT;" + Environment.NewLine,
                        "DECLARE @MaxInventarioId INT;" + Environment.NewLine,
                        "DECLARE @MaxDTId INT;" + Environment.NewLine,
                        "DECLARE @MaxItemInventario INT;" + Environment.NewLine,
                        "DECLARE @MaxDetItemTran INT;" + Environment.NewLine,
                        "DECLARE @MaxDocTran INT;" + Environment.NewLine,
                        "BEGIN TRY" + Environment.NewLine
                    };
                    IEnumerable<Clientes> ListVerifCliente = ListClientes.Where(C2 => C2.Nombre.ToLower() == Product.Cliente.ToLower());
                    if (ListVerifCliente.Count() == 0) {
                        return new RetData<string> {
                            Info = new RetInfo() {
                                CodError = -1,
                                Mensaje = "No existe el cliente " + Product.Cliente,
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    } else {
                        Product.ClienteId = ListVerifCliente.Fod().ClienteId;
                    }
                    if (ListUploadClients.Where(Uc => Uc.ClienteId == Product.ClienteId).Count() == 0) {                        
                        //ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("Transacciones", "TransaccionId", "MaxTransaccionId"));
                        int MaxTransaccionId = (from T3 in WmsDbO.Transacciones select T3.TransaccionId).Max();
                        MaxTransaccionId++;
                        TNew = new Transacciones() {
                            TransaccionId = MaxTransaccionId,
                            NoTransaccion = "IN" + MaxTransaccionId.ToString().PadLeft(5, '0'),
                            IdtipoTransaccion = "IN",
                            FechaTransaccion = DateTime.Now,
                            BodegaId = cboBodega,
                            RegimenId = cboRegimen,
                            ClienteId = Product.ClienteId,
                            TipoIngreso = "IN",
                            Observacion = Product.Observaciones,
                            Usuariocrea = "Hilmer",
                            Fechacrea = DateTime.Now,
                            EstatusId = 5,
                            Exportadorid = Product.Exportador,
                            Destinoid = Product.Destino
                        };
                        WmsDbO.Transacciones.Add(TNew);
                        WmsDbO.SaveChanges();                        
                        //ListSql.Add(SqlGenHelper.GetSqlWmsInsertTransacciones(TNew));
                        ListUploadClients.Add(new Clientes() { ClienteId = Product.ClienteId });
                    }
                    ListSql.Add($"SET @MaxTransaccionId = {TNew.TransaccionId}; {Environment.NewLine}");
                    Producto PNew = new Producto();
                    if (ListProductsWms.Where(Pr1 => Pr1.CodProducto == Product.Barcode).Count() == 0) {
                        PNew = new Producto() {
                            CodProducto = Product.Barcode,
                            Descripcion = Product.Descripcion,
                            UnidadMedida = Product.UOM,
                            ClienteId = Product.ClienteId,
                            EstatusId = 1,
                            CategoriaId = 10,
                            CantMinima = 0,
                            Fecha = DateTime.Now,
                            Comentario = "INGRESOS DESDE INTRANET",
                            StockMaximo = 0,
                            Descargoid = 1,
                            Partida = "0"
                        };
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertProducto(PNew));
                        ListProductsWms.Add(new Producto() { CodProducto = Product.Barcode, Existencia = 1 });
                    } else {
                        PNew = ListProductsWms.Where(Pr1 => Pr1.CodProducto == Product.Barcode).Fod();
                        if (PNew.Existencia.HasValue) {
                            return new RetData<string> {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "El producto ya existe en la base de datos, CodProducto = " + Product.Barcode,
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                    }
                    for (int J = 1; J <= Product.Unidad; J++) {
                        AllRackFull = !(Product.RackId == 0);
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("Inventario", "InventarioId", "MaxInventarioId"));
                        Inventario INew = new Inventario() {
                            //InventarioId = MaxInventarioId,
                            //Barcode = "BRC" + MaxInventarioId.ToString().PadLeft(7, '0'),
                            FechaCreacion = DateTime.Now,
                            ClienteId = Product.ClienteId,
                            Descripcion = Product.Descripcion,
                            Declarado = Product.Unidad,
                            Valor = (Product.Valor / ((double)Product.Cantidad * (double)Product.Unidad)),
                            Articulos = 1,
                            Peso = Product.Peso / (double)Product.Piezas,
                            Volumen = (Product.Volumen / (double)Product.Piezas),
                            EstatusId = 2,
                            IsAgranel = false,
                            TipoBulto = Product.Embalaje,
                            Existencia = Product.Unidad,
                            Auditado = Product.Unidad,
                            CantidadInicial = Product.Unidad
                        };
                        if (Product.RackId != 0)
                            INew.Rack = Product.RackId;
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertInventario(INew));
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("DetalleTransacciones", "DtllTrnsaccionId", "MaxDTId"));
                        LaFecha = Product.Fecha;
                        DetalleTransacciones DTNew = new DetalleTransacciones() {
                            //DtllTrnsaccionId = MaxDTId,
                            //TransaccionId = TNew.TransaccionId,
                            //InventarioId = MaxInventarioId,
                            Conteo = 1,
                            Cantidad = Product.Unidad,
                            Valor = Convert.ToDecimal(Product.Valor / ((double)Product.Cantidad * (double)Product.Unidad)),
                            Fechaitem = Product.Fecha.ToDateFromEspDate(),
                            Rack = INew.Rack,
                            Embalaje = ListEmbalajes.Where(E2 => E2.UnidadMedidaId == Product.Embalaje).Fod().Simbolo,
                            IsEscaneado = INew.Rack > 0
                        };
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertDetalleTransacciones(DTNew));
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("ItemInventario", "ItemInventarioId", "MaxItemInventario"));
                        ItemInventario IINew = new ItemInventario() {
                            //ItemInventarioId = MaxItemInventario,
                            //InventarioId = MaxInventarioId,
                            CodProducto = Product.Barcode,
                            Declarado = Product.Unidad,
                            Precio = Product.ValorUnitario,
                            Observacion = "INGRESOS DESDE INTRANET",
                            Fechaitem = Product.Fecha.ToDateFromEspDate(),
                            Descripcion = Product.Descripcion,
                            Auditado = Product.Unidad,
                            Existencia = Product.Unidad,
                            CantidadInicial = Product.Unidad,
                            CodEquivale = Product.CodEquivalente,
                            PaisOrig = Product.PaisOrigen,
                            Lote = Product.Lote,
                            NumeroOc = Product.OrdenDeCompra.ToString(),
                            Modelo = Product.Modelo,
                            Color = Product.Color,
                            Estilo = Product.Estilo
                        };
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertItemInventario(IINew));
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("DtllItemTransaccion", "DtllItemTransaccionId", "MaxDetItemTran"));
                        DtllItemTransaccion ITNew = new DtllItemTransaccion() {
                            //DtllItemTransaccionId = MaxDetItemTran,
                            //TransaccionId = TNew.TransaccionId,
                            //DtllTransaccionId = DTNew.DtllTrnsaccionId,
                            //ItemInventarioId = MaxItemInventario,
                            Cantidad = Product.Unidad,
                            Precio = Product.ValorUnitario,
                            Rack = INew.Rack
                        };
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertDtllItemTransaccion(ITNew));
                        if (!string.IsNullOrEmpty(Product.Lote)) {
                            ItemParamaetroxProducto ParProdNew = new ItemParamaetroxProducto() {
                                //InventarioId = MaxInventarioId,
                                //ItemInventarioId = MaxItemInventario,
                                CodProducto = Product.Barcode,
                                ParametroId = 23,
                                ValParametro = Product.Valor.ToString()
                            };
                            ListSql.Add(SqlGenHelper.GetSqlWmsInsertItemParamaetroxProducto(ParProdNew));
                        }
                        if (!string.IsNullOrEmpty(Product.Modelo)) {
                            ItemParamaetroxProducto ParProdNew2 = new ItemParamaetroxProducto() {
                                //InventarioId = MaxInventarioId,
                                //ItemInventarioId = MaxItemInventario,
                                CodProducto = Product.Barcode,
                                ParametroId = 15,
                                ValParametro = Product.Modelo
                            };
                            ListSql.Add(SqlGenHelper.GetSqlWmsInsertItemParamaetroxProducto(ParProdNew2));
                        }
                    }
                    if (AllRackFull) {
                        ListSql.Add(SqlGenHelper.GetSqlWmsUpdateTransaccionesRackFull());
                    }
                    //fin de carga bultos
                    if (ListDocXTran.Where(D => D == Product.ClienteId).Count() == 0) {
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("DocumentosxTransaccion", "IddocxTransaccion", "MaxDocTran"));
                        DocumentosxTransaccion DocTranNew = new DocumentosxTransaccion() {
                            //IddocxTransaccion = MaxDocTran,
                            //TransaccionId = TNew.TransaccionId,
                            Fecha = DateTime.Now,
                            InformeAlmacen = ReciboAlmacen,
                            FeInformeAlmacen = Product.Fecha.ToDateFromEspDate()
                        };
                        if (Product.NumeroEntrada == 0 || !Product.NumeroEntrada.HasValue) {
                            DocTranNew.Im5 = Product.NumeroEntrada.ToString();
                            //DocTranNew.FeIm5 = Product.FechaIm5.ToDateFromEspDate();
                        }
                        if (Product.OrdenDeCompra != 0) {
                            DocTranNew.OrdenCompra = Product.OrdenDeCompra.ToString();
                        }
                        if (!string.IsNullOrEmpty(Product.NumeroFactura)) {
                            DocTranNew.FactComercial = Product.NumeroFactura;
                        }
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertDocumentosxTransaccion(DocTranNew));
                        ListDocXTran.Add(Product.ClienteId);
                    }
                    // fin if transaccionId
                    ListSql.Add(@"
COMMIT TRANSACTION TRAN1
END TRY
BEGIN CATCH	
	ROLLBACK TRANSACTION TRAN1
	PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	PRINT '@MaxTransaccionId = ' + CONVERT(VARCHAR(16), @MaxTransaccionId)
	PRINT '@MaxInventarioId = ' + CONVERT(VARCHAR(16), @MaxInventarioId)
	PRINT '@MaxDTId = ' + CONVERT(VARCHAR(16), @MaxDTId)
	PRINT '@MaxItemInventario = ' + CONVERT(VARCHAR(16), @MaxItemInventario)
	PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
	PRINT '@MaxDocTran = ' + CONVERT(VARCHAR(16), @MaxDocTran)
END CATCH
SET XACT_ABORT OFF
                        " + Environment.NewLine);
                    //System.IO.StreamWriter Salida2 = new System.IO.StreamWriter("SetIngresoExcelWms2.sql", true);
                    //ListSql.ForEach(S1 => Salida2.WriteLine(S1));
                    //Salida2.Close();
                    ManualDB.UploadBatch(ref WmsDbOLong, string.Join("", ListSql));
                    ListSql.Clear();
                }
                //ListSql.Add((DateTime.Now - StartTime).TotalSeconds.ToString());
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "Se cargo el archivo",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                System.IO.StreamWriter Salida = new System.IO.StreamWriter("Errores.txt", true);
                Salida.WriteLine(e1.ToString());
                Salida.WriteLine(LaFecha);
                Salida.Close();
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = $"Det: {e1.ToString()}",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpPost]
        public RetData<string> SetSalidaWmsFromEscaner(IEnumerable<string> ListProducts2, string dtpPeriodo, int cboBodegas, int cboRegimen) {
            DateTime StartTime = DateTime.Now;
            string ProductoSinExistencia = "";
            List<SpGeneraSalidaWMSModel> ListSp = new List<SpGeneraSalidaWMSModel>();
            List<string> ListSql = new List<string> {
                "SET XACT_ABORT ON" + Environment.NewLine,
                "BEGIN TRANSACTION TRAN1" + Environment.NewLine,
                "DECLARE @TransaccionID INT;" + Environment.NewLine,
                "DECLARE @MaxPedidoId INT;" + Environment.NewLine,
                "DECLARE @MaxPedidoDet INT;" + Environment.NewLine,
                "DECLARE @MaxDetTran INT;" + Environment.NewLine,
                "DECLARE @MaxDetItemTran INT;" + Environment.NewLine,
                "BEGIN TRY" + Environment.NewLine
            };
            try {
                List<SysTempSalidas> ListSysSalidas = new List<SysTempSalidas>();
                List<string> ListProducts = ListProducts2.ToList();
                List<System.Data.DataTable> ListDt = new List<System.Data.DataTable>();
                int ClienteID = 1432;
                int LocationID = 7;
                int RackID = 0;
                string FechaSalida = dtpPeriodo.ToDateFromEspDate().ToString("dd-MM-yyyy");
                //int TransaccionId = 116085;
                ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("Transacciones", "TransaccionId", "TransaccionID"));
                //ListSql.Add($"SET @TransaccionID = {TransaccionId};{Environment.NewLine}");
                Transacciones TNew = new Transacciones() {
                    IdtipoTransaccion = "SA",
                    FechaTransaccion = dtpPeriodo.ToDateFromEspDate(),
                    BodegaId = cboBodegas,
                    RegimenId = cboRegimen,
                    ClienteId = ClienteID,
                    TipoIngreso = "XL",
                    Observacion = "",
                    Usuariocrea = "Hilmer",
                    Fechacrea = DateTime.Now,
                    EstatusId = 4
                };
                ListSql.Add(SqlGenHelper.GetSqlWmsInsertTransaccionesOut(TNew));
                //Transacciones T = (from T1 in WmsDbO.Transacciones where T1.TransaccionId == TransaccionId select T1).Fod();
                //int MaxPedidoId = (from P in WmsDbO.Pedido select P.PedidoId).Max();
                //MaxPedidoId++;
                ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("Pedido", "PedidoId", "MaxPedidoId"));
                Pedido NewPedido = new Pedido() {
                    //PedidoId = MaxPedidoId,
                    Fechapedido = DateTime.Now,
                    ClienteId = ClienteID,
                    TipoPedido = "XL",
                    FechaRequerido = dtpPeriodo.ToDateFromEspDate(),
                    EstatusId = 8,
                    Observacion = "SALIDA GENERADA DE XLS Hilmer",
                    BodegaId = cboBodegas,
                    RegimenId = cboRegimen,
                    //PedidoBarcode = "PD" + MaxPedidoId.ToString().PadLeft(5, '0')
                };
                ListSql.Add(SqlGenHelper.GetSqlWmsInsertPedido2(NewPedido));
                ListSql.Add(SqlGenHelper.GetSqlWmsUpdateTransaccionesPedidoId());
                //WmsDbO.Pedido.Add(NewPedido);
                //T.PedidoId = MaxPedidoId;
                //WmsDbO.Transacciones.Update(T);
                //WmsDbO.SaveChanges();
                for (int i = 0; i < ListProducts.Count(); i++) {
                    ProductoSinExistencia = ListProducts[i];
                    System.Data.DataTable DtProdExists = ManualDB.SpGeneraSalidaWMS(ref WmsDbO, FechaSalida, ListProducts[i], cboBodegas, cboRegimen, ClienteID, LocationID, RackID);
                    if (DtProdExists != null) {
                        if (DtProdExists.Rows.Count > 0) {
                            ListSp.Add(new SpGeneraSalidaWMSModel() {
                                CodProducto = DtProdExists.Rows[0]["CodProducto"].ToString(),
                                InventarioID = DtProdExists.Rows[0]["InventarioID"].ToString(),
                                ItemInventarioID = DtProdExists.Rows[0]["ItemInventarioID"].ToString(),
                                Precio = DtProdExists.Rows[0]["precio"].ToString(),
                                Lote = DtProdExists.Rows[0]["lote"].ToString(),
                                Rack = DtProdExists.Rows[0]["rack"].ToString()
                            });
                        } else {
                            return new RetData<string> {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "Falta existencia para el producto " + ListProducts[i] + " por favor la fecha de la entrada para el producto.",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                    } else {
                        return new RetData<string> {
                            Info = new RetInfo() {
                                CodError = -1,
                                Mensaje = "Falta existencia para el producto " + ListProducts[i] + " por favor la fecha de la entrada para el producto.",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    }
                    //ListDt.Add(DtProdExists);
                    //int MaxPedidoDet = (from Pd in WmsDbO.DtllPedido select Pd.DtllPedidoId).Max();
                    //MaxPedidoDet++;
                    ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("DtllPedido", "DtllPedidoId", "MaxPedidoDet"));
                    DtllPedido PedidoDet = new DtllPedido() {
                        //DtllPedidoId = MaxPedidoDet,
                        //PedidoId = MaxPedidoId,
                        Cantidad = 1,
                        CodProducto = ListProducts[i]
                    };
                    ListSql.Add(SqlGenHelper.GetSqlWmsInsertDtllPedido(PedidoDet));
                    //WmsDbO.DtllPedido.Add(PedidoDet);
                    //WmsDbO.SaveChanges();
                    SysTempSalidas NewSysSalida = new SysTempSalidas() {
                        //TransaccionId = TransaccionId,
                        //PedidoId = MaxPedidoId,
                        InventarioId = Convert.ToInt32(ListSp.LastOrDefault().InventarioID),
                        //DtllPedidoId = MaxPedidoDet,
                        ItemInventarioId = Convert.ToInt32(ListSp.LastOrDefault().ItemInventarioID),
                        CodProducto = ListProducts[i],
                        Cantidad = 1,
                        Precio = Convert.ToDouble(ListSp.LastOrDefault().Precio),
                        Fecha = DateTime.Now,
                        Usuario = "HCAMPOS",
                        Lote = ListSp.LastOrDefault().Lote
                    };
                    ListSql.Add(SqlGenHelper.GetSqlWmsInsertSysTempSalidas(NewSysSalida));
                    //WmsDbO.SysTempSalidas.Add(NewSysSalida);
                    //WmsDbO.SaveChanges();
                    ListSysSalidas.Add(NewSysSalida);
                    //string strSQL = "Insert Into SysTempSalidas(TransaccionID, PedidoID, " +
                    //    "InventarioID, DtllPedidoID, ItemInventarioID, CodProducto, " +
                    //    "Cantidad, Precio, Fecha, Usuario,doc_fac,lote) Values(" + Valores + ")";
                }
                if (ListSp.Count != ListProducts.Count) {
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Faltan existencias de productos",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                for (int j = 0; j < ListSysSalidas.Count; j++) {
                    try {
                        //int MaxDetTran = (from Pd in WmsDbO.DetalleTransacciones select Pd.DtllTrnsaccionId).Max();
                        //MaxDetTran++;
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("DetalleTransacciones", "DtllTrnsaccionId", "MaxDetTran"));
                        DetalleTransacciones NewDetTran = new DetalleTransacciones() {
                            //DtllTrnsaccionId = MaxDetTran,
                            //TransaccionId = T.TransaccionId,
                            InventarioId = ListSysSalidas[j].InventarioId,
                            Conteo = 1,
                            Cantidad = 1,
                            Valor = Convert.ToDecimal(1.0 * Convert.ToDouble(ListSp[j].Precio)),
                            Rack = Convert.ToInt32(ListSp[j].Rack),
                            Embalaje = "CS",
                            IsEscaneado = false
                        };
                        //WmsDbO.DetalleTransacciones.Add(NewDetTran);
                        //WmsDbO.SaveChanges();
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertDetalleTransacciones2(NewDetTran));
                        //int MaxDetItemTran = (from Pd in WmsDbO.DtllItemTransaccion select Pd.DtllItemTransaccionId).Max();
                        //MaxDetItemTran++;
                        ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("DtllItemTransaccion", "DtllItemTransaccionId", "MaxDetItemTran"));
                        DtllItemTransaccion NewDetItemTran = new DtllItemTransaccion() {
                            //DtllItemTransaccionId = MaxDetItemTran,
                            //TransaccionId = T.TransaccionId,
                            //DtllTransaccionId = MaxDetTran,
                            ItemInventarioId = ListSysSalidas[j].ItemInventarioId,
                            Cantidad = 1,
                            Precio = Convert.ToDouble(ListSp[j].Precio),
                            Rack = Convert.ToInt32(ListSp[j].Rack)
                        };
                        ListSql.Add(SqlGenHelper.GetSqlWmsInsertDtllItemTransaccion2(NewDetItemTran));
                        //WmsDbO.DtllItemTransaccion.Add(NewDetItemTran);
                        //WmsDbO.SaveChanges();
                        //string strSQL = "Insert Into DtllItemTransaccion(DtllItemTransaccionID, " +
                        //    "TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio," +
                        //    "RACK) Values (" + nDtllItemTransaccionID + ", " + Valores + ")";
                    } catch (Exception e1) {
                        throw e1;
                    }                    
                }
                ListSql.Add(@"
                COMMIT TRANSACTION TRAN1
                END TRY
                BEGIN CATCH	
	                ROLLBACK TRANSACTION TRAN1
	                PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	                PRINT '@MaxPedidoId = ' + CONVERT(VARCHAR(16), @MaxPedidoId)
	                PRINT '@MaxPedidoDet = ' + CONVERT(VARCHAR(16), @MaxPedidoDet)
	                PRINT '@MaxDetTran = ' + CONVERT(VARCHAR(16), @MaxDetTran)
	                PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
                END CATCH
                --COMMIT TRANSACTION TRAN1
                --ROLLBACK TRANSACTION TRAN1
                SET XACT_ABORT OFF
                " + Environment.NewLine);
                ManualDB.UploadBatch(ref WmsDbOLong, string.Join("", ListSql));
                //ListSql.Add((DateTime.Now - StartTime).TotalSeconds.ToString());
                //System.IO.StreamWriter Salida = new System.IO.StreamWriter("sql.sql", false);
                //ListSql.ForEach(S => Salida.WriteLine(S));
                //Salida.Close();
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = $"Información cargada",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Fallo el CodProducto = " + ProductoSinExistencia + e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }            
        }
        [HttpGet]
        public RetData<int> TestVel1() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<Transacciones> List = (
                    from T in WmsDbO.Transacciones
                    where T.TransaccionId == 101029
                    select T
                    );
                return new RetData<int> {
                    Data = 1,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<int> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<int> TestVel2() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<Transacciones> List = ManualDB.GetTransaccionById(ref WmsDbO, 101029);
                return new RetData<int> {
                    Data = 1,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<int> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
    }
}