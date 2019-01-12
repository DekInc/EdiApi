using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class GS856 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "GS";
        public string FunctionalIdCode { get; set; }
        public string ApplicationSenderCode { get; set; }
        public string ApplicationReceiverCode { get; set; }
        public string GsDate { get; set; }
        public string GsTime { get; set; }
        public string GroupControlNumber { get; set; }
        public string ResponsibleAgencyCode { get; set; }
        public string Version { get; set; }
        public GSTrailer856 GSTrailerO { get; set; }
        public GS856() : base("") { }
        public GS856(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            GroupControlNumber = _ControlNumber;
            Orden = new string[]{
                "Init",
                "FunctionalIdCode", "ApplicationSenderCode",
                "ApplicationReceiverCode", "GsDate",
                "GsTime", "GroupControlNumber",
                "ResponsibleAgencyCode", "Version"
            };
            switch (_RepType)
            {
                case 0:
                    //ControlNumber = _ControlNumber;
                    GSTrailerO = new GSTrailer856(RepType, SegmentTerminator, _ControlNumber);
                    break;
            }
        }
    }
}
