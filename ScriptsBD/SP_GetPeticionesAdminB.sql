USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_GetPeticionesAdminB', 'P') IS NOT NULL
	DROP PROC dbo.SP_GetPeticionesAdminB
GO
CREATE PROCEDURE dbo.SP_GetPeticionesAdminB
AS
BEGIN
	SELECT DISTINCT
		Pe.Id,		
		Pe.TiendaId,
		T.Transporte,
		Pe.FechaCreacion,
		Pe.FechaPedido,
		Pe.WomanQty,
		Pe.ManQty,
		Pe.KidQty,
		Pe.AccQty,
		(SELECT COUNT(*) FROM (
		SELECT DISTINCT D.Barcode
			FROM EdiDb.dbo.PedidosDetExternos Pd WITH(NOLOCK)
			JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D
				ON D.Barcode = Pd.CodProducto
			WHERE Pd.PedidoId = Pe.Id
		) SB1) TotalSCp,
		--() TotalCp,
		--() Total,
		Pe.IdEstado
	FROM EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_Tiendas T WITH(NOLOCK)
		ON T.Id = Pe.TiendaId	
	--ORDER BY Pe.Id, Pe.TiendaId, M.Periodo, D.Categoria, D.Barcode
END

SELECT COUNT(*) FROM (
SELECT DISTINCT D.Barcode
		FROM EdiDb.dbo.PedidosDetExternos Pd WITH(NOLOCK)
		JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D
			ON D.Barcode = Pd.CodProducto
		WHERE Pd.PedidoId = 19		
) SB1

EXEC EdiDb.dbo.SP_GetPeticionesAdminB 1428