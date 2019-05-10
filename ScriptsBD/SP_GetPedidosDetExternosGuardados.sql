USE EdiDB
GO
IF OBJECT_ID('SP_GetPedidosDetExternosGuardados', 'P') IS NOT NULL
	DROP PROC SP_GetPedidosDetExternosGuardados
GO

CREATE PROCEDURE [dbo].SP_GetPedidosDetExternosGuardados @IdClient INT
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
	WHERE Pe.ClienteID = @IdClient AND Pe.IdEstado = 1
END
GO

EXEC [SP_GetPedidosDetExternosGuardados] 1345
