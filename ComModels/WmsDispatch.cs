using System;
using System.Collections.Generic;
using System.Text;

namespace ComModels {
    public class WmsDispatch {
        public int recid { get {
                return this.GetHashCode();
            } }
        public string CodProducto { get; set; }
        public string Barcode { get; set; }
        public int? TipoBulto { get; set; }
        public string UnidadMedida { get; set; }
        public int? Cantidad { get; set; }
        public string Cp { get; set; }
        public string Categoria { get; set; }
        public string Departamento { get; set; }
        public string Producto { get; set; }
        public string Talla { get; set; }
    }
}
