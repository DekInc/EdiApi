using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class TipoTransacciones
    {
        public TipoTransacciones()
        {
            DocumentoxTipoTransaccion = new HashSet<DocumentoxTipoTransaccion>();
            Transacciones = new HashSet<Transacciones>();
        }

        public string IdtipoTransaccion { get; set; }
        public string TipoTransaccion { get; set; }
        public string Descripcion { get; set; }

        public ICollection<DocumentoxTipoTransaccion> DocumentoxTipoTransaccion { get; set; }
        public ICollection<Transacciones> Transacciones { get; set; }
    }
}
