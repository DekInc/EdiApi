using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EdiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EdiController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        [Route("~/Edi/Form830")] // De modo que se expone https://localhost:44373/Edi/Form830
        public ActionResult<IEnumerable<string>> Form830()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("~/Edi/Form856")]
        public ActionResult<IEnumerable<string>> Form856()
        {
            return new string[] { "value5", "value4" };
        }        
    }
}
