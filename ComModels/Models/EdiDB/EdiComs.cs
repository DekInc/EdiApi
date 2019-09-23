using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class EdiComs
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Log { get; set; }
        public string Freg { get; set; }
    }
}
