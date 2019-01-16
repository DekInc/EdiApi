using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    /// <summary>
    /// One DTM segment in the heading area is mandatory to
    /// provide shipment date and time.Use date and time
    /// shipment leaves supplier's premises.
    /// </summary>
    public class DTM856 : EdiBase
    {
        public string Init { get; set; } = "DTM";
        /// <summary>
        /// 011 = Local date and time shipment leaves supplier’s premises.
        /// </summary>
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string DateTimeQualifier { get; set; }
        /// <summary>
        /// (YYMMDD)
        /// </summary>
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string DtmDate { get; set; }
        /// <summary>
        /// (HHMM) 24 Hour Clock.
        /// </summary>
        [StringLength(maximumLength: 4, MinimumLength = 4)]
        public string DtmTime { get; set; }
        public DTM856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "DateTimeQualifier", "DtmDate", "DtmTime"
            };
        }
    }
}
