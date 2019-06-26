USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdTallaLoteFil', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdTallaLoteFil
GO

CREATE PROCEDURE [dbo].SP_GetPaylessProdTallaLoteFil 
@TxtBarcode VARCHAR(16),
@CboProducto VARCHAR(16),
@CboTalla VARCHAR(8),
@CboLote VARCHAR(8),
@CboCategoria VARCHAR(1)
AS
BEGIN
	SELECT
		*
	FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	WHERE D.Barcode like @TxtBarcode + '%'
	AND D.Producto like @CboProducto + '%'
	AND D.Talla like @CboTalla + '%'
	AND D.Lote like @CboLote + '%'
	AND D.Categoria like (
	CASE @CboCategoria 
		WHEN '0' THEN 'ACCESORIOS' 
		WHEN '1' THEN 'CABALLEROS'
		WHEN '2' THEN 'DAMAS'
		WHEN '3' THEN 'NIÑOS / AS'
		WHEN '' THEN '%'
	END
	)
END
GO

exec SP_GetPaylessProdTallaLoteFil '7372854987', '', '', '', '' 