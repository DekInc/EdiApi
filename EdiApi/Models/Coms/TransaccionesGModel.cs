using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models {
    public class TransaccionesGModel {
        public int TransaccionId { get; set; }
        public string NoTransaccion { get; set; }
        public DateTime? FechaTransaccion { get; set; }
        public string IdtipoTransaccion { get; set; }
        public int? PedidoId { get; set; }
        public int? BodegaId { get; set; }
        public int? RegimenId { get; set; }
        public int? ClienteId { get; set; }
        public double? Total { get; set; }
        public string TipoIngreso { get; set; }
        public string Usuariocrea { get; set; }
        public DateTime? Fechacrea { get; set; }
        public string Observacion { get; set; }
        public int? EstatusId { get; set; }
        public int? Operarioid { get; set; }
        public string TipoPicking { get; set; }
        public int? Exportadorid { get; set; }
        public int? Destinoid { get; set; }
        public int? Transportistaid { get; set; }
        public int? PaisOrig { get; set; }
        public string AduFro { get; set; }
        public string Placa { get; set; }
        public string Marchamo { get; set; }
        public string Contenedor { get; set; }
        public string CodMotoris { get; set; }
        public string Remolque { get; set; }
        public string RecivingCliente { get; set; }
        public DateTime? FechaReciving { get; set; }
        public int? FacturaId { get; set; }
    }
}
