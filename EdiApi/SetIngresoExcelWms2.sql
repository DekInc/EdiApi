13.9621024
SET XACT_ABORT ON

BEGIN TRANSACTION TRAN1

DECLARE @TransaccionID INT;

DECLARE @MaxPedidoId INT;

DECLARE @MaxPedidoDet INT;

DECLARE @MaxDetTran INT;

DECLARE @MaxDetItemTran INT;

BEGIN TRY

SELECT @TransaccionID = ISNULL(MAX(TransaccionId), 0) + 1 FROM dbo.Transacciones; 
INSERT INTO dbo.Transacciones(TransaccionID, NoTransaccion, IDTipoTransaccion, FechaTransaccion, BodegaID, RegimenID, ClienteID, TipoIngreso, Observacion, Usuariocrea, Fechacrea, EstatusID) SELECT @TransaccionID, 'SA' + RIGHT('00000'+ CONVERT(VARCHAR(64), @TransaccionID), 5), 'SA', CONVERT(DATETIME, '19/06/2019', 103), 82, 2, 1432, 'XL', '', 'Hilmer', CONVERT(DATETIME, '18/06/2019', 103), 4; 
SELECT @MaxPedidoId = ISNULL(MAX(PedidoId), 0) + 1 FROM dbo.Pedido; 
INSERT INTO dbo.Pedido(PedidoID, fechapedido, ClienteID, TipoPedido, FechaRequerido, EstatusID, Observacion, BodegaID, RegimenID, PedidoBarcode) SELECT @MaxPedidoId, CONVERT(DATETIME, '18/06/2019', 103), 1432, 'XL', CONVERT(DATETIME, '19/06/2019', 103), 8, 'SALIDA GENERADA DE XLS Intranet', 82, 2, 'PD' + RIGHT('00000'+ CONVERT(VARCHAR(64), @MaxPedidoId), 5); 
UPDATE dbo.Transacciones SET PedidoId = @MaxPedidoId WHERE TransaccionID = @TransaccionID; 
SELECT @MaxPedidoDet = ISNULL(MAX(DtllPedidoId), 0) + 1 FROM dbo.DtllPedido; 
INSERT INTO dbo.DtllPedido(DtllPedidoId, PedidoId, Cantidad, CodProducto) SELECT @MaxPedidoDet, @MaxPedidoId, 1, '7376840999'; 
INSERT INTO dbo.SysTempSalidas(TransaccionId, PedidoId, InventarioId, DtllPedidoId, ItemInventarioId, CodProducto, Cantidad, Precio, Fecha, Usuario, Lote) SELECT @TransaccionID, @MaxPedidoId, 1849246, @MaxPedidoDet, 1830578, '7376840999', 1, 43.19, CONVERT(DATETIME, '18/06/2019', 103), 'HCAMPOS', '177071'; 
SELECT @MaxPedidoDet = ISNULL(MAX(DtllPedidoId), 0) + 1 FROM dbo.DtllPedido; 
INSERT INTO dbo.DtllPedido(DtllPedidoId, PedidoId, Cantidad, CodProducto) SELECT @MaxPedidoDet, @MaxPedidoId, 1, '7376841012'; 
INSERT INTO dbo.SysTempSalidas(TransaccionId, PedidoId, InventarioId, DtllPedidoId, ItemInventarioId, CodProducto, Cantidad, Precio, Fecha, Usuario, Lote) SELECT @TransaccionID, @MaxPedidoId, 1849248, @MaxPedidoDet, 1830580, '7376841012', 1, 43.19, CONVERT(DATETIME, '18/06/2019', 103), 'HCAMPOS', '182758'; 
SELECT @MaxPedidoDet = ISNULL(MAX(DtllPedidoId), 0) + 1 FROM dbo.DtllPedido; 
INSERT INTO dbo.DtllPedido(DtllPedidoId, PedidoId, Cantidad, CodProducto) SELECT @MaxPedidoDet, @MaxPedidoId, 1, '7376841031'; 
INSERT INTO dbo.SysTempSalidas(TransaccionId, PedidoId, InventarioId, DtllPedidoId, ItemInventarioId, CodProducto, Cantidad, Precio, Fecha, Usuario, Lote) SELECT @TransaccionID, @MaxPedidoId, 1849254, @MaxPedidoDet, 1830586, '7376841031', 1, 43.19, CONVERT(DATETIME, '18/06/2019', 103), 'HCAMPOS', '174042'; 
SELECT @MaxDetTran = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, rack, embalaje, IsEscaneado) SELECT @MaxDetTran, @TransaccionID, 1849246, 1, 1, 43.19, 11977, 'CS', 0; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @TransaccionID, @MaxDetTran, 1830578, 1, 43.19, 11977; 
SELECT @MaxDetTran = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, rack, embalaje, IsEscaneado) SELECT @MaxDetTran, @TransaccionID, 1849248, 1, 1, 43.19, 11977, 'CS', 0; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @TransaccionID, @MaxDetTran, 1830580, 1, 43.19, 11977; 
SELECT @MaxDetTran = ISNULL(MAX(DtllTrnsaccionId), 0) + 1 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, rack, embalaje, IsEscaneado) SELECT @MaxDetTran, @TransaccionID, 1849254, 1, 1, 43.19, 11977, 'CS', 0; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 1 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @TransaccionID, @MaxDetTran, 1830586, 1, 43.19, 11977; 

                ROLLBACK TRANSACTION TRAN1 --COMMIT REMOVIDO POR SI LO QUIEREN EJECUTAR...
                END TRY
                BEGIN CATCH	
	                ROLLBACK TRANSACTION TRAN1
	                PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	                PRINT '@MaxPedidoId = ' + CONVERT(VARCHAR(16), @MaxPedidoId)
	                PRINT '@MaxPedidoDet = ' + CONVERT(VARCHAR(16), @MaxPedidoDet)
	                PRINT '@MaxDetTran = ' + CONVERT(VARCHAR(16), @MaxDetTran)
	                PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
                END CATCH
                SET XACT_ABORT OFF
                

