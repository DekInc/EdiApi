﻿using System;
using System.Collections.Generic;

namespace EdiApi.Models.EdiDB
{
    public partial class PedidosExternos
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int? TiendaId { get; set; }
        public string FechaPedido { get; set; }
        public int? IdEstado { get; set; }
        public string FechaCreacion { get; set; }
        public string Periodo { get; set; }
        public string FechaUltActualizacion { get; set; }
    }
}
