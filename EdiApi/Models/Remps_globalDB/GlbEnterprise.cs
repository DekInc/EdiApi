using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbEnterprise
    {
        public GlbEnterprise()
        {
            GlbUserEnterprise = new HashSet<GlbUserEnterprise>();
        }

        public long IdEnterprise { get; set; }
        public long IdCity { get; set; }
        public long IdCurrency { get; set; }
        public long IdCatalogItem { get; set; }
        public long IdCatalogMayorLedger { get; set; }
        public long IdSubAccount1 { get; set; }
        public long IdSubAccount2 { get; set; }
        public long IdSubAccount3 { get; set; }
        public long IdSubAccount4 { get; set; }
        public long IdCrdepartment { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string CatalogType { get; set; }
        public string AccountingType { get; set; }
        public string Signature1 { get; set; }
        public string Position1 { get; set; }
        public string Signature2 { get; set; }
        public string Position2 { get; set; }
        public string Signature3 { get; set; }
        public string Position3 { get; set; }
        public string EnterpriseCode { get; set; }
        public string TaxCode { get; set; }
        public decimal? CapitalValue { get; set; }
        public string ConnectionString { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbCity IdCityNavigation { get; set; }
        public GlbCurrency IdCurrencyNavigation { get; set; }
        public ICollection<GlbUserEnterprise> GlbUserEnterprise { get; set; }
    }
}
