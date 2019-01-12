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

        public virtual DbSet<LearBfr> LearBfr { get; set; }
        public virtual DbSet<LearGs> LearGs { get; set; }
        public virtual DbSet<LearIsa> LearIsa { get; set; }
        public virtual DbSet<LearLin> LearLin { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LearBfr>(entity =>
            {
                entity.ToTable("Lear_Bfr");

                entity.Property(e => e.ContractNumber).HasMaxLength(30);

                entity.Property(e => e.ForecastGenerationDate).HasMaxLength(6);

                entity.Property(e => e.ForecastHorizonEnd).HasMaxLength(6);

                entity.Property(e => e.ForecastHorizonStart).HasMaxLength(6);

                entity.Property(e => e.ForecastOrderNumber).HasMaxLength(30);

                entity.Property(e => e.ForecastQuantityQualifier).HasMaxLength(1);

                entity.Property(e => e.ForecastTypeQualifier).HasMaxLength(2);

                entity.Property(e => e.ForecastUpdatedDate).HasMaxLength(6);

                entity.Property(e => e.IdGs).HasColumnName("Id_Gs");

                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(22);

                entity.Property(e => e.ReleaseNumber).HasMaxLength(4);

                entity.Property(e => e.Time).HasMaxLength(4);

                entity.Property(e => e.TransactionSetPurposeCode).HasMaxLength(2);

                entity.HasOne(d => d.IdGsNavigation)
                    .WithMany(p => p.LearBfr)
                    .HasForeignKey(d => d.IdGs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lear_Bfr_LEAR_GS");
            });

            modelBuilder.Entity<LearGs>(entity =>
            {
                entity.ToTable("LEAR_GS");

                entity.Property(e => e.ApplicationReceiverCode).HasMaxLength(16);

                entity.Property(e => e.ApplicationSenderCode).HasMaxLength(16);

                entity.Property(e => e.FunctionalIdCode).HasMaxLength(2);

                entity.Property(e => e.GroupControlNumber).HasMaxLength(9);

                entity.Property(e => e.GsDate).HasMaxLength(8);

                entity.Property(e => e.GsTime).HasMaxLength(4);

                entity.Property(e => e.IdIsa).HasColumnName("Id_Isa");

                entity.Property(e => e.ResponsibleAgencyCode).HasMaxLength(2);

                entity.Property(e => e.Version).HasMaxLength(12);

                entity.HasOne(d => d.IdIsaNavigation)
                    .WithMany(p => p.LearGs)
                    .HasForeignKey(d => d.IdIsa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LEAR_GS_LEAR_ISA");
            });

            modelBuilder.Entity<LearIsa>(entity =>
            {
                entity.ToTable("LEAR_ISA");

                entity.Property(e => e.AcknowledgmentRequested)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AuthorizationInformation).HasMaxLength(16);

                entity.Property(e => e.AuthorizationInformationQualifier).HasMaxLength(4);

                entity.Property(e => e.ComponentElementSeparator).HasMaxLength(1);

                entity.Property(e => e.InterchangeControlStandardsId)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InterchangeControlVersion).HasMaxLength(8);

                entity.Property(e => e.InterchangeDate).HasMaxLength(6);

                entity.Property(e => e.InterchangeReceiverId).HasMaxLength(16);

                entity.Property(e => e.InterchangeReceiverIdQualifier).HasMaxLength(4);

                entity.Property(e => e.InterchangeSenderId).HasMaxLength(16);

                entity.Property(e => e.InterchangeSenderIdQualifier).HasMaxLength(4);

                entity.Property(e => e.InterchangeTime).HasMaxLength(4);

                entity.Property(e => e.SecurityInformation).HasMaxLength(16);

                entity.Property(e => e.SecurityInformationQualifier).HasMaxLength(4);

                entity.Property(e => e.SegmentTerminator)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.UsageIndicator)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LearLin>(entity =>
            {
                entity.ToTable("LEAR_LIN");

                entity.Property(e => e.AssignedIdentification).HasMaxLength(6);

                entity.Property(e => e.IdBfr).HasColumnName("Id_Bfr");

                entity.Property(e => e.ProductId).HasMaxLength(22);

                entity.Property(e => e.ProductIdQualifier).HasMaxLength(2);

                entity.Property(e => e.ProductPurchaseId).HasMaxLength(30);

                entity.Property(e => e.ProductPurchaseIdQualifier).HasMaxLength(2);

                entity.Property(e => e.ProductRefId).HasMaxLength(30);

                entity.Property(e => e.ProductRefIdQualifier).HasMaxLength(2);

                entity.HasOne(d => d.IdBfrNavigation)
                    .WithMany(p => p.LearLin)
                    .HasForeignKey(d => d.IdBfr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LEAR_LIN_LEAR_LIN");
            });
        }
    }
}
