using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EdiApi
{
    public class CTT830 : EdiBase
    {
        public string Init { get; set; } = "NTE";
        public string TotalLineItems { get; set; }        
        public string HashTotal { get; set; }
        public CTT830(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "TotalLineItems", "HashTotal"
            };
        }            
    }
}
