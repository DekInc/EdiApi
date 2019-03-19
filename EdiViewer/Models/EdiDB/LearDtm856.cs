using System;
using System.Collections.Generic;

namespace EdiViewer.Models.EdiDB
{
    public partial class LearDtm856
    {
        public string DateTimeQualifier { get; set; }
        public string DtmDate { get; set; }
        public string DtmTime { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
