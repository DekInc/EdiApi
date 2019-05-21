--SELECT COUNT(*) FROM (

Select *
--T.TransaccionID, NoTransaccion As NoIngreso, 
--C.Nombre As Cliente, T.FechaTransaccion As FechaIngreso,
-- B.NomBodega As Bodega, R.Regimen, E.Estatus, T.Observacion,
-- T.exportadorid,t.destinoid,X.nombrexp,d.nombredest,
--[INFORME_ALMACEN]
--,[IM_5]
--,[CARTA_ACEPTA]
--,[GUIA_TRANSITO]
--,[FACT_COMERCIAL]
--,[IM_4]
--,[DOCUMENTO_TRANSPORTE]
--,[FACT_EXPORTACION] 
--,[ORDEN_COMPRA] 
--,[MANIFIESTO] 
--,[FE_INFORME_ALMACEN] 
--,[FE_IM_5] 
--,[FE_CARTA_ACEPTA] 
--,[FE_GUIA_TRANSITO] 
--,[FE_FACT_COMERCIAL] 
--,[FE_IM_4] 
--,[FE_DOCUMENTO_TRANSPORTE] 
--,[FE_FACT_EXPORTACION] 
--,[FE_ORDEN_COMPRA] 
--,[FE_MANIFIESTO],
--Ii.CodProducto,
--i.existencia,
----sum(IsNull(i.articulos,1)) as bultos,
----sum(i.peso) as peso,
----sum(i.volumen) as volumen,
----sum(i.existencia) as existencia,
----sum(i.valor) as valor,
--T.BodegaID,T.RegimenID,T.ClienteID,T.Usuariocrea,T.EstatusID, 
--t.transportistaid,t.pais_orig,t.adu_fro,t.placa,t.marchamo,
--t.remolque,t.contenedor,t.cod_motoris,tm.nit_emp as licencia 
From dbo.Transacciones As T WITH(NOLOCK)
left Join Bodegas As B WITH(NOLOCK) On B.BodegaID = T.BodegaID AND B.BodegaId = 81
left Join detalletransacciones As dti WITH(NOLOCK) On dti.transaccionID =  T.transaccionID 
left Join inventario as i WITH(NOLOCK) on i.inventarioid=dti.inventarioid 
left join ItemInventario as Ii WITH(NOLOCK) on Ii.InventarioId = i.InventarioId AND Ii.CodProducto = '7365821923'
left Join Regimen AS R WITH(NOLOCK) On R.IDRegimen = T.RegimenID 
left Join Clientes As C WITH(NOLOCK) On C.ClienteID = T.ClienteID
left Join Estatus As E WITH(NOLOCK) On E.EstatusID = T.EstatusID 
left Join exportador As X WITH(NOLOCK) On X.exportadorID = T.exportadorID 
left Join destconsigna As D WITH(NOLOCK) On D.destinoID = T.destinoID 
left Join DocumentosxTransaccion As DT WITH(NOLOCK) On DT.transaccionID = T.transaccionID 
left Join tab_igard As TM WITH(NOLOCK) On TM.codgen = T.cod_motoris AND TM.tip_data='E'
where C.ClienteID = 1432
and [INFORME_ALMACEN] = 'GLCHN33-5-002'
AND Ii.CodProducto = '7365821923'
--group by T.TransaccionID, NoTransaccion,C.Nombre,T.FechaTransaccion,B.NomBodega,R.Regimen, 
--E.Estatus, T.Observacion,T.exportadorid,t.destinoid,X.nombrexp,d.nombredest,
--INFORME_ALMACEN,IM_5,CARTA_ACEPTA,GUIA_TRANSITO,FACT_COMERCIAL,IM_4,DOCUMENTO_TRANSPORTE,
--FACT_EXPORTACION,ORDEN_COMPRA,MANIFIESTO,FE_INFORME_ALMACEN,FE_IM_5,FE_CARTA_ACEPTA,
--FE_GUIA_TRANSITO,FE_FACT_COMERCIAL,FE_IM_4,
--FE_DOCUMENTO_TRANSPORTE,FE_FACT_EXPORTACION,FE_ORDEN_COMPRA,FE_MANIFIESTO,T.BodegaID,
--T.RegimenID,T.ClienteID,T.Usuariocrea,T.EstatusID, 
--t.transportistaid,t.pais_orig,t.adu_fro,t.placa,t.marchamo,t.remolque,t.contenedor,
--t.cod_motoris,tm.nit_emp 

--) A1

--2,325,625

--select * from DocumentosxTransaccion where TransaccionID = '115856'


