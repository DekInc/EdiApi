using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            ConfiguracionCategoria = new HashSet<ConfiguracionCategoria>();
            Producto = new HashSet<Producto>();
        }

        public int CategoriaId { get; set; }
        public string NomCategoria { get; set; }
        public string Observacion { get; set; }
        public DateTime? Fecha { get; set; }
        public int? EstatusId { get; set; }

        public ICollection<ConfiguracionCategoria> ConfiguracionCategoria { get; set; }
        public ICollection<Producto> Producto { get; set; }
    }
}
