using ComModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models {
    public class PedidosExternosGModel : PedidosExternos {
        public int recid { get { return Id; } }
        public bool ChangeState { get; set; }
        public int Cont { get; set; }
    }
}
