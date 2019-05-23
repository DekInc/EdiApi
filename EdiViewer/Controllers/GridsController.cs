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
                RetData<Tuple<IEnumerable<PedidosExternos>, IEnumerable<PedidosDetExternos>>> ListPe = await ApiClientFactory.Instance.GetPedidosExternos(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
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
                //RetData<IEnumerable<PaylessTiendas>> ListClients = await ApiLongClientFactory.Instance.GetAllPaylessStores(ApiClientFactory.Instance.Encrypt($"Fun|{HttpContext.Session.GetObjSession<string>("Session.HashId")}"));
                //if (ListClients.Data == null)
                //    return Json(new { total = 0, records = "", errorMessage = (ListClients.Info.CodError != 0 ? ListClients.Info.Mensaje : string.Empty) });
                //if (ListClients.Data.Count() == 0)
                //    return Json(new { total = 0, records = "", errorMessage = (ListClients.Info.CodError != 0 ? ListClients.Info.Mensaje : string.Empty) });
                //if (ListClients.Info.CodError != 0)
                //    return Json(new { total = 0, records = "", errorMessage = ListClients.Info.Mensaje });
                ////IEnumerable<PaylessProdPrioriDetModel> AllRecords = ListProd.Data.Item2.Distinct().Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
                //string IdTienda = ListClients.Data.Where(C => C.ClienteId == HttpContext.Session.GetObjSession<int>("Session.ClientId")).Fod().TiendaId.ToString();
                List<PaylessProdPrioriDetModel> Records = ListProd.Data.Where(R => R.Tienda == TiendaId).Select(O => Utility.Funcs.Reflect(O, new PaylessProdPrioriDetModel())).ToList();
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
                    //&& P.Periodo == dtpPeriodoBuscar
                    select Pd
                    ).ToList();
                List<PedidosDetExternos> ListPendientes = (
                    from Pd in TuplePextSent.Data.Item2
                    from P in TuplePextSent.Data.Item1
                    where Pd.PedidoId == P.Id
                    //&& P.Periodo == dtpPeriodoBuscar
                    select Pd
                    ).ToList();
                int Total = Records.Count;
                if (Records.Count() > 0) {
                    RetData<IEnumerable<ExistenciasExternModel>> StockData = await ApiLongClientFactory.Instance.GetStock(HttpContext.Session.GetObjSession<int>("Session.ClientId"));
                    RetData<Tuple<IEnumerable<PaylessProdPrioriArchM>, IEnumerable<PaylessProdPrioriArchDet>>> ListArch = await ApiLongClientFactory.Instance.GetPaylessPeriodPrioriFileExists2(HttpContext.Session.GetObjSession<string>("Session.TiendaId"));

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
                    IFormCollection GridForm = Request.Form;
                    FilteredRecords = Utility.ExpressionBuilderHelper.W2uiSearchNoSkip<PaylessProdPrioriDetModel>(Records, Request.Form);
                    Records = Utility.ExpressionBuilderHelper.W2uiSearch<PaylessProdPrioriDetModel>(Records, Request.Form);
                }
                int NRow = 0;
                Records = (
                    from R in Records
                    group R by R.Categoria
                    into Grp
                    select new PaylessProdPrioriDetModel {
                        Id = NRow++,
                        Categoria = Grp.Fod().Categoria,
                        Existencia = Grp.Sum(O1 => O1.Existencia),
                        Reservado = Grp.Sum(O1 => O1.Reservado),
                        CantPedir = Grp.Sum(O1 => O1.CantPedir)
                    }).ToList();
                Total = Records.Count;
                //group Pp by new { Pp.IdTransporte, Pp.Tienda, Pp.Barcode, Pp.Categoria, Pp.Cp }
                //    into Grp
                //    orderby Grp.Fod().Barcode
                //    select new PaylessProdPrioriDetModel {
                //        Id = Grp.Fod().Id,
                //        IdTransporte = Grp.Fod().IdTransporte,
                return Json(new { Total, Records, errorMessage = "", AllRecords, FilteredRecords, pedidosPendientes = TuplePextSent.Data.Item1 });
            } catch (Exception e1) {
                return Json(new { draw = 0, recordsFiltered = 0, recordsTotal = 0, errorMessage = e1.ToString(), data = "" });
            }
        }
    }
}