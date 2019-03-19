using System;
using System.Collections.Generic;

namespace EdiViewer.Models.EdiDB
{
    public partial class LearIsa856
    {
        public string AuthorizationInformationQualifier { get; set; }
        public string AuthorizationInformation { get; set; }
        public string SecurityInformationQualifier { get; set; }
        public string SecurityInformation { get; set; }
        public string InterchangeSenderIdQualifier { get; set; }
        public string InterchangeSenderId { get; set; }
        public string InterchangeReceiverIdQualifier { get; set; }
        public string InterchangeReceiverId { get; set; }
        public string InterchangeDate { get; set; }
        public string InterchangeTime { get; set; }
        public string InterchangeControlStandardsId { get; set; }
        public string InterchangeControlVersion { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string AcknowledgmentRequested { get; set; }
        public string UsageIndicator { get; set; }
        public string ComponentElementSeparator { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
