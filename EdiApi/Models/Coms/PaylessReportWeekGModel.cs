using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public class PaylessReportWeekGModel {
        public int TiendaId { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public string Tel { get; set; }
        public int TotalBox { get; set; }
        public int Accessories { get; set; }
        public int Kids { get; set; }
        public int Man { get; set; }
        public int Ladies { get; set; }
        public string Date1 { get; set; }        
        public string Time1 { get; set; }
        public int Total1 { get; set; }
        public string Date2 { get; set; }
        public string Time2 { get; set; }
        public int Total2 { get; set; }
        public string Date3 { get; set; }
        public string Time3 { get; set; }
        public int Total3 { get; set; }
    }
}
