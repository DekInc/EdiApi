using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class LearEquivalencias
    {
        public string CodProducto { get; set; }
        public string CodProductoLear { get; set; }
        public double? Spq { get; set; }
        public string Unit { get; set; }
        public double? GrossWeightXcartonKg { get; set; }
        public double? CartonsXpallet { get; set; }
        public string CartonSizemm { get; set; }
        public string PalletSizemm { get; set; }
        public double? PalletWeightKg { get; set; }
        public int Id { get; set; }
    }
}
