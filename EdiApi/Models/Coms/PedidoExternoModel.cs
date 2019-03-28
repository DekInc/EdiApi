using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class PedidoExternoModel
    {
        public int? id { set; get; }
        public string codProducto { set; get; }
        public string producto { set; get; }
        public float existencia { set; get; }
        public string cantPedir { set; get; }
        public string dateProm { set; get; }
    }
}
