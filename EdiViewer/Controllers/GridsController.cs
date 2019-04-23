using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ComModels;
using EdiViewer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace EdiViewer.Controllers
{
    public class GridsController : PreRunExternController {
        public async Task<IActionResult> GetPaylessProdPrioriAdmin(string dtpPeriodoBuscar) {
            try {
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";
                if (string.IsNullOrEmpty(dtpPeriodoBuscar))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Item2.Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())) .ToList();
                int Total = Records.Count;
                if (Records.Count() > 0)
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                return Json(new { Total, Records, errorMessage = "" });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPaylessProdPriori(string dtpPeriodoBuscar) {
            try {
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";
                if (string.IsNullOrEmpty(dtpPeriodoBuscar))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                HttpContext.Session.SetObjSession("dtpPeriodoBuscar", dtpPeriodoBuscar);
                RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Item2.Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Barcode, Pp.Tienda, Pp.Producto, Pp.Talla, Pp.Categoria, Pp.Departamento, Pp.Cp }
                    into Grp
                    select new PaylessProdPrioriDetModel {
                        Barcode = Grp.Fod().Barcode,
                        Pri = Grp.Fod().Tienda,
                        Producto = Grp.Fod().Producto,
                        Talla = Grp.Fod().Talla,
                        Categoria = Grp.Fod().Categoria,
                        Departamento = Grp.Fod().Departamento,
                        Cp = Grp.Fod().Cp,
                        Id = Grp.Fod().Id,
                        Peso = Grp.Count()
                    }
                    ).ToList();
                int Total = Records.Count;
                if (Records.Count() > 0)
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                return Json(new { Total, Records, errorMessage = "" });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<IActionResult> GetPaylessProdPrioriDet(string barcode, string estilo) {
            try {
                string dtpPeriodoBuscar = HttpContext.Session.GetObjSession<string>("dtpPeriodoBuscar");
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";                
                if (string.IsNullOrEmpty(barcode))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                if (string.IsNullOrEmpty(estilo))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Item2.Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                Records = Records.Where(Pp => Pp.Barcode == barcode).ToList();
                Records = Records.Where(Pp => Pp.Producto == estilo).ToList();
                int Total = Records.Count;
                if (Records.Count() > 0)
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                return Json(new { Total, Records, errorMessage = "" });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
    }
}