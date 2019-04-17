using System;
using System.Collections.Generic;
using System.Text;

namespace EdiApi.Models
{
    public class PaylessProdPrioriArchMModel
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public int? ClienteId { get; set; }
        public string ClientName { get; set; }
        public double PorValid { get; set; }
        public string InsertDate { get; set; }
        public string UpdateDate { get; set; }
    }
}
