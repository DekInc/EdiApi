USE EdiDB
GO
IF OBJECT_ID('GetTransDif', 'P') IS NOT NULL
	DROP PROC GetTransDif
GO

CREATE PROCEDURE [dbo].GetTransDif 
@IdM int
AS
BEGIN
	SELECT
		Ad.Barcode,
		'Solo existe en salida' Estado
	FROM EdiDB.dbo.PAYLESS_ProdPrioriArchDet Ad WITH(NOLOCK)	
	WHERE Ad.IdM = @IdM	
	AND SUBSTRING(Ad.barcode, 1, 1) != '-'	
	AND Ad.Barcode NOT IN (
		SELECT 
			SUBSTRING(Ad2.barcode, 2, 10)
		FROM EdiDB.dbo.PAYLESS_ProdPrioriArchDet Ad2 WITH(NOLOCK)
		WHERE Ad2.IdM = @IdM
		AND SUBSTRING(Ad2.barcode, 1, 1) = '-'
	)
	UNION
	SELECT
		SUBSTRING(Ad.barcode, 2, 10) Barcode,
		'Solo existe en entrada' Estado
	FROM EdiDB.dbo.PAYLESS_ProdPrioriArchDet Ad WITH(NOLOCK)	
	WHERE Ad.IdM = @IdM	
	AND SUBSTRING(Ad.barcode, 1, 1) = '-'	
	AND SUBSTRING(Ad.barcode, 2, 10) NOT IN (
		SELECT 
			Ad2.barcode
		FROM EdiDB.dbo.PAYLESS_ProdPrioriArchDet Ad2 WITH(NOLOCK)
		WHERE Ad2.IdM = @IdM
		AND SUBSTRING(Ad2.barcode, 1, 1) != '-'
	)
	--UNION
	--SELECT
	--	SUBSTRING(Ad.barcode, 2, 10) Barcode,
	--	'No existe el barcode en el archivo original de Payless' Estado
	--FROM EdiDB.dbo.PAYLESS_ProdPrioriArchDet Ad WITH(NOLOCK)
	--LEFT JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	--	ON D.Barcode = SUBSTRING(D.barcode, 2, 10)
	--WHERE Ad.IdM = @IdM	
	--AND SUBSTRING(Ad.barcode, 1, 1) = '-'
	--AND D.Barcode IS NULL
END
GO

exec GetTransDif 41


SELECT
		SUBSTRING(Ad.barcode, 2, 10) Barcode,
		'No existe el barcode en el archivo original de Payless' Estado,
		D.Barcode
	FROM EdiDB.dbo.PAYLESS_ProdPrioriArchDet Ad WITH(NOLOCK)
	LEFT JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON D.Barcode = SUBSTRING(D.barcode, 2, 10)
	WHERE Ad.IdM = 41
	AND SUBSTRING(Ad.barcode, 1, 1) = '-'
	AND D.Barcode IS NULL