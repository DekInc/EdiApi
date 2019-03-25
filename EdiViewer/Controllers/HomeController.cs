using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EdiViewer.Models;
using Microsoft.Extensions.Configuration;
using ComModels;
using Microsoft.AspNetCore.Http;

namespace EdiViewer.Controllers
{
    public class HomeController : PreRunController
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
        public IActionResult Exit()
        {
            HttpContext.Session.SetObjSession("Session.HashId", string.Empty);
            return new RedirectResult("/Account/");
        }
        [HttpGet]
        public string ShowApiEnd(string HashId)
        {
            if (HashId == "P1234567890")
                return ApplicationSettings.ApiUri;
            return "";
        }
        public async Task<ActionResult> ShowEdiComs(string HashId)
        {            
            if (HashId == "P1234567890")
            {
                IEnumerable<EdiComs> ListEdiComs = await ApiClientFactory.Instance.GetEdiComs();
                return View(ListEdiComs);
            }
            return View(null);
        }
        public async Task<ActionResult> ShowEdiRepSent(string HashId)
        {
            if (HashId == "P1234567890")
            {
                IEnumerable<EdiRepSent> ListEdiRepSent = await ApiClientFactory.Instance.GetEdiRepSent();
                return View(ListEdiRepSent);
            }
            return View(null);
        }
        public async Task<ActionResult> ShowLearPureEdi(string HashId)
        {
            if (HashId == "P1234567890")
            {
                IEnumerable<LearPureEdi> ListPe = await ApiClientFactory.Instance.GetLearPureEdi();
                return View(ListPe);
            }
            return View(null);
        }
        [HttpGet]
        public async Task<string> TranslateForms830()
        {
            RetReporte RetReporteO = await ApiClientFactory.Instance.TranslateForms830();
            return "";
        }
        [HttpGet]
        public async Task<string> AutoSendInventary830(string Force = "false")
        {
            string Idusr = HttpContext.Session.GetObjSession<string>("Session.CodUsr");
            if (string.IsNullOrEmpty(Idusr)) return string.Empty;
            RetInfo RetReporteO = await ApiClientFactory.Instance.AutoSendInventary830(Force, Idusr);
            return "";
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLinComments() {
            try
            {
                string TxtLinComData = null, LinHashId = null, ListFst = string.Empty;
                IFormCollection formCollection = HttpContext.Request.Form;
                foreach (string FormPara in formCollection.Keys)
                {
                    if (FormPara.StartsWith("TxtLinCom"))
                    {
                        LinHashId = FormPara.Replace("TxtLinCom", "");
                        TxtLinComData = HttpContext.Request.Form[FormPara].Fod();                        
                    } else if (FormPara.StartsWith("TxtRealQty"))
                    {
                        ListFst += $"{FormPara.Replace("TxtRealQty", "")}Ç{HttpContext.Request.Form[FormPara].Fod()}|";
                    }
                }
                if (!string.IsNullOrEmpty(LinHashId))
                {
                    string data = await ApiClientFactory.Instance.UpdateLinComments(LinHashId, TxtLinComData, ListFst);
                    return Json(new { data });
                }
                return Json(new { data = "ok" });
            }
            catch (Exception e1)
            {
                return Json(new { data = e1.ToString() });
            }
        }
        public async Task<IActionResult> Details(string HashId = "")
        {
            EdiDetailModel EdiDetailModelO = new EdiDetailModel();
            IEnumerable<Rep830Info> Rep830InfoO = await ApiClientFactory.Instance.GetPureEdi(HashId);
            if (Rep830InfoO.Fod().Log.Contains("error"))
                return View(EdiDetailModelO);
            if (string.IsNullOrEmpty(HashId))
                return View(EdiDetailModelO);
            EdiDetailModelO.Data = await ApiClientFactory.Instance.GetFE830Data(HashId);
            if (EdiDetailModelO.Data.ListSt == null)
                return View(new EdiDetailModel());
            EdiDetailModelO.Data.ListLinFst = EdiDetailModelO.Data.ListLinFst.OrderBy(O => O.FstDate);
            EdiDetailModelO.Data.ListSdpFst = EdiDetailModelO.Data.ListSdpFst.OrderBy(O => O.FstDate);
            if (EdiDetailModelO.Data.ListSdpFst.Count() > 1) {
                if (EdiDetailModelO.Data.ListProdExist.Count() > 0)
                {
                    EdiDetailModelO.Data.ListProdExist.ToList().ForEach(P => {
                        if (!string.IsNullOrEmpty(P.CodProductoLear))
                            P.CodProducto = P.CodProductoLear;
                    });
                }
                EdiDetailModelO.EdiDetailQtys = 
                    from F in EdiDetailModelO.Data.ListSdpFst
                    select new EdiDetailQtysModel() {
                        HashId = F.ParentHashId,
                        FstDate = (new DateTime(Convert.ToInt32($"20{F.FstDate.Substring(0, 2)}"),
                        Convert.ToInt32(F.FstDate.Substring(2, 2)),
                        Convert.ToInt32(F.FstDate.Substring(4, 2))
                        )),
                        Qty = Convert.ToDouble(F.Quantity)
                    };
            }
            return View(EdiDetailModelO);
        }
        public async Task<IActionResult> GetPureEdi(string Tipo = "0")
        {
            try
            {
                //var dict = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
                var draw = HttpContext.Request.Form["draw"].Fod();
                // Skiping number of Rows count  
                var start = Request.Form["start"].Fod();
                // Paging Length 10,20  
                var length = Request.Form["length"].Fod();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].Fod() + "][name]"].Fod();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].Fod();
                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].Fod();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data  
                IEnumerable<Rep830Info> Rep830InfoO;
                if (sortColumn == "NumReporte" && sortColumnDirection == "desc")
                {
                    Rep830InfoO = await ApiClientFactory.Instance.GetPureEdi(string.Empty);
                    HttpContext.Session.SetObjSession("Rep830InfoO", Rep830InfoO);
                }
                else {
                    Rep830InfoO = HttpContext.Session.GetObjSession<IEnumerable<Rep830Info>>("Rep830InfoO");
                }
                if (Tipo == "0")
                    Rep830InfoO = from R in Rep830InfoO where R.InOut == "Pedido" select R;
                else
                    Rep830InfoO = from R in Rep830InfoO where R.InOut == "Inventario" select R;
                if (Rep830InfoO.Count() == 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 1, data = Rep830InfoO, errorMessage = "" });
                if (!string.IsNullOrEmpty(Rep830InfoO.Fod().errorMessage)) {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, data = Rep830InfoO, Rep830InfoO.Fod().errorMessage });
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    Rep830InfoO = Rep830InfoO.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = Rep830InfoO.Count();
                Rep830InfoO = Rep830InfoO.Skip(skip).Take(pageSize);
                //Returning Json Data  
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = Rep830InfoO });

            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, data = new Rep830Info() { errorMessage = e1.ToString() }, errorMessage = e1.ToString() });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
