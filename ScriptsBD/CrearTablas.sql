USE EdiDB
GO
IF OBJECT_ID('UsuariosExternos', 'U') IS NOT NULL 
	DROP TABLE UsuariosExternos
GO
CREATE TABLE UsuariosExternos(
	Id int IDENTITY(1,1) PRIMARY KEY,
	CodUsr NVARCHAR(128),
	NomUsr NVARCHAR(128),
	UsrPassword NVARCHAR(256),
	ClienteID int, 
	HashId NVARCHAR(128)
)
GO
INSERT INTO UsuariosExternos(CodUsr, NomUsr, UsrPassword, ClienteID)
VALUES('hcampos', 'Hilmer', 'Pass123456', 1345)
GO
SELECT * FROM UsuariosExternos
GO
IF OBJECT_ID('PedidosEstadosExternos', 'U') IS NOT NULL 
	DROP TABLE PedidosEstadosExternos
GO
CREATE TABLE PedidosEstadosExternos(
	Id int PRIMARY KEY,
	Descripcion nvarchar(64)
)
GO
INSERT INTO PedidosEstadosExternos(Id, Descripcion)
VALUES(1, 'Guardado')
GO
INSERT INTO PedidosEstadosExternos(Id, Descripcion)
VALUES(2, 'Enviado')
GO
INSERT INTO PedidosEstadosExternos(Id, Descripcion)
VALUES(3, 'Despachado')
GO
IF OBJECT_ID('PedidosExternos', 'U') IS NOT NULL 
	DROP TABLE PedidosExternos
GO
CREATE TABLE PedidosExternos(
	Id int IDENTITY(1,1) PRIMARY KEY,
	ClienteID int,	
	TiendaId int,	
	FechaPedido nvarchar(16) null,
	IdEstado int,
	FechaCreacion nvarchar(16),
	Periodo nvarchar(10) NULL,
	FechaUltActualizacion nvarchar(10) NULL,
	WomanQty int,
	ManQty int,
	KidQty int,
	AccQty int,
	InvType varchar(4),
	FullPed bit,
	Divert bit,
	TiendaIdDestino int
)
GO
select * from PedidosExternos
ALTER TABLE PedidosExternos   
ADD CONSTRAINT PedidosExternosUnique1 UNIQUE (TiendaId, FechaPedido);   
--ALTER TABLE PedidosExternos
--ADD FullPed bit,
--	Divert bit,
--	TiendaIdDestino int
IF OBJECT_ID('PedidosDetExternos', 'U') IS NOT NULL 
	DROP TABLE PedidosDetExternos
