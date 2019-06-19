Select TOP 40
d.*,
t.nombre as nombretransportista,
dt.locationid,
dt.IDTipoTransaccion
From wms.dbo.despachos d
left join wms.dbo.transportista as t 
	on t.transportistaid=d.transportistaid
left join (
	select 
		dt.despachoid,
		b.locationid,
		IDTipoTransaccion
	from wms.dbo.DtllDespacho dt
	inner join wms.dbo.Transacciones t 
		on t.TransaccionID=dt.TransaccionID
	inner join wms.dbo.Bodegas b 
		on b.BodegaID=t.BodegaID
	where T.IDTipoTransaccion IN ('SA','XL')
	group by 
	dt.despachoid,
	b.locationid,
	IDTipoTransaccion
) dt 
	on dt.DespachoID = d.DespachoID
ORDER BY d.DespachoID DESC