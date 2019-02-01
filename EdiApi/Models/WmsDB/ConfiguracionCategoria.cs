using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class ConfiguracionCategoria
    {
        public ConfiguracionCategoria()
        {
            DetalleProducto = new HashSet<DetalleProducto>();
        }

        public int? CategoriaId { get; set; }
        public int ParametroId { get; set; }
        public string Parametro { get; set; }
        public DateTime? Fecha { get; set; }
        public bool? Isnecesary { get; set; }
        public int? EstatusId { get; set; }
        public string Type { get; set; }
        public bool? IsFijo { get; set; }

        public Categoria Categoria { get; set; }
        public ICollection<DetalleProducto> DetalleProducto { get; set; }
    }
}
