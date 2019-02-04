using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models
{
    public class TsqlDespachosWmsComplex
    {
        public int DespachoId { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string NoContenedor { get; set; }
        public string Motorista { get; set; }
        public string DocumentoMotorista { get; set; }
        public string Destino { get; set; }
        public string DocumentoFiscal { get; set; }
        public DateTime? FechaDocFiscal { get; set; }
        public string NoMarchamo { get; set; }
        public string Observacion { get; set; }
        public int? Transportistaid { get; set; }
        public int? Destinoid { get; set; }

        public long? idclient { get; set; }
        public string code { get; set; }
        public string businessname { get; set; }
        public long IdCountryOrigin { get; set; }
        public string CountryOrigin { get; set; }
        public double? Quantity { get; set; }
        public double? Bulks { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public double? TotalValue { get; set; }

        public int? TipoBulto { get; set; }
        public string CodProducto { get; set; }
        public string NumeroOc { get; set; }
        public string UnidadDeMedida { get; set; }        

        public string NoTransaccion { get; set; }
        public int? ClienteId { get; set; }
        public string Estatus1 { get; set; }

        public int BodegaId { get; set; }
        public string NomBodega { get; set; }
        public string Nompais { get; set; }
        public long? IdRcontrol { get; set; }
    }
}
