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
		(SUBSTRING(Ex.Barcode, 1, 4) + ' - ' + 'payless shoe source ' + (CASE Ti.BodegaId WHEN 81 THEN '- sps' WHEN 82 THEN '- tgu' ELSE '' END)) Tienda,
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
	AND CONVERT(VARCHAR(4), T.TiendaId) = SUBSTRING(Ad.BarCode, 1, 4)) NomCliente	
from EdiDb.dbo.PAYLESS_ProdPrioriArchDet Ad WITH(NOLOCK)
join EdiDb.dbo.PAYLESS_ProdPrioriArchM Am WITH(NOLOCK) 
	on Am.Id = Ad.IdM AND Am.Periodo = @Period
join EdiDb.dbo.PAYLESS_ProdPrioriDet Ex WITH(NOLOCK) 
	on Ex.Barcode = Ad.barcode AND Ex.IdTransporte = @IdTransport
join EdiDb.dbo.PAYLESS_Tiendas Ti WITH(NOLOCK) 
	ON CONVERT(VARCHAR(4), Ti.TiendaId) = SUBSTRING(Ex.Barcode, 1, 4)
LEFT JOIN EdiDb.dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = Ex.IdTransporte
ORDER BY Ad.Id
END
GO
--1592
exec EdiDb.dbo.SP_GetPaylessProdPrioriByPeriodAndIdTransport '20/09/2019', 79
--EXEC SP_GetPaylessProdPrioriByPeriodAndIdTransport '13/05/2019', 6
--select * from EdiDb.dbo.PAYLESS_Transporte
--select * from EdiDb.dbo.PAYLESS_Tiendas where TiendaId = 7366
select top 20000 * from EdiDb.dbo.PAYLESS_ProdPrioriDet order by id desc
update EdiDb.dbo.PAYLESS_ProdPrioriArchDet SET Barcode = REPLACE(Barcode, 'Hilm', '8000')
select top 200 REPLACE(Barcode, 'Hilm', '8000') from EdiDb.dbo.PAYLESS_ProdPrioriDet order by id desc
select * from PAYLESS_Transporte
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
select * from EdiDb.dbo.PAYLESS_ProdPrioriM

