IF OBJECT_ID('PAYLESS_ProdPrioriM', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriM; 

CREATE TABLE PAYLESS_ProdPrioriM(
	[Id] int PRIMARY KEY IDENTITY(1,1),
	[Periodo] [nvarchar](10) NULL,
	[ClienteId] [int] NULL,
	Transporte [nvarchar](12),
	[CodUsr] nvarchar(128),
	InsertDate nvarchar(10),
	UpdateDate nvarchar(10)
)

IF OBJECT_ID('PAYLESS_ProdPrioriDet', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ProdPrioriDet; 

CREATE TABLE [dbo].[PAYLESS_ProdPrioriDet](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,	
	IdPAYLESS_ProdPrioriM int not null,
	[OID] [float] NULL,
	[Barcode] [float] NULL,
	[Estado] [float] NULL,
	[Pri] [float] NULL,
	[PoolP] [float] NULL,
	[Producto] [float] NULL,
	[Talla] [float] NULL,
	[Lote] [float] NULL,
	[Categoria] [nvarchar](255) NULL,
	[Departamento] [float] NULL,
	[CP] [nvarchar](255) NULL,
	[Pickeada] [nvarchar](255) NULL,
	[Etiquetada] [nvarchar](255) NULL,
	[Preinspeccion] [nvarchar](255) NULL,
	[Cargada] [nvarchar](255) NULL,
	[M3] [float] NULL,
	[Peso] [float] NULL	
) ON [PRIMARY]
GO


