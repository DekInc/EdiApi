using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models.EdiDB;
using EdiApi.Models.WmsDB;
using Microsoft.EntityFrameworkCore;

namespace EdiApi.Models
{
    public static class ManualDB
    {
        public static IEnumerable<TsqlDespachosWmsComplex> SP_GetSN(ref Models.EdiDB.EdiDBContext _DbO)
        {
            List<TsqlDespachosWmsComplex> ListSn = new List<TsqlDespachosWmsComplex>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText ="[dbo].[GetSN]";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListSn.Add(new TsqlDespachosWmsComplex()
                        {
                            DespachoId = Dr.GetInt32(0),
                            FechaSalida = Dr.GetDateTime(1),
                            CodProducto = Dr.GetString(2),
                            Producto = Dr.GetString(3),
                            Cliente = Dr.GetString(4),
                            Quantity = Dr.GetDouble(5),
                            Weight = Dr.GetDouble(6),
                            Volume = Dr.GetDouble(7),
                            Bulks = Dr.GetDouble(8),
                            UnidadDeMedida = Dr.GetString(9),
                            Destino = Dr.GetString(10),
                            Procesado = Dr.GetInt32(11)
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            //List<GetSNSP> L = await WmsDbO.Query<GetSNSP>().FromSql("EXEC [dbo].[GetSN]").ToListAsync();
            return ListSn;
        }
        public static IEnumerable<TsqlDespachosWmsComplex> SP_GetSNDet(ref Models.EdiDB.EdiDBContext _DbO, int PedidoId)
        {
            List<TsqlDespachosWmsComplex> ListSn = new List<TsqlDespachosWmsComplex>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[GetSNDet] " + PedidoId.ToString();
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListSn.Add(new TsqlDespachosWmsComplex()
                        {
                            DespachoId = Convert.ToInt32(Dr.GetValue(0)),
                            FechaSalida = Convert.ToDateTime(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            Producto = Convert.ToString(Dr.GetValue(3)),
                            Cliente = Convert.ToString(Dr.GetValue(4)),
                            Quantity = Convert.ToDouble(Dr.GetValue(5)),
                            Weight = Convert.ToDouble(Dr.GetValue(6)),
                            Volume = Convert.ToDouble(Dr.GetValue(7)),
                            Bulks = Convert.ToDouble(Dr.GetValue(8)),
                            UnidadDeMedida = Convert.ToString(Dr.GetValue(9)),
                            Destino = Convert.ToString(Dr.GetValue(10)),

                            NoContenedor = Convert.ToString(Dr.GetValue(11)),
                            Motorista = Convert.ToString(Dr.GetValue(12)),
                            DocumentoMotorista = Convert.ToString(Dr.GetValue(13)),
                            DocumentoFiscal = Convert.ToString(Dr.GetValue(14)),
                            FechaDocFiscal = Convert.ToDateTime(Dr.GetValue(15)),
                            NoMarchamo = Convert.ToString(Dr.GetValue(16)),
                            Observacion = Convert.ToString(Dr.GetValue(17)), 
                            TotalValue = Convert.ToDouble(Dr.GetValue(20)),
                            NumeroOc = Convert.ToString(Dr.GetValue(21)),
                            PedidoId = Convert.ToInt32(Dr.GetValue(22))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            //List<GetSNSP> L = await WmsDbO.Query<GetSNSP>().FromSql("EXEC [dbo].[GetSN]").ToListAsync();
            return ListSn;
        }
        public static IEnumerable<FE830DataAux> SP_GetExistencias(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient)
        {
            List<FE830DataAux> ListExists = new List<FE830DataAux>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetExistencias] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new FE830DataAux()
                        {                            
                            CodProducto = Convert.ToString(Dr.GetValue(0)),
                            Producto = Convert.ToString(Dr.GetValue(1)),
                            Existencia = Convert.ToDouble(Dr.GetValue(2)),
                            UnidadDeMedida = Convert.ToString(Dr.GetValue(3)),
                            CodProductoLear = Convert.ToString(Dr.GetValue(4))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<ExistenciasExternModel> SP_GetExistenciasExtern(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient)
        {
            List<ExistenciasExternModel> ListExists = new List<ExistenciasExternModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetExistenciasExtern] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new ExistenciasExternModel()
                        {
                            Cliente = Convert.ToString(Dr.GetValue(0)),
                            Bodega = Convert.ToString(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            Descripcion = Convert.ToString(Dr.GetValue(3)),
                            Existencia = Convert.ToDouble(Dr.GetValue(4)),
                            Reservado = Convert.ToDouble(Dr.GetValue(5)),
                            Disponible = Convert.ToDouble(Dr.GetValue(6)),
                            ClienteID = Convert.ToInt32(Dr.GetValue(7)),
                            BodegaID = Convert.ToInt32(Dr.GetValue(8)),
                            Bultos = Convert.ToInt32(Dr.GetValue(9)),
                            Peso = Convert.ToDouble(Dr.GetValue(10)),
                            Volumen = Convert.ToDouble(Dr.GetValue(11)),
                            Uxb = Convert.ToInt32(Dr.GetValue(12)),
                            Lote = Convert.ToString(Dr.GetValue(13)),
                            Contenedor = Convert.ToString(Dr.GetValue(14))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosWmsModel> SP_GetPedidosWms(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient)
        {
            List<PedidosWmsModel> ListExists = new List<PedidosWmsModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosWms] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new PedidosWmsModel()
                        {
                            ClienteId = Convert.ToInt32(Dr.GetValue(0)),
                            PedidoBarcode = Convert.ToString(Dr.GetValue(1)),
                            FechaPedido = Convert.ToString(Dr.GetValue(2)),
                            Estatus = Convert.ToString(Dr.GetValue(3)),
                            NomBodega = Convert.ToString(Dr.GetValue(4)),
                            Regimen = Convert.ToString(Dr.GetValue(5)),
                            CodProducto = Convert.ToString(Dr.GetValue(6)),
                            Cantidad = Convert.ToDouble(Dr.GetValue(7)),
                            Observacion = Convert.ToString(Dr.GetValue(8)),
                            PedidoId = Convert.ToInt32(Dr.GetValue(9))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosWmsModel> GetWmsGroupDispatchs(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId) {
            List<PedidosWmsModel> ListExists = new List<PedidosWmsModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetWmsGroupDispatchs] {ClienteId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosWmsModel() {
                            ClienteId = Dr.Gr<int>(0),
                            PedidoBarcode = Dr.Gr<string>(1),
                            FechaPedido = Dr.Gr<string>(2),
                            Estatus = Dr.Gr<string>(3),
                            NomBodega = Dr.Gr<string>(4),
                            Regimen = Dr.Gr<string>(5),
                            Bultos = (double)Dr.Gr<decimal>(6),
                            Cantidad = Dr.Gr<double>(7),
                            Observacion = Dr.Gr<string>(8),
                            PedidoId = Dr.Gr<int>(9),
                            TiendaId = Dr.Gr<string>(10),
                            Total = Dr.Gr<int>(11)
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosWmsModel> GetWmsGroupDispatchsBills(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId) {
            List<PedidosWmsModel> ListExists = new List<PedidosWmsModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetWmsGroupDispatchsBills] {ClienteId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();                
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosWmsModel() {
                            ClienteId = Dr.Gr<int>(0),
                            PedidoBarcode = Dr.Gr<string>(1),
                            FechaPedido = Dr.Gr<string>(2),
                            Estatus = Dr.Gr<string>(3),
                            NomBodega = Dr.Gr<string>(4),
                            Regimen = Dr.Gr<string>(5),
                            Bultos = (double)Dr.Gr<decimal>(6),
                            Cantidad = Dr.Gr<double>(7),
                            Observacion = Dr.Gr<string>(8),
                            PedidoId = Dr.Gr<int>(9),
                            TiendaId = Dr.Gr<string>(10),
                            FactComercial = Dr.Gr<string>(11)
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosWmsModel> GetWmsDetDispatchsBills(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId) {
            List<PedidosWmsModel> ListExists = new List<PedidosWmsModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetWmsDetDispatchsBills] {ClienteId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosWmsModel() {
                            ClienteId = Dr.Gr<int>(0),
                            PedidoBarcode = Dr.Gr<string>(1),
                            FechaPedido = Dr.Gr<string>(2),
                            Estatus = Dr.Gr<string>(3),
                            NomBodega = Dr.Gr<string>(4),
                            Regimen = Dr.Gr<string>(5),
                            CodProducto = Dr.Gr<string>(6),
                            PedidoId = Dr.Gr<int>(7),
                            FactComercial = Dr.Gr<string>(8),
                            Observacion = Dr.Gr<string>(9)
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosDetExternos> SP_GetPedidosDetExternos(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient)
        {
            List<PedidosDetExternos> ListExists = new List<PedidosDetExternos>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosDetExternos] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListExists.Add(new PedidosDetExternos()
                        {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            PedidoId = Convert.ToInt32(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            CantPedir = Convert.ToDouble(Dr.GetValue(3)),
                            Producto = Convert.ToString(Dr.GetValue(4))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosDetExternos> SP_GetPedidosDetExternosByTienda(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId, int TiendaId) {
            List<PedidosDetExternos> ListExists = new List<PedidosDetExternos>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosDetExternosByTienda] {ClienteId}, {TiendaId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosDetExternos() {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            PedidoId = Convert.ToInt32(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            CantPedir = Convert.ToDouble(Dr.GetValue(3)),
                            Producto = Convert.ToString(Dr.GetValue(4))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosDetExternos> SP_GetPedidosDetExternosPendientesByTienda(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId, int TiendaId) {
            List<PedidosDetExternos> ListExists = new List<PedidosDetExternos>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"dbo.SP_GetPedidosDetExternosPendientesByTienda {ClienteId}, {TiendaId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosDetExternos() {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            PedidoId = Convert.ToInt32(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            CantPedir = Convert.ToDouble(Dr.GetValue(3)),
                            Producto = Convert.ToString(Dr.GetValue(4))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PaylessProdPrioriDetModel> SP_GetPedidosExternosDetById(ref Models.EdiDB.EdiDBContext _DbO, int PedidoId) {
            List<PaylessProdPrioriDetModel> List = new List<PaylessProdPrioriDetModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosExternosDetById] {PedidoId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        List.Add(new PaylessProdPrioriDetModel() {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            IdPaylessProdPrioriM = Convert.ToInt32(Dr.GetValue(1)),
                            Oid = Convert.ToString(Dr.GetValue(2)),
                            Barcode = Convert.ToString(Dr.GetValue(3)),
                            Estado = Convert.ToString(Dr.GetValue(4)),
                            Pri = Convert.ToString(Dr.GetValue(5)),
                            PoolP = Convert.ToString(Dr.GetValue(6)),
                            Producto = Convert.ToString(Dr.GetValue(7)),
                            Talla = Convert.ToString(Dr.GetValue(8)),
                            Lote = Convert.ToString(Dr.GetValue(9)),
                            Categoria = Convert.ToString(Dr.GetValue(10)),
                            Departamento = Convert.ToString(Dr.GetValue(11)),
                            Cp = Convert.ToString(Dr.GetValue(12)),
                            Pickeada = Convert.ToString(Dr.GetValue(13)),
                            Etiquetada = Convert.ToString(Dr.GetValue(14)),
                            Preinspeccion = Convert.ToString(Dr.GetValue(15)),
                            Cargada = Convert.ToString(Dr.GetValue(16)),
                            M3 = Convert.ToDouble(Dr.GetValue(17)),
                            Peso = Convert.ToDouble(Dr.GetValue(18)),
                            IdTransporte = Convert.ToInt32(Dr.GetValue(19)),
                            Transporte = Convert.ToString(Dr.GetValue(20)),
                            CantPedir = Convert.ToInt32(Dr.GetValue(21))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return List;
        }
        public static IEnumerable<PedidosDetExternos> SP_GetPedidosDetExternosGuardados(ref Models.EdiDB.EdiDBContext _DbO, int _IdClient) {
            List<PedidosDetExternos> ListExists = new List<PedidosDetExternos>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosDetExternosGuardados] {_IdClient}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosDetExternos() {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            PedidoId = Convert.ToInt32(Dr.GetValue(1)),
                            CodProducto = Convert.ToString(Dr.GetValue(2)),
                            CantPedir = Convert.ToDouble(Dr.GetValue(3)),
                            Producto = Convert.ToString(Dr.GetValue(4))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PedidosDetExternos> SP_GetPedidosDetExternosByDate(ref Models.EdiDB.EdiDBContext _DbO, DateTime DateInit, DateTime DateEnd, int Typ) {
            List<PedidosDetExternos> ListExists = new List<PedidosDetExternos>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosDetExternosByDate] '{DateInit.ToString(ApplicationSettings.DateTimeFormatShort)}', '{DateEnd.ToString(ApplicationSettings.DateTimeFormatShort)}', {Typ}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosDetExternos() {
                            CantPedir = Convert.ToDouble(Dr.GetValue(0)),
                            CodProducto = Convert.ToString(Dr.GetValue(1)),
                            Producto = Convert.ToString(Dr.GetValue(2))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
        public static IEnumerable<PaylessProdPrioriDetModel> SP_GetPaylessProdPrioriByPeriod(ref Models.EdiDB.EdiDBContext _DbO, string Period)
        {
            List<PaylessProdPrioriDetModel> ListProdDet = new List<PaylessProdPrioriDetModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand())
            {
                Cmd.CommandText = $"[dbo].[SP_GetPaylessProdPrioriByPeriod] '{Period}'";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows)
                {
                    while (Dr.Read())
                    {
                        ListProdDet.Add(new PaylessProdPrioriDetModel()
                        {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            IdPaylessProdPrioriM = Convert.ToInt32(Dr.GetValue(1)),
                            Oid = Convert.ToString(Dr.GetValue(2)),
                            Barcode = Convert.ToString(Dr.GetValue(3)),
                            Estado = Convert.ToString(Dr.GetValue(4)),
                            Pri = Convert.ToString(Dr.GetValue(5)),
                            PoolP = Convert.ToString(Dr.GetValue(6)),
                            Producto = Convert.ToString(Dr.GetValue(7)),
                            Talla = Convert.ToString(Dr.GetValue(8)),
                            Lote = Convert.ToString(Dr.GetValue(9)),
                            Categoria = Convert.ToString(Dr.GetValue(10)),
                            Departamento = Convert.ToString(Dr.GetValue(11)),
                            Cp = Convert.ToString(Dr.GetValue(12)),
                            Pickeada = Convert.ToString(Dr.GetValue(13)),
                            Etiquetada = Convert.ToString(Dr.GetValue(14)),
                            Preinspeccion = Convert.ToString(Dr.GetValue(15)),
                            Cargada = Convert.ToString(Dr.GetValue(16)),
                            M3 = Convert.ToDouble(Dr.GetValue(17)),
                            Peso = Convert.ToDouble(Dr.GetValue(18)),
                            IdTransporte = Convert.ToInt32(Dr.GetValue(19)),
                            Transporte = Convert.ToString(Dr.GetValue(20))                            
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }        
        public static IEnumerable<PaylessProdPrioriDetModel> SP_GetPaylessProdPrioriByPeriodAndIdTransport(ref Models.EdiDB.EdiDBContext _DbO, string Period, int IdTransport) {
            List<PaylessProdPrioriDetModel> ListProdDet = new List<PaylessProdPrioriDetModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPaylessProdPrioriByPeriodAndIdTransport] '{Period}', {IdTransport}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new PaylessProdPrioriDetModel() {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            IdPaylessProdPrioriM = Convert.ToInt32(Dr.GetValue(1)),
                            Oid = Convert.ToString(Dr.GetValue(2)),
                            Barcode = Convert.ToString(Dr.GetValue(3)),
                            Estado = Convert.ToString(Dr.GetValue(4)),
                            Pri = Convert.ToString(Dr.GetValue(5)),
                            PoolP = Convert.ToString(Dr.GetValue(6)),
                            Producto = Convert.ToString(Dr.GetValue(7)),
                            Talla = Convert.ToString(Dr.GetValue(8)),
                            Lote = Convert.ToString(Dr.GetValue(9)),
                            Categoria = Convert.ToString(Dr.GetValue(10)),
                            Departamento = Convert.ToString(Dr.GetValue(11)),
                            Cp = Convert.ToString(Dr.GetValue(12)),
                            Pickeada = Convert.ToString(Dr.GetValue(13)),
                            Etiquetada = Convert.ToString(Dr.GetValue(14)),
                            Preinspeccion = Convert.ToString(Dr.GetValue(15)),
                            Cargada = Convert.ToString(Dr.GetValue(16)),
                            M3 = Convert.ToDouble(Dr.GetValue(17)),
                            Peso = Convert.ToDouble(Dr.GetValue(18)),
                            IdTransporte = Convert.ToInt32(Dr.GetValue(19)),
                            Transporte = Convert.ToString(Dr.GetValue(20)),
                            dateProm = Convert.ToString(Dr.GetValue(21)) //Realmente NomCliente
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static int UploadBatch(ref Models.WmsDB.WmsContext _Wms, string Batch) {
            int Res = 0;
            using (DbCommand Cmd = _Wms.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = Batch;
                Cmd.CommandTimeout = 600;
                _Wms.Database.OpenConnection();
                 Res = Cmd.ExecuteNonQuery();
                _Wms.Database.CloseConnection();
            }
            return Res;
        }
        public static DataTable SpGeneraSalidaWMS(ref Models.WmsDB.WmsContext _Wms, string FechaSalida, string CodProducto, int BodegaId, int RegimenId, int ClienteId, int LocationId, int RackId) {
            DataTable Dt = new DataTable();
            using (DbCommand Cmd = _Wms.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[spGeneraSalida] '{FechaSalida}', '{CodProducto}', {BodegaId}, {RegimenId}, {ClienteId}, {LocationId}, {RackId} ";
                Cmd.CommandTimeout = 600;
                _Wms.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                Dt.Load(Dr);
                _Wms.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return Dt;
        }
        public static DataTable SpGeneraSalidaWMS2(ref Models.EdiDB.EdiDBContext _Dbo, string FechaSalida, string CodProducto, int BodegaId, int RegimenId, int ClienteId, int LocationId, int RackId) {
            DataTable Dt = new DataTable();
            using (DbCommand Cmd = _Dbo.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GeneraSalidaWms] '{FechaSalida}', '{CodProducto}', {BodegaId}, {RegimenId}, {ClienteId}, {LocationId}, {RackId} ";
                Cmd.CommandTimeout = 600;
                _Dbo.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                Dt.Load(Dr);
                _Dbo.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return Dt;
        }
        public static IEnumerable<Transacciones> GetTransaccionById(ref WmsContext _Wms, int TransaccionID) {
            List<Transacciones> List = new List<Transacciones>();
            using (DbCommand Cmd = _Wms.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"select TransaccionID, NoTransaccion, FechaTransaccion, IdTipoTransaccion, PedidoId, BodegaId, RegimenId, ClienteId, Total, TipoIngreso, UsuarioCrea, FechaCrea, Observacion, EstatusId, OperarioId, TipoPicking, ExportadorId, DestinoId, TransportistaId, Pais_Orig, Adu_fro, Placa, Marchamo, Contenedor, Cod_Motoris, Remolque, RecivingCliente, FechaReciving, FacturaId, IdrControl from dbo.Transacciones where TransaccionId = {TransaccionID}";
                _Wms.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        List.Add(new Transacciones() {
                            TransaccionId = Dr.Gr<int>(0),
                            NoTransaccion = Dr.Gr<string>(1),
                            FechaTransaccion = Dr.Gr<DateTime>(2),
                            IdtipoTransaccion = Dr.Gr<string>(3),
                            PedidoId = Dr.Gr<int>(4),
                            BodegaId = Dr.Gr<int>(5),
                            RegimenId = Dr.Gr<int>(6),
                            ClienteId = Dr.Gr<int>(7),
                            Total = Dr.Gr<double>(8),
                            TipoIngreso = Dr.Gr<string>(9),
                            Usuariocrea = Dr.Gr<string>(10),
                            Fechacrea = Dr.Gr<DateTime>(11),
                            Observacion = Dr.Gr<string>(12),
                            EstatusId = Dr.Gr<int>(13),
                            Operarioid = Dr.Gr<int>(14),
                            TipoPicking = Dr.Gr<string>(15),
                            Exportadorid = Dr.Gr<int>(16),
                            Destinoid = Dr.Gr<int>(17),
                            Transportistaid = Dr.Gr<int>(18),
                            PaisOrig = Dr.Gr<int>(19),
                            AduFro = Dr.Gr<string>(20),
                            Marchamo = Dr.Gr<string>(21),
                            Contenedor = Dr.Gr<string>(22),
                            CodMotoris = Dr.Gr<string>(23),
                            Remolque = Dr.Gr<string>(24),
                            RecivingCliente = Dr.Gr<string>(25),
                            FechaReciving = Dr.Gr<DateTime>(26),
                            FacturaId = Dr.Gr<int>(27)
                            //TransaccionId = Convert.ToInt32(Dr.GetValue(0)),
                            //NoTransaccion = Convert.ToString(Dr.GetValue(1)),
                            //FechaTransaccion = Dr.GetDateTime(2),
                            //IdtipoTransaccion = Convert.ToString(Dr.GetValue(3)),
                            //PedidoId = Convert.ToInt32(Dr.GetValue(4)),
                            //BodegaId = Convert.ToInt32(Dr.GetValue(5)),
                            //RegimenId = Convert.ToInt32(Dr.GetValue(6)),
                            //ClienteId = Convert.ToInt32(Dr.GetValue(7)),
                            //Total = Convert.ToDouble(Dr.GetValue(8)),
                            //TipoIngreso = Convert.ToString(Dr.GetValue(9)),
                            //Usuariocrea = Convert.ToString(Dr.GetValue(10)),
                            //Fechacrea = Dr.GetDateTime(11),
                            //Observacion = Convert.ToString(Dr.GetValue(12)),
                            //EstatusId = Convert.ToInt32(Dr.GetValue(13)),
                            //Operarioid = Convert.ToInt32(Dr.GetValue(14)),
                            //TipoPicking = Convert.ToString(Dr.GetValue(15)),
                            //Exportadorid = Convert.ToInt32(Dr.GetValue(16)),
                            //Destinoid = Convert.ToInt32(Dr.GetValue(17)),
                            //Transportistaid = Convert.ToInt32(Dr.GetValue(18)),
                            //PaisOrig = Convert.ToInt32(Dr.GetValue(19)),
                            //AduFro = Convert.ToString(Dr.GetValue(20)),
                            //Marchamo = Convert.ToString(Dr.GetValue(21)),
                            //Contenedor = Convert.ToString(Dr.GetValue(22)),
                            //CodMotoris = Convert.ToString(Dr.GetValue(23)),
                            //Remolque = Convert.ToString(Dr.GetValue(24)),
                            //RecivingCliente = Convert.ToString(Dr.GetValue(25)),
                            //FechaReciving = Dr.GetDateTime(26),
                            //FacturaId = Convert.ToInt32(Dr.GetValue(27))
                            //idrcontrol = Convert.ToInt32(Dr.GetValue(27)),
                        });
                    }
                }
                _Wms.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return List;
        }
        public static IEnumerable<PaylessProdPrioriDetModel> SP_GetPaylessProdPrioriFileDif(ref Models.EdiDB.EdiDBContext _DbO, int IdData, int IdProdArch) {
            List<PaylessProdPrioriDetModel> ListProdDet = new List<PaylessProdPrioriDetModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"dbo.SP_GetPaylessProdPrioriFileDif {IdData}, {IdProdArch}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    int NRow = 0;
                    while (Dr.Read()) {
                        ListProdDet.Add(new PaylessProdPrioriDetModel() {
                            Id = NRow++,
                            //Id = Convert.ToInt32(Dr.GetValue(0)),
                            IdPaylessProdPrioriM = Convert.ToInt32(Dr.GetValue(1)),
                            Oid = Convert.ToString(Dr.GetValue(2)),
                            Barcode = Convert.ToString(Dr.GetValue(3)),
                            Estado = Convert.ToString(Dr.GetValue(4)),
                            Pri = Convert.ToString(Dr.GetValue(5)),
                            PoolP = Convert.ToString(Dr.GetValue(6)),
                            Producto = Convert.ToString(Dr.GetValue(7)),
                            Talla = Convert.ToString(Dr.GetValue(8)),
                            Lote = Convert.ToString(Dr.GetValue(9)),
                            Categoria = Convert.ToString(Dr.GetValue(10)),
                            Departamento = Convert.ToString(Dr.GetValue(11)),
                            Cp = Convert.ToString(Dr.GetValue(12)),
                            //Pickeada = Convert.ToString(Dr.GetValue(13)),
                            //Etiquetada = Convert.ToString(Dr.GetValue(14)),
                            //Preinspeccion = Convert.ToString(Dr.GetValue(15)),
                            //Cargada = Convert.ToString(Dr.GetValue(16)),
                            M3 = Convert.ToDouble(Dr.GetValue(17)),
                            Peso = Convert.ToDouble(Dr.GetValue(18)),
                            IdTransporte = Convert.ToInt32(Dr.GetValue(19)),
                            Transporte = Convert.ToString(Dr.GetValue(20))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static IEnumerable<PaylessProdPrioriDetModel> SP_GetPaylessProdPrioriAll(ref Models.EdiDB.EdiDBContext _DbO, string TiendaId) {
            List<PaylessProdPrioriDetModel> ListProdDet = new List<PaylessProdPrioriDetModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPaylessProdPrioriAll] '{TiendaId}'";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new PaylessProdPrioriDetModel() {
                            Id = Convert.ToInt32(Dr.GetValue(0)),
                            IdPaylessProdPrioriM = Convert.ToInt32(Dr.GetValue(1)),
                            Oid = Convert.ToString(Dr.GetValue(2)),
                            Barcode = Convert.ToString(Dr.GetValue(3)),
                            Estado = Convert.ToString(Dr.GetValue(4)),
                            Pri = Convert.ToString(Dr.GetValue(5)),
                            PoolP = Convert.ToString(Dr.GetValue(6)),
                            Producto = Convert.ToString(Dr.GetValue(7)),
                            Talla = Convert.ToString(Dr.GetValue(8)),
                            Lote = Convert.ToString(Dr.GetValue(9)),
                            Categoria = Convert.ToString(Dr.GetValue(10)),
                            Departamento = Convert.ToString(Dr.GetValue(11)),
                            Cp = Convert.ToString(Dr.GetValue(12)),
                            Pickeada = Convert.ToString(Dr.GetValue(13)),
                            Etiquetada = Convert.ToString(Dr.GetValue(14)),
                            Preinspeccion = Convert.ToString(Dr.GetValue(15)),
                            Cargada = Convert.ToString(Dr.GetValue(16)),
                            M3 = Convert.ToDouble(Dr.GetValue(17)),
                            Peso = Convert.ToDouble(Dr.GetValue(18)),
                            IdTransporte = Convert.ToInt32(Dr.GetValue(19)),
                            Transporte = Convert.ToString(Dr.GetValue(20))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static IEnumerable<FE830DataAux> SP_GetExistenciasByTienda(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId, int TiendaId) {
            List<FE830DataAux> ListProdDet = new List<FE830DataAux>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetExistenciasByTienda] {ClienteId}, '{TiendaId}'";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new FE830DataAux() {
                            CodProductoLear = Convert.ToString(Dr.GetValue(0)),
                            CodProducto = Convert.ToString(Dr.GetValue(1)),
                            Existencia = Convert.ToDouble(Dr.GetValue(2))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static IEnumerable<FE830DataAux> SP_GetExistenciasByCliente(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId) {
            List<FE830DataAux> ListProdDet = new List<FE830DataAux>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"dbo.SP_GetExistenciasByCliente {ClienteId}";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new FE830DataAux() {
                            CodProductoLear = Convert.ToString(Dr.GetValue(0)),
                            CodProducto = Convert.ToString(Dr.GetValue(1)),
                            Existencia = Convert.ToDouble(Dr.GetValue(2))
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static List<PaylessProdPrioriDetModel> SP_GetPaylessProdSinPedido(ref Models.EdiDB.EdiDBContext _DbO, int ClienteId, int TiendaId) {
            List<PaylessProdPrioriDetModel> ListProdDet = new List<PaylessProdPrioriDetModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"dbo.SP_GetPaylessProdSinPedido {ClienteId}, '{TiendaId}'";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new PaylessProdPrioriDetModel() {
                            Barcode = Convert.ToString(Dr.GetValue(0)),
                            Cp = Convert.ToString(Dr.GetValue(1)),
                            Categoria = Convert.ToString(Dr.GetValue(2)),
                            IdPaylessProdPrioriM = Convert.ToInt32(Dr.GetValue(3)),
                            Departamento = Convert.ToString(Dr.GetValue(4)),
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static List<PedidosPendientesAdmin> SP_GetPedidosPendientesAdmin(ref Models.EdiDB.EdiDBContext _DbO) {
            List<PedidosPendientesAdmin> ListProdDet = new List<PedidosPendientesAdmin>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"dbo.SP_GetPedidosPendientesAdmin";
                Cmd.CommandTimeout = 600;
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new PedidosPendientesAdmin() {
                            PedidoId = Dr.Gr<int>(0),
                            Bodega = Dr.Gr<string>(1),
                            TiendaId = Dr.Gr<int>(2),
                            FechaPedido = Dr.Gr<string>(3),
                            Periodo = Dr.Gr<string>(4),
                            Categoria = Dr.Gr<string>(5),
                            CP = Dr.Gr<string>(6),
                            Barcode = Dr.Gr<string>(7),
                            IdRack = Dr.Gr<int>(8),
                            NombreRack = Dr.Gr<string>(9),
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static List<PeticionesAdminBGModel> SP_GetPeticionesAdminB(ref Models.EdiDB.EdiDBContext _DbO) {
            List<PeticionesAdminBGModel> ListProdDet = new List<PeticionesAdminBGModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"dbo.SP_GetPeticionesAdminB";
                Cmd.CommandTimeout = 600;
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListProdDet.Add(new PeticionesAdminBGModel() {
                            Id = Dr.Gr<int>(0),
                            TiendaId = Dr.Gr<int>(1),
                            Tienda = Dr.Gr<string>(2),
                            WomanQty = Dr.Gr<int>(3),
                            ManQty = Dr.Gr<int>(4),
                            KidQty = Dr.Gr<int>(5),
                            AccQty = Dr.Gr<int>(6),
                            FechaCreacion = Dr.Gr<string>(7),
                            FechaPedido = Dr.Gr<string>(8),
                            TotalCp = Dr.Gr<int>(9),
                            PedidoWMS = Dr.Gr<int?>(10),
                            IdEstado = Dr.Gr<int>(11),
                            WomanQtyEnv = Dr.Gr<int>(12),
                            ManQtyEnv = Dr.Gr<int>(13),
                            KidQtyEnv = Dr.Gr<int>(14),
                            AccQtyEnv = Dr.Gr<int>(15),
                            TotalCpEnv = Dr.Gr<int>(16),
                            FullPed = Dr.Gr<bool?>(17),
                            Divert = Dr.Gr<bool?>(18),
                            TiendaIdDestino = Dr.Gr<int?>(19)
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListProdDet;
        }
        public static IEnumerable<PedidosWmsModel> SP_GetPedidosMWmsByTienda(ref Models.EdiDB.EdiDBContext _DbO, int IdClient, int TiendaId) {
            List<PedidosWmsModel> ListExists = new List<PedidosWmsModel>();
            using (DbCommand Cmd = _DbO.Database.GetDbConnection().CreateCommand()) {
                Cmd.CommandText = $"[dbo].[SP_GetPedidosMWmsByTienda] {IdClient}, '{TiendaId}'";
                _DbO.Database.OpenConnection();
                DbDataReader Dr = Cmd.ExecuteReader();
                if (Dr.HasRows) {
                    while (Dr.Read()) {
                        ListExists.Add(new PedidosWmsModel() {
                            ClienteId = Dr.Gr<int>(0),
                            PedidoBarcode = Dr.Gr<string>(1),
                            FechaPedido = Dr.Gr<string>(2),
                            Estatus = Dr.Gr<string>(4),
                            NomBodega = Dr.Gr<string>(5),
                            Regimen = Dr.Gr<string>(6),
                            Observacion = Dr.Gr<string>(7),
                            PedidoId = Dr.Gr<int>(8),
                            Total = Dr.Gr<int>(9)
                        });
                    }
                }
                _DbO.Database.CloseConnection();
                if (!Dr.IsClosed)
                    Dr.Close();
            }
            return ListExists;
        }
    }
}
