using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class AsyncStates
    {
        public int Id { get; set; }
        public int? Typ { get; set; }
        public int? Val { get; set; }
        public int? Maximum { get; set; }
        public string Mess { get; set; }
    }
}
