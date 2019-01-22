using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EdiApi.Models
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

        public virtual DbSet<LearAth830> LearAth830 { get; set; }
        public virtual DbSet<LearBfr830> LearBfr830 { get; set; }
        public virtual DbSet<LearFst830> LearFst830 { get; set; }
        public virtual DbSet<LearGs830> LearGs830 { get; set; }
        public virtual DbSet<LearIsa830> LearIsa830 { get; set; }
        public virtual DbSet<LearLin830> LearLin830 { get; set; }
        public virtual DbSet<LearN1830> LearN1830 { get; set; }
        public virtual DbSet<LearN4830> LearN4830 { get; set; }
        public virtual DbSet<LearPrs830> LearPrs830 { get; set; }
        public virtual DbSet<LearRef830> LearRef830 { get; set; }
        public virtual DbSet<LearSdp830> LearSdp830 { get; set; }
        public virtual DbSet<LearShp830> LearShp830 { get; set; }
        public virtual DbSet<LearSt830> LearSt830 { get; set; }
        public virtual DbSet<LearUit830> LearUit830 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearAth830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ATH830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.EndDate).HasMaxLength(6);

                entity.Property(e => e.NotUsed).HasMaxLength(1);

                entity.Property(e => e.Quantity).HasMaxLength(10);

                entity.Property(e => e.ResourceAuthCode).HasMaxLength(2);

                entity.Property(e => e.StartDate).HasMaxLength(6);
            });

            modelBuilder.Entity<LearBfr830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_BFR830");

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

                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(1);

                entity.Property(e => e.ReleaseNumber).HasMaxLength(4);

                entity.Property(e => e.TransactionSetPurposeCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearFst830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_FST830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ForecastQualifier).HasMaxLength(1);

                entity.Property(e => e.ForecastTimingQualifier).HasMaxLength(1);

                entity.Property(e => e.FstDate).HasMaxLength(6);

                entity.Property(e => e.Quantity).HasMaxLength(10);
            });

            modelBuilder.Entity<LearGs830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_GS830");

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

                entity.Property(e => e.ResponsibleAgencyCode).HasMaxLength(2);

                entity.Property(e => e.Version).HasMaxLength(12);
            });

            modelBuilder.Entity<LearIsa830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ISA830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AcknowledgmentRequested).HasMaxLength(1);

                entity.Property(e => e.AuthorizationInformation).HasMaxLength(10);

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

                entity.Property(e => e.SecurityInformation).HasMaxLength(10);

                entity.Property(e => e.SecurityInformationQualifier).HasMaxLength(2);

                entity.Property(e => e.UsageIndicator).HasMaxLength(1);
            });

            modelBuilder.Entity<LearLin830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_LIN830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AssignedIdentification).HasMaxLength(1);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.ProductId).HasMaxLength(22);

                entity.Property(e => e.ProductIdQualifier).HasMaxLength(2);

                entity.Property(e => e.ProductPurchaseId).HasMaxLength(30);

                entity.Property(e => e.ProductPurchaseIdQualifier).HasMaxLength(2);

                entity.Property(e => e.ProductRefId).HasMaxLength(30);

                entity.Property(e => e.ProductRefIdQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearN1830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_N1830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.IdCode).HasMaxLength(17);

                entity.Property(e => e.IdCodeQualifier).HasMaxLength(2);

                entity.Property(e => e.Name).HasMaxLength(35);

                entity.Property(e => e.OrganizationId).HasMaxLength(2);
            });

            modelBuilder.Entity<LearN4830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_N4830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.CityName).HasMaxLength(19);

                entity.Property(e => e.CountryCode).HasMaxLength(2);

                entity.Property(e => e.EdiStr)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.LocationId).HasMaxLength(25);

                entity.Property(e => e.LocationQualifier).HasMaxLength(2);

                entity.Property(e => e.PostalCode).HasMaxLength(9);

                entity.Property(e => e.Province).HasMaxLength(2);
            });

            modelBuilder.Entity<LearPrs830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_PRS830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.StatusCode).HasMaxLength(2);
            });

            modelBuilder.Entity<LearRef830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_REF830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(80);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.RefNumber).HasMaxLength(30);

                entity.Property(e => e.RefNumberQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearSdp830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_SDP830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.CalendarPatternCode).HasMaxLength(2);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.PatternTimeCode).HasMaxLength(1);
            });

            modelBuilder.Entity<LearShp830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_SHP830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccumulationEndDate).HasMaxLength(6);

                entity.Property(e => e.AccumulationStartDate).HasMaxLength(6);

                entity.Property(e => e.AccumulationTime).HasMaxLength(4);

                entity.Property(e => e.DateTimeQualifier).HasMaxLength(3);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.Quantity).HasMaxLength(10);

                entity.Property(e => e.QuantityQualifier).HasMaxLength(2);
            });

            modelBuilder.Entity<LearSt830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_ST830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.ControlNumber).HasMaxLength(9);

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.IdCode).HasMaxLength(3);
            });

            modelBuilder.Entity<LearUit830>(entity =>
            {
                entity.HasKey(e => e.HashId);

                entity.ToTable("LEAR_UIT830");

                entity.Property(e => e.HashId)
                    .HasMaxLength(128)
                    .ValueGeneratedNever();

                entity.Property(e => e.EdiStr).HasColumnType("text");

                entity.Property(e => e.UnitOfMeasure).HasMaxLength(2);
            });
        }
    }
}
