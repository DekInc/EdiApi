USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdPrioriFileDif', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdPrioriFileDif
GO

CREATE PROCEDURE [dbo].SP_GetPaylessProdPrioriFileDif 
@IdData INT,
@IdProdArch INT,
@ClienteId INT
AS
BEGIN
	IF (@IdData = 1)
	BEGIN
	SELECT DISTINCT
		0 Id,
		0 IdPAYLESS_ProdPrioriM,
		null OID,
		D.Barcode,
		null Estado,
		null Pri,
		null PoolP,
		null Producto,
		null Talla,
		null Lote,
		D.Categoria,
		null Departamento,
		D.CP,
		null Pickeada,
		null Etiquetada,
		null Preinspeccion,
		null Cargada,
		0 M3,
		0 Peso,
		D.IdTransporte,
		T.Transporte
	FROM EdiDb.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)	
	LEFT JOIN EdiDb.dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = D.IdTransporte
	WHERE D.Barcode IN (
		SELECT DISTINCT
			Fud.Barcode
		FROM EdiDb.dbo.PAYLESS_ProdPrioriDet Fud WITH(NOLOCK)
		JOIN EdiDb.dbo.PAYLESS_ProdPrioriM Fu WITH(NOLOCK)
			ON Fud.IdPAYLESS_ProdPrioriM = Fu.Id
			AND Fu.ClienteId = @ClienteId
		JOIN EdiDb.dbo.PAYLESS_ProdPrioriArchM Pe WITH(NOLOCK)
			ON Pe.Periodo = Fu.Periodo AND Pe.IdTransporte = Fud.IdTransporte
			AND Pe.Id = @IdProdArch
		JOIN EdiDb.dbo.PAYLESS_ProdPrioriArchDet Dp WITH(NOLOCK)
			ON Dp.IdM = Pe.Id AND Dp.barcode = Fud.Barcode
	)
	ORDER BY D.Barcode
	END
	IF (@IdData = 2)
	BEGIN
	SELECT DISTINCT
		0 Id,
		0 IdPAYLESS_ProdPrioriM,
		null OID,
		D.Barcode,
		null Estado,
		null Pri,
		null PoolP,
		null Producto,
		null Talla,
		null Lote,
		D.Categoria,
		null Departamento,
		D.CP,
		null Pickeada,
		null Etiquetada,
		null Preinspeccion,
		null Cargada,
		0 M3,
		0 Peso,
		D.IdTransporte,
		T.Transporte
	FROM EdiDb.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)	
	JOIN EdiDb.dbo.PAYLESS_ProdPrioriM M WITH(NOLOCK)
		ON M.Id = D.IdPAYLESS_ProdPrioriM
		AND M.ClienteId = @ClienteId
	JOIN (SELECT TOP 1 Am.Periodo, Am.IdTransporte 
		FROM EdiDb.dbo.PAYLESS_ProdPrioriArchM Am
		WHERE Am.Id = @IdProdArch) sb1
		ON M.Periodo = sb1.Periodo AND D.IdTransporte = sb1.IdTransporte
	LEFT JOIN EdiDb.dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = D.IdTransporte
	ORDER BY D.Barcode
	--WHERE D.Barcode NOT IN (
	--	SELECT DISTINCT
	--		Fud.Barcode
	--	FROM EdiDb.dbo.PAYLESS_ProdPrioriDet Fud WITH(NOLOCK)
	--	JOIN EdiDb.dbo.PAYLESS_ProdPrioriM Fu WITH(NOLOCK)
	--		ON Fud.IdPAYLESS_ProdPrioriM = Fu.Id
	--	JOIN EdiDb.dbo.PAYLESS_ProdPrioriArchM Pe WITH(NOLOCK)
	--		ON Pe.Periodo = Fu.Periodo AND Pe.IdTransporte = Fud.IdTransporte
	--		AND Pe.Id = 4
	--	JOIN EdiDb.dbo.PAYLESS_ProdPrioriArchDet Dp WITH(NOLOCK)
	--		ON Dp.IdM = Pe.Id AND Dp.barcode = Fud.Barcode
	--)	
	END
END
GO
--1909
--EXEC SP_GetPaylessProdPrioriFileDif 1, 4