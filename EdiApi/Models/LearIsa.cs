using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class LearIsa
    {
        public int Id { get; set; }
        public string InterchangeSenderIdQualifier { get; set; }
        public string InterchangeSenderId { get; set; }
        public string InterchangeReceiverIdQualifier { get; set; }
        public string InterchangeReceiverId { get; set; }
        public string InterchangeControlVersion { get; set; }
        public string UsageIndicator { get; set; }
        public string AuthorizationInformationQualifier { get; set; }
        public string AuthorizationInformation { get; set; }
        public string SecurityInformationQualifier { get; set; }
        public string SecurityInformation { get; set; }
        public string InterchangeControlStandardsId { get; set; }
        public string AcknowledgmentRequested { get; set; }
        public string ComponentElementSeparator { get; set; }
    }
}
