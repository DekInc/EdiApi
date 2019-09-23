USE EdiDB
GO
IF OBJECT_ID('SP_GetPedidosDetExternos', 'P') IS NOT NULL
	DROP PROC SP_GetPedidosDetExternos
GO

CREATE PROCEDURE [dbo].SP_GetPedidosDetExternos @IdClient INT
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
	WHERE Pe.ClienteID = @IdClient
END
GO

EXEC [SP_GetPedidosDetExternos] 385
