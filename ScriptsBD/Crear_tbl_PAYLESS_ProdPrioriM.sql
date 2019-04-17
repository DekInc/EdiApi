IF OBJECT_ID('PAYLESS_ProdPrioriM', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriM
GO
CREATE TABLE PAYLESS_ProdPrioriM(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	[Periodo] [nvarchar](10) NULL,
	[ClienteId] [int] NULL,
	Transporte [nvarchar](12),
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
	[Peso] [float] NULL
) ON [PRIMARY]
GO
IF OBJECT_ID('PAYLESS_ProdPrioriArchM', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriArchM
GO
CREATE TABLE PAYLESS_ProdPrioriArchM(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	[Periodo] [nvarchar](10) NULL,
	[ClienteId] [int] NULL,	
	[CodUsr] nvarchar(128),
	InsertDate nvarchar(16),
	UpdateDate nvarchar(16)
)
GO
IF OBJECT_ID('PAYLESS_ProdPrioriArchDet', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriArchDet
GO
CREATE TABLE PAYLESS_ProdPrioriArchDet(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	IdM int not null,
	[barcode] nvarchar(16)
)
GO

