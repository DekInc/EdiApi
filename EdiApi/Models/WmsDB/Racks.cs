using System;
using System.Collections.Generic;

namespace EdiApi.Models
{
    public partial class Racks
    {
        public Racks()
        {
            DetalleTransaccionesRackFromNavigation = new HashSet<DetalleTransacciones>();
            DetalleTransaccionesRackNavigation = new HashSet<DetalleTransacciones>();
            DtllItemTransaccionRackFromNavigation = new HashSet<DtllItemTransaccion>();
            DtllItemTransaccionRackNavigation = new HashSet<DtllItemTransaccion>();
            Inventario = new HashSet<Inventario>();
        }

        public int Rack { get; set; }
        public string NombreRack { get; set; }
        public DateTime? Fecha { get; set; }
        public string Observacion { get; set; }
        public int? BodegaId { get; set; }
        public int? EstatusId { get; set; }
        public decimal? Alto { get; set; }
        public decimal? Largo { get; set; }
        public decimal? Ancho { get; set; }
        public int? RegimenId { get; set; }
        public string Barcode { get; set; }

        public Bodegas Bodega { get; set; }
        public ICollection<DetalleTransacciones> DetalleTransaccionesRackFromNavigation { get; set; }
        public ICollection<DetalleTransacciones> DetalleTransaccionesRackNavigation { get; set; }
        public ICollection<DtllItemTransaccion> DtllItemTransaccionRackFromNavigation { get; set; }
        public ICollection<DtllItemTransaccion> DtllItemTransaccionRackNavigation { get; set; }
        public ICollection<Inventario> Inventario { get; set; }
    }
}
