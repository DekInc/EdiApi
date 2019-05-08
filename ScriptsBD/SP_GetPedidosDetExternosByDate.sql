USE EdiDB
GO
IF OBJECT_ID('SP_GetPedidosDetExternosByDate', 'P') IS NOT NULL
	DROP PROC SP_GetPedidosDetExternosByDate
GO

CREATE PROCEDURE [dbo].SP_GetPedidosDetExternosByDate @DateInit varchar(10), @DateEnd varchar(10), @Typ int
AS
BEGIN
IF (@Typ = 0)
BEGIN
	SELECT DISTINCT 
	D.CantPedir,
	D.CodProducto,
	De.Categoria
	FROM EdiDb.dbo.PedidosDetExternos AS D WITH (NOLOCK)
	JOIN EdiDb.dbo.PedidosExternos AS M WITH (NOLOCK) ON M.Id = D.PedidoId AND M.IdEstado = 2 
	AND CONVERT(datetime, SUBSTRING(M.FechaPedido, 0, 11), 103) >= CONVERT(datetime, SUBSTRING(@DateInit, 0, 11), 103)
	AND CONVERT(datetime, SUBSTRING(M.FechaPedido, 0, 11), 103) < CONVERT(datetime, SUBSTRING(@DateEnd, 0, 11), 103)
	JOIN EdiDb.dbo.PAYLESS_ProdPrioriDet AS De WITH (NOLOCK) ON De.Barcode = D.CodProducto
	--WHERE Pe.ClienteID = @IdClient AND Pe.IdEstado = 1
END
END
GO

EXEC SP_GetPedidosDetExternosByDate '06/05/2019', '08/05/2019', 0
