using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComModels {
    public class PeticionesAdminBGModel {
        public int Recid { get { return Id; } }
        public int Id { get; set; }
        public int? TiendaId { get; set; }
        public string Tienda { get; set; }
        public int WomanQty { get; set; }
        public int ManQty { get; set; }
        public int KidQty { get; set; }
        public int AccQty { get; set; }
        public int? WomanQtyT { get; set; }
        public int? ManQtyT { get; set; }
        public int? KidQtyT { get; set; }
        public int? AccQtyT { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaPedido { get; set; }
        public int TotalCp { get; set; }
        public int? PedidoWMS { get; set; }
        public int IdEstado { get; set; }
        public int Total { get; set; }
        public int WomanQtyEnv { get; set; }
        public int ManQtyEnv { get; set; }
        public int KidQtyEnv { get; set; }
        public int AccQtyEnv { get; set; }
        public int TotalCpEnv { get; set; }
        public int TotalEnv { get; set; }
        public double PorcValid { get; set; }
        public bool? FullPed { get; set; }
        public bool? Divert { get; set; }
        public int? TiendaIdDestino { get; set; }
    }
}
