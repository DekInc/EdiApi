using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EdiViewer.Controllers
{
    public class HomeExternController : PreRunExternController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Exit()
        {
            HttpContext.Session.SetObjSession("Session.HashId", string.Empty);
            return new RedirectResult("/Account/");
        }
    }
}