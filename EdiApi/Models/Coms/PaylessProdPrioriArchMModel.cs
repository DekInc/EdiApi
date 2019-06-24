using System;
using System.Collections.Generic;
using System.Text;

namespace EdiApi.Models
{
    public class PaylessProdPrioriArchMModel
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public int? IdTransporte { get; set; }
        public string Transporte { get; set; }
        public string ClientName { get; set; }
        public double? PorValid { get; set; }
        public string InsertDate { get; set; }
        public string UpdateDate { get; set; }
        public int? CantExcel { get; set; }
        public int? CantEscaner { get; set; }
        public int? Typ { get; set; }        
    }
}
