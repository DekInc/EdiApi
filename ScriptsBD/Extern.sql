saddasdasd
select * from wms_test_29_01_2019.dbo.Paises where NomPais like'%pana%'
--487	TIENDAS CARRION, S.A. DE C.V.(TRANSITO A GUATEMAL)
select * from wms.dbo.Clientes with(nolock) where nombre like '%payless%' OR ClienteID = 610 order by ClienteID asc
select * from edidb.dbo.IEnetUsers where Id > 4
--PAYLESS SHOE SOURCE, HONDURAS
--SAN PEDRO SULA, HONDURAS
--select max(ClienteID) + 1 from wms_test.dbo.Clientes
--INSERT INTO wms_test.dbo.Clientes(ClienteID, Nombre, NIT, NRC, Direccion, Telefono, correo, DiasPago, Comentario, EstatusID, Contacto, TelefonoContacto, EmailContacto, orderEmailNotifica)
--VALUES(1432, 'PAYLESS SHOE SOURCE, HONDURAS', 'S/N', 'S/N', 'SAN PEDRO SULA, HONDURAS', 'S/N', 'S/N', '', '', 6, 'Lucrecia Calderon', 'S/N', null, null)
--GLOBAL COMMUNICATIONS EL SALVADOR			CALLE SAN ANTONIO ABAD #3540
select * from wms.dbo.Clientes where ClienteID between 273 and 274
select * from wms_test.dbo.Inventario where ClienteID = 610
--618 es LEAR
-- >= 1397 es Payless
-- 2 4 92
select * from wms.dbo.usrsystem where CodUsr in (2, 4, 92)
select * from wms.dbo.usrsystem where idusr like'%ainventariot%' or nomusr like'%ainventariot%'
select distinct Color, CodProducto from wms.dbo.Producto where CodProducto like '7370%'
select distinct CodProducto from wms.dbo.ItemInventario where CodProducto like '7370%' and color  = 'DRYU 961708-5'
select * from EdiDB.dbo.AsyncStates


select * from wms_test_29_01_2019.dbo.Transacciones where pais_orig = 90
select * from wms_test_29_01_2019.dbo.usrsystem
SELECT *  FROM [EdiDB].[dbo].[UsuariosExternos]
select * from edidb.dbo.IEnetUsers where Id > 4
select @@servicename
--update edidb.dbo.IEnetUsers SET ClienteID = 385, NomUsr = 'EL Salv.' where Id = 3
--UPDATE [UsuariosExternos] SET CodUsr = 'paylesstest'
-- 385
--63232
--1967856447
exec SP_GetExistenciasExtern 618
SELECT * FROM EdiDB.dbo.PedidosEstadosExternos
SELECT * FROM EdiDB.dbo.PedidosExternos
--delete from EdiDB.dbo.PedidosExternos where id in (7)
--delete from EdiDB.dbo.PedidosDetExternos where PedidoId in (7)
--update PedidosExternos SET FechaPedido = '08/05/2019 16:00', IdEstado = 2 where Id = 1
--delete from EdiDB.dbo.PedidosExternos where Id > 15
--truncate table EdiDB.dbo.PedidosExternos
--truncate table EdiDB.dbo.PedidosDetExternos
SELECT * FROM EdiDB.dbo.PedidosDetExternos where PedidoId = 4
SELECT * FROM EdiDB.dbo.PedidosDetExternos where CodProducto = '7375888505'
order by CodProducto, CantPedir
select * from EdiDB.dbo.PAYLESS_ProdPrioriDet where barcode = '7375888505'
--delete from EdiDB.dbo.PedidosDetExternos where PedidoId = 5
--truncate table EdiDB.dbo.PedidosDetExternos
SELECT * FROM wms_test_29_01_2019.dbo.VInformacionPedidosWeb WHERE ClienteID = 618 ORDER BY CodProducto
SELECT * FROM wms_test_29_01_2019.dbo.InventarioExistencias WHERE ClienteID = 618
SELECT * FROM ListaPedidosWeb Where ClienteID = 618 Order by fecha DESC
SELECT     P.ClienteID, P.PedidoBarcode AS Pedido, P.fechapedido AS Fecha, E.Estatus, 
B.NomBodega AS Bodega, R.Regimen, (Dp.CodProducto) AS Articulos, Dp.Cantidad, P.Observacion
FROM dbo.Pedido AS P 
INNER JOIN dbo.Estatus AS E ON E.EstatusID = P.EstatusID 
INNER JOIN dbo.Bodegas AS B ON B.BodegaID = P.BodegaID 
INNER JOIN dbo.Regimen AS R ON R.IDRegimen = P.RegimenID 
INNER JOIN dbo.DtllPedido AS Dp ON Dp.PedidoID = P.PedidoID
WHERE ClienteID = 618
--GROUP BY P.ClienteID, P.PedidoBarcode, P.fechapedido, B.NomBodega, R.Regimen, E.Estatus, P.Observacion
ORDER BY P.fechapedido DESC
SELECT TOP 200 * FROM wms_test_29_01_2019.[dbo].[VPedidosWebProducto]
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Producto
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.Producto WHERE CodProducto = 'E11084800'
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.UnidadMedida WHERE UnidadMedidaID = 346

