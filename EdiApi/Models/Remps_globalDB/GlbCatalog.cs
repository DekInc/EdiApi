using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbCatalog
    {
        public long IdCatalog { get; set; }
        public long IdCatalogParent { get; set; }
        public string Code { get; set; }
        public string AccountName { get; set; }
        public int AccType { get; set; }
        public bool Budget { get; set; }
        public bool CashFlowRelevant { get; set; }
        public int CatalogLevel { get; set; }
        public bool? IsActive { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
