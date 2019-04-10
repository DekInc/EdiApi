using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PaylessProdPrioriDet
    {
        public int Id { get; set; }
        public int IdPaylessProdPrioriM { get; set; }
        public double? Oid { get; set; }
        public double? Barcode { get; set; }
        public double? Estado { get; set; }
        public double? Pri { get; set; }
        public double? PoolP { get; set; }
        public double? Producto { get; set; }
        public double? Talla { get; set; }
        public double? Lote { get; set; }
        public string Categoria { get; set; }
        public double? Departamento { get; set; }
        public string Cp { get; set; }
        public string Pickeada { get; set; }
        public string Etiquetada { get; set; }
        public string Preinspeccion { get; set; }
        public string Cargada { get; set; }
        public double? M3 { get; set; }
        public double? Peso { get; set; }
    }
}