SELECT CodProducto, ClienteId, Count(*) FROM wms_test_29_01_2019.dbo.Producto GROUP BY CodProducto, ClienteId ORDER BY count(*) desc
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Clientes WHERE ClienteId = 609
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Inventario
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.ItemInventario

SELECT * FROM LEAR_EQUIVALENCIAS 

EXEC GetSNDet 63386
select * from wms.dbo.Pedido where PedidoID = 63386

EXEC sp_spaceused;

1,315,952 KB
548,120 KB
[wms_test_15012019]
[wms_test_29_01_2019]
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.Inventario WHERE InventarioID IN (1663770,
1663779)
SELECT TOP 200 * FROM wms.dbo.ItemInventario WHERE CodProducto = '26-4412-BC'
SELECT TOP 200 * FROM wms.dbo.SysTempSalidas WHERE ItemInventarioID IN (1699577)
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.SysTempSalidas WHERE TransaccionID = 104745
SELECT TOP 200 * FROM wms.dbo.Transacciones WHERE TransaccionID = 113707
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Transacciones WHERE IDTipoTransaccion = 'IN' AND year(FechaTransaccion) = 2019
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Transacciones WHERE --IDTipoTransaccion = 'IN' 
 year(FechaTransaccion) = 2019 AND ClienteID = 1134 order by ClienteID
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.DetalleTransacciones WHERE TransaccionID IN (1663770, 1663779)
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.Inventario WHERE ClienteId IS NULL AND year(FechaCreacion) = 2019
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.DetalleTransacciones WHERE InventarioID IN (1663770,
1663779)
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Transacciones WHERE TransaccionID = 105001

---
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.Inventario WHERE InventarioID IN (1663770, 1663779)
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.ItemInventario WHERE CodProducto = 'E11084800'
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.DetalleTransacciones WHERE InventarioID IN (1663770, 1663779)
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Transacciones WHERE TransaccionID = 105001
----
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Inventario WHERE Existencia IS NOT NULL AND YEAR(FechaCreacion) = 2018 ORDER BY Existencia DESC

SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.ItemInventario WHERE InventarioID = 1590313
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.Inventario WHERE InventarioID IN (1590313)
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.DetalleTransacciones WHERE InventarioID IN (1590313)
SELECT TOP 200 * FROM wms_test_29_01_2019.dbo.Transacciones WHERE TransaccionID IN (SELECT TransaccionID FROM wms_test_29_01_2019.dbo.DetalleTransacciones WHERE InventarioID IN (1590313))

--16HB4326-7
EXEC [SP_GetExistenciasExtern] 1345
--1967856449
--16HB4326-7	KIDS SOCKS BOYS HANES COMFORBLEND P6S NPI, ANKLE MEDIUM	0	CAJA	NULL

--select * from PAYLESS_ProdPriori where BarCode = '7392819938' order by OID desc
--delete from PAYLESS_ProdPriori where Id = 2434
--update PAYLESS_ProdPriori SET Periodo = '08/04/2019'
SELECT TOP 200 * FROM [wms_test_29_01_2019].dbo.Producto WHERE CodProducto = '7658893139'
select * from PAYLESS_ProdPrioriM
select * from PAYLESS_ProdPrioriDet where IdPAYLESS_ProdPrioriM = 10 and Barcode = '1983832691'
where IdPAYLESS_ProdPrioriM = 1
--truncate table PAYLESS_ProdPrioriDet
--truncate table PAYLESS_ProdPrioriM
select * from wms_test_29_01_2019.dbo.Producto where Descripcion like'%GEMU3131%'
select * from PAYLESS_ProdPrioriDet where IdTransporte = 9
select * from PAYLESS_ProdPrioriArchM
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchDet
select * from EdiDb.dbo.PAYLESS_Transporte
--8	SMLU 789569-3
--9	SMLU 796382-8
select * from EdiDb.dbo.PAYLESS_ProdPrioriM
select SUBSTRING(s.bc, 1, 4) bc, count(*) from (
select distinct barcode bc from EdiDb.dbo.PAYLESS_ProdPrioriDet 
where IdPAYLESS_ProdPrioriM = 4 and IdTransporte in (9)
) s
group by SUBSTRING(s.bc, 1, 4) order by SUBSTRING(s.bc, 1, 4)
select * from PAYLESS_ProdPrioriArchDet
select * from EdiDb.dbo.PAYLESS_ProdPrioriDet where barcode like '7370872774'
select * from PAYLESS_ProdPrioriArchDet where barcode like '7370872774'
select distinct 
--(select T.Transporte from EdiDb.dbo.PayLess_Transporte T where T.Id = D.IdTransporte) Transporte, 
D.barcode from EdiDb.dbo.PAYLESS_ProdPrioriArchDet D where D.barcode not in(
select barcode from  PAYLESS_ProdPrioriDet 
) order by D.barcode

