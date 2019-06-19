USE EdiDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('SP_GetWmsGroupDispatchsBills', 'P') IS NOT NULL
	DROP PROC SP_GetWmsGroupDispatchsBills
GO

CREATE PROCEDURE dbo.SP_GetWmsGroupDispatchsBills 
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
		SUM(Dp.Cantidad) Bultos,
		SUM(S.Cantidad) Cantidad,
		P.Observacion,
		P.PedidoID,
		SUBSTRING(Dp.CodProducto, 1, 4) TiendaId,
		SB2.FACT_COMERCIAL,
		T3.TransaccionID,
		D3.Destino
	FROM wms.dbo.Pedido AS P WITH (NOLOCK)
	JOIN wms.dbo.Transacciones AS T3 WITH (NOLOCK)
		ON T3.PedidoID = P.PedidoID
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
	LEFT JOIN wms.dbo.DtllDespacho Dde WITH (NOLOCK)
		ON Dde.TransaccionID = T3.TransaccionID
	LEFT JOIN wms.dbo.Despachos D3 WITH (NOLOCK)
		ON D3.DespachoID = Dde.DespachoID
	LEFT JOIN (
	SELECT SB1.CodProducto, SB1.FACT_COMERCIAL
	FROM (
		SELECT
				MIN(ItemInventarioID) OVER(PARTITION BY Ii.CodProducto) MinItemInventarioId, 
				Ii.CodProducto,
				Ii.ItemInventarioID,
				T.TransaccionID,
				DxT.FACT_COMERCIAL
			FROM wms.dbo.ItemInventario Ii,
			wms.dbo.DetalleTransacciones Dt,
			wms.dbo.Transacciones T,
			wms.dbo.DocumentosxTransaccion DxT
			WHERE Dt.InventarioID = Ii.InventarioID
			AND T.TransaccionID = Dt.TransaccionID
			AND T.IDTipoTransaccion = 'IN'
			AND DxT.TransaccionID = T.TransaccionID
			AND T.ClienteID = @ClienteId
			--AND Ii.Existencia = 0
			GROUP BY Ii.CodProducto,
			Ii.ItemInventarioID,
			T.TransaccionID,
			DxT.FACT_COMERCIAL
		) SB1
		WHERE SB1.MinItemInventarioId = SB1.ItemInventarioID
	) SB2
		ON SB2.CodProducto = Dp.CodProducto
	WHERE P.ClienteID = @ClienteId
	GROUP BY 
	P.ClienteID,
	P.PedidoBarcode,
	CONVERT(VARCHAR, P.fechapedido, 103),
	E.Estatus,
	B.NomBodega,
	R.Regimen,	
	P.Observacion,
	P.PedidoID,
	SUBSTRING(Dp.CodProducto, 1, 4),
	SB2.FACT_COMERCIAL,
	T3.TransaccionID,
	D3.Destino
	ORDER BY P.PedidoID DESC
END
GO

EXEC SP_GetWmsGroupDispatchsBills 1432

SELECT 
		P.ClienteID,
		P.PedidoBarcode,
		CONVERT(VARCHAR, P.fechapedido, 103) FechaPedido,
		E.Estatus,
		B.NomBodega,
		R.Regimen,
		SUM(Dp.Cantidad) Bultos,
		SUM(S.Cantidad) Cantidad,
		P.Observacion,
		P.PedidoID,
		SUBSTRING(Dp.CodProducto, 1, 4) TiendaId,
		SB2.FACT_COMERCIAL,
		T3.TransaccionID,
		D3.Destino
	FROM wms.dbo.Pedido AS P WITH (NOLOCK)
	JOIN wms.dbo.Transacciones AS T3 WITH (NOLOCK)
		ON T3.PedidoID = P.PedidoID
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
	LEFT JOIN wms.dbo.DtllDespacho Dde WITH (NOLOCK)
		ON Dde.TransaccionID = T3.TransaccionID
	LEFT JOIN wms.dbo.Despachos D3 WITH (NOLOCK)
		ON D3.DespachoID = Dde.DespachoID
	LEFT JOIN (
	SELECT SB1.CodProducto, SB1.FACT_COMERCIAL
	FROM (
		SELECT
				MIN(ItemInventarioID) OVER(PARTITION BY Ii.CodProducto) MinItemInventarioId, 
				Ii.CodProducto,
				Ii.ItemInventarioID,
				T.TransaccionID,
				DxT.FACT_COMERCIAL
			FROM wms.dbo.ItemInventario Ii,
			wms.dbo.DetalleTransacciones Dt,
			wms.dbo.Transacciones T,
			wms.dbo.DocumentosxTransaccion DxT
			WHERE Dt.InventarioID = Ii.InventarioID
			AND T.TransaccionID = Dt.TransaccionID
			AND T.IDTipoTransaccion = 'IN'
			AND DxT.TransaccionID = T.TransaccionID
			AND T.ClienteID = 1432
			--AND Ii.Existencia = 0
			GROUP BY Ii.CodProducto,
			Ii.ItemInventarioID,
			T.TransaccionID,
			DxT.FACT_COMERCIAL
		) SB1
		WHERE SB1.MinItemInventarioId = SB1.ItemInventarioID
	) SB2
		ON SB2.CodProducto = Dp.CodProducto
	WHERE P.ClienteID = 1432
	GROUP BY 
	P.ClienteID,
	P.PedidoBarcode,
	CONVERT(VARCHAR, P.fechapedido, 103),
	E.Estatus,
	B.NomBodega,
	R.Regimen,	
	P.Observacion,
	P.PedidoID,
	SUBSTRING(Dp.CodProducto, 1, 4),
	SB2.FACT_COMERCIAL,
	T3.TransaccionID,
	D3.Destino
	ORDER BY P.PedidoID DESC