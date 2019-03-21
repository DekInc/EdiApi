using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EdiViewer.Controllers
{
    public partial class PreRunExternController : Controller
    {        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetObjSession<string>("Session.HashId")))
            {
                filterContext.Result = new RedirectResult("/Account/?error=NO_AUTH");
            }
            else
            {
                if (!HttpContext.Session.GetObjSession<bool>("Session.IsExtern"))
                    filterContext.Result = new RedirectResult("/Home/");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}