using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
{
    public partial class Documentos
    {
        public Documentos()
        {
            DocumentosxTransaccion = new HashSet<DocumentosxTransaccion>();
            DocumentoxTipoTransaccion = new HashSet<DocumentoxTipoTransaccion>();
        }

        public int DocumentoId { get; set; }
        public string Documento { get; set; }
        public string Descripcion { get; set; }
        public string ColNomdoc { get; set; }

        public ICollection<DocumentosxTransaccion> DocumentosxTransaccion { get; set; }
        public ICollection<DocumentoxTipoTransaccion> DocumentoxTipoTransaccion { get; set; }
    }
}
