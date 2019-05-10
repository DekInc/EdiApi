USE [EdiDB]
GO
/****** Object:  StoredProcedure [dbo].[GetSNDet]    Script Date: 2/4/2019 05:31:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetSNDet]
@PedidoId int = 0
AS
BEGIN
	select 
		D.DespachoID,
		D.FechaSalida,
		PR.CodProducto,
		PR.Descripcion Producto,
		CL.Nombre Cliente,
		S.Cantidad,
		(S.cantidad * I.peso / I.cantidadinicial) Peso,
		(S.cantidad * I.volumen / I.cantidadinicial) Volumen,
		(S.cantidad * I.articulos / I.cantidadinicial) Bultos,
		UM.UnidadMedida,
		D.Destino,		
		D.nocontenedor,
		D.motorista,
		D.documentomotorista,		
		D.documentofiscal,
		D.fechadocfiscal,
		D.nomarchamo,
		D.observacion,
		D.transportistaid,
		0 destinoid,
		(S.cantidad * S.precio) PrecioTotal,
		Ii.numero_oc,
		P.PedidoID
	from 
		wms.dbo.Despachos D WITH(NOLOCK),
		wms.dbo.DtllDespacho DD WITH(NOLOCK),
		wms.dbo.Transacciones T WITH(NOLOCK),
		wms.dbo.Pedido P WITH(NOLOCK),
		wms.dbo.SysTempSalidas S WITH(NOLOCK),
		wms.dbo.Producto PR WITH(NOLOCK),
		wms.dbo.Clientes CL WITH(NOLOCK),
		wms.dbo.Inventario I WITH(NOLOCK),
		wms.dbo.ItemInventario Ii WITH(NOLOCK),
		wms.dbo.UnidadMedida UM WITH(NOLOCK)
	where PR.CodProducto = S.CodProducto
	and S.PedidoID = T.PedidoID
	and P.PedidoID = T.PedidoID
	and T.TransaccionID = DD.TransaccionID
	and DD.DespachoID = D.DespachoID 
	and CL.ClienteID = T.ClienteID
	and I.InventarioID = S.InventarioID
	and Ii.ItemInventarioID = S.ItemInventarioID
	and UM.UnidadMedidaID = PR.UnidadMedida
	and T.EstatusID = 9		
	and ((D.Fecha > GETDATE() - 31 AND @PedidoId = 0) OR (@PedidoId = P.PedidoID))
	and ((PR.CodProducto in (
		SELECT DISTINCT LTRIM(RTRIM(L.ProductId))
		FROM EdiDB.dbo.LEAR_LIN830 L WITH(NOLOCK)
	) AND @PedidoId = 0) OR (@PedidoId = P.PedidoID))
	order by d.DespachoID desc
END
GO
EXEC [GetSNDet] 0
GO