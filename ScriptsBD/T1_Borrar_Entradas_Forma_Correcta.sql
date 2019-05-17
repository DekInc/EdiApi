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

Delete From wms_test.dbo.ItemParamaetroxProducto Where ItemInventarioID In (
Select ItemInventarioID From wms_test.dbo.ItemInventario Where InventarioID In (
Select InventarioID From wms_test.dbo.DetalleTransacciones Where TransaccionID = @TranId))
GO
Delete From wms_test.dbo.DtllItemTransaccion Where TransaccionID = 123
GO
Delete From wms_test.dbo.DetalleTransacciones Where TransaccionID = @TranId
GO
Delete From wms_test.dbo.ItemInventario Where InventarioID In (Select InventarioID From DetalleTransacciones Where TransaccionID = @TranId)
GO
Delete From wms_test.dbo.Inventario  Where InventarioID In (Select InventarioID From DetalleTransacciones Where TransaccionID = " + TransaccionID + ")";
GO
Delete From wms_test.dbo.DocumentosxTransaccion Where TransaccionID = " + TransaccionID;
GO
Delete From wms_test.dbo.Transacciones Where TransaccionID = " + TransaccionID;
GO
Delete From DetalleIngresoCliente Where TransaccionID = 
GO
Delete From DtllReceivexItemInventario Where TransaccionID = 