select distinct barcode from EdiDb.dbo.PAYLESS_ProdPrioriDet where barcode like '7370%' and IdTransporte = 4
select * from EdiDb.dbo.PAYLESS_Tiendas order by TiendaID
--update EdiDb.dbo.IEnetUsers SET ClienteID = 1432
-- 7379 , 7383 pasan a ser TGU, SAP
select * from EdiDb.dbo.PayLess_Transporte
select * from EdiDB.dbo.PAYLESS_ProdPrioriDet where IdTransporte = 7
--MLU 796554-8
select barcode, count(*) from PAYLESS_ProdPrioriArchDet where IdM = 1 and barcode like'1967%' group by barcode
insert into PAYLESS_ProdPrioriArchDet(IdM, barcode)
VALUES(1, '1983832691')
select * from EdiDb.dbo.PedidosExternos 
select * from EdiDb.dbo.PedidosDetExternos where PedidoId = 1022
--delete from EdiDb.dbo.PedidosExternos where Id in (2, 3) 
--delete from EdiDb.dbo.PedidosDetExternos where PedidoId in (2, 3) 
exec SP_GetExistenciasExtern 1345
select top 10 * from wms_test_29_01_2019.dbo.Inventario where ClienteId = 1345 and InventarioID = 1675373
select top 10 * from wms.dbo.ItemInventario where CodProducto = '1967856448'
select top 10 * from wms_test_29_01_2019.dbo.ItemInventario order by Existencia deSC
select top 10 * from wms_test_29_01_2019.dbo.Transacciones where TransaccionID = 105972
select top 10 * from wms_test_29_01_2019.dbo.Transacciones where ClienteID = 1345
select top 1000 * from wms_test_29_01_2019.dbo.DtllItemTransaccion where TransaccionID = 105970
select top 10 * from wms_test_29_01_2019.dbo.DetalleTransacciones order by TransaccionID DESC

SELECT top 10 
Ii.CodProducto,
Ii.Existencia,
T.IDTipoTransaccion,
T.ClienteID,
T.Fechacrea
FROM wms_test_29_01_2019.dbo.ItemInventario Ii
join wms_test_29_01_2019.dbo.DetalleTransacciones Dt
	ON Dt.InventarioID = Ii.InventarioID
join wms_test_29_01_2019.dbo.Transacciones T
	ON T.TransaccionID = Dt.TransaccionID
where Ii.CodProducto = '1967856448'


select top 10 * from wms_test_29_01_2019.dbo.Producto where CodProducto = '06-PSS-026-18'
select top 10 * from wms_test_29_01_2019.dbo.Producto where CodProducto = '12-PSS-047-18'
select top 10 * from wms_test_29_01_2019.dbo.Producto where CodProducto = '419532811'
select top 10 * from wms_test_29_01_2019.dbo.Producto where CodProducto = 'N� 2083'
select top 10 * from wms_test_29_01_2019.dbo.Producto where CodProducto = 'N� 048 '
select top 10 * from wms_test_29_01_2019.dbo.Producto where CodProducto = '07-PSS-031-18'

