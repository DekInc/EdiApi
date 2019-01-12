using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class EdiBase : EdiCommon
    {
        public string EdiStr { set; get; } = "";
        public string NotUsed { set; get; } = "";
        public IEnumerable<string> Orden { set; get; }
        //public EdiBase(IEnumerable<string> _Orden) { Orden = _Orden; }
        public EdiBase(string _SegmentTerminator) : base(_SegmentTerminator) { }
        public string Ts()
        {
            return base.Ts(this, Orden);
        }        
    }
}
