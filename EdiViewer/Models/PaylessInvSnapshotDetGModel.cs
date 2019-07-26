using ComModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models {
    public class PaylessInvSnapshotDetGModel : PaylessInvSnapshotDet {
        public int Recid { get { return Id; } }
    }
}
