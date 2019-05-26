SET XACT_ABORT ON

BEGIN TRANSACTION TRAN1

DECLARE @MaxTransaccionId INT;

DECLARE @MaxInventarioId INT;

DECLARE @MaxDTId INT;

DECLARE @MaxItemInventario INT;

DECLARE @MaxDetItemTran INT;

DECLARE @MaxDocTran INT;

BEGIN TRY

SET @MaxTransaccionId = 116545; 

SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 1 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '24/05/2019', 103), 1432, 'SOPORTEMETALICO FINAL', 14, 0.94, 1, 3.9, 0.03, 2, 0, 70, 14, 14, 14, 12483; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 14, 0.94, CONVERT(DATETIME, '21/05/2019', 103), 12483, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 1 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, '5713-GALERIAS1', 14, 13.16, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '21/05/2019', 103), 'SOPORTEMETALICO FINAL', 14, 14, 14, '', 166, 'RPF6', '8', 'ESTANTE1', 'AAG1458', ''; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 14, 13.16, 12483; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 23, '13.16'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, '5713-GALERIAS1', 15, 'ESTANTE1'; 
UPDATE dbo.Transacciones SET EstatusId = 6 WHERE TransaccionID = @MaxTransaccionId; 
SELECT @MaxDocTran = ISNULL(MAX(IddocxTransaccion), 0) + 1 FROM dbo.DocumentosxTransaccion; 
INSERT INTO dbo.DocumentosxTransaccion(IDDocxTransaccion, transaccionid, fecha, INFORME_ALMACEN, FE_INFORME_ALMACEN, IM_5, ORDEN_COMPRA) SELECT @MaxDocTran, @MaxTransaccionId, CONVERT(DATETIME, '24/05/2019', 103), 'GLCHN33-5-008', CONVERT(DATETIME, '21/05/2019', 103), '', '8'; 

COMMIT TRANSACTION TRAN1
END TRY
BEGIN CATCH	
	ROLLBACK TRANSACTION TRAN1
	PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	PRINT '@MaxTransaccionId = ' + CONVERT(VARCHAR(16), @MaxTransaccionId)
	PRINT '@MaxInventarioId = ' + CONVERT(VARCHAR(16), @MaxInventarioId)
	PRINT '@MaxDTId = ' + CONVERT(VARCHAR(16), @MaxDTId)
	PRINT '@MaxItemInventario = ' + CONVERT(VARCHAR(16), @MaxItemInventario)
	PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
	PRINT '@MaxDocTran = ' + CONVERT(VARCHAR(16), @MaxDocTran)
END CATCH
SET XACT_ABORT OFF
                        
