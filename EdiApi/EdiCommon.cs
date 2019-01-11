using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public static class EdiCommon
    {
        public static string SegmentTerminator { get; set; } = "~";
        public static string ElementTerminator { get; set; } = "*";
        public static string CompositeTerminator { get; set; } = ">";
        public static string Ts(object O, IEnumerable<string> Orden)
        {
            string Ret = string.Empty;
            foreach (string OrdenO in Orden)
                Ret += O.GetType().GetProperty(OrdenO).GetValue(O, null) + EdiCommon.ElementTerminator;
            Ret = Ret.TrimEnd(EdiCommon.ElementTerminator[0]) + EdiCommon.SegmentTerminator + Environment.NewLine;
            return Ret;
        }
    }
}
