using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EdiApi.Models.EdiDB
{
    public partial class EdiDBContext : DbContext
    {
        public EdiDBContext()
        {
        }

        public EdiDBContext(DbContextOptions<EdiDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EdiComs> EdiComs { get; set; }
        public virtual DbSet<EdiRepSent> EdiRepSent { get; set; }
        public virtual DbSet<EdiSegName> EdiSegName { get; set; }
        public virtual DbSet<EdiUsrSystem> EdiUsrSystem { get; set; }
        public virtual DbSet<IenetAccesses> IenetAccesses { get; set; }
        public virtual DbSet<IenetGroups> IenetGroups { get; set; }
        public virtual DbSet<IenetGroupsAccesses> IenetGroupsAccesses { get; set; }
        public virtual DbSet<IenetUsers> IenetUsers { get; set; }
        public virtual DbSet<LearAth830> LearAth830 { get; set; }
        public virtual DbSet<LearBfr830> LearBfr830 { get; set; }
        public virtual DbSet<LearBsn856> LearBsn856 { get; set; }
        public virtual DbSet<LearCld856> LearCld856 { get; set; }
        public virtual DbSet<LearCodes> LearCodes { get; set; }
        public virtual DbSet<LearCtt856> LearCtt856 { get; set; }
        public virtual DbSet<LearDtm856> LearDtm856 { get; set; }
        public virtual DbSet<LearEquivalencias> LearEquivalencias { get; set; }
        public virtual DbSet<LearEtd856> LearEtd856 { get; set; }
        public virtual DbSet<LearFst830> LearFst830 { get; set; }
        public virtual DbSet<LearGs830> LearGs830 { get; set; }
        public virtual DbSet<LearGs856> LearGs856 { get; set; }
        public virtual DbSet<LearHlol856> LearHlol856 { get; set; }
        public virtual DbSet<LearHlsl856> LearHlsl856 { get; set; }
        public virtual DbSet<LearIsa830> LearIsa830 { get; set; }
        public virtual DbSet<LearIsa856> LearIsa856 { get; set; }
        public virtual DbSet<LearLin830> LearLin830 { get; set; }
        public virtual DbSet<LearLin856> LearLin856 { get; set; }
        public virtual DbSet<LearMea856> LearMea856 { get; set; }
        public virtual DbSet<LearN1830> LearN1830 { get; set; }
        public virtual DbSet<LearN1856> LearN1856 { get; set; }
        public virtual DbSet<LearN4830> LearN4830 { get; set; }
        public virtual DbSet<LearNte830> LearNte830 { get; set; }
        public virtual DbSet<LearPrf856> LearPrf856 { get; set; }
        public virtual DbSet<LearPrs830> LearPrs830 { get; set; }
        public virtual DbSet<LearPureEdi> LearPureEdi { get; set; }
        public virtual DbSet<LearRef830> LearRef830 { get; set; }
        public virtual DbSet<LearRef856> LearRef856 { get; set; }
        public virtual DbSet<LearSdp830> LearSdp830 { get; set; }
        public virtual DbSet<LearShp830> LearShp830 { get; set; }
        public virtual DbSet<LearSn1856> LearSn1856 { get; set; }
        public virtual DbSet<LearSt830> LearSt830 { get; set; }
        public virtual DbSet<LearSt856> LearSt856 { get; set; }
        public virtual DbSet<LearTd1856> LearTd1856 { get; set; }
        public virtual DbSet<LearTd3856> LearTd3856 { get; set; }
        public virtual DbSet<LearTd5856> LearTd5856 { get; set; }
        public virtual DbSet<LearUit830> LearUit830 { get; set; }
        public virtual DbSet<PaylessProdPriori> PaylessProdPriori { get; set; }
        public virtual DbSet<PaylessProdPrioriArchDet> PaylessProdPrioriArchDet { get; set; }
        public virtual DbSet<PaylessProdPrioriArchM> PaylessProdPrioriArchM { get; set; }
        public virtual DbSet<PaylessProdPrioriDet> PaylessProdPrioriDet { get; set; }
        public virtual DbSet<PaylessProdPrioriM> PaylessProdPrioriM { get; set; }
        public virtual DbSet<PaylessReportes> PaylessReportes { get; set; }
        public virtual DbSet<PaylessTiendas> PaylessTiendas { get; set; }
        public virtual DbSet<PedidosDetExternos> PedidosDetExternos { get; set; }
        public virtual DbSet<PedidosEstadosExternos> PedidosEstadosExternos { get; set; }
        public virtual DbSet<PedidosExternos> PedidosExternos { get; set; }
        public virtual DbSet<UsuariosExternos> UsuariosExternos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EdiComs>(entity =>
            {
                entity.Property(e => e.Freg)
                    .HasColumnName("FReg")
                    .HasMaxLength(16);

                entity.Property(e => e.Log).HasColumnType("text");

                entity.Property(e => e.Type).HasMaxLength(16);
            });

            modelBuilder.Entity<EdiRepSent>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(9);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.Fecha).HasMaxLength(18);

                entity.Property(e => e.HashId).HasMaxLength(128);

                entity.Property(e => e.Log).HasColumnType("text");

                entity.Property(e => e.Tipo).HasMaxLength(3);
            });

            modelBuilder.Entity<EdiSegName>(entity =>
            {
                entity.HasKey(e => e.Segment);

                entity.Property(e => e.Segment)
                    .HasMaxLength(16)
                    .ValueGeneratedNever();

                entity.Property(e => e.DescEng).HasColumnType("text");

                entity.Property(e => e.DescSpa).HasColumnType("text");

                entity.Property(e => e.Eng).HasMaxLength(128);

                entity.Property(e => e.Spa).HasMaxLength(128);
            });

            modelBuilder.Entity<EdiUsrSystem>(entity =>
            {
                entity.Property(e => e.HashId).HasMaxLength(128);
            });

            modelBuilder.Entity<IenetAccesses>(entity =>
            {
                entity.ToTable("IEnetAccesses");

                entity.Property(e => e.Descr).HasMaxLength(128);
            });

            modelBuilder.Entity<IenetGroups>(entity =>
            {
                entity.ToTable("IEnetGroups");

                entity.Property(e => e.Descr).HasMaxLength(128);
            });

            modelBuilder.Entity<IenetGroupsAccesses>(entity =>
            {
                entity.ToTable("IEnetGroupsAccesses");

                entity.Property(e => e.IdIenetAccess).HasColumnName("IdIEnetAccess");

                entity.Property(e => e.IdIenetGroup).HasColumnName("IdIEnetGroup");
            });

            modelBuilder.Entity<IenetUsers>(entity =>
            {
                entity.ToTable("IEnetUsers");

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.CodUsr)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.HashId).HasMaxLength(128);

                entity.Property(e => e.IdIenetGroup).HasColumnName("IdIEnetGroup");

                entity.Property(e => e.NomUsr)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.UsrPassword)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<LearAth830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ATH830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_ATH830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.EndDate).HasMaxLength(6);

                entity.Property(e => e.NotUsed).HasMaxLength(1);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.Quantity).HasMaxLength(10);

                entity.Property(e => e.ResourceAuthCode).HasMaxLength(2);

                entity.Property(e => e.StartDate).HasMaxLength(6);
            });

            modelBuilder.Entity<LearBfr830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_BFR830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_BFR830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ContractNumber).HasMaxLength(1);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ForecastGenerationDate).HasMaxLength(6);

                entity.Property(e => e.ForecastHorizonEnd).HasMaxLength(6);

                entity.Property(e => e.ForecastHorizonStart).HasMaxLength(6);

                entity.Property(e => e.ForecastOrderNumber).HasMaxLength(1);

                entity.Property(e => e.ForecastQuantityQualifier).HasMaxLength(1);

                entity.Property(e => e.ForecastTypeQualifier).HasMaxLength(2);

                entity.Property(e => e.ForecastUpdatedDate).HasMaxLength(6);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(1);

                entity.Property(e => e.ReleaseNumber).HasMaxLength(4);

                entity.Property(e => e.TransactionSetPurposeCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearBsn856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_BSN856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_BSN856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.BsnDate).HasMaxLength(6);

                entity.Property(e => e.BsnTime).HasMaxLength(4);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ShipIdentification).HasMaxLength(30);

                entity.Property(e => e.TransactionSetPurposeCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearCld856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_CLD856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_CLD856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.NumberOfCustomerLoads).HasMaxLength(2);

                entity.Property(e => e.PackagingCode).HasMaxLength(6);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.UnitsShipped).HasMaxLength(30);
            });

            modelBuilder.Entity<LearCodes>(entity =>
            {
                entity.ToTable("LEAR_CODES");

                entity.HasIndex(e => new { e.Str, e.Param })
                    .HasName("IX_LEAR_CODES_unique")
                    .IsUnique();

                entity.Property(e => e.Param).HasMaxLength(128);

                entity.Property(e => e.Str)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Value).HasMaxLength(256);

                entity.Property(e => e.ValueEsp).HasMaxLength(256);
            });

            modelBuilder.Entity<LearCtt856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_CTT856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_CTT856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.NumberOfLineItems).HasMaxLength(32);

                entity.Property(e => e.ParentHashId)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<LearDtm856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_DTM856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_DTM856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.DateTimeQualifier).HasMaxLength(3);

                entity.Property(e => e.DtmDate).HasMaxLength(6);

                entity.Property(e => e.DtmTime).HasMaxLength(4);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearEquivalencias>(entity =>
            {
                entity.ToTable("LEAR_EQUIVALENCIAS");

                entity.Property(e => e.CartonSizemm).HasMaxLength(64);

                entity.Property(e => e.CartonsXpallet).HasColumnName("CartonsXPallet");

                entity.Property(e => e.CodProducto).HasMaxLength(50);

                entity.Property(e => e.CodProductoLear).HasMaxLength(50);

                entity.Property(e => e.GrossWeightXcartonKg).HasColumnName("GrossWeightXCartonKg");

                entity.Property(e => e.PalletSizemm).HasMaxLength(64);

                entity.Property(e => e.Spq).HasColumnName("SPQ");

                entity.Property(e => e.Unit).HasMaxLength(16);
            });

            modelBuilder.Entity<LearEtd856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ETD856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_ETD856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ExcessTransportationReasonCode).HasMaxLength(2);

                entity.Property(e => e.ExcessTransportationResponsibilityCode).HasMaxLength(1);

                entity.Property(e => e.ParentHashId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ReferenceNumber).HasMaxLength(30);

                entity.Property(e => e.ReferenceNumberQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearFst830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_FST830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_FST830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ForecastQualifier).HasMaxLength(1);

                entity.Property(e => e.ForecastTimingQualifier).HasMaxLength(1);

                entity.Property(e => e.FstDate).HasMaxLength(6);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.Quantity).HasMaxLength(10);

                entity.Property(e => e.RealQty).HasMaxLength(16);
            });

            modelBuilder.Entity<LearGs830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_GS830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_GS830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationReceiverCode).HasMaxLength(15);

                entity.Property(e => e.ApplicationSenderCode).HasMaxLength(15);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.FunctionalIdCode).HasMaxLength(2);

                entity.Property(e => e.GroupControlNumber).HasMaxLength(9);

                entity.Property(e => e.GsDate).HasMaxLength(8);

                entity.Property(e => e.GsTime).HasMaxLength(8);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.ResponsibleAgencyCode).HasMaxLength(2);

                entity.Property(e => e.Version).HasMaxLength(12);
            });

            modelBuilder.Entity<LearGs856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_GS856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_GS856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationReceiverCode).HasMaxLength(15);

                entity.Property(e => e.ApplicationSenderCode).HasMaxLength(15);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.FunctionalIdCode).HasMaxLength(2);

                entity.Property(e => e.GroupControlNumber).HasMaxLength(9);

                entity.Property(e => e.GsDate).HasMaxLength(8);

                entity.Property(e => e.GsTime).HasMaxLength(8);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.ResponsibleAgencyCode).HasMaxLength(2);

                entity.Property(e => e.Version).HasMaxLength(12);
            });

            modelBuilder.Entity<LearHlol856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_HLOL856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_HLOL856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.HierarchicalIdNumber).HasMaxLength(12);

                entity.Property(e => e.HierarchicalLevelCode).HasMaxLength(2);

                entity.Property(e => e.HierarchicalParentIdNumber).HasMaxLength(12);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearHlsl856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_HLSL856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_HLSL856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.HierarchicalIdNumber).HasMaxLength(12);

                entity.Property(e => e.HierarchicalLevelCode).HasMaxLength(2);

                entity.Property(e => e.ParentHashId)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<LearIsa830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ISA830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_ISA830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AcknowledgmentRequested).HasMaxLength(1);

                entity.Property(e => e.AuthorizationInformation).HasMaxLength(18);

                entity.Property(e => e.AuthorizationInformationQualifier).HasMaxLength(2);

                entity.Property(e => e.ComponentElementSeparator).HasMaxLength(1);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.InterchangeControlNumber).HasMaxLength(9);

                entity.Property(e => e.InterchangeControlStandardsId).HasMaxLength(1);

                entity.Property(e => e.InterchangeControlVersion).HasMaxLength(5);

                entity.Property(e => e.InterchangeDate).HasMaxLength(6);

                entity.Property(e => e.InterchangeReceiverId).HasMaxLength(15);

                entity.Property(e => e.InterchangeReceiverIdQualifier).HasMaxLength(2);

                entity.Property(e => e.InterchangeSenderId).HasMaxLength(15);

                entity.Property(e => e.InterchangeSenderIdQualifier).HasMaxLength(2);

                entity.Property(e => e.InterchangeTime).HasMaxLength(4);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.SecurityInformation).HasMaxLength(10);

                entity.Property(e => e.SecurityInformationQualifier).HasMaxLength(2);

                entity.Property(e => e.UsageIndicator).HasMaxLength(1);
            });

            modelBuilder.Entity<LearIsa856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ISA856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_ISA856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AcknowledgmentRequested).HasMaxLength(1);

                entity.Property(e => e.AuthorizationInformation).HasMaxLength(18);

                entity.Property(e => e.AuthorizationInformationQualifier).HasMaxLength(2);

                entity.Property(e => e.ComponentElementSeparator).HasMaxLength(1);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.InterchangeControlNumber).HasMaxLength(9);

                entity.Property(e => e.InterchangeControlStandardsId).HasMaxLength(1);

                entity.Property(e => e.InterchangeControlVersion).HasMaxLength(5);

                entity.Property(e => e.InterchangeDate).HasMaxLength(6);

                entity.Property(e => e.InterchangeReceiverId).HasMaxLength(15);

                entity.Property(e => e.InterchangeReceiverIdQualifier).HasMaxLength(2);

                entity.Property(e => e.InterchangeSenderId).HasMaxLength(15);

                entity.Property(e => e.InterchangeSenderIdQualifier).HasMaxLength(2);

                entity.Property(e => e.InterchangeTime).HasMaxLength(4);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.SecurityInformation).HasMaxLength(10);

                entity.Property(e => e.SecurityInformationQualifier).HasMaxLength(2);

                entity.Property(e => e.UsageIndicator).HasMaxLength(1);
            });

            modelBuilder.Entity<LearLin830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_LIN830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_LIN830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignedIdentification).HasMaxLength(1);

                entity.Property(e => e.Comments).HasColumnType("text");

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.ProductId).HasMaxLength(22);

                entity.Property(e => e.ProductIdQualifier).HasMaxLength(2);

                entity.Property(e => e.ProductPurchaseId).HasMaxLength(30);

                entity.Property(e => e.ProductPurchaseIdQualifier).HasMaxLength(2);

                entity.Property(e => e.ProductRefId).HasMaxLength(30);

                entity.Property(e => e.ProductRefIdQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearLin856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_LIN856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_LIN856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.ProductId1).HasMaxLength(30);

                entity.Property(e => e.ProductId10).HasMaxLength(30);

                entity.Property(e => e.ProductId11).HasMaxLength(30);

                entity.Property(e => e.ProductId12).HasMaxLength(30);

                entity.Property(e => e.ProductId13).HasMaxLength(30);

                entity.Property(e => e.ProductId14).HasMaxLength(30);

                entity.Property(e => e.ProductId15).HasMaxLength(30);

                entity.Property(e => e.ProductId16).HasMaxLength(30);

                entity.Property(e => e.ProductId2).HasMaxLength(30);

                entity.Property(e => e.ProductId3).HasMaxLength(30);

                entity.Property(e => e.ProductId4).HasMaxLength(30);

                entity.Property(e => e.ProductId5).HasMaxLength(30);

                entity.Property(e => e.ProductId6).HasMaxLength(30);

                entity.Property(e => e.ProductId7).HasMaxLength(30);

                entity.Property(e => e.ProductId8).HasMaxLength(30);

                entity.Property(e => e.ProductId9).HasMaxLength(30);

                entity.Property(e => e.ProductIdQualifier1).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier10).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier11).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier12).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier13).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier14).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier15).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier16).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier2).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier3).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier4).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier5).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier6).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier7).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier8).HasMaxLength(2);

                entity.Property(e => e.ProductIdQualifier9).HasMaxLength(2);
            });

            modelBuilder.Entity<LearMea856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_MEA856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_MEA856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.MeasurementDimensionQualifier).HasMaxLength(3);

                entity.Property(e => e.MeasurementReferenceIdCode).HasMaxLength(2);

                entity.Property(e => e.MeasurementValue).HasMaxLength(10);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.UnitOfMeasure).HasMaxLength(2);
            });

            modelBuilder.Entity<LearN1830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_N1830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_N1830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.IdCode).HasMaxLength(17);

                entity.Property(e => e.IdCodeQualifier).HasMaxLength(2);

                entity.Property(e => e.Name).HasMaxLength(35);

                entity.Property(e => e.OrganizationId).HasMaxLength(2);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearN1856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_N1856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_N1856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.EntityIdentifierCode).HasMaxLength(2);

                entity.Property(e => e.IdCode).HasMaxLength(17);

                entity.Property(e => e.IdCodeQualifier).HasMaxLength(2);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearN4830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_N4830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_N4830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.CityName).HasMaxLength(19);

                entity.Property(e => e.CountryCode).HasMaxLength(4);

                entity.Property(e => e.EdiStr)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.LocationId).HasMaxLength(25);

                entity.Property(e => e.LocationQualifier).HasMaxLength(2);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.PostalCode).HasMaxLength(9);

                entity.Property(e => e.Province).HasMaxLength(2);
            });

            modelBuilder.Entity<LearNte830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_NTE830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_NTE830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.Message).HasMaxLength(60);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.ReferenceCode).HasMaxLength(3);
            });

            modelBuilder.Entity<LearPrf856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_PRF856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_PRF856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ChangeOrderSequenceNumber).HasMaxLength(8);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.PurchaseOrderDate).HasMaxLength(8);

                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(13);

                entity.Property(e => e.ReleaseNumber).HasMaxLength(30);
            });

            modelBuilder.Entity<LearPrs830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_PRS830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_PRS830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.StatusCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearPureEdi>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_PureEdi");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.Fingreso)
                    .HasColumnName("FIngreso")
                    .HasMaxLength(16);

                entity.Property(e => e.Fprocesado)
                    .HasColumnName("FProcesado")
                    .HasMaxLength(16);

                entity.Property(e => e.InOut)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Log).HasColumnType("text");

                entity.Property(e => e.NombreArchivo).HasMaxLength(256);
            });

            modelBuilder.Entity<LearRef830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_REF830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_REF830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(80);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.RefNumber).HasMaxLength(30);

                entity.Property(e => e.RefNumberQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearRef856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_REF856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_REF856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.ReferenceNumber).HasMaxLength(16);

                entity.Property(e => e.ReferenceNumberQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearSdp830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_SDP830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_SDP830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.CalendarPatternCode).HasMaxLength(2);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.PatternTimeCode).HasMaxLength(1);
            });

            modelBuilder.Entity<LearShp830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_SHP830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_SHP830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccumulationEndDate).HasMaxLength(6);

                entity.Property(e => e.AccumulationStartDate).HasMaxLength(6);

                entity.Property(e => e.AccumulationTime).HasMaxLength(4);

                entity.Property(e => e.DateTimeQualifier).HasMaxLength(3);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.Quantity).HasMaxLength(10);

                entity.Property(e => e.QuantityQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearSn1856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_SN1856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_SN1856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.NumberOfUnitsShipped).HasMaxLength(10);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.QuantityShipped).HasMaxLength(9);

                entity.Property(e => e.UnitOfMeasurementCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearSt830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ST830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_ST830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ControlNumber).HasMaxLength(9);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.IdCode).HasMaxLength(3);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearSt856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ST856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_ST856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ControlNumber).HasMaxLength(9);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.IdCode).HasMaxLength(3);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearTd1856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_TD1856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_TD1856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.LadingQuantity).HasMaxLength(7);

                entity.Property(e => e.PackagingCode).HasMaxLength(5);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearTd3856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_TD3856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_TD3856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.EquipmentDescriptionCode).HasMaxLength(2);

                entity.Property(e => e.EquipmentInitial).HasMaxLength(4);

                entity.Property(e => e.EquipmentNumber).HasMaxLength(10);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);
            });

            modelBuilder.Entity<LearTd5856>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_TD5856");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_TD5856IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.IdCodeQualifier).HasMaxLength(2);

                entity.Property(e => e.IdentificationCode).HasMaxLength(17);

                entity.Property(e => e.LocationIdentifier).HasMaxLength(7);

                entity.Property(e => e.LocationQualifier).HasMaxLength(2);

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.RoutingSequenceCode).HasMaxLength(2);

                entity.Property(e => e.TransportationMethodCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearUit830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_UIT830");

                entity.HasIndex(e => e.ParentHashId)
                    .HasName("IndexLEAR_UIT830IParentHashId");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ParentHashId).HasMaxLength(128);

                entity.Property(e => e.UnitOfMeasure).HasMaxLength(2);
            });

            modelBuilder.Entity<PaylessProdPriori>(entity =>
            {
                entity.ToTable("PAYLESS_ProdPriori");

                entity.Property(e => e.Cargada).HasMaxLength(255);

                entity.Property(e => e.Categoria).HasMaxLength(255);

                entity.Property(e => e.Cp)
                    .HasColumnName("CP")
                    .HasMaxLength(255);

                entity.Property(e => e.Etiquetada).HasMaxLength(255);

                entity.Property(e => e.Oid).HasColumnName("OID");

                entity.Property(e => e.Periodo).HasMaxLength(10);

                entity.Property(e => e.Pickeada).HasMaxLength(255);

                entity.Property(e => e.Preinspeccion).HasMaxLength(255);
            });

            modelBuilder.Entity<PaylessProdPrioriArchDet>(entity =>
            {
                entity.ToTable("PAYLESS_ProdPrioriArchDet");

                entity.Property(e => e.Barcode)
                    .HasColumnName("barcode")
                    .HasMaxLength(16);
            });

            modelBuilder.Entity<PaylessProdPrioriArchM>(entity =>
            {
                entity.ToTable("PAYLESS_ProdPrioriArchM");

                entity.Property(e => e.CodUsr).HasMaxLength(128);

                entity.Property(e => e.InsertDate).HasMaxLength(16);

                entity.Property(e => e.Periodo).HasMaxLength(10);

                entity.Property(e => e.UpdateDate).HasMaxLength(16);
            });

            modelBuilder.Entity<PaylessProdPrioriDet>(entity =>
            {
                entity.ToTable("PAYLESS_ProdPrioriDet");

                entity.Property(e => e.Barcode).HasMaxLength(16);

                entity.Property(e => e.Cargada).HasMaxLength(24);

                entity.Property(e => e.Categoria).HasMaxLength(256);

                entity.Property(e => e.Cp)
                    .HasColumnName("CP")
                    .HasMaxLength(8);

                entity.Property(e => e.Departamento).HasMaxLength(16);

                entity.Property(e => e.Estado).HasMaxLength(4);

                entity.Property(e => e.Etiquetada).HasMaxLength(24);

                entity.Property(e => e.IdPaylessProdPrioriM).HasColumnName("IdPAYLESS_ProdPrioriM");

                entity.Property(e => e.Lote).HasMaxLength(8);

                entity.Property(e => e.Oid)
                    .HasColumnName("OID")
                    .HasMaxLength(16);

                entity.Property(e => e.Pickeada).HasMaxLength(24);

                entity.Property(e => e.PoolP).HasMaxLength(4);

                entity.Property(e => e.Preinspeccion).HasMaxLength(32);

                entity.Property(e => e.Pri).HasMaxLength(4);

                entity.Property(e => e.Producto).HasMaxLength(16);

                entity.Property(e => e.Talla).HasMaxLength(8);
            });

            modelBuilder.Entity<PaylessProdPrioriM>(entity =>
            {
                entity.ToTable("PAYLESS_ProdPrioriM");

                entity.Property(e => e.CodUsr).HasMaxLength(128);

                entity.Property(e => e.InsertDate).HasMaxLength(16);

                entity.Property(e => e.Periodo).HasMaxLength(10);

                entity.Property(e => e.Transporte).HasMaxLength(32);

                entity.Property(e => e.UpdateDate).HasMaxLength(16);
            });

            modelBuilder.Entity<PaylessReportes>(entity =>
            {
                entity.ToTable("PAYLESS_Reportes");

                entity.Property(e => e.FechaGen).HasMaxLength(10);

                entity.Property(e => e.Periodo).HasMaxLength(10);

                entity.Property(e => e.Tipo).HasMaxLength(1);
            });

            modelBuilder.Entity<PaylessTiendas>(entity =>
            {
                entity.ToTable("PAYLESS_Tiendas");

                entity.Property(e => e.Cel).HasMaxLength(32);

                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.Descr).HasMaxLength(256);

                entity.Property(e => e.Direc).HasMaxLength(256);

                entity.Property(e => e.Distrito).HasMaxLength(8);

                entity.Property(e => e.Lider).HasMaxLength(128);

                entity.Property(e => e.Tel).HasMaxLength(32);
            });

            modelBuilder.Entity<PedidosDetExternos>(entity =>
            {
                entity.Property(e => e.CodProducto).HasMaxLength(50);

                entity.Property(e => e.Producto).HasMaxLength(1);
            });

            modelBuilder.Entity<PedidosEstadosExternos>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descripcion).HasMaxLength(64);
            });

            modelBuilder.Entity<PedidosExternos>(entity =>
            {
                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.FechaCreacion).HasMaxLength(16);

                entity.Property(e => e.FechaPedido).HasMaxLength(16);

                entity.Property(e => e.FechaUltActualizacion).HasMaxLength(10);

                entity.Property(e => e.Periodo).HasMaxLength(10);
            });

            modelBuilder.Entity<UsuariosExternos>(entity =>
            {
                entity.Property(e => e.ClienteId).HasColumnName("ClienteID");

                entity.Property(e => e.CodUsr).HasMaxLength(128);

                entity.Property(e => e.HashId).HasMaxLength(128);

                entity.Property(e => e.NomUsr).HasMaxLength(128);

                entity.Property(e => e.UsrPassword).HasMaxLength(256);
            });
        }
    }
}
