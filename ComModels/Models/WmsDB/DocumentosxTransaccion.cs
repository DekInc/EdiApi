using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DocumentosxTransaccion
    {
        public int IddocxTransaccion { get; set; }
        public int? DocumentoId { get; set; }
        public int? TransaccionId { get; set; }
        public string NoDocumento { get; set; }
        public DateTime? FechaDocumento { get; set; }
        public DateTime? Fecha { get; set; }
        public string ArchImage { get; set; }
        public string InformeAlmacen { get; set; }
        public string Im5 { get; set; }
        public string CartaAcepta { get; set; }
        public string GuiaTransito { get; set; }
        public string FactComercial { get; set; }
        public string Im4 { get; set; }
        public string DocumentoTransporte { get; set; }
        public string FactExportacion { get; set; }
        public string OrdenCompra { get; set; }
        public string Manifiesto { get; set; }
        public DateTime? FeInformeAlmacen { get; set; }
        public DateTime? FeIm5 { get; set; }
        public DateTime? FeCartaAcepta { get; set; }
        public DateTime? FeGuiaTransito { get; set; }
        public DateTime? FeFactComercial { get; set; }
        public DateTime? FeIm4 { get; set; }
        public DateTime? FeDocumentoTransporte { get; set; }
        public DateTime? FeFactExportacion { get; set; }
        public DateTime? FeOrdenCompra { get; set; }
        public DateTime? FeManifiesto { get; set; }

        public Documentos Documento { get; set; }
        public Transacciones Transaccion { get; set; }
    }
}
