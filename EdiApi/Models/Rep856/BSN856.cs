using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    /// <summary>
    /// To transmit identifying numbers, dates and other
    /// basic data relating to the transaction set.    /// The date and time are the local date and time at the
    /// creation point of the transaction set.
    /// </summary>
    public class BSN856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public string Init { get; set; } = "BSN";
        /// <summary>
        /// “00” = Original
        /// “01” = Cancellation
        /// “12” = Test data
        /// </summary>
        [StringLength(maximumLength: 2, MinimumLength = 2)]
        public string TransactionSetPurposeCode { get; set; }
        /// <summary>
        /// A unique supplier assigned Shipment Identification (SID) number
        /// that is not repeated within a one year period.
        /// </summary>
        [StringLength(maximumLength: 16, MinimumLength = 2)]
        public string ShipIdentification { get; set; }
        /// <summary>
        /// Local ASN Creation Date (YYMMDD)
        /// </summary>
        [StringLength(maximumLength: 6, MinimumLength = 6)]
        public string BsnDate { get; set; }
        /// <summary>
        /// Local ASN Creation Time (HHMM) 24 hour clock.
        /// </summary>
        [StringLength(maximumLength: 4, MinimumLength = 4)]
        public string BsnTime { get; set; }
        public BSN856(string _SegmentTerminator) : base(_SegmentTerminator)
        {
            Orden = new string[]{
                "Init",
                "TransactionSetPurposeCode", "ShipIdentification",
                "BsnDate", "BsnTime"
            };
        }
    }
}
