using System;
using System.Collections.Generic;
using System.Text;

namespace ComModels
{
    public partial class PaylessProdPrioriDetModel : PaylessProdPrioriDet
    {
        public int recid { get { return Id; } }
        public int Existencia { get; set; }
        public int Reservado { get; set; }
        public int Disponible { get { return Existencia - Reservado; } }
        public int CantPedir { get; set; }
        public new string Oid { get { return string.Empty; } }
    }
}
