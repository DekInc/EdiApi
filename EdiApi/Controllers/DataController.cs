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
            IEnumerable<LearPureEdi> Ret = DbO.LearPureEdi.Where(P => !string.IsNullOrEmpty(P.NombreArchivo));
            foreach (LearPureEdi LearPureEdiO in Ret)
                LearPureEdiO.EdiStr = string.Empty;
            return Ret.ToList();
        }
    }
}