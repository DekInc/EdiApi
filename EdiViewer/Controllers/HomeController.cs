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
            Rep830Info Rep830InfoO = await ApiClientFactory.Instance.GetPureEdi(string.Empty);
            if (Rep830InfoO.LearPureEdi.FirstOrDefault().Log.Contains("error"))
                return View(EdiDetailModelO);
            if (string.IsNullOrEmpty(HashId))
                return View(EdiDetailModelO);
            EdiDetailModelO.Data = await ApiClientFactory.Instance.GetFE830Data(HashId);            
            if (EdiDetailModelO.Data.ListSt == null)
                return View(new EdiDetailModel());
            EdiDetailModelO.Data.ListLinFst = EdiDetailModelO.Data.ListLinFst.OrderBy(O => O.FstDate);
            EdiDetailModelO.Data.ListSdpFst = EdiDetailModelO.Data.ListSdpFst.OrderBy(O => O.FstDate);
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
                Rep830Info Rep830InfoO = await ApiClientFactory.Instance.GetPureEdi(string.Empty);                
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    Rep830InfoO.LearPureEdi = Rep830InfoO.LearPureEdi.Where(m => m.Fingreso == searchValue);
                }
                //total number of rows count   
                recordsTotal = Rep830InfoO.LearPureEdi.Count();
                //Paging   
                var data = from Pe in Rep830InfoO.LearPureEdi
                           from F in Rep830InfoO.From
                           .Where(Fr => Fr.HashId == Pe.HashId).DefaultIfEmpty()
                           from T in Rep830InfoO.To
                           .Where(To => To.HashId == Pe.HashId).DefaultIfEmpty()
                           select new {
                               Pe.HashId,
                               Pe.NombreArchivo,
                               From = F?.Dest,
                               To = T?.Dest,
                               Pe.Fingreso,
                               Pe.Fprocesado,
                               Pe.Reprocesar,
                               Pe.Log
                           };

                Rep830InfoO.LearPureEdi = Rep830InfoO.LearPureEdi.Skip(skip).Take(pageSize);
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
