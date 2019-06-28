use edidb
go
IF OBJECT_ID('PAYLESS_ProdPrioriM', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriM
GO
CREATE TABLE PAYLESS_ProdPrioriM(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	[Periodo] [nvarchar](10) NULL,
	[ClienteId] [int] NULL,	
	[CodUsr] nvarchar(128),
	InsertDate nvarchar(16),
	UpdateDate nvarchar(16)
)
GO
IF OBJECT_ID('PAYLESS_ProdPrioriDet', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriDet
GO
CREATE TABLE [dbo].[PAYLESS_ProdPrioriDet](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,	
	IdPAYLESS_ProdPrioriM int not null,
	[OID] nvarchar(16) NULL,
	[Barcode] nvarchar(16) NULL,
	[Estado] nvarchar(4) NULL,
	[Pri] nvarchar(4) NULL,
	[PoolP] nvarchar(4) NULL,
	Producto nvarchar(16) NULL,
	[Talla] nvarchar(8) NULL,
	Lote nvarchar(8) NULL,
	[Categoria] nvarchar(256) NULL,
	[Departamento] nvarchar(16) NULL,
	[CP] nvarchar(8) NULL,
	[Pickeada] nvarchar(24) NULL,
	[Etiquetada] nvarchar(24) NULL,
	[Preinspeccion] nvarchar(32) NULL,
	[Cargada] nvarchar(24) NULL,
	[M3] [float] NULL,
	[Peso] [float] NULL,
	IdTransporte int null
) ON [PRIMARY]
GO
CREATE INDEX PAYLESS_ProdPrioriDetIdxBarcode ON PAYLESS_ProdPrioriDet (Barcode);
GO
--select * from [PAYLESS_ProdPrioriDet]
IF OBJECT_ID('PAYLESS_ProdPrioriArchM', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriArchM
GO
CREATE TABLE PAYLESS_ProdPrioriArchM(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	[Periodo] [nvarchar](18) NULL,
	IdTransporte [int] NULL,	
	[CodUsr] nvarchar(128),
	InsertDate nvarchar(16),
	UpdateDate nvarchar(16),
	PorcValidez float,
	CantExcel int null,
	CantEscaner int null,
	Typ int null
)
GO
select * from EdiDb.dbo.PAYLESS_ProdPrioriArchM
--ALTER TABLE PAYLESS_ProdPrioriArchM
--ADD Typ int null
IF OBJECT_ID('PAYLESS_ProdPrioriArchDet', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriArchDet
GO
CREATE TABLE PAYLESS_ProdPrioriArchDet(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	IdM int not null,
	[barcode] nvarchar(16)
)
GO
--select * from edidb.dbo.PAYLESS_ProdPrioriArchM
--select * from edidb.dbo.PAYLESS_ProdPrioriArchDet order by barcode

select top 10 * from edidb.dbo.PAYLESS_ProdPrioriArchDet

--insert into edidb.dbo.PAYLESS_ProdPrioriArchDet(IdM, barcode) VALUES (2, 12345679)

select * from edidb.dbo.PAYLESS_ProdPrioriArchDet order by Id dESC

--delete from edidb.dbo.PAYLESS_ProdPrioriArchDet where Id in (6035, 6036)

GLCHN33-05-0018

select top 10 * from wms.[dbo].[DocumentosxTransaccion] where informe_almacen like'%GLCHN33-05-0018%'

select top 10 * from wms.[dbo].[DocumentosxTransaccion] order by IDDocxTransaccion desc

select * from wms.dbo.Clientes where nombre like'%payles%'
