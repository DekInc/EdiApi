USE EdiDB
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('SP_GetExistenciasExtern', 'P') IS NOT NULL
	DROP PROC dbo.[SP_GetExistenciasExtern]
GO

CREATE PROCEDURE [dbo].[SP_GetExistenciasExtern] @IdClient INT
AS
BEGIN
	SELECT Inv.Cliente,
		Inv.Bodega,
		Inv.CodProducto,
		Inv.Descripcion,
		Inv.Existencia,
		ISNULL(Pedidos.Reservado, 0) AS Reservado,
		Inv.Existencia - ISNULL(Pedidos.Reservado, 0) AS Disponible,
		Inv.ClienteID,
		Inv.BodegaID,
		Inv.Bultos,
		ROUND(Inv.Peso, 3) AS Peso,
		ROUND(Inv.Volumen, 3) AS Volumen,
		Inv.uxb,
		ISNULL(Inv.lote, '') AS lote,
		ISNULL(Inv.contenedor, '') AS contenedor
	FROM (
		SELECT C.Nombre AS Cliente,
			B.NomBodega AS Bodega,
			Ie.CodProducto,
			Ie.Descripcion,
			SUM(Ie.Existencia) AS Existencia,
			Ie.ClienteID,
			Ie.BodegaID,
			isnull(SUM(Ie.Precio) / nullif(SUM(Ie.Articulos), 0), 0) AS Precio,
			SUM(Ie.Articulos) AS Bultos,
			SUM(ISNULL(Ie.peso / NULLIF(Ie.Articulos, 0), 0)) AS Peso,
			SUM(ISNULL(Ie.Volumen / NULLIF(Ie.Articulos, 0), 0)) AS Volumen,
			Ie.uxb,
			Ie.lote,
			Ie.contenedor
		FROM (
			SELECT T.TransaccionID,
				T.FechaTransaccion,
				I.InventarioID,
				I.Barcode,
				It.ItemInventarioID,
				It.CodProducto,
				P.Descripcion,
				R.NombreRack,
				ISNULL(It.Existencia, 0) - ISNULL(Sy_1.Reservado, 0) AS Existencia,
				It.Precio,
				(ISNULL(It.Existencia, 0) - ISNULL(Sy_1.Reservado, 0)) * It.Precio AS Total,
				T.ClienteID,
				R.Rack,
				Dtr.INFORME_ALMACEN,
				Dtr.FE_INFORME_ALMACEN,
				Dtr.IM_5,
				Dtr.FE_IM_5,
				Dtr.CARTA_ACEPTA,
				Dtr.FE_CARTA_ACEPTA,
				Dtr.GUIA_TRANSITO,
				Dtr.FE_GUIA_TRANSITO,
				Dtr.FACT_COMERCIAL,
				Dtr.FE_FACT_COMERCIAL,
				Dtr.IM_4,
				Dtr.FE_IM_4,
				Dtr.DOCUMENTO_TRANSPORTE,
				Dtr.FE_DOCUMENTO_TRANSPORTE,
				Dtr.FACT_EXPORTACION,
				Dtr.FE_FACT_EXPORTACION,
				Dtr.ORDEN_COMPRA,
				Dtr.FE_ORDEN_COMPRA,
				Dtr.MANIFIESTO,
				Dtr.FE_MANIFIESTO,
				R.BodegaID,
				isnull(it.cantidadinicial / nullif(i.CantidadInicial / nullif(i.peso, 0), 0), 0) AS peso,
				isnull(it.cantidadinicial / nullif(i.CantidadInicial / nullif(i.volumen, 0), 0), 0) AS Volumen,
				isnull((it.existencia - isnull(Sy_1.reservado, 0)) / nullif(i.CantidadInicial / nullif(i.articulos, 0), 0), 0) AS Articulos,
				isnull(It.CantidadInicial / nullif(I.Articulos, 0), 0) AS uxb,
				It.lote,
				T.contenedor
			FROM wms.dbo.Transacciones AS T WITH (NOLOCK)
			INNER JOIN wms.dbo.DetalleTransacciones AS Dt WITH (NOLOCK) ON Dt.TransaccionID = T.TransaccionID
			INNER JOIN wms.dbo.Inventario AS I WITH (NOLOCK) ON I.InventarioID = Dt.InventarioID
			INNER JOIN wms.dbo.ItemInventario AS It WITH (NOLOCK) ON It.InventarioID = Dt.InventarioID
			LEFT JOIN wms.dbo.Racks AS R WITH (NOLOCK) ON R.Rack = I.Rack
			INNER JOIN wms.dbo.Producto AS P WITH (NOLOCK) ON P.CodProducto = It.CodProducto
			LEFT OUTER JOIN wms.dbo.DocumentosxTransaccion AS Dtr WITH (NOLOCK) ON Dtr.TransaccionID = T.TransaccionID
			LEFT OUTER JOIN (
				SELECT Sy.InventarioID,
					Sy.ItemInventarioID,
					Sy.CodProducto,
					sy.lote,
					SUM(ISNULL(Sy.Cantidad, 0)) AS Reservado
				FROM wms.dbo.SysTempSalidas AS Sy WITH (NOLOCK)
				INNER JOIN wms.dbo.Pedido AS Pe WITH (NOLOCK) ON Pe.PedidoID = Sy.PedidoID
					AND Pe.clienteID = @IdClient
				WHERE (
						Pe.EstatusID NOT IN (
							9,
							10
							)
						)
				GROUP BY Sy.InventarioID,
					Sy.ItemInventarioID,
					Sy.CodProducto,
					sy.lote
				) AS Sy_1 ON Sy_1.InventarioID = It.InventarioID
				AND Sy_1.ItemInventarioID = It.ItemInventarioID
				AND Sy_1.CodProducto = It.CodProducto
				AND It.lote = sy_1.lote
			WHERE (T.IDTipoTransaccion = 'IN')
				AND (ISNULL(I.IsReservado, 'False') = 'False')
				AND T.EstatusID > 4
			) AS Ie
		INNER JOIN wms.dbo.Clientes AS C WITH (NOLOCK) ON C.ClienteID = Ie.ClienteID
			AND C.ClienteID = @IdClient
		INNER JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) ON B.BodegaID = Ie.BodegaID
		GROUP BY C.Nombre,
			B.NomBodega,
			Ie.CodProducto,
			Ie.Descripcion,
			Ie.ClienteID,
			Ie.BodegaID,
			Ie.uxb,
			Ie.lote,
			Ie.contenedor
		) AS Inv
	LEFT OUTER JOIN (
		SELECT C.Nombre AS Cliente,
			B.NomBodega AS Bodega,
			Pd.CodProducto,
			Pd.Descripcion,
			SUM(ISNULL(Dp.Cantidad, 0)) AS Reservado,
			C.ClienteID,
			B.BodegaID,
			Dp.uxb,
			Dp.lote
		FROM wms.dbo.Pedido AS P WITH (NOLOCK)
		INNER JOIN wms.dbo.DtllPedido AS Dp WITH (NOLOCK) ON Dp.PedidoID = P.PedidoID
		INNER JOIN wms.dbo.Bodegas AS B WITH (NOLOCK) ON B.BodegaID = P.BodegaID
		INNER JOIN wms.dbo.Clientes AS C WITH (NOLOCK) ON C.ClienteID = P.ClienteID
			AND C.ClienteID = @IdClient
		INNER JOIN wms.dbo.Producto AS Pd WITH (NOLOCK) ON Pd.CodProducto = Dp.CodProducto
		WHERE (
				P.EstatusID = 7
				OR P.EstatusID = 8
				)
			AND (
				Dp.PedidoID NOT IN (
					SELECT DISTINCT PedidoID
					FROM wms.dbo.SysTempSalidas WITH (NOLOCK)
					)
				)
		GROUP BY C.Nombre,
			B.NomBodega,
			Pd.CodProducto,
			Pd.Descripcion,
			C.ClienteID,
			B.BodegaID,
			Dp.uxb,
			Dp.lote
		) AS Pedidos ON Pedidos.CodProducto = Inv.CodProducto
		AND Pedidos.ClienteID = Inv.ClienteID
		AND Pedidos.BodegaID = Inv.BodegaID
		AND IsNull(Pedidos.lote, '') = IsNull(Inv.lote, '')
	--WHERE (Inv.Existencia > 0) 
	ORDER BY Inv.CodProducto
END
GO

EXEC [SP_GetExistenciasExtern] 1432
