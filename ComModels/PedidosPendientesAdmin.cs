using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComModels {
    public class PedidosPendientesAdmin {
        public int PedidoId { get; set; }
        public string Bodega { get; set; }
        public int TiendaId { get; set; }
        public string FechaPedido { get; set; }
        public string Periodo { get; set; }
        public string Categoria { get; set; }
        public string CP { get; set; }
        public string Barcode { get; set; }
        public int IdRack { get; set; }
        public string NombreRack { get; set; }
        public string Departamento { get; set; }
        public string Producto { get; set; }
        public string Lote { get; set; }
        public string Talla { get; set; }
        public bool? FullPed { get; set; }
        public bool? Divert { get; set; }
        public int? TiendaIdDestino { get; set; }
    }
}
