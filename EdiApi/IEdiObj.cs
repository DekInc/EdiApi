using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class EdiBase
    {
        public string NotUsed { set; get; } = "";
        public IEnumerable<string> Orden { set; get; }
        //public EdiBase(IEnumerable<string> _Orden) { Orden = _Orden; }
        public string Ts()
        {
            return EdiCommon.Ts(this, Orden);
        }
    }
}
