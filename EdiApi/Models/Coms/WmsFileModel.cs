using System;
using System.Collections.Generic;
using System.Text;

namespace EdiApi.Models {
    public class WmsFileModel {
        public string Barcode { get; set; }
        public string Descripcion { get; set; }
        public int Piezas { get; set; }
        public int Unidad { get; set; }
        public int Cantidad { get; set; }
        public string CodigoLocalizacion { get; set; }
        public double? Peso { get; set; }
        public double? Volumen { get; set; }
        public string Cliente { get; set; }        
        public int UOM { get; set; }
        public int Exportador { get; set; }
        public int PaisOrigen { get; set; }        
        public string Cp { get; set; }
        public int Cont { get; set; }
        public string Modelo { get; set; }
        public string Lote { get; set; }
        public string Estilo { get; set; }
    }
}
