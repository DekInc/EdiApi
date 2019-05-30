USE EdiDB
GO
IF OBJECT_ID('SP_GetPaylessProdSinPedido', 'P') IS NOT NULL
	DROP PROC SP_GetPaylessProdSinPedido
GO

CREATE PROCEDURE dbo.SP_GetPaylessProdSinPedido 
@ClienteId int,
@TiendaId VARCHAR(4)
AS
BEGIN
	SELECT DISTINCT
		D.Barcode,
		D.CP,
		D.Categoria,
		D.IdPAYLESS_ProdPrioriM,
		M.Periodo
	FROM dbo.PAYLESS_ProdPrioriM M WITH(NOLOCK)
	JOIN dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK) 
		ON D.IdPAYLESS_ProdPrioriM = M.Id
	WHERE (
		SELECT PedM.Id
		FROM dbo.PedidosExternos PedM WITH(NOLOCK)
		JOIN dbo.PedidosDetExternos PedDet WITH(NOLOCK)			
			ON PedDet.PedidoId = PedM.Id
			AND PedM.ClienteID = @ClienteId
			AND PedM.IdEstado = 2
			AND PedM.TiendaId = @TiendaId		
			AND PedDet.CodProducto = D.Barcode
	) IS NULL
	AND M.ClienteId = @ClienteId
	AND D.Barcode like @TiendaId + '%'
END
GO

EXEC EdiDB.dbo.SP_GetPaylessProdSinPedido 1432, '7375'

select distinct Barcode from dbo.PAYLESS_ProdPrioriDet where Barcode like'7375%'
--797
--348