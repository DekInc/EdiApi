USE [EdiDB]
GO
/****** Object:  StoredProcedure [dbo].[GetSNDet]    Script Date: 2/4/2019 05:31:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetSNDet]
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
		wms_test_29_01_2019.dbo.Despachos D,
		wms_test_29_01_2019.dbo.DtllDespacho DD,
		wms_test_29_01_2019.dbo.Transacciones T,
		wms_test_29_01_2019.dbo.Pedido P,
		wms_test_29_01_2019.dbo.SysTempSalidas S,
		wms_test_29_01_2019.dbo.Producto PR,
		wms_test_29_01_2019.dbo.Clientes CL,
		wms_test_29_01_2019.dbo.Inventario I,
		wms_test_29_01_2019.dbo.ItemInventario Ii,
		wms_test_29_01_2019.dbo.UnidadMedida UM
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
	and D.Fecha > GETDATE() - 165 
	and PR.CodProducto in (
		SELECT DISTINCT LTRIM(RTRIM(L.ProductId))
		FROM EdiDB.dbo.LEAR_LIN830 L
	)
	order by d.DespachoID desc
END
GO
EXEC [GetSNDet]
GO