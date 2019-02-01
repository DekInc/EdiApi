using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbUser
    {
        public GlbUser()
        {
            GlbUserEnterprise = new HashSet<GlbUserEnterprise>();
            GlbUserGroup = new HashSet<GlbUserGroup>();
        }

        public long IdUser { get; set; }
        public long IdVendor { get; set; }
        public long IdClient { get; set; }
        public long IdUserType { get; set; }
        public long IdEnterpriseDefault { get; set; }
        public long IdTypeDefault { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Environment { get; set; }
        public string DefaultLanguage { get; set; }
        public string QuotationPrefix { get; set; }
        public bool IsOnline { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }

        public GlbUserType IdUserTypeNavigation { get; set; }
        public ICollection<GlbUserEnterprise> GlbUserEnterprise { get; set; }
        public ICollection<GlbUserGroup> GlbUserGroup { get; set; }
    }
}
