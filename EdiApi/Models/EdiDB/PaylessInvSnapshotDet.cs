using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PaylessInvSnapshotDet
    {
        public int Id { get; set; }
        public int? IdM { get; set; }
        public int? BodegaId { get; set; }
        public int? TiendaId { get; set; }
        public string Tienda { get; set; }
        public int? WomanQty { get; set; }
        public int? ManQty { get; set; }
        public int? KidsQty { get; set; }
        public int? AccQty { get; set; }
        public int? TotalSinCp { get; set; }
        public int? TotalCp { get; set; }
        public int? Total { get; set; }
        public int? TotalSolicitado { get; set; }
        public int? TotalDisponible { get; set; }
        public string Bodega { get; set; }
    }
}
