using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class Locations
    {
        public int Locationid { get; set; }
        public string Dsclocation { get; set; }
        public string Direccion { get; set; }
        public string EmailSender { get; set; }
        public string EmailDomain { get; set; }
        public string EmailPasswd { get; set; }
        public string EmailPort { get; set; }
        public string Mesfin { get; set; }
        public string Yearfin { get; set; }
        public string Perfrec { get; set; }
        public int? IdinvShow { get; set; }
        public int? Paisid { get; set; }
    }
}
