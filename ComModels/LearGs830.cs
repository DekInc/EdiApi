using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class LearGs830
    {
        public string FunctionalIdCode { get; set; }
        public string ApplicationSenderCode { get; set; }
        public string ApplicationReceiverCode { get; set; }
        public string GsDate { get; set; }
        public string GsTime { get; set; }
        public string GroupControlNumber { get; set; }
        public string ResponsibleAgencyCode { get; set; }
        public string Version { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
