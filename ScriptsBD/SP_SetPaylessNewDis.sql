USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_SetPaylessNewDis', 'P') IS NOT NULL
	DROP PROC dbo.SP_SetPaylessNewDis
GO
CREATE PROCEDURE dbo.SP_SetPaylessNewDis
@PedidoId int,
@CodUser VARCHAR(128)
AS
BEGIN
	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = @CodUser + 'Cp'

	INSERT INTO EdiDB.dbo.WmsProductoExistencia (BodegaId, CodProducto, Existencia, CodUser)
	SELECT DISTINCT 
		t.BodegaId,
		ii.CodProducto,
		1,
		@CodUser + 'Cp'
	FROM wms.dbo.ItemInventario AS ii WITH(NOLOCK)	
	JOIN wms.dbo.inventario AS i WITH(NOLOCK)
		ON i.InventarioID=ii.InventarioID
	JOIN wms.dbo.producto AS p WITH(NOLOCK) 
		ON p.codproducto=ii.codproducto		
	JOIN wms.dbo.DetalleTransacciones AS d1 WITH(NOLOCK)
		ON d1.InventarioID=i.InventarioID
	JOIN EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
		ON Pe.Id = @PedidoId
	JOIN EdiDB.dbo.PAYLESS_Tiendas Ti
		ON Ti.TiendaId = Pe.TiendaId
	JOIN wms.dbo.transacciones AS T WITH(NOLOCK) 
		ON T.TransaccionID=d1.TransaccionID
		AND (T.BodegaId = Ti.BodegaId
			OR Ti.BodegaId IS NULL
		)
		AND Pe.ClienteID = T.ClienteID
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
	  AND p.CodProducto like CONVERT(VARCHAR(4), Pe.TiendaId) + '%'
	GROUP BY t.BodegaId, ii.CodProducto
	ORDER BY t.BodegaId, ii.CodProducto

	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = @CodUser  + 'Cp'
	AND CodProducto IN (
		SELECT Ped.CodProducto
		FROM EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
		JOIN EdiDB.dbo.PedidosDetExternos Ped WITH(NOLOCK)
			ON Ped.PedidoId = Pe.Id
		WHERE Pe.PedidoWMS IS NULL
	)

	UPDATE EdiDB.dbo.PedidosExternos
	SET TotalCp = (
		SELECT COUNT(*) FROM (
		SELECT DISTINCT
		D.Barcode
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	) SB1
	) WHERE Id = @PedidoId

	INSERT INTO EdiDB.dbo.PedidosDetExternos(PedidoId, CodProducto, CantPedir)
	SELECT DISTINCT
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	UNION
	SELECT DISTINCT TOP (SELECT Pe1.WomanQty FROM EdiDb.dbo.PedidosExternos Pe1 WHERE Pe1.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'DAMAS'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	UNION
	SELECT DISTINCT TOP (SELECT Pe2.ManQty FROM EdiDb.dbo.PedidosExternos Pe2 WHERE Pe2.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'CABALLEROS'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	UNION
	SELECT DISTINCT TOP (SELECT Pe2.KidQty FROM EdiDb.dbo.PedidosExternos Pe2 WHERE Pe2.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'NIÑOS / AS'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	UNION
	SELECT DISTINCT TOP (SELECT Pe2.AccQty FROM EdiDb.dbo.PedidosExternos Pe2 WHERE Pe2.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'ACCESORIOS'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	--Temporada
	UNION
	SELECT DISTINCT TOP (SELECT ISNULL(Pe1.WomanQtyT, 0) FROM EdiDb.dbo.PedidosExternos Pe1 WHERE Pe1.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'DAMAS'
	AND (
		D.Producto IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
		OR D.Talla IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
		OR D.Lote IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
		OR D.Categoria IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
		OR D.Departamento IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)	
	UNION
	SELECT DISTINCT TOP (SELECT ISNULL(Pe2.ManQtyT, 0) FROM EdiDb.dbo.PedidosExternos Pe2 WHERE Pe2.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'CABALLEROS'
	AND (
		D.Producto IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
		OR D.Talla IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
		OR D.Lote IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
		OR D.Categoria IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
		OR D.Departamento IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)	
	UNION
	SELECT DISTINCT TOP (SELECT ISNULL(Pe2.KidQtyT, 0) FROM EdiDb.dbo.PedidosExternos Pe2 WHERE Pe2.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'NIÑOS / AS'
	AND (
		D.Producto IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
		OR D.Talla IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
		OR D.Lote IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
		OR D.Categoria IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
		OR D.Departamento IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)	
	UNION
	SELECT DISTINCT TOP (SELECT ISNULL(Pe2.AccQtyT, 0) FROM EdiDb.dbo.PedidosExternos Pe2 WHERE Pe2.Id = @PedidoID)
		@PedidoId,
		D.Barcode,
		1
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser + 'Cp'
	AND D.Categoria = 'ACCESORIOS'
	AND (
		D.Producto IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
		OR D.Talla IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
		OR D.Lote IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
		OR D.Categoria IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
		OR D.Departamento IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	)
	AND D.CP NOT IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)		
	
	SELECT TotalCp FROM EdiDb.dbo.PedidosExternos where Id = @PedidoId
END

--truncate table EdiDB.dbo.ProductoUbicacion
DELETE FROM EdiDB.dbo.PedidosDetExternos where PedidoId = 207
EXEC EdiDb.dbo.SP_SetPaylessNewDis 207, 'Admin'
SELECT * FROM EdiDB.dbo.PedidosExternos where Id = 208
SELECT * FROM EdiDB.dbo.PedidosDetExternos where PedidoId = 208
SELECT * FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = 'Admin'
select distinct CodProducto from EdiDB.dbo.ProductoUbicacion where Typ = 7
--2617 not in
select distinct Typ from EdiDB.dbo.ProductoUbicacion 
select distinct CodProducto, NomBodega, Rack, NombreRack, Departamento from EdiDB.dbo.ProductoUbicacion 
WHERE Typ = 3 
AND NomBodega = 'DAMAS'
AND (Departamento IN ('9', '10', '11', '5')
and NombreRack in ('A', 'H')
)
--AND Rack = '152'
ORDER BY NomBodega

--51
UPDATE EdiDB.dbo.PedidosExternos
	SET TotalCp = (
		SELECT COUNT(*) FROM (
		SELECT DISTINCT
		D.Barcode
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = 'Admin'
	AND D.Producto NOT IN (SELECT DISTINCT Ppt.Producto FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Producto IS NOT NULL)
	AND D.Talla NOT IN (SELECT DISTINCT Ppt.Talla FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Talla IS NOT NULL)
	AND D.Lote NOT IN (SELECT DISTINCT Ppt.Lote FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Lote IS NOT NULL)
	AND D.Categoria NOT IN (SELECT DISTINCT Ppt.Categoria FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Categoria IS NOT NULL)
	AND D.Departamento NOT IN (SELECT DISTINCT Ppt.Departamento FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.Departamento IS NOT NULL)
	AND D.CP IN (SELECT DISTINCT Ppt.CP FROM EdiDB.dbo.PaylessPedidosCpT Ppt WITH(NOLOCK) WHERE Ppt.CP IS NOT NULL)
	) SB1
	) WHERE Id = 218