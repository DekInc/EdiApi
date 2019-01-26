using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class FE830Data
    {
        public LearIsa830 ISA { set; get; }
        public IEnumerable<LearSt830> ListSt { set; get; }
        public IEnumerable<LearBfr830> ListStBfr { set; get; }
        public IEnumerable<LearN1830> ListStN1 { set; get; }
        public IEnumerable<LearN4830> ListStN4 { set; get; }
        public IEnumerable<LearNte830> ListStNte { set; get; }
        public IEnumerable<LearLin830> ListStLin { set; get; }
    }
}
