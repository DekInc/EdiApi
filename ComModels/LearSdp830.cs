using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class LearSdp830
    {
        public string CalendarPatternCode { get; set; }
        public string PatternTimeCode { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string ParentHashId { get; set; }
    }
}
