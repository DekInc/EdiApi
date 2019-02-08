using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class Pedido
    {
        public Pedido()
        {
            DtllPedido = new HashSet<DtllPedido>();
        }

        public int PedidoId { get; set; }
        public int? ClienteId { get; set; }
        public string Observacion { get; set; }
        public DateTime? Fechapedido { get; set; }
        public string TipoPedido { get; set; }
        public DateTime? FechaRequerido { get; set; }
        public int? EstatusId { get; set; }
        public string PedidoBarcode { get; set; }
        public int? BodegaId { get; set; }
        public int? RegimenId { get; set; }
        public int? Destinoid { get; set; }

        public Clientes Cliente { get; set; }
        public Destconsigna Destino { get; set; }
        public ICollection<DtllPedido> DtllPedido { get; set; }
    }
}
