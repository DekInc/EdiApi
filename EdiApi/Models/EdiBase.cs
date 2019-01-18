using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EdiApi
{
    [Browsable(true)]
    public class EdiBase
    {
        public int Hash { set; get; }
        public int Coli { set; get; }
        public string NotUsed { set; get; } = "";
        public string EdiStr { set; get; } = "";
        public string[] EdiArray { set; get; }
        public static string SegmentTerminator { get; set; } = "~";
        public static string ElementTerminator { get; set; } = "*";
        public string CompositeTerminator { get; set; } = ">";
        public IEnumerable<string> Orden { set; get; }
        public EdiBase Parent { get; set; }
        public List<EdiBase> Childs { get; set; } = new List<EdiBase>();
        public EdiBase(string _SegmentTerminator)
        {
            SegmentTerminator = _SegmentTerminator;
        }
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
                Coli = 0;
                Hash = GetHashCode();
                EdiStr = _EdiStr;
                EdiArray = EdiStr.Replace(SegmentTerminator, "").Split(ElementTerminator);
                //if (Orden.Count() != EdiArray.Length)
                //    return false;
                foreach (string OrdenO in Orden)
                {
                    if (OrdenO != "Init")
                    {
                        Coli++;
                        if (Coli == EdiArray.Length)
                            break;
                        this.GetType().GetProperty(OrdenO).SetValue(this, EdiArray[Coli]);
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
        public string Validate()
        {
            string Ret = "";
            List<ValidationResult> LVal = new List<ValidationResult>();
            ValidationContext Context = new ValidationContext(this, null, null);
            Validator.TryValidateObject(this, Context, LVal, true);
            if (LVal.Count > 0)
            {
                Ret = string.Join(',', LVal);
            }
            return Ret;
        }        
    }
}
