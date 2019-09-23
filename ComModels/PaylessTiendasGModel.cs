using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class PaylessTiendasGModel
    {
        public int Recid { get { return Id; } }
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
        public string HorarioEntrega { get; set; }
        public int? RutaId { get; set; }
        public int? NumRuta { get; set; }
    }
}
