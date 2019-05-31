using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
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
    }
}
