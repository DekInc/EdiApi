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
	DELETE FROM EdiDB.dbo.ProductoUbicacion WHERE Typ = 2
	INSERT INTO EdiDB.dbo.ProductoUbicacion (Typ, CodProducto, NomBodega, Rack, NombreRack)
	SELECT DISTINCT 2, 
		Dp2.CodProducto, 
		(SELECT TOP 1 
			D2.Categoria
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) Categoria,
		Pe.PedidoWMS,
		(SELECT TOP 1 
			D2.CP
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) CP
	FROM wms.dbo.DtllPedido Dp2 WITH(NOLOCK)			
	JOIN EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
		ON Dp2.PedidoId = Pe.PedidoWMS

	SELECT DISTINCT
		Pe.Id,		
		Pe.TiendaId,
		T.Descr,
		Pe.WomanQty,
		Pe.ManQty,
		Pe.KidQty,
		Pe.AccQty,		
		Pe.FechaCreacion,
		Pe.FechaPedido,		
		(SELECT COUNT(*) FROM (
		SELECT DISTINCT D.Barcode
			FROM EdiDb.dbo.PedidosDetExternos Pd WITH(NOLOCK)
			JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D
				ON D.Barcode = Pd.CodProducto
			WHERE Pd.PedidoId = Pe.Id
			AND D.CP != ''
		) SB1) TotalCp,
		Pe.PedidoWMS,
		Pe.IdEstado,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'DAMAS'
			AND Pu1.Rack = Pe.PedidoWMS
		) WomanQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'CABALLEROS'
			AND Pu1.Rack = Pe.PedidoWMS
		) ManQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'NIÑOS / AS'
			AND Pu1.Rack = Pe.PedidoWMS
		) KidQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'ACCESORIOS'
			AND Pu1.Rack = Pe.PedidoWMS
		) AccQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND Pu1.NombreRack != ''
			AND Pu1.Rack = Pe.PedidoWMS
		) TotalCpEnv
	FROM EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_Tiendas T WITH(NOLOCK)
		ON T.TiendaId = Pe.TiendaId	
END


EXEC EdiDb.dbo.SP_GetPeticionesAdminB

select * from EdiDB.dbo.ProductoUbicacion WHERE Typ = 2 AND Rack = '70405' ORDER BY NomBodega