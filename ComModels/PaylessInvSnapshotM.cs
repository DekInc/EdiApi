using System;
using System.Collections.Generic;

namespace ComModels {
    public partial class PaylessInvSnapshotM
    {
        public int Id { get; set; }
        public string Periodo { get; set; }
        public string Cp { get; set; }
        public int? TotalWoman { get; set; }
        public int? TotalMan { get; set; }
        public int? TotalKids { get; set; }
        public int? TotalAcc { get; set; }
        public int? TotalSinCp { get; set; }
        public int? TotalCp { get; set; }
        public int? Total { get; set; }
        public int? TotalSolicitado { get; set; }
        public int? TotalDisponible { get; set; }
        public int? ClienteId { get; set; }
    }
}
