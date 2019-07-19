using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PaylessEncuestaRepDet
    {
        public int Id { get; set; }
        public int? IdM { get; set; }
        public int? IdQ { get; set; }
        public bool? R { get; set; }
        public string Rv { get; set; }
    }
}
