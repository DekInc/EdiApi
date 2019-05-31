using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Models
{
    public class ErrorModel
    {
        public int Typ { get; set; } = 0;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
