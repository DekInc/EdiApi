USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdPrioriByPeriod', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdPrioriByPeriod
GO

CREATE PROCEDURE [dbo].SP_GetPaylessProdPrioriByPeriod @Period VARCHAR(10)
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
	FROM PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	INNER JOIN PAYLESS_ProdPrioriM M WITH(NOLOCK)
		ON M.Id = D.IdPAYLESS_ProdPrioriM
		AND M.Periodo = @Period
	LEFT JOIN dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = D.IdTransporte
END
GO

--EXEC SP_GetPaylessProdPrioriByPeriod '13/05/2019'
