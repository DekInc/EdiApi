USE WMS
GO

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
		Despachos D,
		DtllDespacho DD,
		Transacciones T,
		Pedido P,
		SysTempSalidas S,
		Producto PR,
		Clientes CL,
		Inventario I,
		UnidadMedida UM
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
		SELECT DISTINCT TRIM(L.ProductId)
		FROM EdiDB.dbo.LEAR_LIN830 L
	)
	order by d.DespachoID desc
END
GO
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
	FROM dbo.Inventario i
		JOIN dbo.ItemInventario ii 
			ON ii.InventarioID =i.InventarioID
		JOIN dbo.Producto pr
			ON pr.CodProducto = ii.CodProducto
		JOIN dbo.UnidadMedida um
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
		Despachos D,
		DtllDespacho DD,
		Transacciones T,
		Pedido P,
		SysTempSalidas S,
		Producto PR,
		Clientes CL,
		Inventario I,
		ItemInventario Ii,
		UnidadMedida UM
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
		SELECT DISTINCT TRIM(L.ProductId)
		FROM EdiDB.dbo.LEAR_LIN830 L
	)
	order by d.DespachoID desc
END
GO
--exec GetSNDet 
