USE EdiDB
GO
IF OBJECT_ID('SP_GetPedidosDetExternosByTienda', 'P') IS NOT NULL
	DROP PROC SP_GetPedidosDetExternosByTienda
GO

CREATE PROCEDURE [dbo].SP_GetPedidosDetExternosByTienda 
@ClienteId int,
@TiendaId int
AS
BEGIN
	SELECT Pde.Id,
	Pde.PedidoId,
	Pde.CodProducto,
	Pde.CantPedir,
	P.Descripcion AS Producto,
	Pe.Periodo
	FROM EdiDb.dbo.PedidosExternos AS Pe WITH (NOLOCK)
	JOIN EdiDb.dbo.PedidosDetExternos AS Pde WITH (NOLOCK) ON Pde.PedidoId = Pe.Id
	LEFT JOIN wms.dbo.Producto AS P WITH (NOLOCK) ON P.CodProducto = Pde.CodProducto
	WHERE Pe.ClienteID = @ClienteId
	AND Pe.TiendaId = @TiendaId
END
GO

--EXEC [SP_GetPedidosDetExternos] 1345
