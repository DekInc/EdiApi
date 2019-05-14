using EdiApi.Models.EdiDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdiApi.Models
{
    public partial class PaylessProdPrioriDetModel : PaylessProdPrioriDet
    {
        public int recid { get { return Id; } }
        public int Existencia { get; set; }
        public int Reservado { get; set; }
        public int Disponible { get { return Existencia - Reservado; } }
        public int CantPedir { get; set; }
        public string dateProm { set; get; }
        public string Transporte { set; get; }
    }
}
