using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Usrsystem
    {
        public int Codusr { get; set; }
        public string Nomusr { get; set; }
        public int? Codgrp { get; set; }
        public string Usrpasswd { get; set; }
        public string Idusr { get; set; }
        public int? LocationId { get; set; }
        public int? ClienteId { get; set; }
        public string Lastemplate { get; set; }

        public Grpsystem CodgrpNavigation { get; set; }
    }
}
