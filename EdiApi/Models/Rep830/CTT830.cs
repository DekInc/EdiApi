using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EdiApi
{
    public class CTT830 : EdiBase
    {
        [StringLength(maximumLength: 10, MinimumLength = 20)]
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

        public bool Validate()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this, null, null);
            return Validator.TryValidateObject(this, context, results, true);
        }
    }
}