13.9621024
SET XACT_ABORT ON

--BEGIN TRANSACTION TRAN1

DECLARE @MaxTransaccionId INT;

DECLARE @MaxInventarioId INT;

DECLARE @MaxDTId INT;

DECLARE @MaxItemInventario INT;

DECLARE @MaxDetItemTran INT;

DECLARE @MaxDocTran INT;

BEGIN TRY

SET @MaxTransaccionId = 119515; 

INSERT INTO dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida) SELECT 'HilmerTest1', 'DAMAS', 1, 1432, 1, 10, 0, CONVERT(DATETIME, '2019-06-28 11:54', 120), 'INGRESOS DESDE INTRANET', 0, 1, '0'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 5 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '2019-06-28 11:54', 120), 1432, 'DAMAS', 1, , 1, 3.65, 0.03714, 2, 0, 70, 1, 1, 1, ; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 5 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 1, 0, CONVERT(DATETIME, '2019-06-28 00:00', 120), , 'CS', 0; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 5 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, 'HilmerTest1', 1, , 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '2019-06-28 00:00', 120), 'DAMAS', 1, 1, 1, '7365 - Payless Shoe Source - Pricesmart SPS', 166, '181477', '0', 'CLB', 'SMLU 792180-6', '564395'; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 5 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 1, , ; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerTest1', 23, ''; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerTest1', 15, 'CLB'; 
SELECT @MaxDocTran = ISNULL(MAX(IddocxTransaccion), 0) + 5 FROM dbo.DocumentosxTransaccion; 
INSERT INTO dbo.DocumentosxTransaccion(IDDocxTransaccion, transaccionid, fecha, INFORME_ALMACEN, FE_INFORME_ALMACEN, IM_5, ORDEN_COMPRA) SELECT @MaxDocTran, @MaxTransaccionId, CONVERT(DATETIME, '2019-06-28 11:54', 120), 'Hilmer12121212', CONVERT(DATETIME, '2019-06-28 00:00', 120), '', ''; 

--COMMIT TRANSACTION TRAN1
END TRY
BEGIN CATCH	
	--ROLLBACK TRANSACTION TRAN1
	PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	PRINT '@MaxTransaccionId = ' + CONVERT(VARCHAR(16), @MaxTransaccionId)
	PRINT '@MaxInventarioId = ' + CONVERT(VARCHAR(16), @MaxInventarioId)
	PRINT '@MaxDTId = ' + CONVERT(VARCHAR(16), @MaxDTId)
	PRINT '@MaxItemInventario = ' + CONVERT(VARCHAR(16), @MaxItemInventario)
	PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
	PRINT '@MaxDocTran = ' + CONVERT(VARCHAR(16), @MaxDocTran)
END CATCH
SET XACT_ABORT OFF
                        

SET XACT_ABORT ON

--BEGIN TRANSACTION TRAN1

DECLARE @MaxTransaccionId INT;

DECLARE @MaxInventarioId INT;

DECLARE @MaxDTId INT;

DECLARE @MaxItemInventario INT;

DECLARE @MaxDetItemTran INT;

DECLARE @MaxDocTran INT;

BEGIN TRY

SET @MaxTransaccionId = 119518; 

