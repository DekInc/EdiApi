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
        public async Task<IActionResult> GetPaylessProdPrioriAdmin(string dtpPeriodoBuscar, int cboTransporte) {
            try {
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";
                if (string.IsNullOrEmpty(dtpPeriodoBuscar))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Where(O2 => O2.IdTransporte == cboTransporte).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())) .ToList();                
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
        public async Task<IActionResult> GetPaylessProdPriori(string dtpPeriodoBuscar, int cboTransporte) {
            try {
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";
                if (string.IsNullOrEmpty(dtpPeriodoBuscar))
                    return Json(new { total = 0, records = "", errorMessage = "" });
                HttpContext.Session.SetObjSession("dtpPeriodoBuscar", dtpPeriodoBuscar);
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });                
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Where(O2 => O2.IdTransporte == cboTransporte).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();                
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.IdTransporte, Pp.Tienda, Pp.Barcode, Pp.Categoria, Pp.Cp }
                    into Grp
                    orderby Grp.Fod().Barcode
                    select new PaylessProdPrioriDetModel {
                        Id = Grp.Fod().Id,
                        IdTransporte = Grp.Fod().IdTransporte,
                        Transporte = Grp.Fod().Transporte,
                        Barcode = Grp.Fod().Barcode,
                        Categoria = Grp.Fod().Categoria,
                        Cp = Grp.Fod().Cp,
                        //Pri = Grp.Fod().Tienda,
                        Producto = Grp.Fod().Producto,
                        //Talla = Grp.Fod().Talla,                        
                        //Departamento = Grp.Fod().Departamento,
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
                            Records[j].Reservado += Convert.ToInt32(Ped.CantPedir);
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
                        Records[i].Reservado += Convert.ToInt32(Ped.CantPedir);
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
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Count() == 0)
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
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Where(R => R.Tienda == IdTienda).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                foreach (PaylessProdPrioriDetModel R in Records) {
                    if (R.Categoria.Contains("ACCESORIOS", StringComparison.InvariantCultureIgnoreCase)) {
                        R.Talla = string.Empty;
                        R.Departamento = string.Empty;
                    }                    
                }                
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Barcode, Pp.Categoria, Pp.Talla, Pp.Departamento, Pp.Cp, Pp.Transporte }
                    //group Pp by new { Pp.Barcode, Pp.Producto, Pp.Talla, Pp.Categoria, Pp.Lote, Pp.Departamento, Pp.Cp, Pp.M3, Pp.Peso }
                    into Grp
                    select new PaylessProdPrioriDetModel {
                        Barcode = Grp.Fod().Barcode,
                        //Producto = Grp.Fod().Producto,
                        Talla = Grp.Fod().Talla,
                        Categoria = Grp.Fod().Categoria,
                        //Lote = Grp.Fod().Lote,
                        //Estado = Grp.Fod().Estado,
                        //Pri = Grp.Fod().Pri,
                        //PoolP = Grp.Fod().PoolP,
                        Departamento = Grp.Fod().Departamento,
                        Cp = Grp.Fod().Cp,
                        Id = Grp.Fod().Id,
                        Peso = Grp.Count(),
                        IdTransporte = Grp.Fod().IdTransporte,
                        Transporte = Grp.Fod().Transporte
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
                                    }
                                    if (AllRecords[i].Reservado == 0) {
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
                    Records.ForEach(R => {
                        if (R.CantPedir < 0)
                            R.CantPedir = 0;                        
                    });
                    FilteredRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);                    
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords, FilteredRecords, pedidosPendientes = TuplePextSent.Data.Item1 });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<IActionResult> GetPaylessProdPrioriDet(string barcode) {
            try {
                string dtpPeriodoBuscar = HttpContext.Session.GetObjSession<string>("dtpPeriodoBuscar");
                if (string.IsNullOrEmpty(dtpPeriodoBuscar)) dtpPeriodoBuscar = "";                
                if (string.IsNullOrEmpty(barcode))
                    return Json(new { total = 0, records = "", errorMessage = "" });                
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProd = await ApiClientFactory.Instance.GetPaylessProdPriori(dtpPeriodoBuscar);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
                Records = Records.Where(Pp => Pp.Barcode == barcode).ToList();
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
                if (ListProdPrioriArch.Data.Item1.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriArchMModel> Records = (
                    from D in ListProdPrioriArch.Data.Item1
                    orderby D.Periodo.ToDateFromEspDate()
                    select D
                    ).ToList();                    
                //ListProdPrioriArch.Data.Item1.OrderByDescending(O => O.Periodo.ToDateFromEspDate()).ToList();
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
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProdPrioriArch = await ApiLongClientFactory.Instance.GetPaylessFileDif(idProdArch, idData);
                if (ListProdPrioriArch.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProdPrioriArch.Info.Mensaje });
                if (ListProdPrioriArch.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });
                if (ListProdPrioriArch.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProdPrioriArch.Info.CodError != 0 ? ListProdPrioriArch.Info.Mensaje : string.Empty) });                
                List<PaylessProdPrioriDetModel> Records = ListProdPrioriArch.Data.ToList();
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
                int Total = Records.Count;
                if (Total > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPeticiones() {
            try {                
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternosByTienda(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                if (ListPe.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPe.Info.Mensaje });
                if (ListPe.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                if (ListPe.Data.Item2.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                List<PedidosExternosGModel> Records = ListPe.Data.Item1.Select(O => Utility.Funcs.Reflect(O, new PedidosExternosGModel())).ToList();
                Records.ForEach(R => {
                    if ((DateTime.Now - R.FechaPedido.ToDateFromEspDate()).TotalHours < 24)
                        R.ChangeState = true;
                });
                List<PedidosExternosGModel> AllRecords = new List<PedidosExternosGModel>();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                }
                List<PedidosDetExternos> ListCant = new List<PedidosDetExternos>();
                foreach (PedidosExternosGModel R in Records) {
                    List<PedidosDetExternos> ListDet = ListPe.Data.Item2.Where(O3 => O3.PedidoId == R.Id).ToList();
                    foreach (PedidosDetExternos Pde in ListDet) {
                        if (ListCant.Where(Lc => Lc.PedidoId == Pde.PedidoId).Count() == 0)
                            ListCant.Add(new PedidosDetExternos() { Id = 1, PedidoId = Pde.PedidoId });
                        else {
                            for (int i = 0; i < ListCant.Count; i++) {
                                if (ListCant[i].PedidoId == Pde.PedidoId) {
                                    ListCant[i].Id++;
                                }
                            }
                        }
                    }                   
                }
                for (int i = 0; i < Records.Count; i++) {
                    Records[i].Cont = ListCant.Where(O => O.PedidoId == Records[i].Id).Fod().Id;
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPeticionesAdminB(string Dtp1 = "", string ChkPending = "") {
            try {
                RetData<IEnumerable<PeticionesAdminBGModel>> ListPe = await ApiClientFactory.Instance.GetPeticionesAdminB();
                if (ListPe.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPe.Info.Mensaje });
                if (ListPe.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                if (ListPe.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListPe.Info.CodError != 0 ? ListPe.Info.Mensaje : string.Empty) });
                List<PeticionesAdminBGModel> Records = new List<PeticionesAdminBGModel>();
                foreach (PeticionesAdminBGModel Pe in ListPe.Data) {
                    Pe.Total = Pe.WomanQty + Pe.ManQty + Pe.KidQty + Pe.AccQty + Pe.TotalCp;
                    Pe.TotalEnv = Pe.WomanQtyEnv + Pe.ManQtyEnv + Pe.KidQtyEnv + Pe.AccQtyEnv + Pe.TotalCpEnv;
                    if (Pe.TotalEnv != 0)
                        Pe.PorcValid = Math.Round((1.0 - (double)(Math.Abs((double)Pe.Total - (double)Pe.TotalEnv)/(double)Pe.Total)) * 100.0, 2);
                    Records.Add(Pe);
                }
                if (!string.IsNullOrEmpty(Dtp1)) {
                    Records = Records.Where(Pe => Pe.FechaPedido.StartsWith(Dtp1)).ToList();
                }
                if (!string.IsNullOrEmpty(ChkPending))
                    if (ChkPending.ToLower() == "true")
                        Records = Records.Where(Pe => Pe.IdEstado == 2).ToList();
                Records = Records.OrderByDescending(O => O.FechaPedido.ToDate()).ToList();
                List<PeticionesAdminBGModel> AllRecords = new List<PeticionesAdminBGModel>();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    bool HaveForm = true;
                    try {
                        if (Request.Form != null) {
                            IFormCollection GridForm = Request.Form;
                        }
                    } catch {
                        HaveForm = false;
                    }
                    if (HaveForm) {
                        AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                        Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                    }
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
                RetData<IEnumerable<PaylessProdPrioriDetModel>> RecordsAux = await ApiClientFactory.Instance.GetPedidosExternosDet(PedidoId);
                if (RecordsAux.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = RecordsAux.Info.Mensaje });
                if (RecordsAux.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (RecordsAux.Info.CodError != 0 ? RecordsAux.Info.Mensaje : string.Empty) });
                if (RecordsAux.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (RecordsAux.Info.CodError != 0 ? RecordsAux.Info.Mensaje : string.Empty) });
                List<PaylessProdPrioriDetModel> Records = RecordsAux.Data.ToList();
                List<PaylessProdPrioriDetModel> AllRecords = new List<PaylessProdPrioriDetModel>();
                int NRow = 0;
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Barcode, Pp.Cp }
                    into Grp
                    orderby Grp.Fod().Cp descending, Grp.Fod().Barcode
                    select new PaylessProdPrioriDetModel {
                        Barcode = Grp.Fod().Barcode,
                        //Producto = Grp.Fod().Producto,
                        //Talla = Grp.Fod().Talla,
                        //Categoria = Grp.Fod().Categoria,
                        //Lote = Grp.Fod().Lote,
                        //Estado = Grp.Fod().Estado,
                        //Pri = Grp.Fod().Pri,
                        //PoolP = Grp.Fod().PoolP,
                        //Departamento = Grp.Fod().Departamento,
                        Cp = Grp.Fod().Cp,
                        Id = NRow++,
                        CantPedir = 1
                        //Peso = Grp.Count(),
                        //IdTransporte = Grp.Fod().IdTransporte,
                        //Transporte = Grp.Fod().Transporte
                    }).Distinct().ToList();
                int Total = RecordsAux.Data.Count();
                if (RecordsAux.Data.Count() > 0) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }
        }
        public async Task<IActionResult> GetPaylessReportes() {
            DateTime StartTime = DateTime.Now;
            try {
                IEnumerable<DateTime> ListMondays = Utility.Funcs.AllDatesInMonth(DateTime.Now.Year, DateTime.Now.Month).Where(i => i.DayOfWeek == DayOfWeek.Monday);
                RetData<IEnumerable<PaylessReportes>> ListPaylessReportes = await ApiClientFactory.Instance.GetPaylessReportes();
                if (ListPaylessReportes.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPaylessReportes.Info.Mensaje });
                if (ListPaylessReportes.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListPaylessReportes.Info.CodError != 0 ? ListPaylessReportes.Info.Mensaje : string.Empty) });                
                List<PaylessReportesGModel> Records = ListPaylessReportes.Data.Select(O => Utility.Funcs.Reflect(O, new PaylessReportesGModel())).ToList();                
                List<PaylessReportesGModel> AllRecords = new List<PaylessReportesGModel>();
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
        public async Task<IActionResult> GetAllPeriods() {
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<string>> ListPeriods = await ApiClientFactory.Instance.GetPaylessPeriodPriori();
                if (ListPeriods.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPeriods.Info.Mensaje });
                if (ListPeriods.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListPeriods.Info.CodError != 0 ? ListPeriods.Info.Mensaje : string.Empty) });
                int Cont = 1;
                List<PeriodModel> Records = ListPeriods.Data.Select(P => new PeriodModel() { Periodo = P, recid = Cont++ }).OrderBy(P2 => P2.Periodo.ToDateFromEspDate()).ToList();
                List<PeriodModel> AllRecords = new List<PeriodModel>();
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
        public async Task<IActionResult> GetPaylessProdPrioriInventario2() {
            try {
                string TiendaId = HttpContext.Session.GetObjSession<string>("Session.TiendaId");
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProd = await ApiLongClientFactory.Instance.GetPaylessProdPrioriAll(TiendaId);
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });                
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Where(R => R.Tienda == TiendaId).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();                
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Barcode, Pp.Categoria, Pp.Cp }
                    into Grp
                    select new PaylessProdPrioriDetModel {
                        Barcode = Grp.Fod().Barcode,
                        //Producto = Grp.Fod().Producto,
                        //Talla = Grp.Fod().Talla,
                        Categoria = Grp.Fod().Categoria,
                        //Lote = Grp.Fod().Lote,
                        //Estado = Grp.Fod().Estado,
                        //Pri = Grp.Fod().Pri,
                        //PoolP = Grp.Fod().PoolP,
                        //Departamento = Grp.Fod().Departamento,
                        Cp = string.IsNullOrEmpty(Grp.Fod().Cp)? "No" : "Si",
                        Id = Grp.Fod().Id
                        //Peso = Grp.Count(),
                        //IdTransporte = Grp.Fod().IdTransporte,
                        //Transporte = Grp.Fod().Transporte
                    }
                    ).ToList();
                List<PaylessProdPrioriDetModel> AllRecords = Records;
                List<PaylessProdPrioriDetModel> ListProdWithStock = new List<PaylessProdPrioriDetModel>();
                List<PaylessProdPrioriDetModel> FilteredRecords = Records;
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> TuplePextSent = await ApiClientFactory.Instance.GetPedidosExternosPendientesByTienda(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                if (TuplePextSent.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = TuplePextSent.Info.Mensaje });
                List<PedidosDetExternos> ListPendientes = (
                    from Pd in TuplePextSent.Data.Item2
                    from P in TuplePextSent.Data.Item1
                    where Pd.PedidoId == P.Id
                    select Pd
                    ).ToList();
                int Total = Records.Count;
                List<string> ListBodegas = new List<string>();
                string CodProductoFuera = string.Empty;
                if (Records.Count() > 0) {
                    RetData<IEnumerable<FE830DataAux>> StockData = await ApiLongClientFactory.Instance.GetStockByTienda(HttpContext.Session.GetObjSession<int>("Session.ClientId"), HttpContext.Session.GetObjSession<int>("Session.TiendaId"));
                    if (StockData.Info.CodError != 0)
                        return Json(new { total = 0, records = "", errorMessage = StockData.Info.Mensaje });
                    foreach (FE830DataAux Stock in StockData.Data) {
                        foreach (PaylessProdPrioriDetModel Product in Records.Where(P => P.Barcode == Stock.CodProducto)) {
                            if (!ListBodegas.Contains(Stock.CodProductoLear)) {
                                ListBodegas.Add(Stock.CodProductoLear);
                                if (ListBodegas.Count > 1)
                                    CodProductoFuera = Product.Barcode;
                            }
                            Product.Existencia = Convert.ToInt32(Stock.Existencia);
                            if (ListProdWithStock.Where(Ws => Ws.Barcode == Product.Barcode).Count() == 0 
                                && Product.Existencia > 0
                                && ListPendientes.Where(Lp => Lp.CodProducto == Product.Barcode).Count() == 0) {
                                ListProdWithStock.Add(Product);
                            }
                        }
                    }
                    if (ListBodegas.Count == 2) {
                        return Json(new { total = 0, records = "", errorMessage = $"Hay un problema con los productos a pedir, el código de producto {CodProductoFuera} está en una bodega diferente." });
                    }                    
                    Records.ForEach(R => {
                        if (R.CantPedir < 0)
                            R.CantPedir = 0;
                    });
                    bool HaveForm = true;
                    try {
                        if (Request.Form != null) {
                            IFormCollection GridForm = Request.Form;
                        }
                    } catch {
                        HaveForm = false;
                    }                    
                    int NRow = 0;
                    Records = (
                        from R in ListProdWithStock
                        group R by new { R.Categoria, R.Cp }
                        into Grp
                        select new PaylessProdPrioriDetModel {
                            Id = NRow++,
                            Categoria = Grp.Fod().Categoria,
                            Existencia = Grp.Sum(O1 => O1.Existencia),
                            Reservado = Grp.Sum(O1 => O1.Reservado),
                            CantPedir = Grp.Sum(O1 => O1.CantPedir),
                            Cp = Grp.Fod().Cp
                        }).ToList();
                    Total = Records.Count;
                    FilteredRecords = Records;
                    if (HaveForm) {
                        FilteredRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                        Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                    }
                }                
                AllRecords = Records;
                if (TuplePextSent.Data.Item1.Count() > 0)
                    return Json(new { Total, Records, errorMessage = "", AllRecords, FilteredRecords, pedidosPendientes = TuplePextSent.Data.Item1 });
                else
                    return Json(new { Total, Records, errorMessage = "", AllRecords, FilteredRecords });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<IActionResult> GetInvByStore() {
            try {
                string ClienteId = HttpContext.Session.GetObjSession<string>("Session.ClientId");
                RetData<IEnumerable<PaylessProdPrioriDetModel>> ListProd = await ApiLongClientFactory.Instance.GetPaylessProdPrioriAll();
                RetData<IEnumerable<PaylessTiendas>> ListStores2 = await ApiClientFactory.Instance.GetStores();                
                if (ListProd.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListProd.Info.Mensaje });
                if (ListProd.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListProd.Data.Count() == 0)
                    return Json(new { total = 0, records = "", errorMessage = (ListProd.Info.CodError != 0 ? ListProd.Info.Mensaje : string.Empty) });
                if (ListStores2.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListStores2.Info.Mensaje });
                if (ListStores2.Data == null)
                    return Json(new { total = 0, records = "", errorMessage = (ListStores2.Info.CodError != 0 ? ListStores2.Info.Mensaje : string.Empty) });
                List<PaylessTiendas> ListStores = ListStores2.Data.Where(S => S.ClienteId == Convert.ToInt32(ClienteId)).ToList();
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                List<StoreCatQtyGModel> GroupRecords = new List<StoreCatQtyGModel>();
                string[] ListCpReal = { "A", "H" };
                Records = (
                    from Pp in Records
                    group Pp by new { Pp.Tienda, Pp.Barcode, Pp.Categoria, Pp.Cp }
                    into Grp
                    select new PaylessProdPrioriDetModel {                        
                        Barcode = Grp.Fod().Barcode,
                        Categoria = Grp.Fod().Categoria,
                        Cp = ListCpReal.Contains(Grp.Fod().Cp.Trim().ToUpper())? "1" : string.Empty,
                        Id = Grp.Fod().Id,
                        Lote = Grp.Fod().Tienda
                    }).ToList();
                List<StoreCatQtyGModel> AllRecords = new List<StoreCatQtyGModel>();
                List<PaylessProdPrioriDetModel> ListProdWithStock = new List<PaylessProdPrioriDetModel>();
                List<StoreCatQtyGModel> FilteredRecords = new List<StoreCatQtyGModel>();
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>, IEnumerable<Clientes>>> TupleListPendientes = await ApiClientFactory.Instance.GetPedidosExternosPendientesAdmin();
                if (TupleListPendientes.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = TupleListPendientes.Info.Mensaje });
                List<PedidosDetExternos> ListPendientes = (
                    from Pd in TupleListPendientes.Data.Item2
                    from P in TupleListPendientes.Data.Item1
                    where Pd.PedidoId == P.Id
                    select Pd
                    ).ToList();
                int Total = Records.Count;
                List<string> ListBodegas = new List<string>();
                string CodProductoFuera = string.Empty;
                if (Records.Count() > 0) {
                    RetData<IEnumerable<FE830DataAux>> StockData = await ApiLongClientFactory.Instance.GetStockByCliente(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                    if (StockData.Info.CodError != 0)
                        return Json(new { total = 0, records = "", errorMessage = StockData.Info.Mensaje });
                    foreach (FE830DataAux Stock in StockData.Data) {
                        foreach (PaylessProdPrioriDetModel Product in Records.Where(P => P.Barcode == Stock.CodProducto)) {
                            if (!ListBodegas.Contains(Stock.CodProductoLear)) {
                                ListBodegas.Add(Stock.CodProductoLear);
                                if (ListBodegas.Count > 1)
                                    CodProductoFuera = Product.Barcode;
                            }
                            Product.Existencia = Convert.ToInt32(Stock.Existencia);
                            if (Product.Existencia > 1) Product.Existencia = 1;
                            if (ListProdWithStock.Where(Ws => Ws.Barcode == Product.Barcode).Count() == 0
                                && Product.Existencia > 0) {
                                ListProdWithStock.Add(Product);
                            }
                        }
                    }                    
                    Records.ForEach(R => {
                        if (R.CantPedir < 0)
                            R.CantPedir = 0;
                    });
                    bool HaveForm = true;
                    try {
                        if (Request.Form != null) {
                            IFormCollection GridForm = Request.Form;
                        }
                    } catch {
                        HaveForm = false;
                    }
                    int NRow = 0;
                    Records = (
                        from R in ListProdWithStock
                        group R by new { R.Lote, R.Categoria, R.Cp }
                        into Grp
                        select new PaylessProdPrioriDetModel {
                            Id = NRow++,
                            Categoria = Grp.Fod().Categoria,
                            Lote = Grp.Fod().Lote,
                            Existencia = Grp.Sum(O1 => O1.Existencia > 2? 1 : O1.Existencia),
                            Reservado = Grp.Sum(O1 => O1.Reservado),
                            CantPedir = Grp.Sum(O1 => O1.CantPedir),
                            Cp = Grp.Fod().Cp
                        }).ToList();
                    NRow = 0;
                    StoreCatQtyGModel TotalRow = new StoreCatQtyGModel() { TiendaId = null, Tienda = "TOTAL" };
                    string[] ArraTiendas = Records.Select(O => O.Lote).Distinct().ToArray();
                    for (int J = 0; J < Records.Count; J++) {
                        if (GroupRecords.Where(Gr => Gr.TiendaId == Convert.ToInt32(Records[J].Lote)).Count() == 0) {
                            GroupRecords.Add(new StoreCatQtyGModel {
                                TiendaId = Convert.ToInt32(Records[J].Lote),
                                Tienda = ListStores.Where(Ls => Ls.TiendaId == Convert.ToInt32(Records[J].Lote)).Fod().Descr
                            });
                            foreach (PaylessProdPrioriDetModel R in Records.Where(R2 => R2.Lote == Records[J].Lote)) {
                                switch (R.Categoria.ToUpper().Trim()) {
                                    case "DAMAS":
                                        if (string.IsNullOrEmpty(R.Cp)) {
                                            GroupRecords.LastOrDefault().WomanQty = R.Existencia;
                                            TotalRow.WomanQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalSCp += R.Existencia;
                                        } else {
                                            GroupRecords.LastOrDefault().WomanCpQty = R.Existencia;
                                            TotalRow.WomanCpQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalCp += R.Existencia;
                                        }                                        
                                        break;
                                    case "CABALLEROS":
                                        if (string.IsNullOrEmpty(R.Cp)) {
                                            GroupRecords.LastOrDefault().ManQty = R.Existencia;
                                            TotalRow.ManQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalSCp += R.Existencia;
                                        } else {
                                            GroupRecords.LastOrDefault().ManCpQty = R.Existencia;
                                            TotalRow.ManCpQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalCp += R.Existencia;
                                        }
                                        break;
                                    case "NIÑOS / AS":
                                        if (string.IsNullOrEmpty(R.Cp)) {
                                            GroupRecords.LastOrDefault().KidsQty = R.Existencia;
                                            TotalRow.KidsQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalSCp += R.Existencia;
                                        } else {
                                            GroupRecords.LastOrDefault().KidsCpQty = R.Existencia;
                                            TotalRow.KidsCpQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalCp += R.Existencia;
                                        }
                                        break;
                                    case "ACCESORIOS":
                                        if (string.IsNullOrEmpty(R.Cp)) {
                                            GroupRecords.LastOrDefault().AccQty = R.Existencia;
                                            TotalRow.AccQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalSCp += R.Existencia;
                                        } else {
                                            GroupRecords.LastOrDefault().AccCpQty = R.Existencia;
                                            TotalRow.AccCpQty += R.Existencia;
                                            GroupRecords.LastOrDefault().TotalCp += R.Existencia;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    if (GroupRecords.Select(Gr => Gr.TiendaId).Distinct().Count() != ListStores.Count()) {
                        List<PaylessTiendas> ListStoresExcept = new List<PaylessTiendas>();
                        ListStores.ToList().ForEach(S => {
                            if (GroupRecords.Where(Gr2 => Gr2.TiendaId == S.TiendaId).Count() == 0)
                                ListStoresExcept.Add(S);
                        });
                        ListStoresExcept.ForEach(Se => {
                            GroupRecords.Add(new StoreCatQtyGModel() {
                                TiendaId = Se.TiendaId ?? 0,
                                Tienda = Se.Descr
                            });
                        });
                    }
                    GroupRecords.ForEach(Ge => {
                        Ge.Total = Ge.TotalSCp + Ge.TotalCp;

                        if (Ge.TiendaId != null) {
                            //string[] Lista1 = ListPendientes.Where(P2 => P2.CodProducto.Substring(0, 4).Equals(Ge.TiendaId)).Select(P => P.CodProducto).Distinct().ToArray();
                            Ge.Requested = ListPendientes.Where(P2 => P2.CodProducto.Substring(0, 4) == Ge.TiendaId.ToString()).Select(P => P.CodProducto).Distinct().Count();
                        }
                        Ge.Available = Ge.Total - Ge.Requested;
                        if (Ge.Available < 0)
                            Ge.Available = Ge.Total;
                    });
                    TotalRow.TotalSCp = TotalRow.WomanQty + TotalRow.ManQty + TotalRow.KidsQty + TotalRow.AccQty;
                    TotalRow.TotalCp = TotalRow.WomanCpQty + TotalRow.ManCpQty + TotalRow.KidsCpQty + TotalRow.AccCpQty;
                    TotalRow.Total = TotalRow.TotalSCp + TotalRow.TotalCp;
                    TotalRow.Requested = ListPendientes.Select(P => P.CodProducto).Distinct().Count();
                    TotalRow.Available = TotalRow.Total - TotalRow.Requested;
                    GroupRecords.Add(TotalRow);
                    Total = GroupRecords.Count;
                    FilteredRecords = GroupRecords;
                    AllRecords = GroupRecords;
                    if (HaveForm) {
                        FilteredRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<StoreCatQtyGModel>(GroupRecords, Request.Form);
                        GroupRecords = Utility.ExpressionBuilderHelper.W2uiSearch<StoreCatQtyGModel>(GroupRecords, Request.Form);
                    }
                }                
                if (TupleListPendientes.Data.Item1.Count() > 0)
                    return Json(new { Total, Records = GroupRecords, errorMessage = "", AllRecords, FilteredRecords, pedidosPendientes = TupleListPendientes.Data.Item1 });
                else
                    return Json(new { Total, Records = GroupRecords, errorMessage = "", AllRecords, FilteredRecords });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
        public async Task<IActionResult> GetPeticionesAdmin(int ClienteId, string Dtp1 = "", string ChkPending = "") {
            if (ClienteId == 0)
                return Json(new { total = 0, records = "", errorMessage = "" });
            DateTime StartTime = DateTime.Now;
            try {
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternosMDetAdmin(ClienteId);
                if (ListPe.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListPe.Info.Mensaje });
                if (ListPe.Data.Item1.Count() == 0) {
                    return Json(new { total = 0, records = "", errorMessage = ListPe.Info.Mensaje });
                }
                List<PedidosExternosGModel> Records = ListPe.Data.Item1.Where(Pe => Pe.IdEstado != 1).Select(O => Utility.Funcs.Reflect(O, new PedidosExternosGModel())).ToList();
                if (!string.IsNullOrEmpty(Dtp1)) {
                    Records = Records.Where(Pe => Pe.FechaPedido.StartsWith(Dtp1)).ToList();
                }
                if (!string.IsNullOrEmpty(ChkPending))
                    if (ChkPending.ToLower() == "true")
                        Records = Records.Where(Pe => Pe.IdEstado == 2).ToList();
                List<PedidosExternosGModel> AllRecords = new List<PedidosExternosGModel>();
                int Total = Records.Count;
                bool HaveForm = true;
                try {
                    if (Request.Form != null) {
                        IFormCollection GridForm = Request.Form;
                    }
                } catch {
                    HaveForm = false;
                }
                if (Records.Count() > 0 && HaveForm) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                }
                Records.ForEach(R => {
                    R.Cont = ListPe.Data.Item2.Where(Ped => Ped.PedidoId == R.Id).Count();
                });
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }

        }
        public async Task<IActionResult> GetPedidosWmsByStore(int ClienteId, int TiendaId) {
            if (ClienteId == 0 || TiendaId == 0)
                return Json(new { total = 0, records = "", errorMessage = "" });
            DateTime StartTime = DateTime.Now;
            try {
                RetData<IEnumerable<PedidosWmsModel>> ListDis = await ApiClientFactory.Instance.GetPedidosMWmsByTienda(ClienteId, TiendaId);
                if (ListDis.Info.CodError != 0)
                    return Json(new { total = 0, records = "", errorMessage = ListDis.Info.Mensaje });
                if (ListDis.Data.Count() == 0) {
                    return Json(new { total = 0, records = "", errorMessage = "" });
                }
                List<PedidosWmsModel> Records = ListDis.Data.ToList();                
                List<PedidosWmsModel> AllRecords = new List<PedidosWmsModel>();
                int Total = Records.Count;
                bool HaveForm = true;
                try {
                    if (Request.Form != null) {
                        IFormCollection GridForm = Request.Form;
                    }
                } catch {
                    HaveForm = false;
                }
                if (Records.Count() > 0 && HaveForm) {
                    AllRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch(Records, Request.Form);
                }
                return Json(new { Total, Records, errorMessage = "", AllRecords });
            } catch (Exception e1) {
                return Json(new { total = 0, records = "", errorMessage = e1.ToString() });
            }

        }
    }
}