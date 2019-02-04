using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Models
{    
    public class Rep830Info
    {
        public string From { get; set; }
        public string To { get; set; }
        public string EdiStr { get; set; }
        public string HashId { get; set; }
        public string Fingreso { get; set; }
        public string Fprocesado { get; set; }
        public bool Reprocesar { get; set; }
        public string NombreArchivo { get; set; }
        public string Log { get; set; }
        public int? CheckSeg { get; set; }
        public string errorMessage { get; set; }
        public string NumReporte { get; set; }
    }
}
