using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComModels
{
    public class PaylessUploadFileModel
    {
        public string Oid { get; set; }
        public string Barcode { get; set; }
        public string Estado { get; set; }
        public string Pri { get; set; }
        public string Poolp { get; set; }
        public string Producto { get; set; }
        public string Talla { get; set; }
        public string Lote { get; set; }
        public string Categoria { get; set; }
        public string Departamento { get; set; }
        public string Cp { get; set; }
        public string Pickeada { get; set; }
        public string Etiquetada { get; set; }
        public string Preinspeccion { get; set; }
        public string Cargada { get; set; }
        public double M3 { get; set; }
        public double Peso { get; set; }
    }
}
