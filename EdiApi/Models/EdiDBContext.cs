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

        public virtual DbSet<LearIsa> LearIsa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=EdiDB;Trusted_Connection=True;");
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.InterchangeReceiverId).HasMaxLength(16);

                entity.Property(e => e.InterchangeReceiverIdQualifier).HasMaxLength(4);

                entity.Property(e => e.InterchangeSenderId).HasMaxLength(16);

                entity.Property(e => e.InterchangeSenderIdQualifier).HasMaxLength(4);

                entity.Property(e => e.SecurityInformation).HasMaxLength(16);

                entity.Property(e => e.SecurityInformationQualifier).HasMaxLength(4);

                entity.Property(e => e.UsageIndicator)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });
        }
    }
}
