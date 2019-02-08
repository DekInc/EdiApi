using System;
using System.Collections.Generic;

namespace EdiApi.Models.WmsDB
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
