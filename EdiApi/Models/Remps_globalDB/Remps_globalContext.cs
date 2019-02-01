using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class Remps_globalContext : DbContext
    {
        public Remps_globalContext()
        {
        }

        public Remps_globalContext(DbContextOptions<Remps_globalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GlbAddress> GlbAddress { get; set; }
        public virtual DbSet<GlbAddressType> GlbAddressType { get; set; }
        public virtual DbSet<GlbAgent> GlbAgent { get; set; }
        public virtual DbSet<GlbArea> GlbArea { get; set; }
        public virtual DbSet<GlbCarrier> GlbCarrier { get; set; }
        public virtual DbSet<GlbCatalog> GlbCatalog { get; set; }
        public virtual DbSet<GlbCity> GlbCity { get; set; }
        public virtual DbSet<GlbCityTerminal> GlbCityTerminal { get; set; }
        public virtual DbSet<GlbClient> GlbClient { get; set; }
        public virtual DbSet<GlbClientIntegration> GlbClientIntegration { get; set; }
        public virtual DbSet<GlbCompanyTransport> GlbCompanyTransport { get; set; }
        public virtual DbSet<GlbContact> GlbContact { get; set; }
        public virtual DbSet<GlbContactType> GlbContactType { get; set; }
        public virtual DbSet<GlbContainer> GlbContainer { get; set; }
        public virtual DbSet<GlbCorporateGroup> GlbCorporateGroup { get; set; }
        public virtual DbSet<GlbCostCenter> GlbCostCenter { get; set; }
        public virtual DbSet<GlbCountry> GlbCountry { get; set; }
        public virtual DbSet<GlbCurrency> GlbCurrency { get; set; }
        public virtual DbSet<GlbDocument> GlbDocument { get; set; }
        public virtual DbSet<GlbDocumentType> GlbDocumentType { get; set; }
        public virtual DbSet<GlbEnterprise> GlbEnterprise { get; set; }
        public virtual DbSet<GlbGroupAcc> GlbGroupAcc { get; set; }
        public virtual DbSet<GlbGroupNotification> GlbGroupNotification { get; set; }
        public virtual DbSet<GlbIncoterm> GlbIncoterm { get; set; }
        public virtual DbSet<GlbInsurance> GlbInsurance { get; set; }
        public virtual DbSet<GlbLanguageConversion> GlbLanguageConversion { get; set; }
        public virtual DbSet<GlbLoadType> GlbLoadType { get; set; }
        public virtual DbSet<GlbLocation> GlbLocation { get; set; }
        public virtual DbSet<GlbMediaNotification> GlbMediaNotification { get; set; }
        public virtual DbSet<GlbNotiCenter> GlbNotiCenter { get; set; }
        public virtual DbSet<GlbPackaging> GlbPackaging { get; set; }
        public virtual DbSet<GlbPartnerAddress> GlbPartnerAddress { get; set; }
        public virtual DbSet<GlbPartnerTax> GlbPartnerTax { get; set; }
        public virtual DbSet<GlbPartnerType> GlbPartnerType { get; set; }
        public virtual DbSet<GlbPartnerTypeDoc> GlbPartnerTypeDoc { get; set; }
        public virtual DbSet<GlbPaymentCondition> GlbPaymentCondition { get; set; }
        public virtual DbSet<GlbPricing> GlbPricing { get; set; }
        public virtual DbSet<GlbPricingDetail> GlbPricingDetail { get; set; }
        public virtual DbSet<GlbPricingDetailProductProperty> GlbPricingDetailProductProperty { get; set; }
        public virtual DbSet<GlbProduct> GlbProduct { get; set; }
        public virtual DbSet<GlbProductClientSubGroupAcc> GlbProductClientSubGroupAcc { get; set; }
        public virtual DbSet<GlbRange> GlbRange { get; set; }
        public virtual DbSet<GlbRegion> GlbRegion { get; set; }
        public virtual DbSet<GlbSegment> GlbSegment { get; set; }
        public virtual DbSet<GlbServiceLevel> GlbServiceLevel { get; set; }
        public virtual DbSet<GlbSharedDocument> GlbSharedDocument { get; set; }
        public virtual DbSet<GlbShipmentType> GlbShipmentType { get; set; }
        public virtual DbSet<GlbSignature> GlbSignature { get; set; }
        public virtual DbSet<GlbSubGroupAcc> GlbSubGroupAcc { get; set; }
        public virtual DbSet<GlbSystemModule> GlbSystemModule { get; set; }
        public virtual DbSet<GlbTax> GlbTax { get; set; }
        public virtual DbSet<GlbTerminal> GlbTerminal { get; set; }
        public virtual DbSet<GlbTermsConditions> GlbTermsConditions { get; set; }
        public virtual DbSet<GlbTrackingNotification> GlbTrackingNotification { get; set; }
        public virtual DbSet<GlbType> GlbType { get; set; }
        public virtual DbSet<GlbUnit> GlbUnit { get; set; }
        public virtual DbSet<GlbUnitMeasure> GlbUnitMeasure { get; set; }
        public virtual DbSet<GlbUnitMeasureConversion> GlbUnitMeasureConversion { get; set; }
        public virtual DbSet<GlbUser> GlbUser { get; set; }
        public virtual DbSet<GlbUserEnterprise> GlbUserEnterprise { get; set; }
        public virtual DbSet<GlbUserGroup> GlbUserGroup { get; set; }
        public virtual DbSet<GlbUserType> GlbUserType { get; set; }
        public virtual DbSet<GlbWarehouse> GlbWarehouse { get; set; }

        // Unable to generate entity type for table 'dbo.GLB.EstadoResultado'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GlbAddress>(entity =>
            {
                entity.HasKey(e => e.IdAddress);

                entity.ToTable("GLB.Address");

                entity.Property(e => e.IdAddressType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCity).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCountry).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdRegion).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Line1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Line2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Line3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Phone1).HasMaxLength(20);

                entity.Property(e => e.Phone2).HasMaxLength(20);

                entity.Property(e => e.Phone3).HasMaxLength(20);

                entity.Property(e => e.ZipOrPostcode).HasMaxLength(10);
            });

            modelBuilder.Entity<GlbAddressType>(entity =>
            {
                entity.HasKey(e => e.IdAddressType);

                entity.ToTable("GLB.AddressType");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbAgent>(entity =>
            {
                entity.HasKey(e => e.IdAgent);

                entity.ToTable("GLB.Agent");

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.IdPartnerTypeNavigation)
                    .WithMany(p => p.GlbAgent)
                    .HasForeignKey(d => d.IdPartnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Agent_GLB.PartnerType");
            });

            modelBuilder.Entity<GlbArea>(entity =>
            {
                entity.HasKey(e => e.IdArea);

                entity.ToTable("GLB.Area");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbCarrier>(entity =>
            {
                entity.HasKey(e => e.IdCarrier);

                entity.ToTable("GLB.Carrier");

                entity.Property(e => e.CarrierAgent).HasMaxLength(150);

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdType).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdPartnerTypeNavigation)
                    .WithMany(p => p.GlbCarrier)
                    .HasForeignKey(d => d.IdPartnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Carrier_GLB.PartnerType");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.GlbCarrier)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Carrier_GLB.Type");
            });

            modelBuilder.Entity<GlbCatalog>(entity =>
            {
                entity.HasKey(e => e.IdCatalog);

                entity.ToTable("GLB.Catalog");

                entity.Property(e => e.AccType).HasDefaultValueSql("((1))");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.CatalogLevel).HasDefaultValueSql("((1))");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.IdCatalogParent).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbCity>(entity =>
            {
                entity.HasKey(e => e.IdCity);

                entity.ToTable("GLB.City");

                entity.Property(e => e.Code)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdCountryNavigation)
                    .WithMany(p => p.GlbCity)
                    .HasForeignKey(d => d.IdCountry)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.City_GLB.Country");
            });

            modelBuilder.Entity<GlbCityTerminal>(entity =>
            {
                entity.HasKey(e => e.IdCityTerminal);

                entity.ToTable("GLB.CityTerminal");

                entity.Property(e => e.IdCity).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdTerminal).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdCityNavigation)
                    .WithMany(p => p.GlbCityTerminal)
                    .HasForeignKey(d => d.IdCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.CityTerminal_GLB.City");

                entity.HasOne(d => d.IdTerminalNavigation)
                    .WithMany(p => p.GlbCityTerminal)
                    .HasForeignKey(d => d.IdTerminal)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.CityTerminal_GLB.Terminal");
            });

            modelBuilder.Entity<GlbClient>(entity =>
            {
                entity.HasKey(e => e.IdClient);

                entity.ToTable("GLB.Client");

                entity.Property(e => e.BenefitType)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessId)
                    .HasColumnName("BusinessID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessName)
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessType)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Chargeable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Clasification)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.ContractExpDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdEnterprise).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdPaymentCondition).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdVendor).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Idnumber)
                    .HasColumnName("IDNumber")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastPaymentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.TaxPayerRegNumber).HasMaxLength(10);

                entity.Property(e => e.WebSite)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCorporateGroupNavigation)
                    .WithMany(p => p.GlbClient)
                    .HasForeignKey(d => d.IdCorporateGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Client_GLB.CorporateGroup");

                entity.HasOne(d => d.IdPartnerTypeNavigation)
                    .WithMany(p => p.GlbClient)
                    .HasForeignKey(d => d.IdPartnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Client_GLB.PartnerType");

                entity.HasOne(d => d.IdPaymentConditionNavigation)
                    .WithMany(p => p.GlbClient)
                    .HasForeignKey(d => d.IdPaymentCondition)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Client_GLB.PaymentCondition");
            });

            modelBuilder.Entity<GlbClientIntegration>(entity =>
            {
                entity.HasKey(e => e.IdClientConsignee);

                entity.ToTable("GLB.ClientIntegration");

                entity.Property(e => e.Dborigin)
                    .IsRequired()
                    .HasColumnName("DBOrigin")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSap)
                    .HasColumnName("IdSAP")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdWms)
                    .HasColumnName("IdWMS")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActiveSap)
                    .IsRequired()
                    .HasColumnName("IsActiveSAP")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsActiveWms)
                    .IsRequired()
                    .HasColumnName("IsActiveWMS")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.GlbClientIntegration)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.ClientConsignee_GLB.Client");
            });

            modelBuilder.Entity<GlbCompanyTransport>(entity =>
            {
                entity.HasKey(e => e.IdCompanyTransport);

                entity.ToTable("GLB.CompanyTransport");

                entity.Property(e => e.IdClient).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.GlbCompanyTransport)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.CompanyTransport_GLB.Type");
            });

            modelBuilder.Entity<GlbContact>(entity =>
            {
                entity.HasKey(e => e.IdContact);

                entity.ToTable("GLB.Contact");

                entity.HasIndex(e => e.IdContact)
                    .HasName("IX_GLB.Contacto");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdContactType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.SkypeName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.GlbContact)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Contact_GLB.Client");

                entity.HasOne(d => d.IdContactTypeNavigation)
                    .WithMany(p => p.GlbContact)
                    .HasForeignKey(d => d.IdContactType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Contact_GLB.ContactType");
            });

            modelBuilder.Entity<GlbContactType>(entity =>
            {
                entity.HasKey(e => e.IdContactType);

                entity.ToTable("GLB.ContactType");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbContainer>(entity =>
            {
                entity.HasKey(e => e.IdContainer);

                entity.ToTable("GLB.Container");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerDesc)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContainerType)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Height).HasDefaultValueSql("((0))");

                entity.Property(e => e.Length).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Size)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Width).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<GlbCorporateGroup>(entity =>
            {
                entity.HasKey(e => e.IdCorporateGroup);

                entity.ToTable("GLB.CorporateGroup");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbCostCenter>(entity =>
            {
                entity.HasKey(e => e.IdCostCenter);

                entity.ToTable("GLB.CostCenter");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbCountry>(entity =>
            {
                entity.HasKey(e => e.IdCountry);

                entity.ToTable("GLB.Country");

                entity.Property(e => e.Abbrv).HasMaxLength(3);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRegionNavigation)
                    .WithMany(p => p.GlbCountry)
                    .HasForeignKey(d => d.IdRegion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Country_GLB.Region");
            });

            modelBuilder.Entity<GlbCurrency>(entity =>
            {
                entity.HasKey(e => e.IdCurrency);

                entity.ToTable("GLB.Currency");

                entity.Property(e => e.Code).HasMaxLength(3);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LstOpe)
                    .HasColumnName("lst_ope")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.SerSta).HasColumnName("ser_sta");
            });

            modelBuilder.Entity<GlbDocument>(entity =>
            {
                entity.HasKey(e => e.IdDocument);

                entity.ToTable("GLB.Document");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdDocumentType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdDocumentTypeNavigation)
                    .WithMany(p => p.GlbDocument)
                    .HasForeignKey(d => d.IdDocumentType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Document_GLB.DocumentType");
            });

            modelBuilder.Entity<GlbDocumentType>(entity =>
            {
                entity.HasKey(e => e.IdDocumentType);

                entity.ToTable("GLB.DocumentType");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbEnterprise>(entity =>
            {
                entity.HasKey(e => e.IdEnterprise);

                entity.ToTable("GLB.Enterprise");

                entity.Property(e => e.AccountingType)
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.CapitalValue).HasColumnType("numeric(12, 2)");

                entity.Property(e => e.CatalogType)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.EnterpriseCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.IdCatalogItem).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCatalogMayorLedger).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCity).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCrdepartment)
                    .HasColumnName("IdCRDepartment")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCurrency).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSubAccount1).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSubAccount2).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSubAccount3).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSubAccount4).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Month)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Position1)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Position2)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Position3)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Signature1)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Signature2)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Signature3)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Year)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCityNavigation)
                    .WithMany(p => p.GlbEnterprise)
                    .HasForeignKey(d => d.IdCity)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Enterprise_GLB.City");

                entity.HasOne(d => d.IdCurrencyNavigation)
                    .WithMany(p => p.GlbEnterprise)
                    .HasForeignKey(d => d.IdCurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Enterprise_GLB.Currency");
            });

            modelBuilder.Entity<GlbGroupAcc>(entity =>
            {
                entity.HasKey(e => e.IdGroupAcc);

                entity.ToTable("GLB.GroupAcc");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbGroupNotification>(entity =>
            {
                entity.HasKey(e => e.IdNotificationGroup);

                entity.ToTable("GLB.GroupNotification");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbIncoterm>(entity =>
            {
                entity.HasKey(e => e.IdIncoterm);

                entity.ToTable("GLB.Incoterm");

                entity.Property(e => e.Code)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(35)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbInsurance>(entity =>
            {
                entity.HasKey(e => e.IdInsurance);

                entity.ToTable("GLB.Insurance");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbLanguageConversion>(entity =>
            {
                entity.HasKey(e => new { e.IdLanguageConversion, e.Language, e.Form, e.Control, e.Type, e.Field });

                entity.ToTable("GLB.LanguageConversion");

                entity.Property(e => e.IdLanguageConversion).ValueGeneratedOnAdd();

                entity.Property(e => e.Language).HasMaxLength(3);

                entity.Property(e => e.Form).HasMaxLength(100);

                entity.Property(e => e.Control).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.Field).HasMaxLength(50);

                entity.Property(e => e.Text).HasMaxLength(50);
            });

            modelBuilder.Entity<GlbLoadType>(entity =>
            {
                entity.HasKey(e => e.IdLoadType);

                entity.ToTable("GLB.LoadType");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GlbLocation>(entity =>
            {
                entity.HasKey(e => e.IdLocation);

                entity.ToTable("GLB.Location");

                entity.Property(e => e.Description)
                    .HasMaxLength(75)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbMediaNotification>(entity =>
            {
                entity.HasKey(e => e.IdMediaNotification);

                entity.ToTable("GLB.MediaNotification");

                entity.Property(e => e.MediaName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbNotiCenter>(entity =>
            {
                entity.HasKey(e => e.IdNoticenter);

                entity.ToTable("GLB.NotiCenter");

                entity.Property(e => e.SentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.HasOne(d => d.IdMediaNotificationNavigation)
                    .WithMany(p => p.GlbNotiCenter)
                    .HasForeignKey(d => d.IdMediaNotification)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.NotiCenter_GLB.MediaNotification");

                entity.HasOne(d => d.IdTrackingNotificationNavigation)
                    .WithMany(p => p.GlbNotiCenter)
                    .HasForeignKey(d => d.IdTrackingNotification)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.NotiCenter_GLB.TrackingNotification");
            });

            modelBuilder.Entity<GlbPackaging>(entity =>
            {
                entity.HasKey(e => e.IdPackaging);

                entity.ToTable("GLB.Packaging");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbPartnerAddress>(entity =>
            {
                entity.HasKey(e => e.IdPartnerAddress);

                entity.ToTable("GLB.PartnerAddress");

                entity.Property(e => e.IdAddressType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdAddressNavigation)
                    .WithMany(p => p.GlbPartnerAddress)
                    .HasForeignKey(d => d.IdAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerAddress_GLB.Address");

                entity.HasOne(d => d.IdAddressTypeNavigation)
                    .WithMany(p => p.GlbPartnerAddress)
                    .HasForeignKey(d => d.IdAddressType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerAddress_GLB.AddressType");

                entity.HasOne(d => d.IdPartnerTypeNavigation)
                    .WithMany(p => p.GlbPartnerAddress)
                    .HasForeignKey(d => d.IdPartnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerAddress_GLB.PartnerType");
            });

            modelBuilder.Entity<GlbPartnerTax>(entity =>
            {
                entity.HasKey(e => e.IdPartnerTax);

                entity.ToTable("GLB.PartnerTax");

                entity.Property(e => e.IdPartnerTax).ValueGeneratedNever();

                entity.Property(e => e.IdPartner).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdTax).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdPartnerTypeNavigation)
                    .WithMany(p => p.GlbPartnerTax)
                    .HasForeignKey(d => d.IdPartnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerTax_GLB.PartnerType");

                entity.HasOne(d => d.IdTaxNavigation)
                    .WithMany(p => p.GlbPartnerTax)
                    .HasForeignKey(d => d.IdTax)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerTax_GLB.Tax");
            });

            modelBuilder.Entity<GlbPartnerType>(entity =>
            {
                entity.HasKey(e => e.IdPartnerType);

                entity.ToTable("GLB.PartnerType");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbPartnerTypeDoc>(entity =>
            {
                entity.HasKey(e => e.IdPartnerTypeDoc);

                entity.ToTable("GLB.PartnerTypeDoc");

                entity.Property(e => e.IdDocument).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdPartnerType).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Required).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdDocumentNavigation)
                    .WithMany(p => p.GlbPartnerTypeDoc)
                    .HasForeignKey(d => d.IdDocument)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerTypeDoc_GLB.Document");

                entity.HasOne(d => d.IdPartnerTypeNavigation)
                    .WithMany(p => p.GlbPartnerTypeDoc)
                    .HasForeignKey(d => d.IdPartnerType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PartnerTypeDoc_GLB.PartnerType");
            });

            modelBuilder.Entity<GlbPaymentCondition>(entity =>
            {
                entity.HasKey(e => e.IdPaymentCondition);

                entity.ToTable("GLB.PaymentCondition");

                entity.Property(e => e.Condition)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreditDays).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GlbPricing>(entity =>
            {
                entity.HasKey(e => e.IdPricing);

                entity.ToTable("GLB.Pricing");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.IdCityDestination).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCityOrigin).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCompanyTransport).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdContainer).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCurrency).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdIncoterm).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdServiceLevel).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdTerminalDestination).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdTerminalOrigin).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.PricingType).HasDefaultValueSql("((1))");

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.TradingType).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.GlbPricing)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Pricing_GLB.Type");
            });

            modelBuilder.Entity<GlbPricingDetail>(entity =>
            {
                entity.HasKey(e => e.IdPricingDetail);

                entity.ToTable("GLB.PricingDetail");

                entity.Property(e => e.IdPricing).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdProduct).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdUnitMeasure).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdPricingNavigation)
                    .WithMany(p => p.GlbPricingDetail)
                    .HasForeignKey(d => d.IdPricing)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PricingDetail_GLB.Pricing");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.GlbPricingDetail)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PricingDetail_GLB.Product");
            });

            modelBuilder.Entity<GlbPricingDetailProductProperty>(entity =>
            {
                entity.HasKey(e => e.IdPricingDetailProductProperty);

                entity.ToTable("GLB.PricingDetailProductProperty");

                entity.Property(e => e.IdPricingDetail).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdProduct).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdPricingDetailNavigation)
                    .WithMany(p => p.GlbPricingDetailProductProperty)
                    .HasForeignKey(d => d.IdPricingDetail)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PricingDetailProductProperty_GLB.PricingDetail");

                entity.HasOne(d => d.IdRangeNavigation)
                    .WithMany(p => p.GlbPricingDetailProductProperty)
                    .HasForeignKey(d => d.IdRange)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.PricingDetailProductProperty_GLB.Range");
            });

            modelBuilder.Entity<GlbProduct>(entity =>
            {
                entity.HasKey(e => e.IdProduct);

                entity.ToTable("GLB.Product");

                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.EnglishDescription)
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.FixedAsset).HasDefaultValueSql("((0))");

                entity.Property(e => e.IdCatalog).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSegment).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdUpdateMethod).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IventoryItem).HasDefaultValueSql("((0))");

                entity.Property(e => e.Modificado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModificadoPor).HasDefaultValueSql("((-1))");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseItem).HasDefaultValueSql("((0))");

                entity.Property(e => e.SaleItem).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdLocationNavigation)
                    .WithMany(p => p.GlbProduct)
                    .HasForeignKey(d => d.IdLocation)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Producto_GLB.Ubicacion");

                entity.HasOne(d => d.IdSegmentNavigation)
                    .WithMany(p => p.GlbProduct)
                    .HasForeignKey(d => d.IdSegment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Producto_GLB.Segmento");

                entity.HasOne(d => d.IdUnitNavigation)
                    .WithMany(p => p.GlbProduct)
                    .HasForeignKey(d => d.IdUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Product_GLB.Unit");

                entity.HasOne(d => d.IdWarehouseNavigation)
                    .WithMany(p => p.GlbProduct)
                    .HasForeignKey(d => d.IdWarehouse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Producto_GLB.Almacen");
            });

            modelBuilder.Entity<GlbProductClientSubGroupAcc>(entity =>
            {
                entity.HasKey(e => e.IdProductClinetSubGroupAcc);

                entity.ToTable("GLB.ProductClientSubGroupAcc");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdCostCenterNavigation)
                    .WithMany(p => p.GlbProductClientSubGroupAcc)
                    .HasForeignKey(d => d.IdCostCenter)
                    .HasConstraintName("FK_GLB.ProductClientSubGroupAcc_GLB.CostCenter");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.GlbProductClientSubGroupAcc)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.ProductClientSubGroupAcc_GLB.Product");

                entity.HasOne(d => d.IdSubGroupAccNavigation)
                    .WithMany(p => p.GlbProductClientSubGroupAcc)
                    .HasForeignKey(d => d.IdSubGroupAcc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.ProductClientSubGroupAcc_GLB.SubGroupAcc");
            });

            modelBuilder.Entity<GlbRange>(entity =>
            {
                entity.HasKey(e => e.IdRange);

                entity.ToTable("GLB.Range");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.ValueType).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<GlbRegion>(entity =>
            {
                entity.HasKey(e => e.IdRegion);

                entity.ToTable("GLB.Region");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbSegment>(entity =>
            {
                entity.HasKey(e => e.IdSegmento);

                entity.ToTable("GLB.Segment");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IdCatalogo1).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCatalogo2).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCatalogo3).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdCatalogo4).HasDefaultValueSql("((-1))");
            });

            modelBuilder.Entity<GlbServiceLevel>(entity =>
            {
                entity.HasKey(e => e.IdServiceLevel);

                entity.ToTable("GLB.ServiceLevel");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbSharedDocument>(entity =>
            {
                entity.HasKey(e => e.IdSharedDocument);

                entity.ToTable("GLB.SharedDocument");

                entity.Property(e => e.IdClient).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdDocument).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdEmployee).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdRcontrol)
                    .HasColumnName("IdRControl")
                    .HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdTransport).HasDefaultValueSql("((-1))");

                entity.Property(e => e.LastModified).HasDefaultValueSql("((-1))");

                entity.Property(e => e.LastModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ObjectUrl).HasColumnName("ObjectURL");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.IdDocumentNavigation)
                    .WithMany(p => p.GlbSharedDocument)
                    .HasForeignKey(d => d.IdDocument)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.SharedDocument_GLB.Document");
            });

            modelBuilder.Entity<GlbShipmentType>(entity =>
            {
                entity.HasKey(e => e.IdShipmentType);

                entity.ToTable("GLB.ShipmentType");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdTypeNavigation)
                    .WithMany(p => p.GlbShipmentType)
                    .HasForeignKey(d => d.IdType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.ShipmentType_GLB.Type");
            });

            modelBuilder.Entity<GlbSignature>(entity =>
            {
                entity.HasKey(e => e.IdSignature);

                entity.ToTable("GLB.Signature");

                entity.Property(e => e.IdPartner).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdPartnerNavigation)
                    .WithMany(p => p.GlbSignature)
                    .HasForeignKey(d => d.IdPartner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.CarrierSignature_GLB.Carrier");
            });

            modelBuilder.Entity<GlbSubGroupAcc>(entity =>
            {
                entity.HasKey(e => e.IdSubGroupAcc);

                entity.ToTable("GLB.SubGroupAcc");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdGroupAccNavigation)
                    .WithMany(p => p.GlbSubGroupAcc)
                    .HasForeignKey(d => d.IdGroupAcc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.SubGroupAcc_GLB.GroupAcc");
            });

            modelBuilder.Entity<GlbSystemModule>(entity =>
            {
                entity.HasKey(e => e.IdSystemModule);

                entity.ToTable("GLB.SystemModule");

                entity.Property(e => e.InstallationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModuleName).HasMaxLength(100);

                entity.Property(e => e.ModuleVersion)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbTax>(entity =>
            {
                entity.HasKey(e => e.IdTax);

                entity.ToTable("GLB.Tax");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IdPurchaseCatalog).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdSalesCatalog).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rate).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<GlbTerminal>(entity =>
            {
                entity.HasKey(e => e.IdTerminal);

                entity.ToTable("GLB.Terminal");

                entity.Property(e => e.Code)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.IdCountry).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbTermsConditions>(entity =>
            {
                entity.HasKey(e => e.IdTermsConditions);

                entity.ToTable("GLB.TermsConditions");

                entity.Property(e => e.Applicable).HasDefaultValueSql("((0))");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.TradingType).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdShipmentTypeNavigation)
                    .WithMany(p => p.GlbTermsConditions)
                    .HasForeignKey(d => d.IdShipmentType)
                    .HasConstraintName("FK_GLB.TermsConditions_GLB.ShipmentType");
            });

            modelBuilder.Entity<GlbTrackingNotification>(entity =>
            {
                entity.HasKey(e => e.IdTrackingNotification);

                entity.ToTable("GLB.TrackingNotification");

                entity.Property(e => e.IdCliente).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modificado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModificadoPor).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdNotificationGroupNavigation)
                    .WithMany(p => p.GlbTrackingNotification)
                    .HasForeignKey(d => d.IdNotificationGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.TrackingNotification_GLB.GroupNotification");
            });

            modelBuilder.Entity<GlbType>(entity =>
            {
                entity.HasKey(e => e.IdType);

                entity.ToTable("GLB.Type");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbUnit>(entity =>
            {
                entity.HasKey(e => e.IdUnit);

                entity.ToTable("GLB.Unit");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Units)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GlbUnitMeasure>(entity =>
            {
                entity.HasKey(e => e.IdUnitMeasure);

                entity.ToTable("GLB.UnitMeasure");

                entity.Property(e => e.ConversionSystem).HasDefaultValueSql("((1))");

                entity.Property(e => e.IdUnit).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdUnitNavigation)
                    .WithMany(p => p.GlbUnitMeasure)
                    .HasForeignKey(d => d.IdUnit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.UnitMeasure_GLB.Unit");
            });

            modelBuilder.Entity<GlbUnitMeasureConversion>(entity =>
            {
                entity.HasKey(e => e.IdUnitMeasureConversion);

                entity.ToTable("GLB.UnitMeasureConversion");

                entity.Property(e => e.IdUnitMeasureFrom).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdUnitMeasureTo).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdUnitMeasureFromNavigation)
                    .WithMany(p => p.GlbUnitMeasureConversion)
                    .HasForeignKey(d => d.IdUnitMeasureFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.UnitMeasureConversion_GLB.UnitMeasure");
            });

            modelBuilder.Entity<GlbUser>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.ToTable("GLB.User");

                entity.Property(e => e.DefaultLanguage)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Environment)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.IdClient).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdEnterpriseDefault).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdTypeDefault).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IdVendor).HasDefaultValueSql("((-1))");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Name)
                    .HasMaxLength(125)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QuotationPrefix)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUserTypeNavigation)
                    .WithMany(p => p.GlbUser)
                    .HasForeignKey(d => d.IdUserType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.Usuario_GLB.TipoUsuario");
            });

            modelBuilder.Entity<GlbUserEnterprise>(entity =>
            {
                entity.HasKey(e => e.IdUserEnterprise);

                entity.ToTable("GLB.UserEnterprise");

                entity.Property(e => e.IdEnterprise).HasDefaultValueSql("((-14))");

                entity.Property(e => e.IdUser).HasDefaultValueSql("((-1))");

                entity.Property(e => e.Modified)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((1900)-(1))-(1))");

                entity.Property(e => e.ModifiedBy).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdEnterpriseNavigation)
                    .WithMany(p => p.GlbUserEnterprise)
                    .HasForeignKey(d => d.IdEnterprise)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.UsuarioEmpresa_GLB.Empresa");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.GlbUserEnterprise)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.UsuarioEmpresa_GLB.Usuario");
            });

            modelBuilder.Entity<GlbUserGroup>(entity =>
            {
                entity.HasKey(e => e.IdUserGroup);

                entity.ToTable("GLB.UserGroup");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Modificado)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('1900-01-01')");

                entity.Property(e => e.ModificadoPor).HasDefaultValueSql("((-1))");

                entity.HasOne(d => d.IdNotificationGroupNavigation)
                    .WithMany(p => p.GlbUserGroup)
                    .HasForeignKey(d => d.IdNotificationGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.UserGroup_GLB.GroupNotification");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.GlbUserGroup)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GLB.UserGroup_GLB.Usuario");
            });

            modelBuilder.Entity<GlbUserType>(entity =>
            {
                entity.HasKey(e => e.IdUserType);

                entity.ToTable("GLB.UserType");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GlbWarehouse>(entity =>
            {
                entity.HasKey(e => e.IdWarehouse);

                entity.ToTable("GLB.Warehouse");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });
        }
    }
}
