using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class PaylessReportesDet
    {
        public int Id { get; set; }
        public int? IdM { get; set; }
        public int? TiendaId { get; set; }
        public int? TotalWomanQty { get; set; }
        public int? TotalManQty { get; set; }
        public int? TotalKidQty { get; set; }
        public int? TotalAccQty { get; set; }
        public int? Total { get; set; }
        public string Fecha1 { get; set; }
        public int? Cant1 { get; set; }
        public string Fecha2 { get; set; }
        public int? Cant2 { get; set; }
        public string Fecha3 { get; set; }
        public int? Cant3 { get; set; }
        public string Fecha4 { get; set; }
        public int? Cant4 { get; set; }
        public string Fecha5 { get; set; }
        public int? Cant5 { get; set; }
        public string Fecha6 { get; set; }
        public int? Cant6 { get; set; }
        public int? RutaId { get; set; }
    }
}
