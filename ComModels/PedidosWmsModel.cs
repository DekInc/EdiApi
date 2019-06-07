using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComModels
{
    public class PedidosWmsModel
    {
        public int Recid { get { return PedidoId; } }
        public int ClienteId { get; set; }
        public string PedidoBarcode { get; set; }
        public string FechaPedido { get; set; }
        public string Estatus { get; set; }
        public string NomBodega { get; set; }
        public string Regimen { get; set; }
        public string CodProducto { get; set; }
        public string Cantidad { get; set; }
        public string Observacion { get; set; }
        public int PedidoId { get; set; }
        public int Total { get; set; }
    }
}
