USE EdiDB
GO
IF OBJECT_ID('GetWmsFileById', 'P') IS NOT NULL
	DROP PROC GetWmsFileById
GO

CREATE PROCEDURE [dbo].GetWmsFileById 
@IdM int
AS
BEGIN
	SELECT DISTINCT 
		Ex.Barcode,
		Ex.Producto,
		Ex.Talla,
		Ex.Lote,
		Ex.Categoria,
		Ex.Departamento,
		Ex.CP,
		Ex.IdTransporte,
		T.Transporte,
(select top 1 C.Nombre
from EdiDb.dbo.PAYLESS_Tiendas T
join wms.dbo.Clientes C
	on C.ClienteID = T.ClienteID
	AND T.TiendaId = SUBSTRING(Ex.BarCode, 1, 4)) NomCliente,
	(SUBSTRING(Ex.Barcode, 1, 4) + ' - ' + Ti.Descr) Tienda,
	Ex.Peso,
	Ex.M3
from EdiDb.dbo.PAYLESS_ProdPrioriArchDet Ad
join EdiDb.dbo.PAYLESS_ProdPrioriArchM Am 
	on Am.Id = Ad.IdM AND Am.Id = @IdM
join EdiDb.dbo.PAYLESS_ProdPrioriDet Ex 
	on Ex.Barcode = SUBSTRING(Ad.barcode, 2, 10)
join EdiDb.dbo.PAYLESS_Tiendas Ti WITH(NOLOCK) 
	ON Ti.TiendaId = SUBSTRING(Ex.Barcode, 1, 4)
LEFT JOIN EdiDb.dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = Ex.IdTransporte
ORDER BY Ex.Barcode
END
GO
--1643
exec GetWmsFileById 32
--EXEC SP_GetPaylessProdPrioriByPeriodAndIdTransport '13/05/2019', 6
--select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
--select * from EdiDb.dbo.PAYLESS_Tiendas where TiendaId = 7366
