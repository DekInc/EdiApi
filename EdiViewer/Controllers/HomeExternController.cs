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
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace EdiViewer.Controllers
{
    public class HomeExternController : PreRunExternController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Exit()
        {
            HttpContext.Session.SetObjSession("Session.HashId", string.Empty);
            return new RedirectResult("/Account/");
        }
        public IActionResult PaylessEncuesta() {
            return View();
        }
        public IActionResult Pedidos()
        {
            return View();
        }
        public IActionResult CrudPaylessTemporadas() {
            return View();
        }        
        public IActionResult PedidosFacturas() {
            return View();
        }
        public IActionResult InvPaylessTienda() {
            return View();
        }        
        public IActionResult PaylessReportes() {
            return View();
        }        
        public async Task<IActionResult> PedidosDet(int PedidoId)
        {
            RetData<IEnumerable<TsqlDespachosWmsComplex>> ListPe = await ApiClientFactory.Instance.GetPedidosDet(PedidoId);
            if (ListPe.Info.CodError == 0)
                return View(ListPe);
            else
                return View();
        }
        //public async Task<string> UpdateDis()
        //{
        //    RetData<IEnumerable<TsqlDespachosWmsComplex>> ListPe = await ApiClientFactory.Instance.GetPedidosDet(PedidoId);
        //    if (ListPe.Info.CodError == 0)
        //        return View(ListPe);
        //    else
        //        return View();
        //}
        public IActionResult Peticiones()
        {
            return View(new ErrorModel());
        }
        public IActionResult PeticionesAdmin() {
            return View();
        }
        public IActionResult PeticionesAdminB() {
            return View();
        }
        public IActionResult PeticionDet(int PedidoId)
        {
            HttpContext.Session.SetObjSession("PedidoId", PedidoId);
            return View();
        }
        public IActionResult CargaProdPriori()
        {
            return View();
        }
        public IActionResult CargaProdPrioriAdmin()
        {
            return View();
        }
        public IActionResult CargaProdPriori2()
        {
            return View();
        }
        public IActionResult CargaProdArchBod(string idM = "0")
        {
            //IdM para exportar a Excel :)
            return View();
        }
        public IActionResult CargaWmsIngreso() {
            return View();
        }
        public IActionResult CrearWmsSalida() {
            return View();
        }
        public async Task<IActionResult> SetNewDisPayless(string dtpFechaEntrega, int txtWomanQty, int txtManQty, int txtKidQty, int txtAccQty, int? txtWomanQtyT, int? txtManQtyT, int? txtKidQtyT, int? txtAccQtyT, string radInvType) {
            ViewBag.PedidoIdToModify = 0;
            txtWomanQtyT = txtWomanQtyT == 0 ? null : txtWomanQtyT;
            txtManQtyT = txtManQtyT == 0 ? null : txtManQtyT;
            txtKidQtyT = txtKidQtyT == 0 ? null : txtKidQtyT;
            txtAccQtyT = txtAccQtyT == 0 ? null : txtAccQtyT;
            if (string.IsNullOrEmpty(radInvType)) {
                return View("PedidosPayless3", new ErrorModel() { ErrorMessage = "El tipo de pedido está vacío." });
            }
            if (string.IsNullOrEmpty(dtpFechaEntrega)) {
                return View("PedidosPayless3", new ErrorModel() { ErrorMessage = "La fecha de entrega está vacía." });
            }
            if (txtWomanQty.Equals("0") && txtManQty.Equals("0") && txtKidQty.Equals("0") && txtAccQty.Equals("0")) {
                return View("PedidosPayless3", new ErrorModel() { ErrorMessage = "La cantidad a pedir es cero para todas las categorias." });
            }
            DateTime DateDis = dtpFechaEntrega.ToDate();
            if ((DateDis - DateTime.Now).TotalHours < 24) {
                if (HttpContext.Session.GetObjSession<int>("Session.IdGroup") != 1)
                    return View("PedidosPayless3", new ErrorModel() { ErrorMessage = "La fecha y hora del pedido debe ser con más de 24 horas de anticipación." });
            }
            if (DateDis < DateTime.Now) {
                return View("PedidosPayless3", new ErrorModel() { ErrorMessage = "La fecha es anterior a la fecha actual." });
            }
            List<PaylessProdPrioriDetModel> ListQtys = HttpContext.Session.GetObjSession<List<PaylessProdPrioriDetModel>>("Session.StoreQtys");
            bool? FullPed = null;
            FullPed = (
                ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "0").Fod().Existencia == txtWomanQty
                && ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "1").Fod().Existencia == txtManQty
                && ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "2").Fod().Existencia == txtKidQty
                && ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "3").Fod().Existencia == txtAccQty
                );
            RetData<string> Ret = await ApiClientFactory.Instance.SetNewDisPayless(dtpFechaEntrega, txtWomanQty, txtManQty, txtKidQty, txtAccQty, radInvType, HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"), null, (FullPed == false? null : FullPed), null, txtWomanQtyT, txtManQtyT, txtKidQtyT, txtAccQtyT, HttpContext.Session.GetObjSession<string>("Session.CodUsr"));
            if (Ret.Info.CodError == 0) {
                return View("Peticiones", new ErrorModel() { Typ = 1, ErrorMessage = Ret.Data });
            } else {
                return View("PedidosPayless3", new ErrorModel() { ErrorMessage = Ret.Info.Mensaje });
            }
        }
        public async Task<IActionResult> SetNewDisPaylessDivert(string dtpFechaEntrega, int txtWomanQty, int txtManQty, int txtKidQty, int txtAccQty, string radInvType, string cboTiendaOr, string cboTiendaDest) {
            ViewBag.PedidoIdToModify = 0;
            //return View("PedidosPaylessDivert", new ErrorModel() { Typ = 1, ErrorMessage = "" });
            if (string.IsNullOrEmpty(radInvType)) {
                return View("PedidosPaylessDivert", new ErrorModel() { ErrorMessage = "El tipo de pedido está vacío." });
            }
            if (string.IsNullOrEmpty(dtpFechaEntrega)) {
                return View("PedidosPaylessDivert", new ErrorModel() { ErrorMessage = "La fecha de entrega está vacía." });
            }
            if (txtWomanQty.Equals("0") && txtManQty.Equals("0") && txtKidQty.Equals("0") && txtAccQty.Equals("0")) {
                return View("PedidosPaylessDivert", new ErrorModel() { ErrorMessage = "La cantidad a pedir es cero para todas las categorias." });
            }
            DateTime DateDis = dtpFechaEntrega.ToDate();
            if ((DateDis - DateTime.Now).TotalHours < 24) {
                return View("PedidosPaylessDivert", new ErrorModel() { ErrorMessage = "La fecha y hora del pedido debe ser con más de 24 horas de anticipación." });
            }
            List<PaylessProdPrioriDetModel> ListQtys = HttpContext.Session.GetObjSession<List<PaylessProdPrioriDetModel>>("Session.StoreQtys");
            bool? FullPed = null;
            FullPed = (
                ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "0").Fod().Existencia == txtWomanQty
                && ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "1").Fod().Existencia == txtManQty
                && ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "2").Fod().Existencia == txtKidQty
                && ListQtys.Where(O => O.Cp == "0" && O.Categoria.ToUpper() == "3").Fod().Existencia == txtAccQty
                );
            RetData<string> Ret = new RetData<string>();
            Ret = await ApiClientFactory.Instance.SetNewDisPayless(dtpFechaEntrega, txtWomanQty, txtManQty, txtKidQty, txtAccQty, radInvType, HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"), true, (FullPed == false ? null : FullPed), Convert.ToInt32(cboTiendaDest), null, null, null, null, HttpContext.Session.GetObjSession<string>("Session.CodUsr"));
            if (Ret.Info.CodError == 0) {
                return View("PedidosPaylessDivert", new ErrorModel() { Typ = 1, ErrorMessage = Ret.Data });
            } else {
                return View("PedidosPaylessDivert", new ErrorModel() { ErrorMessage = Ret.Info.Mensaje });
            }
        }
        public async Task<RetData<string>> SetPaylessProdPriori(string dtpPeriodUpload, string txtTransporte, bool ChkUpDelete)
        {
            DateTime StartTime = DateTime.Now;
            List<string> ListCols = new List<string>();
            List<PaylessUploadFileModel> ListExcelRows = new List<PaylessUploadFileModel>();
            try
            {
                IFormFile FileUploaded = Request.Form.Files[0];
                StringBuilder sb = new StringBuilder();
                if (FileUploaded.Length > 0)
                {
                    string FileExtension = Path.GetExtension(FileUploaded.FileName).ToLower();
                    ISheet Sheet;
                    using (MemoryStream stream = new MemoryStream())
                    {
                        FileUploaded.CopyTo(stream);
                        stream.Position = 0;
                        if (FileExtension == ".xls")
                        {
                            try
                            {
                                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                                Sheet = hssfwb.GetSheetAt(0);
                            }
                            catch (Exception e2)
                            {
                                throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                            }                            
                        }
                        else if (FileExtension == ".xlsx")
                        {
                            try
                            {
                                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                                Sheet = hssfwb.GetSheetAt(0);
                            }
                            catch (Exception e3)
                            {
                                throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e3.ToString());
                            }                            
                        }
                        else
                        {
                            return new RetData<string>()
                            {
                                Data = "",
                                Info = new RetInfo()
                                {
                                    CodError = -1,
                                    Mensaje = "El archivo no tiene la extensión .xls o .xlsx",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                        IRow HeaderRow = Sheet.GetRow(0);
                        PaylessUploadFileModel NewRow = new PaylessUploadFileModel();
                        int CellCount = HeaderRow.LastCellNum;
                        sb.Append("<table class='table'><tr>");
                        for (int j = 0; j < CellCount; j++)
                        {
                            bool PropExists = false;
                            foreach (PropertyInfo Pi in NewRow.GetType().GetProperties())
                            {
                                if (Pi.Name.Trim().ToLower() == ((NPOI.SS.UserModel.ICell)HeaderRow.GetCell(j)).ToString().ToLower().Trim().Replace(".", "").Replace(" ", ""))
                                {
                                    PropExists = true;
                                    ListCols.Add(Pi.Name.Replace(".", "").Replace(" ", "").Trim());
                                }
                            }
                            if (!PropExists)
                            {
                                return new RetData<string>()
                                {
                                    Data = "",
                                    Info = new RetInfo()
                                    {
                                        CodError = -1,
                                        Mensaje = "El archivo contiene columnas que no han sido establecidas, nombre de columna que da error: " + ((NPOI.SS.UserModel.ICell)HeaderRow.GetCell(j)).ToString(),
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                        }
                        for (int i = (Sheet.FirstRowNum + 1); i <= Sheet.LastRowNum; i++)
                        {
                            IRow row = Sheet.GetRow(i);
                            PaylessUploadFileModel NewRowInsert = new PaylessUploadFileModel();
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < CellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    try
                                    {
                                        switch (NewRowInsert.GetType().GetProperty(ListCols[j]).PropertyType.Name)
                                        {
                                            case "String":
                                                NewRowInsert.GetType().GetProperty(ListCols[j]).SetValue(NewRowInsert, row.GetCell(j).ToString());
                                                break;
                                            default:
                                                if (!string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                                {
                                                    if (ListCols[j].Equals("m3", StringComparison.OrdinalIgnoreCase) || ListCols[j].Equals("peso", StringComparison.OrdinalIgnoreCase))
                                                        NewRowInsert.GetType().GetProperty(ListCols[j]).SetValue(NewRowInsert, Convert.ToDouble(row.GetCell(j).ToString()));
                                                }
                                                break;
                                        }                                        
                                    }
                                    catch (Exception ec1)
                                    {
                                        return new RetData<string>()
                                        {
                                            Data = "",
                                            Info = new RetInfo()
                                            {
                                                CodError = -1,
                                                Mensaje = $"Error en conversión para el campo {ListCols[j]} {ec1.ToString()}",
                                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                            }
                                        };
                                    }                                    
                                }
                            }
                            ListExcelRows.Add(NewRowInsert); 
                        }                        
                    }
                }
                if (ListExcelRows.Count > 0) {
                    if (string.IsNullOrEmpty(ListExcelRows.LastOrDefault().Barcode))
                        ListExcelRows.RemoveAt(ListExcelRows.Count - 1);
                }
                RetData<string> Ret = await ApiClientFactory.Instance.SetPaylessProdPriori(ListExcelRows, HttpContext.Session.GetObjSession<int>("Session.ClientId"), dtpPeriodUpload, HttpContext.Session.GetObjSession<string>("Session.CodUsr"), txtTransporte, ChkUpDelete);
                return Ret;
            }
            catch (Exception ex1)
            {
                return new RetData<string>()
                {
                    Data = "",
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = ex1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<IActionResult> GetPedidos()
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
                RetData<IEnumerable<PedidosWmsModel>> ListPe = await ApiClientFactory.Instance.GetWmsGroupDispatchs(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = ListPe.Info.Mensaje, data = "" });
                if (ListPe.Data.Count() == 0)
                {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty), data = "" });
                }
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    if (sortColumn == "fechaPedido")
                //    {
                //        if (sortColumnDirection == "desc")
                //            ListPe.Data = ListPe.Data.OrderByDescending(O => O.FechaPedido.ToDateFromEspDate());
                //        else
                //            ListPe.Data = ListPe.Data.OrderBy(O => O.FechaPedido.ToDateFromEspDate());
                //    }
                //    else
                //        ListPe.Data = ListPe.Data.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //total number of rows count
                recordsTotal = ListPe.Data.Count();
                //Paging
                ListPe.Data = ListPe.Data.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = ListPe.Data, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<IActionResult> GetPedidosFacturas() {
            try {
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
                RetData<IEnumerable<PedidosWmsModel>> ListPe = await ApiClientFactory.Instance.GetWmsGroupDispatchsBills(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = ListPe.Info.Mensaje, data = "" });
                if (ListPe.Data.Count() == 0) {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty), data = "" });
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection))) {
                    if (sortColumn == "fechaPedido") {
                        if (sortColumnDirection == "desc")
                            ListPe.Data = ListPe.Data.OrderByDescending(O => O.FechaPedido.ToDateFromEspDate());
                        else
                            ListPe.Data = ListPe.Data.OrderBy(O => O.FechaPedido.ToDateFromEspDate());
                    } else
                        ListPe.Data = ListPe.Data.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = ListPe.Data.Count();
                //Paging
                ListPe.Data = ListPe.Data.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = ListPe.Data, errorMessage = "" });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<RetData<string>> GetClientName()
        {            
            DateTime StartTime = DateTime.Now;
            try
            {
                if (HttpContext.Session.GetObjSession<int?>("Session.TiendaId") == null) {
                    RetData<string> ClienteP = await ApiClientFactory.Instance.GetClientById(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                    return ClienteP;
                }
                RetData<PaylessTiendas> ClienteO = await ApiClientFactory.Instance.GetClient(HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                if (ClienteO.Info.CodError != 0) {
                    return new RetData<string>() {
                        Data = ClienteO.Info.Mensaje,
                        Info = ClienteO.Info
                    };
                }
                if (ClienteO.Data == null) {
                    return new RetData<string>() {
                        Info = new RetInfo() {
                            CodError = -1,
                            Mensaje = "No existe el cliente",
                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                        }
                    };
                }                
                return new RetData<string>()
                {
                    Data = $"{ClienteO.Data.TiendaId} - {ClienteO.Data.Descr}",
                    Info = ClienteO.Info
                };
            }
            catch (Exception e2)
            {
                return new RetData<string>()
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<Tuple<string, string>>> GetClientNameScheduleById() {
            DateTime StartTime = DateTime.Now;
            try {
                if (HttpContext.Session.GetObjSession<int?>("Session.TiendaId") != null) {
                    RetData<Tuple<string, string>> ClienteP = await ApiClientFactory.Instance.GetClientNameScheduleById(HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                    return ClienteP;
                }                
                return new RetData<Tuple<string, string>>() {
                    Data = new Tuple<string, string>("", ""),
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "El usuario no tiene una tienda asignada, tiene que establecerla para usar está pantalla.",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };                
            } catch (Exception e2) {
                return new RetData<Tuple<string, string>> {
                    Data = new Tuple<string, string>("", ""),
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<IActionResult> PedidosPayless() {
            try {
                ApiClientFactory.Instance.GetSetExistenciasByCliente(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<string>("Session.CodUsr"));
                ViewBag.ListOldDis = null;
                ViewBag.DateLastDis = DateTime.Now.ToString(ApplicationSettings.DateTimeFormatT);
                ViewBag.PedidoId = null;
                //HttpContext.Session.SetObjSession("PedidoId", null);
                //RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListDis = await ApiClientFactory.Instance.GetPedidosExternosByTienda(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                //if (ListDis.Info.CodError == 0) {
                //    if (ListDis.Data.Item1.Count() > 0) {
                //        PedidosExternos Pe = ListDis.Data.Item1.Where(O => O.IdEstado == 1).Fod();
                //        if (Pe != null) {
                //            HttpContext.Session.SetObjSession("PedidoId", Pe.Id);
                //            ViewBag.DateLastDis = Pe.FechaPedido;
                //            //if (ListDis.Data.Item2.Count() > 0) {
                //            //    ViewBag.ListOldDis = JsonConvert.SerializeObject(ListDis.Data.Item2.Where(Pde => Pde.PedidoId == ListDis.Data.Item1.Fod().Id).Select(Pd => new { codProducto = Pd.CodProducto.Replace(" ", "^"), cantPedir = Pd.CantPedir, producto = Pd.Producto }));
                //            //}
                //        }
                //    }
                //}
            } catch (Exception e1) {
                ViewBag.ClientName = e1.ToString();
            }
            return View();
        }
        public async Task<IActionResult> PedidosPayless2() {
            try {
                //ViewBag.ListOldDis = null;
                ViewBag.DateLastDis = DateTime.Now.ToString(ApplicationSettings.DateTimeFormatT);
                //ViewBag.PedidoId = null;
                //HttpContext.Session.SetObjSession("PedidoId", null);
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListDis = await ApiClientFactory.Instance.GetPedidosExternosByTienda(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                if (ListDis.Info.CodError == 0) {
                    if (ListDis.Data.Item1.Count() > 0) {
                        PedidosExternos Pe = ListDis.Data.Item1.Where(O => O.IdEstado == 1).Fod();
                        if (Pe != null) {
                            HttpContext.Session.SetObjSession("PedidoId", Pe.Id);
                            ViewBag.DateLastDis = Pe.FechaPedido;
                            //if (ListDis.Data.Item2.Count() > 0) {
                            //    ViewBag.ListOldDis = JsonConvert.SerializeObject(ListDis.Data.Item2.Where(Pde => Pde.PedidoId == ListDis.Data.Item1.Fod().Id).Select(Pd => new { codProducto = Pd.CodProducto.Replace(" ", "^"), cantPedir = Pd.CantPedir, producto = Pd.Producto }));
                            //}
                        }
                    }
                }
            } catch (Exception e1) {
                ViewBag.ClientName = e1.ToString();
            }
            return View();
        }
        public IActionResult PedidosPayless3() {
            ViewBag.PedidoIdToModify = 0;
            if (HttpContext.Session.GetObjSession<int?>("PedidoIdToModify") != null) {
                ViewBag.PedidoIdToModify = HttpContext.Session.GetObjSession<int?>("PedidoIdToModify");
                HttpContext.Session.SetObjSession("PedidoIdToModify", null);                
            }
            return View(new ErrorModel());
        }
        public IActionResult PedidosPaylessDivert() {
            ViewBag.PedidoIdToModify = 0;            
            return View(new ErrorModel());
        }
        public async Task<IActionResult> Inventario()
        {
            try
            {
                ViewBag.ListOldDis = null;
                ViewBag.DateLastDis = DateTime.Now.ToString(ApplicationSettings.DateTimeFormatT);
                ViewBag.PedidoId = null;
                HttpContext.Session.SetObjSession("PedidoId", null);
                //RetData<Clientes> ClienteO = await ApiClientFactory.Instance.GetClient(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListDis = await ApiClientFactory.Instance.GetPedidosExternosByTienda(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                if (ListDis.Data.Item1.Count() > 0)
                {
                    if (ListDis.Data.Item1.Fod().IdEstado == 1)
                    {
                        ViewBag.DateLastDis = ListDis.Data.Item1.Fod().FechaPedido;
                        HttpContext.Session.SetObjSession("PedidoId", ListDis.Data.Item1.Fod().Id);
                        if (ListDis.Data.Item2.Count() > 0)
                        {
                            ViewBag.ListOldDis = JsonConvert.SerializeObject(ListDis.Data.Item2.Where(Pde => Pde.PedidoId == ListDis.Data.Item1.Fod().Id).Select(Pd => new { codProducto = Pd.CodProducto.Replace(" ", "^"), cantPedir = Pd.CantPedir, producto = Pd.Producto }));
                        }
                    }
                }
                //ViewBag.ClientName = ClienteO.Data.Nombre;
                //if (!ClienteO.Data.EstatusId.HasValue) { 
                //    HttpContext.Session.SetObjSession("Session.IdPedidoExterno", 0);
                //} else {
                //    if (ClienteO.Data.EstatusId.Value != 0)
                //        HttpContext.Session.SetObjSession("Session.IdPedidoExterno", ClienteO.Data.EstatusId.Value);
                //    else
                //        HttpContext.Session.SetObjSession("Session.IdPedidoExterno", 0);
                //}
            }
            catch (Exception e1)
            {
                ViewBag.ClientName = e1.ToString();
            }            
            return View();
        }
        [HttpPost]
        public async Task<RetData<IEnumerable<ExistenciasExternModel>>> GetInventoryJson(bool chkOnlyAvailable, [FromBody]string ListDis)
        {
            DateTime StartTime = DateTime.Now;            
            try
            {
                IEnumerable<PedidoExternoModel> ListDis2 = JsonConvert.DeserializeObject<IEnumerable<PedidoExternoModel>>(ListDis);
                RetData<IEnumerable<ExistenciasExternModel>> StockData = await ApiClientFactory.Instance.GetStock(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (chkOnlyAvailable && StockData.Info.CodError == 0)
                    StockData.Data = StockData.Data.Where(Sd => Sd.Disponible > 0);
                if (ListDis2.Count() > 0)
                {
                    foreach (ExistenciasExternModel Ee in StockData.Data)
                        Ee.ClienteID = 0;
                    ListDis2.Where(D => D.cantPedir != "0").ToList().ForEach(Pr => {
                        IEnumerable<ExistenciasExternModel> ProdFound = StockData.Data.Where(Sd => Sd.CodProducto.Trim() == Pr.codProducto.Replace("^", " ").Trim());
                        ProdFound.Fod().ClienteID = 0;
                        if (ProdFound.Count() > 0)
                            ProdFound.Fod().ClienteID = Convert.ToInt32(Pr.cantPedir);
                        else
                            throw new Exception("El producto " + Pr.codProducto + " no fue encontrado en el array.");
                    });
                }
                return StockData;
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<ExistenciasExternModel>>()
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<IActionResult> GetInventory(bool chkOnlyAvailable)
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
                RetData<IEnumerable<ExistenciasExternModel>> StockData = await ApiClientFactory.Instance.GetStock(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (StockData.Info.CodError != 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = StockData.Info.Mensaje, data = "", listAllProd = "" });
                if (StockData.Data.Count() == 0)
                {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (StockData.Info.CodError != 0? StockData.Info.Mensaje : string.Empty), data = "", listAllProd = "" });
                }
                IEnumerable<ExistenciasExternModel> ListAllProd = StockData.Data;
                if (chkOnlyAvailable)
                    StockData.Data = StockData.Data.Where(Sd => Sd.Disponible > 0);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    StockData.Data = StockData.Data.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = StockData.Data.Count();
                //Paging
                StockData.Data = StockData.Data.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = StockData.Data, errorMessage = "", listAllProd = ListAllProd });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "", listAllProd = "" });
            }
        }        
        //[HttpPost]
        //public async Task<RetData<PedidosExternos>> SetPedidoExterno([FromBody]string Json, string cboPeriod, int TiendaId)
        //{
        //    return await SetPedidoExterno2(Json, 1, cboPeriod, TiendaId);
        //}
        [HttpPost]
        public async Task<RetData<PedidosExternos>> SendPedidoExterno([FromBody]string Json, string CboPeriod, string TiendaId, string TiendaIdDest)
        {
            return await SetPedidoExterno2(Json, 2, CboPeriod, TiendaIdDest);
        }
        private async Task<RetData<PedidosExternos>> SetPedidoExterno2(string Json, int IdEstado, string CboPeriod, string TiendaIdDest)
        {
            DateTime StartTime = DateTime.Now;            
            IEnumerable<PaylessProdPrioriDetModel> ListDis = JsonConvert.DeserializeObject<IEnumerable<PaylessProdPrioriDetModel>>(Json.ToString());            
            if (ListDis.Count() == 0)
            {                
                return new RetData<PedidosExternos> {
                    Info = new RetInfo {
                        CodError = -1,
                        Mensaje = "No hay productos en la lista",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            if ((ListDis.Fod().DateProm.ToDate() - StartTime).TotalHours < 24) {
                return new RetData<PedidosExternos> {
                    Info = new RetInfo {
                        CodError = -1,
                        Mensaje = "No se puede crear un pedido con menos de 24 horas de anticipación",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            foreach (PaylessProdPrioriDetModel Pe in ListDis)
            {
                Pe.IdPaylessProdPrioriM = HttpContext.Session.GetObjSession<int>("PedidoId");
                Pe.Barcode = Pe.Barcode;
            }
            try
            {
                RetData<PedidosExternos> RetDataO = await ApiClientFactory.Instance.SetPedidoExterno(ListDis, HttpContext.Session.GetObjSession<int>("Session.ClientId"), IdEstado, CboPeriod, TiendaIdDest);
                return RetDataO;
            }
            catch (Exception e2)
            {
                return new RetData<PedidosExternos> {
                    Info = new RetInfo {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<string>>> GetPaylessPeriodPriori()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                RetData<IEnumerable<string>> ListProdPriori = await ApiClientFactory.Instance.GetPaylessPeriodPriori();
                return ListProdPriori;
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<string>>()
                {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<string>>> GetPaylessPeriodPrioriByClient() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<string>> ListProdPriori = await ApiClientFactory.Instance.GetPaylessPeriodPriori();
                return ListProdPriori;
            } catch (Exception e1) {
                return new RetData<IEnumerable<string>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<Clientes>>> GetClients()
        {
            DateTime StartTime = DateTime.Now;
            try
            {
                RetData<IEnumerable<Clientes>> ListClients = await ApiClientFactory.Instance.GetClients();
                return ListClients;
            }
            catch (Exception e1)
            {
                return new RetData<IEnumerable<Clientes>>
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<PaylessProdPrioriArchM>> SetPaylessPeriodPrioriFile(string CboPeriod, int IdTransporte, string cboTipo, string dtpPeriodo)
        {
            DateTime StartTime = DateTime.Now;
            if (string.IsNullOrEmpty(cboTipo)) {
                return new RetData<PaylessProdPrioriArchM>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Los parámetros son incorrectos.",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            List<PaylessProdPrioriArchDet> ListBarcodesSalida = new List<PaylessProdPrioriArchDet>();
            List<PaylessProdPrioriArchDet> ListBarcodesEntrada = new List<PaylessProdPrioriArchDet>();
            try
            {
                if (cboTipo == "0") {
                    IFormFile FileUploaded = Request.Form.Files[0];
                    StringBuilder sb = new StringBuilder();
                    if (FileUploaded.Length > 0) {
                        string FileExtension = Path.GetExtension(FileUploaded.FileName).ToLower();
                        using (MemoryStream stream = new MemoryStream()) {
                            FileUploaded.CopyTo(stream);
                            stream.Position = 0;
                            if (!(FileExtension == ".xml" || FileExtension == ".XML")) {
                                return new RetData<PaylessProdPrioriArchM>() {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo no tiene la extensión .xml",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                            System.Data.DataSet FileUploadDs = new System.Data.DataSet();
                            FileUploadDs.ReadXml(stream);
                            if (FileUploadDs.Tables.Count > 0) {

                                int NTblSalida = 0;
                                for (NTblSalida = 0; NTblSalida < FileUploadDs.Tables.Count; NTblSalida++)
                                    if (FileUploadDs.Tables[NTblSalida].TableName.ToLower() == "casedetail")
                                        break;                                
                                if (NTblSalida == FileUploadDs.Tables.Count)
                                    return new RetData<PaylessProdPrioriArchM>() {
                                        Info = new RetInfo() {
                                            CodError = -1,
                                            Mensaje = "El archivo no es correcto",
                                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                        }
                                    };
                                foreach (System.Data.DataRow FileCod in FileUploadDs.Tables[NTblSalida].Rows) {
                                    if (ListBarcodesSalida.Where(Bc => Bc.Barcode == FileCod["CaseNumber"].ToString()).Count() == 0)
                                        ListBarcodesSalida.Add(new PaylessProdPrioriArchDet() { Barcode = FileCod["CaseNumber"].ToString() });
                                }
                            } else {
                                return new RetData<PaylessProdPrioriArchM>() {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo no es correcto",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                        }
                    }
                    RetData<PaylessProdPrioriArchM> Ret = await ApiClientFactory.Instance.SetPaylessProdPrioriFile(ListBarcodesSalida, null, IdTransporte, CboPeriod, HttpContext.Session.GetObjSession<string>("Session.CodUsr"), cboTipo);
                    return Ret;
                } else if(cboTipo == "1") {
                    IFormFile FileUploadedSalida;
                    IFormFile FileUploadedEntrada;
                    try {
                        FileUploadedSalida = Request.Form.Files[0];
                        FileUploadedEntrada = Request.Form.Files[1];
                        if (FileUploadedSalida == null || FileUploadedEntrada == null)
                            return new RetData<PaylessProdPrioriArchM>() {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "No se enviaron los 2 archivos completos.",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                    } catch (Exception e2) {
                        return new RetData<PaylessProdPrioriArchM>() {
                            Info = new RetInfo() {
                                CodError = -1,
                                Mensaje = e2.ToString(),
                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                            }
                        };
                    }
                    StringBuilder sb = new StringBuilder();
                    if (FileUploadedSalida.Length > 0 && FileUploadedEntrada.Length > 0) {
                        string FileExtension = Path.GetExtension(FileUploadedSalida.FileName).ToLower();
                        string FileExtension2 = Path.GetExtension(FileUploadedEntrada.FileName).ToLower();
                        using (MemoryStream StreamEntrada = new MemoryStream())
                        using (MemoryStream StreamSalida = new MemoryStream()) {
                            FileUploadedSalida.CopyTo(StreamSalida);
                            FileUploadedEntrada.CopyTo(StreamEntrada);
                            StreamSalida.Position = 0;
                            StreamEntrada.Position = 0;
                            if (!(FileExtension == ".xml" || FileExtension == ".XML")) {
                                return new RetData<PaylessProdPrioriArchM>() {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo de salida no tiene la extensión .xml",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                            if (!(FileExtension2 == ".xml" || FileExtension == ".XML")) {
                                return new RetData<PaylessProdPrioriArchM>() {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo de entrada no tiene la extensión .xml",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                            System.Data.DataSet FileUploadDsSalida = new System.Data.DataSet();
                            System.Data.DataSet FileUploadDsEntrada = new System.Data.DataSet();
                            FileUploadDsSalida.ReadXml(StreamSalida);
                            FileUploadDsEntrada.ReadXml(StreamEntrada);
                            if (FileUploadDsSalida.Tables.Count > 0 && FileUploadDsEntrada.Tables.Count > 0) {                                
                                int NTblEntrada = 0, NTblSalida = 0;
                                for (NTblSalida = 0; NTblSalida < FileUploadDsSalida.Tables.Count; NTblSalida++)
                                    if (FileUploadDsSalida.Tables[NTblSalida].TableName.ToLower() == "casedetail")
                                        break;
                                for (NTblEntrada = 0; NTblEntrada < FileUploadDsSalida.Tables.Count; NTblEntrada++)
                                    if (FileUploadDsSalida.Tables[NTblEntrada].TableName.ToLower() == "casedetail")
                                        break;
                                if (NTblSalida == FileUploadDsSalida.Tables.Count)
                                    return new RetData<PaylessProdPrioriArchM>() {
                                        Info = new RetInfo() {
                                            CodError = -1,
                                            Mensaje = "El archivo de salida no es correcto",
                                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                        }
                                    };
                                if (NTblEntrada == FileUploadDsSalida.Tables.Count)
                                    return new RetData<PaylessProdPrioriArchM>() {
                                        Info = new RetInfo() {
                                            CodError = -1,
                                            Mensaje = "El archivo de entrada no es correcto",
                                            ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                        }
                                    };
                                foreach (System.Data.DataRow FileCod in FileUploadDsSalida.Tables[NTblSalida].Rows)
                                    if (ListBarcodesSalida.Where(Bc => Bc.Barcode == FileCod["CaseNumber"].ToString()).Count() == 0)
                                        ListBarcodesSalida.Add(new PaylessProdPrioriArchDet() { Barcode = FileCod["CaseNumber"].ToString() });
                                foreach (System.Data.DataRow FileCod in FileUploadDsEntrada.Tables[NTblEntrada].Rows)
                                    if (ListBarcodesEntrada.Where(Bc => Bc.Barcode == FileCod["CaseNumber"].ToString()).Count() == 0)
                                        ListBarcodesEntrada.Add(new PaylessProdPrioriArchDet() { Barcode = FileCod["CaseNumber"].ToString() });
                            } else {
                                return new RetData<PaylessProdPrioriArchM>() {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo no es correcto",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                        }
                    }
                    RetData<PaylessProdPrioriArchM> Ret = await ApiClientFactory.Instance.SetPaylessProdPrioriFile(ListBarcodesSalida, ListBarcodesEntrada, IdTransporte, dtpPeriodo, HttpContext.Session.GetObjSession<string>("Session.CodUsr"), cboTipo);
                    return Ret;
                }
                return new RetData<PaylessProdPrioriArchM>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = "Error",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
            catch (Exception ex1)
            {
                return new RetData<PaylessProdPrioriArchM>()
                {
                    Info = new RetInfo()
                    {
                        CodError = -1,
                        Mensaje = ex1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<PaylessTiendas>>> GetAllPaylessStores() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PaylessTiendas>> ListClients = await ApiClientFactory.Instance.GetAllPaylessStores(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                return ListClients;
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessTiendas>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<bool>> ChangePedidoState(int PedidoId) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<bool> Res = await ApiClientFactory.Instance.ChangePedidoState(PedidoId, HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (Res.Info.CodError != 0)
                    return new RetData<bool>() {
                        Data = false,
                        Info = Res.Info
                    };
                return new RetData<bool>() {
                    Data = true,
                    Info = Res.Info
                };
            } catch (Exception e1) {
                return new RetData<bool>() {
                    Data = false,
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }            
        }
        public async Task<IActionResult> MakeExcelWms1(string Period, int IdTransport, int Typ) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<WmsFileModel>> ListInfo = await ApiClientFactory.Instance.GetWmsFile(Period, IdTransport, Typ);
                if (ListInfo.Info.CodError != 0)
                    return Json( ListInfo.Info );
                Utility.ExceL ExcelO = new Utility.ExceL();
                string HashId = HttpContext.Session.GetObjSession<string>("Session.HashId") + ".xls";
                string[] ExcelColumns = new string[] {
                    "Identificador", "Fecha", "Recibo de Almacén", "Codigo", "Modelo", "Descripción", "Piezas", "Unidad",
                    "Cantidad", "Código de la Localización", "Peso (Kg)", "Volumen (m³)", "Valor Unitario", "Valor",
                    "Número de Entrada", "Observaciones", "Oden de Compra", "Lote", "Número de Factura", "CLIENTE",
                    "RACKID", "fecha_im5", "EMBALAJE", "UOM", "exportador", "destino", "estilo", "cod_equivale", "pais_orig", "COLOR"
                };
                using (FileStream FilePlantilla = new FileStream("plantillaWms1.xls", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    MemoryStream Ms = new MemoryStream();
                    FilePlantilla.CopyTo(Ms);
                    try {
                        ExcelO.ExcelWorkBook = new HSSFWorkbook(Ms);
                        ExcelO.CurrentSheet = ExcelO.ExcelWorkBook.GetSheetAt(0);
                    } catch (Exception e2) {
                        throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                    }                    
                    ExcelO.CurrentRow = 1;                    
                    foreach (WmsFileModel RowO in ListInfo.Data) {
                        ExcelO.CreateRow();
                        ExcelO.CurrentCol = 0;
                        foreach (string Col in ExcelColumns) {
                            ExcelO.CreateCell(CellType.String);
                            switch (Col) {
                                case "Recibo de Almacén":
                                    ExcelO.SetCellValue("");
                                    break;
                                case "Fecha":
                                    ExcelO.SetCellValue(DateTime.Now.ToString(ApplicationSettings.DateTimeFormat));
                                    break;
                                case "Codigo":
                                    ExcelO.SetCellValue(RowO.Barcode);
                                    break;
                                case "Descripción":
                                    ExcelO.SetCellValue(RowO.Descripcion);
                                    break;
                                case "Piezas":
                                    ExcelO.SetCellValue(RowO.Piezas);
                                    break;
                                case "Unidad":
                                    ExcelO.SetCellValue(RowO.Unidad);
                                    break;
                                case "Cantidad":
                                    ExcelO.SetCellValue(RowO.Cantidad);
                                    break;
                                case "Código de la Localización":
                                    ExcelO.SetCellValue(RowO.CodigoLocalizacion);
                                    break;
                                case "Peso (Kg)":
                                    ExcelO.SetCellValue(Math.Round(RowO.Peso.Value, 2));
                                    break;
                                case "Volumen (m³)":
                                    ExcelO.SetCellValue(RowO.Volumen);
                                    break;
                                case "CLIENTE":
                                    ExcelO.SetCellValue(RowO.Cliente);
                                    break;
                                case "UOM":
                                    ExcelO.SetCellValue(RowO.UOM);
                                    break;
                                case "destino":
                                    ExcelO.SetCellValue(RowO.Destino);
                                    break;
                                case "exportador":
                                    ExcelO.SetCellValue(RowO.Exportador);
                                    break;
                                case "pais_orig":
                                    ExcelO.SetCellValue(RowO.PaisOrigen);
                                    break;
                                case "Observaciones":
                                    ExcelO.SetCellValue(RowO.Cp);
                                    break;
                                case "Modelo":
                                    ExcelO.SetCellValue(RowO.Modelo);
                                    break;
                                case "Lote":
                                    ExcelO.SetCellValue(RowO.Lote);
                                    break;
                                case "estilo":
                                    ExcelO.SetCellValue(RowO.Estilo);
                                    break;
                                case "COLOR":
                                    ExcelO.SetCellValue(string.IsNullOrEmpty(RowO.Transporte) ? "" : RowO.Transporte);
                                    break;
                                case "cod_equivale":
                                    ExcelO.SetCellValue(RowO.CodEquivalente);
                                    break;
                                default:
                                    break;
                            }
                            ExcelO.CurrentCol++;
                        }
                        ExcelO.CurrentRow++;
                    }
                    MemoryStream Ms2 = new MemoryStream();
                    ExcelO.ExcelWorkBook.Write(Ms2);
                    string Transporte = "";
                    if (ListInfo.Data.Count() > 0)
                        Transporte = ListInfo.Data.Fod().Transporte;
                    return File(Ms2.ToArray(), "application/octet-stream", "Archivo_WMS_" + Transporte + "_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
                }
            } catch (Exception e1) {
                return Json(JsonConvert.SerializeObject(e1));
            }            
        }
        public async Task<IActionResult> MakeExcelAutoRep(int IdM, string Typ) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<Tuple<PaylessReportes, IEnumerable<PaylessReportesDet>, IEnumerable<PaylessTiendas>>> ListInfo = await ApiClientFactory.Instance.GetWeekReport(IdM, Typ);                
                if (ListInfo.Info.CodError != 0)
                    return Json(ListInfo.Info);
                if (ListInfo.Data.Item1 == null)
                    return Json("ERROR. No existe información del reporte.");                
                if (ListInfo.Data.Item2 == null)
                    return Json("ERROR. No existe información del detalle del reporte 1.");
                if (ListInfo.Data.Item2.Count() == 0)
                    return Json("ERROR. No existe información del detalle del reporte 2.");
                string PlantillaPre = ListInfo.Data.Item1.Tipo == "0" ? "" : "b";
                string Plantilla = $"plantillaOrdenes3{PlantillaPre}.xls";
                if (ListInfo.Data.Item2.Where(O1 => !string.IsNullOrEmpty(O1.Fecha4)).Count() > 0)
                    Plantilla = $"plantillaOrdenes4{PlantillaPre}.xls";
                if (ListInfo.Data.Item2.Where(O1 => !string.IsNullOrEmpty(O1.Fecha5)).Count() > 0)
                    Plantilla = $"plantillaOrdenes5{PlantillaPre}.xls";
                if (ListInfo.Data.Item2.Where(O1 => !string.IsNullOrEmpty(O1.Fecha6)).Count() > 0)
                    Plantilla = $"plantillaOrdenes6{PlantillaPre}.xls";
                Utility.ExceL ExcelO = new Utility.ExceL();                
                using (FileStream FilePlantilla = new FileStream(Plantilla, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    MemoryStream Ms = new MemoryStream();
                    FilePlantilla.CopyTo(Ms);
                    try {
                        ExcelO.ExcelWorkBook = new HSSFWorkbook(Ms);
                        ExcelO.CurrentSheet = ExcelO.ExcelWorkBook.GetSheetAt(0);
                    } catch (Exception e2) {
                        throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                    }
                    ExcelO.SetRow(1);
                    ExcelO.SetCell(4);
                    ExcelO.SetCellValue(ListInfo.Data.Item1.Periodo);
                    ExcelO.SetRow(2);
                    ExcelO.SetCell(4);
                    ExcelO.SetCellValue(ListInfo.Data.Item1.PeriodoF);
                    IEnumerable<PaylessReportesDet> ListDetOrd = ListInfo.Data.Item2.OrderByDescending(O1 => O1.Fecha1.ToDateFromEspDate());
                    for (int i = 0; i < ListDetOrd.Count(); i++) {
                        ExcelO.CreateRow(i + 4);
                        ExcelO.CreateCell(1, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TiendaId);
                        ExcelO.CreateCell(2, CellType.String);
                        ExcelO.SetCellValue(ListInfo.Data.Item3.Where(T => T.TiendaId == ListDetOrd.ElementAt(i).TiendaId).Fod().Direc);
                        ExcelO.CreateCell(3, CellType.String);
                        ExcelO.SetCellValue(ListInfo.Data.Item3.Where(T => T.TiendaId == ListDetOrd.ElementAt(i).TiendaId).Fod().Lider);
                        ExcelO.CreateCell(4, CellType.String);
                        ExcelO.SetCellValue(ListInfo.Data.Item3.Where(T => T.TiendaId == ListDetOrd.ElementAt(i).TiendaId).Fod().Tel);

                        ExcelO.CreateCell(5, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Total);
                        ExcelO.CreateCell(11, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalAccQty);
                        ExcelO.CreateCell(12, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalKidQty);
                        ExcelO.CreateCell(13, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalManQty);
                        ExcelO.CreateCell(14, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).TotalWomanQty);
                        ExcelO.CreateCell(15, CellType.String);
                        ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Total);
                        if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha1)) {
                            ExcelO.CreateCell(16, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha1.Substring(0, 10));
                            ExcelO.CreateCell(17, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant1);
                            ExcelO.CreateCell(18, CellType.String);
                            if (ListInfo.Data.Item1.Tipo == "0")
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha1.Substring(12));
                        }
                        if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha2)) {
                            ExcelO.CreateCell(19, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha2.Substring(0, 10));
                            ExcelO.CreateCell(20, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant2);
                            ExcelO.CreateCell(21, CellType.String);
                            if (ListInfo.Data.Item1.Tipo == "0")
                                ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha2.Substring(12));
                        }
                        if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha3)) {
                            ExcelO.CreateCell(22, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha3.Substring(0, 10));
                            ExcelO.CreateCell(23, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant3);
                            ExcelO.CreateCell(24, CellType.String);
                            if (ListInfo.Data.Item1.Tipo == "0")
                                ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha3.Substring(12));
                        }
                        if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha4)) {
                            ExcelO.CreateCell(25, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha4.Substring(0, 10));
                            ExcelO.CreateCell(26, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant4);
                            ExcelO.CreateCell(27, CellType.String);
                            if (ListInfo.Data.Item1.Tipo == "0")
                                ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha4.Substring(12));
                        }
                        if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha5)) {
                            ExcelO.CreateCell(28, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha5.Substring(0, 10));
                            ExcelO.CreateCell(29, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant5);
                            ExcelO.CreateCell(30, CellType.String);
                            if (ListInfo.Data.Item1.Tipo == "0")
                                ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha5.Substring(12));
                        }
                        if (!string.IsNullOrEmpty(ListDetOrd.ElementAt(i).Fecha6)) {
                            ExcelO.CreateCell(31, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha6.Substring(0, 10));
                            ExcelO.CreateCell(32, CellType.String);
                            ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Cant6);
                            ExcelO.CreateCell(33, CellType.String);
                            if (ListInfo.Data.Item1.Tipo == "0")
                                ExcelO.SetCellValue(ListDetOrd.ElementAt(i).Fecha6.Substring(12));
                        }
                    }
                    MemoryStream Ms2 = new MemoryStream();
                    ExcelO.ExcelWorkBook.Write(Ms2);
                    string ExcelName = ListInfo.Data.Item1.Tipo == "0" ? "Archivo_RepJuevesPedidos_" : "Archivo_RepDomingoEnvios_";
                    return File(Ms2.ToArray(), "application/octet-stream", ExcelName + DateTime.Now.ToString("ddMMyyyy") + ".xls");
                }
            } catch (Exception e1) {
                return Json(JsonConvert.SerializeObject(e1));
            }
        }
        public async Task<RetData<IEnumerable<IenetGroups>>> GetGroups() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<IenetGroups>> ListGroups = await ApiClientFactory.Instance.GetGroups(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));                
                return ListGroups;
            } catch (Exception e1) {
                return new RetData<IEnumerable<IenetGroups>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<IenetAccesses>>> GetAccess() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<IenetAccesses>> ListAccess = await ApiClientFactory.Instance.GetIenetAccesses(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                return ListAccess;
            } catch (Exception e1) {
                return new RetData<IEnumerable<IenetAccesses>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<string>> SetGroupAccess(int cboGroup, int cboAccess) {
            DateTime StartTime = DateTime.Now;            
            try {
                RetData<string> Ret = await ApiClientFactory.Instance.SetGroupAccess(cboGroup, cboAccess);
                return Ret;
            } catch (Exception e1) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<PaylessPeriodoTransporteModel>>> GetTransportByPeriod(string Period) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PaylessPeriodoTransporteModel>> ListTransport = await ApiClientFactory.Instance.GetTransportByPeriod(Period);
                return ListTransport;
            } catch (Exception e1) {
                return new RetData<IEnumerable<PaylessPeriodoTransporteModel>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<Bodegas>>> GetWmsBodegas(string Period) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<Bodegas>> List = await ApiClientFactory.Instance.GetWmsBodegas(7);
                return List;
            } catch (Exception e1) {
                return new RetData<IEnumerable<Bodegas>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<Regimen>>> GetWmsRegimen(int BodegaId) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<Regimen>> List = await ApiClientFactory.Instance.GetWmsRegimen(BodegaId);
                return List;
            } catch (Exception e1) {
                return new RetData<IEnumerable<Regimen>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<string>> SetIngresoExcelWms(int cboBodega, int cboRegimen) {
            DateTime StartTime = DateTime.Now;
            List<string> ListCols = new List<string>();
            List<WmsFileModel> ListExcelRows = new List<WmsFileModel>();
            int i = 0, j = 0;
            if (cboBodega == 0 || cboRegimen == 0)
                return new RetData<string>() {
                    Data = "",
                    Info = new RetInfo() {
                        CodError = -2,
                        Mensaje = "Bodega y regimen inválidos, son cero.",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            try {
                IFormFile FileUploaded = Request.Form.Files[0];
                if (FileUploaded.Length > 0) {
                    string[] ExcelColumns = new string[] {
                        "Identificador", "Fecha", "Recibo de Almacén", "Codigo", "Modelo", "Descripción", "Piezas", "Unidad",
                        "Cantidad", "Código de la Localización", "Peso (Kg)", "Volumen (m³)", "Valor Unitario", "Valor",
                        "Número de Entrada", "Observaciones", "Oden de Compra", "Lote", "Número de Factura", "CLIENTE",
                        "RACKID", "fecha_im5", "EMBALAJE", "UOM", "exportador", "destino", "estilo", "cod_equivale", "pais_orig", "COLOR"
                    };
                    string FileExtension = Path.GetExtension(FileUploaded.FileName).ToLower();
                    ISheet Sheet, Sheet2 = null;                    
                    using (MemoryStream stream = new MemoryStream()) {
                        FileUploaded.CopyTo(stream);
                        stream.Position = 0;
                        if (FileExtension == ".xls") {
                            try {
                                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                                Sheet = hssfwb.GetSheetAt(0);
                                try {
                                    Sheet2 = hssfwb.GetSheetAt(1);
                                } catch {
                                }
                            } catch (Exception e2) {
                                throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                            }
                        } else if (FileExtension == ".xlsx") {
                            try {
                                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                                Sheet = hssfwb.GetSheetAt(0);
                                try {
                                    Sheet2 = hssfwb.GetSheetAt(1);
                                } catch {
                                }
                            } catch (Exception e3) {
                                throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e3.ToString());
                            }
                        } else {
                            return new RetData<string>() {
                                Data = "",
                                Info = new RetInfo() {
                                    CodError = -2,
                                    Mensaje = "El archivo no tiene la extensión .xls o .xlsx",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                        if (Sheet2 != null) {
                            return new RetData<string>() {
                                Data = "",
                                Info = new RetInfo() {
                                    CodError = -2,
                                    Mensaje = "El archivo contiene más de una hoja, por favor coloque la información en la primera hoja.",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                        IRow HeaderRow = Sheet.GetRow(0);
                        PaylessUploadFileModel NewRow = new PaylessUploadFileModel();
                        int CellCount = 30; //HeaderRow.LastCellNum;                        
                        for (j = 0; j < CellCount; j++) {
                            bool PropExists = false;
                            foreach (string Pi in ExcelColumns) {
                                if (Pi.Trim().ToLower() == ((NPOI.SS.UserModel.ICell)HeaderRow.GetCell(j)).ToString().ToLower().Trim()) {
                                    PropExists = true;
                                    ListCols.Add(Pi.ToLower());
                                }
                            }
                            if (!PropExists) {
                                return new RetData<string>() {
                                    Data = "",
                                    Info = new RetInfo() {
                                        CodError = -2,
                                        Mensaje = "El archivo contiene columnas que no han sido establecidas, nombre de columna que da error: " + ((NPOI.SS.UserModel.ICell)HeaderRow.GetCell(j)).ToString(),
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                        }
                        for (i = (Sheet.FirstRowNum + 1); i <= Sheet.LastRowNum; i++) {
                            IRow row = Sheet.GetRow(i);
                            WmsFileModel NewRowInsert = new WmsFileModel();
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (j = row.FirstCellNum; j < CellCount; j++) {
                                if (row.GetCell(j) != null) {
                                    try {
                                        if (string.IsNullOrEmpty(row.GetCell(j).ToString())) continue;
                                        switch (ListCols[j].ToLower()) {
                                            case "identificador":
                                                NewRowInsert.Identificador = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "fecha":
                                                if (row.GetCell(j).ToString().Trim().Length > 10) {
                                                    NewRowInsert.Fecha = row.GetCell(j).DateCellValue.ToString(ApplicationSettings.DateTimeFormat);
                                                } else if (row.GetCell(j).ToString().Trim().Length == 7) {
                                                    NewRowInsert.Fecha = row.GetCell(j).DateCellValue.ToString(ApplicationSettings.DateTimeFormat);
                                                } else NewRowInsert.Fecha = row.GetCell(j).ToString().Trim();
                                                break;
                                            case "recibo de almacén":
                                                NewRowInsert.ReciboAlmacen = row.GetCell(j).ToString().Trim();
                                                break;
                                            case "codigo":
                                                NewRowInsert.Barcode = row.GetCell(j).ToString();
                                                break;
                                            case "modelo":
                                                NewRowInsert.Modelo = row.GetCell(j).ToString();
                                                break;
                                            case "descripción":
                                                NewRowInsert.Descripcion = row.GetCell(j).ToString();
                                                break;
                                            case "piezas":
                                                NewRowInsert.Piezas = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "unidad":
                                                NewRowInsert.Unidad = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "cantidad":
                                                if (string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                                    NewRowInsert.Cantidad = 1;
                                                else
                                                    NewRowInsert.Cantidad = Convert.ToInt32(Convert.ToDouble(row.GetCell(j).ToString()));
                                                break;
                                            case "código de la localización":
                                                NewRowInsert.CodigoLocalizacion = row.GetCell(j).ToString();
                                                break;
                                            case "peso (kg)":
                                                NewRowInsert.Peso = Convert.ToDouble(row.GetCell(j).ToString());
                                                break;
                                            case "volumen (m³)":
                                                NewRowInsert.Volumen = Convert.ToDouble(row.GetCell(j).ToString());
                                                break;
                                            case "valor unitario":
                                                NewRowInsert.ValorUnitario = Convert.ToDouble(row.GetCell(j).ToString());
                                                break;
                                            case "valor":
                                                NewRowInsert.Valor = Convert.ToDouble(row.GetCell(j).ToString());
                                                break;
                                            case "número de entrada":
                                                NewRowInsert.NumeroEntrada = Convert.ToDouble(row.GetCell(j).ToString());
                                                break;
                                            case "observaciones":
                                                NewRowInsert.Observaciones = row.GetCell(j).ToString();
                                                break;
                                            case "oden de compra":
                                                NewRowInsert.OrdenDeCompra = row.GetCell(j).ToString();
                                                break;
                                            case "lote":
                                                NewRowInsert.Lote = row.GetCell(j).ToString();
                                                break;
                                            case "número de factura":
                                                NewRowInsert.NumeroFactura = row.GetCell(j).ToString();
                                                break;
                                            case "cliente":
                                                NewRowInsert.Cliente = row.GetCell(j).ToString();
                                                break;
                                            case "rackid":
                                                NewRowInsert.RackId = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "fecha_im5":
                                                NewRowInsert.FechaIm5 = row.GetCell(j).ToString();
                                                break;
                                            case "embalaje":
                                                if (string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                                    NewRowInsert.Embalaje = 136;
                                                else
                                                    NewRowInsert.Embalaje = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "uom":
                                                if (string.IsNullOrEmpty(row.GetCell(j).ToString()))
                                                    NewRowInsert.UOM = 346;
                                                else
                                                    NewRowInsert.UOM = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "exportador":
                                                NewRowInsert.Exportador = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "destino":
                                                NewRowInsert.Destino = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "estilo":
                                                NewRowInsert.Estilo = row.GetCell(j).ToString();
                                                break;
                                            case "cod_equivale":
                                                NewRowInsert.CodEquivalente = row.GetCell(j).ToString();
                                                break;
                                            case "pais_orig":
                                                NewRowInsert.PaisOrigen = Convert.ToInt32(row.GetCell(j).ToString());
                                                break;
                                            case "color":
                                                NewRowInsert.Color = row.GetCell(j).ToString();
                                                break;
                                            default:
                                                break;
                                        }
                                    } catch (Exception ec1) {
                                        return new RetData<string>() {
                                            Data = "",
                                            Info = new RetInfo() {
                                                CodError = -2,
                                                Mensaje = $"Error en conversión de tipo para el campo {ListCols[j]} {ec1.ToString()} en la fila {i}",
                                                ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                            }
                                        };
                                    }
                                }
                            }
                            ListExcelRows.Add(NewRowInsert);
                        }
                    }
                }
                RetData<string> Ret = await ApiLongClientFactory.Instance.SetIngresoExcelWms2(ListExcelRows, cboBodega, cboRegimen, HttpContext.Session.GetObjSession<string>("Session.CodUsr"));
                return Ret;
            } catch (Exception ex1) {
                return new RetData<string>() {
                    Data = "",
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = ex1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public IActionResult TransformEscanerToWmsIn() {
            DateTime StartTime = DateTime.Now;
            List<string> ListBarcodes = new List<string>();
            try {
                IFormFile FileUploaded = Request.Form.Files[0];
                StringBuilder sb = new StringBuilder();
                if (FileUploaded.Length > 0) {
                    string FileExtension = Path.GetExtension(FileUploaded.FileName).ToLower();
                    using (MemoryStream stream = new MemoryStream()) {
                        FileUploaded.CopyTo(stream);
                        stream.Position = 0;
                        if (!(FileExtension == ".xml" || FileExtension == ".XML")) {
                            return Json(new {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "El archivo no tiene la extensión .xml",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            });
                        }
                        System.Data.DataSet FileUploadDs = new System.Data.DataSet();
                        FileUploadDs.ReadXml(stream);
                        if (FileUploadDs.Tables.Count > 3 && FileUploadDs.Tables[1].Columns[0].ColumnName == "OriginType") {
                            if (FileUploadDs.Tables[3].TableName == "CaseDetail") {
                                foreach (System.Data.DataRow FileCod in FileUploadDs.Tables[3].Rows) {
                                    if (ListBarcodes.Where(Bc => Bc == FileCod["CaseNumber"].ToString()).Count() == 0)
                                        ListBarcodes.Add(FileCod["CaseNumber"].ToString());
                                }
                            } else {
                                return Json(new {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo no es correcto",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                });
                            }
                        } else {
                            return Json(new {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "El archivo no es correcto",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            });
                        }
                    }
                }
                Utility.ExceL ExcelO = new Utility.ExceL();
                using (FileStream FilePlantilla = new FileStream("plantillaWms2.xls", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    MemoryStream Ms = new MemoryStream();
                    FilePlantilla.CopyTo(Ms);
                    try {
                        ExcelO.ExcelWorkBook = new HSSFWorkbook(Ms);
                        ExcelO.CurrentSheet = ExcelO.ExcelWorkBook.GetSheetAt(0);
                    } catch (Exception e2) {
                        throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                    }                    
                    ExcelO.CurrentRow = 1;
                    foreach (string Code1 in ListBarcodes) {
                        ExcelO.CreateRow();
                        ExcelO.CurrentCol = 0;
                        ExcelO.CreateCell(CellType.Numeric);
                        ExcelO.SetCellValue(Code1);
                        ExcelO.CurrentCol = 1;
                        ExcelO.CreateCell(CellType.Numeric);
                        ExcelO.SetCellValue(1);
                        ExcelO.CurrentCol = 4;
                        ExcelO.CreateCell(CellType.Numeric);
                        ExcelO.SetCellValue(1);
                        ExcelO.CurrentRow++;
                    }
                    MemoryStream Ms2 = new MemoryStream();
                    ExcelO.ExcelWorkBook.Write(Ms2);
                    return File(Ms2.ToArray(), "application/octet-stream", "ArchivoSalida_WMS_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
                }
            } catch (Exception ex1) {
                return Json(new {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = ex1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                });
            }
        }
        public async Task<RetData<string>> SetSalidaWmsFromEscaner(string dtpPeriodo, int cboBodegas, int cboRegimen) {
            DateTime StartTime = DateTime.Now;
            List<string> ListBarcodes = new List<string>();
            try {
                IFormFile FileUploaded = Request.Form.Files[0];
                StringBuilder sb = new StringBuilder();
                if (FileUploaded.Length > 0) {
                    string FileExtension = Path.GetExtension(FileUploaded.FileName).ToLower();
                    using (MemoryStream stream = new MemoryStream()) {
                        FileUploaded.CopyTo(stream);
                        stream.Position = 0;
                        if (!(FileExtension == ".xml" || FileExtension == ".XML")) {
                            return new RetData<string> { 
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "El archivo no tiene la extensión .xml",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                        System.Data.DataSet FileUploadDs = new System.Data.DataSet();
                        FileUploadDs.ReadXml(stream);
                        if (FileUploadDs.Tables.Count > 3 && FileUploadDs.Tables[1].Columns[0].ColumnName == "OriginType") {
                            if (FileUploadDs.Tables[3].TableName == "CaseDetail") {
                                foreach (System.Data.DataRow FileCod in FileUploadDs.Tables[3].Rows) {
                                    if (ListBarcodes.Where(Bc => Bc == FileCod["CaseNumber"].ToString()).Count() == 0)
                                        ListBarcodes.Add(FileCod["CaseNumber"].ToString());
                                }
                            } else {
                                return new RetData<string> {
                                    Info = new RetInfo() {
                                        CodError = -1,
                                        Mensaje = "El archivo no es correcto",
                                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                    }
                                };
                            }
                        } else {
                            return new RetData<string> {
                                Info = new RetInfo() {
                                    CodError = -1,
                                    Mensaje = "El archivo no es correcto",
                                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                                }
                            };
                        }
                    }
                }
                RetData<string> Ret = await ApiLongClientFactory.Instance.SetSalidaWmsFromEscaner(ListBarcodes, dtpPeriodo, cboBodegas, cboRegimen);
                return Ret;
            } catch (Exception ex1) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = ex1.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<PaylessTiendas>>> GetStores() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PaylessTiendas>> ListStores = await ApiClientFactory.Instance.GetStores();
                return ListStores;
            } catch (Exception e2) {
                return new RetData<IEnumerable<PaylessTiendas>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<AsyncStates>>> GetAsyncState0() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<AsyncStates>> List = await ApiClientFactory.Instance.GetAsyncState(0, HttpContext.Session.GetObjSession<string>("Session.CodUsr"));
                return List;
            } catch (Exception e2) {
                return new RetData<IEnumerable<AsyncStates>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public bool SetChangeDis(int PedidoId) {
            HttpContext.Session.SetObjSession("PedidoIdToModify", PedidoId);
            return true;
        }
        public async Task<RetData<string>> ChangeUserClient(int IdUser, int ClienteId) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<string> List = await ApiClientFactory.Instance.ChangeUserClient(IdUser, ClienteId);
                return List;
            } catch (Exception e2) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<string>> ChangeUserTienda(int IdUser, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<string> List = await ApiClientFactory.Instance.ChangeUserTienda(IdUser, TiendaId);
                if (IdUser == 1)
                    HttpContext.Session.SetObjSession("Session.TiendaId", TiendaId);
                return List;
            } catch (Exception e2) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetPedidosWmsByStore(int ClienteId, int TiendaId) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PedidosWmsModel>> ListDis = await ApiClientFactory.Instance.GetPedidosMWmsByTienda(ClienteId, TiendaId);
                return ListDis;
            } catch (Exception e2) {
                return new RetData<IEnumerable<PedidosWmsModel>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }        
        public async Task<RetData<string>> ChangePedidoExternoIdWMS(int PedidoId, int PedidoIdWms) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<string> List = await ApiClientFactory.Instance.ChangePedidoExternoIdWMS(PedidoId, PedidoIdWms);
                return List;
            } catch (Exception e2) {
                return new RetData<string> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<PedidosWmsModel>>> GetWmsDetDispatchsBills() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PedidosWmsModel>> ListDis = await ApiClientFactory.Instance.GetWmsDetDispatchsBills(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                return ListDis;
            } catch (Exception e2) {
                return new RetData<IEnumerable<PedidosWmsModel>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<IActionResult> MakeWmsDetDispatchsBills() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PedidosWmsModel>> ListInfo = await ApiClientFactory.Instance.GetWmsDetDispatchsBills(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListInfo.Info.CodError != 0)
                    return Json(ListInfo.Info);
                Utility.ExceL ExcelO = new Utility.ExceL();                
                using (FileStream FilePlantilla = new FileStream("plantillaWms3.xls", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    MemoryStream Ms = new MemoryStream();
                    FilePlantilla.CopyTo(Ms);
                    try {
                        ExcelO.ExcelWorkBook = new HSSFWorkbook(Ms);
                        ExcelO.CurrentSheet = ExcelO.ExcelWorkBook.GetSheetAt(0);
                    } catch (Exception e2) {
                        throw new Exception("El archivo no es de Excel. Utilice un formato propio de Microsoft Excel. " + e2.ToString());
                    }                    
                    ExcelO.CurrentRow = 1;
                    foreach (PedidosWmsModel RowO in ListInfo.Data) {
                        try {
                            ExcelO.CreateRow();
                            for (int Z = 0; Z < 10; Z++) {
                                ExcelO.CurrentCol = Z;
                                ExcelO.CreateCell(CellType.String);
                                switch (Z) {
                                    case 0:
                                        ExcelO.SetCellValue(RowO.PedidoBarcode);
                                        break;
                                    case 1:
                                        ExcelO.SetCellValue(RowO.TransaccionId);
                                        break;
                                    case 2:
                                        ExcelO.SetCellValue(RowO.Destino);
                                        break;
                                    case 3:
                                        ExcelO.SetCellValue(RowO.FechaPedido);
                                        break;
                                    case 4:
                                        ExcelO.SetCellValue(RowO.Estatus);
                                        break;
                                    case 5:
                                        ExcelO.SetCellValue(RowO.NomBodega);
                                        break;
                                    case 6:
                                        ExcelO.SetCellValue(RowO.Regimen);
                                        break;
                                    case 7:
                                        ExcelO.SetCellValue(string.IsNullOrEmpty(RowO.Observacion)? "" : RowO.Observacion);
                                        break;
                                    case 8:
                                        ExcelO.SetCellValue(RowO.CodProducto);
                                        break;
                                    case 9:
                                        ExcelO.SetCellValue(RowO.FactComercial);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            ExcelO.CurrentRow++;
                        } catch (Exception e2) {
                            throw e2;
                        }                        
                    }
                    MemoryStream Ms2 = new MemoryStream();
                    ExcelO.ExcelWorkBook.Write(Ms2);
                    return File(Ms2.ToArray(), "application/octet-stream", "PedidosFacturas_WMS_" + DateTime.Now.ToString("ddMMyyyy") + ".xls");
                }
            } catch (Exception e1) {
                return Json(JsonConvert.SerializeObject(e1));
            }
        }
        public async Task<RetData<Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>>> GetProductoTallaLoteCategoria() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>> ListDis = await ApiClientFactory.Instance.GetProductoTallaLoteCategoria();
                return ListDis;
            } catch (Exception e2) {
                return new RetData<Tuple<IEnumerable<int>, IEnumerable<string>, IEnumerable<int>, IEnumerable<string>>> { 
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<IEnumerable<CboValuesModel>>> GetPaylessEncuestaCboPedidos() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<CboValuesModel>> List = await ApiClientFactory.Instance.GetPaylessEncuestaCboPedidos(HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                return List;
            } catch (Exception e2) {
                return new RetData<IEnumerable<CboValuesModel>>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<Tuple<IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>>>> GetFilterTemporada() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PaylessProdPrioriDet>> ListFilter = await ApiClientFactory.Instance.GetFilterTemporada();
                IEnumerable<string> ListProducts = ListFilter.Data.Select(D1 => D1.Producto).Distinct().Where(O2 => !string.IsNullOrEmpty(O2)).OrderBy(O1 => O1);
                IEnumerable<string> ListTalla = ListFilter.Data.Select(D1 => D1.Talla).Distinct().Where(O2 => !string.IsNullOrEmpty(O2)).OrderBy(O1 => O1);
                IEnumerable<string> ListLote = ListFilter.Data.Select(D1 => D1.Lote).Distinct().Where(O2 => !string.IsNullOrEmpty(O2)).OrderBy(O1 => O1);
                IEnumerable<string> ListCategoria = ListFilter.Data.Select(D1 => D1.Categoria).Distinct().Where(O2 => !string.IsNullOrEmpty(O2)).OrderBy(O1 => O1);
                IEnumerable<string> ListDepartamento = ListFilter.Data.Select(D1 => D1.Departamento).Distinct().Where(O2 => !string.IsNullOrEmpty(O2)).OrderBy(O1 => O1);
                IEnumerable<string> ListCp = ListFilter.Data.Select(D1 => D1.Cp).Distinct().Where(O2 => !string.IsNullOrEmpty(O2)).OrderBy(O1 => O1);
                return new RetData<Tuple<IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>>> {
                    Data = new Tuple<IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>>(
                        ListProducts, ListTalla, ListLote, ListCategoria, ListDepartamento, ListCp),
                    Info = new RetInfo() {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            } catch (Exception e2) {
                return new RetData<Tuple<IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>, IEnumerable<string>>> {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<string>> PaylessAddTemporada(string CboProducto, string CboTalla, string CboLote, string CboCategoria, string CboDepartamento, string CboCp) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<string> Ret = await ApiClientFactory.Instance.PaylessAddTemporada(CboProducto, CboTalla, CboLote, CboCategoria, CboDepartamento, CboCp);
                return Ret;                
            } catch (Exception e2) {
                return new RetData<string>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
        public async Task<RetData<string>> PaylessDeleteTemporada(int Id) {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<string> Ret = await ApiClientFactory.Instance.PaylessDeleteTemporada(Id);
                return Ret;
            } catch (Exception e2) {
                return new RetData<string>() {
                    Info = new RetInfo() {
                        CodError = -1,
                        Mensaje = e2.ToString(),
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
            }
        }
    }
}