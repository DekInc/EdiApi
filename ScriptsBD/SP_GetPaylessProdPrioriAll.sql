USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdPrioriAll', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdPrioriAll
GO

CREATE PROCEDURE dbo.SP_GetPaylessProdPrioriAll
@ClienteId VARCHAR(4)
AS
BEGIN
	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = 'InvAuto'
	INSERT INTO EdiDB.dbo.WmsProductoExistencia(BodegaId, CodProducto, Existencia, CodUser)
	SELECT 
		t.BodegaId,
		ii.CodProducto,
		SUM(ii.CantidadInicial - isnull(Sy_1.reservado, 0)) AS existencia,
		'InvAuto' CodUser
	FROM wms.dbo.ItemInventario AS ii WITH(NOLOCK)
	JOIN wms.dbo.inventario AS i WITH(NOLOCK)
		ON i.InventarioID=ii.InventarioID
	JOIN wms.dbo.producto AS p WITH(NOLOCK) 
		ON p.codproducto=ii.codproducto		
	JOIN wms.dbo.DetalleTransacciones AS d1 WITH(NOLOCK)
		ON d1.InventarioID=i.InventarioID
	JOIN wms.dbo.transacciones AS T WITH(NOLOCK) 
		ON t.TransaccionID=d1.TransaccionID		
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
	WHERE II.existencia > 0 AND 
	  T.IDTipoTransaccion IN ('IN')
	  AND T.ClienteID = @ClienteId
	GROUP BY t.BodegaId, ii.CodProducto
	ORDER BY t.BodegaId, ii.CodProducto

	SELECT DISTINCT
		D.Id,
		D.IdPAYLESS_ProdPrioriM,
		null OID,
		D.Barcode,
		null Estado,
		null Pri,
		null PoolP,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Categoria,
		D.Departamento,
		D.CP,
		null Pickeada,
		null Etiquetada,
		null Preinspeccion,
		null Cargada,
		0 M3,
		0 Peso,
		D.IdTransporte,
		T.Transporte
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)	
	JOIN PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	JOIN PAYLESS_ProdPrioriM M WITH(NOLOCK)
		ON M.Id = D.IdPAYLESS_ProdPrioriM
	LEFT JOIN dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = D.IdTransporte
	WHERE M.ClienteId = @ClienteId
	AND D.Barcode like '7650%'
	AND Wpe.CodUser = 'InvAuto'

	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = 'InvAuto'
END
GO

EXEC SP_GetPaylessProdPrioriAll 385
--379

SELECT * from EdiDB.dbo.WmsProductoExistencia where CodUser = 'Admin' AND Existencia = 1 AND CodProducto like '7650%' --293
