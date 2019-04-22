using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models
{
    public class GridSearchModel
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public string Operator { get; set; }
        public string Value { get; set; }
    }
}
