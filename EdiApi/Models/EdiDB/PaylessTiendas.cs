using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PaylessTiendas
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int? TiendaId { get; set; }
        public string Distrito { get; set; }
        public string Descr { get; set; }
        public string Direc { get; set; }
        public string Tel { get; set; }
        public string Cel { get; set; }
        public string Lider { get; set; }
        public int? BodegaId { get; set; }
    }
}
