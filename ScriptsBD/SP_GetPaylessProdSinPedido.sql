USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdSinPedido', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdSinPedido
GO

CREATE PROCEDURE dbo.SP_GetPaylessProdSinPedido 
@ClienteId int,
@TiendaId VARCHAR(4)
AS
BEGIN
	SELECT DISTINCT
		D.Barcode,
		D.CP,
		D.Categoria,
		D.IdPAYLESS_ProdPrioriM,
		M.Periodo,
		D.Departamento
	FROM dbo.PAYLESS_ProdPrioriM M WITH(NOLOCK)
	JOIN dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK) 
		ON D.IdPAYLESS_ProdPrioriM = M.Id
	WHERE 
	(
		SELECT top 1 PedM.Id
		FROM dbo.PedidosExternos PedM WITH(NOLOCK)
		JOIN dbo.PedidosDetExternos PedDet WITH(NOLOCK)			
			ON PedDet.PedidoId = PedM.Id
			AND PedM.ClienteID = @ClienteId
			AND PedM.IdEstado = 2
			AND PedM.TiendaId = @TiendaId		
			AND PedDet.CodProducto = D.Barcode
	) IS NULL
	AND M.ClienteId = @ClienteId
	AND D.Barcode like @TiendaId + '%'
	--AND D.Barcode IN (
	--	SELECT 		
	--		ii.CodProducto
	--	FROM wms.dbo.ItemInventario AS ii WITH(NOLOCK)
	--	JOIN wms.dbo.inventario AS i WITH(NOLOCK)
	--		ON i.InventarioID=ii.InventarioID
	--	JOIN wms.dbo.producto AS p WITH(NOLOCK) 
	--		ON p.codproducto=ii.codproducto		
	--	JOIN wms.dbo.DetalleTransacciones AS d1 WITH(NOLOCK)
	--		ON d1.InventarioID=i.InventarioID
	--	JOIN EdiDB.dbo.PAYLESS_Tiendas Ti
	--		ON Ti.TiendaId = @TiendaId
	--	JOIN wms.dbo.transacciones AS T WITH(NOLOCK) 
	--		ON T.TransaccionID=d1.TransaccionID
	--		AND T.BodegaId = Ti.BodegaId
	--	LEFT OUTER JOIN
	--	  (SELECT Sy.InventarioID,
	--			  Sy.ItemInventarioID,
	--			  Sy.CodProducto,
	--			  SUM(ISNULL(Sy.Cantidad, 0)) AS Reservado
	--	   FROM wms.dbo.SysTempSalidas AS Sy WITH(NOLOCK)
	--	   INNER JOIN wms.dbo.Pedido AS Pe WITH(NOLOCK) 
	--			ON Pe.PedidoID = Sy.PedidoID
	--	   GROUP BY Sy.InventarioID,
	--				sy.ItemInventarioID,
	--				Sy.CodProducto) AS Sy_1 ON Sy_1.InventarioID = I.InventarioID
	--	AND Sy_1.ItemInventarioID = II.ItemInventarioID
	--	AND Sy_1.CodProducto = II.CodProducto
	--	WHERE II.existencia > 0
	--	  AND T.IDTipoTransaccion IN ('IN')
	--	  AND T.ClienteID = @ClienteId
	--	  AND p.CodProducto like @TiendaId + '%'
	--	GROUP BY ii.CodProducto	
	--)
END
GO

EXEC EdiDB.dbo.SP_GetPaylessProdSinPedido 1432, '7392'

select distinct Barcode from dbo.PAYLESS_ProdPrioriDet where Barcode like'7375%'
--797
--348