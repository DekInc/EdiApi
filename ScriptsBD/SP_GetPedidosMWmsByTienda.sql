USE EdiDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('SP_GetPedidosMWmsByTienda', 'P') IS NOT NULL
	DROP PROC SP_GetPedidosMWmsByTienda
GO

CREATE PROCEDURE [dbo].SP_GetPedidosMWmsByTienda 
@ClienteId INT,
@TiendaId VARCHAR(4)
AS
BEGIN
	DECLARE @Cont INT = 0
	SELECT @Cont = COUNT(*)	
	FROM EdiDB.dbo.PedidosDetExternos Ped WITH (NOLOCK) 
	JOIN EdiDB.dbo.PedidosExternos Pe WITH (NOLOCK) 
		ON Pe.Id = Ped.PedidoId AND Pe.ClienteID = @ClienteId
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH (NOLOCK) 
		ON D.Barcode = Ped.CodProducto
	WHERE Ped.CodProducto like @TiendaId + '%'
	AND D.CP != ''

	IF (@Cont > 0)
	BEGIN
		SELECT DISTINCT 
			P.ClienteID,
			P.PedidoBarcode,
			CONVERT(VARCHAR, P.fechapedido, 103) FechaPedido,
			P.fechapedido fechapedidoDt,
			E.Estatus,
			B.NomBodega,
			R.Regimen,
			P.Observacion,
			P.PedidoID,
			(SELECT COUNT(*) 
			FROM wms.dbo.DtllPedido Dp2 WITH (NOLOCK)
			WHERE Dp2.PedidoID = P.PedidoID
			AND Dp2.CodProducto like @TiendaId + '%') Total
		FROM wms.dbo.Pedido AS P WITH (NOLOCK)
		INNER JOIN wms.dbo.Estatus AS E WITH (NOLOCK) 
			ON E.EstatusID = P.EstatusID
		INNER JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) 
			ON B.BodegaID = P.BodegaID
		INNER JOIN wms.dbo.Regimen AS R WITH (NOLOCK) 
			ON R.IDRegimen = P.RegimenID
		INNER JOIN wms.dbo.DtllPedido AS Dp WITH (NOLOCK) 
			ON Dp.PedidoID = P.PedidoID 
			AND Dp.CodProducto like @TiendaId + '%'		
		WHERE ClienteID = @ClienteId
		AND Dp.CodProducto IN (
			SELECT DISTINCT Ped.CodProducto
			FROM EdiDB.dbo.PedidosDetExternos Ped WITH (NOLOCK) 
			JOIN EdiDB.dbo.PedidosExternos Pe WITH (NOLOCK) 
				ON Pe.Id = Ped.PedidoId
			JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH (NOLOCK) 
				ON D.Barcode = Ped.CodProducto
			WHERE Ped.CodProducto like @TiendaId + '%'
			AND D.CP != ''
		)
		ORDER BY P.fechapedido DESC
	END
	ELSE
	BEGIN
		SELECT DISTINCT 
			P.ClienteID,
			P.PedidoBarcode,
			CONVERT(VARCHAR, P.fechapedido, 103) FechaPedido,
			P.fechapedido fechapedidoDt,
			E.Estatus,
			B.NomBodega,
			R.Regimen,
			P.Observacion,
			P.PedidoID,
			(SELECT COUNT(*) 
			FROM wms.dbo.DtllPedido Dp2 WITH (NOLOCK)
			WHERE Dp2.PedidoID = P.PedidoID
			AND Dp2.CodProducto like @TiendaId + '%') Total
		FROM wms.dbo.Pedido AS P WITH (NOLOCK)
		INNER JOIN wms.dbo.Estatus AS E WITH (NOLOCK) 
			ON E.EstatusID = P.EstatusID
		INNER JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) 
			ON B.BodegaID = P.BodegaID
		INNER JOIN wms.dbo.Regimen AS R WITH (NOLOCK) 
			ON R.IDRegimen = P.RegimenID
		INNER JOIN wms.dbo.DtllPedido AS Dp WITH (NOLOCK) 
			ON Dp.PedidoID = P.PedidoID 
			AND Dp.CodProducto like @TiendaId + '%'		
		WHERE ClienteID = @ClienteId
		ORDER BY P.fechapedido DESC
	END
END
GO

--EXEC SP_GetPedidosMWmsByTienda 1432, '7372'

