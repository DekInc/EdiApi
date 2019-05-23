USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdPrioriAll', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdPrioriAll
GO

CREATE PROCEDURE dbo.SP_GetPaylessProdPrioriAll
@TiendaId VARCHAR(4)
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
		null Pickeada,
		null Etiquetada,
		null Preinspeccion,
		null Cargada,
		0 M3,
		0 Peso,
		D.IdTransporte,
		T.Transporte
	FROM PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	INNER JOIN PAYLESS_ProdPrioriM M WITH(NOLOCK)
		ON M.Id = D.IdPAYLESS_ProdPrioriM
	LEFT JOIN dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = D.IdTransporte
	WHERE D.Barcode like @TiendaId + '%'
END
GO

--EXEC SP_GetPaylessProdPrioriAll '7365'
