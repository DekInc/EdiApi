using System;
using System.Collections.Generic;

namespace ComModels.Models.WmsDB
{
    public partial class Bodegas
    {
        public Bodegas()
        {
            BodegaxRegimen = new HashSet<BodegaxRegimen>();
            Racks = new HashSet<Racks>();
        }

        public int BodegaId { get; set; }
        public string NomBodega { get; set; }
        public string Descripcion { get; set; }
        public int? EstatusId { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Locationid { get; set; }
        public string TitRequisicion { get; set; }
        public bool? Nodescargaxcliente { get; set; }

        public Estatus Estatus { get; set; }
        public Locations Location { get; set; }
        public ICollection<BodegaxRegimen> BodegaxRegimen { get; set; }
        public ICollection<Racks> Racks { get; set; }
    }
}
