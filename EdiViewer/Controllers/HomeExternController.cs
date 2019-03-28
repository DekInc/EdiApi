﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using ComModels;
using EdiViewer.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Pedidos()
        {
            RetData<IEnumerable<PedidosWmsModel>> ListPe = await ApiClientFactory.Instance.GetPedidosWms(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
            return View(ListPe.Data);
        }
        public async Task<IActionResult> Inventario()
        {
            try
            {
                ViewBag.ListOldDis = null;
                ViewBag.DateLastDis = null;
                ViewBag.PedidoId = null;
                HttpContext.Session.SetObjSession("PedidoId", null);
                RetData<Clientes> ClienteO = await ApiClientFactory.Instance.GetClient(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListDis = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListDis.Data.Item1.Count() > 0)
                {
                    if (ListDis.Data.Item1.Fod().IdEstado == 1)
                    {
                        ViewBag.DateLastDis = ListDis.Data.Item1.Fod().FechaPedido;
                        HttpContext.Session.SetObjSession("PedidoId", ListDis.Data.Item1.Fod().Id);
                        if (ListDis.Data.Item2.Count() > 0)
                        {
                            ViewBag.ListOldDis = JsonConvert.SerializeObject(ListDis.Data.Item2.Where(Pde => Pde.PedidoId == ListDis.Data.Item1.Fod().Id).Select(Pd => new { codProducto = Pd.CodProducto.Replace(" ", "^"), cantPedir = Pd.CantPedir }));
                        }
                    }
                }
                ViewBag.ClientName = ClienteO.Data.Nombre;
                if (!ClienteO.Data.EstatusId.HasValue) { 
                    HttpContext.Session.SetObjSession("Session.IdPedidoExterno", 0);
                } else {
                    if (ClienteO.Data.EstatusId.Value != 0)
                        HttpContext.Session.SetObjSession("Session.IdPedidoExterno", ClienteO.Data.EstatusId.Value);
                    else
                        HttpContext.Session.SetObjSession("Session.IdPedidoExterno", 0);
                }
            }
            catch (Exception e1)
            {
                ViewBag.ClientName = e1.ToString();
            }            
            return View();
        }        
        public async Task<IActionResult> GetInventory()
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
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = StockData.Info.Mensaje, ListTo = "" });
                if (StockData.Data.Count() == 0)
                {
                    return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = (StockData.Info.CodError != 0? StockData.Info.Mensaje : string.Empty) });
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    StockData.Data = StockData.Data.AsQueryable().OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //total number of rows count
                recordsTotal = StockData.Data.Count();
                //Paging
                StockData.Data = StockData.Data.Skip(skip).Take(pageSize);
                //Returning Json Data
                return Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = StockData.Data, errorMessage = "" });
            }
            catch (Exception e1)
            {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), ListTo = "" });
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
    }
}