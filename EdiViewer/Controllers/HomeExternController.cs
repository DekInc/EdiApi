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
        public IActionResult Pedidos()
        {
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
        public IActionResult CargaProdPriori2()
        {
            return View();
        }
        public string ImportarExcel()
        {
            return "";
        }
        public RetData<string> OnPostImport()
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
                            HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                            Sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                        }
                        else if (FileExtension == ".xlsx")
                        {
                            XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                            Sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
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
                        IRow HeaderRow = Sheet.GetRow(0); //Get Header Row
                        PaylessUploadFileModel NewRow = new PaylessUploadFileModel();
                        int CellCount = HeaderRow.LastCellNum;
                        sb.Append("<table class='table'><tr>");
                        for (int j = 0; j < CellCount; j++)
                        {
                            bool PropExists = false;
                            foreach (PropertyInfo Pi in NewRow.GetType().GetProperties())
                            {
                                if (Pi.Name.ToLower() == ((NPOI.SS.UserModel.ICell)HeaderRow.GetCell(j)).ToString().ToLower())
                                {
                                    PropExists = true;
                                    ListCols.Add(Pi.Name);
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
                            //NPOI.SS.UserModel.ICell cell = HeaderRow.GetCell(j);
                            //if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                            //sb.Append("<th>" + cell.ToString() + "</th>");
                        }
                        //sb.Append("</tr>");
                        //sb.AppendLine("<tr>");
                        for (int i = (Sheet.FirstRowNum + 1); i <= Sheet.LastRowNum; i++) //Read Excel File
                        {
                            IRow row = Sheet.GetRow(i);
                            PaylessUploadFileModel NewRowInsert = new PaylessUploadFileModel();
                            if (row == null) continue;
                            if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                            for (int j = row.FirstCellNum; j < CellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    //ListCols[j]
                                    NewRowInsert.GetType().GetProperty(ListCols[j]).SetValue(NewRowInsert, row.GetCell(j).ToString());
                                    //sb.Append("<td>" + row.GetCell(j).ToString() + "</td>");
                                }
                            }
                            ListExcelRows.Add(NewRowInsert);
                            //sb.AppendLine("</tr>");
                        }                        
                        //sb.Append("</table>");
                    }
                }
                return new RetData<string>()
                {
                    Data = "",
                    Info = new RetInfo()
                    {
                        CodError = 0,
                        Mensaje = "ok",
                        ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                    }
                };
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
                RetData<IEnumerable<PedidosWmsModel>> ListPe = await ApiClientFactory.Instance.GetPedidosWms(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = ListPe.Info.Mensaje, data = "" });
                if (ListPe.Data.Count() == 0)
                {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty), data = "" });
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn == "fechaPedido")
                    {
                        if (sortColumnDirection == "desc")
                            ListPe.Data = ListPe.Data.OrderByDescending(O => O.FechaPedido.ToDateFromEspDate());
                        else
                            ListPe.Data = ListPe.Data.OrderBy(O => O.FechaPedido.ToDateFromEspDate());
                    }
                    else
                        ListPe.Data = ListPe.Data.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
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
        public async Task<IActionResult> GetPeticiones()
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
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = ListPe.Info.Mensaje, data = "" });
                if (ListPe.Data.Item1.Count() == 0)
                {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty), data = "" });
                }
                IEnumerable<PedidosExternos> ListPed = ListPe.Data.Item1;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn == "fechaPedido")
                    {
                        if (sortColumnDirection == "desc")
                            ListPed = ListPed.OrderByDescending(O => O.FechaPedido.ToDateFromEspDate());
                        else
                            ListPed = ListPed.OrderBy(O => O.FechaPedido.ToDateFromEspDate());
                    }
                    else
                        ListPed = ListPed.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = ListPed.Count();
                //Paging
                ListPed = ListPed.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = ListPed, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }        
        public async Task<IActionResult> GetPeticionDet(int PedidoId)
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
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = ListPe.Info.Mensaje, data = "" });
                if (ListPe.Data.Item2.Count() == 0)
                {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty), data = "" });
                }
                IEnumerable<PedidosDetExternos> ListPed = ListPe.Data.Item2.Where(Pd => Pd.PedidoId == PedidoId);
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    ListPed = ListPed.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = ListPed.Count();
                //Paging
                ListPed = ListPed.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = ListPed, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<RetData<string>> GetClientName()
        {            
            DateTime StartTime = DateTime.Now;
            try
            {
                RetData<Clientes> ClienteO = await ApiClientFactory.Instance.GetClient(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                string ClientName = string.Empty;
                if (ClienteO.Info.CodError == 0)
                    ClientName = ClienteO.Data.Nombre;
                return new RetData<string>()
                {
                    Data = ClientName,
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
        public async Task<IActionResult> Inventario()
        {
            try
            {
                ViewBag.ListOldDis = null;
                ViewBag.DateLastDis = DateTime.Now.ToString(ApplicationSettings.DateTimeFormatT);
                ViewBag.PedidoId = null;
                HttpContext.Session.SetObjSession("PedidoId", null);
                //RetData<Clientes> ClienteO = await ApiClientFactory.Instance.GetClient(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListDis = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
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
        [HttpPost]
        public async Task<RetInfo> SetPedidoExterno([FromBody]string Json)
        {
            return await SetPedidoExterno2(Json, 1);
        }
        [HttpPost]
        public async Task<RetInfo> SendPedidoExterno([FromBody]string Json)
        {
            return await SetPedidoExterno2(Json, 2);            
        }
        private async Task<RetInfo> SetPedidoExterno2(string Json, int IdEstado)
        {
            DateTime StartTime = DateTime.Now;
            IEnumerable<PedidoExternoModel> ListDis = JsonConvert.DeserializeObject<IEnumerable<PedidoExternoModel>>(Json.ToString());
            if (ListDis.Count() == 0)
            {
                return new RetInfo()
                {
                    CodError = -1,
                    Mensaje = "No hay productos en la lista",
                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                };
            }
            foreach (PedidoExternoModel Pe in ListDis)
            {
                Pe.id = HttpContext.Session.GetObjSession<int?>("PedidoId");
                Pe.codProducto = Pe.codProducto.Replace("^", " ");
            }
            try
            {
                RetData<PedidosExternos> RetDataO = await ApiClientFactory.Instance.SetPedidoExterno(ListDis, HttpContext.Session.GetObjSession<int>("Session.ClientId"), IdEstado);
                return RetDataO.Info;
            }
            catch (Exception e2)
            {
                return new RetInfo()
                {
                    CodError = -1,
                    Mensaje = e2.ToString(),
                    ResponseTimeSeconds = (DateTime.Now - StartTime).TotalSeconds
                };
            }
        }
        public async Task<IActionResult> GetPaylessProdPriori2()
        {
            try
            {
                RetData<IEnumerable<PaylessProdPriori>> ListProdPriori = await ApiClientFactory.Instance.GetPaylessProdPriori("08/04/2019");
                if (ListProdPriori.Info.CodError != 0)
                    return Json(new { errorMessage = ListProdPriori.Info.Mensaje, data = "", });
                if (ListProdPriori.Data.Count() == 0)
                {
                    return Json(new { total = ListProdPriori.Data.Count(), errorMessage = (ListProdPriori.Info.CodError != 0 ? ListProdPriori.Info.Mensaje : string.Empty), records = "" });
                }                                
                return Json(new { total = ListProdPriori.Data.Count(), records = ListProdPriori.Data, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { total = "", errorMessage = e1.ToString(), records = "", listAllProd = "" });
            }
        }
        public async Task<IActionResult> GetPaylessProdPriori()
        {
            try
            {
                RetData<IEnumerable<PaylessProdPriori>> ListProdPriori = await ApiClientFactory.Instance.GetPaylessProdPriori("08/04/2019");
                if (ListProdPriori.Info.CodError != 0)
                    return Json(new { errorMessage = ListProdPriori.Info.Mensaje, data = "", });
                if (ListProdPriori.Data.Count() == 0)
                {
                    return Json(new { errorMessage = (ListProdPriori.Info.CodError != 0 ? ListProdPriori.Info.Mensaje : string.Empty), data = "" });
                }
                return Json(new { data = ListProdPriori.Data, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { errorMessage = e1.ToString(), data = "" });
            }
        }
    }
}