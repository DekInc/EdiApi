USE [EdiDB]
GO
/****** Object:  StoredProcedure [dbo].[GetSN]    Script Date: 9/5/2019 17:35:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetSN]
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
		(select count(*) from EdiDB.dbo.EdiRepSent ERS WITH(NOLOCK) where ERS.Tipo = '856' AND Code = D.DespachoID) Procesado
	from 
		wms.dbo.Despachos D WITH(NOLOCK),
		wms.dbo.DtllDespacho DD WITH(NOLOCK),
		wms.dbo.Transacciones T WITH(NOLOCK),
		wms.dbo.Pedido P WITH(NOLOCK),
		wms.dbo.SysTempSalidas S WITH(NOLOCK),
		wms.dbo.Producto PR WITH(NOLOCK),
		wms.dbo.Clientes CL WITH(NOLOCK),
		wms.dbo.Inventario I WITH(NOLOCK),
		wms.dbo.UnidadMedida UM WITH(NOLOCK)
	where PR.CodProducto = S.CodProducto
	and S.PedidoID = T.PedidoID
	and P.PedidoID = T.PedidoID
	and T.TransaccionID = DD.TransaccionID
	and DD.DespachoID = D.DespachoID 
	and CL.ClienteID = T.ClienteID
	and I.InventarioID = S.InventarioID
	and UM.UnidadMedidaID = PR.UnidadMedida
	and T.EstatusID = 9
	and D.Fecha > GETDATE() - 45
	and PR.CodProducto in (
		SELECT DISTINCT RTRIM(LTRIM(L.ProductId))
		FROM EdiDB.dbo.LEAR_LIN830 L WITH(NOLOCK)
	)
	order by d.DespachoID desc
END
GO
EXEC GetSN
GO