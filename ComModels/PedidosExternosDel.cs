using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class PedidosExternosDel
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int? TiendaId { get; set; }
        public string FechaPedido { get; set; }
        public int? IdEstado { get; set; }
        public string FechaCreacion { get; set; }
        public string Periodo { get; set; }
        public string FechaUltActualizacion { get; set; }
        public int? WomanQty { get; set; }
        public int? ManQty { get; set; }
        public int? KidQty { get; set; }
        public int? AccQty { get; set; }
        public int? WomanQtyT { get; set; }
        public int? ManQtyT { get; set; }
        public int? KidQtyT { get; set; }
        public int? AccQtyT { get; set; }
        public string InvType { get; set; }
        public bool? FullPed { get; set; }
        public bool? Divert { get; set; }
        public int? TiendaIdDestino { get; set; }
        public int? TotalCp { get; set; }
        public string FechaBorrado { get; set; }
    }
}
