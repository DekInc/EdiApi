using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class PedidosxSalida
    {
        public int IdpedidoxSalida { get; set; }
        public int? PedidoId { get; set; }
        public int? SalidaId { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
