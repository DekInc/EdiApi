using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdiViewer.Models;
using Microsoft.Extensions.Configuration;
using ComModels;

namespace EdiViewer.Controllers
{
    public class HomeController : Controller
    {
        public readonly IConfiguration Config;
        IConfiguration EdiWebApiConfig => Config.GetSection("EdiWebApi");
        public HomeController(IConfiguration _Config) {
            Config = _Config;
            ApplicationSettings.ApiUri = (string)EdiWebApiConfig.GetValue(typeof(string), "ApiUri");
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(string HashId = "")
        {
            EdiDetailModel EdiDetailModelO = new EdiDetailModel();
            IEnumerable<LearPureEdi> ListPureEdi = await ApiClientFactory.Instance.GetPureEdi(string.Empty);
            if (ListPureEdi.FirstOrDefault().Log.Contains("error"))
                return View(EdiDetailModelO);
            if (string.IsNullOrEmpty(HashId))
                return View(EdiDetailModelO);
            EdiDetailModelO.Data = await ApiClientFactory.Instance.GetFE830Data(HashId);
            if (EdiDetailModelO.Data.ListSt == null)
                return View(new EdiDetailModel());
            return View(EdiDetailModelO);
        }
        public async Task<IActionResult> GetPureEdi()
        {
            try
            {
                //var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                IEnumerable<LearPureEdi> IEPureEdi = await ApiClientFactory.Instance.GetPureEdi(string.Empty);                
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    IEPureEdi = IEPureEdi.Where(m => m.Fingreso == searchValue);
                }
                //total number of rows count   
                recordsTotal = IEPureEdi.Count();
                //Paging   
                var data = IEPureEdi.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception e1)
            {
                throw e1;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
