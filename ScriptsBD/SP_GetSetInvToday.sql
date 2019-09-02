USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_GetSetInvToday', 'P') IS NOT NULL
	DROP PROC SP_GetSetInvToday
GO
CREATE PROCEDURE SP_GetSetInvToday
@ClienteId int,
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
		Wpe.BodegaId,
		D.Barcode,
		D.Categoria,
		D.Cp
	FROM EdiDB.dbo.WmsProductoExistencia Wpe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON Wpe.CodProducto = D.Barcode
	WHERE Wpe.CodUser = @CodUser
	AND Wpe.Existencia > 0

	DELETE FROM EdiDB.dbo.WmsProductoExistencia WHERE CodUser = @CodUser
END

EXEC EdiDb.dbo.SP_GetSetInvToday 1432, 'BOTO2'

SELECT distinct CodProducto from EdiDB.dbo.WmsProductoExistencia where CodUser = 'BOTO2' AND CodProducto like '7365%' and Existencia > 0

select distinct Categoria from EdiDB.dbo.PAYLESS_ProdPrioriDet
select distinct IdTransporte, BarCode, Categoria from EdiDb.dbo.PAYLESS_ProdPrioriDet where Categoria in ('WOMEN', 'MEN')

SELECT distinct D.Barcode
from EdiDB.dbo.WmsProductoExistencia Wpe
JOIN EdiDb.dbo.PAYLESS_ProdPrioriDet D
	ON D.Barcode = Wpe.CodProducto
where Wpe.CodUser = 'BOTO2' 
AND Wpe.CodProducto like '7365%' 
AND D.Categoria = 'ACCESORIOS'
AND Wpe.Existencia > 0
-- Pedido 200
-- M 229 MAN 111 K 198 a 77, t = 615
-- D M 169, Man 81, K 98, A 67 = 415
select top 10 * from EdiDB.dbo.PedidosExternos Pe where Pe.ClienteID = 1432 and Pe.TiendaId = 7366
--and Pe.PedidoWMS IS NULL
order by Pe.Id Desc
select sum(Pe.WomanQty + Pe.ManQty + Pe.KidQty + Pe.AccQty) from EdiDB.dbo.PedidosExternos Pe where Pe.ClienteID = 1432 and Pe.TiendaId = 7366
and Pe.PedidoWMS IS NULL
--200


SELECT DISTINCT
CodProducto
FROM EdiDB.dbo.PedidosExternos Pe
JOIN EdiDB.dbo.PedidosDetExternos Pde
	ON Pde.PedidoId = Pe.Id
WHERE Pe.PedidoWMS IS NULL

