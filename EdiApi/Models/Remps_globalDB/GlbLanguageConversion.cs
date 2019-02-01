using System;
using System.Collections.Generic;

namespace EdiApi.Models.Remps_globalDB
{
    public partial class GlbLanguageConversion
    {
        public long IdLanguageConversion { get; set; }
        public int Ordinal { get; set; }
        public string Language { get; set; }
        public string Form { get; set; }
        public string Control { get; set; }
        public string Type { get; set; }
        public string Field { get; set; }
        public string Text { get; set; }
    }
}
