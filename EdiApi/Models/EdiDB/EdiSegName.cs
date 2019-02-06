using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class EdiSegName
    {
        public string Segment { get; set; }
        public string Eng { get; set; }
        public string Spa { get; set; }
        public string DescEng { get; set; }
        public string DescSpa { get; set; }
    }
}
