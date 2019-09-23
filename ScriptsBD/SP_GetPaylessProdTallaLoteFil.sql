USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdTallaLoteFil', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdTallaLoteFil
GO

CREATE PROCEDURE [dbo].SP_GetPaylessProdTallaLoteFil 
@TxtBarcode VARCHAR(16),
@CboProducto VARCHAR(16),
@CboTalla VARCHAR(8),
@CboLote VARCHAR(8),
@CboCategoria VARCHAR(1),
@CodUser VARCHAR(128),
@BodegaId int
AS
BEGIN
	INSERT INTO EdiDB.dbo.WmsProductoExistencia(BodegaId, CodProducto, Existencia, CodUser)
	SELECT 
		t.BodegaId,
		ii.CodProducto,
		SUM(ii.CantidadInicial - isnull(Sy_1.reservado, 0)) AS existencia,
		@CodUser CodUser
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
	GROUP BY t.BodegaId, ii.CodProducto
	ORDER BY t.BodegaId, ii.CodProducto

	SELECT DISTINCT
		D.Barcode,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Categoria,
		D.CP
	FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	LEFT JOIN EdiDB.dbo.WmsProductoExistencia Ex WITH(NOLOCK)
		ON Ex.CodUser = @CodUser AND Ex.CodProducto = D.Barcode AND Ex.BodegaId = @BodegaId
	WHERE D.Barcode like '%' + @TxtBarcode + '%'
	AND D.Producto like @CboProducto + '%'
	AND D.Talla like @CboTalla + '%'
	AND D.Lote like @CboLote + '%'
	AND D.Categoria like (
	CASE @CboCategoria 
		WHEN '0' THEN 'ACCESORIOS' 
		WHEN '1' THEN 'CABALLEROS'
		WHEN '2' THEN 'DAMAS'
		WHEN '3' THEN 'NIÑOS / AS'
		WHEN '' THEN '%'
	END
	)
	AND Ex.CodUser IS NOT NULL
END
GO

exec EdiDB.dbo.SP_GetPaylessProdTallaLoteFil '', '', '', '', '1', 'Admin', 81

select * from EdiDB.dbo.WmsProductoExistencia where CodUser = 'Admin'