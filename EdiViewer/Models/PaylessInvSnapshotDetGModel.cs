using ComModels.Models.EdiDB;

namespace EdiViewer.Models {
    public class PaylessInvSnapshotDetGModel : PaylessInvSnapshotDet {
        public int Recid { get { return Id; } }
    }
}
