using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComModels
{
    public class ExistenciasExternModel
    {
        public string Cliente { get; set; }
        public string Bodega { get; set; }
        public string CodProducto { get; set; }
        public string Descripcion { get; set; }
        public double Existencia { get; set; }
        public double Reservado { get; set; }
        public double Disponible { get; set; }
        public int ClienteID { get; set; }
        public int BodegaID { get; set; }
        public int Bultos { get; set; }
        public double Peso { get; set; }
        public double Volumen { get; set; }
        public int Uxb { get; set; }
        public string Lote { get; set; }
        public string Contenedor { get; set; }
    }
}
