using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class BodegaxRegimen
    {
        public int BodegaxRegimenId { get; set; }
        public int? BodegaId { get; set; }
        public int? Regimen { get; set; }

        public Bodegas Bodega { get; set; }
        public Regimen RegimenNavigation { get; set; }
    }
}