INSERT INTO dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida) SELECT 'HilmerA1', 'CABALLEROS', 1, 1432, 1, 10, 0, CONVERT(DATETIME, '2019-06-28 12:19', 120), 'INGRESOS DESDE INTRANET', 0, 1, '0'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 5 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '2019-06-28 12:19', 120), 1432, 'CABALLEROS', 1, 44.68, 1, 5.95, 0.06631, 2, 0, 70, 1, 1, 1, 12127; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 5 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 1, 44.68, CONVERT(DATETIME, '2019-06-14 00:00', 120), 12127, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 5 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, 'HilmerA1', 1, 44.68, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '2019-06-14 00:00', 120), 'CABALLEROS', 1, 1, 1, '7365 - PAYLESS SHOE SOURCE', 166, '174077', 'HN190528PB192011', 'DPB', ' FCIU 845795-0', '565301'; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 5 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 1, 44.68, 12127; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerA1', 23, '44.68'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerA1', 15, 'DPB'; 
UPDATE dbo.Transacciones SET EstatusId = 6 WHERE TransaccionID = @MaxTransaccionId; 
SELECT @MaxDocTran = ISNULL(MAX(IddocxTransaccion), 0) + 5 FROM dbo.DocumentosxTransaccion; 
INSERT INTO dbo.DocumentosxTransaccion(IDDocxTransaccion, transaccionid, fecha, INFORME_ALMACEN, FE_INFORME_ALMACEN, IM_5, ORDEN_COMPRA) SELECT @MaxDocTran, @MaxTransaccionId, CONVERT(DATETIME, '2019-06-28 12:19', 120), 'GLCHIL3-06-022', CONVERT(DATETIME, '2019-06-14 00:00', 120), '', 'HN190528PB192011'; 

--COMMIT TRANSACTION TRAN1
END TRY
BEGIN CATCH	
	--ROLLBACK TRANSACTION TRAN1
	PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	PRINT '@MaxTransaccionId = ' + CONVERT(VARCHAR(16), @MaxTransaccionId)
	PRINT '@MaxInventarioId = ' + CONVERT(VARCHAR(16), @MaxInventarioId)
	PRINT '@MaxDTId = ' + CONVERT(VARCHAR(16), @MaxDTId)
	PRINT '@MaxItemInventario = ' + CONVERT(VARCHAR(16), @MaxItemInventario)
	PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
	PRINT '@MaxDocTran = ' + CONVERT(VARCHAR(16), @MaxDocTran)
END CATCH
SET XACT_ABORT OFF
                        

SET XACT_ABORT ON

--BEGIN TRANSACTION TRAN2

DECLARE @MaxTransaccionId INT;

DECLARE @MaxInventarioId INT;

DECLARE @MaxDTId INT;

DECLARE @MaxItemInventario INT;

DECLARE @MaxDetItemTran INT;

DECLARE @MaxDocTran INT;

BEGIN TRY

SET @MaxTransaccionId = 119518; 

INSERT INTO dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida) SELECT 'HilmerA2', 'NIÑOS / AS', 1, 1432, 1, 10, 0, CONVERT(DATETIME, '2019-06-28 12:20', 120), 'INGRESOS DESDE INTRANET', 0, 1, '0'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 5 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '2019-06-28 12:20', 120), 1432, 'NIÑOS / AS', 1, 44.68, 1, 4, 0.11, 2, 0, 70, 1, 1, 1, 12127; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 5 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 1, 44.68, CONVERT(DATETIME, '2019-06-14 00:00', 120), 12127, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 5 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, 'HilmerA2', 1, 44.68, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '2019-06-14 00:00', 120), 'NIÑOS / AS', 1, 1, 1, '7365 - PAYLESS SHOE SOURCE', 166, '3023', 'HN190528PB192011', 'DDN', ' FCIU 845795-0', '572408'; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 5 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 1, 44.68, 12127; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerA2', 23, '44.68'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerA2', 15, 'DDN'; 
UPDATE dbo.Transacciones SET EstatusId = 6 WHERE TransaccionID = @MaxTransaccionId; 

--COMMIT TRANSACTION TRAN2
END TRY
BEGIN CATCH	
	--ROLLBACK TRANSACTION TRAN2
	PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	PRINT '@MaxTransaccionId = ' + CONVERT(VARCHAR(16), @MaxTransaccionId)
	PRINT '@MaxInventarioId = ' + CONVERT(VARCHAR(16), @MaxInventarioId)
	PRINT '@MaxDTId = ' + CONVERT(VARCHAR(16), @MaxDTId)
	PRINT '@MaxItemInventario = ' + CONVERT(VARCHAR(16), @MaxItemInventario)
	PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
	PRINT '@MaxDocTran = ' + CONVERT(VARCHAR(16), @MaxDocTran)
END CATCH
SET XACT_ABORT OFF
                        

SET XACT_ABORT ON

--BEGIN TRANSACTION TRAN3

