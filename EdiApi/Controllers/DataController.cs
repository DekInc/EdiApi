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
        public DataController(EdiDBContext _DbO) { DbO = _DbO; }
        [HttpGet]
        public ActionResult<IEnumerable<LearPureEdi>> GetPureEdi()
        {
            IEnumerable<LearPureEdi> Ret = DbO.LearPureEdi
                .Where(P => !string.IsNullOrEmpty(P.NombreArchivo))
                .OrderByDescending(O => O.Fprocesado);
            foreach (LearPureEdi LearPureEdiO in Ret)
                LearPureEdiO.EdiStr = string.Empty;
            return Ret.ToList();
        }        
        [HttpGet]
        public ActionResult<FE830Data> GetFE830Data(string HashId)
        {
            FE830Data FE830DataRet = new FE830Data
            {
                ISA = (from Isa in DbO.LearIsa830
                      where Isa.ParentHashId == HashId                      
                      select Isa).FirstOrDefault(),
            };
            FE830DataRet.ListSt = from St in DbO.LearSt830
                                  where St.ParentHashId == FE830DataRet.ISA.HashId
                                  select St;
            FE830DataRet.ListStBfr = from Bfr in DbO.LearBfr830
                                   from St in DbO.LearSt830
                                   where Bfr.ParentHashId == St.HashId && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Bfr;
            FE830DataRet.ListStN1 = from N1 in DbO.LearN1830
                                     from St in DbO.LearSt830
                                     where N1.ParentHashId == St.HashId && St.ParentHashId == FE830DataRet.ISA.HashId
                                    select N1;
            FE830DataRet.ListStN4 = from N4 in DbO.LearN4830
                                    from St in DbO.LearSt830
                                    where N4.ParentHashId == St.HashId && St.ParentHashId == FE830DataRet.ISA.HashId
                                    select N4;
            FE830DataRet.ListStNte = from Nte in DbO.LearNte830
                                    from St in DbO.LearSt830
                                    where Nte.ParentHashId == St.HashId && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Nte;
            FE830DataRet.ListStLin = from Lin in DbO.LearLin830
                                     from St in DbO.LearSt830
                                     where Lin.ParentHashId == St.HashId && St.ParentHashId == FE830DataRet.ISA.HashId
                                     select Lin;
            return FE830DataRet;
        }
    }
}