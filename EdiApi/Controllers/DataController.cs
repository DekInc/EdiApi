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
using System.Net.Mail;
using System.Net;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public EdiDBContext DbO;
        public EdiDBContext DbOLong;
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
        public DataController(EdiDBContext _DbO, EdiDBContext _DbOL, WmsContext _WmsDbO, WmsContext _WmsDbOLong, IConfiguration _Config) {
            DbO = _DbO;
            DbOLong = _DbOL;
            WmsDbO = _WmsDbO;            
            WmsDbOLong = _WmsDbOLong;
            Config = _Config;
            DbO.Database.SetCommandTimeout(TimeSpan.FromMinutes(2));
            DbOLong.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
            WmsDbO.Database.SetCommandTimeout(TimeSpan.FromMinutes(4));
            WmsDbOLong.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
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
            return "1.1.3.1";
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
                            InOut = (Pe.Inout == "I" ? "Pedido" : "Inventario"),
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
                     where Pe.Inout == "I"
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
                IEnumerable<ClientesModel> ListClients = (
                    from C in WmsDbO.Clientes                    
                    where C.ClienteId == 1432
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
        [HttpGet]
        public RetData<IEnumerable<FE830DataAux>> GetStockByClient(int ClienteId) {
            DateTime StartTime = DateTime.Now;
            RetData<IEnumerable<FE830DataAux>> RetDataO = new RetData<IEnumerable<FE830DataAux>>();
            try {
               return new RetData<IEnumerable<FE830DataAux>> {
                    Data = ManualDB.SP_GetExistenciasByCliente(ref DbO, ClienteId),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception exm1) {
                return new RetData<IEnumerable<FE830DataAux>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = exm1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<FE830DataAux>> GetStockByTienda(int ClienteId, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            IEnumerable<FE830DataAux> ListStock = ManualDB.SP_GetExistenciasByTienda(ref DbO, ClienteId, TiendaId);
            try {
                return new RetData<IEnumerable<FE830DataAux>> {
                    Data = ListStock,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception exm1) {
                return new RetData<IEnumerable<FE830DataAux>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = exm1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
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
        [HttpGet]
        public RetData<Tuple<string, string>> GetClientNameScheduleById(int TiendaId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                PaylessTiendas Tienda = (
                    from T in DbO.PaylessTiendas
                    where T.TiendaId == TiendaId
                    select T
                    ).Fod();
                return new RetData<Tuple<string, string>>
                {
                    Data = new Tuple<string, string>(Tienda.Descr, Tienda.HorarioEntrega),
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
                return new RetData<Tuple<string, string>> {
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
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternos(int ClienteId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == ClienteId
                    orderby Pe.Id descending
                    select Pe
                    );
                IEnumerable<PedidosDetExternos> ListDePe = ManualDB.SP_GetPedidosDetExternos(ref DbO, ClienteId);                
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
        [HttpGet]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternosByTienda(int ClienteId, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == ClienteId
                    && Pe.TiendaId == TiendaId
                    orderby Pe.Id descending
                    select Pe
                    );
                IEnumerable<PedidosDetExternos> ListDePe = ManualDB.SP_GetPedidosDetExternos(ref DbO, ClienteId);
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
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> GetPedidosExternosPendientesByTienda(int ClienteId, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosExternos> ListPe = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == ClienteId
                    && Pe.TiendaId == TiendaId
                    && Pe.IdEstado == 2
                    orderby Pe.Id descending
                    select Pe
                    );
                List<PedidosDetExternos> ListDePe = ManualDB.SP_GetPedidosDetExternosPendientesByTienda(ref DbO, ClienteId, TiendaId).ToList();
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
        [HttpGet]
        public RetData<IEnumerable<PedidosWmsModel>> GetWmsGroupDispatchs(int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosWmsModel> ListDis = ManualDB.GetWmsGroupDispatchs(ref DbO, ClienteId);
                ListDis = (
                    from Ld in ListDis
                    group Ld by new { Ld.PedidoId } into G
                    orderby G.Fod().FechaPedido.ToDateFromEspDate() descending
                    select new PedidosWmsModel() {
                        ClienteId = G.Fod().ClienteId,
                        PedidoBarcode = G.Fod().PedidoBarcode,
                        FechaPedido = G.Fod().FechaPedido,
                        Estatus = G.Fod().Estatus,
                        NomBodega = G.Fod().NomBodega,
                        Regimen = G.Fod().Regimen,
                        Bultos = G.Sum(O1 => O1.Bultos),
                        Cantidad = G.Sum(O1 => O1.Cantidad),
                        Observacion = G.Fod().Observacion,                        
                        PedidoId = G.Fod().PedidoId,
                        TiendaId = string.Join(", ", G.Select(O2 => O2.TiendaId).Distinct()),
                        Destino = G.Fod().Destino,
                        Total = G.Fod().Total
                    }
                ).Distinct();
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Data = ListDis,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PedidosWmsModel>> GetWmsGroupDispatchsBills(int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosWmsModel> ListDis = ManualDB.GetWmsGroupDispatchsBills(ref DbO, ClienteId);
                ListDis = (
                    from Ld in ListDis
                    group Ld by new { Ld.PedidoId } into G
                    select new PedidosWmsModel() {
                        ClienteId = G.Fod().ClienteId,
                        PedidoBarcode = G.Fod().PedidoBarcode,
                        FechaPedido = G.Fod().FechaPedido,
                        Estatus = G.Fod().Estatus,
                        NomBodega = G.Fod().NomBodega,
                        Regimen = G.Fod().Regimen,
                        Bultos = G.Sum(O1 => O1.Bultos),
                        Cantidad = G.Sum(O1 => O1.Cantidad),
                        Observacion = G.Fod().Observacion,
                        PedidoId = G.Fod().PedidoId,
                        TiendaId = string.Join(", ", G.Select(O2 => O2.TiendaId).Distinct()),
                        FactComercial = string.Join(", ", G.Select(O2 => O2.FactComercial).Distinct()),
                        TransaccionId = G.Fod().TransaccionId,
                        Destino = G.Fod().Destino
                    }
                ).Distinct();
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Data = ListDis,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PedidosWmsModel>> GetWmsDetDispatchsBills(int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosWmsModel> ListDis = ManualDB.GetWmsDetDispatchsBills(ref DbO, ClienteId);                
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Data = ListDis,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PedidosWmsModel>> GetPedidosMWmsByTienda(int ClienteId, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PedidosWmsModel> ListDis = ManualDB.SP_GetPedidosMWmsByTienda(ref DbO, ClienteId, TiendaId);
                ListDis = (
                    from Ld in ListDis
                    group Ld by Ld.PedidoId into G
                    select new PedidosWmsModel() {
                        Cantidad = 0,
                        ClienteId = G.Fod().ClienteId,
                        Estatus = G.Fod().Estatus,
                        FechaPedido = G.Fod().FechaPedido,
                        NomBodega = G.Fod().NomBodega,
                        Observacion = G.Fod().Observacion,
                        PedidoBarcode = G.Fod().PedidoBarcode,
                        PedidoId = G.Fod().PedidoId,
                        Regimen = G.Fod().Regimen,
                        Total = G.Fod().Total
                    }
                ).Distinct();
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Data = ListDis,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PedidosWmsModel>> {
                    Info = new RetInfo() {
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
        [HttpGet]
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetPaylessProdPrioriAll() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriDetModel> ListPaylessProdPrioriDet = (
                    from D in DbO.PaylessProdPrioriDet
                    from T in DbO.PaylessTransporte
                    where T.Id == D.IdTransporte
                    select new PaylessProdPrioriDetModel {
                        Id = D.Id,
                        Barcode = D.Barcode,
                        Categoria = D.Categoria,
                        Cp = D.Cp,
                        Transporte = T.Transporte
                    });
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
        public RetData<PaylessProdPrioriArchM> SetPaylessProdPrioriFile(Tuple<IEnumerable<PaylessProdPrioriArchDet>, IEnumerable<PaylessProdPrioriArchDet>> TupleBarcodes, int IdTransporte, string Periodo, string codUsr, int cboTipo)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PaylessProdPrioriArchM> ListPaylessProdPrioriArchM = DbO.PaylessProdPrioriArchM.Where(Pp => Pp.Periodo == Periodo && Pp.IdTransporte == IdTransporte);
                if (ListPaylessProdPrioriArchM.Count() > 0 && cboTipo == 0)
                {
                    IEnumerable<PaylessProdPrioriArchDet> ListPaylessProdPrioriArchDet = DbO.PaylessProdPrioriArchDet.Where(Pd => Pd.IdM == ListPaylessProdPrioriArchM.Fod().Id);
                    DbO.PaylessProdPrioriArchDet.RemoveRange(ListPaylessProdPrioriArchDet);
                }
                PaylessProdPrioriArchM NewMas = new PaylessProdPrioriArchM();
                if (ListPaylessProdPrioriArchM.Count() > 0 && cboTipo == 0)
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
                        CodUsr = codUsr,
                        Typ = cboTipo
                    };
                    DbO.PaylessProdPrioriArchM.Add(NewMas);
                }                
                DbO.SaveChanges();
                foreach (PaylessProdPrioriArchDet Uf in TupleBarcodes.Item1)
                {
                    DbO.PaylessProdPrioriArchDet.Add(new PaylessProdPrioriArchDet()
                    {
                        IdM = NewMas.Id,
                        Barcode = Uf.Barcode
                    });
                }
                DbO.SaveChanges();
                if (cboTipo == 1) {
                    foreach (PaylessProdPrioriArchDet Uf2 in TupleBarcodes.Item2) {
                        DbO.PaylessProdPrioriArchDet.Add(new PaylessProdPrioriArchDet() {
                            IdM = NewMas.Id,
                            Barcode = "-" + Uf2.Barcode 
                        });
                    }
                    DbO.SaveChanges();
                }
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
                        PorValid = Pe.PorcValidez,
                        CantEscaner = Pe.CantEscaner,
                        CantExcel = Pe.CantExcel,
                        Typ = Pe.Typ                        
                    }).ToList();                
                for(int Ci = 0; Ci < ListArchMaMo.Count(); Ci++)
                {
                    if (ListArchMaMo[Ci].PorValid == null && ListArchMaMo[Ci].Typ == 0) {
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
                        Am.CantExcel = ListExcelOriginal.Count();
                        Am.CantEscaner = ListEscaneados.Count();
                        DbO.PaylessProdPrioriArchM.Update(Am);
                        ListArchMaMo[Ci].PorValid = Am.PorcValidez;
                        ListArchMaMo[Ci].CantExcel = Am.CantExcel;
                        ListArchMaMo[Ci].CantEscaner = Am.CantEscaner;
                        DbO.SaveChanges();
                    } else if(ListArchMaMo[Ci].PorValid == null && ListArchMaMo[Ci].Typ == 1) {
                        IEnumerable<string> ListOut = (                           
                            from Pe in DbO.PaylessProdPrioriArchDet
                            where Pe.IdM == ListArchMaMo[Ci].Id
                            && Pe.Barcode[0] != '-'
                            select Pe.Barcode
                            ).Distinct();
                        IEnumerable<string> ListIn = (
                            from Pe in DbO.PaylessProdPrioriArchDet
                            where Pe.IdM == ListArchMaMo[Ci].Id
                            && Pe.Barcode[0] == '-'
                            select Pe.Barcode
                            ).Distinct();
                        double PorcMatch = 1.0 - ((double)Math.Abs((double)ListOut.Count() - (double)ListIn.Count())) / (double)ListOut.Count();
                        PaylessProdPrioriArchM Am = DbO.PaylessProdPrioriArchM.Where(O => O.Id == ListArchMaMo[Ci].Id).Fod();
                        Am.PorcValidez = Math.Round(PorcMatch * 100.0, 3);
                        Am.CantExcel = ListOut.Count();
                        Am.CantEscaner = ListIn.Count();
                        DbO.PaylessProdPrioriArchM.Update(Am);
                        DbO.SaveChanges();
                    }
                }
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
                        IEnumerable<PaylessProdPrioriDetModel> Ret = ManualDB.SP_GetPaylessProdPrioriFileDif(ref DbOLong, idData, idProdArch);
                        return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                            Data = Ret,
                            Info = new RetInfo() {
                                CodError = 0,
                                Mensaje = "ok",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    case 2:
                        List<PaylessProdPrioriDetModel> ListOriginal = ManualDB.SP_GetPaylessProdPrioriFileDif(ref DbOLong, 2, idProdArch).ToList();
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
        public RetData<IEnumerable<Clientes>> GetClients() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<Clientes> ListUsers = WmsDbO.Clientes.Select(C => new Clientes() { ClienteId = C.ClienteId, Nombre = C.Nombre }).OrderBy(C2 => C2.Nombre);
                return new RetData<IEnumerable<Clientes>> {
                    Data = ListUsers,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<Clientes>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetTransDif(int IdM)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                IEnumerable<PaylessProdPrioriDetModel> List = ManualDB.SP_GetTransDif(ref DbO, IdM);
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>>
                {
                    Data = List,
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
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>>
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
        public RetData<IEnumerable<PaylessProdPrioriDetModel>> GetPaylessProdTallaLoteFil(string TxtBarcode, string CboProducto, string CboTalla, string CboLote, string CboCategoria, string CodUser) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriDetModel> List = ManualDB.SP_GetPaylessProdTallaLoteFil(ref DbO, TxtBarcode, CboProducto, CboTalla, CboLote, CboCategoria, CodUser);
                return new RetData<IEnumerable<PaylessProdPrioriDetModel>> {
                    Data = List,
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
        [HttpGet]
        public RetData<bool> GetSetExistenciasByCliente(int ClienteId, string CodUser) {
            DateTime StartTime = DateTime.Now;
            try {
                ManualDB.SP_GetSetExistenciasByCliente(ref DbO, ClienteId, CodUser);
                return new RetData<bool> {
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<bool> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>> GetProductoTallaLoteCategoria() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<int> ListProduct = (from D in DbO.PaylessProdPrioriDet orderby D.Producto select Convert.ToInt32(D.Producto)).Distinct().OrderBy(O => O);
                IEnumerable<string> ListTalla = (from D in DbO.PaylessProdPrioriDet orderby D.Talla select D.Talla).Distinct();
                IEnumerable<int> ListLote = (from D in DbO.PaylessProdPrioriDet orderby D.Lote select Convert.ToInt32(D.Lote)).Distinct().OrderBy(O2 => O2);
                IEnumerable<string> ListCat = (from D in DbO.PaylessProdPrioriDet orderby D.Categoria select D.Categoria).Distinct();
                return new RetData<Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>> {
                    Data = new Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>(ListProduct, ListTalla, ListLote, ListCat),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>> {
                    Info = new RetInfo() {
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
        public RetData<IEnumerable<WmsFileModel>> GetWmsFile(string Period, int IdTransport, int Typ) { 
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessProdPrioriDetModel> ListData;
                if (Typ == 0) {
                    ListData = ManualDB.SP_GetPaylessProdPrioriByPeriodAndIdTransport(ref DbO, Period, IdTransport);                    
                } else {
                    ListData = ManualDB.GetWmsFileById(ref DbO, Typ);
                }
                IEnumerable<WmsFileModel> ListRep = (
                        from Ex in ListData
                        group new { Ex } by new { Ex.Barcode } into G
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
                            Exportador = 435,
                            Destino = 35308,
                            PaisOrigen = 166,
                            Cp = string.Join(" ", G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Cp)).Select(O2 => O2.Ex.Cp).Distinct().ToArray()),
                            Cont = G.Count(),
                            Modelo = G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Talla)).Select(O2 => O2.Ex.Talla).Distinct().Count() == 1 ? G.Fod().Ex.Talla : "Varios",
                            Lote = G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Producto)).Select(O2 => O2.Ex.Producto).Distinct().Count() == 1 ? G.Fod().Ex.Producto : "Varios",
                            Estilo = G.Where(O1 => !string.IsNullOrEmpty(O1.Ex.Lote)).Select(O2 => O2.Ex.Lote).Distinct().Count() == 1 ? G.Fod().Ex.Lote : "Varios",
                            Transporte = G.Fod().Ex.Transporte,
                            CodEquivalente = G.Fod().Ex.Pri
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
        //[HttpGet]
        //public RetData<IEnumerable<PaylessReportWeekGModel>> GetReportWeek(int Tipo) {
        //    DateTime StartTime = DateTime.Now;
        //    try {
        //        //Jueves
        //        List<PaylessReportWeekGModel> Ret = new List<PaylessReportWeekGModel>();
        //        List<PaylessReportes> ListReportesExistentes = (
        //            from R in DbO.PaylessReportes
        //            orderby R.Periodo.ToDateFromEspDate() descending
        //            select R
        //            ).ToList();
        //        DateTime DateInit, DateEnd, DateMid;
        //        DateTime CurrentPeriod = DateTime.Now;
        //        if (ListReportesExistentes != null) {
        //            if (ListReportesExistentes.Count() > 0) {
        //                CurrentPeriod = ListReportesExistentes.Fod().Periodo.ToDateFromEspDate().AddDays(7);
        //                if (CurrentPeriod.DayOfWeek == DayOfWeek.Monday) {
        //                    DateInit = CurrentPeriod;
        //                } else {
        //                    DateInit = (CurrentPeriod.AddDays(DayOfWeek.Monday - CurrentPeriod.DayOfWeek));
        //                }
        //            } else {
        //                DateInit = (CurrentPeriod.AddDays(DayOfWeek.Monday - CurrentPeriod.DayOfWeek));
        //            }
        //        } else {
        //            DateInit = (CurrentPeriod.AddDays(DayOfWeek.Monday - CurrentPeriod.DayOfWeek));
        //        }
        //        DateEnd = DateInit.AddDays(DayOfWeek.Friday - DateInit.DayOfWeek);
        //        DateMid = DateInit.AddDays(DayOfWeek.Wednesday - DateInit.DayOfWeek);                
        //        List<PedidosExternos> ListPedidosM = (
        //            from Pe in DbO.PedidosExternos
        //            where Pe.FechaPedido.Substring(0, 10).ToDateFromEspDate() >= DateInit 
        //            && Pe.FechaPedido.Substring(0, 10).ToDateFromEspDate() <= DateEnd
        //            && Pe.IdEstado == 2
        //            orderby Pe.FechaPedido
        //            select Pe
        //            ).ToList();
        //        if (ListPedidosM.Count() == 3) {
        //            DateInit = ListPedidosM[2].FechaPedido.Substring(0, 10).ToDateFromEspDate();
        //            DateMid = ListPedidosM[1].FechaPedido.Substring(0, 10).ToDateFromEspDate();
        //            DateEnd = ListPedidosM[0].FechaPedido.Substring(0, 10).ToDateFromEspDate();
        //        }
        //        if (ListPedidosM != null) {
        //            if (ListPedidosM.Count() > 0) {
        //                Clientes ClienteAct = (from C in WmsDbO.Clientes where C.ClienteId == ListPedidosM.Fod().ClienteId select C)?.Fod();
        //                if (ClienteAct != null) {
        //                    PaylessTiendas Tienda = (from T in DbO.PaylessTiendas where T.ClienteId == ListPedidosM.Fod().ClienteId select T)?.Fod();
        //                    IEnumerable<PedidosDetExternos> ListDetTotal = (
        //                        from D in DbO.PedidosDetExternos
        //                        from M in ListPedidosM
        //                        where D.PedidoId == M.Id
        //                        select D
        //                        );
        //                    Ret.Add(new PaylessReportWeekGModel() {
        //                        TiendaId = Tienda.TiendaId ?? 0,
        //                        Location = ClienteAct?.Nombre,
        //                        Manager = Tienda?.Lider,
        //                        Tel = Tienda?.Tel
        //                    });
        //                    for (int i = 0; i < 3; i++) {
        //                        switch (i) {
        //                            case 0:                                            
        //                                Ret[Ret.Count - 1].TotalBox = Convert.ToInt32(Math.Round(ListDetTotal.Sum(O => O.CantPedir.Value), 0));                                        
        //                                Ret[Ret.Count - 1].Date1 = DateInit.ToString(ApplicationSettings.DateTimeFormatShort);
        //                                Ret[Ret.Count - 1].Date2 = DateMid.ToString(ApplicationSettings.DateTimeFormatShort);
        //                                Ret[Ret.Count - 1].Date3 = DateEnd.ToString(ApplicationSettings.DateTimeFormatShort);
        //                                IEnumerable<PedidosDetExternos> ListDet1 = ManualDB.SP_GetPedidosDetExternosByDate(ref DbO, DateInit, DateMid, 0);
        //                                if (ListDet1.Count() > 0) {
        //                                    //Accesories = ListDet1.Where(O1 => O1.)
        //                                    Ret[Ret.Count - 1].Total1 = Convert.ToInt32(Math.Round(ListDet1.Sum(O => O.CantPedir.Value), 0));
        //                                    PedidosExternos PedidoMonday = ListPedidosM.Where(P => P.FechaPedido.Substring(0, 10).ToDateFromEspDate() == DateInit)?.Fod();
        //                                    if (PedidoMonday != null)
        //                                        Ret[Ret.Count - 1].Time1 = PedidoMonday.FechaPedido.ToDateEsp().ToString(ApplicationSettings.ToTimeFormatExcel);
        //                                    else {
        //                                        Ret[Ret.Count - 1].Date1 = "SIN PEDIDO";
        //                                        Ret[Ret.Count - 1].Time1 = string.Empty;
        //                                    }
        //                                    Ret[Ret.Count - 1].Accessories += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("acce", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Kids += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("niñ", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Man += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("caball", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Ladies += Convert.ToInt32(Math.Round(ListDet1.Where(O2 => O2.Producto.Contains("dama", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                } else {
        //                                    Ret[Ret.Count - 1].Total1 = 0;
        //                                    Ret[Ret.Count - 1].Time1 = string.Empty;
        //                                }
        //                                break;
        //                            case 1:
        //                                IEnumerable<PedidosDetExternos> ListDet2 = ManualDB.SP_GetPedidosDetExternosByDate(ref DbO, DateMid, DateEnd, 0);
        //                                if (ListDet2.Count() > 0) {
        //                                    Ret[Ret.Count - 1].Total2 = Convert.ToInt32(Math.Round(ListDet2.Sum(O => O.CantPedir.Value), 0));
        //                                    PedidosExternos PedidoWednesday = ListPedidosM.Where(P => P.FechaPedido.Substring(0, 10).ToDateFromEspDate() == DateMid)?.Fod();
        //                                    if (PedidoWednesday != null)
        //                                        Ret[Ret.Count - 1].Time2 = PedidoWednesday.FechaPedido.ToDateEsp().ToString(ApplicationSettings.ToTimeFormatExcel);
        //                                    else
        //                                        Ret[Ret.Count - 1].Time2 = string.Empty;
        //                                    Ret[Ret.Count - 1].Accessories += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("acce", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Kids += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("niñ", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Man += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("caball", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Ladies += Convert.ToInt32(Math.Round(ListDet2.Where(O2 => O2.Producto.Contains("dama", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                } else {
        //                                    Ret[Ret.Count - 1].Total2 = 0;
        //                                    Ret[Ret.Count - 1].Time2 = string.Empty;
        //                                }
        //                                break;
        //                            case 2:
        //                                IEnumerable<PedidosDetExternos> ListDet3 = ManualDB.SP_GetPedidosDetExternosByDate(ref DbO, DateEnd, DateEnd.AddDays(2), 0);                                        
        //                                if (ListDet3.Count() > 0) {
        //                                    Ret[Ret.Count - 1].Total3 = Convert.ToInt32(Math.Round(ListDet3.Sum(O => O.CantPedir.Value), 0));
        //                                    PedidosExternos PedidoFriday = ListPedidosM.Where(P => P.FechaPedido.Substring(0, 10).ToDateFromEspDate() == DateEnd)?.Fod();
        //                                    if (PedidoFriday != null)
        //                                        Ret[Ret.Count - 1].Time3 = PedidoFriday.FechaPedido.ToDateEsp().ToString(ApplicationSettings.ToTimeFormatExcel);
        //                                    else
        //                                        Ret[Ret.Count - 1].Time3 = string.Empty;
        //                                    Ret[Ret.Count - 1].Accessories += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("acce", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Kids += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("niñ", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Man += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("caball", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                    Ret[Ret.Count - 1].Ladies += Convert.ToInt32(Math.Round(ListDet3.Where(O2 => O2.Producto.Contains("dama", StringComparison.InvariantCultureIgnoreCase)).Sum(O => O.CantPedir.Value), 0));
        //                                } else {
        //                                    Ret[Ret.Count - 1].Total3 = 0;
        //                                    Ret[Ret.Count - 1].Time3 = string.Empty;
        //                                }
        //                                break;
        //                            default:
        //                                break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return new RetData<IEnumerable<PaylessReportWeekGModel>> {
        //            Data = Ret,
        //            Info = new RetInfo() {
        //                CodError = 0,
        //                Mensaje = "ok",
        //                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
        //            }
        //        };
        //    } catch (Exception e1) {
        //        return new RetData<IEnumerable<PaylessReportWeekGModel>> {
        //            Info = new RetInfo() {
        //                CodError = -1,
        //                Mensaje = e1.ToString(),
        //                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
        //            }
        //        };
        //    }
        //}
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
        public RetData<string> SetIngresoExcelWms2(IEnumerable<WmsFileModel> ListProducts, int cboBodega, int cboRegimen, string CodUser) {
            DateTime StartTime = DateTime.Now;
            int MaxTransaccionId = 0;
            //int MaxInventarioId = 0;
            if (ListProducts.Select(P => P.Barcode).Distinct().Count() != ListProducts.Count())
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -2,
                        Mensaje = "Error, el CodProducto debe ser único",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            List<string> ListSql = new List<string>();
            AsyncStates ThisProc = new AsyncStates();            
            try {
                List<Producto> ListProductsWms = (from P in WmsDbOLong.Producto where P.ClienteId == 1432 select new Producto() { CodProducto = P.CodProducto, Existencia = P.Existencia }).ToList();
                List<Clientes> ListUploadClients = new List<Clientes>();
                List<int> ListDocXTran = new List<int>();
                if (ListProducts.Count() == 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
                            Mensaje = "Error, no hay filas a cargar",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Where(O => string.IsNullOrEmpty(O.ReciboAlmacen)).Count() > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
                            Mensaje = "Error, el recibo de almacen está vacio para un registro",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Where(O => O.RackId == 0).Count() > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
                            Mensaje = "Error, el rack está vacio.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Where(O => !O.Valor.HasValue).Count() > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
                            Mensaje = "Error, la columna está vacia.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Where(O => !O.ValorUnitario.HasValue).Count() > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
                            Mensaje = "Error, el valor unitario está vacio.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                IEnumerable<int> ListProdRacks = ListProducts.Select(Lp => Lp.RackId).Distinct();
                List<Racks> ListRacksTemp = (
                    from R in WmsDbO.Racks
                    from Lpr in ListProdRacks
                    where R.Rack == Lpr
                    select R
                    ).ToList();
                foreach (Racks R in ListRacksTemp) {
                    if (R.BodegaId != cboBodega)
                        return new RetData<string> {
                            Info = new RetInfo() {
                                CodError = -2,
                                Mensaje = "Error, Rack no pertenece a la bodega seleccionada.",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                }                
                if (ListProducts.Where(O => !O.ValorUnitario.HasValue).Count() > 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
                            Mensaje = "Error, el valor unitario está vacio.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (ListProducts.Select(O => O.ReciboAlmacen).Distinct().Count() > 1)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -2,
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
                            CodError = -2,
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
                            CodError = -2,
                            Mensaje = "Error, el código de embalaje no existe",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                List<Clientes> ListClientes = (from C in WmsDbO.Clientes orderby C.Nombre select C).ToList();
                Transacciones TNew = new Transacciones();
                //System.IO.StreamWriter Salida = new System.IO.StreamWriter("Errores.txt", false);
                //Salida.WriteLine("Comienza ingreso.");
                //Salida.Close();                
                int NTran = 1;
                IEnumerable<AsyncStates> ListAsyncs = (from A in DbO.AsyncStates where A.Typ == 0 && A.CodUser == CodUser select A);
                if (ListAsyncs.Count() == 0) {
                    ThisProc = new AsyncStates() { Typ = 0, Val = 0, Maximum = ListProducts.Count(), CodUser = CodUser };
                    DbO.AsyncStates.Add(ThisProc);
                    DbO.SaveChanges();
                } else {
                    ThisProc = ListAsyncs.Fod();
                    ThisProc.Mess = string.Empty;
                    ThisProc.Val = 0;
                }
                foreach (WmsFileModel Product in ListProducts) {                    
                    //Salida = new System.IO.StreamWriter("Errores.txt", true);
                    //Salida.WriteLine(Product.Barcode);
                    //Salida.Close();
                    ThisProc.Val++;
                    DbO.AsyncStates.Update(ThisProc);
                    DbO.SaveChanges();
                    bool AllRackFull = true;
                    ListSql = new List<string> {
                        "SET XACT_ABORT ON" + Environment.NewLine,
                        $"--BEGIN TRANSACTION TRAN{NTran}" + Environment.NewLine,
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
                                CodError = -2,
                                Mensaje = "No existe el cliente " + Product.Cliente,
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    } else {
                        Product.ClienteId = ListVerifCliente.Fod().ClienteId;
                    }
                    if (ListUploadClients.Where(Uc => Uc.ClienteId == Product.ClienteId).Count() == 0) {                        
                        //ListSql.Add(SqlGenHelper.GetSqlWmsMaxTbl("Transacciones", "TransaccionId", "MaxTransaccionId"));
                        MaxTransaccionId = (from T3 in WmsDbOLong.Transacciones select T3.TransaccionId).Max();
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
                            Observacion = (string.IsNullOrEmpty(Product.Observaciones)? " " : Product.Observaciones),
                            Usuariocrea = "Hilmer",
                            Fechacrea = DateTime.Now,
                            EstatusId = 5,
                            Exportadorid = Product.Exportador,
                            Destinoid = Product.Destino
                        };
                        try {
                            WmsDbOLong.Transacciones.Add(TNew);
                            WmsDbOLong.SaveChanges();
                        } catch (Exception e2) {
                            IEnumerable<Transacciones> TOld = (from T2 in WmsDbOLong.Transacciones where T2.TransaccionId == MaxTransaccionId select T2);
                            if (TOld.Count() == 0) {
                                throw e2;
                            }
                        }                        
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
                            IEnumerable<Transacciones> TDel = (from T in WmsDbOLong.Transacciones where T.TransaccionId == MaxTransaccionId select T);
                            if (TDel.Count() > 0) {
                                WmsDbOLong.Remove(TDel.Fod());
                                WmsDbOLong.SaveChanges();
                            }
                            return new RetData<string> {
                                Info = new RetInfo() {
                                    CodError = -2,
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
                        if (!string.IsNullOrEmpty(Product.OrdenDeCompra)) {
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
--COMMIT TRANSACTION TRAN" + NTran.ToString() + @"
END TRY
BEGIN CATCH	
	--ROLLBACK TRANSACTION TRAN" + NTran.ToString() + @"
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
                    NTran++;
                }
                //System.IO.StreamWriter Salida2 = new System.IO.StreamWriter("Errores.txt", true);
                //Salida2.WriteLine("TotalSeconds = " + (DateTime.Now - StartTime).TotalSeconds.ToString());
                //Salida2.Close();
                //ListSql.Add((DateTime.Now - StartTime).TotalSeconds.ToString());
                ThisProc.Mess = "Se cargo el archivo";
                DbO.AsyncStates.Update(ThisProc);
                DbO.SaveChanges();
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "Se cargo el archivo",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                ThisProc.Mess = "Error al cargar el archivo. " + e1.ToString();
                DbO.AsyncStates.Update(ThisProc);
                DbO.SaveChanges();
                System.IO.StreamWriter Salida2 = new System.IO.StreamWriter("Errores.txt", true);
                Salida2.WriteLine(e1.ToString());
                Salida2.WriteLine(string.Join("", ListSql));
                Salida2.WriteLine("TotalSeconds = " + (DateTime.Now - StartTime).TotalSeconds.ToString());
                Salida2.Close();
                if (MaxTransaccionId != 0) {
                    try {
                        IEnumerable<Transacciones> TDel = (from T in WmsDbOLong.Transacciones where T.TransaccionId == MaxTransaccionId select T);
                        if (TDel.Count() > 0) {
                            IEnumerable<ItemParamaetroxProducto> Lb1 = (
                                from B1 in WmsDbOLong.ItemParamaetroxProducto
                                from Ii in WmsDbOLong.ItemInventario
                                from Dt in WmsDbOLong.DetalleTransacciones
                                where Dt.TransaccionId == TDel.Fod().TransaccionId
                                && Ii.InventarioId == Dt.InventarioId
                                && B1.ItemInventarioId == Ii.ItemInventarioId
                                select B1
                                );
                            if (Lb1.Count() > 0)
                                WmsDbOLong.ItemParamaetroxProducto.RemoveRange(Lb1);
                            IEnumerable<DtllItemTransaccion> Lb2 = (from B2 in WmsDbOLong.DtllItemTransaccion where B2.TransaccionId == TDel.Fod().TransaccionId select B2);
                            if (Lb2.Count() > 0)
                                WmsDbOLong.DtllItemTransaccion.RemoveRange(Lb2);
                            IEnumerable<ItemInventario> Lb4 = (
                                from Ii in WmsDbOLong.ItemInventario
                                from Dt in WmsDbOLong.DetalleTransacciones
                                where Ii.InventarioId == Dt.InventarioId
                                && Dt.TransaccionId == TDel.Fod().TransaccionId
                                select Ii
                                );
                            if (Lb4.Count() > 0)
                                WmsDbOLong.ItemInventario.RemoveRange(Lb4);
                            IEnumerable<Inventario> Lb5 = (
                                from I in WmsDbOLong.Inventario
                                from Dt in WmsDbOLong.DetalleTransacciones
                                where Dt.TransaccionId == TDel.Fod().TransaccionId
                                && I.InventarioId == Dt.InventarioId
                                select I
                                );
                            if (Lb5.Count() > 0)
                                WmsDbOLong.Inventario.RemoveRange(Lb5);
                            IEnumerable<DocumentosxTransaccion> Lb6 = (from B6 in WmsDbOLong.DocumentosxTransaccion where B6.TransaccionId == TDel.Fod().TransaccionId select B6);
                            if (Lb6.Count() > 0)
                                WmsDbOLong.DocumentosxTransaccion.RemoveRange(Lb6);
                            IEnumerable<DetalleTransacciones> Lb3 = (from B3 in WmsDbOLong.DetalleTransacciones where B3.TransaccionId == TDel.Fod().TransaccionId select B3);
                            if (Lb3.Count() > 0)
                                WmsDbOLong.DetalleTransacciones.RemoveRange(Lb3);
                            IEnumerable<DetalleIngresoCliente> L2 = (from I2 in WmsDbOLong.DetalleIngresoCliente where I2.TransaccionId == TDel.Fod().TransaccionId select I2);
                            if (L2.Count() > 0)
                                WmsDbOLong.DetalleIngresoCliente.RemoveRange(L2);
                            IEnumerable<DtllReceivexItemInventario> L1 = (from I1 in WmsDbOLong.DtllReceivexItemInventario where I1.TransaccionId == TDel.Fod().TransaccionId select I1);
                            if (L1.Count() > 0)
                                WmsDbOLong.DtllReceivexItemInventario.RemoveRange(L1);
                            WmsDbOLong.Transacciones.Remove(TDel.Fod());
                            WmsDbOLong.SaveChanges();
                        }
                    } catch (Exception e2) {
                        return new RetData<string> {
                            Info = new RetInfo() {
                                CodError = -2,
                                Mensaje = $"Error en el ingreso y no se pudo borrar automaticamente la transacción {MaxTransaccionId}. El error principal es: {e1.ToString()} y el error secundario es {e2.ToString()}",
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    }                    
                }
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -2,
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
                    Observacion = "SALIDA GENERADA DE XLS Intranet",
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
                List<ProductoUbicacion> ListPu = new List<ProductoUbicacion>();
                for (int i = 0; i < ListProducts.Count(); i++) {
                    ListPu.Add(new ProductoUbicacion {
                        CodProducto = ListProducts[i],
                        Typ = 4
                    });
                }
                DbO.ProductoUbicacion.RemoveRange(DbO.ProductoUbicacion.Where(Pu => Pu.Typ == 4));
                DbO.ProductoUbicacion.AddRange(ListPu);
                DbO.SaveChanges();
                System.Data.DataTable ListDtProdExistsWms = ManualDB.SpGeneraSalidaWMS2(ref DbO, FechaSalida, "", cboBodegas, cboRegimen, ClienteID, LocationID, RackID);
                for (int i = 0; i < ListProducts.Count(); i++) {
                    ProductoSinExistencia = ListProducts[i];
                    System.Data.DataRow[] DrProdExists = ListDtProdExistsWms.Select("CodProducto='" + ListProducts[i] + "'");
                    if (DrProdExists != null) {
                        if (DrProdExists.Count() > 0) {
                            ListSp.Add(new SpGeneraSalidaWMSModel() {
                                CodProducto = DrProdExists[0]["CodProducto"].ToString(),
                                InventarioID = DrProdExists[0]["InventarioID"].ToString(),
                                ItemInventarioID = DrProdExists[0]["ItemInventarioID"].ToString(),
                                Precio = DrProdExists[0]["precio"].ToString(),
                                Lote = DrProdExists[0]["lote"].ToString(),
                                Rack = DrProdExists[0]["rack"].ToString()
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
                SET XACT_ABORT OFF
                " + Environment.NewLine);
                //ListSql.Add((DateTime.Now - StartTime).TotalSeconds.ToString());
                //System.IO.StreamWriter Salida = new System.IO.StreamWriter("SetIngresoExcelWms2.sql", false);
                //ListSql.ForEach(S => Salida.WriteLine(S));
                //Salida.Close();
                ManualDB.UploadBatch(ref WmsDbOLong, string.Join("", ListSql));
                DbO.ProductoUbicacion.RemoveRange(DbO.ProductoUbicacion.Where(Pu => Pu.Typ == 4));
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = $"Información cargada",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                DbO.ProductoUbicacion.RemoveRange(DbO.ProductoUbicacion.Where(Pu => Pu.Typ == 4));
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
        [HttpPost]
        public RetData<string> SetNewDisPayless(string dtpFechaEntrega, int txtWomanQty, int txtManQty, int txtKidQty, int txtAccQty, string radInvType, int ClienteId, int TiendaId, bool? Divert, bool? FullPed, int? TiendaIdDestino) {
            DateTime StartTime = DateTime.Now;
            try {
                PedidosExternos NewPe = new PedidosExternos() {
                    ClienteId = ClienteId,
                    FechaCreacion = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                    FechaPedido = dtpFechaEntrega,
                    IdEstado = 2,
                    TiendaId = TiendaId,
                    WomanQty = txtWomanQty,
                    ManQty = txtManQty,
                    KidQty = txtKidQty,
                    AccQty = txtAccQty,
                    InvType = radInvType,
                    FullPed = FullPed,
                    Divert = Divert,
                    TiendaIdDestino = TiendaIdDestino
                };
                IEnumerable<int> ListExistPedido = (
                    from Pe in DbO.PedidosExternos
                    where Pe.FechaPedido.Substring(0, 10) == dtpFechaEntrega.Substring(0, 10)
                    && Pe.ClienteId == ClienteId
                    && Pe.TiendaId == TiendaId
                    select Pe.Id
                    );
                if (ListExistPedido.Count() > 0) {
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, Ya existe un pedido para la misma fecha.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                if (StartTime.DayOfWeek == DayOfWeek.Sunday && dtpFechaEntrega.ToDateFromEspDate().DayOfWeek == DayOfWeek.Monday)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, el domingo no se pueden hacer pedidos para el lunes.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                if (StartTime.DayOfWeek == DayOfWeek.Saturday && StartTime.Hour > 10 && dtpFechaEntrega.ToDateFromEspDate().DayOfWeek == DayOfWeek.Monday)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, el sábado no se pueden hacer pedidos a partir de las 10am para el lunes.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                List<PaylessProdPrioriDetModel> ListProdTienda = ManualDB.SP_GetPaylessProdSinPedido(ref DbO, ClienteId, TiendaId);                
                List<PaylessProdPrioriDetModel> ListProdWithStock = new List<PaylessProdPrioriDetModel>();
                IEnumerable<FE830DataAux> ListStock = ManualDB.SP_GetExistenciasByTienda(ref DbO, ClienteId, TiendaId);
                if (ListStock.Count() == 0)
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error, no hay existencias de productos para el cliente.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                foreach (FE830DataAux Stock in ListStock) {
                    foreach (PaylessProdPrioriDetModel Product in ListProdTienda.Where(P => P.Barcode == Stock.CodProducto)) {
                        Product.Existencia = Convert.ToInt32(Stock.Existencia);
                        if (Product.Existencia > 0 && ListProdWithStock.Where(Ws => Ws.Barcode == Product.Barcode).Count() == 0)
                            ListProdWithStock.Add(Product);
                    }
                }
                List<PaylessProdPrioriDetModel> ListProdPedido = new List<PaylessProdPrioriDetModel>();
                if (Divert.HasValue) {
                    if (!Divert.Value) {
                        ListProdPedido = (
                            from P1 in ListProdWithStock
                            where !string.IsNullOrEmpty(P1.Cp)
                            && (P1.Cp.Contains("A", StringComparison.InvariantCultureIgnoreCase)
                            || P1.Cp.Contains("H", StringComparison.InvariantCultureIgnoreCase))
                            select P1
                            ).ToList();
                    }
                }
                int NContCp = ListProdPedido.Count;
                switch (radInvType) {
                    case "fifo":
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "DAMAS"
                            orderby P2.Departamento.ToDateFromEspDate() descending
                            select P2
                            ).Take(txtWomanQty).Distinct().ToList());
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "CABALLEROS"
                            orderby P2.Departamento.ToDateFromEspDate() descending
                            select P2
                            ).Take(txtManQty).Distinct().ToList());
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "NIÑOS / AS"
                            orderby P2.Departamento.ToDateFromEspDate() descending
                            select P2
                            ).Take(txtKidQty).Distinct().ToList());
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "ACCESORIOS"
                            orderby P2.Departamento.ToDateFromEspDate() descending
                            select P2
                            ).Take(txtAccQty).Distinct().ToList());
                        break;
                    case "lifo":
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "DAMAS"
                            orderby P2.Departamento.ToDateFromEspDate() ascending
                            select P2
                            ).Take(txtWomanQty).Distinct().ToList());
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "CABALLEROS"
                            orderby P2.Departamento.ToDateFromEspDate() ascending
                            select P2
                            ).Take(txtManQty).Distinct().ToList());
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "NIÑOS / AS"
                            orderby P2.Departamento.ToDateFromEspDate() ascending
                            select P2
                            ).Take(txtKidQty).Distinct().ToList());
                        ListProdPedido.AddRange((
                            from P2 in ListProdWithStock
                            where string.IsNullOrEmpty(P2.Cp)
                            && P2.Categoria.ToUpper() == "ACCESORIOS"
                            orderby P2.Departamento.ToDateFromEspDate() ascending
                            select P2
                            ).Take(txtAccQty).Distinct().ToList());
                        break;
                    default:
                        break;
                }
                if (ListProdPedido.Count == 0) {
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "No hay productos seleccionados y no hay CP en existencia.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                DbO.PedidosExternos.Add(NewPe);
                DbO.SaveChanges();
                List<PedidosDetExternos> ListPed = ListProdPedido.Select(Pp => new PedidosDetExternos() {
                    PedidoId = NewPe.Id,
                    CodProducto = Pp.Barcode,
                    CantPedir = 1
                }).ToList();
                DbO.PedidosDetExternos.AddRange(ListPed);
                DbO.SaveChanges();
                return new RetData<string> {
                    Data = $"Se realizo el pedido, para la categoria mujeres: {txtWomanQty}, hombres: {txtManQty}, niñ@s: {txtKidQty}, Accesorios: {txtAccQty}, CP: {NContCp}. Número de pedido: {NewPe.Id}",
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = $"Se realizo el pedido, para la categoria mujeres: {txtWomanQty}, hombres: {txtManQty}, niñ@s: {txtKidQty}, Accesorios: {txtAccQty}, CP: {NContCp}. Número de pedido: {NewPe.Id}",
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
        [HttpGet]
        public RetData<IEnumerable<AsyncStates>> GetAsyncState(int Typ, string CodUser) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<AsyncStates> List = (from A in DbO.AsyncStates where A.Typ == Typ && A.CodUser == CodUser select A);
                return new RetData<IEnumerable<AsyncStates>> {
                    Data = List,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<AsyncStates>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<IEnumerable<PaylessTiendas>> GetStores() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PaylessTiendas> List = (from A in DbO.PaylessTiendas select A);
                return new RetData<IEnumerable<PaylessTiendas>> {
                    Data = List,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
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
        [HttpGet]
        public RetData<string> GetClientById(int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                string Ret = (from C in WmsDbO.Clientes where C.ClienteId == ClienteId select C.Nombre).Fod();
                return new RetData<string> {
                    Data = Ret,
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
        [HttpGet]
        public RetData<List<PedidosPendientesAdmin>> GetPedidosPendientesAdmin() {
            DateTime StartTime = DateTime.Now;
            try {
                AsyncStates ThisProc = new AsyncStates();
                IEnumerable<AsyncStates> ListAsyncs = (from A in DbO.AsyncStates where A.Typ == 2 select A);
                if (ListAsyncs.Count() == 0) {
                    ThisProc = new AsyncStates() { Typ = 2, Val = 0, Maximum = 2 };
                    DbO.AsyncStates.Add(ThisProc);
                    DbO.SaveChanges();
                } else {
                    return new RetData<List<PedidosPendientesAdmin>> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "El proceso ya se está ejecutando desde otra PC, por favor intentelo en 2 minutos.",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
                List<PedidosPendientesAdmin> Ret = ManualDB.SP_GetPedidosPendientesAdmin(ref DbOLong);
                DbO.AsyncStates.Remove(ThisProc);
                DbO.SaveChanges();
                return new RetData<List<PedidosPendientesAdmin>> {
                    Data = Ret,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<List<PedidosPendientesAdmin>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<string> ChangeUserClient(int IdUser, int ClienteId) { 
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<IenetUsers> ListUsers = (from U in DbO.IenetUsers where U.Id == IdUser select U);
                if (ListUsers.Count() > 0) {
                    IenetUsers UserO = ListUsers.Fod();
                    UserO.ClienteId = ClienteId;
                    DbO.IenetUsers.Update(UserO);
                    DbO.SaveChanges();
                    return new RetData<string> {
                        Data = "Se ha cambiado el cliente",
                        Info = new RetInfo() {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                } else {
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error desconocido al cambiar el cliente al usuario",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }               
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
        [HttpGet]
        public RetData<string> ChangeUserTienda(int IdUser, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<IenetUsers> ListUsers = (from U in DbO.IenetUsers where U.Id == IdUser select U);
                if (ListUsers.Count() > 0) {
                    IenetUsers UserO = ListUsers.Fod();
                    if (TiendaId != 0)
                        UserO.TiendaId = TiendaId;
                    else
                        UserO.TiendaId = null;
                    DbO.IenetUsers.Update(UserO);
                    DbO.SaveChanges();
                    return new RetData<string> {
                        Data = "Se ha cambiado la tienda",
                        Info = new RetInfo() {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                } else {
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error desconocido al cambiar el cliente al usuario",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
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
        [HttpGet]
        public RetData<IEnumerable<PeticionesAdminBGModel>> GetPeticionesAdminB() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<PeticionesAdminBGModel> ListOrders = ManualDB.SP_GetPeticionesAdminB(ref DbOLong);
                return new RetData<IEnumerable<PeticionesAdminBGModel>> {
                    Data = ListOrders,
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<IEnumerable<PeticionesAdminBGModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<string> ChangePedidoExternoIdWMS(int PedidoId, int PedidoIdWms) {
            DateTime StartTime = DateTime.Now;
            if (PedidoId == 0)
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Error desconocido, no se enviaron parámetros.",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };            
            try {
                IEnumerable<PedidosExternos> ListPed = (from P in DbO.PedidosExternos where P.Id == PedidoId select P);
                if (ListPed.Count() > 0) {
                    PedidosExternos Pedido = ListPed.Fod();
                    if (PedidoIdWms != 0) {
                        Pedido.PedidoWms = PedidoIdWms;
                        Pedido.IdEstado = 3;
                    } else {
                        Pedido.PedidoWms = null;
                        Pedido.IdEstado = 2;
                    }                    
                    DbO.PedidosExternos.Update(Pedido);
                    DbO.SaveChanges();
                    return new RetData<string> {
                        Data = "Se ha cambiado el despacho relacionado",
                        Info = new RetInfo() {
                            CodError = 0,
                            Mensaje = "ok",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                } else {
                    return new RetData<string> {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "Error desconocido al cambiar el despacho del pedido web",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }
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
        [HttpGet]
        private void CreateExcelAutoRepAndSendByMail(PaylessReportes NewRep, bool IsDisp) {
            string PlantillaPre = IsDisp ? "" : "b";
            string TypRep = IsDisp ? "0" : "1";
            RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>> ListInfo = GetWeekReport(NewRep.Id, TypRep);
            string Plantilla = $"plantillaOrdenes3{PlantillaPre}.xls";
            if (ListInfo.Data.Item2.Where(O1 => !string.IsNullOrEmpty(O1.Fecha4)).Count() > 0 && IsDisp)
                Plantilla = $"plantillaOrdenes4{PlantillaPre}.xls";
            if (ListInfo.Data.Item2.Where(O1 => !string.IsNullOrEmpty(O1.Fecha5)).Count() > 0 && IsDisp)
                Plantilla = $"plantillaOrdenes5{PlantillaPre}.xls";
            if (ListInfo.Data.Item2.Where(O1 => !string.IsNullOrEmpty(O1.Fecha6)).Count() > 0 && IsDisp)
                Plantilla = $"plantillaOrdenes6{PlantillaPre}.xls";            
            Utility.ExceL ExcelO = new Utility.ExceL();
            using (FileStream FilePlantilla = new FileStream(Plantilla, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                MemoryStream Ms = new MemoryStream();
                FilePlantilla.CopyTo(Ms);
                try {
                    ExcelO.ExcelWorkBook = new HSSFWorkbook(Ms);
                    ExcelO.CurrentSheet = ExcelO.ExcelWorkBook.GetSheetAt(0);
                } catch (Exception e2) {
                    throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                }
                ExcelO.SetRow(1);
                ExcelO.SetCell(4);
                ExcelO.SetCellValue(ListInfo.Data.Item1.Periodo);
                ExcelO.SetRow(2);
                ExcelO.SetCell(4);
                ExcelO.SetCellValue(ListInfo.Data.Item1.PeriodoF);
                IEnumerable<PaylessReportesDet> ListDetOrd;
                ListDetOrd = IsDisp ? ListInfo.Data.Item2.OrderByDescending(O1 => O1.Fecha1.ToDateEsp()) : ListInfo.Data.Item2.OrderByDescending(O1 => O1.Fecha1.ToDateFromEspDate());
                for (int i = 0; i < ListDetOrd.Count(); i++) {
                    ExcelO.CreateRow(i + 4);
                    ExcelO.CreateCell(1, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TiendaId);
                    ExcelO.CreateCell(2, CellType.String);
                    ExcelO.SetCellValue(ListInfo.Data.Item3.Where(T => T.TiendaId == ListDetOrd.ElementAt(i).TiendaId).Fod().Direc);
                    ExcelO.CreateCell(3, CellType.String);
                    ExcelO.SetCellValue(ListInfo.Data.Item3.Where(T => T.TiendaId == ListDetOrd.ElementAt(i).TiendaId).Fod().Lider);
                    ExcelO.CreateCell(4, CellType.String);
                    ExcelO.SetCellValue(ListInfo.Data.Item3.Where(T => T.TiendaId == ListDetOrd.ElementAt(i).TiendaId).Fod().Tel);

                    ExcelO.CreateCell(5, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Total);
                    ExcelO.CreateCell(11, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalAccQty);
                    ExcelO.CreateCell(12, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalKidQty);
                    ExcelO.CreateCell(13, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalManQty);
                    ExcelO.CreateCell(14, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalWomanQty);
                    ExcelO.CreateCell(15, CellType.String);
                    ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Total);
                    if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha1)) {
                        ExcelO.CreateCell(16, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha1.Substring(0, 10));
                        ExcelO.CreateCell(17, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant1);
                        ExcelO.CreateCell(18, CellType.String);
                        if (IsDisp)
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha1.Substring(12));
                    }
                    if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha2)) {
                        ExcelO.CreateCell(19, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha2.Substring(0, 10));
                        ExcelO.CreateCell(20, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant2);
                        ExcelO.CreateCell(21, CellType.String);
                        if (IsDisp)
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha2.Substring(12));
                    }
                    if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha3)) {
                        ExcelO.CreateCell(22, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha3.Substring(0, 10));
                        ExcelO.CreateCell(23, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant3);
                        ExcelO.CreateCell(24, CellType.String);
                        if (IsDisp)
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha3.Substring(12));
                    }
                    if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha4)) {
                        ExcelO.CreateCell(25, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha4.Substring(0, 10));
                        ExcelO.CreateCell(26, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant4);
                        ExcelO.CreateCell(27, CellType.String);
                        if (IsDisp)
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha4.Substring(12));
                    }
                    if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha5)) {
                        ExcelO.CreateCell(28, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha5.Substring(0, 10));
                        ExcelO.CreateCell(29, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant5);
                        ExcelO.CreateCell(30, CellType.String);
                        if (IsDisp)
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha5.Substring(12));
                    }
                    if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha6)) {
                        ExcelO.CreateCell(31, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha6.Substring(0, 10));
                        ExcelO.CreateCell(32, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant6);
                        ExcelO.CreateCell(33, CellType.String);
                        if (IsDisp)
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha6.Substring(12));
                    }
                }
                MemoryStream Ms2 = new MemoryStream();
                ExcelO.ExcelWorkBook.Write(Ms2);
                IEnumerable<PaylessReportesMails> ListMails = DbO.PaylessReportesMails;                
                using (SmtpClient client = new SmtpClient("10.240.34.119")) {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("hilmer.campos@glcweb.ddns.net", "HilmerServer2019");
                    MailMessage mailMessage = new MailMessage {
                        From = new MailAddress("hilmer.campos@glcweb.ddns.net")
                    };
                    foreach (PaylessReportesMails MO in ListMails)
                        mailMessage.To.Add(MO.MailDir);
                    mailMessage.Body = @"";
                    if (IsDisp)
                        mailMessage.Subject = "GLC - " + "Archivo_RepJuevesPedidos_" + DateTime.Now.ToString("ddMMyyyy_HHmm");
                    else
                        mailMessage.Subject = "GLC - " + "Archivo_RepDomingoEnvios_" + DateTime.Now.ToString("ddMMyyyy_HHmm");
                    System.Net.Mail.Attachment attachment;
                    //System.Net.Mime.ContentType ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Xml);
                    Ms2.Position = 0;
                    if (IsDisp)
                        attachment = new System.Net.Mail.Attachment(Ms2, "Archivo_RepJuevesPedidos_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls");
                    else
                        attachment = new System.Net.Mail.Attachment(Ms2, "Archivo_RepDomingoEnvios_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".xls");
                    mailMessage.Attachments.Add(attachment);
                    client.Send(mailMessage);
                }            
                //Ms2.Close();
                //return File(Ms2.ToArray(), "application/octet-stream", "Archivo_RepJuevesPedidos_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
            }
        }        
        [HttpGet]
        public RetData<string> MakeAutoReportsPayless() {
            DateTime StartTime = DateTime.Now;
            try {
                //Jueves
                if ((StartTime.DayOfWeek == DayOfWeek.Thursday && StartTime.Hour >= 17)
                    || StartTime.DayOfWeek == DayOfWeek.Friday
                    || StartTime.DayOfWeek == DayOfWeek.Saturday
                    || StartTime.DayOfWeek == DayOfWeek.Sunday) {
                    List<DateTime> ListThurs = Utility.Funcs.AllThursdaysInMonth(StartTime.Year, StartTime.Month).Where(D => D <= (new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 23, 59, 59)).AddDays(8)).ToList();
                    if ((new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 23, 59, 59)).AddDays(8).Month != StartTime.Month
                        && (new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 23, 59, 59)).AddDays(8).Year == StartTime.Year) {
                        List<DateTime> ListThursNext = Utility.Funcs.AllThursdaysInMonth(StartTime.Year, StartTime.Month + 1).ToList();
                        ListThurs.Add(ListThursNext.Fod());
                    }                    
                    for (int I = 0; I < ListThurs.Count(); I++) {
                        List<PaylessReportes> ListRep = (
                            from R in DbO.PaylessReportes
                            where R.Periodo == ListThurs[I].AddDays(-3).ToString(ApplicationSettings.DateTimeFormatShort)
                            && R.Tipo == "0"
                            select R
                            ).ToList();
                        if (ListRep.Count() == 0) {
                            PaylessReportes NewRep = new PaylessReportes() {
                                Periodo = ListThurs[I].AddDays(-3).ToString(ApplicationSettings.DateTimeFormatShort),
                                PeriodoF = ListThurs[I].AddDays(1).ToString(ApplicationSettings.DateTimeFormatShort),
                                FechaGen = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                                Tipo = "0"
                            };
                            DbO.PaylessReportes.Add(NewRep);
                            DbO.SaveChanges();
                            List<PeticionesAdminBGModel> ListOrders = ManualDB.SP_GetPeticionesAdminB(ref DbOLong).ToList();
                            ListOrders = ListOrders.Where(Pe => Pe.FechaPedido.ToDateEsp() >= ListThurs[I].AddDays(-4)
                                && Pe.FechaPedido.ToDateEsp() <= ListThurs[I].AddDays(3)).OrderByDescending(Pe2 => Pe2.FechaPedido.ToDateEsp()).ToList();
                            List<int?> ListTiendas = ListOrders.OrderBy(Pe2 => Pe2.FechaPedido.ToDateEsp()).Select(Lo => Lo.TiendaId).Distinct().ToList();
                            List<PaylessReportesDet> ListSubOrders = new List<PaylessReportesDet>();
                            for (int Ti = 0; Ti < ListTiendas.Count(); Ti++) {
                                List<PeticionesAdminBGModel> ListOrdersByTienda = ListOrders.Where(Lo => Lo.TiendaId == ListTiendas.ElementAt(Ti)).OrderBy(Lo => Lo.FechaPedido.ToDateEsp()).ToList();
                                int TotalWomanCp = 0, TotalManCp = 0, TotalKidsCp = 0, TotalAccCp = 0;
                                ListSubOrders.Clear();
                                for (int Pi = 0; Pi < ListOrdersByTienda.Count(); Pi++) {
                                    ListSubOrders.Add(new PaylessReportesDet {
                                        TiendaId = ListTiendas.ElementAt(Ti),
                                        Fecha1 = ListOrdersByTienda.ElementAt(Pi).FechaPedido,
                                        TotalWomanQty = ListOrdersByTienda.ElementAt(Pi).WomanQty,
                                        TotalManQty = ListOrdersByTienda.ElementAt(Pi).ManQty,
                                        TotalKidQty = ListOrdersByTienda.ElementAt(Pi).KidQty,
                                        TotalAccQty = ListOrdersByTienda.ElementAt(Pi).AccQty,
                                        Total = ListOrdersByTienda.ElementAt(Pi).WomanQty
                                            + ListOrdersByTienda.ElementAt(Pi).ManQty
                                            + ListOrdersByTienda.ElementAt(Pi).KidQty
                                            + ListOrdersByTienda.ElementAt(Pi).AccQty
                                            + ListOrdersByTienda.ElementAt(Pi).TotalCp
                                    });
                                    if (ListOrdersByTienda.ElementAt(Pi).TotalCp > 0) {
                                        int WomanCp = 0, ManCp = 0, KidsCp = 0, AccCp = 0;
                                        WomanCp = (
                                            from Pu in DbO.ProductoUbicacion
                                            where Pu.Rack == ListOrdersByTienda.ElementAt(Pi).Id
                                            && Pu.NomBodega.ToUpper() == "DAMAS"
                                            && (Pu.NombreRack == "A" || Pu.NombreRack == "H")
                                            select Pu.Id
                                            ).Count();
                                        ManCp = (
                                            from Pu in DbO.ProductoUbicacion
                                            where Pu.Rack == ListOrdersByTienda.ElementAt(Pi).Id
                                            && Pu.NomBodega.ToUpper() == "CABALLEROS"
                                            && (Pu.NombreRack == "A" || Pu.NombreRack == "H")
                                            select Pu.Id
                                            ).Count();
                                        KidsCp = (
                                            from Pu in DbO.ProductoUbicacion
                                            where Pu.Rack == ListOrdersByTienda.ElementAt(Pi).Id
                                            && Pu.NomBodega.ToUpper() == "NIÑOS / AS"
                                            && (Pu.NombreRack == "A" || Pu.NombreRack == "H")
                                            select Pu.Id
                                            ).Count();
                                        AccCp = (
                                            from Pu in DbO.ProductoUbicacion
                                            where Pu.Rack == ListOrdersByTienda.ElementAt(Pi).Id
                                            && Pu.NomBodega.ToUpper() == "ACCESORIOS"
                                            && (Pu.NombreRack == "A" || Pu.NombreRack == "H")
                                            select Pu.Id
                                            ).Count();
                                        TotalWomanCp += WomanCp;
                                        TotalManCp += ManCp;
                                        TotalKidsCp += KidsCp;
                                        TotalAccCp += AccCp;
                                    }
                                }
                                PaylessReportesDet NewRepDet = new PaylessReportesDet() {
                                    IdM = NewRep.Id,
                                    TiendaId = ListTiendas.ElementAt(Ti),
                                    TotalWomanQty = ListSubOrders.Sum(O1 => O1.TotalWomanQty) + TotalWomanCp,
                                    TotalManQty = ListSubOrders.Sum(O1 => O1.TotalManQty) + TotalManCp,
                                    TotalKidQty = ListSubOrders.Sum(O1 => O1.TotalKidQty) + TotalKidsCp,
                                    TotalAccQty = ListSubOrders.Sum(O1 => O1.TotalAccQty) + TotalAccCp,
                                    Total = ListSubOrders.Sum(O1 => O1.Total)
                                };
                                for (int Z = 0; Z < ListSubOrders.Count; Z++) {
                                    switch (Z) {
                                        case 0:
                                            NewRepDet.Fecha1 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant1 = ListSubOrders[Z].Total;
                                            break;
                                        case 1:
                                            NewRepDet.Fecha2 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant2 = ListSubOrders[Z].Total;
                                            break;
                                        case 2:
                                            NewRepDet.Fecha3 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant3 = ListSubOrders[Z].Total;
                                            break;
                                        case 3:
                                            NewRepDet.Fecha4 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant4 = ListSubOrders[Z].Total;
                                            break;
                                        case 4:
                                            NewRepDet.Fecha5 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant5 = ListSubOrders[Z].Total;
                                            break;
                                        case 5:
                                            NewRepDet.Fecha6 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant6 = ListSubOrders[Z].Total;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                DbO.PaylessReportesDet.Add(NewRepDet);
                                DbO.SaveChanges();
                            }
                            DbO.ProductoUbicacion.RemoveRange(DbO.ProductoUbicacion.Where(Pu => Pu.Typ == 3));
                            CreateExcelAutoRepAndSendByMail(NewRep, true);
                        } else {
                            if (I == ListThurs.Count() - 1) {
                                bool TheSame = true;
                                List<PeticionesAdminBGModel> ListOrders = ManualDB.SP_GetPeticionesAdminB(ref DbOLong).ToList();
                                ListOrders = ListOrders.Where(Pe => Pe.FechaPedido.ToDateEsp() >= ListThurs[I].AddDays(-4)
                                    && Pe.FechaPedido.ToDateEsp() <= ListThurs[I].AddDays(3)).OrderByDescending(Pe2 => Pe2.FechaPedido.ToDateEsp()).ToList();
                                List<PaylessReportesDet> ListRepDet = (from Rd in DbO.PaylessReportesDet where Rd.IdM == ListRep.Fod().Id select Rd).ToList();
                                foreach (PeticionesAdminBGModel PedNuevo in ListOrders) {
                                    if (ListRepDet.Where(O1 =>
                                        O1.Fecha1 == PedNuevo.FechaPedido
                                        || O1.Fecha2 == PedNuevo.FechaPedido
                                        || O1.Fecha3 == PedNuevo.FechaPedido
                                        || O1.Fecha4 == PedNuevo.FechaPedido
                                        || O1.Fecha5 == PedNuevo.FechaPedido
                                        || O1.Fecha6 == PedNuevo.FechaPedido).Count() == 0)
                                        TheSame = false;
                                }
                                if (!TheSame) {
                                    DbO.PaylessReportesDet.RemoveRange(DbO.PaylessReportesDet.Where(R1 => R1.IdM == ListRep.Fod().Id));
                                    DbO.PaylessReportes.Remove(ListRep.Fod());
                                    DbO.SaveChanges();
                                }
                            }
                        }
                    }
                }
                //Fin jueves
                if ((StartTime.DayOfWeek == DayOfWeek.Saturday && StartTime.Hour >= 10)) {
                    List<DateTime> ListSaturdays = Utility.Funcs.AllSaturdayInMonth(StartTime.Year, StartTime.Month).Where(D => D <= (new DateTime(StartTime.Year, StartTime.Month, StartTime.Day, 23, 59, 59)).AddDays(1)).ToList();
                    for (int i = 0; i < ListSaturdays.Count; i++) {
                        List<PaylessReportes> ListRep = (
                            from R in DbO.PaylessReportes
                            where R.Periodo == ListSaturdays[i].AddDays(-6).ToString(ApplicationSettings.DateTimeFormatShort)
                            && R.Tipo == "1"
                            select R
                            ).ToList();
                        if (ListRep.Count() == 0) {
                            PaylessReportes NewRep = new PaylessReportes() {
                                Periodo = ListSaturdays[i].AddDays(-6).ToString(ApplicationSettings.DateTimeFormatShort),
                                PeriodoF = ListSaturdays[i].AddDays(1).ToString(ApplicationSettings.DateTimeFormatShort),
                                FechaGen = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                                Tipo = "1"
                            };
                            DbO.PaylessReportes.Add(NewRep);
                            DbO.SaveChanges();
                            List<PeticionesAdminBGModel> ListOrders = ManualDB.SP_GetWmsGroupDispatchsSunday(ref DbO, 1432, ListSaturdays[i].AddDays(-6).ToString(ApplicationSettings.DateTimeFormatSqlServer), ListSaturdays[i].AddDays(1).ToString(ApplicationSettings.DateTimeFormatSqlServer)).ToList();
                            ListOrders = ListOrders.OrderByDescending(Pe2 => Pe2.FechaPedido.ToDateFromEspDate()).ToList();
                            List<int?> ListTiendas = (
                                from Lo in ListOrders
                                group Lo by new { Lo.TiendaId }
                                into G
                                orderby G.Fod().FechaPedido.ToDateFromEspDate() descending
                                select G.Fod().TiendaId                                
                                ).ToList();
                            List <PaylessReportesDet> ListSubOrders = new List<PaylessReportesDet>();
                            for (int Ti = 0; Ti < ListTiendas.Count(); Ti++) {
                                List<PeticionesAdminBGModel> ListOrdersByTienda = ListOrders.Where(Lo => Lo.TiendaId == ListTiendas[Ti]).OrderBy(Lo => Lo.FechaPedido.ToDateFromEspDate()).ToList();
                                ListSubOrders.Clear();
                                for (int Pi = 0; Pi < ListOrdersByTienda.Count(); Pi++) {
                                    ListSubOrders.Add(new PaylessReportesDet {
                                        TiendaId = ListTiendas[Ti],
                                        Fecha1 = ListOrdersByTienda[Pi].FechaPedido,
                                        TotalWomanQty = ListOrdersByTienda[Pi].WomanQty,
                                        TotalManQty = ListOrdersByTienda[Pi].ManQty,
                                        TotalKidQty = ListOrdersByTienda[Pi].KidQty,
                                        TotalAccQty = ListOrdersByTienda[Pi].AccQty,
                                        Total = ListOrdersByTienda[Pi].Total
                                    });                                    
                                }
                                PaylessReportesDet NewRepDet = new PaylessReportesDet() {
                                    IdM = NewRep.Id,
                                    TiendaId = ListTiendas[Ti],
                                    TotalWomanQty = ListSubOrders.Sum(O1 => O1.TotalWomanQty),
                                    TotalManQty = ListSubOrders.Sum(O1 => O1.TotalManQty),
                                    TotalKidQty = ListSubOrders.Sum(O1 => O1.TotalKidQty),
                                    TotalAccQty = ListSubOrders.Sum(O1 => O1.TotalAccQty),
                                    Total = ListSubOrders.Sum(O1 => O1.Total)
                                };
                                for (int Z = 0; Z < ListSubOrders.Count; Z++) {
                                    switch (Z) {
                                        case 0:
                                            NewRepDet.Fecha1 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant1 = ListSubOrders[Z].Total;
                                            break;
                                        case 1:
                                            NewRepDet.Fecha2 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant2 = ListSubOrders[Z].Total;
                                            break;
                                        case 2:
                                            NewRepDet.Fecha3 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant3 = ListSubOrders[Z].Total;
                                            break;
                                        case 3:
                                            NewRepDet.Fecha4 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant4 = ListSubOrders[Z].Total;
                                            break;
                                        case 4:
                                            NewRepDet.Fecha5 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant5 = ListSubOrders[Z].Total;
                                            break;
                                        case 5:
                                            NewRepDet.Fecha6 = ListSubOrders[Z].Fecha1;
                                            NewRepDet.Cant6 = ListSubOrders[Z].Total;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                DbO.PaylessReportesDet.Add(NewRepDet);
                                DbO.SaveChanges();
                            }
                            CreateExcelAutoRepAndSendByMail(NewRep, false);
                        }
                    }
                }
                //Fin rep domingo
                return new RetData<string> {
                    Data = "ok",
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

        [HttpGet]
        public RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>> GetWeekReport(int Id, string Typ) {
            DateTime StartTime = DateTime.Now;
            try {
                PaylessReportes Pr = (from R in DbO.PaylessReportes where R.Id == Id && R.Tipo == Typ select R).Fod();
                IEnumerable<PaylessReportesDet> PrDet = (from Rd in DbO.PaylessReportesDet where Rd.IdM == Id orderby Rd.TiendaId select Rd);
                return new RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>> {
                    Data = new Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>(Pr, PrDet, DbO.PaylessTiendas),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e1) {
                return new RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        [HttpGet]
        public RetData<string> SendMail() {
            DateTime StartTime = DateTime.Now;            
            try {
                using (SmtpClient client = new SmtpClient("10.240.34.119")) {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("hilmer.campos@glcweb.ddns.net", "HilmerServer2019");
                    MailMessage mailMessage = new MailMessage {
                        From = new MailAddress("hilmer.campos@glcweb.ddns.net")
                    };
                    mailMessage.To.Add("hilmer.campos@glcamerica.com");
                    mailMessage.Body = @"";
                    mailMessage.Subject = "Citación";
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment("830_ejemplo.txt");
                    mailMessage.Attachments.Add(attachment);
                    client.Send(mailMessage);
                }
                return new RetData<string> {
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
    }
}