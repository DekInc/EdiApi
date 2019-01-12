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

        public virtual DbSet<LearGs> LearGs { get; set; }
        public virtual DbSet<LearIsa> LearIsa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
