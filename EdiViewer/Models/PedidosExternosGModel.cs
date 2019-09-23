using ComModels.Models.EdiDB;

namespace EdiViewer.Models {
    public class PedidosExternosGModel : PedidosExternos {
        public int Recid { get { return Id; } }
        public bool ChangeState { get; set; }
        public int Cont { get; set; }
        public string FechaBorrado { get; set; }
    }
}
