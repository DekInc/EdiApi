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
                {                    
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
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
                    group Pp by new { Pp.Barcode, Pp.Tienda, Pp.Talla, Pp.Categoria, Pp.Departamento, Pp.Cp }
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
        public async Task<IActionResult> GetPaylessProdPrioriInventario(string dtpPeriodoBuscar) {
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
                RetData<IEnumerable<PaylessTiendas>> ListClients = await ApiClientFactory.Instance.GetAllPaylessStores(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                if (ListClients.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListClients.Info.CodError != 0 ? ListClients.Info.Mensaje : string.Empty) });
                IEnumerable<PaylessProdPrioriDetModel> AllRecords = ListProd.Data.Item2.Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                string IdTienda = ListClients.Data.Where(C => C.ClienteId == HttpContext.Session.GetObjSession<int>("Session.ClientId")).Fod().TiendaId.ToString();                
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Item2.Where(R => R.Tienda == IdTienda).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Barcode, Pp.Tienda, Pp.Talla, Pp.Categoria, Pp.Departamento, Pp.Cp }
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
                {
                    RetData<IEnumerable<ExistenciasExternModel>> StockData = await ApiClientFactory.Instance.GetStock(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                    foreach (ExistenciasExternModel Stock in StockData.Data)
                    {
                        foreach (PaylessProdPrioriDetModel Product in Records.Where(P => P.Barcode == Stock.CodProducto))
                        {
                            Product.Existencia = Convert.ToInt32(Stock.Existencia);
                            Product.Reservado = Convert.ToInt32(Stock.Reservado);
                        }                        
                    }
                    if (StockData.Info.CodError != 0)
                        return Json(new { total = 0, records = "", errorMessage = StockData.Info.Mensaje });
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<IActionResult> GetPaylessProdPrioriDet(string barcode, string talla) {
            try {
                string dtpPeriodoBuscar = HttpContext.Session.GetObjSession<string>("dtpPeriodoBuscar");
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";                
                if (string.IsNullOrEmpty(barcode))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                if (string.IsNullOrEmpty(talla))
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
                Records = Records.Where(Pp => Pp.Talla == talla).ToList();
                int Total = Records.Count;
                if (Records.Count() > 0)
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                return Json(new { Total, Records, errorMessage = "" });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPaylessPeriodPrioriFile() {
            try {
                RetData<Tuple<IEnumerable<PaylessProdPrioriArchMModel>, IEnumerable<PaylessProdPrioriArchDet>>> ListProdPrioriArch = await ApiClientFactory.Instance.GetPaylessPeriodPrioriFile();
                if (ListProdPrioriArch.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProdPrioriArch.Info.Mensaje });
                if (ListProdPrioriArch.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                if (ListProdPrioriArch.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriArchMModel> Records = ListProdPrioriArch.Data.Item1.OrderByDescending(O => O.Periodo.ToDateFromEspDate()).ToList();
                int Total = Records.Count;
                if (Records.Count() > 0)
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                return Json(new { Total, Records, errorMessage = "" });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPaylessFileDif(string idProdArch, int idData = 1) {
            try {
                //if (idProdArch == "0") return null;
                RetData<Tuple<IEnumerable<PaylessProdPrioriDet>, IEnumerable<PaylessProdPrioriDet>, IEnumerable<PaylessProdPrioriDet>>> ListProdPrioriArch = await ApiClientFactory.Instance.GetPaylessFileDif(Convert.ToInt32(idProdArch));
                if (ListProdPrioriArch.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProdPrioriArch.Info.Mensaje });
                if (ListProdPrioriArch.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                if (ListProdPrioriArch.Data.Item1.Count() == 0 && idData == 1)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                if (ListProdPrioriArch.Data.Item2.Count() == 0 && idData == 2)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                if (ListProdPrioriArch.Data.Item3.Count() == 0 && idData == 3)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDet> Item1 = ListProdPrioriArch.Data.Item1.ToList();
                List<PaylessProdPrioriDet> Item2 = ListProdPrioriArch.Data.Item2.ToList();
                List<PaylessProdPrioriDet> Item3 = ListProdPrioriArch.Data.Item3.ToList();
                int Total = 0;
                switch (idData) {
                    case 1:
                        Total = Item1.Count;
                        if (Total > 0)
                            Item1 = Utility.ExpressionBuilderHelper.W2uiSearch(Item1, Request.Form);
                        break;
                    case 2:
                        Total = Item2.Count;
                        if (Total > 0)
                            Item2 = Utility.ExpressionBuilderHelper.W2uiSearch(Item2, Request.Form);
                        break;
                    case 3:
                        Total = Item3.Count;
                        if (Total > 0)
                            Item3 = Utility.ExpressionBuilderHelper.W2uiSearch(Item3, Request.Form);
                        break;
                    default:
                        break;
                }
                if (idData == 1) return Json(new { Total, Records = Item1, errorMessage = "" });
                if (idData == 2) return Json(new { Total, Records = Item2, errorMessage = "" });
                if (idData == 3) return Json(new { Total, Records = Item3, errorMessage = "" });
                return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
    }
}