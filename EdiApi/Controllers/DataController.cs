using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EdiApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        public EdiDBContext DbO;
        public wmsContext WmsDbO;
        public static readonly string G1;
        public Models.Remps_globalDB.Remps_globalContext Remps_globalDB;
        public DataController(EdiDBContext _DbO, wmsContext _WmsDbO, Models.Remps_globalDB.Remps_globalContext _Remps_globalDB) {
            DbO = _DbO;
            WmsDbO = _WmsDbO;
            Remps_globalDB = _Remps_globalDB;
        }
        private IEnumerable<Rep830InfoAux> GetIsaFromTo(IEnumerable<LearIsa830> _ListIsa, bool _From)
        {
            foreach (LearIsa830 IsaO in _ListIsa)
            {
                yield return new Rep830InfoAux()
                {
                    HashId = IsaO.ParentHashId,
                    Dest = _From? IsaO.InterchangeSenderId : IsaO.InterchangeReceiverId
                };
            }
        }
        [HttpGet]
        public ActionResult<Rep830Info> GetPureEdi(string HashId = "")
        {
            if (string.IsNullOrEmpty(HashId))
            {
                Rep830Info Rep830InfoO = new Rep830Info();
                IEnumerable<LearIsa830> ListIsa = (
                    from Pe in DbO.LearPureEdi
                    from Isa in DbO.LearIsa830
                    where Isa.ParentHashId == Pe.HashId
                    select Isa);
                Rep830InfoO.From = GetIsaFromTo(ListIsa, true);
                Rep830InfoO.To = GetIsaFromTo(ListIsa, false);
                Rep830InfoO.LearPureEdi = from Pe in DbO.LearPureEdi
                                          where !string.IsNullOrEmpty(Pe.NombreArchivo)
                                          orderby Pe.Fingreso descending
                                          select Pe;
                foreach (LearPureEdi LearPureEdiO in Rep830InfoO.LearPureEdi)
                    LearPureEdiO.EdiStr = string.Empty;
                return Rep830InfoO;
            } else
            {
                Rep830Info Rep830InfoO = new Rep830Info();
                IEnumerable<LearIsa830> ListIsa = (
                    from Isa in DbO.LearIsa830
                    where Isa.ParentHashId == HashId
                    select Isa);
                Rep830InfoO.From = GetIsaFromTo(ListIsa, true);
                Rep830InfoO.To = GetIsaFromTo(ListIsa, false);
                Rep830InfoO.LearPureEdi = from Pe in DbO.LearPureEdi
                                               where Pe.HashId == HashId
                                               select Pe;
                foreach (LearPureEdi LearPureEdiO in Rep830InfoO.LearPureEdi)
                    LearPureEdiO.EdiStr = string.Empty;
                return Rep830InfoO;
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
        public ActionResult<string> GetSN() {
            var ListTsql = from T in WmsDbO.Transacciones
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
                       //from G in WmsDbO.g
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
                       select T.AduFro;

            return ListTsql.Count().ToString();
        }
        public static byte[] a2212121(System.IO.Stream input)
        {
            System.Collections.Stack st = new System.Collections.Stack();
            st.Push("Csss");
            st.Push(6.5);
            st.Push(8);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}