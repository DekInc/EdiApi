using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComModels;

namespace EdiViewer.Models
{
    public class EdiDetailModel
    {
        public FE830Data Data { set; get; }
        public IEnumerable<EdiDetailQtysModel> EdiDetailQtys { get; set; }
    }
}
