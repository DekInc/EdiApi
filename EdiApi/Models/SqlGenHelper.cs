using EdiApi.Models.WmsDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public static class SqlGenHelper {
        public static string Temp = "";
        public static string GetSqlWmsMaxTbl(string TableName, string Pk, string Var) {
            return $"SELECT @{Var} = ISNULL(MAX({Pk}), 0) + 1 FROM wms.dbo.{TableName}; {Temp}";
        }
        public static string GetSqlWmsInsertTransacciones(Transacciones T) {
            return "INSERT INTO wms.dbo.Transacciones(TransaccionID, NoTransaccion, IDTipoTransaccion, FechaTransaccion, BodegaID, RegimenID, ClienteID, TipoIngreso, Observacion, Usuariocrea, Fechacrea, EstatusID, exportadorid, destinoid) " +
                $"SELECT @MaxTransaccionId, 'IN' + RIGHT('00000'+ CONVERT(VARCHAR(64), @MaxTransaccionId), 5), '{T.IdtipoTransaccion}', {T.FechaTransaccion.ToSqlDate()}, {T.BodegaId}, {T.RegimenId}, {T.ClienteId}, '{T.TipoIngreso}', '{T.Observacion}', '{T.Usuariocrea}', {T.Fechacrea.ToSqlDate()}, {T.EstatusId}, {T.Exportadorid}, {T.Destinoid}; {Temp}";
        }
        public static string GetSqlWmsInsertProducto(Producto P) {
            return $"INSERT INTO wms.dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida) " +
                $"SELECT '{P.CodProducto}', '{P.Descripcion}', {P.UnidadMedida}, {P.ClienteId}, {P.EstatusId}, {P.CategoriaId}, {P.CantMinima}, {P.Fecha.ToSqlDate()}, '{P.Comentario}', {P.StockMaximo}, {P.Descargoid}, '{P.Partida}'; {Temp}";
        }
        public static string GetSqlWmsInsertInventario(Inventario I) {
            return "INSERT INTO wms.dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)"
                + $"SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), {I.FechaCreacion.ToSqlDate()}, {I.ClienteId}, '{I.Descripcion}', {I.Declarado}, {I.Valor}, {I.Articulos}, {I.Peso}, {I.Volumen}, {I.EstatusId}, {(I.IsAgranel.Value? 1 : 0)}, {I.TipoBulto}, {I.Existencia}, {I.Auditado}, {I.CantidadInicial}, {I.Rack}; {Temp}";
        }
        public static string GetSqlWmsInsertDetalleTransacciones(DetalleTransacciones DT) {
            return "INSERT INTO wms.dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) " +
                $"SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, {DT.Conteo}, {DT.Cantidad}, {DT.Valor}, {DT.Fechaitem.ToSqlDate()}, {DT.Rack}, '{DT.Embalaje}', {(DT.IsEscaneado.Value? 1: 0)}; {Temp}";
        }
        public static string GetSqlWmsInsertItemInventario(ItemInventario Ii) {
            return "INSERT INTO wms.dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) " +
                $"SELECT @MaxItemInventario, @MaxInventarioId, '{Ii.CodProducto}', {Ii.Declarado}, {Ii.Precio}, '{Ii.Observacion}', {Ii.Fechaitem.ToSqlDate()}, '{Ii.Descripcion}', {Ii.Auditado}, {Ii.Existencia}, {Ii.CantidadInicial}, '{Ii.CodEquivale}', {Ii.PaisOrig}, '{Ii.Lote}', '{Ii.NumeroOc}', '{Ii.Modelo}', '{Ii.Color}', '{Ii.Estilo}'; {Temp}";
        }
        public static string GetSqlWmsInsertDtllItemTransaccion(DtllItemTransaccion Dit) {
            return "INSERT INTO wms.dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) " +
                $"SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, {Dit.Cantidad}, {Dit.Precio}, {Dit.Rack}; {Temp}";
        }

        public static string GetSqlWmsInsertItemParamaetroxProducto(ItemParamaetroxProducto Pa) {
            return "INSERT INTO wms.dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) " +
                $"SELECT @MaxInventarioId, @MaxItemInventario, '{Pa.CodProducto}', {Pa.ParametroId}, '{Pa.ValParametro}'; {Temp}";
        }

        public static string GetSqlWmsUpdateTransaccionesRackFull() {
            return $"UPDATE wms.dbo.Transacciones SET EstatusId = 6 WHERE TransaccionID = @MaxTransaccionId; {Temp}";
        }

        public static string GetSqlWmsInsertDocumentosxTransaccion(DocumentosxTransaccion D) {
            return "INSERT INTO wms.dbo.DocumentosxTransaccion(IDDocxTransaccion, transaccionid, fecha, INFORME_ALMACEN, FE_INFORME_ALMACEN, IM_5, fe_im_5, ORDEN_COMPRA) " +
                $"SELECT @MaxDocTran, @MaxTransaccionId, {D.Fecha.ToSqlDate()}, '{D.InformeAlmacen}', {D.FeInformeAlmacen.ToSqlDate()}, '{D.Im5}', {D.FeIm5.ToSqlDate()}, '{D.OrdenCompra}'; {Temp}";
        }
    }
}
