using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ComModels;
using Microsoft.AspNetCore.Mvc;

namespace EdiViewer.Controllers
{
    public class Rep856Controller : PreRunController
    {
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> SendForm()
        {
            IEnumerable<TsqlDespachosWmsComplex> TsqlDespachosWmsComplexO;
            List<string[]> ListSelected = new List<string[]>();
            Microsoft.AspNetCore.Http.IFormCollection formCollection = HttpContext.Request.Form;
            foreach(string FormPara in formCollection.Keys)
                if (FormPara.StartsWith("chkS"))
                    ListSelected.Add(FormPara.Replace("chkS", "").Split('|'));
            if (ListSelected.Count > 0)
            {
                IEnumerable<string> ListDispatch = ListSelected.Select(O1 => O1.Fod()).Distinct();
                //IEnumerable<string> ListProducts = ListSelected.Select(O1 => O1.LastOrDefault()).ToArray();
                //TsqlDespachosWmsComplexO = await ApiClientFactory.Instance.GetSN(ListDispatch, ListProducts);
                string s1 = await ApiClientFactory.Instance.SendForm856(ListDispatch);
                
                return Json(new { data = "ok" });
            }
            return Json(new { data = "" });
        }
        public IActionResult GetGridData(string destino = "") {
            try
            {
                if (destino == "null") destino = string.Empty;
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
                IEnumerable<TsqlDespachosWmsComplex> TsqlDespachosWmsComplexO = ApiClientFactory.Instance.GetSN().Result;                
                IEnumerable<string> ListTo = (from D in TsqlDespachosWmsComplexO
                                             orderby D.CodProducto
                                             select D.CodProducto).Distinct();
                if (TsqlDespachosWmsComplexO.Count() == 1)
                {
                    if (TsqlDespachosWmsComplexO.Fod().DespachoId == 0)
                    {
                        if (string.IsNullOrEmpty(TsqlDespachosWmsComplexO.Fod().ErrorMessage))
                        {
                            return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, TsqlDespachosWmsComplexO.Fod().ErrorMessage, ListTo });
                        }
                    }
                }
                //Search                
                if (!string.IsNullOrEmpty(destino))
                {
                    TsqlDespachosWmsComplexO = TsqlDespachosWmsComplexO.Where(Co => Co.CodProducto == destino);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    TsqlDespachosWmsComplexO = TsqlDespachosWmsComplexO.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = TsqlDespachosWmsComplexO.Count();
                //Paging
                TsqlDespachosWmsComplexO = TsqlDespachosWmsComplexO.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = TsqlDespachosWmsComplexO, errorMessage = "", ListTo });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), ListTo = "" });
            }
        }
    }
}