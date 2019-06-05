using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models {
    public class StoreCatQty {
        public int? Recid { get { return TiendaId; } }
        public int? TiendaId { get; set; }
        public string Tienda { get; set; }
        public int WomanQty { get; set; }
        public int WomanCpQty { get; set; }
        public int ManQty { get; set; }
        public int ManCpQty { get; set; }
        public int KidsQty { get; set; }
        public int KidsCpQty { get; set; }
        public int AccQty { get; set; }
        public int AccCpQty { get; set; }
        public int TotalSCp { get; set; }
        public int TotalCp { get; set; }
        public int Total { get; set; }
        public int Requested { get; set; }
        public int Available { get; set; }
    }
}
