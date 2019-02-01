using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class Rep830InfoAux {
        public string HashId { get; set; } = string.Empty;
        public string Dest { get; set; } = string.Empty;
    }
    public class Rep830Info
    {
        public IEnumerable<Rep830InfoAux> From { get; set; }
        public IEnumerable<Rep830InfoAux> To { get; set; }
        public IEnumerable<LearPureEdi> LearPureEdi { get; set; }
    }
}
