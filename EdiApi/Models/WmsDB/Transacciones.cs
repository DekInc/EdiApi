using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Transacciones
    {
        public Transacciones()
        {
            DetalleTransacciones = new HashSet<DetalleTransacciones>();
            DocumentosxTransaccion = new HashSet<DocumentosxTransaccion>();
        }

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
        public long? IdRcontrol { get; set; }

        public Aduana AduFroNavigation { get; set; }
        public Destconsigna Destino { get; set; }
        public Exportador Exportador { get; set; }
        public TipoTransacciones IdtipoTransaccionNavigation { get; set; }
        public Operarios Operario { get; set; }
        public Paises PaisOrigNavigation { get; set; }
        public Transportista Transportista { get; set; }
        public ICollection<DetalleTransacciones> DetalleTransacciones { get; set; }
        public ICollection<DocumentosxTransaccion> DocumentosxTransaccion { get; set; }
    }
}
