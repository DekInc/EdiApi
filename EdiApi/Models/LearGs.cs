using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class LearGs
    {
        public int Id { get; set; }
        public string FunctionalIdCode { get; set; }
        public string ApplicationSenderCode { get; set; }
        public string ApplicationReceiverCode { get; set; }
        public string GsDate { get; set; }
        public string GsTime { get; set; }
        public string GroupControlNumber { get; set; }
        public string ResponsibleAgencyCode { get; set; }
        public string Version { get; set; }
        public int IdIsa { get; set; }

        public LearIsa IdIsaNavigation { get; set; }
    }
}
