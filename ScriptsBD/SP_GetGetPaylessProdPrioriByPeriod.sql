USE EdiDB
GO
IF OBJECT_ID('SP_GetGetPaylessProdPrioriByPeriod', 'P') IS NOT NULL
	DROP PROC SP_GetGetPaylessProdPrioriByPeriod
GO

CREATE PROCEDURE [dbo].SP_GetGetPaylessProdPrioriByPeriod @Period VARCHAR(10)
AS
BEGIN
	SELECT
		D.Id,
		D.IdPAYLESS_ProdPrioriM,
		D.OID,
		D.Barcode,
		D.Estado,
		D.Pri,
		D.PoolP,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Categoria,
		D.Departamento,
		D.CP,
		D.Pickeada,
		D.Etiquetada,
		D.Preinspeccion,
		D.Cargada,
		D.M3,
		D.Peso,
		D.IdTransporte,
		T.Transporte
	FROM PAYLESS_ProdPrioriDet D
	INNER JOIN PAYLESS_ProdPrioriM M
		ON M.Id = D.IdPAYLESS_ProdPrioriM
		AND M.Periodo = @Period
	LEFT JOIN dbo.PAYLESS_Transporte T
		ON T.Id = D.IdTransporte
END
GO

EXEC SP_GetGetPaylessProdPrioriByPeriod '13/05/2019'
