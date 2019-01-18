using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    public class GS830 : EdiBase
    {
        public UInt16 RepType { get; set; }
        public const string Init = "GS";
        public const string Self = "Functional Group Header";
        public string FunctionalIdCode { get; set; } = "0";
        public string ApplicationSenderCode { get; set; } = "1";
        public string ApplicationReceiverCode { get; set; } = "2";
        public string GsDate { get; set; } = "3";
        public string GsTime { get; set; } = "4";
        public string GroupControlNumber { get; set; } = "5";
        public string ResponsibleAgencyCode { get; set; } = "6";
        public string Version { get; set; } = "7";
        public GE830 GSTrailerO { get; set; }
        public GS830(string _SegmentTerminator) : base(_SegmentTerminator) { InitOrden(); }
        public GS830(UInt16 _RepType, string _SegmentTerminator, string _ControlNumber = "000000001") : base(_SegmentTerminator)
        {
            RepType = _RepType;
            GroupControlNumber = _ControlNumber;
            InitOrden();
            switch (_RepType)
            {
                case 0:
                    //ControlNumber = _ControlNumber;
                    GSTrailerO = new GE830(SegmentTerminator);
                    break;
            }
        }
        private void InitOrden () => Orden = new string[]{
            "Init",
            "FunctionalIdCode", "ApplicationSenderCode",
            "ApplicationReceiverCode", "GsDate",
            "GsTime", "GroupControlNumber",
            "ResponsibleAgencyCode", "Version"
        };
    }
}