select top 10 * from wms_test_29_01_2019.dbo.ItemInventario where CodProducto = '06-PSS-026-18'
ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario NOCHECK CONSTRAINT FK_ItemInventario_Producto
update wms_test_29_01_2019.dbo.ItemInventario SET CodProducto = '1983832690' where CodProducto = '1953849670'
update wms_test_29_01_2019.dbo.SysTempSalidas SET CodProducto = '1983832690' where CodProducto = '1953849670'
update wms_test_29_01_2019.dbo.DtllPedido SET CodProducto = '1983832690' where CodProducto = '1953849670'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1983832690' where CodProducto = '1953849670'
ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario CHECK CONSTRAINT FK_ItemInventario_Producto

ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario NOCHECK CONSTRAINT FK_ItemInventario_Producto
update wms_test_29_01_2019.dbo.ItemInventario SET CodProducto = '1983832611' where CodProducto = '1953850339'
update wms_test_29_01_2019.dbo.SysTempSalidas SET CodProducto = '1983832611' where CodProducto = '1953850339'
update wms_test_29_01_2019.dbo.DtllPedido SET CodProducto = '1983832611' where CodProducto = '1953850339'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1983832611' where CodProducto = '1953850339'
ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario CHECK CONSTRAINT FK_ItemInventario_Producto

ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario NOCHECK CONSTRAINT FK_ItemInventario_Producto
update wms_test_29_01_2019.dbo.ItemInventario SET CodProducto = '1983832700' where CodProducto = '1953850346'
update wms_test_29_01_2019.dbo.SysTempSalidas SET CodProducto = '1983832700' where CodProducto = '1953850346'
update wms_test_29_01_2019.dbo.DtllPedido SET CodProducto = '1983832700' where CodProducto = '1953850346'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1983832700' where CodProducto = '1953850346'
ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario CHECK CONSTRAINT FK_ItemInventario_Producto

ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario NOCHECK CONSTRAINT FK_ItemInventario_Producto
update wms_test_29_01_2019.dbo.ItemInventario SET CodProducto = '1983832691' where CodProducto = '1983832719'
update wms_test_29_01_2019.dbo.SysTempSalidas SET CodProducto = '1983832691' where CodProducto = '1983832719'
update wms_test_29_01_2019.dbo.DtllPedido SET CodProducto = '1983832691' where CodProducto = '1983832719'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1983832691' where CodProducto = '1983832719'
ALTER TABLE wms_test_29_01_2019.dbo.ItemInventario CHECK CONSTRAINT FK_ItemInventario_Producto

update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1953850339' where CodProducto = '06-PSS-027-18'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1953850346' where CodProducto = '06-PSS-028-18'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1953850344' where CodProducto = '07-PSS-029-18'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1953850347' where CodProducto = '07-PSS-030-18'
update wms_test_29_01_2019.dbo.Producto SET CodProducto = '1953850340' where CodProducto = '07-PSS-031-18'

1953849670
1953850339
1953850346
1953850344
1953850347
1953850340
--1953850349

1983832690
1983832611
1983832700
1983832719
1983832691

select * from PAYLESS_Tiendas

select * from PAYLESS_Reportes
--update PAYLESS_Reportes SET FechaGen = '07/05/2019 08:49'


select GETDATE() - convert(datetime,'30/12/2018', 103)


SELECT [De0].[Id], [De0].[Barcode], [De0].[Cargada], [De0].[Categoria], [De0].[CP], [De0].[Departamento], [De0].[Estado], 
[De0].[Etiquetada], [De0].[IdPAYLESS_ProdPrioriM], [De0].[IdTransporte], 
[De0].[Lote], [De0].[M3], [De0].[OID], [De0].[Peso], [De0].[Pickeada], [De0].[PoolP], [De0].[Preinspeccion], [De0].[Pri], 
[De0].[Producto], [De0].[Talla]
FROM [PAYLESS_ProdPrioriDet] AS [De0]

select * from EdiDB.[dbo].[PAYLESS_PeriodoTransporte]

--insert into EdiDB.[dbo].[PAYLESS_PeriodoTransporte] (Periodo, IdTransporte)
--select distinct M.Periodo, D.IdTransporte  from EdiDb.[dbo].[PAYLESS_ProdPrioriDet] D
--join EdiDb.[dbo].[PAYLESS_ProdPrioriM] M
--ON D.IdPAYLESS_ProdPrioriM = M.Id




SELECT P.ClienteID
		,P.PedidoBarcode
		,CONVERT(VARCHAR, P.fechapedido, 103) FechaPedido
		,E.Estatus
		,B.NomBodega
		,R.Regimen
		,Dp.CodProducto
		,Dp.Cantidad
		,P.Observacion
		,P.PedidoID
	FROM wms.dbo.Pedido AS P WITH (NOLOCK)
	INNER JOIN wms.dbo.Estatus AS E WITH (NOLOCK) ON E.EstatusID = P.EstatusID
	INNER JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) ON B.BodegaID = P.BodegaID
	INNER JOIN wms.dbo.Regimen AS R WITH (NOLOCK) ON R.IDRegimen = P.RegimenID
	INNER JOIN wms.dbo.DtllPedido AS Dp WITH (NOLOCK) ON Dp.PedidoID = P.PedidoID
	WHERE ClienteID = 618
	ORDER BY P.fechapedido DESC
