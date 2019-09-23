using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class ConfigFrmBusqueda
    {
        public string FrmCodigo { get; set; }
        public string FrmNombre { get; set; }
        public string FrmFormulario { get; set; }
        public string FrmDataset { get; set; }
        public string FrmWhere { get; set; }
        public string FrmOrderby { get; set; }
        public bool? FrmObtieneDataset { get; set; }
        public bool? FrmObtieneSp { get; set; }
    }
}
