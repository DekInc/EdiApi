using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EdiApi.Models.EdiDB;
namespace EdiApi
{
    //Solo ver 2040
    public class LearRep856
    {
        public List<string> EdiFile { get; set; }
        public static LearIsa856 LearIsa856root { set; get; }
        public GS856 GSO { get; set; } = new GS856(EdiBase.SegmentTerminator);
        public LearRep856()
        {
        }        
    }
}
