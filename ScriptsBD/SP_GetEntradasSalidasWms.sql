USE EdiDB
GO
IF OBJECT_ID('SP_GetEntradasSalidasWms', 'P') IS NOT NULL
	DROP PROC SP_GetEntradasSalidasWms
GO

CREATE PROCEDURE [dbo].SP_GetEntradasSalidasWms 
@ClienteId int
AS
BEGIN
	DELETE FROM EdiDB.dbo.WmsInOut WHERE ClienteId = @ClienteId
	INSERT INTO EdiDB.dbo.WmsInOut(
		TransaccionID,
		NoTransaccion,
		FechaTransaccion,
		IdTipoTransaccion,
		TipoTransaccion,
		PedidoId,
		BodegaId,
		NomBodega,
		RegimenId,
		Regimen,
		ClienteId,
		Nombre,
		TipoIngreso,
		Observacion,
		EstatusId,
		Estatus,
		CantEnt,
		CantSal,
		INFORME_ALMACEN 
	)
	SELECT TOP 230
	T.TransaccionID,
	T.NoTransaccion,
	T.FechaTransaccion,
	T.IdTipoTransaccion,
	CASE T.IdTipoTransaccion WHEN 'SA' THEN 'Salida' WHEN 'IN' THEN 'Entrada' END TipoTransaccion,
	T.PedidoId,
	T.BodegaId,
	B.NomBodega,
	T.RegimenId,
	R.Regimen,
	T.ClienteId,
	C.Nombre,
	T.TipoIngreso,
	T.Observacion,
	T.EstatusId,
	E.Estatus,
	(
		CASE T.IdTipoTransaccion WHEN 'IN' THEN
			(SELECT COUNT(*) 
			FROM wms.dbo.DetalleTransacciones Dt WITH(NOLOCK)
			JOIN wms.dbo.ItemInventario Ii WITH(NOLOCK)
				ON Ii.InventarioId = Dt.InventarioId
			WHERE Dt.TransaccionId = T.TransaccionId
			)
		ELSE NULL
		END
	) CantEnt,
	(
		CASE T.IdTipoTransaccion WHEN 'SA' THEN
			(SELECT COUNT(*) 
			FROM wms.dbo.SysTempSalidas S WITH(NOLOCK)
			WHERE S.TransaccionId = T.TransaccionId)
		ELSE NULL
		END
	) CantSal,
	Dt.INFORME_ALMACEN
	FROM wms.dbo.Transacciones T WITH(NOLOCK)
	JOIN wms.dbo.Bodegas B WITH(NOLOCK)
		ON B.BodegaId = T.BodegaId
	JOIN wms.dbo.Regimen R WITH(NOLOCK)
		ON R.IdRegimen = T.RegimenId
	JOIN wms.dbo.Clientes C WITH(NOLOCK)	
		ON C.ClienteId = T.ClienteId
	JOIN wms.dbo.Estatus E WITH(NOLOCK)
		ON E.EstatusId = T.EstatusId
	LEFT JOIN wms.dbo.DocumentosxTransaccion Dt WITH(NOLOCK)
		ON Dt.TransaccionID = T.TransaccionID
	WHERE C.ClienteID = @ClienteId
	ORDER BY T.TransaccionId DESC
END
GO
--1643
select * from EdiDB.dbo.WmsInOut
exec EdiDB.dbo.SP_GetEntradasSalidasWms 1432
--EXEC SP_GetPaylessProdPrioriByPeriodAndIdTransport '13/05/2019', 6
--select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
--select * from EdiDb.dbo.PAYLESS_Tiendas where TiendaId = 7366




	