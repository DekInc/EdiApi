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
