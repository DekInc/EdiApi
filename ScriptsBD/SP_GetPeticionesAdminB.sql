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
	DELETE FROM EdiDB.dbo.ProductoUbicacion WHERE Typ IN (2, 3)
	INSERT INTO EdiDB.dbo.ProductoUbicacion (Typ, CodProducto, NomBodega, Rack, NombreRack, Departamento)
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
		WHERE D2.Barcode = Dp2.CodProducto) CP,
		(SELECT TOP 1 
			D2.Departamento
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) Depto
	FROM wms.dbo.DtllPedido Dp2 WITH(NOLOCK)			
	JOIN EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
		ON Dp2.PedidoId = Pe.PedidoWMS

	INSERT INTO EdiDB.dbo.ProductoUbicacion (Typ, CodProducto, NomBodega, Rack, NombreRack, Departamento)
	SELECT DISTINCT 3, 
		Dp2.CodProducto, 
		(SELECT TOP 1 
			D2.Categoria
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) Categoria,
		Pe.Id,
		(SELECT TOP 1 
			D2.CP
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) CP,
		(SELECT TOP 1 
			D2.Departamento
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D2 WITH(NOLOCK)
		WHERE D2.Barcode = Dp2.CodProducto) Depto
	FROM EdiDB.dbo.PedidosDetExternos Dp2 WITH(NOLOCK)			
	JOIN EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
		ON Dp2.PedidoId = Pe.Id

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
			AND (D.CP IN ('A')
			OR D.Departamento IN ('9', '10', '11')
			)
		) SB1) TotalCp,
		Pe.PedidoWMS,
		Pe.IdEstado,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'DAMAS'
			AND Pu1.Rack = Pe.PedidoWMS
			AND (Pu1.NombreRack NOT IN ('A')
			OR Pu1.Departamento NOT IN ('9', '10', '11')
			)
		) WomanQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'CABALLEROS'
			AND Pu1.Rack = Pe.PedidoWMS
			AND (Pu1.NombreRack NOT IN ('A')
			OR Pu1.Departamento NOT IN ('9', '10', '11')
			)
		) ManQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'NIÑOS / AS'
			AND Pu1.Rack = Pe.PedidoWMS
			AND (Pu1.NombreRack NOT IN ('A')
			OR Pu1.Departamento NOT IN ('9', '10', '11')
			)
		) KidQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND UPPER(Pu1.NomBodega) = 'ACCESORIOS'
			AND Pu1.Rack = Pe.PedidoWMS
			AND (Pu1.NombreRack NOT IN ('A')
			OR Pu1.Departamento NOT IN ('9', '10', '11')
			)
		) AccQtyEnv,
		(
			SELECT COUNT(*)
			FROM EdiDB.dbo.ProductoUbicacion Pu1 WITH(NOLOCK)
			WHERE Pu1.Typ = 2
			AND (Pu1.NombreRack IN ('A')
			OR Pu1.Departamento IN ('9', '10', '11')
			)
			AND Pu1.Rack = Pe.PedidoWMS
		) TotalCpEnv,
		Pe.FullPed,
		Pe.Divert,
		Pe.TiendaIdDestino
	FROM EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
	JOIN EdiDB.dbo.PAYLESS_Tiendas T WITH(NOLOCK)
		ON T.TiendaId = Pe.TiendaId	
	DELETE FROM EdiDB.dbo.ProductoUbicacion WHERE Typ IN (2)
END

--truncate table EdiDB.dbo.ProductoUbicacion
EXEC EdiDb.dbo.SP_GetPeticionesAdminB
--27763
select * from EdiDB.dbo.ProductoUbicacion WHERE Typ = 3 AND Rack = '70532' ORDER BY NomBodega