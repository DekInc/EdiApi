using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbClient
    {
        public GlbClient()
        {
            GlbClientIntegration = new HashSet<GlbClientIntegration>();
            GlbContact = new HashSet<GlbContact>();
        }

        public long IdClient { get; set; }
        public long? IdVendor { get; set; }
        public long IdEnterprise { get; set; }
        public long IdPaymentCondition { get; set; }
        public long IdPartnerType { get; set; }
        public long IdCorporateGroup { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string BusinessName { get; set; }
        public string BusinessType { get; set; }
        public double PreviousBalance { get; set; }
        public double MonthlyBalance { get; set; }
        public double CurrentBalance { get; set; }
        public string BusinessId { get; set; }
        public string TaxPayerRegNumber { get; set; }
        public int CreditLimit { get; set; }
        public double InterestRate { get; set; }
        public string Idnumber { get; set; }
        public string BenefitType { get; set; }
        public string Comments { get; set; }
        public string Clasification { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public double PendingInterestCharge { get; set; }
        public string WebSite { get; set; }
        public string Email { get; set; }
        public DateTime ContractExpDate { get; set; }
        public bool Consignee { get; set; }
        public bool Corporate { get; set; }
        public bool RegionalClient { get; set; }
        public bool GreatTaxPayer { get; set; }
        public bool? Chargeable { get; set; }
        public bool Contract { get; set; }
        public bool Settled { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbCorporateGroup IdCorporateGroupNavigation { get; set; }
        public GlbPartnerType IdPartnerTypeNavigation { get; set; }
        public GlbPaymentCondition IdPaymentConditionNavigation { get; set; }
        public ICollection<GlbClientIntegration> GlbClientIntegration { get; set; }
        public ICollection<GlbContact> GlbContact { get; set; }
    }
}
