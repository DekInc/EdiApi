using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class Despachos
    {
        public Despachos()
        {
            DtllDespacho = new HashSet<DtllDespacho>();
        }

        public int DespachoId { get; set; }
        public string NoContenedor { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string Motorista { get; set; }
        public string DocumentoMotorista { get; set; }
        public string Destino { get; set; }
        public string Ruta { get; set; }
        public string DocumentoFiscal { get; set; }
        public DateTime? FechaDocFiscal { get; set; }
        public string Custodios { get; set; }
        public string EmpresaCustodios { get; set; }
        public string NoMarchamo { get; set; }
        public DateTime? FechaTransaccion { get; set; }
        public string UsuarioTransaccion { get; set; }
        public string Observacion { get; set; }
        public int? Transportistaid { get; set; }
        public int? Destinoid { get; set; }
        public int? Edistatus { get; set; }
        public DateTime? EdistatusDate { get; set; }
        public int? Ediuser { get; set; }

        public Transportista Transportista { get; set; }
        public ICollection<DtllDespacho> DtllDespacho { get; set; }
    }
}
