using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class FE830Data
    {
        public LearIsa830 ISA { set; get; }
        public LearGs830 GS { set; get; }
        public IEnumerable<EdiSegName> ListEdiSegName { set; get; }
        public IEnumerable<LearSt830> ListSt { set; get; }
        public IEnumerable<LearBfr830> ListStBfr { set; get; }
        public IEnumerable<LearN1830> ListStN1 { set; get; }
        public IEnumerable<LearN4830> ListStN4 { set; get; }
        public IEnumerable<LearNte830> ListStNte { set; get; }
        public IEnumerable<LearLin830> ListStLin { set; get; }
        public IEnumerable<LearN1830> ListLinN1 { set; get; }
        public IEnumerable<LearN4830> ListLinN4 { set; get; }
        public IEnumerable<LearNte830> ListLinNte { set; get; }
        public IEnumerable<LearUit830> ListLinUit { set; get; }
        public IEnumerable<LearPrs830> ListLinPrs { set; get; }
        public IEnumerable<LearSdp830> ListLinSdp { set; get; }
        public IEnumerable<LearFst830> ListLinFst { set; get; }
        public IEnumerable<LearAth830> ListLinAth { set; get; }
        public IEnumerable<LearShp830> ListLinShp { set; get; }
        public IEnumerable<LearRef830> ListLinRef { set; get; }
        public IEnumerable<LearFst830> ListSdpFst { set; get; }
        public IEnumerable<LearRef830> ListShpRef { set; get; }
        public IEnumerable<LearCodes> ListCodes { set; get; }
    }
}