GO
CREATE TABLE PedidosDetExternos(
	Id int IDENTITY(1,1) PRIMARY KEY,
	PedidoId int,
	CodProducto nvarchar(50),
	Producto nvarchar(1),
	CantPedir float
)
GO
CREATE INDEX PedidosDetExternosIdxCodProducto ON PedidosDetExternos (CodProducto);
CREATE INDEX IndexPedidosDetExternosPedidoId ON PedidosDetExternos (PedidoId);
GO
--select * from PedidosDetExternos
IF OBJECT_ID('PAYLESS_Tiendas', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_Tiendas;
GO
CREATE TABLE PAYLESS_Tiendas (
	Id INT PRIMARY KEY IDENTITY(1,1) not null,
	ClienteID int,
	TiendaId int,
	Distrito NVARCHAR(8),
	Descr NVARCHAR(256),
	Direc NVARCHAR(256),
	Tel NVARCHAR(32),
	Cel NVARCHAR(32),
	Lider NVARCHAR(128),
	BodegaId int
)
GO
--SELECT * FROM [EdiDB].[dbo].[PAYLESS_Tiendas]
--select * from wms.dbo.Clientes where nombre like '%payless%' OR ClienteID = 618 order by ClienteID asc
--truncate table EdiDB.dbo.PAYLESS_Tiendas
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (385, 8000, 'R4D', 'Payless Shoe Source - El Salvador', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7365, 'R4D', 'Payless Shoe Source - Pricesmart SPS', 'Pricesmart SPS', '2566-4946', '9986-5064', 'DINA HANDAL', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7366, 'R4D', 'Payless Shoe Source - Pricesmart TGU', 'Colonia Florencia Sur contiguo a Pricesmart TGU', '2235-3034', '9761-8837', 'LESBY ORTIZ ', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7367, 'R4D', 'Payless Shoe Source - Mega Mall SPS', 'Mega Mall SPS Salida hacia la Lima', '2557-0731', '3222-6879', 'DUNIA ERAZO', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7368, 'R4D', 'Payless Shoe Source - Comayuguela TGU', '6 calle 4 y 5 avenida Comayaguela TGU', '2238-9286', '3345-7997', 'GLADYS VELASQUEZ', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7369, 'R4D', 'Payless Shoe Source - Uniplaza Juticalpa TGU', 'Uniplaza Juticalpa', '2785-7117', '3173-1708', 'JORGE IVAN LARA', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7370, 'R4D', 'Payless Shoe Source - Mall Multiplaza SPS', 'Mall Multiplaza SPS', '2550-5588', '9977-3678', 'SACCIA GATTAS', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7371, 'R4D', 'Payless Shoe Source - Blvd del Norte SPS', 'Blvd del Norte Sps', '2552-1510', '9615-6108', 'EVELYN SANABRIA', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7372, 'R4D', 'Payless Shoe Source - Mall Multiplaza Hotel Intercontinental TGU', 'Colonia Payaqui fte al Hotel Intercontinental Mall Multiplaza TGU', '2231-2247', '8732-5386', 'CAROLINA CHIRINOS', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7373, 'R4D', 'Payless Shoe Source - Barrio el Centro TGU', 'Barrio El Centro Calle peatonal fte a Celtel TGU', '2220-0257', '9882-7790', 'HECTOR HERNANDEZ', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7374, 'R4D', 'Payless Shoe Source - 1ra calle Progreso, Yoro SPS', '1 era calle Contiguo a Farmacia Machi Progreso Yoro', '2648-1648', '9882-8148', 'RONNIE SOLIS', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7375, 'R4D', 'Payless Shoe Source - Mall Megaplaza La Ceiba SPS', 'Mall Megaplaza La Ceiba', '2441-3146', '3184-5596', 'LITZA ANTUNEZ', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7376, 'R4D', 'Payless Shoe Source - Plaza Miraflores TGU', 'Blvd Centro America Plaza Miraflores 2do Nivel TGU', '2235-8545', '9500-1845', 'OLGA MATUTE', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7377, 'R4D', 'Payless Shoe Source - 3era Avenida SPS', '3era Avenida centro SPS', '2550-9286', '3143-5605', 'YENNY PINTO', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7378, 'R4D', 'Payless Shoe Source - Centro Pto Cortes SPS', '3 Y4 calle centro de Pto Cortes', '2665-3654', '9577-0771', 'OLGA RIVERA', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7379, 'R4D', 'Payless Shoe Source - Siguatepeque TGU', 'Esquina Opuesta supermercado Paico Siguatepeque', '2773-5881', '9948-8465', 'FRANCISCO AGUILERA', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7380, 'R4D', 'Payless Shoe Source - Calle Vicente William Choluteca TGU', 'Calle Vicente Williams contiguo a farmacia La Nueva Choluteca', '2782-7562', '9561-8770', 'JESSY LOPEZ', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7381, 'R4D', 'Payless Shoe Source - Metromall TGU yBlvd Europea TGU', 'MetroMall TGU Blvd Fuerzas Armadas y Blvd Comunidad Europea TGU', '2225-5131', '9724-0057', 'BLANCA PIKE', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7382, 'R4D', 'Payless Shoe Source - Mall Megaplaza Progreso Yoro SPS', 'Carretera Salida a Tela Mall MegaPlaza Progreso Yoro', '2620-1616', '3191-2148', 'KENIA ORTEGA', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7383, 'R4D', 'Payless Shoe Source - Mall Premier Comayagua TGU', 'Mall Premier Comayagua', '2771-8342', '9873-3222', 'GABRIELA ELVIR', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7384, 'R4D', 'Payless Shoe Source - Mall Cascadas y Blvd Kuwait TGU', 'Cascadas Mall entre Blvd Fuerzas Armadas y Blvd Kuwait TGU', '2245-9355', '9555-6911', 'ANGELICA ESPINAL', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7385, 'R4D', 'Payless Shoe Source - City Mall SPS', 'City Mall SPS Avenida Circunvalacion', '2518-0775', '9585-7087/3173-9222', 'YOLANDA REYES', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7386, 'R4D', 'Payless Shoe Source - Parqueo los Proceres TGU', 'Parqueo Los Proceres fte a restaurante Chillis Edif. Novacentro TGU', '2280-2969', '3176-5353', 'WENDIX SANCHEZ', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7387, 'R4D', 'Payless Shoe Source - Mall Premier Comayaguela TGU', 'Mall Premier Comayaguela Blvd del Norte', '2201-4375', '9834-0205', 'SANDRA ESPINOZA', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7388, 'R4D', 'Payless Shoe Source - City Mall TGU', 'City Mall TGU Vcolonia Las Torres TGU', '2263-0016', '9968-4983', 'CARLOS HERNANDEZ', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7389, 'R4D', 'Payless Shoe Source - Unimall Choluteca TGU', 'Unimall Choluteca , Choluteca', '2705-3942', '9561-8770', 'NELSON ARGUETA', 82)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7392, 'R4D', 'Payless Shoe Source - Uniplaza Santa Rosa de Copan SPS', 'Uniplaza Santa Rosa de Copan', '2662-5923', '9595-4232', 'MERCY DUBON', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7393, 'R4D', 'Nueva tienda Roatán SPS', '', '', '', '', 81)
GO

