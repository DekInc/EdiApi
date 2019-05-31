USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_GetPedidosPendientesAdmin', 'P') IS NOT NULL
	DROP PROC dbo.SP_GetPedidosPendientesAdmin
GO
CREATE PROCEDURE dbo.SP_GetPedidosPendientesAdmin
AS
BEGIN
	SELECT DISTINCT
		Pe.Id,
		(
		select top 1 B.NomBodega
		from wms.dbo.ItemInventario Ii WITH(NOLOCK)
		JOIN wms.dbo.Inventario I WITH(NOLOCK)
			ON I.InventarioID = Ii.InventarioID
		JOIN wms.dbo.Racks R WITH(NOLOCK)
			ON R.Rack = I.Rack
		JOIN wms.dbo.Bodegas B WITH(NOLOCK)
			ON B.BodegaID = R.BodegaID
		where Ii.CodProducto = PeD.CodProducto 
		AND Ii.Existencia > 0
		order by ItemInventarioID desc
		) Bodega,
		Pe.TiendaId,
		Pe.FechaPedido,
		M.Periodo,
		D.Categoria,
		D.CP,
		D.Barcode,
		(
		select top 1 R.Rack 
		from wms.dbo.ItemInventario Ii WITH(NOLOCK)
		JOIN wms.dbo.Inventario I WITH(NOLOCK)
			ON I.InventarioID = Ii.InventarioID
		JOIN wms.dbo.Racks R WITH(NOLOCK)
			ON R.Rack = I.Rack
		where Ii.CodProducto = PeD.CodProducto order by ItemInventarioID desc
		) IdRack,
		(
		select top 1 R.NombreRack 
		from wms.dbo.ItemInventario Ii WITH(NOLOCK)
		JOIN wms.dbo.Inventario I WITH(NOLOCK)
			ON I.InventarioID = Ii.InventarioID
		JOIN wms.dbo.Racks R WITH(NOLOCK)
			ON R.Rack = I.Rack
		where Ii.CodProducto = PeD.CodProducto order by ItemInventarioID desc
		) NombreRack
	FROM EdiDB.dbo.PedidosExternos Pe WITH(NOLOCK)
	JOIN EdiDB.dbo.PedidosDetExternos PeD WITH(NOLOCK)
		ON PeD.PedidoId = Pe.Id
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriDet D WITH(NOLOCK)
		ON D.Barcode = PeD.CodProducto
	JOIN EdiDB.dbo.PAYLESS_ProdPrioriM M WITH(NOLOCK)
		ON M.Id = D.IdPAYLESS_ProdPrioriM
	--WHERE Pe.Id = 9
	ORDER BY Pe.Id, Pe.TiendaId, M.Periodo, D.Categoria, D.Barcode
END
--9	GLC EL POLVORIN	7373	04/06/2019 07:30	17/05/2019	NIÑOS / AS	R	7373823553	11977	StageTGU
select top 10 *
		from wms.dbo.ItemInventario Ii WITH(NOLOCK)
		JOIN wms.dbo.Inventario I WITH(NOLOCK)
			ON I.InventarioID = Ii.InventarioID
		JOIN wms.dbo.Racks R WITH(NOLOCK)
			ON R.Rack = I.Rack
		JOIN wms.dbo.Bodegas B WITH(NOLOCK)
			ON B.BodegaID = R.BodegaID
		where Ii.CodProducto = '7373823553'
		--AND Ii.Existencia > 0
		order by ItemInventarioID desc

select * from wms.dbo.Racks where Rack = '11977'

EXEC EdiDb.dbo.SP_GetPedidosPendientesAdmin