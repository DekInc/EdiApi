using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class Bodegas
    {
        public int BodegaId { get; set; }
        public string NomBodega { get; set; }
        public string Descripcion { get; set; }
        public int? EstatusId { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Locationid { get; set; }
        public string TitRequisicion { get; set; }
        public bool? Nodescargaxcliente { get; set; }
    }
}
