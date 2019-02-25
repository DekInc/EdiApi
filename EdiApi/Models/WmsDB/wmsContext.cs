using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EdiApi.Models.WmsDB
{
    public partial class WmsContext : DbContext
    {
        public WmsContext()
        {
        }

        public WmsContext(DbContextOptions<WmsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aduana> Aduana { get; set; }
        public virtual DbSet<BarcodeProducto> BarcodeProducto { get; set; }
        public virtual DbSet<Bodegas> Bodegas { get; set; }
        public virtual DbSet<BodegaxRegimen> BodegaxRegimen { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Cierreperiodo> Cierreperiodo { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<ConfigFrmBusqueda> ConfigFrmBusqueda { get; set; }
        public virtual DbSet<ConfiguracionCategoria> ConfiguracionCategoria { get; set; }
        public virtual DbSet<Despachos> Despachos { get; set; }
        public virtual DbSet<Destconsigna> Destconsigna { get; set; }
        public virtual DbSet<DetalleIngresoCliente> DetalleIngresoCliente { get; set; }
        public virtual DbSet<DetalleProducto> DetalleProducto { get; set; }
        public virtual DbSet<DetalleTransacciones> DetalleTransacciones { get; set; }
        public virtual DbSet<Documentos> Documentos { get; set; }
        public virtual DbSet<DocumentosxTransaccion> DocumentosxTransaccion { get; set; }
        public virtual DbSet<DocumentoxTipoTransaccion> DocumentoxTipoTransaccion { get; set; }
        public virtual DbSet<DtllDespacho> DtllDespacho { get; set; }
        public virtual DbSet<DtllItemTransaccion> DtllItemTransaccion { get; set; }
        public virtual DbSet<DtllPedido> DtllPedido { get; set; }
        public virtual DbSet<DtllPedidosxOperario> DtllPedidosxOperario { get; set; }
        public virtual DbSet<DtllReceivexItemInventario> DtllReceivexItemInventario { get; set; }
        public virtual DbSet<Embalaje> Embalaje { get; set; }
        public virtual DbSet<Estatus> Estatus { get; set; }
        public virtual DbSet<Exportador> Exportador { get; set; }
        public virtual DbSet<Grpopcional> Grpopcional { get; set; }
        public virtual DbSet<Grpsystem> Grpsystem { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<ItemInventario> ItemInventario { get; set; }
        public virtual DbSet<ItemParamaetroxProducto> ItemParamaetroxProducto { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<LogAsignacionRacks> LogAsignacionRacks { get; set; }
        public virtual DbSet<MetodoDescargo> MetodoDescargo { get; set; }
        public virtual DbSet<Operarios> Operarios { get; set; }
        public virtual DbSet<Optsystem> Optsystem { get; set; }
        public virtual DbSet<Paises> Paises { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<PedidosxSalida> PedidosxSalida { get; set; }
        public virtual DbSet<PedidoxOperario> PedidoxOperario { get; set; }
        public virtual DbSet<Producto> Producto { get; set; }
        public virtual DbSet<Racks> Racks { get; set; }
        public virtual DbSet<Regimen> Regimen { get; set; }
        public virtual DbSet<SysTempSalidas> SysTempSalidas { get; set; }
        public virtual DbSet<TabIgard> TabIgard { get; set; }
        public virtual DbSet<TipoTransacciones> TipoTransacciones { get; set; }
        public virtual DbSet<Transacciones> Transacciones { get; set; }
        public virtual DbSet<Transportista> Transportista { get; set; }
        public virtual DbSet<UnidadMedida> UnidadMedida { get; set; }
        public virtual DbSet<Usrsystem> Usrsystem { get; set; }

        // Unable to generate entity type for table 'dbo.deptoasigna'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tbLayout'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.template_col'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.config_frm_busqueda_detalle'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TemporalInventario'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TemporalItemInventario'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.TraduccionWeb'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aduana>(entity =>
            {
                entity.HasKey(e => e.Aduana1);

                entity.ToTable("aduana");

                entity.Property(e => e.Aduana1)
                    .HasColumnName("aduana")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Descrip)
                    .HasColumnName("descrip")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Frontera).HasColumnName("frontera");

                entity.Property(e => e.Nodo)
                    .HasColumnName("nodo")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Zolizip).HasColumnName("zolizip");
            });

            modelBuilder.Entity<BarcodeProducto>(entity =>
            {
                entity.HasKey(e => e.BarcodeId);

                entity.HasIndex(e => e.CodProducto)
                    .HasName("IX_BarcodeProducto");

                entity.Property(e => e.BarcodeId)
                    .HasColumnName("BarcodeID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodProductoNavigation)
                    .WithMany(p => p.BarcodeProducto)
                    .HasForeignKey(d => d.CodProducto)
                    .HasConstraintName("FK_BarcodeProducto_Producto");
            });

            modelBuilder.Entity<Bodegas>(entity =>
            {
                entity.HasKey(e => e.BodegaId);

                entity.HasIndex(e => e.BodegaId)
                    .HasName("IX_Bodegas")
                    .IsUnique();

                entity.Property(e => e.BodegaId)
                    .HasColumnName("BodegaID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Locationid).HasColumnName("locationid");

                entity.Property(e => e.Nodescargaxcliente).HasColumnName("nodescargaxcliente");

                entity.Property(e => e.NomBodega)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TitRequisicion)
                    .HasColumnName("titRequisicion")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.Bodegas)
                    .HasForeignKey(d => d.EstatusId)
                    .HasConstraintName("FK_Bodegas_Estatus");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Bodegas)
                    .HasForeignKey(d => d.Locationid)
                    .HasConstraintName("FK_location");
            });

            modelBuilder.Entity<BodegaxRegimen>(entity =>
            {
                entity.HasIndex(e => new { e.BodegaId, e.Regimen })
                    .HasName("IX_BodegaxRegimen")
                    .IsUnique();

                entity.Property(e => e.BodegaxRegimenId)
                    .HasColumnName("BodegaxRegimenID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BodegaId).HasColumnName("BodegaID");

                entity.HasOne(d => d.Bodega)
                    .WithMany(p => p.BodegaxRegimen)
                    .HasForeignKey(d => d.BodegaId)
                    .HasConstraintName("FK_BodegaxRegimen_Bodegas");

                entity.HasOne(d => d.RegimenNavigation)
                    .WithMany(p => p.BodegaxRegimen)
                    .HasForeignKey(d => d.Regimen)
                    .HasConstraintName("FK_BodegaxRegimen_Regimen");
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.HasIndex(e => e.CategoriaId)
                    .HasName("IX_Categoria")
                    .IsUnique();

                entity.Property(e => e.CategoriaId)
                    .HasColumnName("CategoriaID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NomCategoria)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Cierreperiodo>(entity =>
            {
                entity.HasKey(e => e.CierreId);

                entity.ToTable("cierreperiodo");

                entity.Property(e => e.CierreId).HasColumnName("cierreId");

                entity.Property(e => e.Anio)
                    .HasColumnName("anio")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Bodegaid).HasColumnName("bodegaid");

                entity.Property(e => e.CategoriaId).HasColumnName("categoriaID");

                entity.Property(e => e.Clienteid).HasColumnName("clienteid");

                entity.Property(e => e.Codproducto)
                    .HasColumnName("codproducto")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Entradas).HasColumnName("entradas");

                entity.Property(e => e.Fechacierre)
                    .HasColumnName("fechacierre")
                    .HasColumnType("date");

                entity.Property(e => e.Iteminventarioid).HasColumnName("iteminventarioid");

                entity.Property(e => e.LocationId).HasColumnName("locationID");

                entity.Property(e => e.Mes)
                    .HasColumnName("mes")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Rackid).HasColumnName("rackid");

                entity.Property(e => e.Regimenid).HasColumnName("regimenid");

                entity.Property(e => e.Salant).HasColumnName("salant");

                entity.Property(e => e.Salidas).HasColumnName("salidas");

                entity.Property(e => e.Transaccionid).HasColumnName("transaccionid");
            });

            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.HasKey(e => e.ClienteId);

                entity.HasIndex(e => e.ClienteId)
                    .HasName("IX_Clientes")
                    .IsUnique();

                entity.Property(e => e.ClienteId)
                    .HasColumnName("ClienteID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comentario)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Contacto)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .HasColumnName("correo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiasPago)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.EmailContacto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Nit)
                    .HasColumnName("NIT")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nrc)
                    .HasColumnName("NRC")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.OrderEmailNotifica)
                    .HasColumnName("orderEmailNotifica")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TelefonoContacto)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.Clientes)
                    .HasForeignKey(d => d.EstatusId)
                    .HasConstraintName("FK_Clientes_Estatus");
            });

            modelBuilder.Entity<ConfigFrmBusqueda>(entity =>
            {
                entity.HasKey(e => e.FrmCodigo);

                entity.ToTable("config_frm_busqueda");

                entity.Property(e => e.FrmCodigo)
                    .HasColumnName("frm_codigo")
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.FrmDataset)
                    .HasColumnName("frm_dataset")
                    .IsUnicode(false);

                entity.Property(e => e.FrmFormulario)
                    .IsRequired()
                    .HasColumnName("frm_formulario")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FrmNombre)
                    .IsRequired()
                    .HasColumnName("frm_nombre")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FrmObtieneDataset).HasColumnName("frm_obtiene_dataset");

                entity.Property(e => e.FrmObtieneSp).HasColumnName("frm_obtiene_sp");

                entity.Property(e => e.FrmOrderby)
                    .HasColumnName("frm_orderby")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FrmWhere)
                    .HasColumnName("frm_where")
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ConfiguracionCategoria>(entity =>
            {
                entity.HasKey(e => e.ParametroId);

                entity.Property(e => e.ParametroId)
                    .HasColumnName("ParametroID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Isnecesary).HasColumnName("isnecesary");

                entity.Property(e => e.Parametro)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.ConfiguracionCategoria)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK_ConfiguracionCategoria_Categoria");
            });

            modelBuilder.Entity<Despachos>(entity =>
            {
                entity.HasKey(e => e.DespachoId);

                entity.Property(e => e.DespachoId)
                    .HasColumnName("DespachoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Custodios)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Destino)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Destinoid).HasColumnName("destinoid");

                entity.Property(e => e.DocumentoFiscal)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoMotorista)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.EmpresaCustodios)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaDocFiscal).HasColumnType("datetime");

                entity.Property(e => e.FechaSalida).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.Motorista)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.NoContenedor)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.NoMarchamo)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Ruta)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Transportistaid).HasColumnName("transportistaid");

                entity.Property(e => e.UsuarioTransaccion)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.Transportista)
                    .WithMany(p => p.Despachos)
                    .HasForeignKey(d => d.Transportistaid)
                    .HasConstraintName("FK_despachos_transportista");
            });

            modelBuilder.Entity<Destconsigna>(entity =>
            {
                entity.HasKey(e => e.Destinoid);

                entity.ToTable("destconsigna");

                entity.Property(e => e.Destinoid).HasColumnName("destinoid");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombredest)
                    .HasColumnName("nombredest")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.Paisid).HasColumnName("paisid");
            });

            modelBuilder.Entity<DetalleIngresoCliente>(entity =>
            {
                entity.HasKey(e => e.DtllReceiveId);

                entity.Property(e => e.DtllReceiveId)
                    .HasColumnName("DtllReceiveID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cbm).HasColumnName("CBM");

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodProductoCliente)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DescCliente)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Estilo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Rack)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Talla)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");
            });

            modelBuilder.Entity<DetalleProducto>(entity =>
            {
                entity.HasKey(e => e.DetalleProdId);

                entity.Property(e => e.DetalleProdId)
                    .HasColumnName("DetalleProdID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ParametroId).HasColumnName("ParametroID");

                entity.HasOne(d => d.CodProductoNavigation)
                    .WithMany(p => p.DetalleProducto)
                    .HasForeignKey(d => d.CodProducto)
                    .HasConstraintName("FK_DetalleProducto_Producto");

                entity.HasOne(d => d.Parametro)
                    .WithMany(p => p.DetalleProducto)
                    .HasForeignKey(d => d.ParametroId)
                    .HasConstraintName("FK_DetalleProducto_ConfiguracionCategoria");
            });

            modelBuilder.Entity<DetalleTransacciones>(entity =>
            {
                entity.HasKey(e => e.DtllTrnsaccionId);

                entity.HasIndex(e => e.DtllTrnsaccionId)
                    .HasName("IX_DetalleTransacciones_DetalleID")
                    .IsUnique();

                entity.HasIndex(e => e.InventarioId);

                entity.HasIndex(e => e.TransaccionId)
                    .HasName("IX_DetalleTransacciones");

                entity.Property(e => e.DtllTrnsaccionId)
                    .HasColumnName("DtllTrnsaccionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Conteo).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.DtllPedidoId).HasColumnName("DtllPedidoID");

                entity.Property(e => e.Embalaje)
                    .HasColumnName("embalaje")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Fechaitem)
                    .HasColumnName("fechaitem")
                    .HasColumnType("date");

                entity.Property(e => e.InventarioId).HasColumnName("InventarioID");

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 3)");

                entity.HasOne(d => d.EmbalajeNavigation)
                    .WithMany(p => p.DetalleTransacciones)
                    .HasForeignKey(d => d.Embalaje)
                    .HasConstraintName("FK_embalaje");

                entity.HasOne(d => d.RackNavigation)
                    .WithMany(p => p.DetalleTransaccionesRackNavigation)
                    .HasForeignKey(d => d.Rack)
                    .HasConstraintName("FK_DetalleTransacciones_Racks");

                entity.HasOne(d => d.RackFromNavigation)
                    .WithMany(p => p.DetalleTransaccionesRackFromNavigation)
                    .HasForeignKey(d => d.RackFrom)
                    .HasConstraintName("FK_DetalleTransacciones_Racks1");

                entity.HasOne(d => d.Transaccion)
                    .WithMany(p => p.DetalleTransacciones)
                    .HasForeignKey(d => d.TransaccionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DetalleTransacciones_Transacciones");
            });

            modelBuilder.Entity<Documentos>(entity =>
            {
                entity.HasKey(e => e.DocumentoId);

                entity.Property(e => e.DocumentoId)
                    .HasColumnName("DocumentoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ColNomdoc)
                    .HasColumnName("col_nomdoc")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Documento)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DocumentosxTransaccion>(entity =>
            {
                entity.HasKey(e => e.IddocxTransaccion);

                entity.Property(e => e.IddocxTransaccion)
                    .HasColumnName("IDDocxTransaccion")
                    .ValueGeneratedNever();

                entity.Property(e => e.ArchImage)
                    .HasColumnName("arch_image")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CartaAcepta)
                    .HasColumnName("CARTA_ACEPTA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoId).HasColumnName("DocumentoID");

                entity.Property(e => e.DocumentoTransporte)
                    .HasColumnName("DOCUMENTO_TRANSPORTE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FactComercial)
                    .HasColumnName("FACT_COMERCIAL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FactExportacion)
                    .HasColumnName("FACT_EXPORTACION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FeCartaAcepta)
                    .HasColumnName("FE_CARTA_ACEPTA")
                    .HasColumnType("date");

                entity.Property(e => e.FeDocumentoTransporte)
                    .HasColumnName("FE_DOCUMENTO_TRANSPORTE")
                    .HasColumnType("date");

                entity.Property(e => e.FeFactComercial)
                    .HasColumnName("FE_FACT_COMERCIAL")
                    .HasColumnType("date");

                entity.Property(e => e.FeFactExportacion)
                    .HasColumnName("FE_FACT_EXPORTACION")
                    .HasColumnType("date");

                entity.Property(e => e.FeGuiaTransito)
                    .HasColumnName("FE_GUIA_TRANSITO")
                    .HasColumnType("date");

                entity.Property(e => e.FeIm4)
                    .HasColumnName("FE_IM_4")
                    .HasColumnType("date");

                entity.Property(e => e.FeIm5)
                    .HasColumnName("FE_IM_5")
                    .HasColumnType("date");

                entity.Property(e => e.FeInformeAlmacen)
                    .HasColumnName("FE_INFORME_ALMACEN")
                    .HasColumnType("date");

                entity.Property(e => e.FeManifiesto)
                    .HasColumnName("FE_MANIFIESTO")
                    .HasColumnType("date");

                entity.Property(e => e.FeOrdenCompra)
                    .HasColumnName("FE_ORDEN_COMPRA")
                    .HasColumnType("date");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaDocumento).HasColumnType("datetime");

                entity.Property(e => e.GuiaTransito)
                    .HasColumnName("GUIA_TRANSITO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Im4)
                    .HasColumnName("IM_4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Im5)
                    .HasColumnName("IM_5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InformeAlmacen)
                    .HasColumnName("INFORME_ALMACEN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Manifiesto)
                    .HasColumnName("MANIFIESTO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumento)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OrdenCompra)
                    .HasColumnName("ORDEN_COMPRA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.DocumentosxTransaccion)
                    .HasForeignKey(d => d.DocumentoId)
                    .HasConstraintName("FK_DocumentosxTransaccion_Documentos");

                entity.HasOne(d => d.Transaccion)
                    .WithMany(p => p.DocumentosxTransaccion)
                    .HasForeignKey(d => d.TransaccionId)
                    .HasConstraintName("FK_DocumentosxTransaccion_Transacciones");
            });

            modelBuilder.Entity<DocumentoxTipoTransaccion>(entity =>
            {
                entity.HasKey(e => e.IddocumentoTipoTransac);

                entity.Property(e => e.IddocumentoTipoTransac)
                    .HasColumnName("IDDocumentoTipoTransac")
                    .ValueGeneratedNever();

                entity.Property(e => e.DocumentoId).HasColumnName("DocumentoID");

                entity.Property(e => e.IdtipoTransaccion)
                    .IsRequired()
                    .HasColumnName("IDTipoTransaccion")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.HasOne(d => d.Documento)
                    .WithMany(p => p.DocumentoxTipoTransaccion)
                    .HasForeignKey(d => d.DocumentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoxTipoTransaccion_Documentos");

                entity.HasOne(d => d.IdtipoTransaccionNavigation)
                    .WithMany(p => p.DocumentoxTipoTransaccion)
                    .HasForeignKey(d => d.IdtipoTransaccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DocumentoxTipoTransaccion_TipoTransacciones");
            });

            modelBuilder.Entity<DtllDespacho>(entity =>
            {
                entity.Property(e => e.DtllDespachoId)
                    .HasColumnName("DtllDespachoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DespachoId).HasColumnName("DespachoID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

                entity.HasOne(d => d.Despacho)
                    .WithMany(p => p.DtllDespacho)
                    .HasForeignKey(d => d.DespachoId)
                    .HasConstraintName("FK_DtllDespacho_Despachos");
            });

            modelBuilder.Entity<DtllItemTransaccion>(entity =>
            {
                entity.HasIndex(e => e.DtllItemTransaccionId)
                    .HasName("IX_DtllItemTransaccion")
                    .IsUnique();

                entity.HasIndex(e => e.ItemInventarioId)
                    .HasName("IX_DtllItemTransaccion_ItemInv");

                entity.HasIndex(e => new { e.TransaccionId, e.DtllTransaccionId })
                    .HasName("IX_DtllItemTransaccion_Transaccion");

                entity.Property(e => e.DtllItemTransaccionId)
                    .HasColumnName("DtllItemTransaccionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.DtllTransaccionId).HasColumnName("DtllTransaccionID");

                entity.Property(e => e.ItemInventarioId).HasColumnName("ItemInventarioID");

                entity.Property(e => e.QtySlFinal).HasColumnName("qty_sl_final");

                entity.Property(e => e.QtySlInitial).HasColumnName("qty_sl_initial");

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

                entity.HasOne(d => d.DtllTransaccion)
                    .WithMany(p => p.DtllItemTransaccion)
                    .HasForeignKey(d => d.DtllTransaccionId)
                    .HasConstraintName("FK_DtllItemTransaccion_DetalleTransacciones");

                entity.HasOne(d => d.ItemInventario)
                    .WithMany(p => p.DtllItemTransaccion)
                    .HasForeignKey(d => d.ItemInventarioId)
                    .HasConstraintName("FK_DtllItemTransaccion_ItemInventario");

                entity.HasOne(d => d.RackNavigation)
                    .WithMany(p => p.DtllItemTransaccionRackNavigation)
                    .HasForeignKey(d => d.Rack)
                    .HasConstraintName("FK_DtllItemTransaccion_Racks");

                entity.HasOne(d => d.RackFromNavigation)
                    .WithMany(p => p.DtllItemTransaccionRackFromNavigation)
                    .HasForeignKey(d => d.RackFrom)
                    .HasConstraintName("FK_DtllItemTransaccion_Racks1");
            });

            modelBuilder.Entity<DtllPedido>(entity =>
            {
                entity.Property(e => e.DtllPedidoId)
                    .HasColumnName("DtllPedidoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CodEquivale)
                    .HasColumnName("cod_equivale")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentoId).HasColumnName("DocumentoID");

                entity.Property(e => e.Estilo)
                    .HasColumnName("estilo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaDocumento).HasColumnType("datetime");

                entity.Property(e => e.FechaVcmto)
                    .HasColumnName("fecha_vcmto")
                    .HasColumnType("date");

                entity.Property(e => e.Lote)
                    .HasColumnName("lote")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .HasColumnName("modelo")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.NoDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroOc)
                    .HasColumnName("numero_oc")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.Property(e => e.Uxb)
                    .HasColumnName("uxb")
                    .HasColumnType("numeric(10, 2)");

                entity.HasOne(d => d.Pedido)
                    .WithMany(p => p.DtllPedido)
                    .HasForeignKey(d => d.PedidoId)
                    .HasConstraintName("FK_DtllPedido_Pedido");
            });

            modelBuilder.Entity<DtllPedidosxOperario>(entity =>
            {
                entity.HasKey(e => e.IddtllPedidoAsigando);

                entity.Property(e => e.IddtllPedidoAsigando).HasColumnName("IDDtllPedidoAsigando");

                entity.Property(e => e.IdpedidoAsigando).HasColumnName("IDPedidoAsigando");

                entity.Property(e => e.InventarioId).HasColumnName("InventarioID");

                entity.Property(e => e.ItemInventarioId).HasColumnName("ItemInventarioID");
            });

            modelBuilder.Entity<DtllReceivexItemInventario>(entity =>
            {
                entity.Property(e => e.DtllReceivexItemInventarioId).HasColumnName("DtllReceivexItemInventarioID");

                entity.Property(e => e.DtllReceiveId).HasColumnName("DtllReceiveID");

                entity.Property(e => e.ItemInventarioId).HasColumnName("ItemInventarioID");

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");
            });

            modelBuilder.Entity<Embalaje>(entity =>
            {
                entity.HasKey(e => e.Embalaje1);

                entity.ToTable("embalaje");

                entity.Property(e => e.Embalaje1)
                    .HasColumnName("embalaje")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Descrip)
                    .HasColumnName("descrip")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Estatus>(entity =>
            {
                entity.HasIndex(e => e.EstatusId)
                    .HasName("IX_Estatus")
                    .IsUnique();

                entity.Property(e => e.EstatusId)
                    .HasColumnName("EstatusID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus1)
                    .HasColumnName("Estatus")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateOut).HasColumnName("update_out");
            });

            modelBuilder.Entity<Exportador>(entity =>
            {
                entity.ToTable("exportador");

                entity.Property(e => e.Exportadorid).HasColumnName("exportadorid");

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombrexp)
                    .HasColumnName("nombrexp")
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Grpopcional>(entity =>
            {
                entity.HasKey(e => new { e.Idgrupo, e.Idgrpopcion });

                entity.ToTable("grpopcional");

                entity.Property(e => e.Idgrupo).HasColumnName("idgrupo");

                entity.Property(e => e.Idgrpopcion).HasColumnName("idgrpopcion");

                entity.Property(e => e.Porcentaje)
                    .HasColumnName("porcentaje")
                    .HasColumnType("numeric(6, 3)");
            });

            modelBuilder.Entity<Grpsystem>(entity =>
            {
                entity.HasKey(e => e.Codgrp);

                entity.ToTable("grpsystem");

                entity.Property(e => e.Codgrp).HasColumnName("codgrp");

                entity.Property(e => e.Nomgrp)
                    .HasColumnName("nomgrp")
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Inventario>(entity =>
            {
                entity.HasIndex(e => e.InventarioId)
                    .HasName("IX_Inventario")
                    .IsUnique();

                entity.Property(e => e.InventarioId)
                    .HasColumnName("InventarioID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Articulos).HasColumnType("decimal(10, 4)");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DetfacId).HasColumnName("detfacID");

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.FechaCreacion).HasColumnType("datetime");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Inventario)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK_Inventario_Clientes");

                entity.HasOne(d => d.RackNavigation)
                    .WithMany(p => p.Inventario)
                    .HasForeignKey(d => d.Rack)
                    .HasConstraintName("FK_Inventario_Racks");
            });

            modelBuilder.Entity<ItemInventario>(entity =>
            {
                entity.HasIndex(e => e.CodProducto)
                    .HasName("IX_ItemInventario_1");

                entity.HasIndex(e => e.InventarioId);

                entity.HasIndex(e => e.ItemInventarioId)
                    .HasName("IX_ItemInventario")
                    .IsUnique();

                entity.Property(e => e.ItemInventarioId)
                    .HasColumnName("ItemInventarioID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CodEquivale)
                    .HasColumnName("cod_equivale")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Color)
                    .HasColumnName("color")
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Estilo)
                    .HasColumnName("estilo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaVcmto)
                    .HasColumnName("fecha_vcmto")
                    .HasColumnType("date");

                entity.Property(e => e.Fechaitem)
                    .HasColumnName("fechaitem")
                    .HasColumnType("date");

                entity.Property(e => e.InventarioId).HasColumnName("InventarioID");

                entity.Property(e => e.ItemInventarioFrom).HasMaxLength(10);

                entity.Property(e => e.Lote)
                    .HasColumnName("lote")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Modelo)
                    .HasColumnName("modelo")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroOc)
                    .HasColumnName("numero_oc")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PaisOrig).HasColumnName("pais_orig");

                entity.HasOne(d => d.CodProductoNavigation)
                    .WithMany(p => p.ItemInventario)
                    .HasForeignKey(d => d.CodProducto)
                    .HasConstraintName("FK_ItemInventario_Producto");

                entity.HasOne(d => d.Inventario)
                    .WithMany(p => p.ItemInventario)
                    .HasForeignKey(d => d.InventarioId)
                    .HasConstraintName("FK_ItemInventario_Inventario");
            });

            modelBuilder.Entity<ItemParamaetroxProducto>(entity =>
            {
                entity.HasKey(e => e.ItemParamProducto);

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InventarioId).HasColumnName("InventarioID");

                entity.Property(e => e.ItemInventarioId).HasColumnName("ItemInventarioID");

                entity.Property(e => e.ParametroId).HasColumnName("ParametroID");

                entity.Property(e => e.ValParametro)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.Locationid);

                entity.ToTable("locations");

                entity.Property(e => e.Locationid)
                    .HasColumnName("locationid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.Dsclocation)
                    .HasColumnName("dsclocation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailDomain)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailPasswd)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailPort)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailSender)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdinvShow).HasColumnName("IDinvShow");

                entity.Property(e => e.Mesfin)
                    .HasColumnName("mesfin")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Paisid).HasColumnName("paisid");

                entity.Property(e => e.Perfrec)
                    .HasColumnName("perfrec")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Yearfin)
                    .HasColumnName("yearfin")
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogAsignacionRacks>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.LogId).HasColumnName("LogID");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Codusr).HasColumnName("codusr");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.InventarioId).HasColumnName("InventarioID");

                entity.Property(e => e.RackId).HasColumnName("RackID");
            });

            modelBuilder.Entity<MetodoDescargo>(entity =>
            {
                entity.HasKey(e => e.Descargoid);

                entity.ToTable("metodo_descargo");

                entity.Property(e => e.Descargoid)
                    .HasColumnName("descargoid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Dscdescargo)
                    .HasColumnName("dscdescargo")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Operarios>(entity =>
            {
                entity.HasKey(e => e.Operarioid);

                entity.ToTable("operarios");

                entity.Property(e => e.Operarioid)
                    .HasColumnName("operarioid")
                    .ValueGeneratedNever();

                entity.Property(e => e.HorasDia)
                    .HasColumnName("horas_dia")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.SalarioDia)
                    .HasColumnName("salario_dia")
                    .HasColumnType("numeric(8, 2)");

                entity.Property(e => e.Tipoper).HasColumnName("tipoper");
            });

            modelBuilder.Entity<Optsystem>(entity =>
            {
                entity.HasKey(e => e.Idopt);

                entity.ToTable("optsystem");

                entity.Property(e => e.Idopt).HasColumnName("idopt");

                entity.Property(e => e.Dscopt)
                    .HasColumnName("dscopt")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Paises>(entity =>
            {
                entity.HasKey(e => e.Paisid);

                entity.ToTable("paises");

                entity.Property(e => e.Paisid)
                    .HasColumnName("paisid")
                    .ValueGeneratedNever();

                entity.Property(e => e.Nompais)
                    .HasColumnName("nompais")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.Property(e => e.PedidoId)
                    .HasColumnName("PedidoID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BodegaId).HasColumnName("BodegaID");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Destinoid).HasColumnName("destinoid");

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.FechaRequerido).HasColumnType("datetime");

                entity.Property(e => e.Fechapedido)
                    .HasColumnName("fechapedido")
                    .HasColumnType("date");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.PedidoBarcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RegimenId).HasColumnName("RegimenID");

                entity.Property(e => e.TipoPedido)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.ClienteId)
                    .HasConstraintName("FK_Pedido_Clientes");

                entity.HasOne(d => d.Destino)
                    .WithMany(p => p.Pedido)
                    .HasForeignKey(d => d.Destinoid)
                    .HasConstraintName("FK_destconsigna_pedido");
            });

            modelBuilder.Entity<PedidosxSalida>(entity =>
            {
                entity.HasKey(e => e.IdpedidoxSalida);

                entity.Property(e => e.IdpedidoxSalida).HasColumnName("IDPedidoxSalida");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.Property(e => e.SalidaId).HasColumnName("SalidaID");
            });

            modelBuilder.Entity<PedidoxOperario>(entity =>
            {
                entity.HasKey(e => e.IdpedidoAsigando);

                entity.Property(e => e.IdpedidoAsigando).HasColumnName("IDPedidoAsigando");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DtllPedidoId).HasColumnName("DtllPedidoID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.Property(e => e.FechaInicio).HasColumnType("datetime");

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.CodProducto);

                entity.HasIndex(e => e.CategoriaId)
                    .HasName("IX_Producto_1");

                entity.HasIndex(e => e.UnidadMedida)
                    .HasName("IX_Producto");

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Descargoid).HasColumnName("descargoid");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.Partida)
                    .HasColumnName("partida")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.StockMaximo).HasColumnName("stock_maximo");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK_Producto_Categoria");

                entity.HasOne(d => d.Descargo)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.Descargoid)
                    .HasConstraintName("FK_Producto_metodo_descargo");

                entity.HasOne(d => d.Estatus)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.EstatusId)
                    .HasConstraintName("FK_Producto_Estatus");

                entity.HasOne(d => d.UnidadMedidaNavigation)
                    .WithMany(p => p.Producto)
                    .HasForeignKey(d => d.UnidadMedida)
                    .HasConstraintName("FK_Producto_UnidadMedida");
            });

            modelBuilder.Entity<Racks>(entity =>
            {
                entity.HasKey(e => e.Rack);

                entity.Property(e => e.Rack).ValueGeneratedNever();

                entity.Property(e => e.Alto).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Ancho).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Barcode)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.BodegaId).HasColumnName("BodegaID");

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Largo).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.NombreRack)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RegimenId).HasColumnName("RegimenID");

                entity.HasOne(d => d.Bodega)
                    .WithMany(p => p.Racks)
                    .HasForeignKey(d => d.BodegaId)
                    .HasConstraintName("FK_Racks_Bodegas");
            });

            modelBuilder.Entity<Regimen>(entity =>
            {
                entity.HasKey(e => e.Idregimen);

                entity.HasIndex(e => e.Idregimen)
                    .HasName("IX_Regimen")
                    .IsUnique();

                entity.Property(e => e.Idregimen)
                    .HasColumnName("IDRegimen")
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Regimen1)
                    .HasColumnName("Regimen")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysTempSalidas>(entity =>
            {
                entity.HasKey(e => e.IdtempSalida);

                entity.HasIndex(e => e.CodProducto)
                    .HasName("IX_SysTempSalidas_4");

                entity.HasIndex(e => e.InventarioId)
                    .HasName("IX_SysTempSalidas");

                entity.HasIndex(e => e.PedidoId)
                    .HasName("IX_SysTempSalidas_2");

                entity.HasIndex(e => e.TransaccionId)
                    .HasName("IX_SysTempSalidas_3");

                entity.HasIndex(e => new { e.ItemInventarioId, e.CodProducto })
                    .HasName("IX_SysTempSalidas_1");

                entity.Property(e => e.IdtempSalida).HasColumnName("IDTempSalida");

                entity.Property(e => e.CodProducto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocFac)
                    .HasColumnName("doc_fac")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.DtllPedidoId).HasColumnName("DtllPedidoID");

                entity.Property(e => e.Estilo)
                    .HasColumnName("estilo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Fecha).HasColumnType("datetime");

                entity.Property(e => e.InventarioId).HasColumnName("InventarioID");

                entity.Property(e => e.ItemInventarioId).HasColumnName("ItemInventarioID");

                entity.Property(e => e.Lote)
                    .HasColumnName("lote")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.Property(e => e.Pventa)
                    .HasColumnName("pventa")
                    .HasColumnType("numeric(12, 4)");

                entity.Property(e => e.TransaccionId).HasColumnName("TransaccionID");

                entity.Property(e => e.Usuario)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TabIgard>(entity =>
            {
                entity.HasKey(e => new { e.Codgen, e.TipData });

                entity.ToTable("tab_igard");

                entity.Property(e => e.Codgen)
                    .HasColumnName("codgen")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.TipData)
                    .HasColumnName("tip_data")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AduFro)
                    .HasColumnName("adu_fro")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.CodTransp)
                    .HasColumnName("cod_transp")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Direcc)
                    .HasColumnName("direcc")
                    .HasMaxLength(175)
                    .IsUnicode(false);

                entity.Property(e => e.Giro)
                    .HasColumnName("giro")
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.NitEmp)
                    .HasColumnName("nit_emp")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.Nomgen)
                    .HasColumnName("nomgen")
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.Nrc)
                    .HasColumnName("nrc")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Nremolque)
                    .HasColumnName("nremolque")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Nuchasis)
                    .HasColumnName("nuchasis")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Numotor)
                    .HasColumnName("numotor")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Passport)
                    .HasColumnName("passport")
                    .HasMaxLength(17)
                    .IsUnicode(false);

                entity.Property(e => e.Placa)
                    .HasColumnName("placa")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Vehcolor)
                    .HasColumnName("vehcolor")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Vehmarca)
                    .HasColumnName("vehmarca")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoTransacciones>(entity =>
            {
                entity.HasKey(e => e.IdtipoTransaccion);

                entity.Property(e => e.IdtipoTransaccion)
                    .HasColumnName("IDTipoTransaccion")
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TipoTransaccion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transacciones>(entity =>
            {
                entity.HasKey(e => e.TransaccionId);

                entity.HasIndex(e => e.BodegaId)
                    .HasName("IX_Transacciones_4");

                entity.HasIndex(e => e.ClienteId)
                    .HasName("IX_Transacciones_1");

                entity.HasIndex(e => e.Destinoid)
                    .HasName("IX_Transacciones_2");

                entity.HasIndex(e => e.Exportadorid)
                    .HasName("IX_Transacciones_6");

                entity.HasIndex(e => e.IdtipoTransaccion)
                    .HasName("IX_Transacciones_7");

                entity.HasIndex(e => e.PaisOrig)
                    .HasName("IX_Transacciones_5");

                entity.HasIndex(e => e.PedidoId)
                    .HasName("IX_Transacciones_3");

                entity.HasIndex(e => e.TransaccionId)
                    .HasName("IX_Transacciones")
                    .IsUnique();

                entity.Property(e => e.TransaccionId)
                    .HasColumnName("TransaccionID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AduFro)
                    .HasColumnName("adu_fro")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.BodegaId).HasColumnName("BodegaID");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.CodMotoris)
                    .HasColumnName("cod_motoris")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Contenedor)
                    .HasColumnName("contenedor")
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.Destinoid).HasColumnName("destinoid");

                entity.Property(e => e.EstatusId).HasColumnName("EstatusID");

                entity.Property(e => e.Exportadorid).HasColumnName("exportadorid");

                entity.Property(e => e.FacturaId).HasColumnName("facturaID");

                entity.Property(e => e.FechaReciving).HasColumnType("datetime");

                entity.Property(e => e.FechaTransaccion).HasColumnType("datetime");

                entity.Property(e => e.Fechacrea).HasColumnType("datetime");

                entity.Property(e => e.IdRcontrol).HasColumnName("IdRControl");

                entity.Property(e => e.IdtipoTransaccion)
                    .HasColumnName("IDTipoTransaccion")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Marchamo)
                    .HasColumnName("marchamo")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.NoTransaccion)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Observacion).IsUnicode(false);

                entity.Property(e => e.Operarioid).HasColumnName("operarioid");

                entity.Property(e => e.PaisOrig).HasColumnName("pais_orig");

                entity.Property(e => e.PedidoId).HasColumnName("PedidoID");

                entity.Property(e => e.Placa)
                    .HasColumnName("placa")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RecivingCliente)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.RegimenId).HasColumnName("RegimenID");

                entity.Property(e => e.Remolque)
                    .HasColumnName("remolque")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.TipoIngreso)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.TipoPicking)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Transportistaid).HasColumnName("transportistaid");

                entity.Property(e => e.Usuariocrea)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.AduFroNavigation)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.AduFro)
                    .HasConstraintName("FK_transacciones_aduana1");

                entity.HasOne(d => d.Destino)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.Destinoid)
                    .HasConstraintName("FK_Transacciones_destconsigna");

                entity.HasOne(d => d.Exportador)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.Exportadorid)
                    .HasConstraintName("FK_transacciones_exportador");

                entity.HasOne(d => d.IdtipoTransaccionNavigation)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.IdtipoTransaccion)
                    .HasConstraintName("FK_Transacciones_TipoTransacciones");

                entity.HasOne(d => d.Operario)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.Operarioid)
                    .HasConstraintName("FK_transacciones_operarios");

                entity.HasOne(d => d.PaisOrigNavigation)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.PaisOrig)
                    .HasConstraintName("FK_transacciones_pais1");

                entity.HasOne(d => d.Transportista)
                    .WithMany(p => p.Transacciones)
                    .HasForeignKey(d => d.Transportistaid)
                    .HasConstraintName("FK_transacciones_transportista1");
            });

            modelBuilder.Entity<Transportista>(entity =>
            {
                entity.ToTable("transportista");

                entity.Property(e => e.Transportistaid).HasColumnName("transportistaid");

                entity.Property(e => e.CodTransp)
                    .HasColumnName("cod_transp")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Direccion)
                    .HasColumnName("direccion")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasColumnName("nombre")
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UnidadMedida>(entity =>
            {
                entity.Property(e => e.UnidadMedidaId)
                    .HasColumnName("UnidadMedidaID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Simbolo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoRegistro)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.UnidadMedida1)
                    .HasColumnName("UnidadMedida")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usrsystem>(entity =>
            {
                entity.HasKey(e => e.Codusr);

                entity.ToTable("usrsystem");

                entity.Property(e => e.Codusr).HasColumnName("codusr");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Codgrp).HasColumnName("codgrp");

                entity.Property(e => e.Idusr)
                    .HasColumnName("idusr")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Lastemplate)
                    .HasColumnName("lastemplate")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.Nomusr)
                    .HasColumnName("nomusr")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Usrpasswd)
                    .HasColumnName("usrpasswd")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CodgrpNavigation)
                    .WithMany(p => p.Usrsystem)
                    .HasForeignKey(d => d.Codgrp)
                    .HasConstraintName("FK_usrsystem_usrsystem");
            });
        }
    }
}