SELECT * FROM EdiDB.dbo.PAYLESS_Tiendas
GO
IF OBJECT_ID('PAYLESS_Reportes', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_Reportes
GO
CREATE TABLE PAYLESS_Reportes(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Periodo NVARCHAR(10),
	PeriodoF NVARCHAR(10),
	FechaGen NVARCHAR(20),
	Tipo NVARCHAR(1)
)
GO
IF OBJECT_ID('PAYLESS_ReportesDet', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ReportesDet
GO
CREATE TABLE PAYLESS_ReportesDet(
	Id int IDENTITY(1,1) PRIMARY KEY,
	IdM int,
	TiendaId int,
	TotalWomanQty int,
	TotalManQty int,
	TotalKidQty int,
	TotalAccQty int,
	Total int,
	Fecha1 VARCHAR(20),
	Cant1 int,
	Fecha2 VARCHAR(20),
	Cant2 int,
	Fecha3 VARCHAR(20),
	Cant3 int,
	Fecha4 VARCHAR(20),
	Cant4 int,
	Fecha5 VARCHAR(20),
	Cant5 int,
	Fecha6 VARCHAR(20),
	Cant6 int,
	RutaId int
)
GO
IF OBJECT_ID('PAYLESS_ReportesMails', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_ReportesMails
GO
CREATE TABLE PAYLESS_ReportesMails(
	Id int IDENTITY(1,1) PRIMARY KEY,
	MailDir NVARCHAR(256) UNIQUE
)
GO
INSERT INTO PAYLESS_ReportesMails(MailDir) VALUES ('Hilmer.Campos@GlcAmerica.com')
GO
INSERT INTO PAYLESS_ReportesMails(MailDir) VALUES ('Mayra.Jaimes@glcamerica.com')
GO
INSERT INTO PAYLESS_ReportesMails(MailDir) VALUES ('keyfireone@gmail.com')
GO
--insert into PAYLESS_Reportes(Periodo, FechaGen, Tipo) VALUES('13/05/2019', '02/05/2019 08:49', '0')
--go
IF OBJECT_ID('PAYLESS_Transporte', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_Transporte
GO
CREATE TABLE PAYLESS_Transporte(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Transporte [nvarchar](128)
)
GO
IF OBJECT_ID('PAYLESS_PeriodoTransporte', 'U') IS NOT NULL 
	DROP TABLE PAYLESS_PeriodoTransporte
GO
CREATE TABLE PAYLESS_PeriodoTransporte(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Periodo [nvarchar](10),
	IdTransporte int
)
GO
IF OBJECT_ID('AsyncStates', 'U') IS NOT NULL 
	DROP TABLE AsyncStates
GO
CREATE TABLE AsyncStates(
	Id int IDENTITY(1,1) PRIMARY KEY,
	Typ int,
	Val int,
	Maximum int,
	Mess varchar(2048),
	Fecha varchar(24)
)
GO
IF OBJECT_ID('ProductoUbicacion', 'U') IS NOT NULL 
	DROP TABLE ProductoUbicacion
GO
CREATE TABLE ProductoUbicacion (
	Id int IDENTITY(1,1) PRIMARY KEY,
	Typ int,
	CodProducto VARCHAR(50),
	NomBodega VARCHAR(150),
	Rack int,
	NombreRack VARCHAR(150)
)
GO