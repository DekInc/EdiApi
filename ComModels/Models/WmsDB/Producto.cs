using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Producto
    {
        public Producto()
        {
            BarcodeProducto = new HashSet<BarcodeProducto>();
            DetalleProducto = new HashSet<DetalleProducto>();
            ItemInventario = new HashSet<ItemInventario>();
        }

        public string CodProducto { get; set; }
        public string Descripcion { get; set; }
        public int? UnidadMedida { get; set; }
        public int? ClienteId { get; set; }
        public int? EstatusId { get; set; }
        public int? CategoriaId { get; set; }
        public double? Existencia { get; set; }
        public double? Reservado { get; set; }
        public double? Despachado { get; set; }
        public double? CantMinima { get; set; }
        public double? PrecioPromedio { get; set; }
        public DateTime? Fecha { get; set; }
        public string Comentario { get; set; }
        public double? StockMaximo { get; set; }
        public int? Descargoid { get; set; }
        public string Partida { get; set; }

        public Categoria Categoria { get; set; }
        public MetodoDescargo Descargo { get; set; }
        public Estatus Estatus { get; set; }
        public UnidadMedida UnidadMedidaNavigation { get; set; }
        public ICollection<BarcodeProducto> BarcodeProducto { get; set; }
        public ICollection<DetalleProducto> DetalleProducto { get; set; }
        public ICollection<ItemInventario> ItemInventario { get; set; }
    }
}
