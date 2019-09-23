using System;
using System.Collections.Generic;

namespace ComModels.Models.EdiDB
{
    public partial class WmsInOut
    {
        public int Id { get; set; }
        public int TransaccionId { get; set; }
        public string NoTransaccion { get; set; }
        public DateTime? FechaTransaccion { get; set; }
        public string IdTipoTransaccion { get; set; }
        public string TipoTransaccion { get; set; }
        public int? PedidoId { get; set; }
        public int? BodegaId { get; set; }
        public string NomBodega { get; set; }
        public int? RegimenId { get; set; }
        public string Regimen { get; set; }
        public int? ClienteId { get; set; }
        public string Nombre { get; set; }
        public string TipoIngreso { get; set; }
        public string Observacion { get; set; }
        public int? EstatusId { get; set; }
        public string Estatus { get; set; }
        public int? CantEnt { get; set; }
        public int? CantSal { get; set; }
        public string InformeAlmacen { get; set; }
    }
}
