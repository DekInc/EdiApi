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
    public class DataController : Controller
    {
        public EdiDBContext DbO;
        public WmsContext WmsDbO;
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
        public DataController(EdiDBContext _DbO, WmsContext _WmsDbO, IConfiguration _Config) {
            DbO = _DbO;
            WmsDbO = _WmsDbO;
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
                IEnumerable<TsqlDespachosWmsComplex> ListSNO = ManualDB.SP_GetSNDet(ref DbO);
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
        public RetData<Clientes> GetClient(object ClientId)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                int? IdPedidoExterno = (
                    from Pe in DbO.PedidosExternos
                    where Pe.ClienteId == Convert.ToInt32(ClientId)
                    && Pe.IdEstado == 1
                    orderby Pe.Id descending
                    select Pe.Id                    
                    ).Fod();
                Clientes ClienteO = WmsDbO.Clientes.Where(C => C.ClienteId == Convert.ToInt32(ClientId)).Fod();
                ClienteO.EstatusId = IdPedidoExterno;
                return new RetData<Clientes>
                {
                    Data = ClienteO,
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
                return new RetData<Clientes>
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
                IEnumerable<PedidosDetExternos> ListDePe = (
                    from Dp in DbO.PedidosDetExternos
                    from Pe in DbO.PedidosExternos
                    where Dp.PedidoId == Pe.Id
                    && Pe.ClienteId == Convert.ToInt32(ClienteId)
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
        [HttpGet]
        public RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>> GetPedidosExternosPendientes()
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
                    (
                    from C in WmsDbO.Clientes
                    from Pe in ListPe
                    where C.ClienteId == Pe.ClienteId
                    select C
                    ).Distinct().ToList().ForEach(Cl => {
                        foreach (PedidosExternos PedC in ListPe.Where(Ped => Ped.ClienteId == Cl.ClienteId))
                        {
                            ListClients.Add(new Clientes() { ClienteId = Cl.ClienteId, Nombre = Cl.Nombre });
                        }
                    });
                IEnumerable<PedidosDetExternos> ListDePe = (
                    from Dp in DbO.PedidosDetExternos
                    from Pe in DbO.PedidosExternos
                    where Dp.PedidoId == Pe.Id
                    && Pe.IdEstado == 2
                    orderby Dp.Id descending
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
        public RetData<PedidosExternos> SetPedidoExterno(IEnumerable<PedidoExternoModel> ListDis, int ClienteId, int IdEstado)
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                if (ListDis.Count() == 0)
                    throw new Exception("No hay productos en la lista. WebAPI.");
                string DateProm = "";
                foreach (PedidoExternoModel Pem in ListDis)
                    if (!string.IsNullOrEmpty(Pem.dateProm)) DateProm = Pem.dateProm;
                PedidosExternos PedidoExterno = new PedidosExternos() {
                    ClienteId = ClienteId,
                    FechaCreacion = DateTime.Now.ToString(ApplicationSettings.DateTimeFormat),
                    IdEstado = IdEstado,
                    FechaPedido = DateProm,
                    Id = ListDis.Fod().id ?? 0
                };
                if (PedidoExterno.Id == 0)
                    DbO.PedidosExternos.Add(PedidoExterno);
                else
                {
                    DbO.PedidosExternos.Update(PedidoExterno);
                    foreach (PedidosDetExternos Pde in DbO.PedidosDetExternos.Where(Pd => Pd.PedidoId == PedidoExterno.Id))
                        DbO.PedidosDetExternos.Remove(Pde);
                }
                DbO.SaveChanges();                
                foreach (PedidoExternoModel PedidoDet in ListDis)
                {
                    PedidosDetExternos PedidoExtDet = new PedidosDetExternos() {
                        CodProducto = PedidoDet.codProducto,
                        PedidoId = PedidoExterno.Id,
                        CantPedir = Convert.ToDouble(PedidoDet.cantPedir)
                    };
                    DbO.PedidosDetExternos.Add(PedidoExtDet);
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
                return new RetData<IEnumerable<PedidosWmsModel>>
                {
                    Data = ManualDB.SP_GetPedidosWms(ref DbO, Convert.ToInt32(ClienteId)),
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
    }
}