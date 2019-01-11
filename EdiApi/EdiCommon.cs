using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class EdiCommon
    {
        public string SegmentTerminator { get; set; } = "~";
        public string ElementTerminator { get; set; } = "*";
        public string CompositeTerminator { get; set; } = ">";
        public EdiCommon(string _SegmentTerminator) { SegmentTerminator = _SegmentTerminator; }
        public string Ts(object O, IEnumerable<string> Orden)
        {
            string Ret = string.Empty;
            foreach (string OrdenO in Orden)
                Ret += O.GetType().GetProperty(OrdenO).GetValue(O, null) + ElementTerminator;
            Ret = Ret.TrimEnd(ElementTerminator[0]) + SegmentTerminator + Environment.NewLine;
            return Ret;
        }
    }
}
