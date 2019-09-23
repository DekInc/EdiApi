using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class UnidadMedida
    {
        public UnidadMedida()
        {
            Producto = new HashSet<Producto>();
        }

        public int UnidadMedidaId { get; set; }
        public string UnidadMedida1 { get; set; }
        public string Simbolo { get; set; }
        public string TipoRegistro { get; set; }

        public ICollection<Producto> Producto { get; set; }
    }
}
