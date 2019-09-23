using ComModels.Models.WmsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public static class SqlGenHelper {
        public static string Temp = "";
        public static string GetSqlWmsMaxTbl(string TableName, string Pk, string Var) {
            return $"SELECT @{Var} = ISNULL(MAX({Pk}), 0) + 5 FROM dbo.{TableName}; {Temp}";
        }
        public static string GetSqlWmsInsertTransacciones(Transacciones T) {
            return $"INSERT INTO dbo.Transacciones(TransaccionID, NoTransaccion, IDTipoTransaccion, FechaTransaccion, BodegaID, RegimenID, ClienteID, TipoIngreso, Observacion, Usuariocrea, Fechacrea, EstatusID, exportadorid, destinoid) " +
                $"SELECT @MaxTransaccionId, 'IN' + RIGHT('000000'+ CONVERT(VARCHAR(64), @MaxTransaccionId), 6), '{T.IdtipoTransaccion}', {T.FechaTransaccion.ToSqlDate()}, {T.BodegaId}, {T.RegimenId}, {T.ClienteId}, '{T.TipoIngreso}', '{T.Observacion}', '{T.Usuariocrea}', {T.Fechacrea.ToSqlDate()}, {T.EstatusId}, {T.Exportadorid}, {T.Destinoid}; {Temp}";
        }
        public static string GetSqlWmsInsertProducto(Producto P) {
            return $"INSERT INTO dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida) " +
                $"SELECT '{P.CodProducto}', '{P.Descripcion}', {P.UnidadMedida}, {P.ClienteId}, {P.EstatusId}, {P.CategoriaId}, {P.CantMinima}, {P.Fecha.ToSqlDate()}, '{P.Comentario}', {P.StockMaximo}, {P.Descargoid}, '{P.Partida}'; {Temp}";
        }
        public static string GetSqlWmsInsertInventario(Inventario I) {
            return $"INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)"
                + $"SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), {I.FechaCreacion.ToSqlDate()}, {I.ClienteId}, '{I.Descripcion}', { (I.Declarado ?? 0) }, {I.Valor}, {I.Articulos}, {I.Peso}, {I.Volumen}, {I.EstatusId}, {(I.IsAgranel.Value? 1 : 0)}, {I.TipoBulto}, {I.Existencia}, {I.Auditado}, {I.CantidadInicial}, {I.Rack ?? 0}; {Temp}";
        }
        public static string GetSqlWmsInsertDetalleTransacciones(DetalleTransacciones DT) {
            return $"INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) " +
                $"SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, {DT.Conteo}, {DT.Cantidad}, {DT.Valor}, {DT.Fechaitem.ToSqlDate()}, {DT.Rack ?? 0}, '{DT.Embalaje}', {(DT.IsEscaneado.Value? 1: 0)}; {Temp}";
        }
        public static string GetSqlWmsInsertItemInventario(ItemInventario Ii) {
            return $"INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) " +
                $"SELECT @MaxItemInventario, @MaxInventarioId, '{Ii.CodProducto}', {Ii.Declarado}, {Ii.Precio ?? 0}, '{Ii.Observacion}', {Ii.Fechaitem.ToSqlDate()}, '{Ii.Descripcion}', {Ii.Auditado}, {Ii.Existencia}, {Ii.CantidadInicial}, '{Ii.CodEquivale}', {Ii.PaisOrig}, '{Ii.Lote}', '{Ii.NumeroOc}', '{Ii.Modelo}', '{Ii.Color}', '{Ii.Estilo}'; {Temp}";
        }
        public static string GetSqlWmsInsertDtllItemTransaccion(DtllItemTransaccion Dit) {
            return $"INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) " +
                $"SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, {Dit.Cantidad}, {Dit.Precio}, {Dit.Rack ?? 0}; {Temp}";
        }
        public static string GetSqlWmsInsertItemParamaetroxProducto(ItemParamaetroxProducto Pa) {
            return $"INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) " +
                $"SELECT @MaxInventarioId, @MaxItemInventario, '{Pa.CodProducto}', {Pa.ParametroId}, '{Pa.ValParametro}'; {Temp}";
        }
        public static string GetSqlWmsUpdateTransaccionesRackFull() {
            return $"UPDATE dbo.Transacciones SET EstatusId = 6 WHERE TransaccionID = @MaxTransaccionId; {Temp}";
        }
        public static string GetSqlWmsInsertDocumentosxTransaccion(DocumentosxTransaccion D) {
            return $"INSERT INTO dbo.DocumentosxTransaccion(IDDocxTransaccion, transaccionid, fecha, INFORME_ALMACEN, FE_INFORME_ALMACEN, IM_5, ORDEN_COMPRA) " +
                $"SELECT @MaxDocTran, @MaxTransaccionId, {D.Fecha.ToSqlDate()}, '{D.InformeAlmacen}', {D.FeInformeAlmacen.ToSqlDate()}, '{D.Im5}', '{D.OrdenCompra}'; {Temp}";
        }
        public static string GetSqlWmsInsertPedido(Pedido P) {
            return $"INSERT INTO dbo.Pedido(PedidoID, fechapedido, ClienteID, TipoPedido, FechaRequerido, EstatusID, Observacion, BodegaID, RegimenID, PedidoBarcode) " +
                $"SELECT @MaxPedidoId, {P.Fechapedido.ToSqlDate()}, {P.ClienteId}, '{P.TipoPedido}', {P.FechaRequerido.ToSqlDate()}, {P.EstatusId}, '{P.Observacion}', {P.BodegaId}, {P.RegimenId}, 'IN' + RIGHT('00000'+ CONVERT(VARCHAR(64), @MaxPedidoId), 5); {Temp}";
        }
        public static string GetSqlWmsUpdateTransaccionesPedido(int TransaccionID) {
            return $"UPDATE dbo.Transacciones SET PedidoId = @MaxPedidoId WHERE TransaccionID = {TransaccionID}; {Temp}";
        }
        public static string GetSqlWmsInsertDtllPedido(DtllPedido Dp) {
            return $"INSERT INTO dbo.DtllPedido(DtllPedidoId, PedidoId, Cantidad, CodProducto) " +
                $"SELECT @MaxPedidoDet, @MaxPedidoId, {Dp.Cantidad}, '{Dp.CodProducto}'; {Temp}";
        }
        public static string GetSqlWmsInsertSysTempSalidas(SysTempSalidas St) {
            return $"INSERT INTO dbo.SysTempSalidas(TransaccionId, PedidoId, InventarioId, DtllPedidoId, ItemInventarioId, CodProducto, Cantidad, Precio, Fecha, Usuario, Lote) " +
                $"SELECT @TransaccionID, @MaxPedidoId, {St.InventarioId}, @MaxPedidoDet, {St.ItemInventarioId}, '{St.CodProducto}', {St.Cantidad}, {St.Precio}, {St.Fecha.ToSqlDate()}, '{St.Usuario}', '{St.Lote}'; {Temp}";
        }
        public static string GetSqlWmsInsertDetalleTransacciones2(DetalleTransacciones DT) {
            return $"INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, rack, embalaje, IsEscaneado) " +
                $"SELECT @MaxDetTran, @TransaccionID, {DT.InventarioId}, {DT.Conteo}, {DT.Cantidad}, {DT.Valor}, {DT.Rack}, '{DT.Embalaje}', {(DT.IsEscaneado.Value ? 1 : 0)}; {Temp}";
        }
        public static string GetSqlWmsInsertDtllItemTransaccion2(DtllItemTransaccion Dit) {
            return $"INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) " +
                $"SELECT @MaxDetItemTran, @TransaccionID, @MaxDetTran, {Dit.ItemInventarioId}, {Dit.Cantidad}, {Dit.Precio}, {Dit.Rack}; {Temp}";
        }
        public static string GetSqlWmsInsertPedido2(Pedido P) {
            return $"INSERT INTO dbo.Pedido(PedidoID, fechapedido, ClienteID, TipoPedido, FechaRequerido, EstatusID, Observacion, BodegaID, RegimenID, PedidoBarcode) " +
                $"SELECT @MaxPedidoId, {P.Fechapedido.ToSqlDate()}, {P.ClienteId}, '{P.TipoPedido}', {P.FechaRequerido.ToSqlDate()}, {P.EstatusId}, '{P.Observacion}', {P.BodegaId}, {P.RegimenId}, 'PD' + RIGHT('00000'+ CONVERT(VARCHAR(64), @MaxPedidoId), 5); {Temp}";
        }
        public static string GetSqlWmsUpdateTransaccionesPedidoId() {
            return $"UPDATE dbo.Transacciones SET PedidoId = @MaxPedidoId WHERE TransaccionID = @TransaccionID; {Temp}";
        }
        public static string GetSqlWmsInsertTransaccionesOut(Transacciones T) {
            return $"INSERT INTO dbo.Transacciones(TransaccionID, NoTransaccion, IDTipoTransaccion, FechaTransaccion, BodegaID, RegimenID, ClienteID, TipoIngreso, Observacion, Usuariocrea, Fechacrea, EstatusID) " +
                $"SELECT @TransaccionID, '{T.IdtipoTransaccion}' + RIGHT('000000'+ CONVERT(VARCHAR(64), @TransaccionID), 6), '{T.IdtipoTransaccion}', {T.FechaTransaccion.ToSqlDate()}, {T.BodegaId}, {T.RegimenId}, {T.ClienteId}, '{T.TipoIngreso}', '{T.Observacion}', '{T.Usuariocrea}', {T.Fechacrea.ToSqlDate()}, {T.EstatusId}; {Temp}";
        }
    }
}
