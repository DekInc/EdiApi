using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class Regimen
    {
        public int Idregimen { get; set; }
        public string Regimen1 { get; set; }
        public string Descripcion { get; set; }
        public bool? MostrarDetalle { get; set; }
    }
}
