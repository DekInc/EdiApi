using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class PedidoxOperario
    {
        public int IdpedidoAsigando { get; set; }
        public int? PedidoId { get; set; }
        public int? Operario { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Comentario { get; set; }
        public int? DtllPedidoId { get; set; }
    }
}
