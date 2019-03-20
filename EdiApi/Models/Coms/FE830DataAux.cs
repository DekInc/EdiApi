using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{
    public class FE830DataAux
    {
        public string CodProducto { get; set; }
        public string CodProductoLear { get; set; }
        public string Producto { get; set; }        
        public double Existencia { get; set; }
        public string UnidadDeMedida { get; set; }
    }
}
