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
@CboCategoria VARCHAR(1),
@CodUser VARCHAR(128),
@BodegaId int
AS
BEGIN
	SELECT DISTINCT
		D.Barcode,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Categoria,
		D.CP
	FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
	LEFT JOIN EdiDB.dbo.WmsProductoExistencia Ex WITH(NOLOCK)
		ON Ex.CodUser = @CodUser AND Ex.CodProducto = D.Barcode AND Ex.BodegaId = @BodegaId
	WHERE D.Barcode like '%' + @TxtBarcode + '%'
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
	AND Ex.CodUser IS NOT NULL
END
GO

exec EdiDB.dbo.SP_GetPaylessProdTallaLoteFil '', '', '', '', '', 'Admin', 81

select * from EdiDB.dbo.WmsProductoExistencia where CodUser = 'Admin'