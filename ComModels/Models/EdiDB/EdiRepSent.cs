using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class EdiRepSent
    {
        public string Tipo { get; set; }
        public string Fecha { get; set; }
        public int Id { get; set; }
        public string Log { get; set; }
        public string Code { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public int? CodUsr { get; set; }
    }
}
