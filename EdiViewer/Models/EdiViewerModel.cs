using ComModels.Models.EdiDB;
using System.Collections.Generic;

namespace EdiViewer.Models {
    public class EdiViewerModel
    {
        public string EdiPureHashId { get; set; }
        public IEnumerable<LearPureEdi> ListEdiPure { get; set; }
    }
}
