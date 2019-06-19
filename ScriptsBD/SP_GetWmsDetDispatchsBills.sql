USE EdiDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('SP_GetWmsDetDispatchsBills', 'P') IS NOT NULL
	DROP PROC SP_GetWmsDetDispatchsBills
GO

CREATE PROCEDURE dbo.SP_GetWmsDetDispatchsBills 
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
		Dp.CodProducto,
		P.PedidoID,
		SB2.FACT_COMERCIAL,
		(SELECT top 1 D.Categoria from EdiDb.dbo.PAYLESS_ProdPrioriDet D where D.Barcode = Dp.CodProducto ) Categoria,
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
	ORDER BY P.PedidoID, Dp.CodProducto DESC
END
GO

EXEC SP_GetWmsDetDispatchsBills 1432
--25011