USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_GetExistenciasByTienda', 'P') IS NOT NULL
	DROP PROC dbo.SP_GetExistenciasByTienda
GO
CREATE PROCEDURE dbo.SP_GetExistenciasByTienda
@ClienteId int,
@TiendaId VARCHAR(4)
AS
BEGIN
	SELECT 
		t.BodegaId,
		ii.CodProducto,
		SUM(ii.CantidadInicial - isnull(Sy_1.reservado, 0)) AS existencia
	FROM wms.dbo.ItemInventario AS ii WITH(NOLOCK)
	JOIN wms.dbo.inventario AS i WITH(NOLOCK)
		ON i.InventarioID=ii.InventarioID
	JOIN wms.dbo.producto AS p WITH(NOLOCK) 
		ON p.codproducto=ii.codproducto		
	JOIN wms.dbo.DetalleTransacciones AS d1 WITH(NOLOCK)
		ON d1.InventarioID=i.InventarioID
	JOIN EdiDB.dbo.PAYLESS_Tiendas Ti
		ON Ti.TiendaId = @TiendaId
	JOIN wms.dbo.transacciones AS T WITH(NOLOCK) 
		ON T.TransaccionID=d1.TransaccionID
		AND T.BodegaId = Ti.BodegaId
	LEFT OUTER JOIN
	  (SELECT Sy.InventarioID,
			  Sy.ItemInventarioID,
			  Sy.CodProducto,
			  SUM(ISNULL(Sy.Cantidad, 0)) AS Reservado
	   FROM wms.dbo.SysTempSalidas AS Sy WITH(NOLOCK)
	   INNER JOIN wms.dbo.Pedido AS Pe WITH(NOLOCK) 
			ON Pe.PedidoID = Sy.PedidoID
	   GROUP BY Sy.InventarioID,
				sy.ItemInventarioID,
				Sy.CodProducto) AS Sy_1 ON Sy_1.InventarioID = I.InventarioID
	AND Sy_1.ItemInventarioID = II.ItemInventarioID
	AND Sy_1.CodProducto = II.CodProducto
	WHERE II.existencia > 0
	  AND T.IDTipoTransaccion IN ('IN')
	  AND T.ClienteID = @ClienteId
	  AND p.CodProducto like @TiendaId + '%'
	GROUP BY t.BodegaId, ii.CodProducto
	ORDER BY t.BodegaId, ii.CodProducto
END

EXEC EdiDb.dbo.SP_GetExistenciasByTienda 1432, '7386'
--657