using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models;
using EdiApi.Models.EdiDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : Controller
    {
        public EdiDBContext DbO;
        public wmsContext WmsDbO;
        public Models.Remps_globalDB.Remps_globalContext Remps_globalDbO;
        public static readonly string G1;        
        public DataController(EdiDBContext _DbO, wmsContext _WmsDbO, Models.Remps_globalDB.Remps_globalContext _Remps_globalDB) {
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
            yield return new TsqlDespachosWmsComplex() { errorMessage = E1.ToString() };
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
                            NumReporte = IsaF.InterchangeControlNumber
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
            return FE830DataRet;
        }
        delegate void del(int i);
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
                List<TsqlDespachosWmsComplex> ListDespachosWms = (
                from T in WmsDbO.Transacciones
                from B in WmsDbO.Bodegas
                from L in WmsDbO.Locations
                from Y in WmsDbO.Paises
                from P in WmsDbO.Pedido
                from D in WmsDbO.DtllDespacho
                from D2 in WmsDbO.Despachos
                from E in WmsDbO.Estatus
                from S in WmsDbO.SysTempSalidas
                from Ii in WmsDbO.ItemInventario
                from I in WmsDbO.Inventario
                from Um in WmsDbO.UnidadMedida
                where T.BodegaId == B.BodegaId
                && L.Locationid == B.Locationid
                && Y.Paisid == L.Paisid
                && P.PedidoId == T.PedidoId
                && D.TransaccionId == T.TransaccionId
                && D2.DespachoId == D.DespachoId
                && E.EstatusId == T.EstatusId
                && S.PedidoId == P.PedidoId
                && Ii.ItemInventarioId == S.ItemInventarioId
                && I.InventarioId == S.InventarioId
                && T.EstatusId == 9
                && T.FechaTransaccion > DateTime.Now.AddMonths(-1)
                && Um.UnidadMedidaId == I.TipoBulto
                group new { T, B, L, Y, P, D, D2, E, S, Ii, I }
                by new
                {
                    D2.DespachoId,
                    D2.FechaSalida,
                    D2.NoContenedor,
                    D2.Motorista,
                    D2.DocumentoMotorista,
                    D2.Destino,
                    D2.DocumentoFiscal,
                    D2.FechaDocFiscal,
                    D2.NoMarchamo,
                    D2.Observacion,
                    D2.Transportistaid,
                    D2.Destinoid,

                    I.TipoBulto,
                    Ii.CodProducto,
                    Ii.Descripcion,
                    Ii.NumeroOc,
                    Um.UnidadMedida1,

                    T.NoTransaccion,
                    T.ClienteId,
                    E.Estatus1,

                    B.BodegaId,
                    B.NomBodega,
                    Y.Nompais,
                    T.IdRcontrol,
                }
                into Grp
                orderby Grp.Key.FechaSalida descending, Grp.Key.Destino ascending, Grp.Key.CodProducto ascending
                select new TsqlDespachosWmsComplex()
                {
                    DespachoId = Grp.Key.DespachoId,
                    FechaSalida = Grp.Key.FechaSalida,
                    NoContenedor = Grp.Key.NoContenedor,
                    Motorista = Grp.Key.Motorista,
                    DocumentoMotorista = Grp.Key.DocumentoMotorista,
                    Destino = Grp.Key.Destino,
                    DocumentoFiscal = Grp.Key.DocumentoFiscal,
                    FechaDocFiscal = Grp.Key.FechaDocFiscal,
                    NoMarchamo = Grp.Key.NoMarchamo,
                    Observacion = Grp.Key.Observacion,
                    Transportistaid = Grp.Key.Transportistaid,
                    Destinoid = Grp.Key.Destinoid,

                    Quantity = Grp.Sum(Col => Col.S.Cantidad),
                    Bulks = Grp.Sum(ColO => ColO.S.Cantidad * (double)ColO.I.Articulos / ColO.I.CantidadInicial),
                    Weight = Grp.Sum(ColO => ColO.S.Cantidad * ColO.I.Peso / ColO.I.CantidadInicial),
                    Volume = Grp.Sum(ColO => ColO.S.Cantidad * ColO.I.Volumen / ColO.I.CantidadInicial),
                    TotalValue = Grp.Sum(ColO2 => ColO2.S.Cantidad * ColO2.S.Precio),

                    TipoBulto = Grp.Key.TipoBulto,
                    CodProducto = Grp.Key.CodProducto,
                    Producto = Grp.Key.Descripcion,
                    NumeroOc = Grp.Key.NumeroOc,
                    UnidadDeMedida = Grp.Key.UnidadMedida1,

                    NoTransaccion = Grp.Key.NoTransaccion,
                    ClienteId = Grp.Key.ClienteId,
                    Estatus1 = Grp.Key.Estatus1,

                    BodegaId = Grp.Key.BodegaId,
                    NomBodega = Grp.Key.NomBodega,
                    Nompais = Grp.Key.Nompais,
                    IdRcontrol = Grp.Key.IdRcontrol
                }).ToList();
                List<Models.Remps_globalDB.GlbClient> LGlbClient =
                    (from C in Remps_globalDbO.GlbClient
                     from R in ListDespachosWms
                     where C.IdClient == R.ClienteId
                     select C).ToList();
                List<Models.Remps_globalDB.GlbCountry> LGlbCountry =
                    (from U in Remps_globalDbO.GlbCountry
                     from R in ListDespachosWms
                     where U.Name.Equals(R.Nompais, StringComparison.OrdinalIgnoreCase)
                     select U).ToList();
                //List<Models.Remps_globalDB.GlbClientIntegration> LGlbClientIntegration =
                //    (from G in Remps_globalDbO.GlbClientIntegration
                //    from R in ListDespachosWms
                //    where G.IdWms == R.ClienteId
                //    select G).ToList();
                foreach (TsqlDespachosWmsComplex D in ListDespachosWms)
                {
                    foreach (Models.Remps_globalDB.GlbClient C in LGlbClient)
                    {
                        D.idclient = C.IdClient;
                        D.code = C.Code;
                        D.businessname = C.BusinessName;
                    }
                    foreach (Models.Remps_globalDB.GlbCountry U in LGlbCountry)
                    {
                        D.IdCountryOrigin = U.IdCountry;
                        D.CountryOrigin = U.Name;
                    }
                }
                return ListDespachosWms;
            }
            catch (Exception e1)
            {
                return GetExToIe2(e1);
            }            
        }
        [HttpGet]
        public IEnumerable<TsqlDespachosWmsComplex> GetSNDetails(string ListDispatch, string ListProducts)
        {
            try
            {
                List<int> ListDispatch2 = new List<int>();
                string[] ListDispatchArray = ListDispatch.Split('|');
                List<string> ListProducts2 = ListProducts.Split('|').ToList();
                for (int Pi = 0; Pi < ListProducts2.Count(); Pi++)
                    ListProducts2[Pi] = ListProducts2[Pi].Replace("º", " ").Replace("[", "").Replace("]", "");
                for (int Pi = 0; Pi < ListDispatchArray.Count(); Pi++)
                    ListDispatch2.Add(Convert.ToInt32(ListDispatchArray[Pi]));
                List<TsqlDespachosWmsComplex> ListDespachosWms = (
                    from MoreC in (
                    from T in WmsDbO.Transacciones
                    from B in WmsDbO.Bodegas
                    from L in WmsDbO.Locations
                    from Y in WmsDbO.Paises
                    from P in WmsDbO.Pedido
                    from D in WmsDbO.DtllDespacho
                    from D2 in WmsDbO.Despachos
                    from E in WmsDbO.Estatus
                    from S in WmsDbO.SysTempSalidas
                    from Ii in WmsDbO.ItemInventario
                    from I in WmsDbO.Inventario
                    from Um in WmsDbO.UnidadMedida
                    where T.BodegaId == B.BodegaId
                    && L.Locationid == B.Locationid
                    && Y.Paisid == L.Paisid
                    && P.PedidoId == T.PedidoId
                    && D.TransaccionId == T.TransaccionId
                    && D2.DespachoId == D.DespachoId
                    && E.EstatusId == T.EstatusId
                    && S.PedidoId == P.PedidoId
                    && Ii.ItemInventarioId == S.ItemInventarioId
                    && I.InventarioId == S.InventarioId
                    && T.EstatusId == 9
                    && T.FechaTransaccion > DateTime.Now.AddMonths(-1)
                    && Um.UnidadMedidaId == I.TipoBulto
                //&& D2.DespachoId == Dis
                group new { T, B, L, Y, P, D, D2, E, S, Ii, I }
                    by new
                    {
                        D2.DespachoId,
                        D2.FechaSalida,
                        D2.NoContenedor,
                        D2.Motorista,
                        D2.DocumentoMotorista,
                        D2.Destino,
                        D2.DocumentoFiscal,
                        D2.FechaDocFiscal,
                        D2.NoMarchamo,
                        D2.Observacion,
                        D2.Transportistaid,
                        D2.Destinoid,

                        I.TipoBulto,
                        Ii.CodProducto,
                        Ii.Descripcion,
                        Ii.NumeroOc,
                        Um.UnidadMedida1,

                        T.NoTransaccion,
                        T.ClienteId,
                        E.Estatus1,

                        B.BodegaId,
                        B.NomBodega,
                        Y.Nompais,
                        T.IdRcontrol,
                    }
                    into Grp
                    orderby Grp.Key.FechaSalida descending, Grp.Key.Destino ascending, Grp.Key.CodProducto ascending
                    select new TsqlDespachosWmsComplex()
                    {
                        DespachoId = Grp.Key.DespachoId,
                        FechaSalida = Grp.Key.FechaSalida,
                        NoContenedor = Grp.Key.NoContenedor,
                        Motorista = Grp.Key.Motorista,
                        DocumentoMotorista = Grp.Key.DocumentoMotorista,
                        Destino = Grp.Key.Destino,
                        DocumentoFiscal = Grp.Key.DocumentoFiscal,
                        FechaDocFiscal = Grp.Key.FechaDocFiscal,
                        NoMarchamo = Grp.Key.NoMarchamo,
                        Observacion = Grp.Key.Observacion,
                        Transportistaid = Grp.Key.Transportistaid,
                        Destinoid = Grp.Key.Destinoid,

                        Quantity = Grp.Sum(Col => Col.S.Cantidad),
                        Bulks = Grp.Sum(ColO => ColO.S.Cantidad * (double)ColO.I.Articulos / ColO.I.CantidadInicial),
                        Weight = Grp.Sum(ColO => ColO.S.Cantidad * ColO.I.Peso / ColO.I.CantidadInicial),
                        Volume = Grp.Sum(ColO => ColO.S.Cantidad * ColO.I.Volumen / ColO.I.CantidadInicial),
                        TotalValue = Grp.Sum(ColO2 => ColO2.S.Cantidad * ColO2.S.Precio),

                        TipoBulto = Grp.Key.TipoBulto,
                        CodProducto = Grp.Key.CodProducto,
                        Producto = Grp.Key.Descripcion,
                        NumeroOc = Grp.Key.NumeroOc,
                        UnidadDeMedida = Grp.Key.UnidadMedida1,

                        NoTransaccion = Grp.Key.NoTransaccion,
                        ClienteId = Grp.Key.ClienteId,
                        Estatus1 = Grp.Key.Estatus1,

                        BodegaId = Grp.Key.BodegaId,
                        NomBodega = Grp.Key.NomBodega,
                        Nompais = Grp.Key.Nompais,
                        IdRcontrol = Grp.Key.IdRcontrol
                    })
                    from Dis in ListDispatch2
                    from Pr in ListProducts2
                    where MoreC.DespachoId == Dis
                    && MoreC.CodProducto == Pr
                    select MoreC)
                    .ToList();
                List<Models.Remps_globalDB.GlbClient> LGlbClient =
                    (from C in Remps_globalDbO.GlbClient
                     from R in ListDespachosWms
                     where C.IdClient == R.ClienteId
                     select C).ToList();
                List<Models.Remps_globalDB.GlbCountry> LGlbCountry =
                    (from U in Remps_globalDbO.GlbCountry
                     from R in ListDespachosWms
                     where U.Name.Equals(R.Nompais, StringComparison.OrdinalIgnoreCase)
                     select U).ToList();
                //List<Models.Remps_globalDB.GlbClientIntegration> LGlbClientIntegration =
                //    (from G in Remps_globalDbO.GlbClientIntegration
                //    from R in ListDespachosWms
                //    where G.IdWms == R.ClienteId
                //    select G).ToList();
                foreach (TsqlDespachosWmsComplex D in ListDespachosWms)
                {
                    foreach (Models.Remps_globalDB.GlbClient C in LGlbClient)
                    {
                        D.idclient = C.IdClient;
                        D.code = C.Code;
                        D.businessname = C.BusinessName;
                    }
                    foreach (Models.Remps_globalDB.GlbCountry U in LGlbCountry)
                    {
                        D.IdCountryOrigin = U.IdCountry;
                        D.CountryOrigin = U.Name;
                    }
                }
                return ListDespachosWms;
            }
            catch (Exception e1)
            {
                return GetExToIe2(e1);
            }            
        }
    }
}