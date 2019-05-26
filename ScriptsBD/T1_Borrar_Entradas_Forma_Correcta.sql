select * from wms.dbo.Clientes where ClienteID between 1397 and 1422 AND Nombre like'%pay%'
select top 1* from wms.dbo.Transacciones where ClienteID in (
select ClienteID from wms.dbo.Clientes where ClienteID between 1397 and 1422 AND Nombre like'%pay%'
)
--delete from wms.dbo.ItemInventario where InventarioID in (
--	select InventarioID from wms.dbo.Inventario where ClienteId in (
--	select ClienteID from wms.dbo.Clientes where ClienteID between 1397 and 1422 AND Nombre like'%pay%'
--	)
--)
----1
--delete from  wms.dbo.Inventario where ClienteId in (
--select ClienteID from wms.dbo.Clientes where ClienteID between 1397 and 1422 AND Nombre like'%pay%'
--)
select * from wms.dbo.Inventario where ClienteID in (
select ClienteID from wms.dbo.Clientes where ClienteID between 1397 and 1422 AND Nombre like'%pay%'
)
select top 6 * from dbo.Transacciones
----1

begin
DECLARE @TransaccionId int;
SET @TransaccionId = 116551;

Delete From wms.dbo.ItemParamaetroxProducto Where ItemInventarioID In (
Select ItemInventarioID From wms.dbo.ItemInventario Where InventarioID In (
Select InventarioID From wms.dbo.DetalleTransacciones Where TransaccionID IN (
@TransaccionId
)));

Delete From wms.dbo.DtllItemTransaccion Where TransaccionID  IN (
@TransaccionId
);
Delete From wms.dbo.ItemInventario Where InventarioID In (
Select InventarioID From wms.dbo.DetalleTransacciones Where TransaccionID  IN (
@TransaccionId
));
--SELECT TransaccionID From wms.dbo.Transacciones Where ClienteID = @ClienteId
Delete From wms.dbo.Inventario  Where InventarioID In (
Select InventarioID From wms.dbo.DetalleTransacciones Where TransaccionID  IN (
@TransaccionId
));
Delete From wms.dbo.DocumentosxTransaccion Where TransaccionID  IN (
@TransaccionId
);
Delete From wms.dbo.DetalleTransacciones Where TransaccionID  IN (
@TransaccionId
);
Delete From wms.dbo.DetalleIngresoCliente Where TransaccionID  IN (
@TransaccionId
);
Delete From wms.dbo.DtllReceivexItemInventario Where TransaccionID  IN (
@TransaccionId
);
Delete From wms.dbo.Transacciones Where TransaccionID  IN (
@TransaccionId
);
end