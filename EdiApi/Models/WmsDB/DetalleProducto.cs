using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class DetalleProducto
    {
        public int DetalleProdId { get; set; }
        public string CodProducto { get; set; }
        public int? ParametroId { get; set; }
        public string Descripcion { get; set; }

        public Producto CodProductoNavigation { get; set; }
        public ConfiguracionCategoria Parametro { get; set; }
    }
}
