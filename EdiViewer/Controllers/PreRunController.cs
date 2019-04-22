﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EdiViewer.Controllers
{
    public partial class PreRunController : Controller
    {        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetObjSession<string>("Session.HashId")))
            {
                filterContext.Result = new RedirectResult("/Account/?error=NO_AUTH");
            }
            List<string> ListPermits = HttpContext.Session.GetObjSession<List<string>>("ListPermits");
            if (ListPermits == null)
                ListPermits = new List<string>();
            if (ListPermits.Count == 0)
            {
                IEnumerable<IenetAccesses> ListAccesses = HttpContext.Session.GetObjSession<IEnumerable<IenetAccesses>>("ListAccesses");
                if (ListAccesses != null)
                {
                    foreach (IenetAccesses A in ListAccesses)
                        if (HttpContext.Session.HavePermits(A.Descr))
                            ListPermits.Add(A.Descr);
                    HttpContext.Session.SetObjSession("ListPermits", ListPermits);
                }
            }
            //else {
            //    if (HttpContext.Session.GetObjSession<bool>("Session.IsExtern"))
            //        filterContext.Result = new RedirectResult("/HomeExtern/");
            //}            
            base.OnActionExecuting(filterContext);
        }
    }
}