--2114
--24063
--26488
select count(*) from (
select --Gc.Nombre, 
(select top 1 C.Nombre 
from EdiDb.[dbo].[PAYLESS_Tiendas] T
join wms.dbo.Clientes C
	on C.ClienteID = T.ClienteID
	AND T.TiendaId = SUBSTRING(Ad.BarCode, 0, 5)) NomCliente,
Ex.*
from EdiDb.dbo.PAYLESS_ProdPrioriArchDet Ad
join EdiDb.dbo.PAYLESS_ProdPrioriArchM Am 
	on Am.Id = Ad.IdM AND Am.Periodo = '13/05/2019'
join EdiDb.dbo.PAYLESS_ProdPrioriDet Ex 
	on Ex.Barcode = Ad.barcode AND Ex.IdTransporte = 9
	) r1

select  C.ClienteID, T.TiendaId, C.Nombre 
from EdiDb.[dbo].[PAYLESS_Tiendas] T
join [wms_test_29_01_2019].dbo.Clientes C
	on C.ClienteID = T.ClienteID

select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchDet where IdM = 1
select * from EdiDb.dbo.PAYLESS_ProdPrioriM where id = 9
select * from EdiDb.[dbo].[PAYLESS_Transporte]
select * from EdiDb.dbo.PAYLESS_ProdPrioriDet 
where IdPayless_prodprioriM = 2 AND IdTransporte = 4

--7365822058

select --Gc.Nombre, 
(select top 1 C.Nombre 
from EdiDb.[dbo].[PAYLESS_Tiendas] T
join wms.dbo.Clientes C
	on C.ClienteID = T.ClienteID
	AND T.TiendaId = SUBSTRING(Ad.BarCode, 0, 5)) NomCliente,
Ex.*
from EdiDb.dbo.PAYLESS_ProdPrioriArchDet Ad
join EdiDb.dbo.PAYLESS_ProdPrioriArchM Am 
	on Am.Id = Ad.IdM AND Am.Periodo = '13/05/2019'
join EdiDb.dbo.PAYLESS_ProdPrioriDet Ex 
	on Ex.Barcode = Ad.barcode AND Ex.IdTransporte = 9
--24063
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM where Periodo = '13/05/2019'
update EdiDb.dbo.PAYLESS_ProdPrioriArchM SET PorcValidez = null where Id = 4
--6046
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchDet
select * from EdiDb.dbo.PAYLESS_ProdPrioriM
--delete from EdiDb.dbo.PAYLESS_ProdPrioriM where Id = 1
select count(*) from (select distinct Barcode from EdiDb.dbo.PAYLESS_ProdPrioriDet) a1
select count(*) from (select distinct Barcode from EdiDb.dbo.PAYLESS_ProdPrioriArchDet) a1
--delete from EdiDb.dbo.PAYLESS_ProdPrioriDet WHERE IdPAYLESS_ProdPrioriM = 1
--6046
--2598
--6034
select count(*) from (
select distinct p1.barcode from EdiDb.dbo.PAYLESS_ProdPrioriArchDet p1,
EdiDb.dbo.PAYLESS_ProdPrioriDet p2
where p1.barcode = p2.Barcode) ca1
--6033 match

select * from wms.dbo.Producto where CodProducto = '7369829284'

--Este voy a probar a ver si soy sysadmin
--TS8CACC16-900-155 
'CAMISA PLATINUM SPORT MC '
select * from wms.dbo.Producto where CodProducto = 'TS8CACC16-900-155'

--update wms.dbo.Producto SET Descripcion = 'CAMISA PLATINUM SPORT MC '
--where CodProducto = 'TS8CACC16-900-155'

select * from wms.dbo.Transacciones T where ClienteID = 1181
select * from wms.dbo.Locations
select * from wms.dbo.Bodegas where LocationId =7
select * from wms_test.dbo.Racks where BodegaId = 81
select * from wms_test.dbo.Racks order by Rack desc
select * from wms.dbo.UnidadMedida where UnidadMedida like'%bult%'
select * from wms.dbo.Transacciones T where T.Usuariocrea = 'Hilmer'
select count(*) from wms_test_29_01_2019.dbo.Producto
select  * from wms_test_29_01_2019.dbo.Producto order by Fecha DESC
select max(InventarioID) from wms.dbo.Inventario
--1820626
--1820687
--1820858
---1821022
--1821028
select count(*)
select * from wms.dbo.DocumentosxTransaccion where INFORME_ALMACEN like'%GLCHN33-5-008%' order by IDDocxTransaccion desc
select top 100 * from wms.dbo.Bodegas
select top 10 * from wms.dbo.Regimen
select top 10 * from wms.dbo.Producto where CodProducto = '7380872774'
select top 10 * from wms.dbo.Producto where Descripcion = '19003833B PORTA ANILLOS MANOS GRD'
--delete from wms.dbo.Producto where Descripcion = '19003833B PORTA ANILLOS MANOS GRD'
select top 1 * from wms.dbo.Inventario where CodProducto = '7380872774'
select top 1 * from wms.dbo.ItemInventario where CodProducto = '7380872774'
--update wms.dbo.Producto SET CodProducto = '19003833B' where Descripcion = '19003833B PORTA ANILLOS MANOS GRD'
select top 10 * from wms.dbo.Inventario where InventarioID in (
1837157)
select top 10 * from wms.dbo.ItemInventario where CodProducto = '7366830856' 

