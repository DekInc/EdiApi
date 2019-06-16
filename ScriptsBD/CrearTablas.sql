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
VALUES (1432, 7365, 'R4D', 'Payless Shoe Source - Pricesmart SPS - 7365', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7367, 'R4D', 'Payless Shoe Source - Mega Mall SPS Salida hacia la Lima - 7367', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7370, 'R4D', 'Payless Shoe Source - Mall Multiplaza SPS - 7370', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7371, 'R4D', 'Payless Shoe Source - Blvd del Norte Sps - 7371', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7374, 'R4D', 'Payless Shoe Source - 1 era calle Contiguo a Farmacia Machi Progreso Yoro - 7374', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7375, 'R4D', 'Payless Shoe Source - Mall Megaplaza La Ceiba - 7375', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7377, 'R4D', 'Payless Shoe Source - 3era Avenida centro SPS - 7377', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7378, 'R4D', 'Payless Shoe Source - 3 Y4 calle centro de Pto Cortes - 7378', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7379, 'R4D', 'Payless Shoe Source - Esquina Opuesta supermercado Paico Siguatepeque - 7379', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7382, 'R4D', 'Payless Shoe Source - Carretera Salida a Tela Mall MegaPlaza Progreso Yoro - 7382', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7383, 'R4D', 'Payless Shoe Source - Mall Premier Comayagua - 7383', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7385, 'R4D', 'Payless Shoe Source - City Mall SPS Avenida Circunvalacion - 7385', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7392, 'R4D', 'Payless Shoe Source - Uniplaza Santa Rosa de Copan - 7392', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7366, 'R4D', 'Payless Shoe Source - Colonia Florencia Sur contiguo a Pricesmart TGU - 7366', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7368, 'R4D', 'Payless Shoe Source - 6 calle 4 y 5 avenida Comayaguela TGU - 7368', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7369, 'R4D', 'Payless Shoe Source - Uniplaza Juticalpa - 7369', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7372, 'R4D', 'Payless Shoe Source - Colonia Payaqui fte al Hotel Intercontinental Mall Multiplaza TGU - 7372', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7373, 'R4D', 'Payless Shoe Source - Barrio El Centro Calle peatonal fte a Celtel TGU - 7373', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7376, 'R4D', 'Payless Shoe Source - Blvd Centro America Plaza Miraflores 2do Nivel TGU - 7376', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7380, 'R4D', 'Payless Shoe Source - Calle Vicente Williams contiguo a farmacia La Nueva Choluteca - 7380', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7381, 'R4D', 'Payless Shoe Source - MetroMall TGU Blvd Fuerzas Armadas y Blvd Comunidad Europea TGU - 7381', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7384, 'R4D', 'Payless Shoe Source - Cascadas Mall entre Blvd Fuerzas Armadas y Blvd Kuwait TGU - 7384', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7387, 'R4D', 'Payless Shoe Source - Mall Premier Comayaguela Blvd del Norte - 7387', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7388, 'R4D', 'Payless Shoe Source - City Mall TGU Vcolonia Las Torres TGU - 7388', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7389, 'R4D', 'Payless Shoe Source - Unimall Choluteca , Choluteca - 7389', '', '', '', '', 81)
GO
INSERT INTO PAYLESS_Tiendas(ClienteID, TiendaId, Distrito, Descr, Direc, Tel, Cel, Lider, BodegaId)
VALUES (1432, 7386, 'R4D', 'Payless Shoe Source - Parqueo Los Proceres fte a restaurante Chillis Edif. Novacentro TGU - 7386', '', '', '', '', 81)
GO
SELECT * FROM PAYLESS_Tiendas
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
	MailDir NVARCHAR(256)
)
GO
INSERT INTO PAYLESS_ReportesMails(MailDir) VALUES ('Hilmer.Campos@GlcAmerica.com')
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