DECLARE @MaxTransaccionId INT;

DECLARE @MaxInventarioId INT;

DECLARE @MaxDTId INT;

DECLARE @MaxItemInventario INT;

DECLARE @MaxDetItemTran INT;

DECLARE @MaxDocTran INT;

BEGIN TRY

SET @MaxTransaccionId = 119518; 

INSERT INTO dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida) SELECT 'HilmerA3', 'DAMAS', 1, 1432, 1, 10, 0, CONVERT(DATETIME, '2019-06-28 12:20', 120), 'INGRESOS DESDE INTRANET', 0, 1, '0'; 
SELECT @MaxInventarioId = ISNULL(MAX(InventarioId), 0) + 5 FROM dbo.Inventario; 
INSERT INTO dbo.Inventario(InventarioID, Barcode, FechaCreacion, ClienteID, Descripcion, Declarado, Valor, Articulos, Peso, Volumen, EstatusID, IsAgranel, TipoBulto, existencia, auditado, cantidadinicial, Rack)SELECT @MaxInventarioId, 'BRC' + RIGHT('0000000'+ CONVERT(VARCHAR(64), @MaxInventarioId), 7), CONVERT(DATETIME, '2019-06-28 12:20', 120), 1432, 'DAMAS', 1, 44.68, 1, 4.29, 0.04347, 2, 0, 70, 1, 1, 1, 12127; 
SELECT @MaxDTId = ISNULL(MAX(DtllTrnsaccionId), 0) + 5 FROM dbo.DetalleTransacciones; 
INSERT INTO dbo.DetalleTransacciones(DtllTrnsaccionID, TransaccionID, InventarioID, Conteo, Cantidad, Valor, fechaitem, rack, embalaje, IsEscaneado) SELECT @MaxDTId, @MaxTransaccionId, @MaxInventarioId, 1, 1, 44.68, CONVERT(DATETIME, '2019-06-14 00:00', 120), 12127, 'CS', 1; 
SELECT @MaxItemInventario = ISNULL(MAX(ItemInventarioId), 0) + 5 FROM dbo.ItemInventario; 
INSERT INTO dbo.ItemInventario(ItemInventarioID, InventarioID, CodProducto, Declarado, Precio, Observacion, fechaitem, descripcion, auditado, existencia, CantidadInicial, cod_equivale, pais_orig, lote, numero_oc, modelo, color, estilo) SELECT @MaxItemInventario, @MaxInventarioId, 'HilmerA3', 1, 44.68, 'INGRESOS DESDE INTRANET', CONVERT(DATETIME, '2019-06-14 00:00', 120), 'DAMAS', 1, 1, 1, '7365 - PAYLESS SHOE SOURCE', 166, '160429', 'HN190528PB192011', 'QZM', ' FCIU 845795-0', '564083'; 
SELECT @MaxDetItemTran = ISNULL(MAX(DtllItemTransaccionId), 0) + 5 FROM dbo.DtllItemTransaccion; 
INSERT INTO dbo.DtllItemTransaccion(DtllItemTransaccionID, TransaccionID, DtllTransaccionID, ItemInventarioID, Cantidad, Precio, RACK) SELECT @MaxDetItemTran, @MaxTransaccionId, @MaxDTId, @MaxItemInventario, 1, 44.68, 12127; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerA3', 23, '44.68'; 
INSERT INTO dbo.ItemParamaetroxProducto(InventarioID, ItemInventarioID, CodProducto, ParametroID, ValParametro) SELECT @MaxInventarioId, @MaxItemInventario, 'HilmerA3', 15, 'QZM'; 
UPDATE dbo.Transacciones SET EstatusId = 6 WHERE TransaccionID = @MaxTransaccionId; 

--COMMIT TRANSACTION TRAN3
END TRY
BEGIN CATCH	
	--ROLLBACK TRANSACTION TRAN3
	PRINT 'ERROR, LINEA: ' + CONVERT(VARCHAR(16), ERROR_LINE()) + ' - ' + ERROR_MESSAGE()
	PRINT '@MaxTransaccionId = ' + CONVERT(VARCHAR(16), @MaxTransaccionId)
	PRINT '@MaxInventarioId = ' + CONVERT(VARCHAR(16), @MaxInventarioId)
	PRINT '@MaxDTId = ' + CONVERT(VARCHAR(16), @MaxDTId)
	PRINT '@MaxItemInventario = ' + CONVERT(VARCHAR(16), @MaxItemInventario)
	PRINT '@MaxDetItemTran = ' + CONVERT(VARCHAR(16), @MaxDetItemTran)
	PRINT '@MaxDocTran = ' + CONVERT(VARCHAR(16), @MaxDocTran)
END CATCH
SET XACT_ABORT OFF
                        

