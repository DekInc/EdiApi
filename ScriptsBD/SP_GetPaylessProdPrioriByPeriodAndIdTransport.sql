USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdPrioriByPeriodAndIdTransport', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdPrioriByPeriodAndIdTransport
GO

CREATE PROCEDURE [dbo].SP_GetPaylessProdPrioriByPeriodAndIdTransport 
@Period VARCHAR(10),
@IdTransport int
AS
BEGIN
	SELECT
		Ex.Id,
		Ex.IdPAYLESS_ProdPrioriM,
		Ex.OID,
		Ex.Barcode,
		Ex.Estado,
		(SUBSTRING(Ex.Barcode, 1, 4) + ' - ' + Ti.Descr) Tienda,
		Ex.PoolP,
		Ex.Producto,
		Ex.Talla,
		Ex.Lote,
		Ex.Categoria,
		Ex.Departamento,
		Ex.CP,
		Ex.Pickeada,
		Ex.Etiquetada,
		Ex.Preinspeccion,
		Ex.Cargada,
		Ex.M3,
		Ex.Peso,
		Ex.IdTransporte,
		T.Transporte,
(select top 1 C.Nombre
from EdiDb.dbo.PAYLESS_Tiendas T WITH(NOLOCK)
join wms.dbo.Clientes C WITH(NOLOCK)
	on C.ClienteID = T.ClienteID
	AND T.TiendaId = SUBSTRING(Ad.BarCode, 1, 4)) NomCliente	
from EdiDb.dbo.PAYLESS_ProdPrioriArchDet Ad WITH(NOLOCK)
join EdiDb.dbo.PAYLESS_ProdPrioriArchM Am WITH(NOLOCK) 
	on Am.Id = Ad.IdM AND Am.Periodo = @Period
join EdiDb.dbo.PAYLESS_ProdPrioriDet Ex WITH(NOLOCK) 
	on Ex.Barcode = Ad.barcode AND Ex.IdTransporte = @IdTransport
join EdiDb.dbo.PAYLESS_Tiendas Ti WITH(NOLOCK) 
	ON Ti.TiendaId = SUBSTRING(Ex.Barcode, 1, 4)
LEFT JOIN EdiDb.dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = Ex.IdTransporte
ORDER BY Ex.Barcode
END
GO
--1592
exec SP_GetPaylessProdPrioriByPeriodAndIdTransport '17/05/2019', 8
--EXEC SP_GetPaylessProdPrioriByPeriodAndIdTransport '13/05/2019', 6
--select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
--select * from EdiDb.dbo.PAYLESS_Tiendas where TiendaId = 7366
