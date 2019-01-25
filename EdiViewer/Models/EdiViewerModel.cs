using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComModels;

namespace EdiViewer.Models
{
    public class EdiViewerModel
    {
        public string EdiPureHashId { get; set; }
        public IEnumerable<LearPureEdi> ListEdiPure { get; set; }
    }
}
