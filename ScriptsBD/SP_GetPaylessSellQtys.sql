USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_GetPaylessSellQtys', 'P') IS NOT NULL
	DROP PROC dbo.SP_GetPaylessSellQtys
GO
CREATE PROCEDURE dbo.SP_GetPaylessSellQtys
@ClienteId int,
@TiendaId VARCHAR(4),
@CodUser VARCHAR(128)
AS
BEGIN
	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = @CodUser
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
	JOIN EdiDB.dbo.PAYLESS_Tiendas Ti WITH(NOLOCK)
		ON Ti.TiendaId = @TiendaId
	JOIN wms.dbo.transacciones AS T WITH(NOLOCK)
		ON t.TransaccionID=d1.TransaccionID
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

	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = @CodUser
	AND CodProducto IN (
		SELECT DISTINCT
			CodProducto
		FROM EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
		JOIN EdiDB.dbo.PedidosDetExternos Pde WITH(NOLOCK)
			ON Pde.PedidoId = Pe.Id
		WHERE Pe.PedidoWMS IS NULL
	)

	SELECT DISTINCT
		D.Barcode,
		D.Categoria,
		D.Cp,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Departamento,
		3 Typ
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser
	AND (
		D.Producto IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
		OR D.Talla IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
		OR D.Lote IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
		OR D.Categoria IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
		OR D.Departamento IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	UNION
	SELECT DISTINCT
		D.Barcode,
		D.Categoria,
		D.Cp,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Departamento,
		2 Typ
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser	
	AND D.CP IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	UNION
	SELECT DISTINCT
		D.Barcode,
		D.Categoria,
		D.Cp,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Departamento,
		1 Typ
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	--SELECT DISTINCT
	--	D.Barcode,
	--	D.Categoria,
	--	D.Cp,
	--	D.Producto,
	--	D.Talla,
	--	D.Lote,
	--	D.Departamento,
	--	3
	--FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	--JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	--	ON Wpe.CodProducto = D.Barcode
	--WHERE Wpe.CodUser = @CodUser
END

EXEC EdiDb.dbo.SP_GetPaylessSellQtys 1432, '7393', 'Admin'
SELECT * FROM EdiDB.dbo.PaylessPedidosCpT
SELECT * from EdiDB.dbo.WmsProductoExistencia where CodUser = 'Admin' AND CodProducto like '7386%'



