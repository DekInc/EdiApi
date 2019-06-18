USE EdiDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('SP_GetWmsGroupDispatchs', 'P') IS NOT NULL
	DROP PROC SP_GetWmsGroupDispatchs
GO

CREATE PROCEDURE dbo.SP_GetWmsGroupDispatchs 
@ClienteId INT
AS
BEGIN
	SELECT 
	P.ClienteID,
	P.PedidoBarcode,
	CONVERT(VARCHAR, P.fechapedido, 103) FechaPedido,
	E.Estatus,
	B.NomBodega,
	R.Regimen,
	--SUM(Dp.Cantidad) Bultos,
	--SUM(S.Cantidad) Cantidad,
	P.Observacion,
	P.PedidoID,
	SUBSTRING(Dp.CodProducto, 1, 4) TiendaId,
	(SELECT COUNT(*) FROM EdiDB.dbo.PedidosExternos WHERE PedidoWMS = P.PedidoID) PedidoWeb
	FROM wms.dbo.Pedido AS P WITH (NOLOCK)
	JOIN wms.dbo.Estatus AS E WITH (NOLOCK) 
		ON E.EstatusID = P.EstatusID
	JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) 
		ON B.BodegaID = P.BodegaID
	JOIN wms.dbo.Regimen AS R WITH (NOLOCK) 
		ON R.IDRegimen = P.RegimenID
	JOIN wms.dbo.DtllPedido AS Dp WITH (NOLOCK) 
		ON Dp.PedidoID = P.PedidoID
	JOIN wms.dbo.SysTempSalidas S WITH (NOLOCK)
		ON S.DtllPedidoID = Dp.DtllPedidoID
	WHERE ClienteID = @ClienteId
	--GROUP BY 
	--P.ClienteID,
	--P.PedidoBarcode,
	--CONVERT(VARCHAR, P.fechapedido, 103),
	--E.Estatus,
	--B.NomBodega,
	--R.Regimen,	
	--P.Observacion,
	--P.PedidoID,
	--SUBSTRING(Dp.CodProducto, 1, 4)
	ORDER BY P.PedidoID DESC
END
GO

EXEC SP_GetWmsGroupDispatchs 1432




SELECT 
	P.ClienteID,
	P.PedidoBarcode,
	CONVERT(VARCHAR, P.fechapedido, 103) FechaPedido,
	E.Estatus,
	B.NomBodega,
	R.Regimen,
	--SUM(Dp.Cantidad) Bultos,
	--SUM(S.Cantidad) Cantidad,
	P.Observacion,
	P.PedidoID,
	SUBSTRING(Dp.CodProducto, 1, 4) TiendaId,
	Dp.CodProducto,
	 Dp.DtllPedidoID,
	(SELECT COUNT(*) FROM EdiDB.dbo.PedidosExternos WHERE PedidoWMS = P.PedidoID) PedidoWeb
	FROM wms.dbo.Pedido AS P WITH (NOLOCK)
	JOIN wms.dbo.Estatus AS E WITH (NOLOCK) 
		ON E.EstatusID = P.EstatusID
	JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) 
		ON B.BodegaID = P.BodegaID
	JOIN wms.dbo.Regimen AS R WITH (NOLOCK) 
		ON R.IDRegimen = P.RegimenID
	JOIN wms.dbo.DtllPedido AS Dp WITH (NOLOCK) 
		ON Dp.PedidoID = P.PedidoID
	JOIN wms.dbo.SysTempSalidas S WITH (NOLOCK)
		ON S.DtllPedidoID = Dp.DtllPedidoID
	WHERE ClienteID = 1432
	AND P.PedidoBarcode = 'PD70644'
	--GROUP BY 
	--P.ClienteID,
	--P.PedidoBarcode,
	--CONVERT(VARCHAR, P.fechapedido, 103),
	--E.Estatus,
	--B.NomBodega,
	--R.Regimen,	
	--P.Observacion,
	--P.PedidoID,
	--SUBSTRING(Dp.CodProducto, 1, 4)
	ORDER BY P.PedidoID DESC