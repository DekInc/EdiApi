SELECT B.BodegaID, B.NomBodega, B.EstatusID, E.Estatus, B.Descripcion, 
LocationID  FROM Bodegas As B 
Inner Join Estatus As E On B.EstatusID = E.EstatusID
where B.NomBodega like'%polvorin%'
--81
Select R.IDRegimen, R.Regimen, B.Regimen
From dbo.Regimen As R 
Inner Join dbo.BodegaxRegimen As B 
	On B.Regimen = R.IDRegimen
Where B.BodegaID = 81


--Regimen = almacenaje simple