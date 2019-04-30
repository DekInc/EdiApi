SELECT ii.InventarioID ,
       ii.ItemInventarioID ,
       ii.CodProducto ,
       p.descripcion ,
       II.CantidadInicial ,
       isnull(II.CantidadInicial/(i.CantidadInicial/nullif(i.articulos, 0)), 0) AS bultosinicial ,
       II.CantidadInicial*II.precio AS valorInicial ,
       i.rack ,
       r.NombreRack ,
       isnull(i.CantidadInicial/nullif(i.articulos, 0), 0) AS uxb ,
       ii.CantidadInicial-isnull(Sy_1.reservado, 0) AS existencia ,
       isnull((ii.CantidadInicial-isnull(Sy_1.reservado, 0))/(i.CantidadInicial/nullif(i.articulos, 0)), 0) AS BultosExis ,
       (ii.existencia-isnull(Sy_1.reservado, 0))*ii.precio AS valorexis ,
       ii.numero_oc ,
       ii.lote ,
       ii.fecha_vcmto ,
       ii.modelo ,
       ii.cod_equivale,
       ii.estilo,
       paises.nompais,
       dt.INFORME_ALMACEN AS nodt,
       dt.FE_INFORME_ALMACEN AS Fecha_Dt,
       dt.IM_5 AS nodm,
       dt.FE_IM_5 AS fecha_dm,
       0000000000000000000.00 AS solicitado,
       ii.precio,
       ii.observacion,
       t.fechatransaccion,
       u.UnidadMedida,
       iI.fechaitem,
       t.bodegaid,
       t.regimenid,
       isnull((ii.existencia-isnull(Sy_1.reservado, 0))/(i.CantidadInicial/nullif(i.peso, 0)), 0) AS peso,
       isnull((ii.existencia-isnull(Sy_1.reservado, 0))/(i.CantidadInicial/nullif(i.volumen, 0)), 0) AS cbm,
       t.clienteid,
       c.nombre AS nombrecliente,
       B.nombodega,
       i.barcode,
       t.NoTransaccion,
       isnull(ii.cantidadinicial/(i.CantidadInicial/nullif(i.volumen, 0)), 0) AS cbm_ini,
       isnull(ii.cantidadinicial/(i.CantidadInicial/nullif(i.peso, 0)), 0) AS peso_ini,
       rg.regimen,
       t.contenedor,
       ii.color,
       l.dsclocation,
       t.EstatusID
FROM ItemInventario AS ii
INNER JOIN inventario AS i ON i.InventarioID=ii.InventarioID
INNER JOIN producto AS p ON p.codproducto=ii.codproducto
LEFT JOIN racks AS r ON r.rack=i.rack
INNER JOIN DetalleTransacciones AS d1 ON d1.InventarioID=i.InventarioID
INNER JOIN transacciones AS T ON t.TransaccionID=d1.TransaccionID
LEFT JOIN Bodegas AS B ON b.bodegaid=t.bodegaid
INNER JOIN locations l ON l.locationID=b.locationID
INNER JOIN Clientes AS C ON C.ClienteID = T.ClienteID
LEFT JOIN DocumentosxTransaccion AS dt ON dt.transaccionid = t.transaccionid
LEFT JOIN paises ON paises.paisid=ii.pais_orig
LEFT JOIN unidadmedida AS u ON u.unidadmedidaid=p.unidadmedida
INNER JOIN Regimen AS rg ON rg.IDRegimen=t.RegimenID
LEFT OUTER JOIN
  (SELECT Sy.InventarioID,
          Sy.ItemInventarioID,
          Sy.CodProducto,
          SUM(ISNULL(Sy.Cantidad, 0)) AS Reservado
   FROM SysTempSalidas AS Sy
   INNER JOIN Pedido AS Pe ON Pe.PedidoID = Sy.PedidoID
   GROUP BY Sy.InventarioID,
            sy.ItemInventarioID,
            Sy.CodProducto) AS Sy_1 ON Sy_1.InventarioID = I.InventarioID
AND Sy_1.ItemInventarioID = II.ItemInventarioID
AND Sy_1.CodProducto = II.CodProducto
WHERE II.existencia>0.01
  AND T.IDTipoTransaccion IN ('IN')
  AND p.CodProducto = '1967856453'
ORDER BY t.fechatransaccion