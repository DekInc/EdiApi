using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public class SpGeneraSalidaWMSModel {
        public string CodProducto { get; set; }
        public string InventarioID { get; set; }
        public string ItemInventarioID { get; set; }
        public string Precio { get; set; }
        public string Lote { get; set; }
        public string Rack { get; set; }
    }
}
