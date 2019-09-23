using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class DocumentoxTipoTransaccion
    {
        public int IddocumentoTipoTransac { get; set; }
        public string IdtipoTransaccion { get; set; }
        public int DocumentoId { get; set; }
        public bool IsRequerido { get; set; }

        public Documentos Documento { get; set; }
        public TipoTransacciones IdtipoTransaccionNavigation { get; set; }
    }
}