select top 10 * from wms_test.dbo.Transacciones where TransaccionID = 101029
select TransaccionID, NoTransaccion, FechaTransaccion, IdTipoTransaccion, PedidoId, BodegaId, RegimenId, ClienteId, Total, TipoIngreso, UsuarioCrea, FechaCrea, Observacion, EstatusId, OperarioId, TipoPicking, ExportadorId, DestinoId, TransportistaId, Pais_Orig, Adu_fro, Placa, Marchamo, Contenedor, Cod_Motoris, Remolque, RecivingCliente, FechaReciving, FacturaId, IdrControl from dbo.Transacciones where TransaccionId = 101029

select top 20 * from wms.dbo.Estatus
select top 20 * from wms.dbo.Transacciones where ClienteId = 1432 
AND IDTipoTransaccion = 'SA'
order by Fechacrea DESC --IN115900
select top 20 * from wms.dbo.Transacciones where TransaccionID = 116804
 order by TransaccionId DESC
--update wms.dbo.Transacciones SET PedidoId = 69251 where TransaccionID = 116085
--salida SA115914 y SA116005 son salidas 
--reuso 116085
--116160 mal receiving
--delete from wms.dbo.Transacciones where TransaccionID in (
--116137
--)
--PedidoId = 69104
--Problema con receiving, la TransaccionId 115912 esta bien, pero la 116160 da mal receiving
select * from wms.dbo.DocumentosxTransaccion where INFORME_ALMACEN like'%GLCHN%'
'GLCHN33-5-007'
'GLCHN33-5-007'
select * from wms.dbo.DocumentosxTransaccion where TransaccionID = 116137
--update wms.dbo.DocumentosxTransaccion SET INFORME_ALMACEN = 'GLCHNTGU-05-003' where TransaccionID = 116137
--delete from wms.dbo.DocumentosxTransaccion where IDDocxTransaccion = 31128
select top 10 * from wms.dbo.Pedido where ClienteId = 1432 or PedidoId = 69334
--update wms.dbo.Pedido SET PedidoBarcode = 'PD69251' where PedidoId = 69251 AND ClienteId = 1432
--delete from wms.dbo.Pedido where PedidoId in (69160)
select top 10 * from wms.dbo.Estatus
select top 10000 * from wms.dbo.SysTempSalidas where PedidoId = 69335
--delete from  wms.dbo.SysTempSalidas where PedidoId in (69160)
select top 10000 * from wms.dbo.DtllPedido where PedidoId = 69334
--delete from wms.dbo.DtllPedido where PedidoId in (69160)
select top 100000 * from wms.dbo.DetalleTransacciones where TransaccionID = 116524
--delete from wms.dbo.DetalleTransacciones where TransaccionID = 116137
select top 1000000 * from wms.dbo.DtllItemTransaccion where TransaccionID = 116367
--delete from wms.dbo.DtllItemTransaccion where TransaccionID = 116137
--116081
--116074
--blocking
exec sp_who2
select * from wms.dbo.embalaje
select * from wms.dbo.Inventario where InventarioID in (1869645)
select * from wms.dbo.ItemInventario WITH(NOLOCK) where InventarioID IN (
select InventarioID from wms.dbo.DetalleTransacciones WITH(NOLOCK) where TransaccionID = 116551
) 
--853
--and Existencia > 1  
order by CodProducto
--update wms.dbo.Inventario SET Existencia = 1 where InventarioID = 1869645
--update wms.dbo.ItemInventario SET Existencia = 1 where ItemInventarioID = 1850341
--7380872774
select * from wms.dbo.ItemInventario where ItemInventarioId = 1834892
--update wms.dbo.ItemInventario SET CodProducto = '7366831282' where ItemInventarioId = 1834892
116209
116206
116201
116173
116272
116271
--Entradas a TEGUS
--116137 - 1688
--116081 - 1586
--116074 - 1862
--and lote = ' 175771'
order by CodProducto
--InventarioId = 1841744
select * from wms.dbo.ItemInventario where ItemInventarioId in (1806365,
1812398,
1826261,
1829322)
select * from wms.dbo.Inventario where InventarioID in (
select InventarioID from wms.dbo.ItemInventario where ItemInventarioId in (1806365)
)
select I.* from 
wms.dbo.Inventario I
JOIN wms.dbo.ItemInventario Ii
	ON Ii.InventarioID = I.InventarioID
