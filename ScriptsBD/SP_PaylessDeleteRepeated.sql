USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_WmsGetDisDet', 'P') IS NOT NULL
	DROP PROC SP_WmsGetDisDet
GO
CREATE PROCEDURE dbo.SP_WmsGetDisDet 
@TransaccionId INT
AS
BEGIN
	select --top 20 
		P.CodProducto,
		I.Barcode,
		--P.Descripcion,
		I.TipoBulto,
		Um.UnidadMedida,
		1 Cantidad,
		D.CP,
		D.Categoria,
		D.Departamento,
		D.Producto,
		D.Talla		
	from wms.dbo.Transacciones T
	JOIN wms.dbo.DetalleTransacciones Dt
		ON Dt.TransaccionID = T.TransaccionID
	JOIN wms.dbo.Inventario I
		ON I.InventarioID = Dt.InventarioID
	JOIN wms.dbo.ItemInventario Ii
		ON Ii.InventarioID = I.InventarioID
	JOIN wms.dbo.Producto P
		ON P.CodProducto = Ii.CodProducto
	LEFT JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D
		ON D.Barcode = P.CodProducto
	LEFT JOIN wms.dbo.UnidadMedida Um
		ON Um.UnidadMedidaID = I.TipoBulto
	where T.TransaccionID = @TransaccionId
END
GO

EXEC SP_WmsGetDisDet 127871
--25011