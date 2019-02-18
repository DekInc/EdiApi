USE EdiDB

GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GetSN]') AND type in (N'P', N'PC'))
	DROP PROCEDURE GetSN
GO
CREATE PROCEDURE GetSN
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
		(select count(*) from EdiDB.dbo.EdiRepSent ERS where ERS.Tipo = '856' AND Code = D.DespachoID) Procesado
	from 
		wms_test_04022019.dbo.Despachos D,
		wms_test_04022019.dbo.DtllDespacho DD,
		wms_test_04022019.dbo.Transacciones T,
		wms_test_04022019.dbo.Pedido P,
		wms_test_04022019.dbo.SysTempSalidas S,
		wms_test_04022019.dbo.Producto PR,
		wms_test_04022019.dbo.Clientes CL,
		wms_test_04022019.dbo.Inventario I,
		wms_test_04022019.dbo.UnidadMedida UM
	where PR.CodProducto = S.CodProducto
	and S.PedidoID = T.PedidoID
	and P.PedidoID = T.PedidoID
	and T.TransaccionID = DD.TransaccionID
	and DD.DespachoID = D.DespachoID 
	and CL.ClienteID = T.ClienteID
	and I.InventarioID = S.InventarioID
	and UM.UnidadMedidaID = I.TipoBulto
	and T.EstatusID = 9
	and D.Fecha > GETDATE() - 366 
	and PR.CodProducto in (
		SELECT DISTINCT RTRIM(LTRIM(L.ProductId))
		FROM EdiDB.dbo.LEAR_LIN830 L
	)
	order by d.DespachoID desc
END
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SP_GetExistencias]') AND type in (N'P', N'PC'))
	DROP PROCEDURE SP_GetExistencias
GO
CREATE PROCEDURE SP_GetExistencias
	@IdClient int
AS
BEGIN
	SELECT 
		pr.CodProducto,
		pr.Descripcion,
		SUM(ii.Existencia) Existencia,
		um.UnidadMedida
	FROM wms_test_04022019.dbo.Inventario i
		JOIN wms_test_04022019.dbo.ItemInventario ii 
			ON ii.InventarioID =i.InventarioID
		JOIN wms_test_04022019.dbo.Producto pr
			ON pr.CodProducto = ii.CodProducto
		JOIN wms_test_04022019.dbo.UnidadMedida um
			ON um.UnidadMedidaID = i.TipoBulto
	WHERE i.ClienteID = @IdClient
	GROUP BY pr.CodProducto, 
		pr.Descripcion, 
		um.UnidadMedida
	ORDER BY pr.CodProducto
END
GO
--exec SP_GetExistencias 618
-- exec GetSN
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[GetSNDet]') AND type in (N'P', N'PC'))
	DROP PROCEDURE GetSNDet
GO
CREATE PROCEDURE GetSNDet
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
		D.destinoid,
		(S.cantidad * S.precio) PrecioTotal,
		Ii.numero_oc
	from 
		wms_test_04022019.dbo.Despachos D,
		wms_test_04022019.dbo.DtllDespacho DD,
		wms_test_04022019.dbo.Transacciones T,
		wms_test_04022019.dbo.Pedido P,
		wms_test_04022019.dbo.SysTempSalidas S,
		wms_test_04022019.dbo.Producto PR,
		wms_test_04022019.dbo.Clientes CL,
		wms_test_04022019.dbo.Inventario I,
		wms_test_04022019.dbo.ItemInventario Ii,
		wms_test_04022019.dbo.UnidadMedida UM
	where PR.CodProducto = S.CodProducto
	and S.PedidoID = T.PedidoID
	and P.PedidoID = T.PedidoID
	and T.TransaccionID = DD.TransaccionID
	and DD.DespachoID = D.DespachoID 
	and CL.ClienteID = T.ClienteID
	and I.InventarioID = S.InventarioID
	and Ii.ItemInventarioID = S.ItemInventarioID
	and UM.UnidadMedidaID = I.TipoBulto
	and T.EstatusID = 9		
	and D.Fecha > GETDATE() - 366 
	and PR.CodProducto in (
		SELECT DISTINCT LTRIM(RTRIM(L.ProductId))
		FROM EdiDB.dbo.LEAR_LIN830 L
	)
	order by d.DespachoID desc
END
GO
--exec GetSNDet 
