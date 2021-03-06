USE EdiDB
GO
/****** Object:  StoredProcedure [dbo].[SP_GetExistencias]    Script Date: 3/20/2019 9:08:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetExistencias]
	@IdClient int
AS
BEGIN
	SELECT 
		pr.CodProducto,
		pr.Descripcion,
		SUM(ii.Existencia) Existencia,
		um.UnidadMedida,
		eq.CodProductoLear
	FROM wms.dbo.Inventario i WITH(NOLOCK)
		JOIN wms.dbo.ItemInventario ii WITH(NOLOCK) 
			ON ii.InventarioID =i.InventarioID
		JOIN wms.dbo.Producto pr WITH(NOLOCK)
			ON pr.CodProducto = ii.CodProducto
		JOIN wms.dbo.UnidadMedida um WITH(NOLOCK)
			ON um.UnidadMedidaID = pr.UnidadMedida
		LEFT JOIN LEAR_EQUIVALENCIAS eq WITH(NOLOCK)
			ON eq.CodProducto = pr.CodProducto
	WHERE i.ClienteID = @IdClient
	GROUP BY pr.CodProducto, 
		pr.Descripcion, 
		um.UnidadMedida,
		eq.CodProductoLear
	ORDER BY pr.CodProducto
END
GO
EXEC [SP_GetExistencias] 618