WHERE Ii.ItemInventarioId in (1806365)
 
--update wms.dbo.ItemInventario SET numero_oc = 8, lote = '572403', modelo = 'PRR', cod_equivale = '7365 - PAYLESS SHOE SOURCE', estilo = '149089', pais_orig = 166, color = 'SMLU 789569-3'
--where ItemInventarioId = 1833186
select * from wms.dbo.Producto where CodProducto = '7370872774'
--INSERT INTO wms.dbo.Producto(CodProducto, Descripcion, UnidadMedida, ClienteID, EstatusID, CategoriaID, CantMinima, Fecha, Comentario, stock_maximo, descargoid, partida)
--VALUES ('7366831282', 'ACCESORIOS', 1, 1432, 1, 10, 0, '21-05-2019', 'IngresoM Hilmer', 0, 1, null)
--update wms.dbo.ItemInventario Set CodProducto = '7366831282' where ItemInventarioId = 1823077
AND CodProducto = '7366831282'
select * from EdiDb.dbo.PAYLESS_ProdPrioriM
--update EdiDb.dbo.PAYLESS_ProdPrioriM SET ClienteId = 1432
--delete from EdiDb.dbo.PAYLESS_ProdPrioriDet where [IdPAYLESS_ProdPrioriM] in (5, 6)
delete from EdiDb.dbo.PAYLESS_ProdPrioriM where id in (5, 6)
select count(*) from EdiDb.dbo.PAYLESS_ProdPrioriDet where [IdPAYLESS_ProdPrioriM] in (5, 6)
--3441
--3441
--4739
--6882
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchDet where Barcode = '7365822070'
select * from EdiDb.dbo.PAYLESS_ProdPrioriDet where Barcode = '7377820804'

select * from EdiDb.dbo.PAYLESS_ProdPrioriDet where Barcode

select distinct IdTransporte, Barcode from EdiDb.dbo.PAYLESS_ProdPrioriDet where Barcode like '7365%'
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
--delete from EdiDb.dbo.PAYLESS_ProdPrioriArchM where Id in (8)
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchDet where IdM = 8
--delete from EdiDb.dbo.PAYLESS_ProdPrioriArchDet where IdM = 8
--update EdiDb.dbo.PAYLESS_ProdPrioriArchM SET PorcValidez = null where Id = 1
select * from EdiDb.dbo.PAYLESS_ProdPrioriM
--delete from EdiDb.dbo.PAYLESS_ProdPrioriM where Id = 7
select * from EdiDb.dbo.PAYLESS_ProdPrioriDet where IdPAYLESS_ProdPrioriM = 7
--delete from EdiDb.dbo.PAYLESS_ProdPrioriDet where IdPAYLESS_ProdPrioriM = 7
select distinct barcode bc from EdiDb.dbo.PAYLESS_ProdPrioriDet 
where IdPAYLESS_ProdPrioriM = 4 and IdTransporte in (8)
and barcode not in
(
select barcode from EdiDb.dbo.PAYLESS_ProdPrioriArchDet
where IdM = 6)
select * from wms.dbo.Pedido where PedidoId = 69328
select * from 
wms.dbo.DetalleTransacciones Dt
join wms.dbo.ItemInventario Ii on Ii.InventarioID = Dt.InventarioID
join wms.dbo.Transacciones t
on t.TransaccionID = Dt.TransaccionID
where Ii.CodProducto = '7368865833'
--7381831861 tiene dos entradas pero no la salida...
select * from wms.dbo.Racks where Rack = 11759
select * from wms.dbo.Transacciones where usuarioCrea = 'RPERDOMO' and IDTipoTransaccion = 'IN' and ClienteID = 1432 order by FechaCrea DESC
select * from wms.dbo.Transacciones WITH(NOLOCK) where usuarioCrea = 'Hilmer' order by FechaCrea DESC
select * from wms.dbo.Transacciones WITH(NOLOCK) where IDTipoTransaccion = 'SA' and ClienteID = 1432 order by PedidoId DESC
--Salidas ultimo es 116661 y est� ok
EXEC SP_WHO2
select * from wms.dbo.DocumentosxTransaccion where TransaccionID = 116834
select * from wms.dbo.DocumentosxTransaccion where TransaccionID in (
select TransaccionID from wms.dbo.Transacciones WITH(NOLOCK) where ClienteID = 1432
)
--update wms.dbo.DocumentosxTransaccion SET IM_5 = null WHERE TransaccionID in (116074, 116395)
--update wms.dbo.Transacciones Set Observacion = '' where TransaccionID = 116395
--delete from wms.dbo.Transacciones where TransaccionID in (116389, 116368, 116367)
-- 7370872774 es la que viene duplicada, la 7380 vino junto
select top 2 * from wms.dbo.SysTempSalidas where CodProducto = '7380872774'
--7369829033 duplicado en archivos de escaners ?
select * from wms.dbo.Producto where CodProducto = '7380872774'
select * from wms.dbo.Transacciones where TransaccionId in (115884,
116074,
116272)
--update wms.dbo.Transacciones SET FechaTransaccion = '16-05-2019' where TransaccionID = 116081
--7366831282 no se escaneo, falta cargar en WMS
--update EdiDb.dbo.PAYLESS_Tiendas SET ClienteID = 1432
--8	SMLU 789569-3
--9	SMLU 796382-8
exec SP_GetPaylessProdPrioriByPeriodAndIdTransport '17/05/2019', 9
	
