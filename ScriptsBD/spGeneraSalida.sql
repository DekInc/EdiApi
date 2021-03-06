USE [wms]
GO
/****** Object:  StoredProcedure [dbo].[spGeneraSalida]    Script Date: 22/5/2019 15:21:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[spGeneraSalida]
 @fecha_sal as date=null,
 @cod_producto as varchar(50)=null,
 @bodegaID as int=null,
 @regimenID as int=null,
 @clienteID as int=null,
 @locationID as int=null,
 @rackID as int=null

as


if @rackID=0
 set @rackID=null

 if @locationID=0
 set @locationID=null

 if @regimenID=0
 set @regimenID=null

 if @clienteID=0
 set @clienteID=null

 if @bodegaID=0
 set @bodegaID=null
if @fecha_sal='01/01/01'
set	@fecha_sal=null

SELECT ii.InventarioID
      ,ii.ItemInventarioID
      ,ii.CodProducto
      ,p.descripcion
      ,II.CantidadInicial
      ,isnull(II.CantidadInicial/(i.CantidadInicial/nullif(i.articulos,0)),0) as bultosinicial
      ,II.CantidadInicial*II.precio as valorInicial 
      ,i.rack  
      ,r.NombreRack
      ,isnull(i.CantidadInicial/nullif(i.articulos,0),0) as uxb 
      ,ii.CantidadInicial-isnull(Sy_1.reservado,0) as existencia
	  ,isnull(Sy_2.reservado,0) as reservado
      ,isnull((ii.CantidadInicial-isnull(Sy_1.reservado,0))/(i.CantidadInicial/nullif(i.articulos,0)),0) as BultosExis
      ,(ii.CantidadInicial-isnull(Sy_1.reservado,0))*ii.precio as valorexis 
      ,ii.numero_oc
      ,ii.lote
      ,ii.fecha_vcmto
      ,ii.modelo
      ,ii.cod_equivale,
	  ii.estilo,paises.nompais,
      dt.INFORME_ALMACEN as nodt,
	  dt.FE_INFORME_ALMACEN as Fecha_Dt,
	  dt.IM_5 as nodm,
	  dt.FE_IM_5 as fecha_dm,
      0000000000000000000.00 as solicitado,
	  ii.precio,
	  ii.observacion,
	  t.fechatransaccion,
	  u.UnidadMedida,
	  iI.fechaitem,
	  t.bodegaid,
	  t.regimenid, 
      isnull((ii.CantidadInicial-isnull(Sy_1.reservado,0))/(i.CantidadInicial/nullif(i.peso,0)),0) as peso, 
      isnull((ii.CantidadInicial-isnull(Sy_1.reservado,0))/(i.CantidadInicial/nullif(i.volumen,0)),0) as cbm,
      t.clienteid,
	  c.nombre as nombrecliente,
	  B.nombodega, 
      i.barcode,
	  t.NoTransaccion,
	  isnull(ii.cantidadinicial/(i.CantidadInicial/nullif(i.volumen,0)),0) as cbm_ini,
      isnull(ii.cantidadinicial/(i.CantidadInicial/nullif(i.peso,0)),0) as peso_ini,rg.regimen,
	  t.contenedor
      FROM ItemInventario as ii 
    inner join inventario as i on i.InventarioID=ii.InventarioID  
    inner join producto as p on p.codproducto=ii.codproducto 
    left join racks as r on r.rack=i.rack 
	left join metodo_descargo md on md.descargoID=p.descargoid
    inner join DetalleTransacciones as d1 on d1.InventarioID=i.InventarioID 
    inner join transacciones as T on t.TransaccionID=d1.TransaccionID 
    left join Bodegas as B on b.bodegaid=t.bodegaid 
    Inner Join Clientes As C On C.ClienteID = T.ClienteID 
    left Join DocumentosxTransaccion As dt On dt.transaccionid = t.transaccionid 
    left join paises on paises.paisid=ii.pais_orig 
    left join unidadmedida as u on u.unidadmedidaid=p.unidadmedida 
    inner join Regimen as rg on rg.IDRegimen=t.RegimenID 
	inner join locations l on l.locationid=b.locationid
    LEFT OUTER JOIN (SELECT Sy.InventarioID, Sy.ItemInventarioID, Sy.CodProducto, SUM(ISNULL(Sy.Cantidad, 0)) AS Reservado 
                     FROM SysTempSalidas AS Sy 
					 INNER JOIN Pedido AS Pe ON Pe.PedidoID = Sy.PedidoID 
					 INNER JOIN Transacciones as t ON t.TransaccionID=sy.TransaccionID and t.IDTipoTransaccion in ('SA','XL')
                   --  WHERE (Pe.EstatusID  )
                     GROUP BY Sy.InventarioID, sy.ItemInventarioID, Sy.CodProducto) AS Sy_1 ON Sy_1.InventarioID = I.InventarioID AND 
                              Sy_1.ItemInventarioID = II.ItemInventarioID And Sy_1.CodProducto = II.CodProducto 
	 LEFT OUTER JOIN (SELECT Sy.InventarioID, Sy.ItemInventarioID, Sy.CodProducto, SUM(ISNULL(Sy.Cantidad, 0)) AS Reservado 
                     FROM SysTempSalidas AS Sy 
					 INNER JOIN Pedido AS Pe ON Pe.PedidoID = Sy.PedidoID 
					 INNER JOIN Transacciones as t ON t.TransaccionID=sy.TransaccionID and t.IDTipoTransaccion in ('SA','XL')
                     WHERE (Pe.EstatusID not in (9,10) )
                     GROUP BY Sy.InventarioID, sy.ItemInventarioID, Sy.CodProducto) AS Sy_2 ON Sy_2.InventarioID = I.InventarioID AND 
                              Sy_2.ItemInventarioID = II.ItemInventarioID And Sy_2.CodProducto = II.CodProducto 
     where ii.CodProducto=isnull(@cod_producto,ii.CodProducto) and 
	       (isnull(ii.cantidadinicial,0) -isnull(sy_1.Reservado,0))>0  and
		   T.IDTipoTransaccion IN ('IN') and
		   t.ClienteID=isnull(@clienteID,t.ClienteID) and 
		   t.BodegaID=isnull(@bodegaID,t.BodegaID) and 
		   t.RegimenID=isnull(@regimenID,t.RegimenID) and
		   i.rack=isnull(@rackID,i.Rack) and
		   b.locationid=isnull(@locationID,b.locationid) and
		   t.EstatusID>=l.IDinvShow and
		  case when p.descargoid =1 then t.FechaTransaccion
			   when p.descargoid =2 then ii.fecha_vcmto
		       when p.descargoid =3 then t.FechaTransaccion end <= @fecha_sal           
        order by case when p.descargoid in (1,3) then t.FechaTransaccion else ii.fecha_vcmto end 

