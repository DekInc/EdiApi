using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class PaylessReportesMails
    {
        public int Id { get; set; }
        public string MailDir { get; set; }
        public int? Typ { get; set; }
    }
}
