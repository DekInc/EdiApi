using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EdiApi
{
    [Browsable(true)]
    public class EdiBase : EdiCommon
    {        
        public string NotUsed { set; get; } = "";        
        //public EdiBase(IEnumerable<string> _Orden) { Orden = _Orden; }
        public EdiBase(string _SegmentTerminator) : base(_SegmentTerminator) { }        
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
