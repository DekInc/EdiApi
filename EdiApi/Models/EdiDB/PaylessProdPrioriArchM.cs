using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PaylessProdPrioriArchM
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public int? IdTransporte { get; set; }
        public string CodUsr { get; set; }
        public string InsertDate { get; set; }
        public string UpdateDate { get; set; }
        public double? PorcValidez { get; set; }
        public int? CantExcel { get; set; }
        public int? CantEscaner { get; set; }
        public int? Typ { get; set; }
    }
}
