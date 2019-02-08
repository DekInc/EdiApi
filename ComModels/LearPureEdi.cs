using System;
using System.Collections.Generic;

namespace ComModels
{
    public partial class LearPureEdi
    {
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string Fingreso { get; set; }
        public string Fprocesado { get; set; }
        public bool Reprocesar { get; set; }
        public string NombreArchivo { get; set; }
        public string Log { get; set; }
        public int? CheckSeg { get; set; }
        public bool Shp { get; set; }
    }
}
