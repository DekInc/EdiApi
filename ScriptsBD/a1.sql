--3447
--4067
select distinct BarCode,
Producto as Lote,
Talla,
Lote as Estilo,
Categoria,
Departamento,
CP
 from  EdiDb.dbo.PAYLESS_ProdPrioriDet where BarCode in (
SELECT [Barcode]
  FROM [EdiDB].[dbo].[Trasladado1]
) order by barcode

