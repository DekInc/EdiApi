using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi
{
    //Solo ver 2040
    public class LearRep856
    {
        public List<string> EdiFile { get; set; }
        public ISA856 ISAO { get; set; } = new ISA856(EdiBase.SegmentTerminator);
        public GS856 GSO { get; set; } = new GS856(EdiBase.SegmentTerminator);
        public LearRep856()
        {
        }
        public void Test()
        {
            ST856 s = new ST856(1, "", "");
        }        
    }
}