select top 100 * from EdiDb.dbo.PAYLESS_ProdPrioriM
select top 10 * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
select distinct COUNT(*) from EdiDb.dbo.PAYLESS_ProdPrioriDet where Barcode like '7365%'
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchDet where IdM = 1
--truncate table EdiDb.dbo.PAYLESS_ProdPrioriArchM
--truncate table EdiDb.dbo.PAYLESS_ProdPrioriArchDet
--truncate table EdiDb.dbo.PAYLESS_ProdPrioriM
--truncate table EdiDb.dbo.PAYLESS_ProdPrioriDet

7366831282

exec [spGeneraSalida]
'10-05-2019',
'7380872774',
81,
2,
1432,
7,
0

select * from EdiDb.[dbo].[IEnetUsers]
--update EdiDb.dbo.IEnetUsers SET TiendaId = 7365
--where Id = 1
select * from EdiDb.[dbo].[IEnetGroups]
select * from EdiDb.[dbo].PAYLESS_Tiendas
select distinct Color, CodProducto from wms.dbo.Producto where CodProducto like '7370%'
select top 20 * from wms.dbo.Transacciones where ClienteID = 1432
select top 20 * from wms.dbo.DtllItemTransaccion
select top 20 * from wms.dbo.DocumentosxTransaccion order by IDDocxTransaccion desc
select top 20 * from wms.dbo.Inventario

select * 
from wms.dbo.ItemInventario Ii
where Ii.CodProducto like '7370%' and Ii.color  = 'DRYU 961708-5'

--Obtener numeros de factura, bodega, 
select distinct
B.NomBodega,
DoT.Informe_Almacen, 
DoT.Fact_Comercial,
T.NoTransaccion,
Ii.color Transporte,
Ii.CodProducto,
CONVERT(VARCHAR(64), T.Fechacrea, 109) FechaCrea,
CONVERT(VARCHAR(64), Dt.FechaItem, 109) FechaIngreso,
(
	SELECT distinct P2.PedidoBarcode
	FROM wms.dbo.Transacciones T2
	JOIN wms.dbo.Pedido P2
		ON P2.PedidoID = T2.PedidoID
	JOIN wms.dbo.Bodegas B2
		ON B2.BodegaID = T2.BodegaID
	JOIN wms.dbo.DetalleTransacciones Dt2
		ON Dt2.TransaccionID = T2.TransaccionID
	JOIN wms.dbo.ItemInventario Ii2
		ON Ii2.InventarioID = Dt2.InventarioID
	WHERE T2.ClienteID = 1432
	and B2.BodegaID = 82
	AND T2.IDTipoTransaccion = 'SA'
	AND Ii2.CodProducto = Ii.CodProducto
) PedidoBarcode
from wms.dbo.Transacciones T
JOIN wms.dbo.DocumentosxTransaccion DoT
	ON DoT.TransaccionId = T.TransaccionId
JOIN wms.dbo.Bodegas B
	ON B.BodegaID = T.BodegaID
JOIN wms.dbo.DetalleTransacciones Dt
	ON Dt.TransaccionID = T.TransaccionID
JOIN wms.dbo.ItemInventario Ii
	ON Ii.InventarioID = Dt.InventarioID
where T.ClienteID = 1432
and B.BodegaID = 81
AND T.IDTipoTransaccion = 'IN'
ORDER BY B.NomBodega, T.NoTransaccion, Ii.color, Ii.CodProducto





