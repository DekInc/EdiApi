using System;
using System.Collections.Generic;

namespace EdiViewer.Models.EdiDB
{
    public partial class LearCodes
    {
        public int Id { get; set; }
        public string Str { get; set; }
        public string Param { get; set; }
        public string Value { get; set; }
        public string ValueEsp { get; set; }
    }
}
