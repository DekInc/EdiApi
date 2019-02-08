using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class Cierreperiodo
    {
        public string Mes { get; set; }
        public string Anio { get; set; }
        public int? Iteminventarioid { get; set; }
        public double? Salant { get; set; }
        public double? Entradas { get; set; }
        public double? Salidas { get; set; }
        public int? Clienteid { get; set; }
        public int? Bodegaid { get; set; }
        public int? Regimenid { get; set; }
        public int? Rackid { get; set; }
        public string Codproducto { get; set; }
        public int? CategoriaId { get; set; }
        public int? LocationId { get; set; }
        public int? Transaccionid { get; set; }
        public DateTime? Fechacierre { get; set; }
        public int CierreId { get; set; }
    }
}
