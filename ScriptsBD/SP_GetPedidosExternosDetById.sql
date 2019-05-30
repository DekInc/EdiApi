USE EdiDB
GO
IF OBJECT_ID('SP_GetPedidosExternosDetById', 'P') IS NOT NULL
	DROP PROC SP_GetPedidosExternosDetById
GO

CREATE PROCEDURE [dbo].SP_GetPedidosExternosDetById 
@PedidoId int
AS
BEGIN
	SELECT DISTINCT
		0 Id,
		D.IdPAYLESS_ProdPrioriM,
		D.OID,
		D.Barcode,
		D.Estado,
		D.Pri,
		D.PoolP,
		D.Producto,
		D.Talla,
		D.Lote,
		D.Categoria,
		D.Departamento,
		D.CP,
		D.Pickeada,
		D.Etiquetada,
		D.Preinspeccion,
		D.Cargada,
		D.M3,
		D.Peso,
		D.IdTransporte,
		T.Transporte,
		Pde.CantPedir
	FROM PAYLESS_ProdPrioriDet D WITH(NOLOCK)	
	JOIN PedidosDetExternos Pde WITH(NOLOCK)
		ON Pde.PedidoId = @PedidoId 
		AND Pde.CodProducto = D.Barcode
	LEFT JOIN dbo.PAYLESS_Transporte T WITH(NOLOCK)
		ON T.Id = D.IdTransporte
END
GO

EXEC SP_GetPedidosExternosDetById 4

--select * from EdiDB.dbo.PedidosExternos
--select * from EdiDB.dbo.PedidosDetExternos