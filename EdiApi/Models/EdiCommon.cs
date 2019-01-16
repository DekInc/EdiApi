using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class EdiCommon
    {
        public Int16 Di { set; get; }
        public string EdiStr { set; get; } = "";
        public string [] EdiArray { set; get; }
        public static string SegmentTerminator { get; set; } = "~";
        public static string ElementTerminator { get; set; } = "*";
        public string CompositeTerminator { get; set; } = ">";
        public IEnumerable<string> Orden { set; get; }
        public EdiCommon(string _SegmentTerminator) { SegmentTerminator = _SegmentTerminator; }
        public string Ts()
        {
            string Ret = string.Empty;
            foreach (string OrdenO in Orden)
                Ret += $"{this.GetType().GetProperty(OrdenO).GetValue(this, null)}{ElementTerminator}";
            Ret = Ret.TrimEnd(ElementTerminator[0]) + SegmentTerminator + Environment.NewLine;
            return Ret;
        }
        public bool Parse(string _EdiStr)
        {
            try
            {
                Di = 0;
                EdiStr = _EdiStr;
                EdiArray = EdiStr.Replace(SegmentTerminator, "").Split(ElementTerminator);
                if (Orden.Count() != EdiArray.Length)
                    return false;
                foreach (string OrdenO in Orden)
                {
                    if (OrdenO != "Init")
                    {
                        Di++;
                        this.GetType().GetProperty(OrdenO).SetValue(this, EdiArray[Di]);
                    }
                    //Ret += $"{this.GetType().GetProperty(OrdenO).GetValue(this, null)}{ElementTerminator}";
                }
                return true;
            }
            catch
            {
                return false;
            }            
        }
    }
}
