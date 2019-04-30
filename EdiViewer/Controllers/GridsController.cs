﻿using System;
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
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
                int Total = Records.Count;
                if (Records.Count() > 0)
                {                    
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
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
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
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
                if (Records.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        private void DispatchAddReserved(ref List<PedidosDetExternos> List, ref List<PaylessProdPrioriDetModel> AllRecords, ref List<PaylessProdPrioriDetModel> Records) {
            if (List.Count() > 0) {
                for (int Pi = 0; Pi < List.Count(); Pi++) {
                    PedidosDetExternos Ped = List[Pi];
                    for (int j = 0; j < AllRecords.Count; j++) {
                        if (AllRecords[j].Barcode == Ped.CodProducto)
                            AllRecords[j].Reservado += Convert.ToInt32(Ped.CantPedir);
                    }
                    for (int j = 0; j < Records.Count; j++) {
                        if (Records[j].Barcode == Ped.CodProducto)
                            Records[j].Reservado = Convert.ToInt32(Ped.CantPedir);
                    }
                }
            }

        }
        private void DispatchAddReservedAr(ref List<PedidosDetExternos> List, ref List<PaylessProdPrioriDetModel> AllRecords, int i) {
            if (List.Count() > 0) {
                for (int Pi = 0; Pi < List.Count(); Pi++) {
                    PedidosDetExternos Ped = List[Pi];
                    if (AllRecords[i].Barcode == Ped.CodProducto)
                        AllRecords[i].Reservado += Convert.ToInt32(Ped.CantPedir);
                }
            }
        }
        private void DispatchAddReservedR(ref List<PedidosDetExternos> List, ref List<PaylessProdPrioriDetModel> Records, int i) {
            if (List.Count() > 0) {
                for (int Pi = 0; Pi < List.Count(); Pi++) {
                    PedidosDetExternos Ped = List[Pi];                    
                    if (Records[i].Barcode == Ped.CodProducto)
                        Records[i].Reservado = Convert.ToInt32(Ped.CantPedir);
                }
            }
        }
        private void SetExistenciaWirhArch(ref RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> ListArch, ref List<PaylessProdPrioriDetModel> AllRecords, int i) {
            if (ListArch.Data != null) {
                if (ListArch.Data.Item2.Count() > 0) {
                    string BarCode = AllRecords[i].Barcode;
                    IEnumerable<PaylessProdPrioriArchDet> ExistenciaInterna = ListArch.Data.Item2.Where(D => D.Barcode == BarCode);
                    if (ExistenciaInterna != null) {
                        if (ExistenciaInterna.Count() > 0) {
                            AllRecords[i].Existencia = 1;
                        }
                    }
                }
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
                if (ListClients.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListClients.Info.CodError != 0 ? ListClients.Info.Mensaje : string.Empty) });
                if (ListClients.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListClients.Info.CodError != 0 ? ListClients.Info.Mensaje : string.Empty) });
                if (ListClients.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListClients.Info.Mensaje });
                //IEnumerable<PaylessProdPrioriDetModel> AllRecords = ListProd.Data.Item2.Distinct().Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                string IdTienda = ListClients.Data.Where(C => C.ClienteId == HttpContext.Session.GetObjSession<int>("Session.ClientId")).Fod().TiendaId.ToString();
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Item2.Where(R => R.Tienda == IdTienda).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Barcode, Pp.Producto, Pp.Talla, Pp.Categoria, Pp.Lote, Pp.Departamento, Pp.Cp, Pp.M3, Pp.Peso }
                    into Grp
                    select new PaylessProdPrioriDetModel {
                        Barcode = Grp.Fod().Barcode,
                        Producto = Grp.Fod().Producto,
                        Talla = Grp.Fod().Talla,
                        Categoria = Grp.Fod().Categoria,
                        Lote = Grp.Fod().Lote,
                        Estado = Grp.Fod().Estado,
                        Pri = Grp.Fod().Pri,
                        PoolP = Grp.Fod().PoolP,
                        Departamento = Grp.Fod().Departamento,
                        Cp = Grp.Fod().Cp,
                        Id = Grp.Fod().Id,
                        Peso = Grp.Count()
                    }
                    ).ToList();
                List<PaylessProdPrioriDetModel> AllRecords = Records;
                List<PaylessProdPrioriDetModel> FilteredRecords = Records;
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> TuplePext = await ApiClientFactory.Instance.GetPedidosExternosGuardados(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> TuplePextSent = await ApiClientFactory.Instance.GetPedidosExternosPendientes(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (TuplePext.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = TuplePext.Info.Mensaje });
                if (TuplePextSent.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = TuplePextSent.Info.Mensaje });
                List<PedidosDetExternos> ListGuardados = (
                    from Pd in TuplePext.Data.Item2
                    from P in TuplePext.Data.Item1
                    where Pd.PedidoId == P.Id
                    && P.Periodo == dtpPeriodoBuscar
                    select Pd
                    ).ToList();
                List<PedidosDetExternos> ListPendientes = (
                    from Pd in TuplePextSent.Data.Item2
                    from P in TuplePextSent.Data.Item1
                    where Pd.PedidoId == P.Id
                    && P.Periodo == dtpPeriodoBuscar
                    select Pd
                    ).ToList();                
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    RetData<IEnumerable<ExistenciasExternModel>> StockData = await ApiClientFactory.Instance.GetStock(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                    RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> ListArch = await ApiClientFactory.Instance.GetPaylessPeriodPrioriFileExists(dtpPeriodoBuscar, HttpContext.Session.GetObjSession<int>("Session.ClientId"));

                    //foreach (ExistenciasExternModel Stock in StockData.Data) {
                    //    foreach (PaylessProdPrioriDetModel Product in Records.Where(P => P.Barcode == Stock.CodProducto)) {
                    //        Product.Existencia = Convert.ToInt32(Stock.Existencia);
                    //        Product.Reservado = Convert.ToInt32(Stock.Reservado);
                    //    }
                    //    foreach (PaylessProdPrioriDetModel Are in AllRecords.Where(Ar => Ar.Barcode == Stock.CodProducto)) {
                    //        Are.Existencia = Convert.ToInt32(Stock.Existencia);
                    //        Are.Reservado = Convert.ToInt32(Stock.Reservado);
                    //    }
                    //}                    
                    if (StockData.Info.CodError != 0)
                        return Json(new { total = 0, records = "", errorMessage = StockData.Info.Mensaje });
                    if (ListArch.Info.CodError != 0)
                        return Json(new { total = 0, records = "", errorMessage = ListArch.Info.Mensaje });
                    if (StockData.Data != null) {
                        if (StockData.Data.Count() > 0) {
                            for (int i = 0; i < AllRecords.Count; i++) {
                                IEnumerable<ExistenciasExternModel> ExistenciaWms = StockData.Data.Where(Sd => Sd.CodProducto == AllRecords[i].Barcode);
                                if (ExistenciaWms != null) {
                                    if (ExistenciaWms.Count() > 0) {
                                        AllRecords[i].Existencia = Convert.ToInt32(ExistenciaWms.Fod().Existencia);
                                        AllRecords[i].Reservado = Convert.ToInt32(ExistenciaWms.Fod().Reservado);
                                    } else {
                                        SetExistenciaWirhArch(ref ListArch, ref AllRecords, i);
                                        DispatchAddReservedAr(ref ListGuardados, ref AllRecords, i);
                                        DispatchAddReservedR(ref ListPendientes, ref Records, i);
                                    }
                                } else {
                                    SetExistenciaWirhArch(ref ListArch, ref AllRecords, i);
                                    DispatchAddReservedAr(ref ListGuardados, ref AllRecords, i);
                                    DispatchAddReservedR(ref ListPendientes, ref Records, i);
                                }
                            }
                        } else {
                            for (int i = 0; i < AllRecords.Count; i++)
                                SetExistenciaWirhArch(ref ListArch, ref AllRecords, i);
                            DispatchAddReserved(ref ListGuardados, ref AllRecords, ref Records);
                            DispatchAddReserved(ref ListPendientes, ref AllRecords, ref Records);
                        }
                    } else {
                        for (int i = 0; i < AllRecords.Count; i++)
                            SetExistenciaWirhArch(ref ListArch, ref AllRecords, i);
                        DispatchAddReserved(ref ListGuardados, ref AllRecords, ref Records);
                        DispatchAddReserved(ref ListPendientes, ref AllRecords, ref Records);
                    }                    
                    Records.ForEach(R => {
                        if (!string.IsNullOrEmpty(R.Cp) && R.Disponible > 0) {
                            R.CantPedir = R.Disponible;
                        }
                    });
                    AllRecords.ForEach(R => {
                        if (!string.IsNullOrEmpty(R.Cp) && R.Disponible > 0) {                            
                            R.CantPedir = R.Disponible;
                        }
                    });
                    if (ListGuardados.Count() > 0) {
                        for (int Pi = 0; Pi < ListGuardados.Count(); Pi++) {
                            PedidosDetExternos Ped = ListGuardados[Pi];
                            for (int j = 0; j < AllRecords.Count; j++) {
                                if (AllRecords[j].Barcode == Ped.CodProducto) {
                                    AllRecords[j].CantPedir = Convert.ToInt32(Ped.CantPedir);
                                }
                            }
                            for (int j = 0; j < Records.Count; j++) {
                                if (Records[j].Barcode == Ped.CodProducto) {
                                    Records[j].CantPedir = Convert.ToInt32(Ped.CantPedir);
                                }
                            }
                        }
                    }
                    FilteredRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords, FilteredRecords, pedidosPendientes = TuplePextSent.Data.Item1 });
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
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
                Records = Records.Where(Pp => Pp.Barcode == barcode).ToList();
                Records = Records.Where(Pp => Pp.Talla == talla).ToList();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
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
                List<PaylessProdPrioriArchMModel> AllRecords = new List<PaylessProdPrioriArchMModel>();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
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
                List<PaylessProdPrioriDet> AllRecords = new List<PaylessProdPrioriDet>();
                int Total = 0;
                switch (idData) {
                    case 1:
                        Total = Item1.Count;
                        if (Total > 0) {
                            AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Item1, Request.Form);
                            Item1 = Utility.ExpressionBuilderHelper.W2uiSearch(Item1, Request.Form);
                        }
                        break;
                    case 2:
                        Total = Item2.Count;
                        if (Total > 0) {
                            AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Item2, Request.Form);
                            Item2 = Utility.ExpressionBuilderHelper.W2uiSearch(Item2, Request.Form);
                        }
                        break;
                    case 3:
                        Total = Item3.Count;
                        if (Total > 0) {
                            AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Item3, Request.Form);
                            Item3 = Utility.ExpressionBuilderHelper.W2uiSearch(Item3, Request.Form);
                        }
                        break;
                    default:
                        break;
                }
                if (idData == 1) return Json(new { Total, Records = Item1, errorMessage = "", AllRecords });
                if (idData == 2) return Json(new { Total, Records = Item2, errorMessage = "", AllRecords });
                if (idData == 3) return Json(new { Total, Records = Item3, errorMessage = "", AllRecords });
                return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPeticiones() {
            try {                
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPe.Info.Mensaje });
                if (ListPe.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                if (ListPe.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                List<PedidosExternosGModel> Records = ListPe.Data.Item1.Select(O => Utility.Funcs.Reflect(O, new PedidosExternosGModel())).ToList();
                List<PedidosExternosGModel> AllRecords = new List<PedidosExternosGModel>();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPeticionDet(int PedidoId) {
            try {
                if (PedidoId == 0)
                    return Json(new { total = 0, records = "", errorMessage = "" });                

                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPe.Info.Mensaje });
                if (ListPe.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                if (ListPe.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                PedidosExternos Pe = ListPe.Data.Item1.Where(I1 => I1.Id == PedidoId).Fod();
                RetData<Tuple<IEnumerable<PaylessProdPrioriM>, IEnumerable<PaylessProdPrioriDet>>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(Pe.Periodo);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Item1.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = (
                    from D in ListProd.Data.Item2
                    from De in ListPe.Data.Item2
                    where D.Barcode == De.CodProducto
                    && De.PedidoId == Pe.Id
                    && D.IdPaylessProdPrioriM == ListProd.Data.Item1.Fod().Id
                    select D
                    ).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                foreach (PaylessProdPrioriDetModel Pr in Records) {
                    Pr.CantPedir = Convert.ToInt32(ListPe.Data.Item2.Where(O => O.CodProducto == Pr.Barcode).Fod().CantPedir);
                }
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
    }
}