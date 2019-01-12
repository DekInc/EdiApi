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
        public ISA856 ISAO { get; set; } = new ISA856();
        public GS856 GSO { get; set; } = new GS856();
        public LearRep856()
        {            
        }
        public void AssignObjs()
        {
            string Ident = "";
            foreach (string EdiStr in EdiFile)
            {
                Ident = EdiStr.IndexOf(EdiCommon.ElementTerminator) > 0 ? EdiStr.Substring(0, EdiStr.IndexOf(EdiCommon.ElementTerminator) - 1) : string.Empty;
                if (Ident != string.Empty)
                {
                    switch (Ident)
                    {
                        case ISA856.Init:
                            ISAO.EdiStr = EdiStr;
                            break;
                        case GS856.Init:
                            GSO.EdiStr = EdiStr;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
