--use wms
SELECT ii.CodProducto,ii.Descripcion,
 sum(IsNull(ii.Declarado,0)) as declarado,
 sum(IsNull(ii.Auditado,0)) as auditado,
 sum(IsNull(ii.averia,0)) as averia,
 sum(IsNull(ii.Existencia,0)) as existencia,
 sum(IsNull(ii.Precio*ii.Auditado,0)) as valor,
 SUM(IsNull(Volumen, 0)) As Cbm,
 (IsNull(i.cantidadinicial,0)/IsNull(nullif(Articulos,0), 1)) As uxb, sum(i.Articulos) as bultos,
 SUM(IsNull(peso, 0)) As peso,c.Nombre as cliente, e.nombrexp as exportador,
 t.FechaTransaccion,t.IDTipoTransaccion,
 b.NomBodega As Bodega,l.dsclocation as locacion , R.Regimen, 
 d.INFORME_ALMACEN as nodt,d.FE_INFORME_ALMACEN as Fecha_Dt,d.IM_5 as nodm,d.FE_IM_5 as fecha_dm, 
 d.FACT_COMERCIAL as nofac,d.FE_FACT_COMERCIAL as fecha_fac,u.nomusr,Um.unidadmedida
  ,ii.numero_oc,ii.lote,ii.fecha_vcmto ,ii.modelo,ii.cod_equivale,ii.estilo,paises.nompais,ii.precio,tb.unidadmedida as embalaje
  ,tr.nombre as NomTransporte,t.contenedor,t.fechareciving ,ii.color
 FROM ItemInventario as ii
  inner join inventario as i on i.InventarioID=ii.InventarioID 
 inner join DetalleTransacciones as dt on dt.InventarioID=ii.InventarioID 
  inner join Transacciones as t on t.TransaccionID=dt.TransaccionID and t.IDTipoTransaccion in ('IN')
  inner join Bodegas as b on b.BodegaID=t.BodegaID
  inner join Clientes as c on c.ClienteID=t.ClienteID
  inner join Regimen as r on r.IDRegimen=t.RegimenID
  inner join locations as l on l.locationid=b.locationid
  inner join DocumentosxTransaccion as d on d.TransaccionID=t.TransaccionID
  inner join producto as pr on pr.codproducto=II.codproducto
  left outer join exportador as e on e.exportadorID=t.exportadorID
  left outer join usrsystem as u on u.codusr=t.operarioid
  left join paises on paises.paisid=ii.pais_orig
  left join transportista as tr on tr.transportistaid=t.transportistaID
  LEFT OUTER Join UnidadMedida As um On um.UnidadMedidaID = PR.UnidadMedida
  LEFT OUTER Join UnidadMedida As tb On tb.UnidadMedidaID = i.tipobulto 
  WHERE d.INFORME_ALMACEN=''
  group by  b.NomBodega ,l.dsclocation ,r.Regimen,
   d.INFORME_ALMACEN, d.FE_INFORME_ALMACEN, d.IM_5, d.FE_IM_5 
  ,d.FACT_COMERCIAL ,d.FE_FACT_COMERCIAL,c.Nombre ,
  ii.CodProducto,ii.Descripcion,t.FechaTransaccion,t.IDTipoTransaccion,l.locationid,t.BodegaID, 
  t.RegimenID,t.ClienteID,e.nombrexp,u.nomusr,i.cantidadinicial,i.Articulos,Um.unidadmedida,ii.numero_oc,ii.lote,ii.fecha_vcmto
  ,ii.modelo,ii.cod_equivale,ii.estilo,paises.nompais,ii.precio,tb.unidadmedida,tr.nombre,t.contenedor,t.fechareciving,ii.color
  ORDER BY l.locationid,t.BodegaID,t.RegimenID,t.ClienteID;