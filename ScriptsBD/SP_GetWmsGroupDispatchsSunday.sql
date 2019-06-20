USE EdiDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('SP_GetWmsGroupDispatchsSunday', 'P') IS NOT NULL
	DROP PROC SP_GetWmsGroupDispatchsSunday
GO

CREATE PROCEDURE dbo.SP_GetWmsGroupDispatchsSunday
@ClienteId INT,
@DateI VARCHAR(10),
@DateF VARCHAR(10)
AS
BEGIN
	DELETE FROM EdiDB.dbo.ProductoUbicacion WHERE Typ IN (5)
	INSERT INTO EdiDB.dbo.ProductoUbicacion (Typ, CodProducto, NomBodega, Rack, NombreRack)
	SELECT DISTINCT 5, 
		Dp2.CodProducto,
		(SELECT TOP 1 
			D2.Categoria
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) Categoria,
		Dp2.PedidoID,
		(SELECT TOP 1 
			D2.CP
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) CP
	FROM wms.dbo.DtllPedido Dp2 WITH(NOLOCK)			
	JOIN wms.dbo.Pedido Pe WITH(NOLOCK)
		ON Dp2.PedidoId = Pe.PedidoId
		AND Pe.ClienteID = @ClienteId
		AND Pe.fechapedido BETWEEN CONVERT(DATETIME, @DateI, 103) AND CONVERT(DATETIME, @DateF, 103)

	SELECT
	P.PedidoId,
	SUBSTRING(Dp.CodProducto, 1, 4) TiendaId,
	Ti.Descr,

	(
		SELECT COUNT(*)
		FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
		WHERE Pu1.Typ = 5
		AND UPPER(Pu1.NomBodega) = 'DAMAS'
		AND Pu1.Rack = P.PedidoId
		AND SUBSTRING(Pu1.CodProducto, 1, 4) = SUBSTRING(Dp.CodProducto, 1, 4)
	) WomanQty,
	(
		SELECT COUNT(*)
		FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
		WHERE Pu1.Typ = 5
		AND UPPER(Pu1.NomBodega) = 'CABALLEROS'
		AND Pu1.Rack = P.PedidoId		
		AND SUBSTRING(Pu1.CodProducto, 1, 4) = SUBSTRING(Dp.CodProducto, 1, 4)
	) ManQtyEnv,
	(
		SELECT COUNT(*)
		FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
		WHERE Pu1.Typ = 5
		AND UPPER(Pu1.NomBodega) = 'NIÑOS / AS'
		AND Pu1.Rack = P.PedidoId		
		AND SUBSTRING(Pu1.CodProducto, 1, 4) = SUBSTRING(Dp.CodProducto, 1, 4)
	) KidQtyEnv,
	(
		SELECT COUNT(*)
		FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
		WHERE Pu1.Typ = 5
		AND UPPER(Pu1.NomBodega) = 'ACCESORIOS'
		AND Pu1.Rack = P.PedidoId		
		AND SUBSTRING(Pu1.CodProducto, 1, 4) = SUBSTRING(Dp.CodProducto, 1, 4)
	) AccQtyEnv,	
	CONVERT(VARCHAR(10), P.fechapedido, 103) FechaPedido,
	SUM(S.Cantidad) Cantidad,
	D3.Destino
	FROM wms.dbo.Pedido AS P WITH (NOLOCK)
	JOIN wms.dbo.Transacciones T3
		ON T3.PedidoId = P.PedidoId
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
	LEFT JOIN EdiDB.dbo.PayLess_Tiendas Ti
		ON CONVERT(VARCHAR(4), Ti.TiendaId) = SUBSTRING(Dp.CodProducto, 1, 4)
	LEFT JOIN wms.dbo.DtllDespacho Dde WITH (NOLOCK)
		ON Dde.TransaccionID = T3.TransaccionID
	LEFT JOIN wms.dbo.Despachos D3 WITH (NOLOCK)
		ON D3.DespachoID = Dde.DespachoID
	WHERE P.ClienteID = @ClienteId
	AND D3.Destino != 'TEGUCIGALPA'
	AND P.PedidoId IN (
		SELECT DISTINCT Rack FROM EdiDB.dbo.ProductoUbicacion WHERE TYP = 5
	)
	AND Ti.Descr IS NOT NULL
	GROUP BY 
	P.PedidoId,
	SUBSTRING(Dp.CodProducto, 1, 4),
	Ti.Descr,
	P.fechapedido,
	E.Estatus,
	B.NomBodega,
	R.Regimen,	
	P.Observacion,
	P.PedidoID,
	D3.Destino
	ORDER BY P.PedidoID DESC

	DELETE FROM EdiDB.dbo.ProductoUbicacion WHERE Typ IN (5)
END
GO

EXEC SP_GetWmsGroupDispatchsSunday 1432, '09-06-2019', '16-06-2019'
--174

