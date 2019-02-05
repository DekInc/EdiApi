﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComModels;
using Microsoft.AspNetCore.Mvc;

namespace EdiViewer.Controllers
{
    public class Rep856Controller : Controller
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
                IEnumerable<string> ListDispatch = ListSelected.Select(O1 => O1.FirstOrDefault()).Distinct();
                IEnumerable<string> ListProducts = ListSelected.Select(O1 => O1.LastOrDefault()).ToArray();
                TsqlDespachosWmsComplexO = await ApiClientFactory.Instance.GetSN(ListDispatch, ListProducts);
                return View(TsqlDespachosWmsComplexO);
            }
            return View();
        }
        public async Task<IActionResult> GetGridData(string destino = "") {
            try
            {
                if (destino == "null") destino = string.Empty;
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
                IEnumerable<TsqlDespachosWmsComplex> TsqlDespachosWmsComplexO = await ApiClientFactory.Instance.GetSN();
                if (TsqlDespachosWmsComplexO.Count() == 1)
                {
                    if (TsqlDespachosWmsComplexO.FirstOrDefault().DespachoId == 0)
                    {
                        if (string.IsNullOrEmpty(TsqlDespachosWmsComplexO.FirstOrDefault().errorMessage))
                        {
                            return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = TsqlDespachosWmsComplexO.FirstOrDefault().errorMessage });
                        }
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(destino))
                {
                    TsqlDespachosWmsComplexO = TsqlDespachosWmsComplexO.Where(Co => Co.Destino == destino);
                }
                //total number of rows count
                recordsTotal = TsqlDespachosWmsComplexO.Count();
                //Paging
                TsqlDespachosWmsComplexO = TsqlDespachosWmsComplexO.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = TsqlDespachosWmsComplexO, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString() });
            }
        }
    }
}