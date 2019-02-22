using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreApiClient;

namespace EdiViewer.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View(new Models.ErrorModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(string TxtUser, string TxtPassword) {
            try
            {
                string HashId = await ApiClientFactory.Instance.Login(TxtUser, TxtPassword);
                if (HashId.StartsWith("Error:"))
                    return View("Index", new Models.ErrorModel() { ErrorMessage = System.Net.WebUtility.HtmlEncode(HashId.Replace(Environment.NewLine, "<br />")) });
                if (!string.IsNullOrEmpty(HashId))
                    HashId += DateTime.Now.ToString(ApplicationSettings.DateTimeFormatL);
                HttpContext.Session.SetObjSession("Session.HashId", HashId);
                if (string.IsNullOrEmpty(HashId))
                    return LocalRedirect("/Account/?error=USER_INCORRECT");
                return LocalRedirect("/");
            }
            catch (Exception e1)
            {
                return View("Index", new Models.ErrorModel() { ErrorMessage = e1.ToString().Replace("'", "") });
            }            
        }
    }
}